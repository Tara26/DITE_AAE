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
using System.Data;
using Spire.Xls;
using log4net;
using System.Dynamic;
using Models.Affiliation;
using System.Data.OleDb;
using System.Globalization;
using DIT.Utilities;
using BLL.Common;
using Models.Logs;


namespace DIT.Controllers
{
    [SessionExpire]
    public class AffiliationController : Controller
    {
        private readonly string UploadFolder = ConfigurationManager.AppSettings["TemplateDocumentsPath"];
        private static readonly ILog Log = LogManager.GetLogger(typeof(AffiliationController));
        private readonly IAffiliationBLL _AffilBll;
        private readonly ICommonBLL _CommonBLL;
        private readonly string ExcelMasterDataTempleUploadFolder = "/Content/Affiliation upload templete.xlsx";
        
         
        public AffiliationController()
        {
            _AffilBll = new AffiliationBLL();
            _CommonBLL = new CommonBLL();
        }
        // GET: Affiliation
        public ActionResult Index()
        {
            return View();
        }

        #region Upload Affiliation Details
        //GET:UploadAffiliationDetails
        public ActionResult AffiliationCollegeDetails()
        {
            if (Session["UserId"] != null)
            {
                _CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);

                var role_id = Convert.ToInt32(Session["RoleId"]);

                AffiliationNested model = new AffiliationNested();
                model.aff = new AffiliationCollegeDetails();
                model.list = new List<AffiliationCollegeDetails>();
                model.pubs = new List<AffiliationCollegeDetails>();
                model.pubs_list = new List<AffiliationCollegeDetails>();
                model.upl_list = new List<AffiliationCollegeDetails>();
                model.aff.trades_list = new List<AffiliationTrade>();
                AffiliationTrade trade = new AffiliationTrade();
                model.aff.trades_list.Add(trade);

                Session["TradeShift"] = new NestedTradeSession();
                NestedTradeSession session = (NestedTradeSession)Session["TradeShift"];
                session.sessions = new List<TradeShiftSessions>();

                try
                {

                    if ((int)Session["RoleId"] == (int)CsystemType.getCommon.ITIAdmin)
                    {
                        int UserId = (int)Session["UserId"];
                        int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
                        model.aff_list = _AffilBll.GetAllMyAffiliatedCollegesBLL(college_id);
                    }
                    else
                    {
                        if ((int)Session["RoleId"] != (int)CsystemType.getCommon.CaseWorker)
                        {
                            //model.aff_list = _AffilBll.GetAllAffiliateCollegeListDLL(role_id).OrderByDescending(a => a.CreatedOn).ToList();
                            model.aff_list = _AffilBll.GetAllAffiliateCollegeListDLL1().OrderByDescending(a => a.CreatedOn).ToList();
                        }
                        else
                        {       //tab 1
                            model.upl_list = _AffilBll.GetAllUploadedAffiliationBLL().OrderByDescending(a => a.CreatedOn).ToList();
                            //tab2 
                            model.list = _AffilBll.GetAllAffiliationCollegeDetailsBLL().ToList();
                            model.list = model.list.Union(model.upl_list).OrderByDescending(a => a.CreatedOn).ThenBy(a => a.iti_college_id).ToList();


                        }
                        //tab 5
                        model.pubs_list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();
                        //model.aff.flag = 1;
                        if (TempData["flag"] != null)
                        {
                            model.aff.flag = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Entered Exception:" + ex.Message.ToString());
                    model.aff.flag = 0;

                }

                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AffiliationCollegeDetails1()
        {
            TempData["flag"] = 1;


          return  RedirectToAction("AffiliationCollegeDetails");
        }


            //POST:UploadAffiliationDetails
            [HttpPost]
        public ActionResult UploadAffiliationFile()
        {
            AffiliationNested output = new AffiliationNested();
            output.aff = new AffiliationCollegeDetails();
            output.upl_list = new List<AffiliationCollegeDetails>();
            try
            {
                if (Request.Files.Count > 0)
                {
                    string[] _fileformat = Request.Files[0].FileName.Split('.');
                    string _farmat = _fileformat.LastOrDefault();

                    var supportedTypes = new[] { "xls", "xlsx", "csv" };
                    if (supportedTypes.Contains(_farmat))
                    {
                        string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

                        Random rand = new Random();

                        string File_name = "ExcelUploadAffiliation" + rand.Next() + "." + _farmat;

                        HttpPostedFileBase file = Request.Files[0];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = File_name.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = File_name;
                        }
                        var uploadRootFolderInput = DocumentsFolder;
                        if (!Directory.Exists(uploadRootFolderInput))
                        {
                            Directory.CreateDirectory(uploadRootFolderInput);
                        }

                        var directoryFullPathInput = uploadRootFolderInput;
                        fname = Path.Combine(directoryFullPathInput, fname);
                        file.SaveAs(fname);

                        string validate = ValidateAffiliationUploadExcelFile(fname);
                        if (validate == "valid")
                        {
                            output.upl_list = GetUploadAffiliationInstitutesList(fname);
                            if (output.upl_list.Count() > 0)
                            {
                                TempData["AffiliationUploadFilePath"] = fname;
                                output.aff.flag = 1;
                                output.aff.status = "Upload Successful";
                            }
                            else
                            {
                                TempData["AffiliationUploadFilePath"] = null;
                                output.aff.flag = 0;
                                output.aff.status = "Your File Contains No Data";
                            }
                        }
                        else
                        {
                            //Directory.Delete(fname);

                            output.aff.flag = 0;
                            output.aff.status = validate;
                            TempData["AffiliationUploadFilePath"] = null;
                        }
                        var jsonResult = Json(output);
                        jsonResult.MaxJsonLength = int.MaxValue;
                        return jsonResult;
                        //return Json(output);

                    }
                    else
                    {
                        output.aff.flag = 0;
                        output.aff.status = "Invalid File Type";
                        return Json(output);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
                output.aff.flag = 0;
                output.aff.status = "Error occured while uploading the data";
            }

            return Json(output);
        }

        //GET:SaveUploadFileData
        public JsonResult SaveUploadFileData()
        {
            Models.UploadAffiliation output = new Models.UploadAffiliation();

            try
            {

                var uploadedPath = TempData["AffiliationUploadFilePath"] as string;
                if (uploadedPath != null)
                {
                    DataTable dt = new DataTable();
                    if (Path.GetExtension(uploadedPath) == ".csv")
                    {
                        dt = GetDataTableFromCsv(uploadedPath);
                    }
                    else
                    {
                        dt = ConvertXSLXtoDataTable(uploadedPath);
                    }

                    var UserId = Convert.ToInt32(Session["RoleId"]);
                    output = _AffilBll.UploadAffiliationDetailsBLL(dt, UserId);

                    if (output.flag == 1)
                    {
                        Log.Info("Upload file successful");
                        TempData["AffiliationUploadFilePath"] = null;
                    }
                    else
                    {
                        output.flag = 0;
                        output.status = output.status;
                        TempData["AffiliationUploadFilePath"] = uploadedPath;
                        Log.Error("Entered Exception:" + output.status);
                    }

                }
                else
                {
                    output.status = "Pls Upload File";
                    output.flag = 0;
                    TempData["AffiliationUploadFilePath"] = uploadedPath;
                    Log.Error("Entered Exception: Upload file path not found!");
                }


            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
                output.flag = 0;
                output.status = "Error occured while uploading the data";
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }

        //POST Upload Multiple Affiliation Files
        [HttpPost]
        public ActionResult UploadMultipleAffiliationFiles(int[] CollegeId)
        {
            List<UploadAffiliation> output = new List<UploadAffiliation>();
            try
            {
                if (Request.Files.Count > 0)
                {
                    for (var y = 0; y < Request.Files.Count; y++)
                    {
                        string[] _fileformat = Request.Files[y].FileName.Split('.');
                        string _farmat = _fileformat.LastOrDefault();
                        UploadAffiliation Filestatus = new UploadAffiliation();

                        var supportedTypes = new[] { "pdf" };
                        if (supportedTypes.Contains(_farmat))
                        {
                            string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

                            Random rand = new Random();

                            string File_name = "AffiliationCollegePdf" + rand.Next() + "." + _farmat;

                            HttpPostedFileBase file = Request.Files[0];
                            string fname;
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = File_name.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = File_name;
                            }
                            var uploadRootFolderInput = DocumentsFolder;
                            if (!Directory.Exists(uploadRootFolderInput))
                            {
                                Directory.CreateDirectory(uploadRootFolderInput);
                            }

                            var directoryFullPathInput = uploadRootFolderInput;
                            fname = Path.Combine(directoryFullPathInput, fname);
                            file.SaveAs(fname);


                            var fileName = Request.Files[y].FileName;
                            var filePath = fname;
                            var College_Id = CollegeId[y];

                            Filestatus = _AffilBll.UploadMultipleAffiliationFilesBLL(College_Id, fileName, filePath);
                            if (Filestatus.flag == 0)
                            {
                                Log.Error("Entered Exception:" + Filestatus.status);
                                Filestatus.status = "File Name: " + Request.Files[y].FileName + " " + " Upload Failed!";
                                Filestatus.flag = 0;

                                output.Add(Filestatus);
                            }
                        }
                        else
                        {

                            Filestatus.status = "File Name: " + Request.Files[y].FileName + " " + " Upload Failed!";
                            Filestatus.flag = 0;

                            output.Add(Filestatus);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message);
            }

            return Json(output);
        }
        #endregion

        #region Convert Excel into Data Table
        public static DataTable ConvertXSLXtoDataTable(string strFilePath)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(strFilePath);
            Worksheet sheet = workbook.Worksheets[0];
            DataTable dt = sheet.ExportDataTable();
            try
            {
                dt = dt.AsEnumerable().Where(row => !row.ItemArray.All(f => f is null || String.IsNullOrWhiteSpace(f.ToString()))).CopyToDataTable();
            }
            catch (Exception ex)
            {
                Log.Error("Exception Entered: " + ex.Message.ToString());
                dt = null;
            }
            return dt;
        }

        #endregion

        #region Get Course
        public ActionResult GetCourseTypes()
        {
            var Course_list = _AffilBll.GetCourseListBLL();
            return Json(Course_list, JsonRequestBehavior.AllowGet);
        }
        #endregion

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

        #region Get Trades
        [HttpGet]
        public ActionResult GetTradesList()
        {
            var Trades_list = _AffilBll.GetTradesBLL();
            return Json(Trades_list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get District
        [HttpGet]
        public ActionResult GetDistricts()
        {
            var District_list = _AffilBll.GetDistrictsBLL();
            return Json(District_list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Affiliation Schemes
        [HttpGet]
        public ActionResult GetAffiliationSchemes()
        {
            var Schemes = _AffilBll.GetAffiliationSchemesBLL();
            return Json(Schemes, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Search Submitted or Rejected College Details

        public JsonResult FilterCollegeDetails(int? courseId, int? divisionId, int? districtId, int? tradeId)
        {
            List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            List<AffiliationCollegeDetails> up_list = new List<AffiliationCollegeDetails>();
            try
            {
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);
                int Trade_Id = Convert.ToInt32(tradeId);

                if (Course_Id != 0 && District_Id != 0 && Trade_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id, (int)CsystemType.getCommon.Rejected).OrderByDescending(a => a.CreatedOn).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                else
               if (Course_Id != 0 && Division_Id != 0 && District_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id,District_Id, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id, District_Id, (int)CsystemType.getCommon.Rejected).OrderByDescending(a => a.CreatedOn).ToList();
                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id, District_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }

                else if (District_Id != 0 && Division_Id != 0 && Trade_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByTradeAndDistrictBLL(District_Id, Trade_Id, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByTradeAndDistrictBLL(District_Id, Trade_Id, (int)CsystemType.getCommon.Rejected).OrderByDescending(a => a.CreatedOn).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeAndDistrictBLL(District_Id, Trade_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                else if (Course_Id != 0 && District_Id != 0 && Trade_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id, (int)CsystemType.getCommon.Rejected).OrderByDescending(a => a.CreatedOn).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                else if (Course_Id != 0 && District_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id, District_Id, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id, District_Id, (int)CsystemType.getCommon.Rejected).OrderByDescending(a => a.CreatedOn).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id, District_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                else if (Course_Id != 0 && Division_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionAndCourseBLL(Course_Id, Division_Id, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionAndCourseBLL(Course_Id, Division_Id, (int)CsystemType.getCommon.Rejected).OrderByDescending(a => a.CreatedOn).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionAndCourseBLL(Course_Id, Division_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                else if (Division_Id != 0 && District_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id, (int)CsystemType.getCommon.Rejected).OrderByDescending(a => a.CreatedOn).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                else if (Course_Id != 0 && Trade_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id, (int)CsystemType.getCommon.Submitted).Where(a => a.trade_id == Trade_Id).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id, (int)CsystemType.getCommon.Rejected).Where(a => a.trade_id == Trade_Id).OrderByDescending(a => a.CreatedOn).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id).Where(a => a.trade_id == Trade_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                else if (Division_Id != 0 && Trade_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id, (int)CsystemType.getCommon.Submitted).Where(a => a.trade_id == Trade_Id).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id, (int)CsystemType.getCommon.Rejected).Where(a => a.trade_id == Trade_Id).OrderByDescending(a => a.CreatedOn).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id).Where(a => a.trade_id == Trade_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }

                else if (Course_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id, (int)CsystemType.getCommon.Rejected).OrderByDescending(a => a.CreatedOn).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                else if (Division_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id, (int)CsystemType.getCommon.Rejected).OrderByDescending(a => a.CreatedOn).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                else if (District_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id, (int)CsystemType.getCommon.Rejected).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                else if (Trade_Id != 0)
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsByTradeBLL(Trade_Id, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsByTradeBLL(Trade_Id, (int)CsystemType.getCommon.Rejected).OrderByDescending(a => a.CreatedOn).ToList();

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeBLL(Trade_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                else
                {
                    //List<AffiliationCollegeDetails> list_1 = _AffilBll.GetAllAffiliationCollegeDetailsBLL(100, (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList(); // get Added/Updated colleges by case worker
                    //List<AffiliationCollegeDetails> list_2 = _AffilBll.GetAllAffiliationCollegeDetailsBLL(100, (int)CsystemType.getCommon.Rejected).OrderByDescending(a => a.CreatedOn).ToList(); // get Rejected colleges by JDD

                    //list = list_1.Union(list_2).ToList();
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL().OrderByDescending(a => a.CreatedOn).ToList();

                }

                up_list = _AffilBll.GetAllUploadedAffiliationBLL().OrderByDescending(a => a.CreatedOn).ToList();

                if (Course_Id != 0)
                {
                    up_list = up_list.Where(a => a.course_code == Course_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                if (Division_Id != 0)
                {
                    up_list = up_list.Where(a => a.division_id == Division_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                if (District_Id != 0)
                {
                    up_list = up_list.Where(a => a.dist_id == District_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }
                if (Trade_Id != 0)
                {
                    up_list = up_list.Where(a => a.trade_id == Trade_Id).OrderByDescending(a => a.CreatedOn).ToList();
                }

                list = list.Union(up_list).OrderByDescending(a => a.CreatedOn).ThenBy(a => a.iti_college_id).ToList();
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
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Search Approved College Details

        public JsonResult FilterApprovedCollegeDetails(int? courseId, int? divisionId, int? districtId, int? tradeId)
        {
            List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();

            try
            {
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);
                int Trade_Id = Convert.ToInt32(tradeId);

                if (Course_Id != 0 && Division_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id, District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else if (Course_Id != 0 && District_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else if (District_Id != 0 && Division_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeAndDistrictBLL(District_Id, Trade_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else if (Course_Id != 0 && District_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else if (Course_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id, District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else if (Course_Id != 0 && Division_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionAndCourseBLL(Course_Id, Division_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else if (Division_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else if (Course_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id).Where(a => a.trade_id == Trade_Id && a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else if (Division_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id).Where(a => a.trade_id == Trade_Id && a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }

                else if (Course_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else if (Division_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else if (District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else if (Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeBLL(Trade_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();

                }
                else
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.Approved).Where(a => a.status_id == (int)CsystemType.getCommon.Approved).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Search Submitted College Details

        public JsonResult FilterSubmittedCollegeDetails(int? courseId, int? divisionId, int? districtId, int? tradeId)
        {
            List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();

            try
            {
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);
                int Trade_Id = Convert.ToInt32(tradeId);

                if (Course_Id != 0 && Division_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id, District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Course_Id != 0 && District_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (District_Id != 0 && Division_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeAndDistrictBLL(District_Id, Trade_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Course_Id != 0 && District_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Course_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id, District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Course_Id != 0 && Division_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionAndCourseBLL(Course_Id, Division_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Division_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Course_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id).Where(a => a.trade_id == Trade_Id && a.status_id == (int)CsystemType.getCommon.Submitted).ToList();

                }
                else if (Division_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id).Where(a => a.trade_id == Trade_Id && a.status_id == (int)CsystemType.getCommon.Submitted).ToList();

                }

                else if (Course_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Division_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeBLL(Trade_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Submitted).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.Submitted).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get College Details
        public JsonResult GetCollegeDetails(int collegId)
        {
            AffiliationNested model = new AffiliationNested();
            model.his_list = new List<TradeHistory>();
            Session["TradeShift"] = new NestedTradeSession();



            try
            {

                model.aff = _AffilBll.GetAAffiliationCollegeDetailsBLL(collegId);
                if (model.aff.trades_list != null)
                {
                    NestedTradeSession session = (NestedTradeSession)Session["TradeShift"];
                    session.sessions = new List<TradeShiftSessions>();

                    int inc = 1; //due to radom generating same number
                    foreach (var item in model.aff.trades_list)
                    {
                        if (item.list.Count() > 0)
                        {
                            int sessionKey = GenerateRandomNo() + inc;

                            TradeShiftSessions addSession = new TradeShiftSessions();
                            addSession.list = new List<TradeShift>();
                            addSession.list = item.list;
                            addSession.sessionKey = sessionKey;

                            session.sessions.Add(addSession);

                            item.sessionKey = addSession.sessionKey;
                            inc++;
                        }
                        else
                        {
                            item.sessionKey = 0;
                        }
                    }
                }
                model.inst_list = _AffilBll.GetInstitutionTypesBLL().ToList();
                model.loca_type_list = _AffilBll.GetLocationTypesBLL().ToList();
                model.trades_list = _AffilBll.GetTradesBLL().Select(a => new SelectListItem
                {
                    Text = a.trade_code.ToString(),
                    Value = a.trade_id.ToString()
                }).ToList();
                model.div_list = _AffilBll.GetDivisionListBLL().Select(a => new SelectListItem
                {
                    Text = a.division_name,
                    Value = a.division_id.ToString()

                }).ToList();
                if (model.aff.division_id != 0 && model.aff.division_id != null)
                {
                    model.dist_list = _AffilBll.GetDistrictListBLL(Convert.ToInt32(model.aff.division_id)).Select(a => new SelectListItem
                    {
                        Text = a.district,
                        Value = a.district_id.ToString()

                    }).ToList();
                }
                else
                {
                    model.dist_list = _AffilBll.GetDistrictsBLL().ToList();
                }

                model.taluk_list = _AffilBll.GetTalukBLL(Convert.ToInt32(model.aff.dist_id)).ToList();
                model.consti_list = _AffilBll.GetConstiteuncyBLL().ToList();
                model.pancha_list = _AffilBll.GetPanchayatBLL(Convert.ToInt32(model.aff.taluk_id)).ToList();
                model.village_list = _AffilBll.GetVillageBLL(Convert.ToInt32(model.aff.taluk_id)).ToList();
                model.css_code_list = _AffilBll.GetCssCodeBLL().ToList();
                model.his_list = _AffilBll.GetAllTradeHistoriesBLL(collegId).OrderByDescending(a => a.CreatedOn).ToList();
                model.cou_list = _AffilBll.GetCourseListBLL().Select(a => new SelectListItem
                {
                    Text = a.course_name,
                    Value = a.course_id.ToString()

                }).ToList();
                model.schem_list = _AffilBll.GetAffiliationSchemesBLL();
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Trade details for update
        public JsonResult GetUpdateTradeDetails(int trade_id)
        {
            AffiliationNested model = new AffiliationNested();
            model.his_list = new List<TradeHistory>();

            try
            {
                var rol_id = Convert.ToInt32(Session["RoleId"]);

                model.aff = _AffilBll.GetATradeDetailsBLL(trade_id, rol_id);
                if (rol_id == (int)CsystemType.getCommon.ITIAdmin /*|| rol_id == (int)CsystemType.getCommon.CaseWorker*/)
                {
                    if (model.aff.status_id == (int)CsystemType.getCommon.Published || model.aff.status_id == (int)CsystemType.getCommon.pub)
                    {
                        model.aff.en_edit = true; // for institute login
                    }

                }
                model.inst_list = _AffilBll.GetInstitutionTypesBLL().ToList();
                model.loca_type_list = _AffilBll.GetLocationTypesBLL().ToList();
                model.trades_list = _AffilBll.GetTradesBLL().Select(a => new SelectListItem
                {
                    Text = a.trade_code.ToString(),
                    Value = a.trade_id.ToString()
                }).ToList();
                model.div_list = _AffilBll.GetDivisionListBLL().Select(a => new SelectListItem
                {
                    Text = a.division_name,
                    Value = a.division_id.ToString()

                }).ToList();
                if (model.aff.division_id != 0 && model.aff.division_id != null)
                {
                    model.dist_list = _AffilBll.GetDistrictListBLL(Convert.ToInt32(model.aff.division_id)).Select(a => new SelectListItem
                    {
                        Text = a.district,
                        Value = a.district_id.ToString()

                    }).ToList();
                }
                else
                {
                    model.dist_list = _AffilBll.GetDistrictsBLL().ToList();
                }

                model.taluk_list = _AffilBll.GetTalukBLL(Convert.ToInt32(model.aff.dist_id)).ToList();
                model.consti_list = _AffilBll.GetConstiteuncyBLL().ToList();
                model.pancha_list = _AffilBll.GetPanchayatBLL(Convert.ToInt32(model.aff.taluk_id)).ToList();
                model.village_list = _AffilBll.GetVillageBLL(Convert.ToInt32(model.aff.taluk_id)).ToList();
                model.css_code_list = _AffilBll.GetCssCodeBLL().ToList();
                model.his_list = _AffilBll.GetTradeHistoriesBLL(trade_id).OrderByDescending(a => a.CreatedOn).ToList();
                model.cou_list = _AffilBll.GetCourseListBLL().Select(a => new SelectListItem
                {
                    Text = a.course_name,
                    Value = a.course_id.ToString()

                }).ToList();
                model.schem_list = _AffilBll.GetAffiliationSchemesBLL();
                model.aff.flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
                model.aff.flag = 0;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Get College Details For Uploaded Institute
        public JsonResult GetUploadedCollegeDetails(int collegId)
        {
            AffiliationNested model = new AffiliationNested();
            model.his_list = new List<TradeHistory>();
            Session["TradeShift"] = new NestedTradeSession();

            try
            {

                model.aff = _AffilBll.GetAAffiliationUploadedCollegeDetailsBLL(collegId);
                if (model.aff.trades_list != null)
                {
                    NestedTradeSession session = (NestedTradeSession)Session["TradeShift"];
                    session.sessions = new List<TradeShiftSessions>();

                    int inc = 1; //due to radom generating same number
                    foreach (var item in model.aff.trades_list)
                    {
                        if (item.list.Count() > 0)
                        {

                            int sessionKey = GenerateRandomNo() + inc;
                            TradeShiftSessions addSession = new TradeShiftSessions();
                            addSession.list = new List<TradeShift>();
                            addSession.list = item.list;
                            addSession.sessionKey = sessionKey;

                            session.sessions.Add(addSession);

                            item.sessionKey = addSession.sessionKey;
                            inc++;
                        }
                        else
                        {
                            item.sessionKey = 0;
                        }
                    }
                }
                model.inst_list = _AffilBll.GetInstitutionTypesBLL().ToList();
                model.loca_type_list = _AffilBll.GetLocationTypesBLL().ToList();
                model.trades_list = _AffilBll.GetTradesBLL().Select(a => new SelectListItem
                {
                    Text = a.trade_code.ToString(),
                    Value = a.trade_id.ToString()
                }).ToList();
                if (model.aff.division_id != 0 && model.aff.division_id != null)
                {
                    model.dist_list = _AffilBll.GetDistrictListBLL(Convert.ToInt32(model.aff.division_id)).Select(a => new SelectListItem
                    {
                        Text = a.district,
                        Value = a.district_id.ToString()

                    }).ToList();
                }
                else
                {
                    model.dist_list = _AffilBll.GetDistrictsBLL().ToList();
                }

                model.taluk_list = _AffilBll.GetTalukBLL(Convert.ToInt32(model.aff.dist_id)).ToList();
                model.consti_list = _AffilBll.GetConstiteuncyBLL().ToList();
                model.pancha_list = _AffilBll.GetPanchayatBLL(Convert.ToInt32(model.aff.taluk_id)).ToList();
                model.village_list = _AffilBll.GetVillageBLL(Convert.ToInt32(model.aff.taluk_id)).ToList();
                model.css_code_list = _AffilBll.GetCssCodeBLL().ToList();
                model.his_list = _AffilBll.GetAllTradeHistoriesBLL(collegId).OrderByDescending(a => a.CreatedOn).ToList();
                model.cou_list = _AffilBll.GetCourseListBLL().Select(a => new SelectListItem
                {
                    Text = a.course_name,
                    Value = a.course_id.ToString()

                }).ToList();
                model.schem_list = _AffilBll.GetAffiliationSchemesBLL();
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(model, JsonRequestBehavior.AllowGet);
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
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Panchayat by Taluk
        public JsonResult GetPanchayat(int TalukId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                list = _AffilBll.GetPanchayatBLL(TalukId);
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Village by Panchayat
        public JsonResult GetVillage(int PanchaId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                list = _AffilBll.GetVillageBLL(PanchaId);
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save College Affiliations Update
        [HttpPost]
        public JsonResult UpdateCollegeAffilationDetails(AffiliationCollegeDetailsTest formCollection)
        {
            UploadAffiliation output = new UploadAffiliation();
            try
            {

                if (formCollection.date != null && formCollection.date != "--")
                {
                    formCollection.affiliation_date = Convert.ToDateTime(formCollection.date);
                }
                if (formCollection.order_no_date != null && formCollection.order_no_date != "--")
                {
                    formCollection.AffiliationOrderNoDate = Convert.ToDateTime(formCollection.order_no_date);
                }
                var UserId = Convert.ToInt32(Session["RoleId"]);
                formCollection.CreatedBy = UserId;

                //if (formCollection.list_trades != null && formCollection.list_units != null)
                //{
                //    if (formCollection.list_trades.Count() == formCollection.list_units.Count() && formCollection.list_trades.Count() == formCollection.list_it_trade_id.Count() && formCollection.list_trades.Count() == formCollection.list_keys.Count())
                //    {
                //        formCollection.trades_list = new List<AffiliationTrade>();
                //        // int f_i=0;
                //        for (var i = 0; i < formCollection.list_trades.Count(); i++)
                //        {
                //            var model = new AffiliationTrade();
                //            model.trade_id = formCollection.list_trades[i];
                //            model.units = formCollection.list_units[i];
                //            model.trade_iti_id = formCollection.list_it_trade_id[i];
                //            model.sessionKey = formCollection.list_keys[i];

                //            formCollection.trades_list.Add(model);
                //        }
                //    }
                //}
                if (Request.Files.Count > 0)
                {
                    if(formCollection.UploadTradeAffiliationDoc!=null)
                    {
                        string[] _fileformat = Request.Files[0].FileName.Split('.');
                        string _farmat = _fileformat.LastOrDefault();

                        var supportedTypes = new[] { "pdf" };
                        if (supportedTypes.Contains(_farmat))
                        {
                            string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;
                            Log.Info("UploadFolder::" + Convert.ToString(UploadFolder));
                            Log.Info("DocumentsFolder::" + Convert.ToString(DocumentsFolder));

                            Random rand = new Random();

                            string File_name = Request.Files[0].FileName;//"AffiliationUnitPdf" + rand.Next() + "." + _farmat;

                            HttpPostedFileBase file = Request.Files[0];
                            string fname;
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = File_name.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = File_name;
                            }
                            var uploadRootFolderInput = DocumentsFolder;
                            if (!Directory.Exists(uploadRootFolderInput))
                            {
                                Directory.CreateDirectory(uploadRootFolderInput);
                            }

                            var directoryFullPathInput = uploadRootFolderInput;
                            fname = Path.Combine(directoryFullPathInput, fname);
                            file.SaveAs(fname);
                            formCollection.UploadTradeAffiliationDoc = fname;

                            if (!IsPDFHeader(fname))
                            {
                                output.status = "Pls upload valid pdf file!";
                                output.flag = 0;

                                return Json(output, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            output.status = "Pls upload only pdf file formate";
                            output.flag = 0;

                            return Json(output, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        string[] _fileformat = Request.Files[0].FileName.Split('.');
                        string _farmat = _fileformat.LastOrDefault();

                        var supportedTypes = new[] { "pdf" };
                        if (supportedTypes.Contains(_farmat))
                        {
                            string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;
                            Log.Info("UploadFolder::" + Convert.ToString(UploadFolder));
                            Log.Info("DocumentsFolder::" + Convert.ToString(DocumentsFolder));

                            Random rand = new Random();

                            string File_name = Request.Files[0].FileName;//"AffiliationCollegePdf" + rand.Next() + "." + _farmat;

                            HttpPostedFileBase file = Request.Files[0];
                            string fname;
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = File_name.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = File_name;
                            }
                            var uploadRootFolderInput = DocumentsFolder;
                            if (!Directory.Exists(uploadRootFolderInput))
                            {
                                Directory.CreateDirectory(uploadRootFolderInput);
                            }

                            var directoryFullPathInput = uploadRootFolderInput;
                            fname = Path.Combine(directoryFullPathInput, fname);
                            file.SaveAs(fname);
                            formCollection.FileUploadPath = fname;

                            if (!IsPDFHeader(fname))
                            {
                                output.status = "Pls upload valid pdf file!";
                                output.flag = 0;

                                return Json(output, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            output.status = "Pls upload only pdf file formate";
                            output.flag = 0;

                            return Json(output, JsonRequestBehavior.AllowGet);
                        }
                    }
                   
                }
               

                formCollection.color_flag = 1;
                _AffilBll.UpdateAffiliationTradeDetailsBLL(formCollection);
                output.flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
                output.flag = 0;
                output.status = "Error occured!";
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Institutions Type
        [HttpGet]
        public ActionResult GetInstitutionType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                list = _AffilBll.GetInstitutionTypesBLL();
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Location Type
        [HttpGet]
        public ActionResult GetLocationType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                list = _AffilBll.GetLocationTypesBLL();
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Constituency
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
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Css
        [HttpGet]
        public ActionResult GetCss()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                list = _AffilBll.GetCssCodeBLL();
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save New College Affiliations 
        [HttpPost]

        public JsonResult SaveCollegeAffilationDetails(AffiliationCollegeDetails formCollection)
        {
            UploadAffiliation output = new UploadAffiliation();
            try
            {
                var UserId = Convert.ToInt32(Session["RoleId"]);
                formCollection.CreatedBy = UserId;

                //if (formCollection.date != null)
                if (formCollection.date.Trim() != string.Empty)
                {
                    formCollection.affiliation_date = Convert.ToDateTime(formCollection.date);
                }
                // if (formCollection.order_no_date != null)
                if (formCollection.order_no_date.Trim() != string.Empty)
                {
                    formCollection.AffiliationOrderNoDate = Convert.ToDateTime(formCollection.order_no_date);
                }


                if (formCollection.list_trades != null && formCollection.list_units != null)
                {
                    if (formCollection.list_trades.Count() == formCollection.list_units.Count() && formCollection.list_trades.Count() == formCollection.list_keys.Count())
                    {

                        formCollection.trades_list = new List<AffiliationTrade>();
                        // int f_i = 0;
                        for (var i = 0; i < formCollection.list_trades.Count(); i++)
                        {
                            var model = new AffiliationTrade();
                            model.trade_id = formCollection.list_trades[i];
                            model.units = formCollection.list_units[i];
                            model.sessionKey = formCollection.list_keys[i];

                            formCollection.trades_list.Add(model);
                        }

                        if (Request.Files.Count > 0)
                        {
                            string[] _fileformat = Request.Files[0].FileName.Split('.');
                            string _farmat = _fileformat.LastOrDefault();

                            var supportedTypes = new[] { "pdf" };
                            if (supportedTypes.Contains(_farmat))
                            {

                                string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

                                Random rand = new Random();

                                string File_name = "AffiliationCollegePdf" + rand.Next() + "." + _farmat;

                                HttpPostedFileBase file = Request.Files[0];
                                string fname;
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = File_name.Split(new char[] { '\\' });
                                    fname = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    fname = File_name;
                                }
                                var uploadRootFolderInput = DocumentsFolder;
                                if (!Directory.Exists(uploadRootFolderInput))
                                {
                                    Directory.CreateDirectory(uploadRootFolderInput);
                                }

                                var directoryFullPathInput = uploadRootFolderInput;
                                fname = Path.Combine(directoryFullPathInput, fname);
                                file.SaveAs(fname);
                                formCollection.FileUploadPath = fname;

                                if (!IsPDFHeader(fname))
                                {
                                    output.status = "Pls upload valid pdf file!";
                                    output.flag = 0;

                                    return Json(output, JsonRequestBehavior.AllowGet);
                                }

                            }
                            else
                            {
                                output.status = "Pls upload only pdf file formate";
                                output.flag = 0;

                                return Json(output, JsonRequestBehavior.AllowGet);
                            }
                        }

                        formCollection.color_flag = 1;
                        _AffilBll.AddAffiliationCollegeDetailsBLL(formCollection);
                        output.flag = 1;
                    }
                    else
                    {
                        output.status = "failed";
                        output.flag = 0;
                    }
                }



            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get College Details New

        public JsonResult GetCollegeDetailsNew(int collegeId)
        {
            AffiliationCollegeDetails model = new AffiliationCollegeDetails();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                model = _AffilBll.GetAffiliationCollegeDetailsBLL(collegeId, role_id);
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Publish Affiliated Colleges
        [HttpPost]
        public ActionResult PublishAffiliatedColleges(int[] colleges)
        {
            UploadAffiliation output = new UploadAffiliation();
            try
            {
                foreach (var item in colleges)
                {
                    _AffilBll.PublishAffiliatedCollegesBLL(item);
                }

                output.flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
            }

            return Json(output);
        }
        #endregion

        #region Affiliation College Approver
        //GET: AffiliationCollegeApprover
        public ActionResult AffiliationCollegeApprover()
        {
            AffiliationNested model = new AffiliationNested();
            model.list = new List<AffiliationCollegeDetails>();
            model.pubs_list = new List<AffiliationCollegeDetails>();

            try
            {
                model.list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.Submitted).ToList();
                model.pubs_list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.Published).ToList();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
            }
            return View(model);
        }
        #endregion

        #region Approve Affiliated College
        [HttpPost]
        public JsonResult ApproveAffiliatedCollege(int[] collegeIds)
        {
            UploadAffiliation output = new UploadAffiliation();
            try
            {
                foreach (var item in collegeIds)
                {
                    _AffilBll.ApproveAffiliatedCollegeBLL(item);
                }

                output.flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
                output.status = ex.Message.ToString();
                output.flag = 0;
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reject Affiliated College
        public JsonResult RejectAffiliatedCollege(int[] collegeIds)
        {
            UploadAffiliation output = new UploadAffiliation();
            try
            {
                foreach (var item in collegeIds)
                {
                    _AffilBll.RejectAffiliatedCollegeBLL(item);
                }

                output.flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
                output.status = ex.Message.ToString();
                output.flag = 0;
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Affiliation PDF File Upload
        public string AffiliationFileUpload(HttpPostedFile File)
        {
            string[] _fileformat = File.FileName.Split('.');
            string _farmat = _fileformat.LastOrDefault();

            var supportedTypes = new[] { "pdf" };
            if (supportedTypes.Contains(_farmat))
            {
                string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

                Random rand = new Random();

                string File_name = "AffiliationCollegePdf" + rand.Next() + "." + _farmat;

                HttpPostedFileBase file = Request.Files[0];
                string fname;
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = File_name.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = File_name;
                }
                var uploadRootFolderInput = DocumentsFolder;
                if (!Directory.Exists(uploadRootFolderInput))
                {
                    Directory.CreateDirectory(uploadRootFolderInput);
                }

                var directoryFullPathInput = uploadRootFolderInput;
                fname = Path.Combine(directoryFullPathInput, fname);
                file.SaveAs(fname);

                return fname;
            }
            else
            {
                return "Formate not supported";
            }
        }
        #endregion

        #region Affiliate Colleges
        public ActionResult AffiliateCollege()
        {
            try
            {
                AffiliationNested model = new AffiliationNested();
                model.list = new List<AffiliationCollegeDetails>();
                model.pubs_list = new List<AffiliationCollegeDetails>();

                var role_id = Convert.ToInt32(Session["RoleId"]);
                if (Session["CollegeId"] != null)
                {
                    var college_id = (int)Session["CollegeId"];
                    model.list = _AffilBll.GetAllMyAffiliatedCollegesBLL(college_id);
                }
                else
                {
                    model.list = _AffilBll.GetAllAffiliateCollegeListDLL(role_id);
                    model.pubs_list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.Published);
                }


                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get All Status
        public JsonResult GetAllStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<SelectListItem> statuses = new List<SelectListItem>();
            int _flag = 0;
            string _status = "";
            try
            {
                list = _AffilBll.GetAllStatusBLL();
                var role_id = Convert.ToInt32(Session["RoleId"]);
                if (list.Count() > 0)
                {
                    if (role_id == (int)CsystemType.getCommon.ITIAdmin)
                    {
                        var _rev = (int)CsystemType.getCommon.Sent_for_Correction;
                        var SendCorrec = list.Find(a => a.Value == _rev.ToString());

                        statuses.Add(SendCorrec);

                        _flag = 1;
                    }
                    else
                    if (role_id == (int)CsystemType.getCommon.OFFICE_SUPERINTENDENT|| role_id == (int)CsystemType.getCommon.CaseWorker
                        || role_id == (int)CsystemType.getCommon.Office_Supeintendent_Div || role_id == (int)CsystemType.getCommon.CaseWorkerDiv)
                    {
                        int send_bck = (int)CsystemType.getCommon.Sent_back;
                        int revrecom = (int)CsystemType.getCommon.Review_and_Recommend;

                        var send_back = list.Find(a => a.Value == send_bck.ToString());
                        var reviewRecom = list.Find(a => a.Value == revrecom.ToString());

                        statuses.Add(send_back);
                        statuses.Add(reviewRecom);

                        _flag = 1;
                    }
                    else if (role_id == (int)CsystemType.getCommon.Additional_Director)
                    {
                        int _approve = (int)CsystemType.getCommon.Approved;
                        // int _reject = (int)CsystemType.getCommon.Rejected;
                        int send_bck = (int)CsystemType.getCommon.Sent_back;



                        var send_back = list.Find(a => a.Value == send_bck.ToString());
                        var Approve = list.Find(a => a.Value == _approve.ToString());
                        // var Reject = list.Find(a => a.Value == _reject.ToString());

                        statuses.Add(send_back);
                        statuses.Add(Approve);
                        // statuses.Add(Reject);

                        _flag = 1;
                    }
                    else if (role_id == (int)CsystemType.getCommon.Deputy_Director|| role_id == (int)CsystemType.getCommon.Deputy_Director_Div)
                    {
                        int _approve = (int)CsystemType.getCommon.Approved;
                        // int _reject = (int)CsystemType.getCommon.Rejected;
                        int send_bck = (int)CsystemType.getCommon.Sent_back;



                        var send_back = list.Find(a => a.Value == send_bck.ToString());
                        var Approve = list.Find(a => a.Value == _approve.ToString());
                        //var Reject = list.Find(a => a.Value == _reject.ToString());

                        statuses.Add(send_back);
                        statuses.Add(Approve);
                        //statuses.Add(Reject);

                        _flag = 1;
                    }
                    else
                    {
                        int _approve = (int)CsystemType.getCommon.Approved;
                        //int _reject = (int)CsystemType.getCommon.Rejected;
                        int _send_bck = (int)CsystemType.getCommon.Sent_back;
                        int _revrecom = (int)CsystemType.getCommon.Review_and_Recommend;

                        var Approve = list.Find(a => a.Value == _approve.ToString());
                        //var Reject = list.Find(a => a.Value == _reject.ToString());
                        var SendBack = list.Find(a => a.Value == _send_bck.ToString());
                        var RevRecom = list.Find(a => a.Value == _revrecom.ToString());

                        statuses.Add(Approve);
                        //statuses.Add(Reject);
                        statuses.Add(SendBack);
                        statuses.Add(RevRecom);

                        _flag = 1;

                    }


                }
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(new { list = statuses, flag = _flag, status = _status }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Users
        public JsonResult GetAllUsers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                list = _AffilBll.GetAllUsersBLL();
                int os = (int)CsystemType.getCommon.OFFICE_SUPERINTENDENT;
                int ad = (int)CsystemType.getCommon.Assistant_Director;
                int dd = (int)CsystemType.getCommon.Deputy_Director;

                var role_1 = list.Find(a => a.Value == os.ToString());
                var role_2 = list.Find(a => a.Value == ad.ToString());
                var role_3 = list.Find(a => a.Value == dd.ToString());

                list = new List<SelectListItem>();
                list.Add(role_1);
                list.Add(role_2);
                list.Add(role_3);

                if (list.Count() > 0)
                {
                    int removeIndex = list.FindIndex(a => a.Value == role_id.ToString());
                    list.RemoveRange(0, removeIndex + 1);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add Trade Transaction
        [HttpPost]
        public JsonResult AddTradeTransaction(AffiliationTrade aTrade)
        {
            UploadAffiliation output = new UploadAffiliation();
            try
            {
                int role_id = Convert.ToInt32(Session["RoleId"]);
                aTrade.CreatedBy = role_id;
                if (aTrade.date != null)
                {
                    aTrade.affiliation_date = Convert.ToDateTime(aTrade.date);
                }
                if (aTrade.order_no_date != null)
                {
                    aTrade.AffiliationOrderNoDate = Convert.ToDateTime(aTrade.order_no_date);
                }
                if (Request.Files.Count > 0)
                {
                    string[] _fileformat = Request.Files[0].FileName.Split('.');
                    string _farmat = _fileformat.LastOrDefault();

                    var supportedTypes = new[] { "pdf" };
                    if (supportedTypes.Contains(_farmat))
                    {
                        string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;
                        Log.Info("UploadFolder::" + Convert.ToString(UploadFolder));
                        Log.Info("DocumentsFolder::" + Convert.ToString(DocumentsFolder));

                        Random rand = new Random();

                        string File_name = Request.Files[0].FileName;//"AffiliationCollegePdf" + rand.Next() + "." + _farmat;

                        HttpPostedFileBase file = Request.Files[0];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = File_name.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = File_name;
                        }
                        var uploadRootFolderInput = DocumentsFolder;
                        if (!Directory.Exists(uploadRootFolderInput))
                        {
                            Directory.CreateDirectory(uploadRootFolderInput);
                        }

                        var directoryFullPathInput = uploadRootFolderInput;
                        fname = Path.Combine(directoryFullPathInput, fname);
                        file.SaveAs(fname);
                        aTrade.file_upload_path = fname;

                        if (!IsPDFHeader(fname))
                        {
                            output.status = "Pls upload valid pdf file!";
                            output.flag = 0;

                            return Json(output, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                    _AffilBll.AddTradeTransactionBLL(aTrade);
                output.flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
                output.flag = 0;
            }
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Affiliated colleges by role
        public JsonResult GetAffiliatedCollegesByRole()
        {
            List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                if (role_id == (int)CsystemType.getCommon.ITIAdmin)
                {
                    int UserId = (int)Session["UserId"];
                    int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
                    list = _AffilBll.GetAllMyAffiliatedCollegesBLL(college_id);
                }
                else
                {//Commented code to show  the data same as on page load instead of role wise
                    //list = _AffilBll.GetAllAffiliateCollegeListDLL(role_id).OrderByDescending(a => a.CreatedOn).ToList();
                    list = _AffilBll.GetAllAffiliateCollegeListDLL1().OrderByDescending(a => a.CreatedOn).ToList();
                }

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Show All Affiliated Colleges
        public ActionResult ShowAffiliatedColleges()
        {
            List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            try
            {
                //list = _AffilBll.GetAllAffiliateCollegeListDLL
            }
            catch (Exception ex)
            {
                Log.Error("Enterted Exception: " + ex.Message.ToString());
            }

            return View();
        }
        #endregion

        #region Get Trade Details
        public JsonResult GetTradeDetails(int trade_id)
        {
            AffiliationCollegeDetails model = new AffiliationCollegeDetails();
            try
            {
                var rol_id = Convert.ToInt32(Session["RoleId"]);

                model = _AffilBll.GetATradeDetailsBLL(trade_id, rol_id);
                if (rol_id == (int)CsystemType.getCommon.ITIAdmin)
                {
                    if (model.status_id == (int)CsystemType.getCommon.pub)
                    {
                        model.en_edit = true; // for institute login
                    }

                }
                model.flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message);
                model.flag = 0;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Trade History
        public JsonResult GetTradeHistory(int trade_id)
        {
            int staus_flag = 0;
            List<TradeHistory> aff_list = new List<TradeHistory>();
            try
            {
                aff_list = _AffilBll.GetTradeHistoriesBLL(trade_id).OrderByDescending(a => a.CreatedOn).ToList();
                staus_flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message);
            }

            return Json(new { flag = staus_flag, list = aff_list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Trade Shifts Session
        [HttpPost]
        public JsonResult TradeShiftSession(TradeShiftSessions aTrade)
        {
            Random rand = new Random();
            int sessionKey = rand.Next();
            int _flag = 0;
            string _status = "";
            try
            {

                NestedTradeSession session = (NestedTradeSession)Session["TradeShift"];
                if (!session.sessions.Any(a => a.sessionKey == aTrade.sessionKey))
                {
                    TradeShiftSessions addSession = new TradeShiftSessions();
                    addSession.list = aTrade.list;
                    addSession.sessionKey = sessionKey;
                    session.sessions.Add(addSession);
                    aTrade.sessionKey = sessionKey;
                }
                else
                {
                    TradeShiftSessions UpdateSession = session.sessions.Where(a => a.sessionKey == aTrade.sessionKey).FirstOrDefault();
                    UpdateSession.list = aTrade.list;
                }

                _flag = 1;
                _status = "success";
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
                _status = ex.Message.ToString();
            }

            return Json(new { falg = _flag, status = _status, sessionkey = aTrade.sessionKey });
        }
        #endregion

        #region Get Trade Shift Session
        public JsonResult GetTradeShiftSession(int sessionKey)
        {
            int _flag = 0;
            string _status = "";
            TradeShiftSessions sessionData = new TradeShiftSessions();
            try
            {
                NestedTradeSession session = (NestedTradeSession)Session["TradeShift"];
                sessionData = session.sessions.Find(a => a.sessionKey == sessionKey);
                if (sessionData == null)
                {
                    sessionData = new TradeShiftSessions();
                    sessionData.list = new List<TradeShift>();
                }
                _flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Exception Entered:" + ex.Message.ToString());
                _flag = 0;
                _status = ex.Message.ToString();
            }
            return Json(new { session = sessionData, flag = _flag, status = _status }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Uploaded Affiliation Institutes
        public JsonResult GetUploadedAffiliationInstitutes()
        {
            List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();

            try
            {
                list = _AffilBll.GetAllUploadedAffiliationBLL().OrderByDescending(a => a.CreatedOn).ToList(); ;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetAffiliationTradeCode
        public JsonResult GetAffiliationTradeCode(int trade_id)
        {
            Trade tradeCode = new Trade();
            try
            {
                tradeCode = _AffilBll.GetAffiliationTradeCodeBLL(trade_id);

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
            }

            return Json(tradeCode, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save Uploaded Institute Details
        [HttpPost]
        public JsonResult SaveUploadedAffiliationCollegeDetails(AffiliationCollegeDetailsTest formCollection)
        {
            UploadAffiliation output = new UploadAffiliation();
            try
            {
                var UserId = Convert.ToInt32(Session["RoleId"]);
                formCollection.CreatedBy = UserId;
                if (formCollection.date != null)
                {
                    formCollection.affiliation_date = Convert.ToDateTime(formCollection.date);
                }
                if (formCollection.order_no_date != null)
                {
                    formCollection.AffiliationOrderNoDate = Convert.ToDateTime(formCollection.order_no_date);
                }
                //if (formCollection.list_trades != null && formCollection.list_units != null)
                //{
                //    if (formCollection.list_trades.Count() == formCollection.list_units.Count() && formCollection.list_trades.Count() == formCollection.list_keys.Count())
                //    {

                //        formCollection.trades_list = new List<AffiliationTrade>();
                //        // int f_i = 0;
                //        for (var i = 0; i < formCollection.list_trades.Count(); i++)
                //        {
                //            var model = new AffiliationTrade();
                //            model.trade_id = formCollection.list_trades[i];
                //            model.units = formCollection.list_units[i];
                //            model.sessionKey = formCollection.list_keys[i];

                //            formCollection.trades_list.Add(model);
                //        }

                if (Request.Files.Count > 0)
                {
                    string[] _fileformat = Request.Files[0].FileName.Split('.');
                    string _farmat = _fileformat.LastOrDefault();

                    var supportedTypes = new[] { "pdf" };
                    if (supportedTypes.Contains(_farmat))
                    {
                        string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

                        Random rand = new Random();

                        string File_name = Request.Files[0].FileName; //++;//"AffiliationCollegePdf" + rand.Next() + "." + _farmat;

                        HttpPostedFileBase file = Request.Files[0];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = File_name.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = File_name;
                        }
                        var uploadRootFolderInput = DocumentsFolder;
                        if (!Directory.Exists(uploadRootFolderInput))
                        {
                            Directory.CreateDirectory(uploadRootFolderInput);
                        }

                        var directoryFullPathInput = uploadRootFolderInput;
                        fname = Path.Combine(directoryFullPathInput, fname);
                        file.SaveAs(fname);
                        formCollection.FileUploadPath = fname;

                    }
                    else
                    {
                        output.status = "Pls upload only pdf file formate";
                        output.flag = 0;
                        return Json(output, JsonRequestBehavior.AllowGet);

                    }
                }

                int college_id_temp = formCollection.iti_college_id;

                _AffilBll.SaveUploadedAffiliationTradeDetailsBLL(formCollection);
                //delete uploaded affiliation institute after saved in main tables

                output.flag = 1;
                //    }
                //    else
                //    {
                //        output.status = "failed";
                //        output.flag = 0;
                //    }
                //}


            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetAffiliatedInstituteMISCode
        public JsonResult GetAffiliatedInstituteMISCode(string parm, int? page)
        {
            MisCodes misCodes = new MisCodes();
            int paging = Convert.ToInt32(page);
            try
            {
                misCodes = _AffilBll.FetchAffiliatedInstituteMISCodesBLL(parm, paging);
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
            }

            Session["MISKey"] = parm;
            return Json(misCodes, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get A Affiliated Institute
        public JsonResult GetAAffiliatedInstitute(int CollegeId)
        {
            AffiliationCollegeDetails model = new AffiliationCollegeDetails();
            try
            {
                model = _AffilBll.GetAffiliatedInstituteDetailsBLL(CollegeId);
                model.flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Exception Entered: " + ex.Message.ToString());
                model.flag = 0;
                model.status = ex.Message.ToString();
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save New Affiliated Trade
        [HttpPost]
        public JsonResult SaveNewAffiliatedTrade(AffiliationCollegeDetailsTest formCollection)
        {
            UploadAffiliation output = new UploadAffiliation();
            try
            {
                if (formCollection.list_trades != null && formCollection.list_units != null)
                {
                    if (formCollection.list_trades.Count() == formCollection.list_units.Count() && formCollection.list_trades.Count() == formCollection.list_keys.Count())
                    {

                        formCollection.trades_list = new List<AffiliationTrade>();
                        // int f_i = 0;
                        for (var i = 0; i < formCollection.list_trades.Count(); i++)
                        {
                            var model = new AffiliationTrade();
                            model.trade_id = formCollection.list_trades[i];
                            model.units = formCollection.list_units[i];
                            model.sessionKey = formCollection.list_keys[i];
                            model.type = formCollection.list_type[i];
                            formCollection.trades_list.Add(model);
                        }

                        if (Request.Files.Count > 0)
                        {
                            string[] _fileformat = Request.Files[0].FileName.Split('.');
                            string _farmat = _fileformat.LastOrDefault();

                            var supportedTypes = new[] { "pdf" };
                            if (supportedTypes.Contains(_farmat))
                            {
                                string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

                                Random rand = new Random();

                                string File_name = Request.Files[0].FileName;//"AffiliationCollegePdf" + rand.Next() + "." + _farmat;

                                HttpPostedFileBase file = Request.Files[0];
                                string fname;
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = File_name.Split(new char[] { '\\' });
                                    fname = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    fname = File_name;
                                }
                                var uploadRootFolderInput = DocumentsFolder;
                                if (!Directory.Exists(uploadRootFolderInput))
                                {
                                    Directory.CreateDirectory(uploadRootFolderInput);
                                }

                                var directoryFullPathInput = uploadRootFolderInput;
                                fname = Path.Combine(directoryFullPathInput, fname);
                                file.SaveAs(fname);
                                formCollection.UploadTradeAffiliationDoc = fname;

                                if (!IsPDFHeader(fname))
                                {
                                    output.status = "Pls upload valid pdf file!";
                                    output.flag = 0;

                                    return Json(output, JsonRequestBehavior.AllowGet);
                                }

                            }
                            else
                            {
                                output.status = "Pls upload only pdf file formate";
                                output.flag = 0;
                            }
                        }

                        formCollection.color_flag = 1;
                        _AffilBll.AddNewAffiliatedInstituteTradeBLL(formCollection);
                        output.flag = 1;
                    }
                    else
                    {
                        output.status = "failed";
                        output.flag = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception Entered: " + ex.Message.ToString());
            }

            return Json(output, JsonRequestBehavior.AllowGet);
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
                Log.Error("Exception Entered: " + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Affiliated Institute By District
        public JsonResult GetAllAffiliatedInstituteByDistrict(int dist_id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                list = _AffilBll.GetAllAffiliatedInstituteByDistrictBLL(dist_id);
            }
            catch (Exception ex)
            {
                Log.Error("Exception Entered: " + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get College Details For Uploaded Institute
        public JsonResult GetUploadedTradeDetails(int iti_trade_id)
        {
            AffiliationNested model = new AffiliationNested();
            model.his_list = new List<TradeHistory>();

            try
            {

                model.aff = _AffilBll.GetAAffiliationUploadedTradeDetailsBLL(iti_trade_id);
                model.aff.flag = 1;
                model.inst_list = _AffilBll.GetInstitutionTypesBLL().ToList();
                model.loca_type_list = _AffilBll.GetLocationTypesBLL().ToList();
                model.trades_list = _AffilBll.GetTradesBLL().Select(a => new SelectListItem
                {
                    Text = a.trade_code.ToString(),
                    Value = a.trade_id.ToString()
                }).ToList();
                model.div_list = _AffilBll.GetDivisionListBLL().Select(a => new SelectListItem
                {
                    Text = a.division_name,
                    Value = a.division_id.ToString()

                }).ToList();
                if (model.aff.division_id != 0 && model.aff.division_id != null)
                {
                    model.dist_list = _AffilBll.GetDistrictListBLL(Convert.ToInt32(model.aff.division_id)).Select(a => new SelectListItem
                    {
                        Text = a.district,
                        Value = a.district_id.ToString()

                    }).ToList();
                }
                else
                {
                    model.dist_list = _AffilBll.GetDistrictsBLL().ToList();
                }

                model.taluk_list = _AffilBll.GetTalukBLL(Convert.ToInt32(model.aff.dist_id)).ToList();
                model.consti_list = _AffilBll.GetConstiteuncyBLL().ToList();
                model.pancha_list = _AffilBll.GetPanchayatBLL(Convert.ToInt32(model.aff.taluk_id)).ToList();
                model.village_list = _AffilBll.GetVillageBLL(Convert.ToInt32(model.aff.taluk_id)).ToList();
                model.css_code_list = _AffilBll.GetCssCodeBLL().ToList();

                model.cou_list = _AffilBll.GetCourseListBLL().Select(a => new SelectListItem
                {
                    Text = a.course_name,
                    Value = a.course_id.ToString()

                }).ToList();
                model.schem_list = _AffilBll.GetAffiliationSchemesBLL();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
                model.aff.flag = 0;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Is Affiliated Trade Already Exists
        public JsonResult IsAffiliatedTradeAlreadyExists(int trade_code, int college_id)
        {
            bool isExists = new bool();
            try
            {
                isExists = _AffilBll.IsAffiliatedTradeExistsBLL(trade_code, college_id);
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
            }

            return Json(isExists, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Get All Upload Affiliation Institutes

        public List<AffiliationCollegeDetails> GetUploadAffiliationInstitutesList(string uploadedPath)
        {
            List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            try
            {
                DataTable dt = new DataTable();
                if (Path.GetExtension(uploadedPath) == ".xls" || Path.GetExtension(uploadedPath) == ".xlsx")
                {
                    dt = ConvertXSLXtoDataTable(uploadedPath);
                }
                else
                if (Path.GetExtension(uploadedPath) == ".csv")
                {
                    dt = GetDataTableFromCsv(uploadedPath);
                }

                if (dt != null)
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        var item = row.ItemArray;                        
                        try
                        {
                            AffiliationCollegeDetails model = new AffiliationCollegeDetails();
                            model.name_of_iti = item[1].ToString();
                            model.mis_code = item[0].ToString();
                            model.trade = item[14].ToString();
                            if (item[22].ToString() != "")
                            {
                                model.NoofTrades = Convert.ToInt32(item[22]);
                            }
                            else
                            {

                                model.NoofTrades = null;
                                //model.NoofTrades = 0;
                            }
                            //model.NoofTrades = Convert.ToInt32(item[22]);                      
                            model.no_units = Convert.ToInt32(item[19]);
                            model.district = item[4].ToString();
                            model.taluka = item[3].ToString();
                            model.date = DateTime.Now.ToString("dd/MM/yyyy");

                            list.Add(model);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Data Row Exception: (" + item[0].ToString() + ")" + ex.Message.ToString());
                            //log exception
                        }

                    }

                    ////list = list.GroupBy(a => new { a.mis_code, a.trade }).Select(z => new AffiliationCollegeDetails

                    //list = list.GroupBy(a => new { a.mis_code, a.NoofTrades, a.trade, a.no_units,a.no_shifts }).Select(z => new AffiliationCollegeDetails                    
                    //{

                    //    mis_code = z.Key.mis_code,
                    //    trade = z.Key.trade,
                    //    NoofTrades = z.Key.NoofTrades,
                    //    //nooftrades = z.Count().ToString(),
                    //    name_of_iti = z.Select(x => x.name_of_iti).FirstOrDefault(),
                    //    no_units = z.Key.no_units,
                    //    //units = z.Count().ToString(),
                    //    district = z.Select(x => x.district).FirstOrDefault(),
                    //    taluka = z.Select(x => x.taluka).FirstOrDefault(),
                    //    date = z.Select(x => x.date).FirstOrDefault()
                    //}).OrderBy(x => x.mis_code).ToList();
                    
                    
                    //string msicode1 = "";
                    //foreach (var list1 in list)
                    //{
                    //    string msicode = list1.mis_code;
                    //    if (msicode == msicode1)
                    //    {

                    //        //list1.Slno = "";
                    //        list1.mis_code = "";
                    //        list1.name_of_iti = "";
                    //        list1.district = "";
                    //        list1.taluka = "";
                    //        list1.date = "";
                    //    }
                    //    msicode1 = list1.mis_code;
                    //}

                    return list;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return list;
        }


        #endregion

        #region ValidateAffiliationUploadExcelFile
        public string ValidateAffiliationUploadExcelFile(string path)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                List<string> ColumnsList = new List<string>();

                if (System.IO.Path.GetExtension(path) == ".xls" || System.IO.Path.GetExtension(path) == ".xlsx")
                {
                    dt = ConvertXSLXtoDataTable(path);
                }
                else if (System.IO.Path.GetExtension(path) == ".csv")
                {
                    dt = GetDataTableFromCsv(path);
                }
                else
                {
                    return "Invalid file format!";
                }

                ColumnsList = GetExcelFormateHeaders();

                if (dt != null)
                {
                    int index = 0;
                    bool IsValid = true;
                    bool IsFieldLenght = true;
                    foreach (DataColumn dataColumn in dt.Columns)
                    {
                        string columnName = dataColumn.ColumnName;
                        if (ColumnsList.Count() > index)
                        {
                            if (ColumnsList[index] != columnName)
                            {
                                IsValid = false;
                            }
                            index++;
                        }
                        else
                        {
                            IsValid = false;
                        }

                    }
                    bool isData = true;
                    if (IsValid)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var _item = row.ItemArray;
                            string miscode = _item[0].ToString();
                            string name = _item[1].ToString();
                            string address = _item[2].ToString();
                            string trade = _item[14].ToString();
                            string unit = _item[19].ToString();
                            string shifts = _item[20].ToString();
                            string nooftrades = _item[22].ToString();

                            if (miscode == "" || name == "" || address == "" || trade == "" || unit == "" || shifts == "")
                            {
                                isData = false;
                            }
                            if(miscode.Any(ch => !Char.IsLetterOrDigit(ch)) || miscode.Length!=10)
                            {
                                string str =" Contains Special Characters";
                                if(miscode.Length != 10)
                                {
                                    str = " should be of 10 characters only";
                                }
                                return "<br><br>MisCode : <b>" + miscode + "</b>" + str + ". Please enter only characters and numeric value";
                            }
                            if (isData)
                            {
                                var fdfd = _item[1].ToString();
                                if (_item[2].ToString().Length > 250)
                                {
                                    IsFieldLenght = false;
                                }
                                if (_item[1].ToString().Length > 100)
                                {
                                    IsFieldLenght = false;
                                }
                                if (_item[10].ToString().Length > 200)
                                {
                                    IsFieldLenght = false;
                                }
                                if (_item[30].ToString().Length > 50)
                                {
                                    IsFieldLenght = false;
                                }
                                if (_item[28].ToString().Length > 25)
                                {
                                    IsFieldLenght = false;
                                }
                                if (_item[0].ToString().Length > 50)
                                {
                                    IsFieldLenght = false;
                                }
                                if (_item[24].ToString().Length > 200)
                                {
                                    IsFieldLenght = false;
                                }
                                if (_item[29].ToString().Length > 200)
                                {
                                    IsFieldLenght = false;
                                }
                            }
                            else
                            {
                                return "Some required fields are empty! \n ex: name, miscode, address, trade, unit and shifts";
                            }

                        }
                    }


                    if (!IsValid)
                    {
                        return "Uploaded file is in a invalid template format, please upload Using valid template !";
                    }
                    else
                    {
                        if (IsFieldLenght)
                        {
                            return "valid";
                        }
                        else
                        {
                            return "Uploaded File data has Invalid Field Lenght!";
                        }

                    }
                }
                else
                {
                    return "Your Uploaded File Contains No Data!";
                }

            }
            catch (Exception ex)
            {
                Log.Error("Exception Entered: " + ex.Message.ToString());

                return ex.Message.ToString();
            }


        }
        #endregion

        #region Get Affiliated Institute Upload Excel 
        public List<string> GetExcelFormateHeaders()
        {
            List<string> ColumnsList = new List<string>();

            Workbook workbook = new Workbook();
            workbook.LoadFromFile(System.Web.Hosting.HostingEnvironment.MapPath(ExcelMasterDataTempleUploadFolder));
            Worksheet sheet = workbook.Worksheets[0];
            DataTable dt = sheet.ExportDataTable();

            foreach (DataColumn dataColumn in dt.Columns)
            {
                string columnName = dataColumn.ColumnName;
                ColumnsList.Add(columnName);
            }

            return ColumnsList;
        }
        #endregion

        #region Get DataTable From Csv
        public static DataTable GetDataTableFromCsv(string path)
        {
            //string header = isFirstRowHeader ? "Yes" : "No";
            string header = "Yes";
            string pathOnly = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            string sql = @"SELECT * FROM [" + fileName + "]";

            using (OleDbConnection connection = new OleDbConnection(
                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                      ";Extended Properties=\"Text;HDR=" + header + "\""))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.CurrentCulture;
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
        #endregion

        #region Validate PDF
        public bool IsPDFHeader(string fileName)
        {
            byte[] buffer = null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            //buffer = br.ReadBytes((int)numBytes);
            buffer = br.ReadBytes(5);

            var enc = new ASCIIEncoding();
            var header = enc.GetString(buffer);

            //%PDF−1.0
            // If you are loading it into a long, this is (0x04034b50).
            if (buffer[0] == 0x25 && buffer[1] == 0x50
                && buffer[2] == 0x44 && buffer[3] == 0x46)
            {
                return header.StartsWith("%PDF-");
            }
            return false;

        }
        #endregion

        #region If MisCode Exists
        public JsonResult IsMisCodeExists(string miscode)
        {
            bool exists = new bool();
            try
            {
                exists = _AffilBll.IsMisCodeExistsBLL(miscode);
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
            }

            return Json(exists, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region If ITI College Name Exists
        public JsonResult IsITIColllegeNameExists(string iticollegename)
        {
            bool exists = new bool();
            try
            {
                exists = _AffilBll.IsITICollegeNameExistsBLL(iticollegename);
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message.ToString());
            }

            return Json(exists, JsonRequestBehavior.AllowGet);
        }
        #endregion

        private string GenerateId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public ActionResult ActiveandDeactive()
        {
            AffiliationNested model = new AffiliationNested();

            model.list1 = new List<ActiveandDeactiveDeatils>();
            model.list1 = _AffilBll.GetAllActiveandDeactiveDeatilsBLL();

            return View(model);
        }

        public ActionResult DeAffiliateColleges()
        {
            return View();
        }

        public FileResult Download(string path)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string fileName = System.IO.Path.GetFileName(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        //public FileResult DownloadAffiliationDoc(int CollegeId)
        //{
        //    var data = _AffilBll.GetAAffiliationCollegeDetailsBLL(CollegeId);
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(data.FileUploadPath);
        //    string fileName = System.IO.Path.GetFileName(data.FileUploadPath);
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}

        //public FileResult DownloadAffiliationTradeDoc(int CollegeId)
        //{
        //    var data = _AffilBll.GetAAffiliationCollegeDetailsBLL(CollegeId);
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(data.UploadTradeAffiliationDoc);
        //    string fileName = System.IO.Path.GetFileName(data.UploadTradeAffiliationDoc);
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}

        public FileResult DownloadAffiliationDoc(int CollegeId = 0, int Trade_Id=0,int? shift_id=0, int? flag =0)
        {
            var data = _AffilBll.GetAllAffiliationDocForDownload(CollegeId, Trade_Id, shift_id, flag);
            byte[] fileBytes = System.IO.File.ReadAllBytes(data.FileName);
            string fileName = System.IO.Path.GetFileName(data.FileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        //Generate RandomNo
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        #region Get Units wise  Details
        public JsonResult GetTradeUnitswiseDetails(int ITI_Trade_ShiftId)
        {
            AffiliationCollegeDetails model = new AffiliationCollegeDetails();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                model = _AffilBll.GetATradeUnitwiseDetailsBLL(ITI_Trade_ShiftId, role_id);
                // model = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsAffBLL(ITI_Trade_ShiftId);
                model.activeDeactiveTradeHistories = _AffilBll.ActiveDeactiveGetTradeHistoriesUnitwiseBLL(ITI_Trade_ShiftId);

                model.flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message);
                model.flag = 0;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        public JsonResult TradeActiveandDeactiveStatusUnitwise(Tradeactivedeactiveuintwsie model)
        {
            int userId = Convert.ToInt32(Session["RoleId"]);
            model.userID = userId;
            bool status = false;
            //validation
            var supportedTypes = new[] { "pdf" };
            //This Code is to check whether file size exides more than 3 mb
            int maxcontentlength = 1024 * 3000;// 3MB
            if (model.ImageFile != null || model.ActiveImageFile != null)
            {
                //This code is to check valid file format (i.e.,PDF)



                string UniqueFileName = null;
                string[] string1;
                string[] _fileformat = Request.Files[0].FileName.Split('.');
                Random rand = new Random();
                string _farmat = _fileformat.LastOrDefault();

                if (model.ImageFile != null || model.ActiveImageFile != null)
                {
                    if (model.ImageFile != null)
                    {
                        string extension = System.IO.Path.GetExtension(model.ImageFile.FileName).Substring(1);
                        if (!supportedTypes.Contains(extension))
                        {
                            return Json("Please upload only pdf file");
                        }
                        if (model.ImageFile.ContentLength > maxcontentlength)
                        {
                            ModelState.AddModelError(string.Empty, "Size of pdf Upload file size exceeded max file upload size(3 MB) ");
                        }
                        string _FileName = Path.GetFileName(model.ImageFile.FileName);
                        extension = System.IO.Path.GetExtension(model.ImageFile.FileName).Substring(1);
                        UniqueFileName =  "Deactive_"+Request.Files[0].FileName;//"shiftupload" + rand.Next() + "." + _farmat;
                        string _pathCreate = Request.PhysicalApplicationPath + UploadFolder;
                        string _path = Path.Combine(_pathCreate, UniqueFileName);
                       
                        if (!Directory.Exists(_pathCreate))
                        {
                            Directory.CreateDirectory(_pathCreate);
                        }
                        model.ImageFile.SaveAs(_path);

                        model.FileName = model.ImageFile.FileName;
                        model.FilePath = _pathCreate + UniqueFileName;
                        //model.FilePath = _pathCreate + _FileName;//"Content/ADFiles/" + UniqueFileName;
                    }
                    else if (model.ActiveImageFile != null)
                    {
                        string extension = System.IO.Path.GetExtension(model.ActiveImageFile.FileName).Substring(1);
                        if (!supportedTypes.Contains(extension))
                        {
                            return Json("Please upload only pdf file");
                        }
                        if (model.ActiveImageFile.ContentLength > maxcontentlength)
                        {
                            ModelState.AddModelError(string.Empty, "Size of pdf Upload file size exceeded max file upload size(3 MB) ");
                        }
                        string _FileName = Path.GetFileName(model.ActiveImageFile.FileName);
                        extension = System.IO.Path.GetExtension(model.ActiveImageFile.FileName).Substring(1);
                        UniqueFileName = "Active_"+Request.Files[0].FileName ;//"shiftupload" + rand.Next() + "." + _farmat;
                        string _pathCreate = Request.PhysicalApplicationPath + UploadFolder;
                        string _path = Path.Combine(_pathCreate, UniqueFileName);
                        //UniqueFileName = model.ITI_Trade_Shift_Id + "_AD_" + Guid.NewGuid().ToString() + "." + extension;
                        //string _path = Path.Combine(Server.MapPath("Content/ADFiles/"), UniqueFileName);
                        //string _pathCreate = Request.PhysicalApplicationPath + UploadFolder;
                        if (!Directory.Exists(_pathCreate))
                        {
                            Directory.CreateDirectory(_pathCreate);
                        }
                        model.ActiveImageFile.SaveAs(_path);

                        model.ActivateFileName = model.ActiveImageFile.FileName;
                        //model.ActivateFilePath = "Content/ADFiles/" + UniqueFileName;
                        model.ActivateFilePath = _pathCreate + UniqueFileName;
                    }


                    // model.userID = userId;

                    status = _AffilBll.CreateActiveDeactiveUnitwiseBLL(model);
                }
            }

            else { status = _AffilBll.CreateActiveDeactiveUnitwiseBLL(model); }

            return Json(status);
        }

        public ActionResult ActiveDeactiveShift()
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    Log.Info("Entered ActiveDeactiveShift()");
                    _CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
                    AffiliationNested model = new AffiliationNested();
                    model.Cwunits_list = new List<ActiveandDeactiveUnitsDeatils>();
                    model.Cwunits_list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLL().OrderByDescending(a => a.CreatedonOrderby).ToList(); ;
                    model.Osunit_list = _AffilBll.GetAllActiveandDeactiveUnitwsieOSDeatilsBLL().OrderByDescending(a => a.CreatedonOrderby).ToList();
                    model.ADDDunit_list = _AffilBll.GetAllActiveandDeactiveUnitwiseADDDDeatilsBLL().OrderByDescending(a => a.CreatedonOrderby).ToList();
                    model.ViewUnitDetails = _AffilBll.GetAllActiveandDeactiveUnitwiseViewDeatilsBLL().OrderByDescending(a => a.createdon).ToList();
                    if ((int)Session["RoleId"] == (int)CsystemType.getCommon.ITIAdmin)
                    {
                        int UserId = (int)Session["UserId"];
                        int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
                        model.adTrade_list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLL().Where(a => a.iti_college_id == college_id).ToList();
                    }
                    Log.Info("Left ActiveDeactiveShift()");
                    return View(model);
                }
                catch (Exception ex)
                {
                    Log.Error("Entered Exception - ActiveDeactiveShift():" + ex.Message.ToString());
                    throw ex;
                }
            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult DeaffiliateInstitute()
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    Log.Info("Entered DeaffiliateInstitute()");
                    _CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
                    AffiliationNested model = new AffiliationNested();
                    model.Cwinst_list = new List<DeAffiliateInstitute>();
                    model.Cwinst_list = _AffilBll.GetAllDeaffiliateInstituteBLL().OrderByDescending(a => a.date).ToList();
                    //model.list2 = _AffilBll.GetAllActiveandDeactiveOSDeatilsBLL();
                    //model.list3 = _AffilBll.GetAllActiveandDeactiveADDDDeatilsBLL();
                    model.Osinst_list = _AffilBll.GetAllDeaffiliateInstituteOSBLL().OrderByDescending(a => a.createdon).ToList();
                    model.ADDDinst_list = _AffilBll.GetAllDeaffiliateInstituteADDDBLL().OrderByDescending(a => a.createdon).ToList();
                    model.Approvedinst_list = _AffilBll.GetAllDeaffiliateInstituteApprovedRejectedBLL().OrderByDescending(a => a.createdon).ToList();
                    if ((int)Session["RoleId"] == (int)CsystemType.getCommon.ITIAdmin)
                    {
                        int UserId = (int)Session["UserId"];
                        int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
                        model.aff_list = _AffilBll.GetAllDeaffiliateInstituteAffBLL(college_id);
                    }
                    ////List<ActiveandDeactiveDeatils> model = new ActiveandDeactiveDeatils();
                    //List<ActiveandDeactiveDeatils> model = _AffilBll.GetAllActiveandDeactiveDeatilsBLL();
                    return View(model);
                }
                catch (Exception ex)
                {
                    Log.Error("Entered Exception - DeaffiliateInstitute():" + ex.Message.ToString());
                    throw ex;
                }
            }
            return RedirectToAction("Index", "Home");
        }


        #region Get Institute Details New

        public JsonResult GetCollegeDetailsInstitute(int collegeId)
        {
            AffiliationCollegeDetails model = new AffiliationCollegeDetails();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                model = _AffilBll.GetAffiliationCollegeDetailsBLL(collegeId, role_id);
                model.activeDeactiveTradeHistories = _AffilBll.ActiveDeactiveGetTradeHistoriesInstBLL(collegeId);
                model.flag = 1;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Institute Deaffiliation details

        [HttpPost]
        public JsonResult InstituteDeaffiliateDeatils(TradeActiveandDeactiveStatus model)
        {
            int userId = Convert.ToInt32(Session["RoleId"]);
            model.userID = userId;
            bool status = false;
            //validation
            var supportedTypes = new[] { "pdf" };
            //This Code is to check whether file size exides more than 3 mb
            int maxcontentlength = 1024 * 3000;// 3MB
            if (model.ImageFile != null || model.ActiveImageFile != null)
            {
                //This code is to check valid file format (i.e.,PDF)


                if (model.ImageFile != null)
                {
                    string UniqueFileName = null;
                    string[] string1;
                    string extension = System.IO.Path.GetExtension(model.ImageFile.FileName).Substring(1);
                    if (!supportedTypes.Contains(extension))
                    {
                        return Json("Please upload only pdf file");
                    }
                    if (model.ImageFile.ContentLength > maxcontentlength)
                    {
                        ModelState.AddModelError(string.Empty, "Size of pdf Upload file size exceeded max file upload size(3 MB) ");
                    }
                    string _FileName = Path.GetFileName(model.ImageFile.FileName);
                    extension = System.IO.Path.GetExtension(model.ImageFile.FileName).Substring(1);
                    UniqueFileName = "DeAffiliate_"+Request.Files[0].FileName;//model.clgId + "_ADI_" + Guid.NewGuid().ToString() + "." + extension;
                    string _pathCreate = Request.PhysicalApplicationPath + UploadFolder;//Path.Combine(Server.MapPath("Content/ADFiles/"));
                    string _path = Path.Combine(_pathCreate, UniqueFileName);//Path.Combine(Server.MapPath("Content/ADFiles/"), UniqueFileName);
                    
                    if (!Directory.Exists(_pathCreate))
                    {
                        Directory.CreateDirectory(_pathCreate);
                    }
                    model.ImageFile.SaveAs(_path);

                    model.fileName = model.ImageFile.FileName;
                    model.filePath = _pathCreate + UniqueFileName;//"Content/ADFiles/" + UniqueFileName;

                    // model.userID = userId;

                    status = _AffilBll.InstituteDeaffiliateDeatilsBLL(model);
                }
                else if (model.ActiveImageFile != null)
                {
                    string UniqueFileName = null;
                    string[] string1;
                    string extension = System.IO.Path.GetExtension(model.ActiveImageFile.FileName).Substring(1);
                    if (!supportedTypes.Contains(extension))
                    {
                        return Json("Please upload only pdf file");
                    }
                    if (model.ActiveImageFile.ContentLength > maxcontentlength)
                    {
                        ModelState.AddModelError(string.Empty, "Size of pdf Upload file size exceeded max file upload size(3 MB) ");
                     }
                    string _FileName = Path.GetFileName(model.ActiveImageFile.FileName);
                    extension = System.IO.Path.GetExtension(model.ActiveImageFile.FileName).Substring(1);
                    UniqueFileName = "Affiliate_" + Request.Files[0].FileName;//model.clgId + "_ADI_" + Guid.NewGuid().ToString() + "." + extension;
                    string _pathCreate = Request.PhysicalApplicationPath + UploadFolder;//Path.Combine(Server.MapPath("Content/ADFiles/"));
                    string _path = Path.Combine(_pathCreate, UniqueFileName);//Path.Combine(Server.MapPath("Content/ADFiles/"), UniqueFileName);
                    
                    if (!Directory.Exists(_pathCreate))
                    {
                        Directory.CreateDirectory(_pathCreate);
                    }
                    model.ActiveImageFile.SaveAs(_path);

                    model.AffiliateFileName = model.ActiveImageFile.FileName;
                    model.AffiliateFilePath = _pathCreate + UniqueFileName;//"Content/ADFiles/" + UniqueFileName;

                    status = _AffilBll.InstituteDeaffiliateDeatilsBLL(model);
                }
            }

            else { status = _AffilBll.InstituteDeaffiliateDeatilsBLL(model); }

            return Json(status);
        }

        #endregion

        #region Get AffiliateInstitute Details
        public JsonResult GetAffiliateInstituteDetails()
        {
            List<DeAffiliateInstitute> list = new List<DeAffiliateInstitute>();

            try
            {
                list = _AffilBll.GetAllDeaffiliateInstituteBLL().OrderByDescending(a => a.date).ToList(); ;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Get Affiliate InstituteDetails Os
        public JsonResult GetAffiliateInstituteDetailsOs()
        {
            List<DeAffiliateInstitute> list = new List<DeAffiliateInstitute>();

            try
            {
                Log.Info("Entered GetAffiliateInstituteDetailsOs()");
                list = _AffilBll.GetAllDeaffiliateInstituteOSBLL().OrderByDescending(a => a.createdon).ToList();
                Log.Info("Exited GetAffiliateInstituteDetailsOs()");
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Affiliate InstituteDetails DD
        public JsonResult GetAffiliateInstituteDetailsDD()
        {
            List<DeAffiliateInstitute> list = new List<DeAffiliateInstitute>();

            try
            {
                list = _AffilBll.GetAllDeaffiliateInstituteADDDBLL().OrderByDescending(a => a.createdon).ToList(); ;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Get AffiliateInstitute Details
        public JsonResult GetActiveDeactiveDetails()
        {
            List<ActiveandDeactiveUnitsDeatils> list = new List<ActiveandDeactiveUnitsDeatils>();

            try
            {
                list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLL().OrderByDescending(a => a.CreatedonOrderby).ToList(); ;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get AffiliateInstitute Details
        public JsonResult GetActiveDeactiveDetailsOS()
        {
            List<ActiveandDeactiveUnitsDeatils> list = new List<ActiveandDeactiveUnitsDeatils>();

            try
            {
                list = _AffilBll.GetAllActiveandDeactiveUnitwsieOSDeatilsBLL().OrderByDescending(a => a.CreatedonOrderby).ToList(); ;
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get ActiveDeactiveDetails AD
        public JsonResult GetActiveDeactiveDetailsAD()
        {
            List<ActiveandDeactiveUnitsDeatils> list = new List<ActiveandDeactiveUnitsDeatils>();
            List<ActiveandDeactiveUnitsDeatils> Approvedlist = new List<ActiveandDeactiveUnitsDeatils>();
            try
            {
                list = _AffilBll.GetAllActiveandDeactiveUnitwiseADDDDeatilsBLL().OrderByDescending(a => a.CreatedonOrderby).ToList();
               //Approvedlist= _AffilBll.GetAllActiveandDeactiveUnitwiseViewDeatilsBLL().OrderByDescending(a => a.createdon).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
            // return Json(new { datalist=list,aplist=Approvedlist }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult GetActiveDeactiveDetailsAdmin()
        {
            List<ActiveandDeactiveUnitsDeatils> list = new List<ActiveandDeactiveUnitsDeatils>();

            try
            {
                int UserId = (int)Session["UserId"];
                int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
                list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLL().Where(a => a.iti_college_id == college_id).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        #region FilterSubmittedTradeDetails

        public JsonResult FilterSubmittedTradeDetails(int? courseId, int? divisionId, int? districtId)
        {
            List<ActiveandDeactiveUnitsDeatils> list = new List<ActiveandDeactiveUnitsDeatils>();
            //List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            List<AffiliationCollegeDetails> up_list = new List<AffiliationCollegeDetails>();
            try
            {
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);


                if (Course_Id != 0 && District_Id != 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilter().Where(a => a.Courseid == Course_Id && a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }

                else if (Course_Id != 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilter().Where(a => a.Courseid == Course_Id && a.District_id == District_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilter().Where(a => a.Courseid == Course_Id && a.Division_id == Division_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilter().Where(a => a.District_id == District_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilter().Where(a => a.Division_id == Division_Id).ToList();
                }
                else if (District_Id != 0 && Division_Id != 0 && Course_Id == 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilter().Where(a => a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }
                else if (Course_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilter().Where(a => a.Courseid == Course_Id).ToList();
                }

                //list = list.Union(up_list).OrderByDescending(a => a.CreatedOn).ThenBy(a => a.iti_college_id).ToList();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FilterSubmittedTradeDetailsForOS

        public JsonResult FilterSubmittedTradeDetailsForOS(int? courseId, int? divisionId, int? districtId)
        {
            List<ActiveandDeactiveUnitsDeatils> list = new List<ActiveandDeactiveUnitsDeatils>();
            //List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            List<AffiliationCollegeDetails> up_list = new List<AffiliationCollegeDetails>();
            try
            {
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);


                if (Course_Id != 0 && District_Id != 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForOs().Where(a => a.Courseid == Course_Id && a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }

                else if (Course_Id != 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForOs().Where(a => a.Courseid == Course_Id && a.District_id == District_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForOs().Where(a => a.Courseid == Course_Id && a.Division_id == Division_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForOs().Where(a => a.District_id == District_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForOs().Where(a => a.Division_id == Division_Id).ToList();
                }
                else if (District_Id != 0 && Division_Id != 0 && Course_Id == 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForOs().Where(a => a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }
                else if (District_Id == 0 && Division_Id == 0 && Course_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForOs().Where(a => a.Courseid == Course_Id).ToList();
                }

                //list = list.Union(up_list).OrderByDescending(a => a.CreatedOn).ThenBy(a => a.iti_college_id).ToList();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FilterSubmittedTradeDetailsForAdDD

        public JsonResult FilterSubmittedTradeDetailsForAdDD(int? courseId, int? divisionId, int? districtId)
        {
            List<ActiveandDeactiveUnitsDeatils> list = new List<ActiveandDeactiveUnitsDeatils>();
            //List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            List<AffiliationCollegeDetails> up_list = new List<AffiliationCollegeDetails>();
            try
            {
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);


                if (Course_Id != 0 && District_Id != 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForAdDD().Where(a => a.Courseid == Course_Id && a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }

                else if (Course_Id != 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForAdDD().Where(a => a.Courseid == Course_Id && a.District_id == District_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForAdDD().Where(a => a.Courseid == Course_Id && a.Division_id == Division_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForAdDD().Where(a => a.District_id == District_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForAdDD().Where(a => a.Division_id == Division_Id).ToList();
                }
                else if (District_Id != 0 && Division_Id != 0 && Course_Id == 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForAdDD().Where(a => a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }
                else if (District_Id == 0 && Division_Id == 0 && Course_Id != 0)
                {

                    list = _AffilBll.GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForAdDD().Where(a => a.Courseid == Course_Id).ToList();
                }

                //list = list.Union(up_list).OrderByDescending(a => a.CreatedOn).ThenBy(a => a.iti_college_id).ToList();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FilterSubmittedAffiliateDetails

        public JsonResult FilterSubmittedAffiliateDetails(int? courseId, int? divisionId, int? districtId)
        {
            List<DeAffiliateInstitute> list = new List<DeAffiliateInstitute>();
            //List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            List<AffiliationCollegeDetails> up_list = new List<AffiliationCollegeDetails>();
            try
            {
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);


                if (Course_Id != 0 && District_Id != 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOnFilter().Where(a => a.Course_id == Course_Id && a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }

                else if (Course_Id != 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOnFilter().Where(a => a.Course_id == Course_Id && a.District_id == District_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOnFilter().Where(a => a.Course_id == Course_Id && a.Division_id == Division_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOnFilter().Where(a => a.District_id == District_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOnFilter().Where(a => a.Division_id == Division_Id).ToList();
                }
                else if (District_Id != 0 && Division_Id != 0 && Course_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOnFilter().Where(a => a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id != 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOnFilter().Where(a => a.Course_id == Course_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id == 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOnFilter().Where(a => a.Course_id == Course_Id).ToList();
                }

                //list = list.Union(up_list).OrderByDescending(a => a.CreatedOn).ThenBy(a => a.iti_college_id).ToList();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FilterSubmittedAffiliateDetailsForOS

        public JsonResult FilterSubmittedAffiliateDetailsForOS(int? courseId, int? divisionId, int? districtId)
        {
            List<DeAffiliateInstitute> list = new List<DeAffiliateInstitute>();
            //List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            List<AffiliationCollegeDetails> up_list = new List<AffiliationCollegeDetails>();
            try
            {
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);


                if (Course_Id != 0 && District_Id != 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOSOnFilter().Where(a => a.Course_id == Course_Id && a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }

                else if (Course_Id != 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOSOnFilter().Where(a => a.Course_id == Course_Id && a.District_id == District_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOSOnFilter().Where(a => a.Course_id == Course_Id && a.Division_id == Division_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOSOnFilter().Where(a => a.District_id == District_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOSOnFilter().Where(a => a.Division_id == Division_Id).ToList();
                }
                else if (District_Id != 0 && Division_Id != 0 && Course_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOSOnFilter().Where(a => a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id != 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOSOnFilter().Where(a => a.Course_id == Course_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id == 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLOSOnFilter().Where(a => a.Course_id == Course_Id).ToList();
                }

                //list = list.Union(up_list).OrderByDescending(a => a.CreatedOn).ThenBy(a => a.iti_college_id).ToList();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FilterSubmittedAffiliateDetailsForAD

        public JsonResult FilterSubmittedAffiliateDetailsForAD(int? courseId, int? divisionId, int? districtId)
        {
            List<DeAffiliateInstitute> list = new List<DeAffiliateInstitute>();
            //List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            List<AffiliationCollegeDetails> up_list = new List<AffiliationCollegeDetails>();
            try
            {
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);


                if (Course_Id != 0 && District_Id != 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLADOnFilter().Where(a => a.Course_id == Course_Id && a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }

                else if (Course_Id != 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLADOnFilter().Where(a => a.Course_id == Course_Id && a.District_id == District_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLADOnFilter().Where(a => a.Course_id == Course_Id && a.Division_id == Division_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id != 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLADOnFilter().Where(a => a.District_id == District_Id).ToList();
                }
                else if (Course_Id == 0 && District_Id == 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLADOnFilter().Where(a => a.Division_id == Division_Id).ToList();
                }
                else if (District_Id != 0 && Division_Id != 0 && Course_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLADOnFilter().Where(a => a.District_id == District_Id && a.Division_id == Division_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id != 0 && Division_Id != 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLADOnFilter().Where(a => a.Course_id == Course_Id).ToList();
                }
                else if (Course_Id != 0 && District_Id == 0 && Division_Id == 0)
                {

                    list = _AffilBll.GetAllAffiliateInsDeatilsBLLADOnFilter().Where(a => a.Course_id == Course_Id).ToList();
                }

                //list = list.Union(up_list).OrderByDescending(a => a.CreatedOn).ThenBy(a => a.iti_college_id).ToList();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get All Users1
        public JsonResult GetAllUsers1(int? statusValue)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                list = _AffilBll.GetAllUsersBLL();
                if (statusValue == 4 && role_id == 3)
                {
                    list.RemoveAll(t => t.Value != "7" && t.Value != "8");
                }
                else if (statusValue == 4 && role_id == 5)
                {
                    list.RemoveAll(t => t.Value != "7" && t.Value != "8" && t.Value != "3");
                }
                else if (statusValue == 7 && role_id == 5)
                {
                    list.RemoveAll(t => t.Value != "3");
                }
                else if (statusValue == 4 && role_id == 7)
                {
                    list.RemoveAll(t => t.Value != "8");
                }
                else if (statusValue == 7 && role_id == 7)
                {
                    list.RemoveAll(t => t.Value != "3" && t.Value != "5");
                }
                else if (statusValue == 7 && role_id == 3)
                {
                    list.RemoveAll(t => t.Value != "5");
                }
                else if (statusValue == 4 && role_id == 6)
                {
                    list.RemoveAll(t => t.Value != "7" && t.Value != "8" && t.Value != "5");
                }
                if (statusValue == null)
                {
                    list.DefaultIfEmpty(null);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

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

        #region Affiliation popup in login page
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
        #endregion

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
            return Json(new {listi= InstDetails }, JsonRequestBehavior.AllowGet);
        }

        #region Save New College Affiliations New
        [HttpPost]
        public JsonResult SaveCollegeAffilationDetailses(AffiliationCollegeDetailsTest formCollection1)
        {
            UploadAffiliation output = new UploadAffiliation();

            try
            {
                var UserId = Convert.ToInt32(Session["RoleId"]);
                formCollection1.CreatedBy = UserId;


                if (formCollection1.date != null)
                {
                    formCollection1.affiliation_date = Convert.ToDateTime(formCollection1.date);
                }

                if (formCollection1.order_no_date != null)
                {
                    formCollection1.AffiliationOrderNoDate = Convert.ToDateTime(formCollection1.order_no_date);
                }


                if (formCollection1.list_trades != null && formCollection1.list_units != null)
                {
                    if (formCollection1.list_trades.Count() == formCollection1.list_units.Count() && formCollection1.list_trades.Count() == formCollection1.list_keys.Count())
                    {

                        formCollection1.trades_list = new List<AffiliationTrade>();
                        // int f_i = 0;
                        for (var i = 0; i < formCollection1.list_trades.Count(); i++)
                        {
                            var model = new AffiliationTrade();
                            model.trade_id = formCollection1.list_trades[i];
                            model.units = formCollection1.list_units[i];
                            model.sessionKey = formCollection1.list_keys[i];
                            model.type = formCollection1.list_type[i];
                            formCollection1.trades_list.Add(model);
                        }

                        if (Request.Files.Count > 0)
                        {
                            string[] _fileformat = Request.Files[0].FileName.Split('.');
                            string _farmat = _fileformat.LastOrDefault();

                            var supportedTypes = new[] { "pdf" };
                            if (supportedTypes.Contains(_farmat))
                            {

                                string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

                                Random rand = new Random();

                                string File_name = Request.Files[0].FileName;//"AffiliationCollegePdf" + rand.Next() + "." + _farmat;

                                HttpPostedFileBase file = Request.Files[0];
                                string fname;
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = File_name.Split(new char[] { '\\' });
                                    fname = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    fname = File_name;
                                }
                                var uploadRootFolderInput = DocumentsFolder;
                                if (!Directory.Exists(uploadRootFolderInput))
                                {
                                    Directory.CreateDirectory(uploadRootFolderInput);
                                }

                                var directoryFullPathInput = uploadRootFolderInput;
                                fname = Path.Combine(directoryFullPathInput, fname);
                                file.SaveAs(fname);
                                formCollection1.FileUploadPath = fname;

                                if (!IsPDFHeader(fname))
                                {
                                    output.status = "Pls upload valid pdf file!";
                                    output.flag = 0;

                                    return Json(output, JsonRequestBehavior.AllowGet);
                                }

                            }
                            else
                            {
                                output.status = "Pls upload only pdf file formate";
                                output.flag = 0;

                                return Json(output, JsonRequestBehavior.AllowGet);
                            }
                        }

                        formCollection1.color_flag = 1;
                        _AffilBll.AddAffiliationCollegeDetailsBLL1(formCollection1);
                        output.flag = 1;
                    }
                    else
                    {
                        output.status = "failed";
                        output.flag = 0;
                    }
                }



            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult PublishActiveDeactiveShift(AffiliationNested model)
        {
            AffiliationNested mdl = new AffiliationNested();
            List<ActiveandDeactiveUnitsDeatils> InstDetails = new List<ActiveandDeactiveUnitsDeatils>();
            // var res = _affilia.GetGenerateMeritListBLL(model, loginId, remarks);
            var pblish = _AffilBll.PublishActiveDeactiveTradeUnit(model);
            return Json(pblish, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PublishAffiliateDeaffiliateInstitute(AffiliationNested model)
        {
            AffiliationNested mdl = new AffiliationNested();
            List<ActiveandDeactiveUnitsDeatils> InstDetails = new List<ActiveandDeactiveUnitsDeatils>();
            // var res = _affilia.GetGenerateMeritListBLL(model, loginId, remarks);
            var pblish = _AffilBll.PublishAffiliateDeaffiliateInstitute(model);
            return Json(pblish, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PublishITIInstitute(ToPublishRecords model)
        {
            //AffiliationNested mdl = new AffiliationNested();
            //List<AffiliationCollegeDetails> InstDetails = new List<AffiliationCollegeDetails>();
            //var res = _affilia.GetGenerateMeritListBLL(model, loginId, remarks);
            var pblish = _AffilBll.PublishAffiliateInstitutesBLL(model);
            //var pblish = "TEST";
            return Json(pblish, JsonRequestBehavior.AllowGet);
        }

        #region Search Published College Details

        public JsonResult FilterPublishedCollegeDetails(int? courseId, int? divisionId, int? districtId, int? tradeId)
        {
            List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();

            try
            {
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);
                int Trade_Id = Convert.ToInt32(tradeId);

                if (District_Id != 0 && Division_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeAndDistrictBLL(District_Id, Trade_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else
              if (Course_Id != 0 && Division_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id, District_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Course_Id != 0 && District_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }

                else if (Course_Id != 0 && District_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(Course_Id, District_Id, Trade_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Course_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(Course_Id, District_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Course_Id != 0 && Division_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionAndCourseBLL(Course_Id, Division_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Division_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Course_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Division_Id != 0 && Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }

                else if (Course_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLL(Course_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Division_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLL(Division_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLL(District_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Trade_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeBLL(Trade_Id).Where((a => a.status_id == (int)CsystemType.getCommon.Published || a.status_id == (int)CsystemType.getCommon.pub)).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLL((int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();
                   //_AffilBll.GetAllAffiliationCollegeDetailsBLL().Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();
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
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion
       
        public JsonResult FilterPublishedCollegeDetails1(int? divisionId, int? districtId, int? talukId, int? constituencyId,int? Coursetype,int? iticollegeId)
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
                int College_id = Convert.ToInt32(iticollegeId);

                if (District_Id != 0 && Division_Id != 0 && Constituency_Id != 0 && Coursetype_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeAndDistrictBLLFilter(District_Id, Constituency_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Taluk_Id != 0 && Division_Id != 0 && District_Id != 0 )
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLLFilter(Taluk_Id, District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Taluk_Id != 0 && District_Id != 0 && Constituency_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLLFilter(Taluk_Id, District_Id, Constituency_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }

                else if (Taluk_Id != 0 && District_Id != 0 && Constituency_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLLFilter(Taluk_Id, District_Id, Constituency_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Taluk_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictAndCourseBLLFilter(Taluk_Id, District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Taluk_Id != 0 && Division_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionAndCourseBLLFilter(Taluk_Id, Division_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Division_Id != 0 && District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLLFilter(District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Taluk_Id != 0 && Constituency_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLLFilter(Taluk_Id).Where(a => a.trade_id == Constituency_Id && a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Division_Id != 0 && Constituency_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLLFilter(Division_Id).Where(a => a.trade_id == Constituency_Id && a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }

                else if (Taluk_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByCourseBLLFilter(Taluk_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Division_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDivisionBLLFilter(Division_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (District_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByDistrictBLLFilter(District_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Taluk_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsByTradeBLLFilter(Constituency_Id).Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else if (Coursetype_Id != 0)
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLLFilter().Where(a => a.status_id == (int)CsystemType.getCommon.Published && a.course_code==Coursetype_Id).OrderByDescending(a => a.CreatedOn).ToList();

                }
                else
                {
                    list = _AffilBll.GetAllAffiliationCollegeDetailsBLLFilter().Where(a => a.status_id == (int)CsystemType.getCommon.Published).OrderByDescending(a => a.CreatedOn).ToList();
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
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //end 
        #region Get All Designation
        public JsonResult GetAllDesignation()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                list = _AffilBll.GetAllDesignationDLL();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Teaching Subject
        public JsonResult GetAllTeachingSubject()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                list = _AffilBll.GetAllTeachingSubjectDLL();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Trades
        public JsonResult GetAllTrades()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                list = _AffilBll.GetAllTradesDLL();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Year
        public JsonResult GetAllYear()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                list = _AffilBll.GetAllYearDLL();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Institute
        public JsonResult GetAllInstitute(int DistId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                list = _AffilBll.GetAllInstitute(DistId);

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Gender
        public JsonResult GetAllGender()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                list = _AffilBll.GetAllGender();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region affiliate details
        public ActionResult ItiInstituteAffiliationDetails()
        {
            if (Session["UserId"] != null)
            {
                _CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
                int LoginId = Convert.ToInt32(Session["LoginID"]);
                ViewBag.UserDivionId = _AffilBll.GetUserDivionIdBLL(LoginId);
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
        public JsonResult InstituteAffiliationreport(int? Year, int? courseId, int? divisionId, 
            int? districtId, int? taluk, int? Insttype, int? location, int? tradeId, 
            int? scheme, string training, int? ReportType)
        {
            List<ActiveandDeactiveUnitsDeatils> list = new List<ActiveandDeactiveUnitsDeatils>();
            ActiveandDeactiveUnitsDeatils Activedeactives = new ActiveandDeactiveUnitsDeatils();
            Activedeactives.InstCount = 0;
            try
            {
                Log.Info("Entered GetSeatAvailability()");
                int Year_id = Convert.ToInt32(Year);
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);
                int taluk_id = Convert.ToInt32(taluk);
                int Insttype_Id = Convert.ToInt32(Insttype);
                int location_Id = Convert.ToInt32(location);
                int Trade_Id = Convert.ToInt32(tradeId);
                int scheme_Id = Convert.ToInt32(scheme);
                string training_Id =training;
                int ReportType_Id = Convert.ToInt32(ReportType);


                int x = 1;

                list = _AffilBll.GetAllAffiliatedInstituteReportDLL(Year_id, Course_Id, Division_Id, District_Id, taluk_id, Insttype_Id, location_Id, (int)tradeId, scheme_Id, training_Id, ReportType_Id);

                //if (Course_Id != 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id != 0 && scheme_Id != 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == Course_Id && a.Division_id == Division_Id && a.District_id == District_Id && a.ITI_trade_id == Trade_Id && a.scheme_Id == scheme_Id && a.Taluk_id == taluk_id && a.location_Id == location_Id && a.Insttype_Id == Insttype_Id).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id != 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == Course_Id && a.Division_id == Division_Id && a.District_id == District_Id && a.Taluk_id == taluk_id && a.location_Id == location_Id && a.Insttype_Id == Insttype_Id && a.ITI_trade_id == Trade_Id).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == Course_Id && a.Division_id == Division_Id && a.District_id == District_Id && a.Taluk_id == taluk_id && a.location_Id == location_Id && a.Insttype_Id == Insttype_Id).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == Course_Id && a.Division_id == Division_Id && a.District_id == District_Id && a.Taluk_id == taluk_id && a.Insttype_Id == Insttype_Id).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == Course_Id && a.Division_id == Division_Id && a.District_id == District_Id && a.Taluk_id == taluk_id).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id != 0 && District_Id != 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == Course_Id && a.Division_id == Division_Id && a.District_id == District_Id).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id != 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == Course_Id && a.Division_id == Division_Id).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && ReportType_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == Course_Id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id != 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Division_id == Division_Id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id == 0 && District_Id != 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.District_id == District_Id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id != 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Taluk_id == taluk_id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id != 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Insttype_Id == Insttype_Id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id != 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.location_Id == location_Id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id != 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.ITI_trade_id == Trade_Id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id != 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.scheme_Id == scheme_Id).ToList();

                //}
                //else if (Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && Year_id==2 && ReportType_Id==0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().ToList();

                //}
                //else if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && Year_id == 2 && ReportType_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().ToList();

                //}
                //else if (Course_Id == 0 && Division_Id != 0 && District_Id != 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && ReportType_Id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Division_id == Division_Id && a.District_id==districtId).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && ReportType_Id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Division_id == Division_Id && a.District_id == districtId && a.Taluk_id == taluk_id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id == 0 && scheme_Id == 0 && ReportType_Id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Insttype_Id == Insttype_Id && a.location_Id == location_Id ).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id != 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && ReportType_Id == 0 && ReportType_Id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == courseId && a.Insttype_Id == Insttype_Id).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id != 0 && scheme_Id == 0 && ReportType_Id == 0 && ReportType_Id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == courseId && a.ITI_trade_id == tradeId).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id != 0 && Trade_Id == 0 && scheme_Id == 0 && ReportType_Id == 0 && ReportType_Id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == courseId && a.location_Id == location_Id).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && Year_id==2 && ReportType_Id == 0)
                //{

                //    //list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == courseId && a.location_Id == location_Id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && training != "0" && ReportType_Id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.ModeofTraining == training).ToList();

                //}
                //else if (Course_Id != 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && training == "0" && ReportType_Id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid==courseId && a.Division_id==divisionId && a.District_id==District_Id && a.Taluk_id==taluk_id&& a.Insttype_Id == Insttype_Id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && training == "0" && ReportType_Id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Division_id == divisionId && a.District_id == District_Id && a.Taluk_id == taluk_id && a.Insttype_Id == Insttype_Id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && training == "0" && ReportType_Id==0 && Year_id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().ToList();

                //}
                //else if (Course_Id == 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id != 0 && scheme_Id != 0 && training != "0" && ReportType_Id == 0)
                //{

                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Division_id == divisionId && a.District_id == District_Id && a.Taluk_id == taluk_id && a.Insttype_Id == Insttype_Id && a.location_Id ==location_Id &&a.ITI_trade_id==tradeId && a.scheme_Id==scheme_Id && a.ModeofTraining==training).ToList(); 

                //}
                //if (Course_Id == 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id != 0 && scheme_Id != 0 && training == "0" && ReportType_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Division_id == divisionId && a.District_id == District_Id && a.Taluk_id == taluk_id && a.Insttype_Id == Insttype_Id && a.location_Id == location_Id && a.ITI_trade_id == tradeId && a.scheme_Id == scheme_Id).ToList();

                //}
                //if (Course_Id == 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id != 0 && scheme_Id == 0 && training == "0" && ReportType_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Division_id == divisionId && a.District_id == District_Id && a.Taluk_id == taluk_id && a.Insttype_Id == Insttype_Id && a.location_Id == location_Id && a.ITI_trade_id == tradeId).ToList();

                //}
                //if (Course_Id == 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id == 0 && scheme_Id == 0 && training == "0" && ReportType_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Division_id == divisionId && a.District_id == District_Id && a.Taluk_id == taluk_id && a.Insttype_Id == Insttype_Id && a.location_Id == location_Id ).ToList();

                //}
                //if (Course_Id == 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && training == "0" && ReportType_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Division_id == divisionId && a.District_id == District_Id && a.Taluk_id == taluk_id && a.Insttype_Id == Insttype_Id ).ToList();

                //}
                //if (Course_Id != 0 && Division_Id != 0 && District_Id != 0 && taluk_id != 0 && Insttype_Id != 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 && training == "0" && ReportType_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid== Course_Id &&a.Division_id == divisionId && a.District_id == District_Id && a.Taluk_id == taluk_id && a.Insttype_Id == Insttype_Id).ToList();

                //}
                //if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id == 0 && scheme_Id == 0 && training == "0" && ReportType_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a =>  a.Insttype_Id == Insttype_Id && a.location_Id==location_Id).ToList();

                //}
                //if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id != 0 && scheme_Id == 0 && training == "0" && ReportType_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Insttype_Id == Insttype_Id && a.location_Id == location_Id && a.ITI_trade_id == Trade_Id).ToList();

                //}
                //if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id == 0 && scheme_Id != 0 && training == "0" && ReportType_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Insttype_Id == Insttype_Id && a.location_Id == location_Id && a.scheme_Id == scheme_Id).ToList();

                //}
                //if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id != 0 && Trade_Id == 0 && scheme_Id == 0 && training != "0" && ReportType_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a =>  a.location_Id == location_Id && a.ModeofTraining == training).ToList();

                //}
                //else if (ReportType_Id == 1 && Course_Id != 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a=>a.Courseid==Course_Id && a.ReqIsActive == true).ToList();
                //}
                //else if (ReportType_Id == 2 && Course_Id != 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.Courseid == Course_Id && a.ReqIsActive == false).ToList();
                //}
                //else if (ReportType_Id == 3 && Course_Id != 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLLForTrade().Where(a => a.Courseid == Course_Id).ToList();
                //}
                //else if (ReportType_Id == 4 && Course_Id != 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLLForUnits().Where(a => a.Courseid == Course_Id).ToList();

                //}
                ////else if (ReportType_Id == 1 && (training_Id != null || training_Id != "0") && Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0 )
                ////{
                ////    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.ModeofTraining == training_Id && a.ReqIsActive == true).ToList();
                ////}
                //else if (ReportType_Id == 2 && training_Id != null && training_Id != "0" && Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.ModeofTraining == training_Id && a.ReqIsActive == false).ToList();
                //}
                //else if (ReportType_Id == 2 && training_Id != null && training_Id != "0" && scheme_Id != 0 && Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 )
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.ModeofTraining == training_Id && a.scheme_Id == scheme_Id && a.ReqIsActive == false).ToList();
                //}
                //else if (ReportType_Id == 3 && training_Id != null && training_Id != "0" && Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLLForTrade().Where(a => a.ModeofTraining == training_Id).ToList();
                //}
                //else if (ReportType_Id == 4 && training_Id != null && training_Id != "0" && Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id == 0 && location_Id == 0 && Trade_Id == 0 && scheme_Id == 0)
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLLForUnits().Where(a => a.ModeofTraining == training_Id).ToList();

                //}
                //else if (Course_Id == 0 && Division_Id == 0 && District_Id == 0 && taluk_id == 0 && Insttype_Id != 0 && location_Id != 0 && Trade_Id != 0 && scheme_Id != 0 && (training_Id != null || training_Id != "0"))
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a =>  a.location_Id == location_Id && a.Insttype_Id == Insttype_Id && a.ITI_trade_id == Trade_Id && a.scheme_Id==scheme_Id && a.ModeofTraining==training_Id).ToList();

                //}
                //else if (ReportType_Id == 1)
                //{
                //     list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a=>a.ReqIsActive==true).ToList();
                //}
                //else if (ReportType_Id == 2 )
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLL().Where(a => a.ReqIsActive == false).ToList();
                //}
                //if (ReportType_Id == 3 )
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLLForTrade().ToList();
                //}
                //else if (ReportType_Id == 4 )
                //{
                //    list = _AffilBll.GetAllAffiliatedInstituteReportDLLForUnits().ToList();

                //}
                foreach (var item in list)
                {
                    item.slno = x;
                    //item.Rank = x;
                    x++;
                    item.Year = "2021";
                    Activedeactives.InstCount++ ;
                    item.ReportType = "ALL";

                    if (ReportType == 1)
                    {
                        item.ReportType = "Affiliated";
                        item.trades="NA";
                        item.Scheme = "NA";
                        item.NoOfUnits = "NA";
                    }
                    else if (ReportType == 2)
                    {
                        item.ReportType = "De-Affiliated";
                        item.trades = "NA";
                        item.trades = "NA";
                        item.Scheme = "NA";
                        item.NoOfUnits = "NA";
                    }
                    if (ReportType == 3)
                    {
                        item.ReportType = "Trade De-Active";
                        ;
                    }
                    if (ReportType == 4)
                    {
                        item.ReportType = "Units De-Active";
                        ;
                    }

                }
                Log.Info("Exited InstitutteAffiliationReport()");
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(new { Institutelist = list, resultllist = Activedeactives }, JsonRequestBehavior.AllowGet);
        }

        #region Get Institute Details For Admin

        public JsonResult GetInstituteDetailsForAdmin()
        {
            List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            try
            {
                int UserId = (int)Session["UserId"];
                int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
               list = _AffilBll.GetAllDeaffiliateInstituteAffBLL(college_id).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception: " + ex.Message);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Staff Module logic
        //InstitutestaffDetails
        public ActionResult InstitutestaffDetails()
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    Log.Info("Entered InstitutestaffDetails()");
                    _CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
                    Log.Info("Entered InstitutestaffDetails()");
                    return View();
                    //int UserId = (int)Session["UserId"];
                    //int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
                    //List<StaffInstituteDetails> model = new List<StaffInstituteDetails>();
                    //model = _AffilBll.ListstaffDetails(college_id);
                    //if (model != null)
                    //    model.ForEach(i => i.slno = ++i.slno);
                    ////if (model != null)
                    ////    model.slno = 1;
                    ////model.Year = "2021";
                    //return View(model);
                }
                catch (Exception ex)
                {
                    Log.Error("Entered Exception - InstitutestaffDetails():" + ex.Message.ToString());
                    throw ex;
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public JsonResult GetAllStaffType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                list = _AffilBll.GetAllStaffTypeDLL();

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddStaffDetails(StaffInstituteDetails staff)
        {
            int loginId = Convert.ToInt32(Session["LoginId"].ToString());
            int UserId = (int)Session["UserId"];
            int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
            staff.InstituteId = college_id;
            staff.UserId = UserId;
            if (staff.ImageFile != null)
            {
                string UniqueFileName = null;
                string[] string1;
                string extension = System.IO.Path.GetExtension(staff.ImageFile.FileName).Substring(1);
               
                string _FileName = Path.GetFileName(staff.ImageFile.FileName);
                extension = System.IO.Path.GetExtension(staff.ImageFile.FileName).Substring(1);
                UniqueFileName = "_ADI_" + Guid.NewGuid().ToString() + "." + extension;
                string _path = Path.Combine(Server.MapPath("Content/ADFiles/"), UniqueFileName);
                string _pathCreate = Path.Combine(Server.MapPath("Content/ADFiles/"));
                if (!Directory.Exists(_pathCreate))
                {
                    Directory.CreateDirectory(_pathCreate);
                }
                staff.ImageFile.SaveAs(_path);

                
                staff.Photo = "Content/ADFiles/" + UniqueFileName;

                            }
            var res = _AffilBll.AddStaffDetail(staff, loginId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStaffDetails()
        {
            int loginId = Convert.ToInt32(Session["LoginId"].ToString());
            int UserId = (int)Session["UserId"];
            int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
            var res = _AffilBll.GetstaffDetails(college_id);
            var fdata = res.Where(a => a.Appeovalstatus == 2 && a.IsActive == true).ToList();

            int c = 1;
            foreach (var item in res)
            {
                item.slno = c;
                c++;

            }
            int j = 1;
            foreach (var lst in fdata)
            {
                lst.slno = j;
                j++;


            }
            return Json(new { result = res, filt = fdata }, JsonRequestBehavior.AllowGet);

        }
        //public ActionResult GetStaffDetailsForInstitutes(int id)
        //{
        //    int loginId = Convert.ToInt32(Session["LoginId"].ToString());
        //    int UserId = (int)Session["UserId"];
        //    int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
        //    var res = _AffilBll.GetstaffDetails(id);
        //    var resultlist = _AffilBll.GetstaffDetails(id).Where(a => a.Appeovalstatus == (int)CsystemType.getCommon.Approved && a.IsActive == true).ToList();

        //    int c = 1;
        //    foreach (var item in res)
        //    {
        //        item.slno = c;
        //        c++;

        //    }
        //    int d = 1;
        //    foreach (var item in resultlist)
        //    {
        //        item.slno = d;
        //        d++;

        //    }
        //    return Json(new { res, resultlist }, JsonRequestBehavior.AllowGet);

        //}
        public ActionResult EditStaff(int id)
        {
            StaffInstituteDetails staffdetails = new StaffInstituteDetails();

            staffdetails = _AffilBll.EditStaff(id);
            staffdetails.MultiSelectSubjectList = _AffilBll.GetStaffSubjectList(id);
            staffdetails.MultiSelectTradeList = _AffilBll.GetStafTradeList(id);
            int i = 0;
            staffdetails.staffsub = null; staffdetails.selectstaffsub = null;
            foreach (var SelectedSubject in staffdetails.MultiSelectSubjectList)
            {
                if (i == 0)
                    staffdetails.staffsub += SelectedSubject;
                else
                    staffdetails.staffsub += "," + SelectedSubject;
                i++;
            }
            staffdetails.selectstaffsub = staffdetails.staffsub;

            int j = 0;
            //staffdetails.stafftrade = null; staffdetails.selectstafftrade = null;
            foreach (var SelectedTrade in staffdetails.MultiSelectTradeList)
            {
                if (j == 0)
                    staffdetails.stafftrade += SelectedTrade;
                else
                    staffdetails.stafftrade += "," + SelectedTrade;
                j++;
            }
            staffdetails.selectstafftrade = staffdetails.stafftrade;
            staffdetails.DesignationList = _AffilBll.GetAllDesignationDLL();
            staffdetails.StafftypeList = _AffilBll.GetAllStaffTypeDLL();
            staffdetails.SubjectList = _AffilBll.GetAllTeachingSubjectDLL();
            staffdetails.TradeList = _AffilBll.GetAllTradesDLL();
            staffdetails.GenderList = _AffilBll.GetAllGender();
            staffdetails.CourseList = _AffilBll.GetCourseListBLL();

            return Json(new { Resultlist = staffdetails, des = staffdetails.DesignationList, type = staffdetails.StafftypeList, subj = staffdetails.SubjectList, trade = staffdetails.TradeList, gen = staffdetails.GenderList,course=staffdetails.CourseList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Updatestaff(StaffInstituteDetails staff)
        {
            int loginId = Convert.ToInt32(Session["LoginId"].ToString());
            if (staff.ImageFile != null)
            {
                string UniqueFileName = null;
                string[] string1;
                string extension = System.IO.Path.GetExtension(staff.ImageFile.FileName).Substring(1);

                string _FileName = Path.GetFileName(staff.ImageFile.FileName);
                extension = System.IO.Path.GetExtension(staff.ImageFile.FileName).Substring(1);
                UniqueFileName = "_ADI_" + Guid.NewGuid().ToString() + "." + extension;
                string _path = Path.Combine(Server.MapPath("Content/ADFiles/"), UniqueFileName);
                string _pathCreate = Path.Combine(Server.MapPath("Content/ADFiles/"));
                if (!Directory.Exists(_pathCreate))
                {
                    Directory.CreateDirectory(_pathCreate);
                }
                staff.ImageFile.SaveAs(_path);


                staff.Photo = "Content/ADFiles/" + UniqueFileName;

            }
            var res = _AffilBll.UpdateStaff(staff, loginId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Deletestaff(int id)
        {
            var res = _AffilBll.DeleteStaff(id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetStaffstatusForCW()
        {
            int UserId = (int)Session["UserId"];
            int roleid = (int)Session["RoleId"];
            // userid must be 0 because CW must see all institute details
            var res = FilterData(UserId);//_AffilBll.GetstaffStatusForCW();
            res = res?.Where(a => a.StatusName.Length > 4).ToList();
            res?.ForEach(x =>
            {
                x.IsAction = false;
                if (x.ApprovalFlowId == roleid)
                    x.IsAction = true;
            }
                );

            int c = 1;
            foreach (var item in res)
            {
                item.slno = c;
                c++;

            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetStaffstatusRole()
        {

            var res = _AffilBll.GetstaffStatusOSAD(0,0,0,0);
            int c = 1;
            foreach (var item in res)
            {
                item.slno = c;
                c++;

            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }
        public ActionResult InstitutestaffApproval()
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    Log.Info("Entered InstitutestaffApproval()");
                    _CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
                    int LoginId = Convert.ToInt32(Session["LoginID"]);
                    ViewBag.UserDivionId = _AffilBll.GetUserDivionIdBLL(LoginId);
                    Log.Info("Exited InstitutestaffApproval()");
                    return View();
                }
                catch (Exception ex)
                {
                    Log.Error("Entered Exception - InstitutestaffApproval():" + ex.Message.ToString());
                    throw ex;
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult staffApproval()
        {
            return View();
        }
        public ActionResult ViewStaff(int id)
        {
            try
            {
                Log.Info("Entered ViewStaff()");
                var res = _AffilBll.ViewStaff(id);
                Log.Info("Exited ViewStaff()");
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception - ViewStaff():" + ex.Message.ToString());
                throw ex;
            }
        }
        public ActionResult Approvestaff(List<StaffInstituteDetails> staff)
        {
            int loginId = Convert.ToInt32(Session["RoleId"].ToString());
            int UserId = (int)Session["UserId"];
            // staff.UserId = UserId;
            var res = _AffilBll.ApproveStaff(staff, loginId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ItiInstituteStaffReport()

        {
            if (Session["UserId"] != null)
            {
                _CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
                int LoginId = Convert.ToInt32(Session["LoginID"]);
                ViewBag.UserDivionId = _AffilBll.GetUserDivionIdBLL(LoginId);//GetUserDivionId(LoginId);
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public JsonResult Institutestaffreport(int? Year, int? courseId, int? divisionId,
            int? districtId, int? taluk, int? Insttype, int? location, int? stafftype, int? tradeId,
            int? gender, int? scheme, string training,int? quarter)
        {
            //List<AffiliationCollegeDetails> list = new List<AffiliationCollegeDetails>();
            List<StaffInstituteDetails> list = new List<StaffInstituteDetails>();
            StaffInstituteDetails staffdetails = new StaffInstituteDetails();
            staffdetails.StaffCount = 0;
            try
            {
                int Year_id = Convert.ToInt32(Year);
                int Course_Id = Convert.ToInt32(courseId);
                int Division_Id = Convert.ToInt32(divisionId);
                int District_Id = Convert.ToInt32(districtId);
                int? taluk_id = Convert.ToInt32(taluk);
                int Insttype_Id = Convert.ToInt32(Insttype);
                int location_Id = Convert.ToInt32(location);
                int stafftype_Id = Convert.ToInt32(stafftype);
                int Trade_Id = Convert.ToInt32(tradeId);
                int gender_Id = Convert.ToInt32(gender);
                int scheme_Id = Convert.ToInt32(scheme);
                string training_Id = training;
                int quarter_Id = Convert.ToInt32(quarter);

                int x = 1;
                list = _AffilBll.GetAllstaffInstituteReport((int)Year, (int)courseId, (int)divisionId,
             (int)districtId, (int)taluk, (int)Insttype, (int)location, (int)stafftype, (int)tradeId,
             (int)gender, (int)scheme, training, (int)quarter);
                
                foreach (var item in list)
                {
                    item.slno = x;

                    x++;
                    staffdetails.StaffCount++;
                }

            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(new { Stafflist = list, resultllist = staffdetails }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetAllUsersforstaff(int? statusValue)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var role_id = Convert.ToInt32(Session["RoleId"]);
                list = _AffilBll.GetAllUsersBLL();
                if (statusValue == 4 && role_id == 3)
                {
                    list.RemoveAll(t => t.Value != "9");
                }
                else if (statusValue == 4 && role_id == 15)
                {
                    list.RemoveAll(t => t.Value != "9");
                }
                else if (statusValue == 7 && role_id == 15)
                {
                    list.RemoveAll(t => t.Value != "14");
                }
                else if (statusValue == 4 && role_id == 7)
                {
                    list.RemoveAll(t => t.Value != "9");
                }
                else if (statusValue == 7 && role_id == 13)
                {
                    list.RemoveAll(t => t.Value != "14" && t.Value != "15");
                }
                else if (statusValue == 7 && role_id == 14)
                {
                    list.RemoveAll(t => t.Value != "15");
                }
                else if (statusValue == 4 && role_id == 6)
                {
                    list.RemoveAll(t => t.Value != "13" && t.Value != "17" && t.Value != "15");
                }
                else if (statusValue == 7 && role_id == 17)
                {
                    list.RemoveAll(t => t.Value != "13" && t.Value != "14" && t.Value != "15");
                }
                if (statusValue == null)
                {
                    list.DefaultIfEmpty(null);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StaffDeatilsonSearch(int? Year, int? courseId)
        {
            List<StaffInstituteDetails> list = new List<StaffInstituteDetails>();
            List<StaffInstituteDetails> Approvedlist = new List<StaffInstituteDetails>();
            try
            {

                int UserId = (int)Session["UserId"];
                int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);

                if (courseId != null && Year == null)
                {
                    list = _AffilBll.GetstaffDetails(college_id).Where(a => a.Courseid == courseId).ToList();
                }
                else if (Year != null && courseId != null)
                {
                    list = _AffilBll.GetstaffDetails(college_id).Where(a => a.Courseid == courseId && a.YearId == Year).ToList();
                }
                else if (Year != null && courseId == null)
                {
                    list = _AffilBll.GetstaffDetails(college_id).Where(a => a.YearId == Year).ToList();
                }

                if (courseId != null && Year == null)
                {
                    Approvedlist = _AffilBll.GetstaffDetails(college_id).Where(a => a.Courseid == courseId && a.Appeovalstatus == (int)CsystemType.getCommon.Approved && a.IsActive == true).ToList();
                }
                else if (Year != null && courseId != null)
                {
                    Approvedlist = _AffilBll.GetstaffDetails(college_id).Where(a => a.Courseid == courseId && a.YearId == Year && a.Appeovalstatus == (int)CsystemType.getCommon.Approved && a.IsActive == true).ToList();
                }
                else if (Year != null && courseId == null)
                {
                    Approvedlist = _AffilBll.GetstaffDetails(college_id).Where(a => a.YearId == Year && a.Appeovalstatus == (int)CsystemType.getCommon.Approved && a.IsActive == true).ToList();
                }

                int c = 1;
                foreach (var item in list)
                {
                    item.slno = c;
                    c++;

                }
                int j = 1;
                foreach (var aplist in Approvedlist)
                {
                    aplist.slno = j;
                    j++;

                }


            }
            catch (Exception ex)
            {
                Log.Error("Entered Exception:" + ex.Message.ToString());
            }

            return Json(new { list, Approvedlist }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchStaff(int? Year, int? courseId,int? Quarter1)
        {
            int usrid = Convert.ToInt32(Session["UserId"].ToString());
            int roleid = (int)Session["RoleId"];
            List<StaffInstituteDetails> list = new List<StaffInstituteDetails>();
            //list = _AffilBll.GetstaffStatus(usrid, Convert.ToInt32(Year), Convert.ToInt32(courseId), Convert.ToInt32(Quarter1));
            if (courseId != null && Year != null && Quarter1!=null)
            {
                list = _AffilBll.GetstaffStatus(usrid, Convert.ToInt32(Year), Convert.ToInt32(courseId), Convert.ToInt32(Quarter1)).Where(a => a.Courseid == courseId && a.YearId==Year && a.Quarter==Quarter1) .ToList();
            }
            else if (courseId != null && Year != null && Quarter1 == null)
            {
                list = _AffilBll.GetstaffStatus(usrid, Convert.ToInt32(Year), Convert.ToInt32(courseId), Convert.ToInt32(Quarter1)).Where(a => a.Courseid == courseId && a.YearId == Year ).ToList();
            }
            else if (courseId == null && Year != null && Quarter1 == null)
            {
                list = _AffilBll.GetstaffStatus(usrid, Convert.ToInt32(Year), Convert.ToInt32(courseId), Convert.ToInt32(Quarter1)).Where(a =>  a.YearId == Year && a.Quarter == Quarter1).ToList();
            }
            if (courseId != null && Year == null && Quarter1 != null)
            {
                list = _AffilBll.GetstaffStatus(usrid, Convert.ToInt32(Year), Convert.ToInt32(courseId), Convert.ToInt32(Quarter1)).Where(a => a.Courseid == courseId && a.Quarter == Quarter1).ToList();
            }
            if (courseId == null && Year != null && Quarter1 != null)
            {
                list = _AffilBll.GetstaffStatus(usrid, Convert.ToInt32(Year), Convert.ToInt32(courseId), Convert.ToInt32(Quarter1)).Where(a =>  a.YearId == Year && a.Quarter == Quarter1).ToList();
            }
            if (courseId == null && Year == null && Quarter1 != null)
            {
                list = _AffilBll.GetstaffStatus(usrid, Convert.ToInt32(Year), Convert.ToInt32(courseId), Convert.ToInt32(Quarter1)).Where(a => a.Quarter == Quarter1).ToList();
            }
            list?.ForEach(x =>
            {
                x.IsAction = false;
                if (x.ApprovalFlowId == roleid)
                    x.IsAction = true;
            }
                );
            int c = 1;
            foreach (var item in list)
            {
                item.slno = c;
                c++;

            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SearchStaffForOSAD(int? Year, int? courseId, int? quarter)
        {
            List<StaffInstituteDetails> list = new List<StaffInstituteDetails>();
            int usrid = Convert.ToInt32(Session["UserId"].ToString());
            int roleid = (int)Session["RoleId"];
            string year = Year.ToString();
          
                list = _AffilBll.GetstaffStatusOSAD( usrid, Convert.ToInt32(Year), Convert.ToInt32(courseId), Convert.ToInt32(quarter)).ToList();

            //else if (courseId != null && Year != null)
            //{
            //    list = _AffilBll.GetstaffStatusOSAD().Where(a => a.Courseid == courseId && a.Year== Year.ToString()).OrderByDescending(a => a.CreatedOn).ToList();
            //}
            //else if (courseId == null && Year != null)
            //{
            //    list = _AffilBll.GetstaffStatusOSAD().Where(a => a.Year == Year.ToString()).OrderByDescending(a=>a.CreatedOn).ToList();
            //}
            list?.ForEach(x =>
            {
                x.IsAction = false;
                if (x.ApprovalFlowId == roleid)
                    x.IsAction = true;
            }
              );
            int c = 1;
            foreach (var item in list)
            {
                item.slno = c;
                c++;

            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult SaveStaffDetails(List<StaffInstituteDetails> staff)
        {
            int loginId = Convert.ToInt32(Session["UserId"].ToString());
            int roleId = Convert.ToInt32(Session["RoleId"].ToString());
            var response = _AffilBll.SubmitStaffDetails(staff, loginId, roleId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StaffDetailsView()
        {
            int UserId = (int)Session["UserId"];
            int roleid = (int)Session["RoleId"];
            //var res = _AffilBll.GetApprovedstaffInstitute();
            var res= _AffilBll.GetApprovedstaffInstituteDivisionWise(UserId);

            int c = 1;
            foreach (var item in res)
            {
                item.slno = c;
                c++;

            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ViewStaffDetailsView(int? year, int? courseId, int? DivisionId,
            int? DistrictId, int? InstituteId,int? quarter)
        {
            List<StaffInstituteDetails> list = new List<StaffInstituteDetails>();
            list = _AffilBll.GetApprovedstaffInstitute(Convert.ToInt32(Session["UserId"].ToString()), Convert.ToInt32(year), Convert.ToInt32(courseId), Convert.ToInt32(DivisionId),
            Convert.ToInt32(DistrictId), Convert.ToInt32(InstituteId), Convert.ToInt32(quarter));
            //if (year != null && courseId != null && DivisionId == null && InstituteId == null && quarter!= null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute(Convert.ToInt32(Session["UserId"].ToString()), Convert.ToInt32(year), Convert.ToInt32(courseId), Convert.ToInt32(DivisionId),
            //Convert.ToInt32(DistrictId), Convert.ToInt32(InstituteId), Convert.ToInt32(quarter)).Where(a=>a.YearId==year && a.Courseid==courseId && a.Quarter==quarter).ToList();
            //}
            //else if (year != null && courseId != null && DivisionId != null && DistrictId != null && InstituteId == null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute().Where(a => a.Courseid == courseId && a.DivisionId == DivisionId && a.DistrictId == DistrictId).ToList();
            //}
            //else if (year == null && courseId == null && DivisionId == null && InstituteId != null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute()?.Where(a => a.InstituteId == InstituteId).ToList();
            //}
            //else if (year != null && courseId == null && DivisionId == null && InstituteId == null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute()?.Where(a => a.YearId == year).ToList();
            //}
            //else if (year != null && courseId != null && DivisionId == null && InstituteId == null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute()?.Where(a => a.YearId == year && a.Courseid == courseId).ToList();
            //}
            //else if (year == null && courseId != null && DivisionId == null && InstituteId == null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute()?.Where(a => a.Courseid == courseId).ToList();
            //}
            //else if (year == null && courseId == null && DivisionId != null && DistrictId == null && InstituteId == null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute()?.Where(a => a.DivisionId == DivisionId).ToList();
            //}
            //else if (year == null && courseId == null && DivisionId != null && DistrictId != null && InstituteId == null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute()?.Where(a => a.DivisionId == DivisionId && a.DistrictId==DistrictId).ToList();
            //}
            //else if (year == null && courseId != null && DivisionId != null && DistrictId != null && InstituteId == null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute()?.Where(a => a.Courseid==courseId && a.DivisionId == DivisionId && a.DistrictId == DistrictId).ToList();
            //}
            //else if (year != null && quarter!=null &&courseId != null && DivisionId != null && DistrictId != null && InstituteId == null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute().Where(a => a.Courseid == courseId && a.DivisionId == DivisionId && a.DistrictId == DistrictId&& a.Quarter==quarter).ToList();
            //}
            //else if (year != null && quarter!=null && courseId != null && DivisionId != null && DistrictId != null && InstituteId != null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute().Where(a => a.Courseid == courseId && a.DivisionId == DivisionId && a.DistrictId == DistrictId && a.InstituteId==InstituteId && a.Quarter==quarter && a.YearId == year).ToList();
            //}
            //else if (year != null && quarter != null && courseId != null && DivisionId != null && DistrictId == null && InstituteId == null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute().Where(a => a.Courseid == courseId && a.DivisionId == DivisionId && a.Quarter == quarter && a.YearId==year).ToList();
            //}
            //else if (year != null && quarter != null && courseId != null && DivisionId == null && DistrictId == null && InstituteId == null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute().Where(a => a.Courseid == courseId && a.Quarter == quarter && a.YearId == year).ToList();
            //}
            //else if (year != null && quarter != null && courseId == null && DivisionId == null && DistrictId == null && InstituteId == null)
            //{
            //    list = _AffilBll.GetApprovedstaffInstitute().Where(a => a.Quarter == quarter && a.YearId == year).ToList();
            //}
            int c = 1;
            foreach (var item in list)
            {
                item.slno = c;
                c++;

            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        #region re-implementation -- staff details
        public ActionResult GetStaffDetailsSessionWise(string session = "", int id=0)
        {
            int UserId = (int)Session["UserId"];
            var res = _AffilBll.GetStaffDetailsSessionWise(UserId, session,id);
            var fdata = res.Where(a => a.Appeovalstatus == 2 && a.IsActive == true).ToList();

            int c = 1;
            foreach (var item in res)
            {
                item.slno = c;
                c++;

            }
            int j = 1;
            foreach (var lst in fdata)
            {
                lst.slno = j;
                j++;


            }
            return Json(new { result = res, filt = fdata }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetStaffstatus(string year="",int course=0,int quarter=0, int tabId=0)
        {
            int UserId = (int)Session["UserId"];
            int roleid = (int)Session["RoleId"];
            //int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
            var res = _AffilBll.ListstaffDetails(UserId).OrderByDescending(a=> a.CreatedOn).ToList();
            //(objAdmissionApplicantsDistLogin.ApplicantType != 0 ? tssm.ApplicantType == objAdmissionApplicantsDistLogin.ApplicantType : true)
            res?.ForEach(x =>
            {
                x.IsAction = false;
                if (x.ApprovalFlowId == roleid)
                    x.IsAction = true;
            });
            res = res?.Where(a => a.StatusName.Length > 4).ToList();
            if (year != "")
                res = res?.Where(x => x.Year == year).ToList();
            if (course != 0)
                res = res?.Where(x => x.Courseid == course).ToList();
            if (quarter != 0)
                res = res?.Where(x => x.Quarter == quarter).ToList();
            if (tabId==3)
                res = res?.Where(x => x.Appeovalstatus == (int)CsystemType.getCommon.Approved).ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        private List<StaffInstituteDetails> FilterData(int userId,int? iniId=0,string year="")
        {
            //here userid is being used for to identify instituteid when institute wise login
            var res = _AffilBll.ListstaffDetails(userId, iniId).OrderByDescending(a => a.CreatedOn).ToList(); ;
            if (year != "")
                res = res?.Where(x => x.Year == year).ToList();
            return res;
        }
        public ActionResult Viewstaffhistory(int id, int session ,int quarter)
        {
            //int loginId = Convert.ToInt32(Session["LoginId"].ToString());
            var res = _AffilBll.ViewStaffhistory(id, session, quarter);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStaffDetailsForInstitute(int id,string year="")
        {
            //int loginId = Convert.ToInt32(Session["LoginId"].ToString());
            int UserId = (int)Session["UserId"];
            //int college_id = _AffilBll.GetAffiliationInstituteIdBLL(UserId);
            //var res = _AffilBll.GetstaffDetails(UserId);
            var res = FilterData(userId: UserId, iniId:id);
            var resultlist = res?.Where(a => a.Appeovalstatus == (int)CsystemType.getCommon.Approved && a.IsActive == true).ToList();

            int c = 1;
            foreach (var item in res)
            {
                item.slno = c;
                c++;

            }
            int d = 1;
            foreach (var item in resultlist)
            {
                item.slno = d;
                d++;

            }
            return Json(new { res, resultlist }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #endregion
    }
}
