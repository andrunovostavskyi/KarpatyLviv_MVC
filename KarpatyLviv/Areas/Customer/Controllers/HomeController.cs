using Karpaty.Models;
using Karpaty.Services.IServices;
using Karpaty.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace KarpatyLviv.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public IActionResult Index()
        {
            var temp = _unitOfWork.Product.GetAll(includeProperties:"Images");
            return View(temp);
        }

        public IActionResult Details(int productId)
        {
            Product prod = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category,Images");
            return View(prod);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(Product prod)
        {
            var claimUser = (ClaimsIdentity)User.Identity!;
            var userId = claimUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            ShoppingCard newCard = _unitOfWork.ShoppingCard.Get(u=>u.ProductId == prod.Id && u.AplicationUserId == userId);
            Product prodFromDb = _unitOfWork.Product.Get(u=>u.Id == prod.Id);
            if (newCard == null)
            {
                ShoppingCard shoppingCard = new()
                {
                    ProductId = prod.Id,
                    AplicationUserId = userId,
                    Count = prod.Count
                };
                _unitOfWork.ShoppingCard.Add(shoppingCard);
            }
            else
            {
                newCard.Count += prod.Count;
                _unitOfWork.ShoppingCard.Update(newCard);
            }
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCard.GetAll(u => u.AplicationUserId == userId).Count());
            TempData["success"] = "Product was added in basket";
            return RedirectToAction("Index");
        }
       
    }
}
