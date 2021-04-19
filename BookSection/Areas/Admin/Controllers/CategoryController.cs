using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookSection.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var category = new Category();

            //this is for create
            if (id == null)
                return View(category);

            //this is for edit
            category = _unitOfWork.Category.Get(id.GetValueOrDefault());
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            if (category.Id == 0)
                _unitOfWork.Category.Add(category);
            else
                _unitOfWork.Category.Update(category);

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        #region API calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Category.GetAll();
            return Json(new {data = allObj});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Category.Get(id);

            if (objFromDb == null)
                return Json(new {success = false, message = "Error while deleting"});

            _unitOfWork.Category.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new {success = true, message = "Deleted successful"});
        }

        #endregion API calls
    }
}