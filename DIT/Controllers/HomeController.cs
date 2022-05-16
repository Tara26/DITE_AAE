using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using BLL;
using Models.User;
using BLL.User;
using Models.ExamNotification;
using BLL.ExamNotification;
using System.Configuration;
using BLL.Admission;
using Models.Admission;
using Models.Admin;
using Models.Affiliation;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Net;
using DIT.Utilities;
using log4net;
using BLL.AuditTrail;
using System.IO;
using System.Web.Security;
using Models.Master;
using BLL.Common;
using System.Web.Configuration;
using BLL.Admin;
using Models.SMS;

namespace DIT.Controllers
{
    public class HomeController : Controller
    {

        private IUserBLL _LoginService;
        private readonly INotificationBLL _NotifBll;
        private readonly IAdmissionBLL _admissionBll;
        private readonly IAdminBLL _AdminBLL;
        private readonly ICommonBLL _CommonBLL;
        private readonly IAffiliationBLL _AffilBll;
        private readonly IAdmissionSeatMatrixBLL _adseatmatBll;
        private readonly IInsertaudittrailBLL _auditBll;
        private readonly string UploadFolder = ConfigurationManager.AppSettings["TemplateDocumentsPath"];
        private readonly string UploadWordFolder = ConfigurationManager.AppSettings["WordTemplateDocumentsPath"];
        Encryption encryption = new Encryption();
        string ErrorLogpath = ConfigurationManager.AppSettings["logPath"].ToString();
        Errorhandling mobjErrorLog = new Errorhandling();
        Audittraillog audittrail = new Audittraillog();
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        // (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        private readonly string valBtnClick = "Validate";
        private readonly string pwdMessage = "Password must contain at least one uppercase and lowercase letter and one number, and at least 8 or more characters";
        private readonly string pwdPattern = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
        public HomeController()
        {

            //this.loginBLL = loginBLL;
            this._LoginService = new UserBLL();
            _NotifBll = new NotificationBLL();
            _admissionBll = new AdmissionBLL();
            _AdminBLL = new AdminBLL();
            _AffilBll = new AffiliationBLL();
            _adseatmatBll = new AdmissionSeatMatrixBLL();
            _auditBll = new InsertAudittrailBLL();
            _CommonBLL = new CommonBLL();
        }
        public ActionResult Index1()
        {
            return View();
        }
        public ActionResult Gallery()
        {
            string path = "~/Content/img/Gallery/";
            if (TempData.ContainsKey("scrPhoto") && TempData["scrPhoto"].ToString() == "Scrolling")
                path = "~/Content/img/Scrolling/";
            ViewBag.ImgList = new List<string>();
            string[] files = Directory.GetFiles(Server.MapPath(path));
            foreach (var file in files)
            {
                ViewBag.ImgList.Add(Path.GetFileName(file));
            }
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult Index()
        {
            TempData["scrPhoto"] = "Scrolling";
            Gallery();
            string id = encryption.RandomString(20, false);
            if (Session["lid"] == null)
            {
                Session["lid"] = id;
            }
            Random d = new Random();
            string s = d.Next().ToString();
            Session["log"] = s;
            //if (!Session.IsNewSession && Request.UrlReferrer == null)
            //{
            //	return RedirectToAction("Index", "Home");
            //}
            Notification model = new Notification();
            model.GetUpdateNotifDet = _NotifBll.GetUpdateNotificationBLL();
            model.NotificationList = _NotifBll.GetNotificationListBLL();
            return View(model);
        }
        //Index Tentattive Marque
        public JsonResult GetIndexTentativeGradationMeritList()
        {
            List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
            int x = 1;
            MaritList = _admissionBll.GetIndexTentativeGradationMeritListBLL();
            if (MaritList != null)
            {
                foreach (var item in MaritList)
                {
                    item.slno = x;
                    //item.Rank = x;
                    x++;
                }
            }
            return Json(MaritList, JsonRequestBehavior.AllowGet);
        }
        //Index Final Marque
        public JsonResult GetIndexFinalGradationMeritList()
        {
            List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
            int x = 1;
            MaritList = _admissionBll.GetIndexFinalGradationMeritListBLL();
            if (MaritList != null)
            {
                foreach (var item in MaritList)
                {
                    item.slno = x;
                    // item.Rank = x;
                    x++;
                }
            }
            return Json(MaritList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DEO_upload()
        {
            ViewBag.Message = "DEO Upload.";

            return View();
        }

        public ActionResult Print_admin()
        {
            ViewBag.Message = "Print Admin.";

            return View();
        }

        public ActionResult Super_admin()
        {
            ViewBag.Message = "Super Admin.";

            return View();
        }
        public ActionResult Registration()
        {
            ViewBag.Message = "Registration";

            return View();
        }
        public ActionResult Print_markscard()
        {
            ViewBag.Message = "Print Marks Card.";

            return View();
        }


        public ActionResult Markscard()
        {
            ViewBag.Message = "Print Marks Card.";

            return View();
        }
        public ActionResult Print_markscard1()
        {
            ViewBag.Message = "Print Marks Card.";

            return View();
        }

        //[HttpPost]
        //public ActionResult Index(string UserName, string UserPassword)
        //{
        //    //    //string Password = DecryptStringAES(UserPassword);
        //    //    //var userData = _LoginService.LoginBll(UserName, Password);
        //    //  if (userData != null && userData.UserName != null)
        //    //    //{
        //    //    //    Session["UserId"] = userData.UserId;
        //    //    //    Session["CategoryID"] = userData.UserCategoryId;
        //    //    //    Session["UserRole"] = "Admin";
        //    //    //    Session["UserName"] = userData.UserName;
        //    //    //    Session["AgencyId"] = userData.AgencyId;
        //    //    //    Session["AgencyName"] = userData.AgencyName;
        //    //    //    Session["LocationId"] = userData.LocationId;
        //    //    //    Session["DesignationName"] = userData.DesignationName;
        //    //    //    Session["DistId"] = userData.DistId;

        //    //    //    if (userData.LocationId == 2 || userData.LocationId == 3)
        //    //    //    {
        //    //    //        Session["UserInfo"] = userData.DesignationName + " - " + userData.DistName;
        //    //    //    }
        //    //    //    else if (userData.LocationId == 1)
        //    //    //    {
        //    //    //        Session["UserInfo"] = userData.DesignationName + " - Karnataka";
        //    //    //    }
        //    //    //    Session["PendingApprovalCount"] = GetPendingApprovalCount();
        //    //    //    Session["PendingCostCount"] = GetCostEstimateApprovalCount();
        //    // return Json(true, JsonRequestBehavior.AllowGet);
        //    //    //}
        //    //    else
        //    //    {
        //    //        return Json(false, JsonRequestBehavior.AllowGet);
        //    //    }
        //}

        [HttpPost]
        public ActionResult Index(string userName, string Password, string HDNpwd)
        {
            Utilities.Security.ValidateRequestHeader(Request);
            bool success = false;
            var randomstring = Session["lid"].ToString();
            System.Web.SessionState.SessionIDManager manager = new System.Web.SessionState.SessionIDManager();
            HttpContext Context = System.Web.HttpContext.Current;
            string newId = manager.CreateSessionID(Context);
            Session["newId"] = newId;
            Response.Cookies["ASP.NET_SessionId"].Value = newId;
            var path = ConfigurationManager.AppSettings["sessionpath"].ToString();
            Response.Cookies["ASP.NET_SessionId"].Path = path;
            Session["sessionid"] = System.Web.HttpContext.Current.Session.SessionID;
            string HDNpwd1 = Request.Form["HDNpwd"];
            //Response.Cookies.Add(new HttpCookie("key", "value") { HttpOnly = true});
            //lock
            // Utilities.Security.regenerateId();
            try
            {
                var splitHiddenPwd = HDNpwd.Replace("$", "");
                var OriginalHiddenPwd = splitHiddenPwd.Remove(0, 20);
                OriginalHiddenPwd = OriginalHiddenPwd.Remove(OriginalHiddenPwd.Length - 20);
                if (!string.IsNullOrEmpty(OriginalHiddenPwd))
                {
                    if (!Regex.IsMatch(OriginalHiddenPwd, GetPasswordRegex()))
                    {
                        return Json(new { success, message = "Enter password as per password policy!" }, JsonRequestBehavior.AllowGet);
                    }

                    char[] array = OriginalHiddenPwd.ToCharArray();
                    if (char.IsLower(array[0]))
                    {
                        return Json(new { success, message = "Enter password as per password policy!" }, JsonRequestBehavior.AllowGet);
                    }

                }
                if (ModelState.IsValid)
                {
                    //var shaPasswordWithoutsalt = Utilities.Encryption.CreateHashWithoutSalt(OriginalHiddenPwd);

                    //  _LoginService.UpdatePassword(userName, shaPasswordWithoutSalt);
                    var shaPasswordWithoutSalt = Utilities.Encryption.CreateHashWithoutSalt(OriginalHiddenPwd);

                    var userData = _LoginService.LoginBll(userName, shaPasswordWithoutSalt);
                    if (userData != null && (userData.KGIDNum != null || userData.userName != null) && userData.Password != null)
                    {
                        if (userData.user_is_active == false)
                        {
                            return Json(new { success, message = "<br>The current user is deactivated. Please contact Super Admin for more info!!!" }, JsonRequestBehavior.AllowGet);
                        }
                        var shaPassword = Utilities.Encryption.CreateHashWithSalt(OriginalHiddenPwd, randomstring.ToString());
                        var shaUserName = Utilities.Encryption.CreateHashWithSalt(OriginalHiddenPwd, randomstring.ToString());
                        //	var shaPassword = CreateToken(userData.Password, Session["lid"].ToString());
                        //		var shaUserName = CreateToken(userData.userName, Session["lid"].ToString());

                        Session["UserId"] = userData.userId;
                        Session["LoginId"] = userData.userId;
                        if (userData.userName != null)
                            Session["UserName"] = userData.userName;
                        Session["KGIDNumber"] = userData.KGIDNum;
                        Session["RoleName"] = userData.RoleName;
                        Session["RoleId"] = userData.RoleId;
                        //Session["lid"] = null;
                        _LoginService.InsertUserHistory(userName, shaPasswordWithoutSalt);
                        var userHistory = _LoginService.GetUserHistory(userData.KGIDNum != null ? userData.KGIDNum.ToString() : userData.userName);

                        if (userHistory != null)
                        {
                            Session["LoginAttempt"] = userHistory.ToString();
                        }
                        else
                        {
                            Session["LoginAttempt"] = null;
                        }

                        userData.Password = userData.Password.ToString();
                        userData.userName = shaUserName.ToString();
                        //userData.RoleId = 0;
                        //userData.RoleName = "";
                        userData.HDNpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(userData.Password, "SHA256");
                        HDNpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(userData.Password, "SHA256");
                        if (/*(userData.SubDeptCount == 0 || userData.SubDeptCount == 1) &&*/ (Convert.ToString(userData.Password) == Convert.ToString(shaPasswordWithoutSalt)))
                        {
                            ContinueLogin(userData.SubDeptId, userData.RoleId);
                        }
                        else
                        {
                            return Json(new { success, message = "<br>The password entered does NOT match the UserName. Please enter correct password." }, JsonRequestBehavior.AllowGet);
                        }
                        #region Audit Trail
                        var currentAction = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                        var Currenturl = Request.Url;
                        audittrail.UserId = Session["UserName"].ToString();
                        audittrail.IPAddress = Request.UserHostAddress.ToString();
                        audittrail.DateAndTime = DateTime.Now.ToString();
                        audittrail.ActionPerformed = Convert.ToString(currentAction);
                        audittrail.Status = "User Loggedin Successfully";
                        _auditBll.InsertAuditLog(audittrail);

                        #endregion
                        success = true;
                        var subdeptCount = userData.RoleCount; // subdeptcount var is being userd for role
                        Logger.Info("User Logged in successfully");
                        return Json(new { success, subdeptCount }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if(userData != null && userData.Password != null)
                        {
                            mobjErrorLog.Err_Handler(" DITE: ", "Invalid Credentials - " + userName.ToString(), "Method:Home-Index");
                            return Json(new { success, message = "<br>Please contact system admin for Menu and Role Mapping!" }, JsonRequestBehavior.AllowGet);
                        }
                        else {
                        mobjErrorLog.Err_Handler(" DITE: ", "Invalid Credentials - " + userName.ToString(), "Method:Home-Index");
                        return Json(new { success, message = "<br>User Not Registered, Please Register In DITE Portal Using Employee Registration!" }, JsonRequestBehavior.AllowGet);
                        }
                        // return Json(success, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {
                mobjErrorLog.Err_Handler(" DITE: ", "Invalid Credentials - " + Session["UserId"].ToString(), "Method:Home-Index");
                return RedirectToAction("Dispose", "Home");
            }
            return Json(new { success, message = "<br>System unable to process your request!" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDepartments()
        {
            int loginId = Convert.ToInt32(Session["LoginId"].ToString());
            var res = _LoginService.GetDepartments(loginId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ContinueLogin(int? subDeptId, int roleId)
        {
            if (roleId == 0)
            {
                roleId = Convert.ToInt32(Session["RoleId"].ToString());
            }
            var loginId = Convert.ToInt32(Session["LoginId"].ToString());
            var Response = _LoginService.GetMappingId(roleId, loginId, subDeptId);
            if (Response != null)
            {
                var res = _LoginService.GetLoginRoles(loginId, roleId);
                if (res != null)
                {
                    Session["RoleId"] = res.Select(a => a.RoleID).FirstOrDefault();
                    Session["RoleName"] = res.Select(a => a.RoleName).FirstOrDefault();
                }
                Session["MenuMappingId"] = Response.MenuMappingId;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            int roleId = 0;
            if (Session["MenuMappingId"] != null)
                roleId = Convert.ToInt32(Session["MenuMappingId"].ToString());
            var response = _LoginService.MenuList(roleId);
            return View(response);
        }
        public ActionResult DITEIndex()
        {
            return View();
        }
        public ActionResult GetLoginRoles(int subDeptId)
        {
            var Response = _LoginService.GetLoginRoles(Convert.ToInt32(Session["LoginId"].ToString()), subDeptId);
            return Json(Response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AdmissionNotificationBox()
        {

            var res = _admissionBll.GetAdmissionNotificationBox();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AdmissionCalendarNotificationBox()
        {
            var res = _admissionBll.AdmissionCalendarNotificationBox();
            return Json(res, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetIndexseatavailabilityList()
        {
            List<SeatAvailabilty> seatAvail = new List<SeatAvailabilty>();
            seatAvail = _admissionBll.GetSeatAvailabilityListStatusFilter(0, 0, 0, 0, 0, 0, 0, 0, 0);
            //seatAvail = _admissionBll.GetIndexseatavailabilityList();
            int c = 1;
            foreach (var itm in seatAvail)
            {
                itm.Slno = c;
                c++;
            }
            return Json(seatAvail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstituteList()
        {
            AffiliationNested model = new AffiliationNested();
            model.Cwinst_list = new List<DeAffiliateInstitute>();
            List<DeAffiliateInstitute> InstDetails = new List<DeAffiliateInstitute>();
            InstDetails = _AffilBll.GetAllDeaffiliateInstitutePOPUP();

            int x = 1;
            //MaritList = _admissionBll.GetIndexGradationMeritListBLL(rbId);
            if (InstDetails != null)
            {
                foreach (var item in InstDetails)
                {
                    item.slno = x;
                    // item.Rank = x;
                    x++;
                }
            }
            return Json(new { list = InstDetails }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTradeList()
        {
            AffiliationNested model = new AffiliationNested();
            model.Cwinst_list = new List<DeAffiliateInstitute>();
            List<ActiveandDeactiveUnitsDeatils> InstDetails = new List<ActiveandDeactiveUnitsDeatils>();
            InstDetails = _AffilBll.GetAllActiveandDeactiveUnitwsieViewPOPUP().OrderByDescending(a => a.createdon).ToList();

            int x = 1;
            //MaritList = _admissionBll.GetIndexGradationMeritListBLL(rbId);
            if (InstDetails != null)
            {
                foreach (var item in InstDetails)
                {
                    item.slno = x;
                    // item.Rank = x;
                    x++;
                }
            }
            return Json(new { listi = InstDetails }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstituteViewList()
        {
            try
            {
                List<AffiliationCollegeDetails> pubs_list = new List<AffiliationCollegeDetails>(); ;
                // listOfSeatAllocation = _admissionBll.GetSeatMatrixViewListBLL();
                pubs_list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();
                int x = 1;
                if (pubs_list != null)
                {
                    foreach (var item in pubs_list)
                    {
                        item.Slno = x;
                        x++;
                    }
                }
                return Json(pubs_list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ViewSeatAutoAllotment(string lblstring)
        {
            int round = 0;
            List<SeatAutoAllocationModel> Response = null;
            var res = _admissionBll.GetEligibleDateFrmCalenderEventsBLL();

            // var ff=res["FromDt_1stRoundAdmissionProcess"].ToString();
            foreach (var item in res)
            {
                DateTime Nowdate = Convert.ToDateTime(System.DateTime.Now.Date.ToString());

                if ((item.FromDt_1stRoundAdmissionProcess <= Nowdate) && (item.ToDt_1stRoundAdmissionProcess >= Nowdate))
                {
                    Session["roundcount"] = 1;
                    lblstring = "true";
                    Response = _admissionBll.ViewSeatAutoAllotment(1);

                    return this.Json(new { Response, lblstring }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json(new { Response, lblstring }, JsonRequestBehavior.AllowGet);
                }

            }

            return this.Json(new { Response, lblstring }, JsonRequestBehavior.AllowGet);
            // return Json(Response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetViewSeatMatrixGrid(int ApplicantTypeId, int AcademicYearId, int Round, int? DistrictId, int? DivisionId, int? TalukId, int? InstituteId)
        {
            List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
            seatmatrix = _adseatmatBll.GetViewSeatMatrixGridBLL(0, ApplicantTypeId, AcademicYearId, Round, Convert.ToInt32(DistrictId), Convert.ToInt32(DivisionId), Convert.ToInt32(TalukId), Convert.ToInt32(InstituteId));
            int x = 1;
            if (seatmatrix != null)
            {
                foreach (var item in seatmatrix)
                {
                    item.slno = x;
                    x++;
                }
            }
            return Json(seatmatrix, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSeatmatrixNotification()
        {
            var res = _adseatmatBll.GetSeatmatrixNotification();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetConstituency()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                list = _AffilBll.GetConstiteuncyBLL();
            }
            catch (Exception ex)
            {

            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCourseTypes()
        {
            var Course_list = _AffilBll.GetCourseListBLL();
            return Json(Course_list, JsonRequestBehavior.AllowGet);
        }

        #region Get Division
        public ActionResult GetDivisions()
        {
            var Division_list = _AffilBll.GetDivisionListBLL();
            return Json(Division_list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get District by division
        [HttpGet]
        public ActionResult GetDistrictList(int Divisions)
        {
            var District_list = _AffilBll.GetDistrictListBLL(Divisions);
            return Json(District_list, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Get Taluk by District
        public JsonResult GetTaluk(int DistId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                list = _AffilBll.GetTalukBLL(DistId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Get All Affiliated Institute By Taluk
        public JsonResult GetAllAffiliatedInstituteByTaluk(int taluk_id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                list = _AffilBll.GetAllAffiliatedInstituteByTalukBLL(taluk_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult FilterPublishedCollegeDetails1(int? divisionId, int? districtId, int? talukId, int? constituencyId, int? Coursetype, int? iticollegeId)
        {
            List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();

            try
            {
                //int Course_Id = Convert.ToInt32(courseId);
                //int Division_Id = Convert.ToInt32(divisionId);
                //int District_Id = Convert.ToInt32(districtId);
                //int Trade_Id = Convert.ToInt32(tradeId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);
                int Taluk_Id = Convert.ToInt32(talukId);
                int Constituency_Id = Convert.ToInt32(constituencyId);
                int Coursetype_Id = Convert.ToInt32(Coursetype);
                int college_Id = Convert.ToInt32(iticollegeId);

                if (District_Id != 0 && Division_Id != 0 && Constituency_Id != 0 && Coursetype_Id != 0 && college_Id != 0 && Taluk_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.division_id == divisionId && a.dist_id == districtId && a.consti_id == Constituency_Id && a.course_code == Coursetype_Id && a.iti_college_id == college_Id && a.taluk_id == Taluk_Id).OrderByDescending(a => a.CreatedOn).ToList();
                    //list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeAndDistrictBLLFilter(District_Id, Constituency_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (District_Id != 0 && Division_Id != 0 && Constituency_Id != 0 && Coursetype_Id != 0 && Taluk_Id != 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.division_id == divisionId && a.dist_id == districtId && a.consti_id == Constituency_Id && a.course_code == Coursetype_Id).OrderByDescending(a => a.CreatedOn).ToList();


                }
                else if (District_Id == 0 && Division_Id != 0 && Constituency_Id == 0 && Coursetype_Id != 0 && Taluk_Id == 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.division_id == divisionId &&  a.course_code == Coursetype_Id).OrderByDescending(a => a.CreatedOn).ToList();


                }
                else if (District_Id != 0 && Division_Id != 0 && Constituency_Id == 0 && Coursetype_Id != 0 && Taluk_Id == 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.division_id == divisionId && a.course_code == Coursetype_Id && a.dist_id==districtId).OrderByDescending(a => a.CreatedOn).ToList();


                }
                else if (District_Id != 0 && Division_Id != 0 && Constituency_Id == 0 && Coursetype_Id != 0 && Taluk_Id != 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.division_id == divisionId && a.course_code == Coursetype_Id && a.dist_id == districtId && a.taluk_id == Taluk_Id).OrderByDescending(a => a.CreatedOn).ToList();


                }
                else if (District_Id != 0 && Division_Id != 0 && Constituency_Id != 0 && Coursetype_Id != 0 && Taluk_Id != 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.division_id == divisionId && a.course_code == Coursetype_Id && a.dist_id == districtId && a.taluk_id == Taluk_Id && a.consti_id == Constituency_Id  ).OrderByDescending(a => a.CreatedOn).ToList();


                }
                else if (District_Id != 0 && Division_Id != 0 && Constituency_Id != 0 && Coursetype_Id != 0 && Taluk_Id != 0 && college_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.division_id == divisionId && a.course_code == Coursetype_Id && a.dist_id == districtId && a.taluk_id == Taluk_Id && a.consti_id == Constituency_Id && a.iti_college_id == college_Id).OrderByDescending(a => a.CreatedOn).ToList();


                }
                else if (District_Id != 0 && Division_Id != 0 && Constituency_Id == 0 && Coursetype_Id != 0 && Taluk_Id != 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.division_id == divisionId && a.course_code == Coursetype_Id && a.dist_id == districtId && a.taluk_id==Taluk_Id).OrderByDescending(a => a.CreatedOn).ToList();


                }
                else if (District_Id != 0 && Division_Id != 0 && Constituency_Id != 0 && Coursetype_Id == 0 && Taluk_Id != 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.division_id == divisionId && a.dist_id == districtId && a.consti_id == Constituency_Id).OrderByDescending(a => a.CreatedOn).ToList();


                }
                else if (District_Id != 0 && Division_Id != 0 && Constituency_Id == 0 && Coursetype_Id == 0 && Taluk_Id != 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.division_id == divisionId && a.dist_id == districtId && a.taluk_id == Taluk_Id).OrderByDescending(a => a.CreatedOn).ToList();
                
                }
                else if (District_Id != 0 && Division_Id == 0 && Constituency_Id == 0 && Coursetype_Id == 0 && Taluk_Id != 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.dist_id == districtId).OrderByDescending(a => a.CreatedOn).ToList();


                }
                else if (District_Id == 0 && Division_Id == 0 && Constituency_Id == 0 && Coursetype_Id != 0 && Taluk_Id == 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.pub).Where(a => a.course_code == Coursetype_Id).OrderByDescending(a => a.CreatedOn).ToList();


                }
                else if (Taluk_Id != 0 && Division_Id == 0 && District_Id != 0 && Constituency_Id == 0 && Coursetype_Id == 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLLFilter(Taluk_Id, District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Taluk_Id != 0 && District_Id != 0 && Constituency_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLLFilter(Taluk_Id, District_Id, Constituency_Id).Where(a => a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }

                else if (Taluk_Id != 0 && District_Id != 0 && Constituency_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLLFilter(Taluk_Id, District_Id, Constituency_Id).Where(a => a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Taluk_Id != 0 && District_Id != 0 & Division_Id == 0  && Constituency_Id == 0 && Coursetype_Id == 0 && college_Id == 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLLFilter(Taluk_Id, District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Taluk_Id != 0 && Division_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionAndCourseBLLFilter(Taluk_Id, Division_Id).Where(a => a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Division_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLLFilter(District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Taluk_Id != 0 && Constituency_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLLFilter(Taluk_Id).Where(a => a.trade_id == Constituency_Id && a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Division_Id != 0 && Constituency_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLLFilter(Division_Id).Where(a => a.trade_id == Constituency_Id && a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }

                else if (Taluk_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLLFilter(Taluk_Id).Where(a => a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Division_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLLFilter(Division_Id).Where(a => a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLLFilter(District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Taluk_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeBLLFilter(Constituency_Id).Where(a => a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }
                //else if (Coursetype_Id != 0)
                //{
                //    list = _AffilBll.GetAllAffiliationCollegeDetailsBLLFilter().Where(a => a.status_id == (int)CsystemType.getCommon.Published && a.course_code == Coursetype_Id).OrderByDescending(a => a.CreatedOn).ToList();

                //}
                else
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLLFilter().Where(a => a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();
                }

                foreach (var item in list)
                {
                    if (item.FileUploadPath != "")
                    {
                        item.isSelect = System.IO.File.Exists(item.FileUploadPath);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdmissionSeatMatrixNested(int round, int instiId, int year, int tradeId)
        {
            var sList = _adseatmatBll.GetGenerateSeatMatrixBLLNested(round, instiId, year, tradeId);
            return Json(sList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserDataByKGIDNumber(dynamic KGIDNumber)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            List<clsAddUserMapping> res = new List<clsAddUserMapping>();
            res = _AdminBLL.GetUserDataByKGIDNumberBLL(KGIDNumber);
            //if (res.Count > 0)
            //{
            //    string str = res.Select(a => a.DateOfBirth).First().Split('-').Reverse();
            //    //res.Select(a => a.DateOfBirth).First() = str.Split('-').Reverse();
            //}
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public string sendSMS(string TO, string Message)
        {
            string n = "";
            string sURL;
            Message = Message.Replace("/n", "");
            sURL = ConfigurationManager.AppSettings["api_url"] + "api_key=" + ConfigurationManager.AppSettings["api_key"].ToString() + "&api_secret=" + ConfigurationManager.AppSettings["api_secret"] + "&from=" + "test" + "&to=" + TO + "&text=" + Message;

            try
            {
                using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString(sURL);
                    var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Admission.RootObject>(s);
                    n = responseObject.Status.ToString();
                    return n;
                }
            }
            catch (Exception ex)
            {

            }
            return n;
        }

        [HttpGet]
        public ActionResult Validate_otp(dynamic objApplicant)
        {
            string message = "";
            ViewBag.JavaScriptFunction = string.Empty;
            bool issucess = false;
            try
            {
                if (Session["generatedMobileOTP"] != null && Session["generatedEmailOTP"] != null && ModelState.ContainsKey("EmployeeKGIDNumber"))
                {
                    if (Session["generatedMobileOTP"].ToString() == objApplicant.MobileOTP.ToString() && (Session["generatedEmailOTP"].ToString() == objApplicant.EmailOTP.ToString())/*&& (objApplicant.EmailOTP.ToString() != "0" ? Session["generatedEmailOTP"].ToString() == objApplicant.EmailOTP.ToString() : false)*/)
                    {
                        issucess = true;
                        message = "Mobile and Email OTP Validated Successfully. Kindly set your password.";
                        ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Validate_otp", issucess);
                    }
                    else
                    {
                        message = "Incorrect OTP.";
                        ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Validate_otp", issucess);
                        // return this.Json(new { issucess, message = "Incorrect OTP." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (Session["generatedMobileOTP"] != null && ModelState.ContainsKey("Name")) {
                    if (Session["generatedMobileOTP"].ToString() == objApplicant.MobileOTP.ToString())
                    {
                        issucess = true;
                        message = "Mobile otp Validated Successfully. Kindly set your password.";
                        ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Validate_otp", issucess);
                    }
                    else
                    {
                        message = "Incorrect OTP.";
                        ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Validate_otp", issucess);
                        // return this.Json(new { issucess, message = "Incorrect OTP." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    message = "OTP time duration has been expired. Kindly click the resend OTP link.";
                    ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Validate_otp", issucess);
                    //  return this.Json(new { issucess, message = "OTP time duration has been expired. Kindly click the resend OTP link." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // mobjErrorLog.Err_Handler(ex.Message, "Home", "Generate_otp");
            }
            return View();
        }

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        [HttpGet]
        public ActionResult Generate_otp(dynamic objApplicant)
        {
            bool isExist = false;
            //isExist = _admissionBll.CheckApplicantExistorNot(objApplicant.MobileNumber, objApplicant.Email);
            if (isExist)
            {
                string message = "Entered Mobile Number/Email already exists in DITE, please register with a different mobile number and Email Address.";
                ViewBag.JavaScriptFunction = string.Format("ShowMessage1('{0}');", message);
            }
            else
            {
                bool issucess = false;
                //char[] charArr = "0123456789".ToCharArray();
                Session.Remove("generatedMobileOTP");
                Session.Remove("generatedEmailOTP");

                Session["generatedMobileOTP"] = null;
                Session["generatedEmailOTP"] = null;
                ViewBag.JavaScriptFunction = string.Empty;

                if (Session["generatedMobileOTP"] == null)
                {
                    //int MobileOtpData = GenerateRandomNo();
                    int MobileOtpData = 1234;
                    Session["generatedMobileOTP"] = MobileOtpData;
                }
                
                //while (true)
                //{
                //string strrandom = string.Empty;
                //Random objran = new Random();
                //for (int i = 0; i < 4; i++)
                //{
                //    //It will not allow Repetation of Characters
                //    int pos = objran.Next(1, charArr.Length);
                //    if (!strrandom.Contains(charArr.GetValue(pos).ToString())) strrandom += charArr.GetValue(pos);
                //    else i--;
                //}


                //}
                //var x = Session["generatedMobileOTP"];
                //var y = Session["generatedEmailOTP"];
                //Session["generatedMobileOTP"] = Session["generatedEmailOTP"];
                //var TOMobileNumber = string.Format("{0}{1}", "91", objApplicant.MobileNumber);
                string Message = "";
                if (ModelState.ContainsKey("EmployeeKGIDNumber"))
                {
                    if (Session["generatedEmailOTP"] == null)
                    {
                        //int EmailOtpData = GenerateRandomNo();
                        int EmailOtpData = 1234;
                        Session["generatedEmailOTP"] = EmailOtpData;
                    }
                    Message = CmnClass.EmailAndMobileMsgs.EmployeeOTPMsg;
                }
                else
                {
                    Message = CmnClass.EmailAndMobileMsgs.ApplicantOTPMsg;
                }
                string subject = CmnClass.EmailAndMobileMsgs.EmailOTPSubject;

                try
                {  
                    var smsresponse = "0";
                    string templateid = WebConfigurationManager.AppSettings["NewApplicantOTPRegistration"];
                   var OTPSuccuessFailure = SMSHttpPostClient.SendOTPMSG(Convert.ToString(objApplicant.MobileNumber), string.Format(Message, Session["generatedMobileOTP"].ToString()), templateid);
                        //smsresponse = _CommonBLL.SendSMSBLL(TOMobileNumber, string.Format(Message, objApplicant.Name, Session["generatedMobileOTP"].ToString()));
                        //smsresponse = sendSMS(TOMobileNumber, string.Format(Message, objApplicant.Name, Session["generatedMobileOTP"].ToString()));
                        var emailresponse = false;
                    if (ModelState.ContainsKey("EmployeeKGIDNumber"))
                    {
                        emailresponse = _CommonBLL.SendEmailBLL(objApplicant.Email, subject, string.Format(Message, Session["generatedEmailOTP"].ToString()));
                    }
                    if ((ModelState.ContainsKey("EmployeeKGIDNumber") && OTPSuccuessFailure.Contains("402") && OTPSuccuessFailure.Contains("MsgID")) && (emailresponse))
                    {
                        issucess = true;
                        string message = "OTP sent successfully. Kindly check entered mobile number/email.";
                        if (Session["Resend_OTP"] == null)
                        {
                            ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Generate_otp", issucess);
                        }
                        else
                        {
                            ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Resend_OTP", issucess);
                        }
                    }
                    else if ((OTPSuccuessFailure.Contains("402") && OTPSuccuessFailure.Contains("MsgID")) && emailresponse == false)
                    {
                        issucess = true;
                        string message = "OTP sent successfully. Kindly check your entered mobile number.";
                        if (Session["Resend_OTP"] == null)
                        {
                            ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Generate_otp", issucess);
                        }
                        else
                        {
                            ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Resend_OTP", issucess);
                        }
                    }
                    else
                    {
                        string message = "System failed to send OTP.";
                        ViewBag.JavaScriptFunction = string.Format("ShowMessage1('{0}');", message);

                        //  return this.Json(new { issucess, message = "System failed to send OTP." }, JsonRequestBehavior.AllowGet);

                    }

                }
                catch (Exception ex)
                {
                    //mobjErrorLog.Err_Handler(ex.Message, "Home", "Generate_otp");
                }
            }

            return View();
            // return this.Json(new { issucess, message = "System failed to send OTP." }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult NewApplicantRegister()
        {
            NewApplicant model = new NewApplicant();
            model.pwdMessage = pwdMessage;
            model.pwdPattern = pwdPattern;
            return View(model);
        }
        
        [HttpPost]
        public ActionResult NewApplicantRegister(NewApplicant newApplicant, string submit)
        {
            string message = "";
            bool issucess = false;
            ViewBag.JavaScriptFunction = string.Empty;
            submit = submit.Contains(valBtnClick) ? valBtnClick : submit;

            switch (submit)
            {
                case "Generate OTP":
                    if (ModelState.ContainsKey("MobileOTP"))
                        ModelState["MobileOTP"].Errors.Clear();
                    if (ModelState.ContainsKey("EmailOTP"))
                        ModelState["EmailOTP"].Errors.Clear();
                    if (ModelState.ContainsKey("ConfirmNewPassword"))
                        ModelState["ConfirmNewPassword"].Errors.Clear();
                    if (ModelState.ContainsKey("NewPassword"))
                        ModelState["NewPassword"].Errors.Clear();
                    if (ModelState.IsValid)
                    {
                        Generate_otp(newApplicant);

                    }
                    break;
                case "Validate":
                    if (ModelState.ContainsKey("ConfirmNewPassword"))
                        ModelState["ConfirmNewPassword"].Errors.Clear();
                    if (ModelState.ContainsKey("NewPassword"))
                        ModelState["NewPassword"].Errors.Clear();
                    if (ModelState.IsValid)
                    {
                        Validate_otp(newApplicant);
                    }
                    break;
                case "Resend OTP":
                    if (ModelState.ContainsKey("MobileOTP"))
                        ModelState["MobileOTP"].Errors.Clear();
                    if (ModelState.ContainsKey("EmailOTP"))
                        ModelState["EmailOTP"].Errors.Clear();
                    if (ModelState.ContainsKey("ConfirmNewPassword"))
                        ModelState["ConfirmNewPassword"].Errors.Clear();
                    if (ModelState.ContainsKey("NewPassword"))
                        ModelState["NewPassword"].Errors.Clear();
                    if (ModelState.IsValid)
                    {
                        Session["Resend_OTP"] = "Resend_OTP";
                        Generate_otp(newApplicant);
                    }
                    break;
                case "Register Applicant":
                    if (string.IsNullOrEmpty(newApplicant.NewPassword))
                    {
                        ModelState.AddModelError("NewPassword", "Please Enter New Password.");
                    }
                    if (string.IsNullOrEmpty(newApplicant.NewPassword))
                    {
                        ModelState.AddModelError("ConfirmNewPassword", "Please Enter Confirm Password.");
                    }
                    if (ModelState.IsValid)
                    {
                        #region Password validation
                        if (!string.IsNullOrEmpty(newApplicant.NewPassword))
                        {
                            if (!Regex.IsMatch(newApplicant.NewPassword, GetPasswordRegex()))
                            {
                                ModelState.AddModelError("NEWPASSWORD", "Password does not meet the criteria!");

                            }

                            char[] array = newApplicant.NewPassword.ToCharArray();
                            if (char.IsLower(array[0]))
                            {
                                ModelState.AddModelError("NEWPASSWORD", "Password does not meet the criteria!");
                            }

                        }
                        if (!string.IsNullOrEmpty(newApplicant.ConfirmNewPassword))
                        {
                            if (!Regex.IsMatch(newApplicant.ConfirmNewPassword, GetPasswordRegex()))
                            {
                                ModelState.AddModelError("ConfirmNewPassword", "Confirm Password does not meet the criteria!");

                            }

                            char[] array = newApplicant.ConfirmNewPassword.ToCharArray();
                            if (char.IsLower(array[0]))
                            {
                                ModelState.AddModelError("ConfirmNewPassword", "Confirm Password does not meet the criteria!");
                            }

                        }
                        if (newApplicant.NewPassword != newApplicant.ConfirmNewPassword)
                        {
                            //message = "New password and confirm password should be same!!!";
                            //ViewBag.JavaScriptFunction = string.Format("ShowMessage1('{0}');", message);

                            issucess = false;
                            message = string.Format("New password and confirm password should be same!!!", newApplicant.MobileNumber);
                            ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Register_Applicant", issucess);

                            return View(newApplicant);
                        }
                        #endregion

                        bool applicantInserted = _admissionBll.InsertNewApplicantRegistrationDetailsBLL(newApplicant);
                        if (applicantInserted == true)
                        {
                            string Message = "";
                            string subject = CmnClass.EmailAndMobileMsgs.EmailOTPSubject;
                            try
                            {
                                Message = String.Format(CmnClass.EmailAndMobileMsgs.ApplicantMobileConfirmMsg, newApplicant.Name, newApplicant.MobileNumber);
                                var TOMobileNumber = string.Format("{0}{1}", "91", newApplicant.MobileNumber);
                                var smsresponse = "0";
                                //var smsresponse = _CommonBLL.SendSMSBLL(TOMobileNumber, Message);
                                //smsresponse = sendSMS(TOMobileNumber, Message);
                                var emailresponse = true;
                                Message = String.Format(CmnClass.EmailAndMobileMsgs.ApplicantEmailConfirmMsg, newApplicant.Name, newApplicant.MobileNumber);
                                emailresponse = _CommonBLL.SendEmailBLL(newApplicant.Email, subject, Message);
                                emailresponse = true;
                                if ((smsresponse == "0") && (emailresponse))
                                {
                                    issucess = true;
                                    message = string.Format("Applicant Registered Successfully. Use registered mobile number ({0}) to login. ", newApplicant.MobileNumber);
                                    ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Register_Applicant", issucess);
                                    ModelState.Clear();
                                    Session["generatedMobileOTP"] = null;
                                    Session["generatedEmailOTP"] = null;
                                    //return this.Json(new { issucess, message = "Applicant Registered Successfully." }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    message = "System failed to register applicants.";
                                    ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Register_Applicant", issucess);
                                    // return this.Json(new { issucess, message = "Syatem failed to register applicants." }, JsonRequestBehavior.AllowGet);
                                }

                            }
                            catch (Exception ex)
                            {
                                // mobjErrorLog.Err_Handler(ex.Message, "Home", "NewApplicantRegister");
                            }

                        }
                        else
                        {
                            message = "System failed to register applicants.";
                            ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Register_Applicant", issucess);
                            //return this.Json(new { issucess, message = "Syatem failed to register applicants." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        message = "Kindly check input fields";
                        ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Register_Applicant", issucess);
                    }

                    break;
            }
            newApplicant.pwdMessage = pwdMessage;
            newApplicant.pwdPattern = pwdPattern;
            return View(newApplicant);

            }

        [HttpGet]
        public ActionResult NewEmployeeRegister()
        {
            NewEmployee model = new NewEmployee();
            model.EmployeeGenderList = _admissionBll.GetGenderDetailsBLL().Select(a=> new SelectListItem { Text = a.Gender, Value = a.Gender_Id.ToString()});
            model.pwdMessage = pwdMessage;
            model.pwdPattern = pwdPattern;
            return View(model);
        }

        [HttpPost]
        public ActionResult NewEmployeeRegister(NewEmployee newEmployee, string submit)
        {
            string strForgotPass = "ForgotPassword";
            string message = "";
            bool issucess = false;
            ViewBag.JavaScriptFunction = string.Empty;
            submit = submit.Contains(valBtnClick) ? valBtnClick : submit;

            switch (submit)
            {
                case "Generate OTP":
                    if (ModelState.ContainsKey("MobileOTP"))
                        ModelState["MobileOTP"].Errors.Clear();
                    if (ModelState.ContainsKey("EmailOTP"))
                        ModelState["EmailOTP"].Errors.Clear();
                    if (ModelState.ContainsKey("ConfirmNewPassword"))
                        ModelState["ConfirmNewPassword"].Errors.Clear();
                    if (ModelState.ContainsKey("NewPassword"))
                        ModelState["NewPassword"].Errors.Clear();
                    if (ModelState.IsValid || newEmployee.hdnValue == strForgotPass)
                    {
                        Generate_otp(newEmployee);

                    }
                    break;
                case "Validate":
                    if (ModelState.ContainsKey("ConfirmNewPassword"))
                        ModelState["ConfirmNewPassword"].Errors.Clear();
                    if (ModelState.ContainsKey("NewPassword"))
                        ModelState["NewPassword"].Errors.Clear();
                    if (ModelState.IsValid || newEmployee.hdnValue == strForgotPass)
                    {
                        Validate_otp(newEmployee);
                    }
                    break;
                case "Resend OTP":
                    if (ModelState.ContainsKey("MobileOTP"))
                        ModelState["MobileOTP"].Errors.Clear();
                    if (ModelState.ContainsKey("EmailOTP"))
                        ModelState["EmailOTP"].Errors.Clear();
                    if (ModelState.ContainsKey("ConfirmNewPassword"))
                        ModelState["ConfirmNewPassword"].Errors.Clear();
                    if (ModelState.ContainsKey("NewPassword"))
                        ModelState["NewPassword"].Errors.Clear();
                    if (ModelState.IsValid)
                    {
                        Session["Resend_OTP"] = "Resend_OTP";
                        Generate_otp(newEmployee);
                    }
                    break;
                case "Register Employee":
                    if (string.IsNullOrEmpty(newEmployee.NewPassword))
                    {
                        ModelState.AddModelError("NewPassword", "Please Enter New Password.");
                        if (newEmployee.hdnValue == strForgotPass)
                        {
                            message = "Please Enter New Password."; break;
                        }
                    }
                    if (string.IsNullOrEmpty(newEmployee.NewPassword))
                    {
                        ModelState.AddModelError("ConfirmNewPassword", "Please Enter Confirm Password.");
                        if (newEmployee.hdnValue == strForgotPass)
                        {
                            message = "Please Enter Confirm Password."; break;
                        }
                    }
                    if (ModelState.IsValid || newEmployee.hdnValue == strForgotPass)
                    {
                        #region Password validation
                        if (!string.IsNullOrEmpty(newEmployee.NewPassword))
                        {
                            if (!Regex.IsMatch(newEmployee.NewPassword, GetPasswordRegex()))
                            {
                                ModelState.AddModelError("NEWPASSWORD", "Password does not meet criteria!");
                                if (newEmployee.hdnValue == strForgotPass)
                                {
                                    message = "Password does not meet criteria!"; break;
                                }
                            }

                            char[] array = newEmployee.NewPassword.ToCharArray();
                            if (char.IsLower(array[0]))
                            {
                                ModelState.AddModelError("NEWPASSWORD", "Password does not meet criteria!");
                                if (newEmployee.hdnValue == strForgotPass)
                                {
                                    message = "Password does not meet criteria!"; break;
                                }
                            }

                        }
                        if (!string.IsNullOrEmpty(newEmployee.ConfirmNewPassword))
                        {
                            if (!Regex.IsMatch(newEmployee.ConfirmNewPassword, GetPasswordRegex()))
                            {
                                ModelState.AddModelError("ConfirmNewPassword", "Confirm Password does not meet criteria!");
                                if (newEmployee.hdnValue == strForgotPass)
                                {
                                    message = "Confirm Password does not meet criteria!"; break;
                                }
                            }

                            char[] array = newEmployee.ConfirmNewPassword.ToCharArray();
                            if (char.IsLower(array[0]))
                            {
                                ModelState.AddModelError("ConfirmNewPassword", "Confirm Password does not meet criteria!");
                                if (newEmployee.hdnValue == strForgotPass)
                                {
                                    message = "Confirm Password does not meet criteria!"; break;
                                }
                            }

                        }
                        if (newEmployee.NewPassword != newEmployee.ConfirmNewPassword)
                        {
                            //message = "New password and confirm password should be same";
                            //ViewBag.JavaScriptFunction = string.Format("ShowMessage1('{0}');", message);
                            newEmployee.EmployeeGenderList = _admissionBll.GetGenderDetailsBLL().Select(a => new SelectListItem { Text = a.Gender, Value = a.Gender_Id.ToString() });

                            issucess = false;
                            message = string.Format("New password and confirm password should be same!!!");
                            ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Register Employee", issucess);

                            if (newEmployee.hdnValue != strForgotPass)

                                return View(newEmployee);
                            else
                                break;
                        }
                        #endregion
                        var shaPasswordWithoutSalt = Utilities.Encryption.CreateHashWithoutSalt(newEmployee.NewPassword);
                        newEmployee.NewPassword = shaPasswordWithoutSalt;
                        bool applicantInserted = _admissionBll.InsertNewEmployeeRegistrationDetailsBLL(newEmployee);
                        if (applicantInserted == true)
                        {
                            string Message = String.Format(CmnClass.EmailAndMobileMsgs.EmployeeMobileConfirmMsg, newEmployee.Name, newEmployee.EmployeeKGIDNumber);
                            string subject = CmnClass.EmailAndMobileMsgs.EmpEmailSubject;
                            var TOMobileNumber = string.Format("{0}{1}", "91", newEmployee.MobileNumber);
                            try
                            {
                                var smsresponse = "0";
                                //var smsresponse = _CommonBLL.SendSMSBLL(TOMobileNumber, Message);
                                //smsresponse = sendSMS(TOMobileNumber, Message);
                                var emailresponse = true;
                                Message = String.Format(CmnClass.EmailAndMobileMsgs.EmployeeEmailConfirmMsg, newEmployee.Name, newEmployee.EmployeeKGIDNumber);

                                emailresponse = _CommonBLL.SendEmailBLL(newEmployee.Email, subject, Message);
                                emailresponse = true;
                                if ((smsresponse == "0") && (emailresponse))
                                {
                                    issucess = true;
                                    message = string.Format("Employee Registered Successfully. Please Use KGID number ({0}) to login. ", newEmployee.EmployeeKGIDNumber);
                                    if (newEmployee.hdnValue == strForgotPass)
                                        message = "Password updated Successfully!! Please use New Password to Login.";

                                    ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Register Employee", issucess);
                                    ModelState.Clear();
                                    NewEmployee model = new NewEmployee();
                                    //return this.Json(new { issucess, message = "Applicant Registered Successfully." }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    message = "System failed to register applicants.";
                                    ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Register Employee", issucess);
                                    // return this.Json(new { issucess, message = "Syatem failed to register applicants." }, JsonRequestBehavior.AllowGet);
                                }

                            }
                            catch (Exception ex)
                            {
                                // mobjErrorLog.Err_Handler(ex.Message, "Home", "NewApplicantRegister");
                            }

                        }
                        else
                        {
                            message = "System failed to register applicants.";
                            ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Register Employee", issucess);
                            //return this.Json(new { issucess, message = "Syatem failed to register applicants." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        message = "Kindly check input fields";
                        ViewBag.JavaScriptFunction = string.Format("ShowMessage('{0}','{1}','{2}');", message, "Register_Applicant", issucess);
                    }

                    break;
            }
            if (newEmployee.hdnValue == strForgotPass)
            {
                string message1 = "";
                if (ViewBag.JavaScriptFunction != null && ViewBag.JavaScriptFunction.ToString().Trim() != "")
                {
                    message1 = Regex.Replace(ViewBag.JavaScriptFunction.ToString(), @"[^0-9a-zA-Z\._, (/]", "");
                    message = message1.Substring(message1.IndexOf('(') + 1).Split(',')[0].Replace("'", "");
                    if (message1.IndexOf(',', message1.IndexOf(',') + 1) > 1)
                        issucess = bool.Parse(message1.Split(',')[2].Replace("'", ""));
                }
                return Json(new { issucess, message }, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                newEmployee.EmployeeGenderList = _admissionBll.GetGenderDetailsBLL().Select(a => new SelectListItem { Text = a.Gender, Value = a.Gender_Id.ToString() });
                newEmployee.pwdMessage = pwdMessage;
                newEmployee.pwdPattern = pwdPattern;
                return View(newEmployee);
            }

        }

        #region 5.ChangePassword
        public ActionResult ChangePassword(string id)
        {
            Session["CurrentUrl"] = null;
            #region logs
            if (!string.IsNullOrEmpty(id))
            {
                if (Session["log"].ToString() != id)
                {
                    // return Json(false, JsonRequestBehavior.AllowGet);
                    return Redirect("~Error.htm");
                }
            }

            #endregion
            //Utilities.Security.CrossBrowser();
            Random d = new Random();
            string s = d.Next().ToString();
            // Session["log"] = s;
            ViewData["ID"] = Session["log"];
            return View();
        }
        [HttpPost]
        // [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            bool success = false;
            string userName = Session["UserName"].ToString();
            var DecryptOldPassword = Utilities.Encryption.DecryptStringAES(oldPassword);
            var DecryptNewPassword = Utilities.Encryption.DecryptStringAES(newPassword);
            var DecryptConfirmPassword = Utilities.Encryption.DecryptStringAES(confirmPassword);
            try
            {
                #region validate password
                //var splitoldPassword = oldPassword.Replace("$", "");
                //var OriginaloldPassword = splitoldPassword.Remove(0, 50);
                //	OriginaloldPassword = OriginaloldPassword.Remove(OriginaloldPassword.Length - 50);
                var OriginaloldPassword = Utilities.Encryption.DecryptStringAES(oldPassword);
                //var splitnewPassword = newPassword.Replace("$", "");
                //var OriginalnewPassword = splitnewPassword.Remove(0, 50);
                //	OriginalnewPassword = OriginalnewPassword.Remove(OriginalnewPassword.Length - 50);
                var OriginalnewPassword = Utilities.Encryption.DecryptStringAES(newPassword);
                //var splitconfirmPassword = confirmPassword.Replace("$", "");
                //var OriginalconfirmPassword = splitconfirmPassword.Remove(0, 50);
                //	OriginalconfirmPassword = OriginalconfirmPassword.Remove(OriginalconfirmPassword.Length - 50);
                var OriginalconfirmPassword = Utilities.Encryption.DecryptStringAES(confirmPassword);
                #endregion
                #region password validation
                if (!string.IsNullOrEmpty(OriginaloldPassword))
                {
                    if (!Regex.IsMatch(OriginaloldPassword, GetPasswordRegex()))
                    {
                        ModelState.AddModelError("NewPassword", "Password does not meet policy!");

                    }

                    char[] array = OriginaloldPassword.ToCharArray();
                    if (char.IsLower(array[0]))
                    {
                        ModelState.AddModelError("NewPassword", "Password does not meet policy!");
                    }

                }
                if (!string.IsNullOrEmpty(OriginalnewPassword))
                {
                    if (!Regex.IsMatch(OriginalnewPassword, GetPasswordRegex()))
                    {
                        ModelState.AddModelError("NewPassword", "Password does not meet policy!");

                    }

                    char[] array = OriginalnewPassword.ToCharArray();
                    if (char.IsLower(array[0]))
                    {
                        ModelState.AddModelError("NewPassword", "Password does not meet policy!");
                    }

                }
                if (!string.IsNullOrEmpty(OriginalconfirmPassword))
                {
                    if (!Regex.IsMatch(OriginalconfirmPassword, GetPasswordRegex()))
                    {
                        ModelState.AddModelError("ConfirmPassword", "Password does not meet policy!");

                    }

                    char[] array = OriginalconfirmPassword.ToCharArray();
                    if (char.IsLower(array[0]))
                    {
                        ModelState.AddModelError("ConfirmPassword", "Password does not meet policy!");
                    }

                }
                if (userName == OriginalnewPassword)
                {
                    ModelState.AddModelError("NewPassword", "Password should match with UserName!");
                    return this.Json(new { success, message = "Password should match with UserName!" }, JsonRequestBehavior.AllowGet);
                }


                #endregion
                if (ModelState.IsValid)
                {
                    var shaOldPassword = Utilities.Encryption.CreateHashWithoutSalt(OriginaloldPassword);
                    var shaNewPassword = Utilities.Encryption.CreateHashWithoutSalt(OriginalnewPassword);
                    //var shaOldPassword = Utilities.Encryption.CreateToken(OriginaloldPassword, Session["lid"].ToString());
                    //var shaNewPassword = Utilities.Encryption.CreateToken(OriginalnewPassword, Session["lid"].ToString());
                    var shaConfirmPassword = Utilities.Encryption.CreateHashWithoutSalt(OriginalconfirmPassword);
                    if (oldPassword != newPassword)
                    {
                        if (newPassword == confirmPassword)
                        {
                            try
                            {
                                _LoginService.ChangePassword(userName, shaOldPassword, shaNewPassword);
                            }
                            catch (Exception ex)
                            {
                                mobjErrorLog.Err_Handler(ex.Message, "Controller:Home", "Action:ChangePassword_post");
                                ModelState.AddModelError("NEWPASSWORD", "Sorry! System unable to process your request");
                                return this.Json(new { success, message = "Sorry! System unable to process your request!" }, JsonRequestBehavior.AllowGet);
                            }

                            OriginaloldPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(OriginaloldPassword, "SHA256");
                            OriginalnewPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(OriginalnewPassword, "SHA256");
                            OriginalconfirmPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(OriginalconfirmPassword, "SHA256");
                            var currentAction = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                            var Currenturl = Request.Url;
                            audittrail.UserId = userName;
                            audittrail.IPAddress = Request.UserHostAddress.ToString();
                            audittrail.DateAndTime = DateTime.Now.ToString();
                            audittrail.ActionPerformed = Convert.ToString(currentAction);
                            audittrail.Status = "Password Changed Successfully";
                            _auditBll.InsertAuditLog(audittrail);
                            Logger.Info("Password Changed Successfully");
                            success = true;
                            Session["LoginAttempt"] = null;
                            return Json(success, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            ModelState.AddModelError("ChangePassword", "New Password and Confirm Password does not Equal!");
                            mobjErrorLog.Err_Handler("System unable to process your request", "Controller:Home", "Action:ChangePassword_post");
                            return this.Json(new { success, message = "New Password and Confirm Password does not Equal!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ChangePassword", "Old Password and New Password Should be different!");
                        mobjErrorLog.Err_Handler("System unable to process your request", "Controller:Home", "Action:ChangePassword_post");
                        return this.Json(new { success, message = "Old Password and New Password Should be different!" }, JsonRequestBehavior.AllowGet);
                    }



                }
            }

            catch (Exception ex)
            {
                mobjErrorLog.Err_Handler(ex.Message, "Controller:Home", "Action:ChangePassword_post");
                ModelState.AddModelError("NEWPASSWORD", "Sorry! Password could not be changed. Please re-enter as per instruction");
                return this.Json(new { success, message = "Sorry! Password could not be changed. Please re-enter as per instruction!" }, JsonRequestBehavior.AllowGet);
            }

            //	ErrorUpdate();


            return Json(success, JsonRequestBehavior.AllowGet);
        }
        #endregion
        internal string GetPasswordRegex()
        {

            StringBuilder sbPasswordRegx = new StringBuilder(string.Empty);
            try
            {

                XDocument xmlDoc = XDocument.Load(Request.MapPath(@"~/PasswordPolicy.xml"));
                var passwordSetting = (from p in xmlDoc.Descendants("Password")
                                       select new PasswordSetting
                                       {
                                           Duration = int.Parse(p.Element("duration").Value),
                                           MinLength = int.Parse(p.Element("minLength").Value),
                                           MaxLength = int.Parse(p.Element("maxLength").Value),
                                           NumsLength = int.Parse(p.Element("numsLength").Value),
                                           SpecialLength = int.Parse(p.Element("specialLength").Value),
                                           UpperLength = int.Parse(p.Element("upperLength").Value),
                                           SpecialChars = p.Element("specialChars").Value,
                                           LowerLength = int.Parse(p.Element("LowerLength").Value)
                                       }).First();


                //min and max
                sbPasswordRegx.Append(@"(?=^.{" + passwordSetting.MinLength + "," + passwordSetting.MaxLength + "}$)");
                //numbers length
                sbPasswordRegx.Append(@"(?=(?:.*?\d){" + passwordSetting.NumsLength + "})");
                //a-z characters
                // sbPasswordRegx.Append(@"(?=.*[a-z])");
                sbPasswordRegx.Append(@"(?=(?:.*?[a-z]){" + passwordSetting.LowerLength + "})");
                //A-Z length
                sbPasswordRegx.Append(@"(?=(?:.*?[A-Z]){" + passwordSetting.UpperLength + "})");
                //special characters length
                sbPasswordRegx.Append(@"(?=(?:.*?[" + passwordSetting.SpecialChars + "]){" + passwordSetting.SpecialLength + "})");
                //(?!.*\s) - no spaces
                //[0-9a-zA-Z!@#$%*()_+^&] -- valid characters
                sbPasswordRegx.Append(@"(?!.*\s)[0-9a-zA-Z" + passwordSetting.SpecialChars + "]*$");
            }
            catch (Exception ex)
            {
                //mobjErrorLog.Err_Handler(ex.Message, "Home", "GetPasswordRegex");
            }
            return sbPasswordRegx.ToString();
        }
        public void ClearCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            Response.Cookies.Clear();
            Request.Cookies.Clear();
        }
        private void clearchachelocalall()
        {
            //%USERPROFILE%: represents the location of the profile of the current user.
            string GooglePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Google\Chrome\User Data\Default\";
            string MozilaPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Mozilla\Firefox\";
            string Opera1 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Opera\Opera";
            string Opera2 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Opera\Opera";
            string Safari1 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Apple Computer\Safari";
            string Safari2 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Apple Computer\Safari";
            string IE1 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Microsoft\Intern~1";
            string IE2 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Microsoft\Windows\History";
            //string IE3 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Microsoft\Windows\Tempor~1";
            string IE4 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Microsoft\Windows\Cookies";
            //string Flash = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Macromedia\Flashp~1";
            //Call This Method ClearAllSettings and Pass String Array Param
            ClearAllSettings(new string[] { GooglePath, MozilaPath, Opera1, Opera2, Safari1, Safari2, IE1, IE2, IE4 });

        }
        public void ClearAllSettings(string[] ClearPath)
        {
            foreach (string HistoryPath in ClearPath)
            {
                if (Directory.Exists(HistoryPath))
                {
                    DoDelete(new DirectoryInfo(HistoryPath));
                }

            }
        }
        void DoDelete(DirectoryInfo folder)
        {
            try
            {

                foreach (FileInfo file in folder.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    { }

                }
                foreach (DirectoryInfo subfolder in folder.GetDirectories())
                {
                    DoDelete(subfolder);
                }
            }
            catch
            {
            }
        }
        [HttpGet]
        public JsonResult Logout()
        {
            _LoginService.UpdatewhileLogOut(Session["UserName"].ToString());
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetValidUntilExpires(true);
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            //Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddYears(-1);
            foreach (var cookie in Request.Cookies.AllKeys)
            {
                Request.Cookies.Remove(cookie);
            }
            foreach (var cookie in Response.Cookies.AllKeys)
            {
                Response.Cookies.Remove(cookie);
            }
            return Json(@Url.Action("Index", "Home"), JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", "Home");
        }

        //public ActionResult Logout()
        //{
        //    _LoginService.UpdatewhileLogOut(Session["UserName"].ToString());
        //    ClearCache();
        //    //clearchachelocalall();
        //    Session["UserId"] = null;
        //    Session["LoginId"] = null;
        //    Session["UserName"] = null;
        //    Session["KGIDNumber"] = null;

        //    Session["RoleName"] = null;
        //    Session["RoleId"] = null;
        //    Session["lid"] = null;

        //    Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate,pre-check=0,post-check=0,s-maxage=0");
        //    Response.AddHeader("X-XSS-Protection", "1; mode=block");
        //    Response.Cache.SetAllowResponseInBrowserHistory(false);
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Cache.SetValidUntilExpires(true);
        //    Response.Cache.AppendCacheExtension("no-cache, no-store, max-age=0, must-revalidate,pre-check=0,post-check=0,s-maxage=0");
        //    Response.AppendHeader("Pragma", "no-cache");
        //    Response.AppendHeader("Expires", "0");
        //    Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
        //    Response.AddHeader("Pragma", "no-cache");
        //    Response.AddHeader("Expires", "0");
        //    Response.Cookies.Clear();
        //    FormsAuthentication.SignOut();
        //    Session.Clear();
        //    Session.Abandon();
        //    Session.RemoveAll();
        //    foreach (var cookie in Request.Cookies.AllKeys)
        //    {
        //        Request.Cookies.Remove(cookie);
        //    }

        //    //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
        //    //cookie.Expires = DateTime.Now.AddYears(-1);
        //    ////  response.Cookies.Add(cookie);
        //    //Response.Cookies.Add(cookie);
        //    ////  lock
        //    System.Web.SessionState.SessionIDManager manager = new System.Web.SessionState.SessionIDManager();
        //    HttpContext Context = System.Web.HttpContext.Current;
        //    string newId = manager.CreateSessionID(Context);
        //    Response.Cookies["ASP.NET_SessionId"].Value = newId;
        //    Response.Cookies["ASP.NET_SessionId"].Path = ConfigurationManager.AppSettings["sessionpath"].ToString();
        //    Utilities.Security.regenerateId();
        //    return RedirectToAction("Dispose", "Home");
        //    //Session.Clear();
        //    //Session.Abandon();
        //    //return RedirectToAction("Index");
        //}
       
        
        
        
        
        //commented code captcha by rohan mahagaonkar on 25-04-2022
        
        public ActionResult generateCaptchaold()
        {
            CaptchaMessage msg_ = new CaptchaMessage();
            string text = "";
            try
            {
                System.Drawing.FontFamily family = new System.Drawing.FontFamily("Arial");
                CaptchaImage img = new CaptchaImage(100, 50, family);
                text = img.CreateRandomText(6);
                img.SetText(text);
                img.GenerateImage();
                string path = "";
                path = Server.MapPath("~/Content/img");
                //this.Session.SessionID.ToString()
                if (System.IO.File.Exists(path + "\\" + "Captcha" + ".png"))
                {
                    System.IO.File.Delete(path + "\\" + "Captcha" + ".png");
                }
                img.Image.Save(path + "\\" + "Captcha" + ".png", System.Drawing.Imaging.ImageFormat.Png);
                Session["captchaText"] = text;

            }
            catch (Exception ex)
            {
                mobjErrorLog.Err_Handler(ex.Message, "Controller:Admin", "Action:generateCaptcha");
            }

            msg_.Message = "Captcha" + ".png?t=" + DateTime.Now.Ticks;
            msg_.Msgcode = text;

            return Json(msg_, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCaptcha()
        {

            Random rnd = new Random();
            String b = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string c = "0123456789";
            String captcha = "";

            //StringBuilder captcha = new StringBuilder();

            //for (int i = 0; i < 5; i++)
            //{
            //    captcha.Append(rnd.Next(1, 9).ToString() + " ");

            //    captcha.Append(rnd.Next("A", "Z").ToString() + " ");

            //    //captcha.Append(rnd.Next(1, 9).ToString());
            //}

            for (int i = 0; i < 5; i++)
            {
                int a = 0;

                if (i == 0 || i == 2 || i == 4)
                {
                    a = rnd.Next(b.Length);
                    captcha = captcha + b.ElementAt(a) + " ";
                }//string.Lenght gets the size of string
                else
                {
                    a = rnd.Next(c.Length);
                    captcha = captcha + c.ElementAt(a) + " ";
                }

            }


            var _res = captcha.ToString().TrimEnd(' ');
            var _resSession = _res;
            _resSession = _resSession.Replace(" ", "");
            Session.Remove("GenCaptcha");
            Session["GenCaptcha"] = _resSession;
            // return 
            return Json(_res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dispose()
        {
            Session.Abandon();
            //  lock
            System.Web.SessionState.SessionIDManager manager = new System.Web.SessionState.SessionIDManager();
            HttpContext Context = System.Web.HttpContext.Current;
            string newId = manager.CreateSessionID(Context);
            Response.Cookies["ASP.NET_SessionId"].Value = newId;

            var path = ConfigurationManager.AppSettings["sessionpath"].ToString();
            Response.Cookies["ASP.NET_SessionId"].Path = path;

            return RedirectToAction("Index");
        }

        public ActionResult Encrypt_Click(object sender, EventArgs e)
        {
            // Get configuration information about Web.config
            Configuration config =
                WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            // Let's work with the <connectionStrings> section
            ConfigurationSection connectionStrings = config.GetSection("connectionStrings");
            if (connectionStrings != null)
            {
                // Only encrypt the section if it is not already protected
                if (!connectionStrings.SectionInformation.IsProtected)
                {
                    // Encrypt the <connectionStrings> section using the 
                    // DataProtectionConfigurationProvider provider
                    connectionStrings.SectionInformation.ProtectSection(
                        "DataProtectionConfigurationProvider");
                    config.Save();

                    // Refresh the Web.config display
                    //DisplayWebConfig();
                }
            }
            return null;
        }


        [HttpGet]
        public JsonResult Get_Notification_PublishDetails()
        {
            var MaxpublishData = this._CommonBLL.Get_Notification_PublishDetails_Bll();
            return Json(MaxpublishData, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadAffiliationDoc(int CollegeId = 0, int Trade_Id = 0, int? shift_id = 0, int? flag = 0)
        {
            var data = _AffilBll.GetAllAffiliationDocForDownload(CollegeId, Trade_Id, shift_id, flag);
            byte[] fileBytes = System.IO.File.ReadAllBytes(data.FileName);
            string fileName = System.IO.Path.GetFileName(data.FileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        [HttpPost]
        public JsonResult IsMobileNumberExist(string MobileNumber)
        {

            var x = _CommonBLL.IsMobileExistBLL(MobileNumber);
            bool exist;
            if (x == 0)
            {
                exist = false;
            }
            else
            {
                exist = true;
            }
            return Json(exist);
        }

        public JsonResult IsEmailExist(string Email)
        {

            var x = _CommonBLL.IsEmailExistBLL(Email);
            bool exist;
            if (x == 0)
            {
                exist = false;
            }
            else
            {
                exist = true;
            }
            return Json(exist);
        }
    }
}
