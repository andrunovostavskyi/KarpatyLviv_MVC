using Karpaty.Data;
using Karpaty.Models;
using Karpaty.Services.IServices;
using Karpaty.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarpatyLviv.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin +"," +SD.Role_Employee)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category was added"; // Зберігаємо успішне повідомлення
                return RedirectToAction(nameof(Index)); // Редиректимо на Index
            }
            TempData["error"] = "Something went wrong"; // Зберігаємо повідомлення про помилку
            return View(category); // Повертаємося до форми, якщо не вдалося
        }


        public IActionResult Edit(int? id)
        {
            Category catFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            return View(catFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category was updated success";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Something went wrong";
            return View(category);
        }

        
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new {data =  _unitOfWork.Category.GetAll().ToList()});
        }

        [HttpDelete]
        public IActionResult Delete(Category category)
        {
            if (category is not null)
            {
                _unitOfWork.Category.Delete(category);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Category was deleted successfully" });
            }
            return Json(new { success = false, message = "Category not found" });

        }
    }
}
