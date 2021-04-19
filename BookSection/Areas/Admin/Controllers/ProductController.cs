using System.Linq;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookSection.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var productVm = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            //this is for create
            if (id == null)
                return View(productVm);

            //this is for edit
            productVm.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (productVm.Product == null)
                return NotFound();

            return View(productVm);
        }

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product product)
        {
            if (!ModelState.IsValid) return View(product);

            if (product.Id == 0)
                _unitOfWork.Product.Add(product);
            else
                _unitOfWork.Product.Update(product);

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        */
        #region API calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new {data = allObj});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Product.Get(id);

            if (objFromDb == null)
                return Json(new {success = false, message = "Error while deleting"});

            _unitOfWork.Product.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new {success = true, message = "Deleted successful"});
        }

        #endregion
    }
}