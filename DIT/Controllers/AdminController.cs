using DIT.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Admin;
using BLL.Common;
using Models.Admin;
using BLL.User;
using BLL.Admission;
using System.IO;
using log4net;
using log4net.Repository.Hierarchy;
using BLL.ExceptionLogger;

namespace DIT.Controllers
{
    //[SessionExpire]
    public class AdminController : Controller
    {
        private readonly IUserBLL _LoginService;
        private readonly ICommonBLL _CommonBLL;
        private readonly IAdminBLL _AdminBLL;
        private readonly IAdmissionBLL _admissionBll;
        private static readonly ILog Log = LogManager.GetLogger(typeof(AdmissionController));

        AdmissionController Admission = new AdmissionController();
        // GET: Admin
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //constructor
        public AdminController()
        {
            _admissionBll = new AdmissionBLL();

            _LoginService = new UserBLL();
            this._AdminBLL = new AdminBLL();
            _CommonBLL = new CommonBLL();
        }

        // GET: Admin
        public ActionResult AdminIndex()
        {
            if (Session["UserId"] != null)
            {
                Log.Info("Entered AdminIndex()");
                ViewBag.JavaScriptFunction = string.Empty;
                _CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
                clsEditProfile profileModel = new clsEditProfile();
                ViewBag.GenderList = Admission.GetGenderList();
                profileModel.EmployeeGenderList = _admissionBll.GetGenderDetailsBLL().Select(a => new SelectListItem { Text = a.Gender, Value = a.Gender_Id.ToString() });

                ViewBag.GalleryImgList = GetAllImagesFromFolder("Content/img/Gallery/");
                ViewBag.ScrollingImgList = GetAllImagesFromFolder("Content/img/Scrolling/");
                if (TempData.ContainsKey("UploadStatus"))
                    ViewBag.JavaScriptFunction = string.Format("ShowMessage1('{0}');", TempData["UploadStatus"].ToString());
                Log.Info("Left AdminIndex()");
                return View(profileModel);
            }
            return RedirectToAction("Index", "Home");
        }

