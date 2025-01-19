using Karpaty.Data;
using Karpaty.Models;
using Karpaty.Models.ViewModels;
using Karpaty.Services.IServices;
using Karpaty.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KarpatyLviv.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin +","+ SD.Role_Employee)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork; 
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult RoleManagment(string userId)
        {
            RoleManagerVM roleManagerVM = new()
            {
                AplicationUser = _unitOfWork.AplicationUser.Get(u => u.Id == userId),
                RoleList = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Value = i.Name,
                    Text = i.Name,
                })
            };
            roleManagerVM.AplicationUser.Role = _userManager
                .GetRolesAsync(_unitOfWork.AplicationUser.Get(u => u.Id == userId))
                .GetAwaiter()
                .GetResult()
                .FirstOrDefault()!;
            return View(roleManagerVM);
        }

        [HttpPost]
        public IActionResult RoleManagment(RoleManagerVM roleManagerVM)
        {
            AplicationUser user = roleManagerVM.AplicationUser;
            var oldRole = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
            if (oldRole != null && oldRole != roleManagerVM.AplicationUser.Role)
            {
                _userManager.RemoveFromRoleAsync(user,oldRole).GetAwaiter().GetResult().ToString();
                _userManager.AddToRoleAsync(user,roleManagerVM.AplicationUser.Role).GetAwaiter().GetResult();
                _unitOfWork.Save();
                TempData["success"] = "Roles was updated successfully";
            } 
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<AplicationUser> users = _unitOfWork.AplicationUser.GetAll().ToList();
            if(users == null)
            {
                users = new List<AplicationUser>();
            }
            
            foreach(var item in users)
            {
                item.Role = _userManager.GetRolesAsync(item).GetAwaiter().GetResult().FirstOrDefault()!;
            }
            return Json(new { data = users });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            AplicationUser user = _unitOfWork.AplicationUser.Get(u=>u.Id == id);
            if(user.LockoutEnd != null)
            {
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnd = DateTime.UtcNow.AddYears(1000);
            }
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation success" });
        }
    }
}
