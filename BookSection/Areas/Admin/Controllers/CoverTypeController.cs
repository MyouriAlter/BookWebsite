using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookSection.Areas.Admin.Controllers
{
    [Area("Admin")] 

    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            
            //this is for create
            if (id == null)
                return View(coverType);
            
            //this is for edit
            coverType = _UnitOfWork.CoverType.Get(id.GetValueOrDefault());
            if (coverType == null)
                return NotFound();
            
            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (!ModelState.IsValid) return View(coverType);

            if (coverType.Id == 0)
                _UnitOfWork.CoverType.Add(coverType);
            else
                _UnitOfWork.CoverType.Update(coverType);
            _UnitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        #region API Calls

        public IActionResult GetAll()
        {
            var allObj = _UnitOfWork.CoverType.GetAll();
            return Json(new {data = allObj});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _UnitOfWork.CoverType.Get(id);
            
            if (objFromDb == null)
                return Json(new {success = false, message = "Error while deleting"});

            _UnitOfWork.CoverType.Remove(objFromDb);
            _UnitOfWork.Save();
            return Json(new {success = true, message = "Deleted successfully"});
        }

        #endregion
        

    }
}