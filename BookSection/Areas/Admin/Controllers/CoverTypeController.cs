using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Book.Utility;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BookSection.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var coverType = new CoverType();

            //this is for create
            if (id == null)
                return View(coverType);

            //this is for edit
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            coverType = _unitOfWork.SpCall.OneRecord<CoverType>(Sd.ProcCoverTypeGet, parameter);

            if (coverType == null)
                return NotFound();

            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (!ModelState.IsValid) return View(coverType);

            var parameter = new DynamicParameters();
            parameter.Add("@Name", coverType.Name);

            if (coverType.Id == 0)
            {
                _unitOfWork.SpCall.Execute(Sd.ProcCoverTypeCreate, parameter);
            }
            else
            {
                parameter.Add("@Id", coverType.Id);
                _unitOfWork.SpCall.Execute(Sd.ProcCoverTypeUpdate, parameter);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SpCall.List<CoverType>(Sd.ProcCoverTypeGetAll);
            return Json(new {data = allObj});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            var objFromDb = _unitOfWork.SpCall.OneRecord<CoverType>(Sd.ProcCoverTypeGet, parameter);

            if (objFromDb == null)
                return Json(new {success = false, message = "Error while deleting"});

            _unitOfWork.SpCall.Execute(Sd.ProcCoverTypeDelete, parameter);
            _unitOfWork.Save();
            return Json(new {success = true, message = "Deleted successfully"});
        }

        #endregion API Calls
    }
}