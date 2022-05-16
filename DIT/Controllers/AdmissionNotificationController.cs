using BLL.Admission;
using BLL.ExamNotification;
using Models.ExamNotification;
using Models.Admission;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.User;
using Models;

namespace DIT.Controllers
{
    public class AdmissionNotificationController : Controller
    {
        private readonly IAdmissionNotificationBLL _AdminNotifBll;
        private readonly INotificationBLL _NotifBll;
    private IUserBLL _LoginService;
    string ErrorLogpath = ConfigurationManager.AppSettings["logPath"].ToString();
    Errorhandling mobjErrorLog = new Errorhandling();
    private readonly string UploadFolder = ConfigurationManager.AppSettings["TemplateDocumentsPath"];
        private readonly string UploadWordFolder = ConfigurationManager.AppSettings["WordTemplateDocumentsPath"];

        // strings to be replaced in template
        private readonly string ReplaceNoticeNumberKannada = "&lt;ಅಧಿಸೂಚನೆ ಸಂ.&gt;";
        private readonly string ReplaceNoticeDateKannada = "&lt;ಅಧಿಸೂಚನೆ ದಿನಾಂಕ&gt;";
        private readonly string ReplaceFeesLastDateKannada = "&lt;ಕೊನೆಯ ದಿನಾಂಕ&gt;";

        private readonly string ReplaceNoticeNumberEnglish = "&lt;Notication Number&gt;";
        private readonly string ReplaceNoticeDateEnglish = "&lt;Notication Date&gt;";
        private readonly string ReplaceFeesLastDateEnglish = "&lt;Last Date&gt;";

        // strings to generate file name
        private readonly string PdfFileNameFormat = "Notification_{0}.pdf";
        private readonly string DocFileNameFormat = "Notification_{0}.docx";

        // function to replace the strings
        private string ReplaceDataInTemplate(string content, AdmissionNotification model)
        {
            content = content.Replace(ReplaceNoticeDateKannada, model.Exam_notif_date.ToString("dd:MM:yyyy"));
            content = content.Replace(ReplaceNoticeNumberKannada, model.Exam_Notif_Number);

            content = content.Replace(ReplaceNoticeDateEnglish, model.Exam_notif_date.ToString("dd:MM:yyyy"));
            content = content.Replace(ReplaceNoticeNumberEnglish, model.Exam_Notif_Number);

            return content;
        }

        public AdmissionNotificationController()
        {
      this._LoginService = new UserBLL();
      _NotifBll = new NotificationBLL();
            _AdminNotifBll = new AdmissionNotificationBLL();
        }

        // GET: AdmissionNotification
        public ActionResult Index()
        {
      CheckBrowserAndMenuUl();
      return View();
        }

        //public ActionResult CreateAdmissionNotification(int? notificationId)
        //{
        //    AdmissionNotification model = new AdmissionNotification();
        //    string content = string.Empty;
        //    model.Exam_notif_date = DateTime.Now;
        //    //Kannada template
        //    content = "<p style=\"text-align: center; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಕರ್ನಾಟಕ ಸರ್ಕಾರ </p><p style=\"font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ದಿನಾಂಕ : &lt;ಅಧಿಸೂಚನೆ ದಿನಾಂಕ&gt;</p><p style=\"text-align: right; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ ಸಂ.: &lt;ಅಧಿಸೂಚನೆ ಸಂ.&gt;</p><p>&nbsp;</p><p style=\"text-align: center;font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ</p><br>";
        //    int id = Convert.ToInt32(Session["LoginId"].ToString());
        //    if (notificationId != null)
        //    {
        //        model = _AdminNotifBll.GetUpdateNotificationBLL(id, notificationId)[0];
        //    }

        //    model.content = content;
        //    //read the notification detail dropdowns
        //    model.CourseList = _NotifBll.GetCourseListBLL();
        //    model.DeptList = _AdminNotifBll.GetDepartmentListDLL();
        //    model.NotifDescList = _NotifBll.GetNotificationDescListBLL();
        //    model.selectTab = 0;
        //    return View(model);
        //}

        public ActionResult CreateAdmissionNotification1()
        {
      CheckBrowserAndMenuUl();
      return View();
        }

