using BLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DIT.Controllers
{
	public class LegacyMarksController : Controller
	{
		private readonly IMarksBLL _marksBll;
		// GET: LegacyMarks
		public LegacyMarksController()
		{
			_marksBll = new MarksBLL();
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult LegacyMarksMasterEntry()
		{
			MarksUpload model = new MarksUpload();
			model.SubjectList = _marksBll.SubjectListBLL();
			model.SemesterList = _marksBll.SemesterListBLL();
			model.ExamTypeList = _marksBll.ExamTypeListBLL();
			return View(model);
		}

		[HttpPost]
		public JsonResult LegacyMarksMasterEntry(MarksUpload model)
		{
			model.user_id = Convert.ToInt32(Session["LoginId"]);
			var res = _marksBll.LegacyMarksMasterUploadBLL(model);
			//var res = "";
			return Json(res, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		public JsonResult LegacyMarksMasterData(MarksUpload model)
		{
			model.user_id = Convert.ToInt32(Session["LoginId"]);
			var res = _marksBll.UploadDataBLL(model);
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult CaptureMarks(MarksUpload model)
		{
			//MarksUpload model = new MarksUpload();
			model.SubjectList = _marksBll.SubjectListBLL();
			model.Tradesector = _marksBll.TradeSectorBLL();
			model.Tradescheme = _marksBll.TradeSchemeBLL();
			model.Tradetype = _marksBll.TradeTypeBLL();
			if (model != null)
				model.marksUploads = _marksBll.GetTraineesBLL(model);

			return View(model);
		}

		[HttpPost]
		public ActionResult ExamMarks(MarksUpload model)
		{

			return View();
		}


	}
}