        #region Drop-Down data
        //Get Units List for Unit drop down from tbl_LevelMaster
        [HttpGet]
        public ActionResult GetUnitsList()
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetUnitsListBLL();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //Get Sections List for Section drop down from tbl_section_master
        [HttpGet]
        public ActionResult GetSectionsList(int UnitId)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetSectionsListBLL(UnitId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //Get Wings List drop down from tbl_wing_master
        [HttpGet]
        public ActionResult GetWingsList()
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetWingsListBLL();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        //Get Roles Drop Down Data on 2nd, 3rd, 4th section
        [HttpGet]
        public ActionResult GetRolesList(int unitId, int sectionId)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetRolesListBLL(unitId, sectionId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update data in Tables/DB
        //Set/Update Roles in Role Master
        [HttpPost]
        public ActionResult GetUnitsList(clsAddRolesUnits roles)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetUnitsListBLL(roles);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddUserMapping(List<clsAddUserMapping> lstUserMap)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.AddUserMappingBLL(lstUserMap);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateActiveInActiveStatus(clsRoleMapping clsRole, string userName, bool status)
        {
            List<clsEditProfile> lstEditProfile = new List<clsEditProfile>();
            clsEditProfile objEditProfile = new clsEditProfile();
            objEditProfile.status = status;
            lstEditProfile.Add(objEditProfile);
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetEditProfileDataBLL(clsRole, lstEditProfile, userName);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region GridData
        // Get Existing Roles for DataTable Grid
        [HttpGet]
        public ActionResult GetRolesData()
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetRolesDataBLL("", "", 0, 0);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRolesDataById(string userId, string value)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetRolesDataBLL(userId, value, 0, 0);
            var res1 = _AdminBLL.GetUnitsListBLL();
            return Json(new { res = res, res1 = res1 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEditProfileData()
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var res = _AdminBLL.GetEditProfileDataBLL(userId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public ActionResult GetEditProfileData(clsEditProfile clsEdit)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            List<clsEditProfile> lstEditProfile = new List<clsEditProfile>();
            lstEditProfile.Add(clsEdit);
            clsRoleMapping clsRole = new clsRoleMapping();
            clsRole.UnitId = Convert.ToInt32(Session["UserId"].ToString());
            var res = _AdminBLL.GetEditProfileDataBLL(clsRole, lstEditProfile, Server.MapPath("Content/Uploads"));
            return Json(res, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]

        public ActionResult AddUserPostsavedata(clsEditProfile UserProfileData)
        {
            LoggerModel Logger = new LoggerModel();
            Logger.ErrorHandler("AddUserPostsavedata Method entered", 177);
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            List<clsEditProfile> lstEditProfile = new List<clsEditProfile>();
            lstEditProfile.Add(UserProfileData);
            clsRoleMapping clsRole = new clsRoleMapping();
            if (UserProfileData.userId != 0)
            {
                clsRole.UnitId = Convert.ToInt32(Session["UserId"].ToString());
            }
            else
            {
                clsRole.UnitId = 0;
            }
            Logger.ErrorHandler(clsRole.UnitId + "  role " , 193);

            var res = _AdminBLL.GetEditProfileDataBLL(clsRole, lstEditProfile, Server.MapPath("Content/Uploads"));
            Logger.ErrorHandler("result is  "  + res  , 196);

            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetEnteredRoleList(string prefix)
        {
            var searchlist = _AdminBLL.GetEnteredRoleListBLL(prefix);
            return Json(searchlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRoleMapData(int val)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _LoginService.MenuList(0);
            var res1 = new List<clsMenuList>();
            foreach (var item in res)
            {
                if (item.ParentMenu == (int)Models.CmnClass.MenuList.Modules && item.Url == "#")
                {
                    foreach (var item1 in res.Where(a => a.ParentMenu == item.MenuId))
                    {
                        clsMenuList objClsMenu = new clsMenuList();
                        objClsMenu.MenuId = Convert.ToInt32(item.MenuId);
                        objClsMenu.MenuName = item.MenuName;
                        objClsMenu.SubMenuId = Convert.ToInt32(item1.MenuId);
                        objClsMenu.SubMenuName = item1.MenuName;
                        res1.Add(objClsMenu);
                    }
                }
            }
            res1 = val != 0 ? res1.Where(a => a.MenuId == val).ToList() : res1;
            //var res = _AdminBLL.GetRoleMapDataBLL(val);
            //MenuId = tdm.dept_id,
            //                   SubMenuId = tsdm.sub_dept_id,
            //                   MenuName = tdm.dept_description,
            //                   SubMenuName = tsdm.sub_dept_description,
            return Json(res1, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetRoleMapData(List<clsRoleMapping> lstRoleMapping)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetRoleMapDataBLL(lstRoleMapping);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRoleMapDetailsData(clsRoleMapping clsRole)
        {

            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetRoleMapDetailsDataBLL(0, "", clsRole);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserDataByKGIDNumber(dynamic KGIDNumber)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetUserDataByKGIDNumberBLL(KGIDNumber);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserMappingDataById(int id, string value)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetUserMappingDataBLL(id, value);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetRoleMappingDataById(int id, string value)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetRoleMapDetailsDataBLL(id, value, new clsRoleMapping());
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserData()
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetEditProfileDataBLL(0);
            int c = 1;
            foreach (var x in res)
            {
                x.sino = c++;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserMappingData()
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetUserMappingDataBLL(0, "").ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetHolidayList(int id, string value)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetHolidayListBLL(id, value);
            int c = 1;
            foreach (var x in res)
            {
                x.sino = c++;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFiles(clsEditProfile clsEdit)
        {
            TempData["UploadStatus"] = string.Empty;
            HttpPostedFileBase[] files;
            string path = "~/Content/img/Gallery/";
            files = clsEdit.files;
            if (clsEdit.files1 != null)
            {
                path = "~/Content/img/Scrolling/";
                files = clsEdit.files1;
            }
            //Ensure model state is valid  
            if (files != null)
            {   //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        //Checking file is available to save.  
                        var InputFileName = Path.GetFileName(file.FileName).Replace(" ", "");
                        var ServerSavePath = Path.Combine(Server.MapPath(path) + InputFileName);
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.  
                    }
                }
                TempData["UploadStatus"] = files.Count().ToString() + " files uploaded successfully.";
                //TempData["UploadStatus"] = files.Count().ToString() + " files uploaded successfully.";
            }
            return RedirectToAction("AdminIndex");
        }

        [HttpPost]
        public ActionResult GetHolidayList(clsHolidayList clsHoliday)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            var res = _AdminBLL.GetHolidayListBLL(clsHoliday);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteImagesFromFolder(string path)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            bool isDelete = false;
            var filePath = Server.MapPath(path);
            filePath = filePath.Replace("Admin\\", "");
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                isDelete = true;
            }
            return Json(isDelete, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        private List<string> GetAllImagesFromFolder(string path)
        {
            //_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            List<string> ImgList = new List<string>();
            var filePath = Server.MapPath(path);
            filePath = filePath.Replace("Admin\\", "");
            string[] files = Directory.GetFiles(filePath);
            foreach (var file in files)
            {
                ImgList.Add(Path.GetFileName(file));
            }
            return ImgList;
        }

        #endregion

        [HttpPost]
        public JsonResult IsKGIDNumberExist(int EmployeeKGIDNumber)
       {
            int um_kgid_number = Convert.ToInt32(EmployeeKGIDNumber);

            var x = _CommonBLL.IsKGIDNumberExistBLL(um_kgid_number);
            bool exist;
            if (x == 0)
            {
                exist = false;
            }
            else
            {
                exist = true;
            }
            return Json(!exist);
        }


        [HttpPost]
        public JsonResult IsMobileNumberExist(string Mobile)
        {

            var x = _CommonBLL.IsMobileExistBLL(Mobile);
            bool exist;
            if (x == 0)
            {
                exist = false;
            }
            else
            {
                exist = true;
            }
            return Json(!exist);
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
            return Json(!exist);
        }


    }
}