        private JsonResult Word2Pdf(string wordFile, string pdfFile)
        {
            try
            {
                // Create a new Microsoft Word application object
                Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                // C# doesn't have optional arguments so we'll need a dummy value
                object oMissing = System.Reflection.Missing.Value;
                word.Visible = false;
                word.ScreenUpdating = false;
                // Cast as Object for word Open method

                Object filename = (Object)wordFile;
                // Use the dummy value as a placeholder for optional arguments
                Microsoft.Office.Interop.Word.Document doc = word.Documents.Open(ref filename, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                doc.Activate();

                object outputFileName = pdfFile;
                object fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;

                // Save document into PDF Format
                doc.SaveAs(ref outputFileName,
                     ref fileFormat, ref oMissing, ref oMissing,
                     ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                     ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                     ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                // Close the Word document, but leave the Word application open.
                // doc has to be cast to type _Document so that it will find the
                // correct Close method.           

                object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
                ((Microsoft.Office.Interop.Word._Document)doc).Close(ref saveChanges, ref oMissing, ref oMissing);
                doc = null;

                // word has to be cast to type _Application so that it will find
                // the correct Quit method.
                ((Microsoft.Office.Interop.Word._Application)word).Quit(ref oMissing, ref oMissing, ref oMissing);
                word = null;

                return Json("success");

            }
            catch
            {
                return Json("Failed");
            }
        }

        //[HttpPost]
        //public ActionResult CreateAdmissionNotification(AdmissionNotification model)
        //{
        //    model.Role_id = Convert.ToInt32(Session["RoleId"]);
        //    model.user_id = Convert.ToInt32(Session["UserId"]);
        //    model.CourseList = _NotifBll.GetCourseListBLL();
        //    model.DeptList = _NotifBll.GetDepartmentListBLL();
        //    model.NotifDescList = _NotifBll.GetNotificationDescListBLL();
        //    if (TempData["Admission_Notif_Id"] != null)
        //        model.Admission_Notif_Id = Convert.ToInt32(TempData["Admission_Notif_Id"].ToString());
        //    if (ModelState.IsValid)
        //    {
        //        List<SelectListItem> notifDescr = (List<SelectListItem>)model.NotifDescList.Items;
        //        model.NotifDescName = notifDescr[model.NotifDescId].Text;

        //        // create the folder directory wherer notifcation are stored if it doesn't exists
        //        string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

        //        if (!Directory.Exists(DocumentsFolder))
        //        {
        //            Directory.CreateDirectory(DocumentsFolder);
        //        }

        //        // get full file path
        //        string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat, model.Exam_Notif_Number);
        //        string wordpath = string.Format(DocumentsFolder + DocFileNameFormat, model.Exam_Notif_Number);

        //        // replace the date amd notification number notification data
        //        model.content = ReplaceDataInTemplate(model.content, model);

        //        //Doc save
        //        Spire.Doc.Document doc = new Spire.Doc.Document();
        //        doc.AddSection();
        //        Spire.Doc.Documents.Paragraph para = doc.Sections[0].AddParagraph();
        //        para.AppendHTML(model.content);
        //        doc.SaveToFile(wordpath, FileFormat.Docx);
        //        // convert to pdf
        //        Word2Pdf(wordpath, pdfpath);

        //        // remove the word document post creation of respective pdf
        //        if (System.IO.File.Exists(wordpath))
        //        {
        //            System.IO.File.Delete(wordpath);
        //        }

        //        // now save the notification details to database
        //        model.SavePath = string.Format(UploadFolder + PdfFileNameFormat, model.Exam_Notif_Number);

        //        var saved = _AdminNotifBll.CreateAdmissionNotificationDetailsBLL(model);
        //        TempData["Saved"] = saved;
        //    }
        //    else
        //    {
        //        TempData["Saved"] = "Invalid";
        //    }
        //    return View(model);
        //}

        public Microsoft.Office.Interop.Word.Document wordDocument { get; set; }

        public ActionResult UpdateAdmissionNotificationDet()
        {
      CheckBrowserAndMenuUl();
      AdmissionNotification model = new AdmissionNotification();
            if (TempData["Admission_Notif_Id"] != null)
                model.Admission_Notif_Id = Convert.ToInt32(TempData["Admission_Notif_Id"].ToString());
            int id = Convert.ToInt32(Session["UserId"].ToString());
            model.GetUpdateNotifDet = _AdminNotifBll.GetUpdateNotificationBLL(id, model.Admission_Notif_Id);
            return View(model);
        }

        public ActionResult AdmissionNotificationBox()
        {

            var res = _AdminNotifBll.GetAdmissionNotificationBox();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Notificationstatus()
        {
      CheckBrowserAndMenuUl();
      Notification modal = new Notification();
            List<Notification> notifications = new List<Notification>();
            modal.login_id = Convert.ToInt32(Session["LoginId"]);
            List<Notification> list = _NotifBll.GetNotificationStatusBLL(modal).ToList();
            modal.LoginRoleList = _NotifBll.GetLoginRoleListBLL();
            modal.notifications = list;
            return View(modal);
        }

        [HttpPost]
        public ActionResult GetDetails(int ID)
        {
            try
            {
                int loginId = Convert.ToInt32(Session["LoginId"]);
                Notification details = this._NotifBll.GetViewBLL(ID, loginId);
                return Json(details, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public string UpdateTransStatus(int Id, int Status, int Loginid)
        {
      Utilities.Security.ValidateRequestHeader(Request);
      Notification model = new Notification();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.Exam_Notif_Id = Id;
            model.exam_notif_status_id = Status;
            model.RoleId = Loginid;
            int? updateemployee = this._NotifBll.UpdateTransStatusBLL(model);
            string res = updateemployee.ToString();
            return res;
        }

        [HttpPost]
        public JsonResult UpdateCommentTransStatus(int Id, int Status, int Loginid, string comments)
        {
      Utilities.Security.ValidateRequestHeader(Request);
      Notification model = new Notification();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.Exam_Notif_Id = Id;
            model.exam_notif_status_id = Status;
            model.RoleId = Loginid;
            model.comments = comments;
            string updateemployee = this._NotifBll.UpdateCommentsTransStatusBLL(model);
            return Json(updateemployee, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PublishNotification(int Id, int Status)
        {
      Utilities.Security.ValidateRequestHeader(Request);
      Notification model = new Notification();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.Exam_Notif_Id = Id;
            model.exam_notif_status_id = Status;
            string updateStatus = this._NotifBll.PublishNotificationBLL(model);
            string res = updateStatus.ToString();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Notificationstatus1()
        {
      CheckBrowserAndMenuUl();
      Notification modal = new Notification();
            modal.LoginRoleList = _NotifBll.GetLoginRoleListBLL();
            modal.RoleId = Convert.ToInt32(Session["LoginId"]);
            List<Notification> list = _NotifBll.GetNotificationStatus1BLL(modal).ToList();
            modal.notifications = list;
            return View(modal);
        }

        public JsonResult SaveRemarkAndForwardToUser(Notification model)
        {
            model.user_id = Convert.ToInt32(Session["LoginId"]);
            model.role_id = Convert.ToInt32(Session["UserId"]);
            var srList = _NotifBll.SaveRemarkAndForwardToUserBLL(model);
            return Json(srList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCommentRemarksDetails(int NotificationId)
        {
            var Remarks = _NotifBll.GetCommentRemarksDetailsBLL(NotificationId);
            return Json(Remarks, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BackToCW(int Id, int Status, int Loginid, string comments, int StatusforCW)
        {
            Notification model = new Notification();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.Exam_Notif_Id = Id;
            model.exam_notif_status_id = Status;
            model.RoleId = Loginid;
            model.updatestatusCW = StatusforCW;
            model.comments = comments;
            string updateemployee = this._NotifBll.UpdateCWStatusBLL(model);
            string res = updateemployee.ToString();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    public void CheckBrowserAndMenuUl()
    { // check direct url
      if (Session["UserId"] == null)
      {
        Response.Redirect("~/Home/Logout", true);
      }
      #region check Menu url
      var response = _LoginService.MenuList(Convert.ToInt32(Session["MenuMappingId"].ToString()));
      var currentpath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
      bool Validmenu = false;
      foreach (var mlist in response)
      {
        Validmenu = mlist.Url.Contains(currentpath);
      }
      if (!Validmenu)
      {
        Response.Redirect("~/Home/Logout", true);

      }
      #endregion
      #region CrossBrowser
      // var idurl = Request.RawUrl;
      if (Session["CurrentPage"] == null)
      {
        Session["CurrentPage"] = Request.RawUrl;
      }
      else
      {
        if (Session.IsNewSession && Request.UrlReferrer == null)
        {
          mobjErrorLog.Err_Handler("UrlReferrer", "NULL", Session["CurrentPage"].ToString());
          Response.Redirect("~/Home/Logout", true);
        }

      }
      #endregion


    }
  }
}