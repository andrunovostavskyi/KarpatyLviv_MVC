using Karpaty.Models;
using Karpaty.Models.ViewModels;
using Karpaty.Services.IServices;
using Karpaty.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Diagnostics;
using System.Security.Claims;

namespace KarpatyLviv.Areas.Admin.Controllers
{
    [Area(SD.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int orderId)
        {
            OrderVM orderVM = new()
            {
                OrderDeteilsList = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId,includeProperties:"Product").ToList(),
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "AplicationUser")
            };
            return View(orderVM);
        }
        
        [HttpPost]
        [Authorize(Roles = SD.Role_Employee+","+SD.Role_Admin)]
        public IActionResult CancelOrder(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderVM.OrderHeader.Id);
            if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();
            TempData["success"] = "Order was cancelled successfuly";
            return RedirectToAction(nameof(Details), new {orderId = orderHeader.Id});
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Employee + "," + SD.Role_Admin)]
        public IActionResult UpdateOrderDetail(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u=>u.Id == orderVM.OrderHeader.Id, includeProperties:"AplicationUser");
            ModelState.Remove("OrderHeader.AplicationUserId");
            ModelState.Remove("OrderHeader.Email");
            if (ModelState.IsValid) {
                orderHeader.Name = orderVM.OrderHeader.Name;
                orderHeader.PhoneNumber = orderVM.OrderHeader.PhoneNumber;
                orderHeader.Adress = orderVM.OrderHeader.Adress;
                orderHeader.City = orderVM.OrderHeader.City;
                orderHeader.Country = orderVM.OrderHeader.Country;
                orderHeader.Carried = orderVM.OrderHeader.Carried;
                orderHeader.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            }


            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["success"] = "Order was updated successfuly";
            return RedirectToAction(nameof(Details), new { orderId = orderVM.OrderHeader.Id});
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Employee + "," + SD.Role_Admin)]
        public IActionResult StartProcessing(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderVM.OrderHeader.Id);
            if(orderVM == null)
            {
                TempData["error"] = "something go wrong";
                return RedirectToAction(nameof(Index));
            }
            _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusInProcess, orderHeader.PaymentStatus);
            _unitOfWork.Save();
            TempData["success"] = "Status was updated succesfully";
            return RedirectToAction(nameof(Details), new { orderId = orderHeader.Id});
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Employee + "," + SD.Role_Admin)]
        public IActionResult ShipOrder(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderVM.OrderHeader.Id);
            if(orderVM == null || string.IsNullOrEmpty(orderVM.OrderHeader.TrackingNumber) || string.IsNullOrEmpty(orderVM.OrderHeader.Carried))
            {
                TempData["error"] = "something go wrong";
                return RedirectToAction(nameof(Index));
            }
            orderHeader.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            orderHeader.Carried = orderVM.OrderHeader.Carried;
            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id,SD.StatusShipped,orderHeader.PaymentStatus);
            _unitOfWork.Save();
            TempData["success"] = "Status was updated succesfully";
            return RedirectToAction(nameof(Details), new { orderId = orderHeader.Id });
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            List<OrderHeader> orderHeaderList = new List<OrderHeader>();
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { data = orderHeaderList });
            }
            else if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaderList = _unitOfWork.OrderHeader.GetAll().ToList();
            }
            else
            {
                var userClaim =(ClaimsIdentity)User.Identity;
                string userId = userClaim.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                orderHeaderList = _unitOfWork.OrderHeader.GetAll(u=>u.AplicationUserId == userId).ToList();
            }
            switch (status)
            {
                case "cancelled":
                    orderHeaderList=orderHeaderList.Where(u=>u.OrderStatus ==SD.StatusCancelled).ToList();
                    break;
                case "inprocess":
                    orderHeaderList = orderHeaderList.Where(u => u.OrderStatus == SD.StatusInProcess).ToList();
                    break;
                case "completed":
                    orderHeaderList = orderHeaderList.Where(u => u.OrderStatus == SD.StatusShipped).ToList();
                    break;
                case "approved":
                    orderHeaderList = orderHeaderList.Where(u => u.OrderStatus == SD.StatusApproved).ToList();
                    break;
            }

            return Json(new { data = orderHeaderList });
        }
    }
}
