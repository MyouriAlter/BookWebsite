using Microsoft.AspNetCore.Mvc;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Microsoft.AspNetCore.Hosting;

namespace BookSection.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

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
            Product product = new Product();
            
            //this is for create
            if (id == null)
                return View(product);

            //this is for edit
            product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (product == null)
                return NotFound();
            
            return View(product);
        }

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

        #region API calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Product.GetAll();
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
