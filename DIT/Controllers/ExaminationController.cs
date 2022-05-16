using BLL;
using BLL.ExamNotification;
using Models;
using Models.ExamCenterMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DIT.Controllers
{
    public class ExaminationController : Controller
    {
        private readonly IExaminationBLL _ExamBll;
        private readonly INotificationBLL _NotifBll;
        public ExaminationController()
        {
            _ExamBll = new ExaminationBLL();
            _NotifBll = new NotificationBLL();
        }

        // GET: Examination
        public ActionResult UniqueIdentification(Examination model)
        {
            model.selectTab = 0;
            return View(model);
        }

        public JsonResult GenerateUniqueCodeForTrainee(Examination model)
        {
            model.user_id = Convert.ToInt32(Session["LoginId"]);
            var res = _ExamBll.GenerateUniqueCodeForTraineeBLL(model);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubjectAndTrade(string ExamDate)
        {
            var res = _ExamBll.GetSubjectAndTradeBLL(ExamDate);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PackingSlip(PackingSlip model)
        {
            model.selectTab = 0;
            model.CourseTypeList = _NotifBll.GetCourseListBLL();
            model.ExamCenterList = _ExamBll.GetExamCenterListBLL();
            model.TradeList = _NotifBll.GetTradeListBLL();
            //model.SubjectTypeList = _NotifBll.GetSubjectTypeListBLL();

            return View(model);
        }

        public JsonResult GeneratePackingSlip(PackingSlip model)
        {
            model.user_id = Convert.ToInt32(Session["LoginId"]);
            var res = _ExamBll.GeneratePackingSlipBLL(model);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Examination Center Mapping
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ExamCenterMapping(ExamcenterMapping model)
        {
            model.Course_Type_List = _NotifBll.GetCourseListBLL();
            model.DivisionList = _NotifBll.GetDivisionListBLL();
            model.DistrictList = _NotifBll.GetDistrictBLL();
            model.CollegeLists = _NotifBll.GetCollegeBLL();
            return View(model);
        }

        public JsonResult DisticListBasedOnDivId(int DivId)
        {
            ExamcenterMapping model = new ExamcenterMapping();
            var res = _ExamBll.GetDisticListBLL(DivId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DisticBasedCentersandITIClg(int distic_Id, int DivId)
        {
            // ExamcenterMapping model = new ExamcenterMapping();
            var res = _ExamBll.DisticBasedCentersITIClgBLL(distic_Id, DivId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }




    }
}