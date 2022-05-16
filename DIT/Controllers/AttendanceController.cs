using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Models;
using System.Web.Mvc;
using BLL.Attendance;
using System.Text;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Models.AttendanceDetails;

namespace DIT.Controllers
{
    public class AttendanceController : Controller
    {
      private readonly IAttendanceBLL _attendBll;
      private readonly string UploadFolder = ConfigurationManager.AppSettings["TemplateDocumentsPath"];

        // strings to generate file name
        private readonly string PdfFileNameFormat = "InvigilatorDairy{0}.pdf";
        private readonly string ExcelFileNameFormat = "InvigilatorDairy{0}.pdf";
        // GET: Attendance

        public AttendanceController()
        {
            _attendBll = new AttendanceBLL();
        }
        public ActionResult Index()
        {
            return View();
        }



        //public ActionResult AttendanceDet()
        //{
           
        //    return View();
        //}


        public ActionResult AttendanceDet()
        {
            AtendanceDet model = new AtendanceDet();
            model.User_Id = Convert.ToInt32(Session["LoginId"]);
            model.divisionList = _attendBll.GetDivisionListBLL();
            model.ExamCentrList = _attendBll.GetCenterListBLL();
            return View(model);
        }

        [HttpPost]
        public JsonResult AttendanceDetUpload(AtendanceDet model)
        {
            model.User_Id = Convert.ToInt32(Session["LoginId"]);
            // var res = _attendBll.ExamAttendanceUploadBLL(model);
            var res = 2;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getAttendanceDet(int divId)
        {
            AtendanceDet model = new AtendanceDet();
            StringBuilder sb = new StringBuilder();
            model.User_Id = Convert.ToInt32(Session["LoginId"]);
            // model.divId = Convert.ToInt16(divId);
           // var res = 0;
           var res = _attendBll.getAttendanceDetBLL(divId);
           // sb.Append("< tr >< td > " ' + slNo + " </ td >< td > " + attentdance.Day + " </ td >< td > " + dateFormat(new Date(parseInt((attentdance.ECT_ExamDate).match(/\d+/)[0]))) + " </ td >< td > " + attentdance.padStart + " </ td >< td > " + attentdance.Day+ " </ td >< td > " + attentdance.Day + " </ td >< td > " + attentdance.Day + " </ td ></ tr > ";")
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ModifyAttendanceDtls(int divId)
        {
            AtendanceDet model = new AtendanceDet();
          
            model.User_Id = Convert.ToInt32(Session["LoginId"]);
           
            var res = _attendBll.getModifyAttendanceDetBLL(divId);
            // sb.Append("< tr >< td > " ' + slNo + " </ td >< td > " + attentdance.Day + " </ td >< td > " + dateFormat(new Date(parseInt((attentdance.ECT_ExamDate).match(/\d+/)[0]))) + " </ td >< td > " + attentdance.padStart + " </ td >< td > " + attentdance.Day+ " </ td >< td > " + attentdance.Day + " </ td >< td > " + attentdance.Day + " </ td ></ tr > ";")
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDtlsByTraineRollNumber(int rollNo)
        {
            AtendanceDet model = new AtendanceDet();
           
            model.User_Id = Convert.ToInt32(Session["LoginId"]);
           
            var res = _attendBll.getTraineeDtlsByRollNoBLL(rollNo);
            // sb.Append("< tr >< td > " ' + slNo + " </ td >< td > " + attentdance.Day + " </ td >< td > " + dateFormat(new Date(parseInt((attentdance.ECT_ExamDate).match(/\d+/)[0]))) + " </ td >< td > " + attentdance.padStart + " </ td >< td > " + attentdance.Day+ " </ td >< td > " + attentdance.Day + " </ td >< td > " + attentdance.Day + " </ td ></ tr > ";")
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult saveExamattendanceDetails(FormCollection formCollection)
        {
            // get the files from view
            HttpFileCollectionBase files = Request.Files;
            List<AtendanceDet> models = new List<AtendanceDet>();

            // get all the row data
            foreach(var key in formCollection.AllKeys)
            {
                //Deserialize JSON to class
                AtendanceDet model = JsonConvert.DeserializeObject<AtendanceDet>(formCollection[key]);
                model.User_Id = Convert.ToInt32(Session["LoginId"]);
                for (int index =0; index < files.Count; index++)
                {
                    // assign file to be uploaded
                    if(model.attendSavePath == files[index].FileName)
                    {
                        model.UploadPdf = files[index];
                    }
                    else if (model.attendExcelPath == files[index].FileName)
                    {
                        model.UploadXcel = files[index];
                    }
                }
                models.Add(model);
            }
           
            var res = _attendBll.CreateAttendanceDetailsBLL(models);
            return Json(res, JsonRequestBehavior.AllowGet);

        }
		
		  [HttpPost]
        public JsonResult UpdateExamattendanceDetails(FormCollection formCollection)
        {
            // get the files from view
            HttpFileCollectionBase files = Request.Files;
            List<AtendanceDet> models = new List<AtendanceDet>();

            //AtendanceDet models = new AtendanceDet();


            // get all the row data
            foreach (var key in formCollection.AllKeys)
            {
                //Deserialize JSON to class
                AtendanceDet model = JsonConvert.DeserializeObject<AtendanceDet>(formCollection[key]);
                model.User_Id = Convert.ToInt32(Session["LoginId"]);
                for (int index = 0; index < files.Count; index++)
                {
                    // assign file to be uploaded
                    if (model.attendSavePath == files[index].FileName)
                    {
                        model.UploadPdf = files[index];
                    }
                    else if (model.attendExcelPath == files[index].FileName)
                    {
                        model.UploadXcel = files[index];
                    }
                }
                models.Add(model);
            }

            var res = _attendBll.UpdateAttendanceDetailsBLL(models);
            //  var res = _attendBll.ExamAttendanceUploadBLL(models);
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ModifiyExamAttendanceDet()
        {
            AtendanceDet model = new AtendanceDet();
            model.User_Id = Convert.ToInt32(Session["LoginId"]);
            model.divisionList = _attendBll.GetDivisionListBLL();
            model.ExamCentrList = _attendBll.GetCenterListBLL();
            return View(model);
            
        }

        [HttpGet]
        public ActionResult GetExamAttendanceDtls()
        {
            AtendanceDet model = new AtendanceDet();
            model.LoginRoleList = _attendBll.GetLoginRoleListBLL();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.AttendanceDetList = _attendBll.getAttendanceDtlsBLL(model);
            
            return View(model);

        }

        [HttpPost]
        public JsonResult AttendanceDtls(int ID)
        {
            AtendanceDet model = new AtendanceDet();
            model.LoginRoleList = _attendBll.GetLoginRoleListBLL();
            model.attenId = ID;
            var List = _attendBll.ViewAttendanceDtlsBLL(model);

            return Json(List, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult AttendanceApproveStatus(int Id, int Status, int Loginid)//loginid => roleID
        {
            AtendanceDet model = new AtendanceDet();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.attenId = Id;
            model.status_id = Status;
            model.RoleId = Loginid;
            int? updateemployee = this._attendBll.ApproveStatusBLL(model);
            string res = updateemployee.ToString();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoginBasedExamAttendanceList()
        {
            AtendanceDet modal = new AtendanceDet();
            modal.LoginRoleList = _attendBll.GetLoginRoleListBLL();
            int loginId = Convert.ToInt32(Session["LoginId"]);
            //modal.RoleId = Convert.ToInt32(Session["LoginId"]);
            modal.RoleId = loginId;
            //modal.user_role = Session["UserRole"].ToString();
            List<AtendanceDet> list = _attendBll.GetAttendanceDtlsByLoginIdBLL(modal).ToList();
            modal.attendanceList = list;
            return View(modal);
        }

        [HttpPost]
      //  public JsonResult AttendBackToCW(int Id, int Status, int Loginid, string comments, int StatusforCW)
             public JsonResult AttendBackToCW(string Id, string Status, string Loginid, string comments, string StatusforCW)
        {
            AtendanceDet model = new AtendanceDet();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.attenId = Convert.ToInt32(Id);
            model.exam_notif_status_id = Convert.ToInt32(Status);
            model.RoleId = Convert.ToInt32(Loginid);
            model.updatestatusCW = Convert.ToInt32(StatusforCW);
            model.comments = comments;
            string updateemployee = this._attendBll.UpdateCWStatusAttendanceBLL(model);
            string res = updateemployee.ToString();
            return Json(res, JsonRequestBehavior.AllowGet);
        }


    }
}