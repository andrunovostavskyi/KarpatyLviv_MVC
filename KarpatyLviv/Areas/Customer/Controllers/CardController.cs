using Karpaty.Models;
using Karpaty.Models.ViewModels;
using Karpaty.Services.IServices;
using Karpaty.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using Product = Karpaty.Models.Product;

namespace KarpatyLviv.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimUser = (ClaimsIdentity)User.Identity!;
            var userId = claimUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            ShoppingCardVM cardVM = new()
            {
                ShoppingCardList = _unitOfWork.ShoppingCard.GetAll(u => u.AplicationUserId == userId, includeProperties: "Product").ToList(),
                OrderHeader = new()
            };
            List<ProductImage> productImages = _unitOfWork.ProductImage.GetAll().ToList();
            
            if (cardVM.ShoppingCardList == null)
            {
				cardVM.ShoppingCardList = new List<ShoppingCard>();
            }
            
            foreach (var items in  cardVM.ShoppingCardList)
            {
                items.Product.Images = productImages.Where(x => x.ProductId == items.ProductId).ToList();
                items.Price = GetPrice(items.Count, items.Product.Price, items.Product.Discount);
                cardVM.OrderHeader.OrderTotal += items.Count * items.Price;
            }
            return View(cardVM);
        }

        public IActionResult Increment(int cardId)
        {
            var cardFromDB = _unitOfWork.ShoppingCard.Get(u=>u.Id== cardId, includeProperties:"Product");
            if(cardFromDB is null)
            {
                TempData["error"] = "Something go wrong";
                return RedirectToAction(nameof(Index));
            }
            cardFromDB.Count++;
            cardFromDB.Price = GetPrice(cardFromDB.Count, cardFromDB.Product.Price, cardFromDB.Product.Discount);
            _unitOfWork.ShoppingCard.Update(cardFromDB);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrement(int cardId)
        {
            var cardFromDB = _unitOfWork.ShoppingCard.Get(u => u.Id == cardId, includeProperties:"Product");
            if (cardFromDB.Count==1)
            {
                Remove(cardId);
                return RedirectToAction(nameof(Index));
            }
            cardFromDB.Count--;
            cardFromDB.Price = GetPrice(cardFromDB.Count, cardFromDB.Product.Price, cardFromDB.Product.Discount);
            _unitOfWork.ShoppingCard.Update(cardFromDB);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cardId)
        {
            var cardFromDb = _unitOfWork.ShoppingCard.Get(u => u.Id == cardId);
            if (cardFromDb is null)
            {
                TempData["error"] = "Something go wrong";
                return RedirectToAction(nameof(Index));
            }
            _unitOfWork.ShoppingCard.Delete(cardFromDb);
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCard.GetAll(u => u.AplicationUserId == cardFromDb.AplicationUserId).Count()-1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimUser = (ClaimsIdentity)User.Identity;
            var userId = claimUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCardVM cardVM = new()
            {
                ShoppingCardList = _unitOfWork.ShoppingCard.GetAll(u => u.AplicationUserId == userId, includeProperties: "Product,AplicationUser").ToList(),
                OrderHeader = new()
            };

            cardVM.OrderHeader.AplicationUserId = userId;
            cardVM.OrderHeader.AplicationUser = _unitOfWork.AplicationUser.Get(u=>u.Id == userId);
            cardVM.OrderHeader.Name = cardVM.OrderHeader.AplicationUser.Name!;
            cardVM.OrderHeader.Adress = cardVM.OrderHeader.AplicationUser.StreetAdress!;
            cardVM.OrderHeader.City = cardVM.OrderHeader.AplicationUser.City!;
            cardVM.OrderHeader.Country = cardVM.OrderHeader.AplicationUser.Country!;
            cardVM.OrderHeader.PhoneNumber = cardVM.OrderHeader.AplicationUser.PhoneNumber!;


            foreach (var item in cardVM.ShoppingCardList)
			{
				item.Price = GetPrice(item.Count, item.Product.Price, item.Product.Discount);
                cardVM.OrderHeader.OrderTotal += item.Count * item.Price;
			}
			return View(cardVM);
        }

        [HttpPost]
        [ActionName(nameof(Summary))]
        public IActionResult SummaryPost(ShoppingCardVM cardFromBody)
        {
			var claimUser = (ClaimsIdentity)User.Identity;
			var userId = claimUser.FindFirst(ClaimTypes.NameIdentifier).Value;
			ShoppingCardVM cardVM = new()
			{
				ShoppingCardList = _unitOfWork.ShoppingCard.GetAll(u => u.AplicationUserId == userId, includeProperties: "Product,AplicationUser").ToList(),
				OrderHeader = new()
			};
			foreach (var items in cardVM.ShoppingCardList)
			{
                double price = GetPrice(items.Count, items.Product.Price, items.Product.Discount);
                cardVM.OrderHeader.OrderTotal += price * items.Count;
			}

            cardVM.OrderHeader.AplicationUserId = userId;
            cardVM.OrderHeader.AplicationUser = _unitOfWork.AplicationUser.Get(u => u.Id == userId);
            cardVM.OrderHeader.Name = cardFromBody.OrderHeader.Name;
            cardVM.OrderHeader.Country = cardFromBody.OrderHeader.Country;
            cardVM.OrderHeader.City = cardFromBody.OrderHeader.City;
            cardVM.OrderHeader.Adress = cardFromBody.OrderHeader.Adress;
            cardVM.OrderHeader.Email = cardVM.OrderHeader.AplicationUser.Email;
            cardVM.OrderHeader.PhoneNumber = cardVM.OrderHeader.AplicationUser.PhoneNumber;

            _unitOfWork.OrderHeader.Add(cardVM.OrderHeader);
            _unitOfWork.Save();


            foreach (var item in cardVM.ShoppingCardList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = item.ProductId,
                    Count = item.Count,
                    OrderHeaderId = cardVM.OrderHeader.Id,
                    Price =  GetPrice(item.Count, item.Product.Price, item.Product.Discount)
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            var domain = "https://localhost:7113/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"customer/card/OrderConfirmation?id={cardVM.OrderHeader.Id}",
                CancelUrl = domain + "customer/card/index",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
            };


            foreach ( var item in cardVM.ShoppingCardList)
            {
                var sessionLineItems = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)(GetPrice(item.Count, item.Product.Price, item.Product.Discount) * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItems);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateSessionPaymentId(cardVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
		}

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties:"AplicationUser");
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateSessionPaymentId(orderHeader.Id, orderHeader.SessionId, session.PaymentIntentId);
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id,SD.StatusApproved, SD.PaymentStatusApproved);
                _unitOfWork.Save();
            }

            List<ShoppingCard> shoppingCards = _unitOfWork.ShoppingCard.GetAll(u => u.AplicationUserId == orderHeader.AplicationUserId).ToList();
            foreach(var item in shoppingCards)
            {
                _unitOfWork.ShoppingCard.Delete(item);
            }
            _unitOfWork.Save();
            HttpContext.Session.Clear();
            return View(id);
        }

        public double GetPrice(int count,double price, int discount)
        {
            if (count > 100)
            {
                price = price - (price * Product.discount100) / 100;
            }
            else if (count <= 50)
            {
                price = price - (price * discount) / 100;
            }
            else
            {
                price = price - (price * Product.discount50) / 100;
            }
            return price;
        }
    }
}
