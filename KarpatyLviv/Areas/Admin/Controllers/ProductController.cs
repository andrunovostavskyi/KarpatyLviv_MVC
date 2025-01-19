using Karpaty.Models;
using Karpaty.Models.ViewModels;
using Karpaty.Services.IServices;
using Karpaty.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KarpatyLviv.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin +","+ SD.Role_Employee)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(_unitOfWork.Product.GetAll().ToList());
        }

        public IActionResult UpSert(int? id)
        {

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };
            if (id != 0 && id != null)
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "Images");
            }
            return View(productVM);
        }


        [HttpPost]
        public IActionResult UpSert(ProductVM productVM, List<IFormFile> files)
        {
            if (ModelState.IsValid) 
            {
                if (productVM.Product.Id != 0)
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }

                if (files != null) 
                {
                    string wwwroot = _webHostEnvironment.WebRootPath;

                    foreach(IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        string finalPath = Path.Combine(wwwroot,productPath);

                        if(!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }

                        using (var filestream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }

                        ProductImage productImage = new()
                        {
                            ProductId = productVM.Product.Id,
                            ImageUrl = @"\" + productPath + @"\" + fileName
                        };
                        if(productVM.Product.Images == null)
                        {
                            productVM.Product.Images = new List<ProductImage>();
                        }

                        productVM.Product.Images.Add(productImage);
                        _unitOfWork.ProductImage.Add(productImage);
                    }
                }

                _unitOfWork.Save();
                TempData["success"] = "Product was added/updated successfuly";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Something go wrong";
            return View(productVM);
        }

        public IActionResult Edit(int id)
        {
            return View(_unitOfWork.Product.Get(u => u.Id == id));
        }

        public IActionResult DeleteImage(int imageId)
        {
            var imageFromDb = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
            if (!string.IsNullOrEmpty(imageFromDb.ImageUrl)){
                var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, imageFromDb.ImageUrl.Trim('\\'));
                if(System.IO.File.Exists(oldPath)){
                    System.IO.File.Delete(oldPath);
                }

                _unitOfWork.ProductImage.Delete(imageFromDb);
            }
            _unitOfWork.Save();
            TempData["success"]= "Image was deleted";
            return RedirectToAction(nameof(UpSert), new { id = imageFromDb.ProductId });
        }    

        [HttpDelete]
        public IActionResult Delete(Product product)
        {
            var productForDelete = _unitOfWork.Product.Get(u=>u.Id == product.Id, includeProperties:"Images");
            if(productForDelete is not null)
            {
                var productPath = @"images\products\product-" + productForDelete.Id;
                var finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);
                if (Directory.Exists(finalPath))
                {
                    string[] filepaths = Directory.GetFiles(finalPath);
                    foreach (string filepath in filepaths)
                    {
                        System.IO.File.Delete(filepath);
                    }
                    Directory.Delete(finalPath);
                }

                _unitOfWork.Product.Delete(productForDelete);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Product was deleted successfully" });
            }
            return Json(new { success = false, message = "Product not found" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productList });
        }
    }
}
