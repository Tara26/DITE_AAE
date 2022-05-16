
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Data;
using Models.Admission.MasterDataEntry;
using Models.ExamNotification;
using Models.Admission;
using Models;
using BLL.Admission;
using BLL.ExamNotification;
using log4net;
using Spire.Doc;
using System.Configuration;
using System.Web.Mvc;
using BLL.Admission.MasterDataEntry;
using Spire.Pdf;
using Spire.Pdf.HtmlConverter;
using Spire.Pdf.Graphics;
using System.Threading;
using System.Text;
using System.Web.UI;
using DIT.Utilities;
using System.Reflection;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using BLL.Common;
using BLL.User;
using Models.User;
using DLL.Common;
using System.Web.Script.Serialization;
using static DLL.Common.FileUtility;
using Models.Logs;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using BLL;
using System.Web.Configuration;
using Models.SMS;

namespace DIT.Controllers
{
	[SessionExpire]
	public class AdmissionController : Controller
	{
		private readonly IAffiliationBLL _AffilBll;
		private readonly IMasterDataEntry _masterBll;
		private readonly IAdmissionBLL _admissionBll;
		private readonly INotificationBLL _NotifBll;
		private readonly IAdmissionSeatMatrixBLL _adseatmatBll;
		private IUserBLL _LoginService;
		private readonly ICommonBLL _CommonBLL;
		private static readonly ILog Log = LogManager.GetLogger(typeof(AdmissionController));
		Encryption encryption = new Encryption();
		string ErrorLogpath = ConfigurationManager.AppSettings["logPath"].ToString();
		Errorhandling mobjErrorLog = new Errorhandling();
		Audittraillog audittrail = new Audittraillog();
		private readonly string UploadFolder = ConfigurationManager.AppSettings["TemplateDocumentsPath"];
		private readonly string UploadWordFolder = ConfigurationManager.AppSettings["WordTemplateDocumentsPath"];

		// strings to be replaced in template
		private readonly string ReplaceNoticeNumberKannada = "&lt;ಅಧಿಸೂಚನೆ ಸಂ.&gt;";
		private readonly string ReplaceNoticeDateKannada = "&lt;ಅಧಿಸೂಚನೆ ದಿನಾಂಕ&gt;";
		private readonly string ReplaceFeesLastDateKannada = "&lt;ಕೊನೆಯ ದಿನಾಂಕ&gt;";

		private readonly string ReplaceNoticeNumberEnglish = "&lt;Notication Number&gt;";
		private readonly string ReplaceNoticeDateEnglish = "&lt;Notication Date&gt;";
		private readonly string ReplaceApplyDateFrom = "&lt;From Date&gt;";
		private readonly string ReplaceApplyDateTo = "&lt;To Date&gt;";
		private readonly string ReplaceDocVerifyDateFrom = "&lt;From DocVerify Date&gt;";
		private readonly string ReplaceDocVerfyDateTo = "&lt;To DocVerify Date&gt;";

		private readonly string ReplaceDisplayEligibilityDate = "&lt;ReplaceDisplayEligibilityDate&gt;";
		private readonly string ReplaceDatabasebackupDate = "&lt;ReplaceDatabasebackupDate&gt;";
		private readonly string ReplaceTentativeDate = "&lt;ReplaceTentativeDate&gt;";
		private readonly string ReplacefinalDate = "&lt;ReplacefinalDate&gt;";
		private readonly string listofseatallotment = "&lt;listofseatallotment&gt;";
		private readonly string FirstroundadmissionprocessFrom = "&lt;FirstroundadmissionprocessFrom&gt;";
		private readonly string FirstroundadmissionprocessTo = "&lt;FirstroundadmissionprocessTo&gt;";
		private readonly string Firstlistofadmittedcandidates = "&lt;Firstlistofadmittedcandidates&gt;";

		private readonly string FromDt2ndRoundEntryChoiceTrade = "&lt;FromDt2ndRoundEntryChoiceTrade&gt;";
		private readonly string ToDt2ndRoundEntryChoiceTrade = "&lt;ToDt2ndRoundEntryChoiceTrade&gt;";
		private readonly string FromDtDbBkp2ndRoundOnlineSeat = "&lt;FromDtDbBkp2ndRoundOnlineSeat&gt;";
		private readonly string ToDtDbBkp2ndRoundOnlineSeat = "&lt;ToDtDbBkp2ndRoundOnlineSeat&gt;";
		private readonly string FromDt2ndRoundAdmissionProcess = "&lt;FromDt2ndRoundAdmissionProcess&gt;";
		private readonly string ToFDt2ndRoundAdmissionProcess = "&lt;ToFDt2ndRoundAdmissionProcess&gt;";
		private readonly string Dt2ndAdmittedCand = "&lt;Dt2ndAdmittedCand&gt;";
		private readonly string FromDt3rdRoundEntryChoiceTrade = "&lt;FromDt3rdRoundEntryChoiceTrade&gt;";
		private readonly string ToDt3rdRoundEntryChoiceTrade = "&lt;ToDt3rdRoundEntryChoiceTrade&gt;";
		private readonly string DtDbBkp3rdRoundOnlineSeat = "&lt;DtDbBkp3rdRoundOnlineSeat&gt;";
		private readonly string FromDt3rdRoundAdmissionProcess = "&lt;FromDt3rdRoundAdmissionProcess&gt;";
		private readonly string ToDt3rdRoundAdmissionProcess = "&lt;ToDt3rdRoundAdmissionProcess&gt;";
		private readonly string Dt3rdAdmittedCand = "&lt;Dt3rdAdmittedCand&gt;";
		private readonly string FromDtFinalRoundEntryChoiceTrade = "&lt;FromDtFinalRoundEntryChoiceTrade&gt;";
		private readonly string ToDtFinalRoundEntryChoiceTrade = "&lt;ToDtFinalRoundEntryChoiceTrade&gt;";
		private readonly string DtFinalRoundSeatAllotment = "&lt;DtFinalRoundSeatAllotment&gt;";
		private readonly string FromDtAdmissionFinalRound = "&lt;FromDtAdmissionFinalRound&gt;";
		private readonly string ToDtAdmissionFinalRound = "&lt;ToDtAdmissionFinalRound&gt;";
		private readonly string DtCommencementOfTraining = "&lt;DtCommencementOfTraining&gt;";




		// strings to generate file name Calendar Events
		private readonly string PdfFileNameFormat1 = "CalendarNotification_{0}.pdf";
		private readonly string DocFileNameFormat1 = "CalendarNotification_{0}.docx";
		private readonly string HTMLFilename1 = "CalendarNotification_{0}.html";

		//strings to generate file name
		private readonly string PdfFileNameFormat = "Notification_{0}.pdf";
		private readonly string DocFileNameFormat = "Notification_{0}.docx";
		private readonly string HTMLFilename = "Notification_{0}.html";
		public AdmissionController()
		{
			this._LoginService = new UserBLL();
			this._masterBll = new MasterDataEntry();
			_NotifBll = new NotificationBLL();
			_admissionBll = new AdmissionBLL();
			_adseatmatBll = new AdmissionSeatMatrixBLL();
			_CommonBLL = new CommonBLL();

			_AffilBll = new AffiliationBLL();
		}
		// GET: Admission
		public ActionResult AdmissionHome()
		{
			if (Session["UserId"] != null)
			{
				_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);

				return View();
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ActionResult GetCourseTypes()
		{
			var response = _masterBll.GetCourseTypes();
			return Json(response, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Upload(FormCollection formCollection, int from)
		{
			TableDetails _model = new TableDetails();
			if (Request != null)
			{
				HttpPostedFileBase file = Request.Files["UploadedFile"];
				if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
				{
					string _extension = Path.GetExtension(Request.Files["UploadedFile"].FileName).ToLower();

					string[] _validFileTypes = { ".xls", ".xlsx" };

					string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["UploadedFile"].FileName);
					if (!Directory.Exists(path1))
					{
						Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
					}
					if (_validFileTypes.Contains(_extension))
					{
						_model.FileName = Request.Files["UploadedFile"].FileName;
						if (System.IO.File.Exists(path1))
						{
							System.IO.File.Delete(path1);
						}
						Request.Files["UploadedFile"].SaveAs(path1);
						//DataTable dt1 = null; DataTable dt2 = null;
						//getTables(path1, _extension, out dt1, out dt2);
						//if (dt1 != null && dt1.Rows.Count > 0)
						//    _model.Table = dt1;
						//if (dt2 != null && dt2.Rows.Count > 0)
						//    _model.Table1 = dt2;

						if (_extension.Trim() == ".xls")
						{
							DataTable dt = FileUtility.ConvertXSLXtoDataTable(path1);
							_model.Table = dt;
						}
						else if (_extension.Trim() == ".xlsx")
						{
							DataTable dt = FileUtility.ConvertXSLXtoDataTable(path1);
							foreach (DataRow row in dt.Rows)
							{
								try
								{
									int sm1 = Convert.ToInt32(row.ItemArray[1]);
									int sm2 = Convert.ToInt32(row.ItemArray[3]);
									int sm3 = Convert.ToInt32(row.ItemArray[6]);
									int sm4 = Convert.ToInt32(row.ItemArray[8]);
									int sm5 = Convert.ToInt32(row.ItemArray[10]);
									int sm6 = Convert.ToInt32(row.ItemArray[11]);
									int sm7 = Convert.ToInt32(row.ItemArray[12]);
									int sm8 = Convert.ToInt32(row.ItemArray[13]);
									int sm9 = Convert.ToInt32(row.ItemArray[14]);
									int sm10 = Convert.ToInt32(row.ItemArray[16]);
								}
								catch (Exception e)
								{
									_model.Table1 = dt;
									//break;

								}
								//_model.Table1 = dt;
							}
							if (_model.Table1 == null)
							{
								_model.Table = dt;
							}

						}
					}
					else
					{
						ViewBag.Error = "Please Upload Files in .xls or .xlsx format";

					}
				}
			}
			_model.ScreenId = from;
			if (from == 1)
				return View("../Admission/TradeSeatTransUpload", _model);
			else
				return View("../Admission/TradeSeatTransUpload", _model);
		}

		[Route("Admission/TradeSeatTransUpload")]
		[HttpGet]
		public ActionResult TradeSeatTransUpload()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			TableDetails model = new TableDetails();
			return View(model);
		}


		public string UploadFile(string FileName, int CourseId)
		{
			if (Request != null)
			{
				string _extension = Path.GetExtension(FileName).ToLower();

				string[] _validFileTypes = { ".xls", ".xlsx" };

				if (_validFileTypes.Contains(_extension))
				{
					string path = Server.MapPath("~/Content/Uploads/" + FileName);
					DataTable dt = new DataTable();

					if (_extension.Trim() == ".xls")
					{
						dt = FileUtility.ConvertXSLXtoDataTable(path);
					}
					else if (_extension.Trim() == ".xlsx")
					{
						dt = FileUtility.ConvertXSLXtoDataTable(path);
					}
					var res = _masterBll.UpdateDetails(dt, CourseId, Convert.ToInt32(Session["LoginId"]));
				}
				else
				{
					ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";

				}
			}
			return "success";
		}


		[HttpGet]
		public ActionResult GetSeatAvailability()
		{
			try
			{
				Log.Info("Entered GetSeatAvailability()");
				var response = _masterBll.GetSeatAvailability();
				Log.Info("Left GetSeatAvailability()");
				return Json(response, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - GetSeatAvailability():" + ex.Message.ToString());
				throw ex;
			}
		}

		public ActionResult GetDivisionTypes()
		{
			try
			{
				Log.Info("Entered GetDivisionTypes()");
				var response = _masterBll.GetDivisionTypes();
				Log.Info("Left GetDivisionTypes()");
				return Json(response, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - GetDivisionTypes():" + ex.Message.ToString());
				throw ex;
			}
		}

		[HttpGet]
		public ActionResult GetDistricts(int divisionId)
		{
			try
			{
				Log.Info("Entered GetDistricts()");
				var response = _masterBll.GetDistricts(divisionId);
				Log.Info("Left GetDistricts()");
				return Json(response, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - GetDistricts():" + ex.Message.ToString());
				throw ex;
			}
		}

		public ActionResult GetTaluks(int districtId)
		{
			try
			{
				Log.Info("Entered districtId()");
				var response = _masterBll.GetTaluks(districtId);
				Log.Info("Left districtId()");
				return Json(response, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - districtId():" + ex.Message.ToString());
				throw ex;
			}
		}

		public ActionResult GetAailableSeats()
		{
			try
			{
				Log.Info("Entered GetAailableSeats()");
				var response = _masterBll.GetAailableSeats();
				Log.Info("Left GetAailableSeats()");
				return Json(response, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - GetAailableSeats():" + ex.Message.ToString());
				throw ex;
			}
		}


		public ActionResult GetDivisions()
		{
			var Division_list = _admissionBll.GetDivisionListBLL();//.GetDistrictbasedonDivisionIDBLL();
			return Json(Division_list, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetDistrictList(int Divisions)
		{
			var District_list = _admissionBll.GetDistrictListBLL(Divisions);//.GetDistrictbasedonDivisionIDBLL();
			return Json(District_list, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetTalukList(int Districts)
		{
			var Taluk_list = _admissionBll.GetTalukListBLL(Districts);//.GetDistrictbasedonDivisionIDBLL();
			return Json(Taluk_list, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetRoleList()
		{
			var Course_list = _admissionBll.GetRoles(Convert.ToInt32(Session["RoleId"].ToString()), 1);
			return Json(Course_list, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetGridDetails(int CourseTypes, int Divisions, int Districts, int Talukas)
		{
			var Grid_list = _admissionBll.GetGridDetailsBLL(CourseTypes, Divisions, Districts, Talukas);
			return Json(Grid_list, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetOnLoadGridDetails()
		{
			int CourseTypes = 0, Divisions = 0, Districts = 0, Talukas = 0;
			var Grid_list = _admissionBll.GetGridDetailsBLL(CourseTypes, Divisions, Districts, Talukas);
			return Json(Grid_list, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetAllData()
		{
			var Course_list = _admissionBll.GetRoleListBLL();
			return Json(Course_list, JsonRequestBehavior.AllowGet);
		}

		public ActionResult SeatAvailability_DD(ExamCalendarMaster model)
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			model.user_id = Convert.ToInt32(Session["LoginId"]);
			//model.CourseTypeList = _NotifBll.GetCourseListBLL();
			//model.ExamSemList = _NotifBll.GetExamSemListBLL();
			//model.ExamTypeList = _NotifBll.GetExamTypeListBLL();
			//model.SubjectList = _NotifBll.GetSubjectListBLL();
			//model.SubjectTypeList = _NotifBll.GetSubjectTypeListBLL();
			//model.TradeList = _NotifBll.GetTradeListBLL();
			//model.TradeTypeList = _NotifBll.GetTradeTypeListBLL();
			//model.TradeYearList = _NotifBll.GetTradeYearListBLL();
			//model.NotificationForApprovalList = _NotifBll.GetNotificationForApprovalBLL(model).ToList();
			//model.LoginRoleList = _NotifBll.GetLoginRoleListBLL();
			//ViewBag.mySkills = _NotifBll.GetSubjectListBLL();

			//if (model.CourseName != null || model.ExamSemName != null || model.ExamTypeName != null
			//    || model.SubjectName != null || model.SubjectTypeName != null || model.TradeName != null || model.TradeTypeName != null)
			//{

			//    var res = _NotifBll.SaveExamCalNotificationBLL(model);
			//}
			//model.selectTab = 2;
			return View(model);
		}

		// GET: Admission
		public ActionResult Index()
		{
			if (Session["UserId"] != null)
			{
				_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
				return View();
			}
			return RedirectToAction("Index", "Home");
		}

		public ActionResult TradeSeatTransITIAdmin(SeatAvailabilityMaster modal)
		{

			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			List<SeatAvailabilityMaster> list = new List<SeatAvailabilityMaster>();
			list = _admissionBll.GetAdmissionSeatAvailList(modal);
			modal.seatAvailabilityMaster = list;
			return View(modal);
		}

		public JsonResult GetTradeNameTypes()
		{
			var TradeName_list = _admissionBll.GetTradeNameListBLL();
			return Json(TradeName_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetUnitTradeTypes()
		{
			var UnitTrade_list = _admissionBll.GetUnitTradeListBLL();
			return Json(UnitTrade_list, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetTradeIsPPPTypes()
		{
			var TradeIsPPP_list = _admissionBll.GetTradeIsPPPListBLL();
			return Json(TradeIsPPP_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetSysTrainingTypes()
		{
			var TradeIsPPP_list = _admissionBll.GetSysTrainingListBLL();
			return Json(TradeIsPPP_list, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult InsertTradeSeat(insertRecordsForTrade model)
		{
			try
			{
				_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
				Log.Info("Entered InsertTradeSeat()");
				var srList = _admissionBll.InsertTradeseatMasterBLL(model);
				srList = _admissionBll.InsertTradeseatTranseBLL(model);
				Log.Info("Left InsertTradeSeat()");
				return Json(srList, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - TradeSeatTransJD():" + ex.Message.ToString());
				throw ex;
			}

		}
		public ActionResult TradeSeatTransJD(SeatAvailabilityMaster model)
		{

			try
			{
				_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
				Log.Info("Entered TradeSeatTransJD()");
				SeatAvailabilityMaster modal = new SeatAvailabilityMaster();
				List<SeatAvailabilityMaster> list = _admissionBll.GetAdmissionSeatAvailListBLL(modal).ToList();
				modal.StatusList = _admissionBll.GetStatusListBLL();
				modal.seatAvailabilityMaster = list;
				Log.Info("Left TradeSeatTransJD()");
				return View(modal);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - TradeSeatTransJD():" + ex.Message.ToString());
				throw ex;
			}


		}

		[HttpPost]
		public ActionResult GetSeatList(int CourseTypes, int Divisions, int Districts, int Talukas)
		{
			try
			{
				Log.Info("Entered GetSeatList()");
				List<SeatAvailabilityMaster> Seat_list = new List<SeatAvailabilityMaster>();
				Seat_list = _admissionBll.GetSeatListBLL(CourseTypes, Divisions, Districts, Talukas);
				int c = 1;
				foreach (var x in Seat_list)
				{
					x.SlNo = c++;
				}
				Log.Info("Left GetSeatList()");
				return Json(Seat_list, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - GetSeatList():" + ex.Message.ToString());
				throw ex;
			}
		}

		public ActionResult GetStatusList()
		{
			try
			{
				Log.Info("Entered GetStatusList()");
				List<SelectListItem> responseList = _admissionBll.GetAllStatusBasedOnUserBLL();
				var role_id = Convert.ToInt32(Session["RoleId"]);

				if (role_id == 2)// Director
					responseList.RemoveAll(t => t.Value != "2" && t.Value != "4" && t.Value != "7");
				else if (role_id == 1)    // Commissioner
					responseList.RemoveAll(t => t.Value != "2" && t.Value != "4");
				else if (role_id == 5)    // Deputy Director
					responseList.RemoveAll(t => t.Value != "5");
				else      //Apart from DD, Director, Commissioner
					responseList.RemoveAll(t => t.Value != "4" && t.Value != "7");

				return Json(responseList, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - GetStatusList():" + ex.Message.ToString());
				throw ex;
			}
		}

		public ActionResult GetSeatListById(string ListId)
		{
			try
			{
				Log.Info("Entered GetSeatListById()");
				var response = _admissionBll.GetSeatListByIdBLL(ListId);
				Log.Info("Left GetSeatListById()");
				return Json(response, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - GetSeatListById():" + ex.Message.ToString());
				throw ex;
			}
		}
		public ActionResult UpdateRemarksDetailsForSeat(string Remarks, int Status, int TradeITIseatidID)
		{
			try
			{
				_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
				Log.Info("Entered UpdateRemarksDetailsForSeat()");
				_admissionBll.UpdateRemarksDetailsForSeatBLL(Remarks, Status, TradeITIseatidID);
				List<SeatAvailabilityMaster> Seat_list = new List<SeatAvailabilityMaster>();
				Seat_list = _admissionBll.GetSeatListBLL();
				int c = 1;
				foreach (var x in Seat_list)
				{
					x.SlNo = c++;
				}
				Log.Info("Left UpdateRemarksDetailsForSeat()");
				return Json(Seat_list, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - UpdateRemarksDetailsForSeat():" + ex.Message.ToString());
				throw ex;
			}
		}

		public ActionResult GetStatusListById(int TradeITISeatid)
		{
			try
			{
				Log.Info("Entered GetStatusListById()");
				var StatusListById = _admissionBll.GetStatusListByIdBLL(TradeITISeatid);
				Log.Info("Left GetStatusListById()");
				return Json(StatusListById, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - GetStatusListById():" + ex.Message.ToString());
				throw ex;
			}
		}

		#region admission notification        
		private string ReadWordFile(string wordFile)
		{
			string result = string.Empty;
			try
			{
				if (System.IO.File.Exists(wordFile))
				{
					//Doc save
					Spire.Doc.Document doc = new Spire.Doc.Document(wordFile, Spire.Doc.FileFormat.Docx);
					string htmlFilePath = wordFile.Replace("docx", "html"); // changed docx to doc
					doc.SaveToFile(htmlFilePath, Spire.Doc.FileFormat.Html);

					// //Read the saved Html File.
					string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString());
					System.IO.File.Delete(htmlFilePath.ToString());

					// delete temparary folder generated while saving as HTML
					string folderPath = htmlFilePath as string;
					folderPath = folderPath.Replace(".html", "_files");
					if (System.IO.Directory.Exists(folderPath))
					{
						System.IO.Directory.Delete(folderPath, true);
					}
					doc.Close();
					result = wordHTML;
				}

			}
			catch (Exception ex)
			{

			}
			return result;
		}
		public ActionResult CreateAdmissionNotification(int? notificationId)
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			AdmissionNotification model = new AdmissionNotification();
			string content = string.Empty;
			string notDesc = "Notification Date: ";

			model.Exam_notif_date = DateTime.Now;
			int status;
			int id = Convert.ToInt32(Session["LoginId"].ToString());
			if (notificationId != null)
			{
				//content = "<p style=\"text-align: center; \">Government of Karnataka</p><p>Notification Date: &lt;Notification Date&gt;</p><p style=\"text-align: right;\">Notification Number : &lt;Notification Number&gt;</p><p>&nbsp;</p><p style=\"text-align: center;\">NOTIFICATION</p><br>";
				model = _admissionBll.GetUpdateNotificationBLL(id, notificationId)[0];
				string filename = Path.ChangeExtension(model.Admsn_notif_doc, ".docx");
				model.SavePath = filename;
				//content = ReadWordFile(wordpath);
				//sb.Append(content);
				//sb.Replace(content.Substring(content.IndexOf(notDesc) + notDesc.Length, 10), model.Exam_notif_date.ToString("dd:MM:yyyy"));
				//content = sb.ToString();
				status = _admissionBll.GetNotificationStatus(notificationId);
			}
			else
			{
				status = 0;
				//Kannada template
				content = "<p style=\"text-align: center; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಕರ್ನಾಟಕ ಸರ್ಕಾರ </p><p style=\"font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ದಿನಾಂಕ : &lt;ಅಧಿಸೂಚನೆ ದಿನಾಂಕ&gt;</p><p style=\"text-align: right; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ ಸಂ.: &lt;ಅಧಿಸೂಚನೆ ಸಂ.&gt;</p><p>&nbsp;</p><p style=\"text-align: center;font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ</p><br>";
				notificationId = 0;
			}
			//model.content = content;
			model.StatusId = status;
			//read the notification detail dropdowns            
			model.CourseList = _NotifBll.GetCourseListBLL();
			model.ApplicantTypeList = _NotifBll.GetApplicantTypeListBLL();
			model.DeptList = _admissionBll.GetDepartmentListDLL();
			model.NotifDescList = _NotifBll.GetNotificationDescListBLL();
			if (notificationId != null)
			{
				model.selectTab = 0;
			}
			else
			{
				model.selectTab = 1;
			}
			return View(model);
		}
		[HttpPost]
		public ActionResult CreateAdmissionNotification(AdmissionNotification model)
		{
			//Utilities.Security.ValidateRequestHeader(Request);
			model.role_id = Convert.ToInt32(Session["RoleId"].ToString());
			model.user_id = Convert.ToInt32(Session["UserId"].ToString());
			model.CourseList = _NotifBll.GetCourseListBLL();
			model.ApplicantTypeList = _NotifBll.GetApplicantTypeListBLL();
			model.DeptList = _NotifBll.GetDepartmentListBLL();
			model.NotifDescList = _NotifBll.GetNotificationDescListBLL();
			if (TempData["Admission_Notif_Id"] != null)
				model.Admission_Notif_Id = Convert.ToInt32(TempData["Admission_Notif_Id"].ToString());

			if (model.file.ContentLength > 0)
			{
				List<SelectListItem> notifDescr = (List<SelectListItem>)model.NotifDescList.Items;
				model.NotifDescName = notifDescr[model.NotifDescId].Text;
				model.toPDF = true;
				string filename = _admissionBll.ConvertUploadedAdmsnNotifToPDFBLL(model, PdfFileNameFormat, DocFileNameFormat, Request.PhysicalApplicationPath + UploadFolder);
				model.DeptId = 1;
				model.SavePath = string.Format(UploadFolder + PdfFileNameFormat, filename);
				var saved = _admissionBll.CreateAdmissionNotificationDetailsBLL(model);
				TempData["Saved"] = saved;
			}
			else
			{
				TempData["Saved"] = "Invalid";
			}
			return View(model);
		}
		[HttpPost]
		public ActionResult UploadNotifDoc(AdmissionNotification model)
		{
			model.role_id = Convert.ToInt32(Session["RoleId"].ToString());
			string filename = _admissionBll.ConvertUploadedAdmsnNotifToPDFBLL(model, PdfFileNameFormat, DocFileNameFormat, Request.PhysicalApplicationPath + UploadFolder);
			model.SavePath = string.Format(UploadFolder + PdfFileNameFormat, filename);
			var saved = _admissionBll.CreateAdmissionNotificationDetailsBLL(model);
			return Json(saved, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		public ActionResult SaveDraftAdmissionNotification(AdmissionNotification model)
		{
			Utilities.Security.ValidateRequestHeader(Request);
			model.role_id = Convert.ToInt32(Session["RoleId"]);
			model.user_id = Convert.ToInt32(Session["UserId"]);
			model.CourseList = _NotifBll.GetCourseListBLL();
			model.ApplicantTypeList = _NotifBll.GetApplicantTypeListBLL();
			model.DeptList = _NotifBll.GetDepartmentListBLL();
			model.NotifDescList = _NotifBll.GetNotificationDescListBLL();
			if (TempData["Admission_Notif_Id"] != null)
				model.Admission_Notif_Id = Convert.ToInt32(TempData["Admission_Notif_Id"].ToString());
			if (ModelState.IsValid)
			{
				List<SelectListItem> notifDescr = (List<SelectListItem>)model.NotifDescList.Items;
				model.NotifDescName = notifDescr[model.NotifDescId].Text;

				// create the folder directory wherer notifcation are stored if it doesn't exists
				string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

				if (!Directory.Exists(DocumentsFolder))
				{
					Directory.CreateDirectory(DocumentsFolder);
				}
				// replace 
				string filename = model.Exam_Notif_Number.Replace("/", "_");
				filename = filename.Replace("-", "_");

				// get full file path
				string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat, filename);
				string wordpath = string.Format(DocumentsFolder + DocFileNameFormat, filename);
				string htmlpath = string.Format(DocumentsFolder + HTMLFilename, filename);//vidhya Changes


				// replace the date amd notification number notification data
				model.content = ReplaceDataInTemplate(model.content, model);

				//Doc save
				Spire.Doc.Document doc = new Spire.Doc.Document();
				doc.AddSection();
				Spire.Doc.Documents.Paragraph para = doc.Sections[0].AddParagraph();
				para.AppendHTML(model.content);
				doc.SaveToFile(wordpath, Spire.Doc.FileFormat.Docx);
				doc.SaveToFile(htmlpath, Spire.Doc.FileFormat.Html);//vidhya Changes


				PdfDocument pdf = new PdfDocument();
				PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat
				{
					Layout = PdfLayoutType.Paginate,
					FitToPage = Clip.Width,
					LoadHtmlTimeout = 60 * 1000
				};
				htmlLayoutFormat.IsWaiting = true;
				PdfPageSettings setting = new PdfPageSettings();
				setting.Size = PdfPageSize.A4;
				Thread thread = new Thread(() => { pdf.LoadFromHTML(htmlpath, true, true, true); });
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
				pdf.SaveToFile(pdfpath, Spire.Pdf.FileFormat.PDF);

				if (System.IO.File.Exists(htmlpath))
				{
					System.IO.File.Delete(htmlpath);
				}
				string folderPath = htmlpath.Replace(".html", "_files");
				if (System.IO.Directory.Exists(folderPath))
				{
					System.IO.Directory.Delete(folderPath, true);
				}

				doc.Close();
				// remove the word document post creation of respective pdf
				//if (System.IO.File.Exists(wordpath))
				//{
				//    System.IO.File.Delete(wordpath);
				//}

				// now save the notification details to database
				model.SavePath = string.Format(UploadFolder + PdfFileNameFormat, filename);
				model.DeptId = 2;
				var saved = _admissionBll.CreateAdmissionNotificationDetailsBLL(model);
				if (saved == "Saved")
					TempData["Saved"] = "Draft";
				else
					TempData["Saved"] = saved;
			}
			else
			{
				TempData["Saved"] = "Invalid";
			}
			return View("CreateAdmissionNotification", model);
		}
		[HttpPost]
		public ActionResult UpdateAdmissionNotification(AdmissionNotification model)
		{
			Utilities.Security.ValidateRequestHeader(Request);
			model.role_id = Convert.ToInt32(Session["RoleId"]);
			model.user_id = Convert.ToInt32(Session["UserId"]);
			model.CourseList = _NotifBll.GetCourseListBLL();
			model.ApplicantTypeList = _NotifBll.GetApplicantTypeListBLL();
			model.DeptList = _NotifBll.GetDepartmentListBLL();
			model.NotifDescList = _NotifBll.GetNotificationDescListBLL();
			if (TempData["Admission_Notif_Id"] != null)
				model.Admission_Notif_Id = Convert.ToInt32(TempData["Admission_Notif_Id"].ToString());
			if (ModelState.IsValid)
			{
				List<SelectListItem> notifDescr = (List<SelectListItem>)model.NotifDescList.Items;
				model.NotifDescName = notifDescr[model.NotifDescId].Text;

				// create the folder directory wherer notifcation are stored if it doesn't exists
				string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

				if (!Directory.Exists(DocumentsFolder))
				{
					Directory.CreateDirectory(DocumentsFolder);
				}
				// replace 
				string filename = model.Exam_Notif_Number.Replace("/", "_");
				filename = filename.Replace("-", "_");

				// get full file path
				string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat, filename);
				string wordpath = string.Format(DocumentsFolder + DocFileNameFormat, filename);
				string htmlpath = string.Format(DocumentsFolder + HTMLFilename, filename);//vidhya Changes


				// replace the date amd notification number notification data
				model.content = ReplaceDataInTemplate(model.content, model);

				//Doc save
				Spire.Doc.Document doc = new Spire.Doc.Document();
				doc.AddSection();
				Spire.Doc.Documents.Paragraph para = doc.Sections[0].AddParagraph();
				para.AppendHTML(model.content);
				doc.SaveToFile(wordpath, Spire.Doc.FileFormat.Docx);
				doc.SaveToFile(htmlpath, Spire.Doc.FileFormat.Html);//vidhya Changes


				PdfDocument pdf = new PdfDocument();
				PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat
				{
					Layout = PdfLayoutType.Paginate,
					FitToPage = Clip.Width,
					LoadHtmlTimeout = 60 * 1000
				};
				htmlLayoutFormat.IsWaiting = true;
				PdfPageSettings setting = new PdfPageSettings();
				setting.Size = PdfPageSize.A4;
				Thread thread = new Thread(() => { pdf.LoadFromHTML(htmlpath, true, true, true); });
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
				pdf.SaveToFile(pdfpath, Spire.Pdf.FileFormat.PDF);

				if (System.IO.File.Exists(htmlpath))
				{
					System.IO.File.Delete(htmlpath);
				}
				string folderPath = htmlpath.Replace(".html", "_files");
				if (System.IO.Directory.Exists(folderPath))
				{
					System.IO.Directory.Delete(folderPath, true);
				}

				doc.Close();
				// remove the word document post creation of respective pdf
				//if (System.IO.File.Exists(wordpath))
				//{
				//    System.IO.File.Delete(wordpath);
				//}

				// now save the notification details to database
				model.SavePath = string.Format(UploadFolder + PdfFileNameFormat, filename);
				model.DeptId = 3;
				var saved = _admissionBll.CreateAdmissionNotificationDetailsBLL(model);
				TempData["Saved"] = "Updated";
			}
			else
			{
				TempData["Saved"] = "Invalid";
			}
			return View("CreateAdmissionNotification", model);
		}
		public JsonResult ViewAdmissionNotification(int id)
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			var res = _admissionBll.GetUpdateNotificationBLL(Convert.ToInt32(Session["LoginId"].ToString()), id);
			int x = 1;
			foreach (var item in res)
			{
				item.slno = x;
				if (item.slno == res.Count)
				{
					item.toPDF = true;
				}
				x++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetAdmissionNotification()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			var Remarks = _admissionBll.GetAdmissionNotification(Convert.ToInt32(Session["RoleId"].ToString()));
			int x = 1;
			foreach (var item in Remarks)
			{
				item.slno = x;
				x++;
			}



			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetComments(int id)
		{
			List<Notification> Remarks = new List<Notification>();
			Remarks = _admissionBll.GetComments(id);
			int x = 1;
			foreach (var item in Remarks)
			{
				item.SlNo = x;
				x++;
			}
			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetAdmissionNotificationDetails(int id)
		{
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var Response = _admissionBll.GetAdmissionNotificationDetails(id, roleId);
			Response.RoleId = roleId;
			if (Response.ForwardId == roleId)
			{
				Response.forwardStatus = true;
			}
			if (roleId <= 7)
			{
				//Response.ApprovedStatus = true;
				Response.ChangesStatus = true;
			}
			else
			{
				//Response.ApprovedStatus = false;
				Response.ChangesStatus = false;
			}
			if (roleId == 1 || roleId == 2 || roleId == 4 || roleId == 6)
			{
				Response.ApprovedStatus = true;
			}
			else
			{
				Response.ApprovedStatus = false;
			}

			if (Response.StatusId == 109 || Response.StatusId == 103)
			{
				Response.ApprovedStatus = false;
			}

			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetRoles(int level)
		{
			var Response = _admissionBll.GetRoles(Convert.ToInt32(Session["RoleId"].ToString()), level);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ForwardAdmissionNotification(int id, int admiNotifId, string remarks, string filePathName)
		{
			var Response = _admissionBll.ForwardAdmissionNotification(Convert.ToInt32(Session["UserId"].ToString()), id, admiNotifId, remarks, Convert.ToInt32(Session["RoleId"].ToString()), filePathName);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult SendbackAdmissionNotification(int id, int admiNotifId, string remarks)
		{
			var Response = _admissionBll.SendbackAdmissionNotification(Convert.ToInt32(Session["UserId"].ToString()), id, admiNotifId, remarks, Convert.ToInt32(Session["RoleId"].ToString()));
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ApproveAdmissionNotification(int id, int admiNotifId, string remarks)
		{
			var res = _admissionBll.GetUpdateNotificationBLL(Convert.ToInt32(Session["LoginId"].ToString()), admiNotifId);
			string wordpath = res.Where(a => a.SavePath != "").OrderByDescending(a => a.admissionNotificationId).Select(a => a.SavePath).FirstOrDefault();
			string pdfpath = Request.PhysicalApplicationPath + Path.ChangeExtension(wordpath, ".pdf");
			wordpath = Request.PhysicalApplicationPath + wordpath;
			string SavePath = _admissionBll.ConvertWordToPDF(true, wordpath, pdfpath);
			SavePath = UploadFolder + Path.GetFileName(SavePath);
			var Response = _admissionBll.ApproveAdmissionNotification(Convert.ToInt32(Session["UserId"].ToString()), id, admiNotifId, remarks, Convert.ToInt32(Session["RoleId"].ToString()), SavePath);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ChangesAdmissionNotification(int id, int admiNotifId, string remarks)
		{
			var Response = _admissionBll.ChangesAdmissionNotification(Convert.ToInt32(Session["UserId"].ToString()), id, admiNotifId, remarks, Convert.ToInt32(Session["RoleId"].ToString()));
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult PublishNotification(int notificationId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.PublishNotification(notificationId, loginId, Convert.ToInt32(Session["RoleId"].ToString()));
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult AdmissionNotificationBox()
		{

			var res = _admissionBll.GetAdmissionNotificationBox();
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		#endregion

		// function to replace the strings
		private string ReplaceDataInTemplate(string content, AdmissionNotification model)
		{
			content = content.Replace("&lt;Course Type&gt;", model.CourseList.Where(a => a.Value == model.CourseTypeId.ToString()).Select(a => a.Text).FirstOrDefault());
			content = content.Replace("&lt;Notication Desc&gt;", model.NotifDesc);

			content = content.Replace(ReplaceNoticeDateKannada, model.Exam_notif_date.ToString("dd:MM:yyyy"));
			content = content.Replace(ReplaceNoticeNumberKannada, model.Exam_Notif_Number);

			content = content.Replace(ReplaceNoticeDateEnglish, model.Exam_notif_date.ToString("dd:MM:yyyy"));
			content = content.Replace(ReplaceNoticeNumberEnglish, model.Exam_Notif_Number);

			content = content.Replace("&lt;Applicant Type&gt;", model.ApplicantTypeList.Where(a => a.Value == model.applicantTypeId.ToString()).Select(a => a.Text).FirstOrDefault());

			return content;
		}
		private string ReplaceDataInTemplateCal(string content, AdmissionNotification model)
		{
			model.Exam_notif_date = model.Notification_Date ?? DateTime.Now;
			model.FromDt_ApplyingOnlineApplicationForm_pdf = model.FromDt_ApplyingOnlineApplicationForm ?? DateTime.Now;
			model.ToDt_ApplyingOnlineApplicationForm_pdf = model.ToDt_ApplyingOnlineApplicationForm ?? DateTime.Now;
			model.FromDt_DocVerificationPeriod_pdf = model.FromDt_DocVerificationPeriod ?? DateTime.Now;
			model.ToDt_DocVerificationPeriod_pdf = model.ToDt_DocVerificationPeriod ?? DateTime.Now;
			model.Dt_DisplayEigibleVerifiedlist_pdf = model.Dt_DisplayEigibleVerifiedlist ?? DateTime.Now;
			model.Dt_DBBackupSeatMatrixFInalByDept_pdf = model.Dt_DBBackupSeatMatrixFInalByDept ?? DateTime.Now;
			model.Dt_DisplayTentativeGradation_pdf = model.Dt_DisplayTentativeGradation ?? DateTime.Now;
			model.Dt_DisplayFinalGradationList_pdf = model.Dt_DisplayFinalGradationList ?? DateTime.Now;
			model.Dt_1stListSeatAllotment_pdf = model.Dt_1stListSeatAllotment ?? DateTime.Now;
			model.Dt_1stListAdmittedCand_pdf = model.Dt_1stListAdmittedCand ?? DateTime.Now;
			model.FromDt_1stRoundAdmissionProcess_pdf = model.FromDt_1stRoundAdmissionProcess ?? DateTime.Now;
			model.ToDt_1stRoundAdmissionProcess_pdf = model.ToDt_1stRoundAdmissionProcess ?? DateTime.Now;

			/////////
			//model.Dt_1stListAdmittedCand_pdf = model.Dt_1stListAdmittedCand ?? DateTime.Now;
			model.FromDt_2ndRoundEntryChoiceTrade_pdf = model.FromDt_2ndRoundEntryChoiceTrade ?? DateTime.Now;
			model.ToDt_2ndRoundEntryChoiceTrade_pdf = model.ToDt_2ndRoundEntryChoiceTrade ?? DateTime.Now;
			model.FromDt_DbBkp2ndRoundOnlineSeat_pdf = model.FromDt_DbBkp2ndRoundOnlineSeat ?? DateTime.Now;
			model.ToDt_DbBkp2ndRoundOnlineSeat_pdf = model.ToDt_DbBkp2ndRoundOnlineSeat ?? DateTime.Now;
			model.FromDt_2ndRoundAdmissionProcess_pdf = model.FromDt_2ndRoundAdmissionProcess ?? DateTime.Now;
			model.ToFDt_2ndRoundAdmissionProcess_pdf = model.ToFDt_2ndRoundAdmissionProcess ?? DateTime.Now;
			model.Dt_2ndAdmittedCand_pdf = model.Dt_2ndAdmittedCand ?? DateTime.Now;
			model.FromDt_3rdRoundEntryChoiceTrade_pdf = model.FromDt_3rdRoundEntryChoiceTrade ?? DateTime.Now;
			model.ToDt_3rdRoundEntryChoiceTrade_pdf = model.ToDt_3rdRoundEntryChoiceTrade ?? DateTime.Now;
			model.Dt_DbBkp3rdRoundOnlineSeat_pdf = model.Dt_DbBkp3rdRoundOnlineSeat ?? DateTime.Now;
			model.FromDt_3rdRoundAdmissionProcess_pdf = model.FromDt_3rdRoundAdmissionProcess ?? DateTime.Now;
			model.ToDt_3rdRoundAdmissionProcess_pdf = model.ToDt_3rdRoundAdmissionProcess ?? DateTime.Now;
			model.Dt_3rdAdmittedCand_pdf = model.Dt_3rdAdmittedCand ?? DateTime.Now;
			model.FromDt_FinalRoundEntryChoiceTrade_pdf = model.FromDt_FinalRoundEntryChoiceTrade ?? DateTime.Now;
			model.ToDt_FinalRoundEntryChoiceTrade_pdf = model.ToDt_FinalRoundEntryChoiceTrade ?? DateTime.Now;
			model.Dt_FinalRoundSeatAllotment_pdf = model.Dt_FinalRoundSeatAllotment ?? DateTime.Now;
			model.FromDt_AdmissionFinalRound_pdf = model.FromDt_AdmissionFinalRound ?? DateTime.Now;
			model.ToDt_AdmissionFinalRound_pdf = model.ToDt_AdmissionFinalRound ?? DateTime.Now;
			model.Dt_CommencementOfTraining_pdf = model.Dt_CommencementOfTraining ?? DateTime.Now;

			content = content.Replace(ReplaceNoticeDateEnglish, model.Exam_notif_date.ToString("dd:MM:yyyy"));
			content = content.Replace(ReplaceNoticeNumberEnglish, model.Exam_Notif_Number);
			content = content.Replace(ReplaceApplyDateFrom, model.FromDt_ApplyingOnlineApplicationForm_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ReplaceApplyDateTo, model.ToDt_ApplyingOnlineApplicationForm_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ReplaceDocVerifyDateFrom, model.FromDt_DocVerificationPeriod_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ReplaceDocVerfyDateTo, model.ToDt_DocVerificationPeriod_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ReplaceDisplayEligibilityDate, model.Dt_DisplayEigibleVerifiedlist_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ReplaceDatabasebackupDate, model.Dt_DBBackupSeatMatrixFInalByDept_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ReplaceTentativeDate, model.Dt_DisplayTentativeGradation_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ReplacefinalDate, model.Dt_DisplayFinalGradationList_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(listofseatallotment, model.Dt_1stListSeatAllotment_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(FirstroundadmissionprocessFrom, model.FromDt_1stRoundAdmissionProcess_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(FirstroundadmissionprocessTo, model.ToDt_1stRoundAdmissionProcess_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(Firstlistofadmittedcandidates, model.Dt_1stListAdmittedCand_pdf.ToString("dd:MM:yyyy"));


			//content = content.Replace(Dt1stListAdmittedCand, model.Dt_1stListAdmittedCand_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(FromDt2ndRoundEntryChoiceTrade, model.FromDt_2ndRoundEntryChoiceTrade_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ToDt2ndRoundEntryChoiceTrade, model.ToDt_2ndRoundEntryChoiceTrade_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(FromDtDbBkp2ndRoundOnlineSeat, model.FromDt_DbBkp2ndRoundOnlineSeat_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ToDtDbBkp2ndRoundOnlineSeat, model.ToDt_DbBkp2ndRoundOnlineSeat_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(FromDt2ndRoundAdmissionProcess, model.FromDt_2ndRoundAdmissionProcess_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ToFDt2ndRoundAdmissionProcess, model.ToFDt_2ndRoundAdmissionProcess_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(Dt2ndAdmittedCand, model.Dt_2ndAdmittedCand_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(FromDt3rdRoundEntryChoiceTrade, model.FromDt_3rdRoundEntryChoiceTrade_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ToDt3rdRoundEntryChoiceTrade, model.ToDt_3rdRoundEntryChoiceTrade_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(DtDbBkp3rdRoundOnlineSeat, model.Dt_DbBkp3rdRoundOnlineSeat_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(FromDt3rdRoundAdmissionProcess, model.FromDt_3rdRoundAdmissionProcess_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ToDt3rdRoundAdmissionProcess, model.ToDt_3rdRoundAdmissionProcess_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(Dt3rdAdmittedCand, model.Dt_3rdAdmittedCand_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(FromDtFinalRoundEntryChoiceTrade, model.FromDt_FinalRoundEntryChoiceTrade_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ToDtFinalRoundEntryChoiceTrade, model.ToDt_FinalRoundEntryChoiceTrade_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(DtFinalRoundSeatAllotment, model.Dt_FinalRoundSeatAllotment_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(FromDtAdmissionFinalRound, model.FromDt_AdmissionFinalRound_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(ToDtAdmissionFinalRound, model.ToDt_AdmissionFinalRound_pdf.ToString("dd:MM:yyyy"));
			content = content.Replace(DtCommencementOfTraining, model.Dt_CommencementOfTraining_pdf.ToString("dd:MM:yyyy"));

			return content;
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

		public Microsoft.Office.Interop.Word.Document wordDocument { get; set; }


		#region Calendar18022021
		//Start to 18-02-2021
		private string GetCalendarTextFromPDF(string filepath)
		{
			PdfDocument document = new PdfDocument();
			document.LoadFromFile(Request.PhysicalApplicationPath + filepath);
			StringBuilder content = new StringBuilder();
			content.Append(document.Pages[0].ExtractText());

			return content.ToString();
		}

		public ActionResult AdmissionCalendar(int? calendarId)
		{
			Log.Info("Entered GET AdmissionCalendar()");
			//_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			// create model object  
			AdmissionNotification model = new AdmissionNotification();
			string content = string.Empty;
			model.Notification_Date = DateTime.Now;
			int status;
			//model.FromDt_ApplyingOnlineApplicationForm = DateTime.Now;
			//model.ToDt_ApplyingOnlineApplicationForm = DateTime.Now;
			//model.FromDt_DocVerificationPeriod = DateTime.Now;
			//model.ToDt_DocVerificationPeriod = DateTime.Now;
			//model.Dt_DisplayEigibleVerifiedlist = DateTime.Now;
			//model.Dt_DBBackupSeatMatrixFInalByDept = DateTime.Now;
			//model.Dt_DisplayTentativeGradation = DateTime.Now;
			//model.Dt_DisplayFinalGradationList = DateTime.Now;
			//model.Dt_1stListSeatAllotment = DateTime.Now;
			//model.FromDt_1stRoundAdmissionProcess = DateTime.Now;
			//model.ToDt_1stRoundAdmissionProcess = DateTime.Now;
			//model.Dt_1stListAdmittedCand = DateTime.Now;
			//model.FromDt_2ndRoundEntryChoiceTrade = DateTime.Now;
			//model.ToDt_2ndRoundEntryChoiceTrade = DateTime.Now;
			//model.FromDt_DbBkp2ndRoundOnlineSeat = DateTime.Now;
			//model.ToDt_DbBkp2ndRoundOnlineSeat = DateTime.Now;
			//model.FromDt_2ndRoundAdmissionProcess = DateTime.Now;
			//model.ToFDt_2ndRoundAdmissionProcess = DateTime.Now;
			//model.Dt_2ndAdmittedCand = DateTime.Now;
			//model.FromDt_3rdRoundEntryChoiceTrade = DateTime.Now;
			//model.ToDt_3rdRoundEntryChoiceTrade = DateTime.Now;
			//model.Dt_DbBkp3rdRoundOnlineSeat = DateTime.Now;
			//model.FromDt_3rdRoundAdmissionProcess = DateTime.Now;
			//model.ToDt_3rdRoundAdmissionProcess = DateTime.Now;
			//model.Dt_3rdAdmittedCand = DateTime.Now;
			//model.FromDt_FinalRoundEntryChoiceTrade = DateTime.Now;
			//model.ToDt_FinalRoundEntryChoiceTrade = DateTime.Now;
			//model.Dt_FinalRoundSeatAllotment = DateTime.Now;
			//model.FromDt_AdmissionFinalRound = DateTime.Now;
			//model.ToDt_AdmissionFinalRound = DateTime.Now;
			//model.Dt_CommencementOfTraining = DateTime.Now;
			//Kannada template
			//content = "<p style=\"text-align: center; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಕರ್ನಾಟಕ ಸರ್ಕಾರ </p><p style=\"font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ದಿನಾಂಕ : &lt;ಅಧಿಸೂಚನೆ ದಿನಾಂಕ&gt;</p><p style=\"text-align: right; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ ಸಂ.: &lt;ಅಧಿಸೂಚನೆ ಸಂ.&gt;</p><p>&nbsp;</p><p style=\"text-align: center;font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ</p><br>";
			int id = Convert.ToInt32(Session["RoleId"].ToString());
			if (calendarId != null)
			{
				//content = "<p style=\"text-align: center; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಕರ್ನಾಟಕ ಸರ್ಕಾರ </p><p style=\"font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ದಿನಾಂಕ : &lt;ಅಧಿಸೂಚನೆ ದಿನಾಂಕ&gt;</p><p style=\"text-align: right; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ ಸಂ.: &lt;ಅಧಿಸೂಚನೆ ಸಂ.&gt;</p><p>&nbsp;</p><p style=\"text-align: center;font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ</p><br>";
				content = "<div id=\"PreviewrootPdf\"><div class=\"row\" style=\"margin-left:4%;\"><div class=\"box-body \"><div class=\"col-lg-12\" ><div style=\"text-align:left;font-size: 12px;\" class=\"col-lg-4\">" +
								"<div> <label>E-Mail Id-cet.bangalore@gmail.com</label><br /><label>Kareet.iti.admissions@gmail.com</label><br /><label>WEB-www.emptrg.kar.nic.in</label></div></div>" +
								"<div style=\"text-align:center;\" class=\"col-md-4\"><img src=\"//Content//frontend//images//gok-en-wh-sml.png\" alt=\"gok-logo\" style=\"width:15%;\"></div></div><br /><br /><br />" +
								"<table class=\"table table-bordered table-responsive\" style=\"width:93%;overflow-x: auto;font-size\">" +
								"<caption style=\"background-color:lightblue;\"><center><span> <b>Tentative Calendar of Events for ITI Admission</b></span></center></caption><thead> </thead>" +
								"<tbody>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size:15px\"><label>Sl.No.</label></div><div class=\"col-lg-6\" style=\"font-size:15px\"><label>Description</label></div><div class=\"col-lg-2\" style=\"font-size:15px\"><label>Date</label></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>1</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Date of Notification</label></div><div class=\"col-lg-2\"><span> &lt;Notication Date&gt; </span> </div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>2</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Dates for applying online application form</label></div><div class=\"col-lg-2\"><label>From</label><span id=\"idDaopPreF\">&lt;From Date&gt;</span></div><div class=\"col-lg-2\"><label>To</label> <span id=\" idDaopPreT\">&lt;To Date&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>3</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Document (original) verification of students by officers, counselling the students on their preferences, and correcting their application form andobtaining priority of choice from applicants (As per the slot Based on applicants Number) in 270 Govt I.T.I's</label></div><div class=\"col-lg-2\"><label>From</label><span>&lt;From DocVerify Date&gt;</span></div><div class=\"col-lg-2\"><label>To</label><span>&lt;To DocVerify Date&gt;</span></div></td> </tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>4</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Display of eligible verified list</label></div><div class=\"col-lg-2\"><span>&lt;ReplaceDisplayEligibilityDate&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>5</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Database backup / Process for duplication / SMS integration - testing process of gradation list. Seat matrix finalization by the department </label></div><div class=\"col-lg-2\"><span>&lt;ReplaceDatabasebackupDate&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>6</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Display of tentative gradation</label></div><div class=\"col-lg-2\"><span>&lt;ReplaceTentativeDate&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>7</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Display of final gradation list</label></div><div class=\"col-lg-2\"><span>&lt;ReplacefinalDate&gt;</span></div></td></tr>" +
								"<tr><td> <div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>8</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>1st list of seat allotment </label></div><div class=\"col-lg-2\"><span>&lt;listofseatallotment&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>9</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>First list of admitted candidates and vacancy of seats available for 2nd round.</label></div><div class=\"col-lg-2\"><span>&lt;Firstlistofadmittedcandidates&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>10</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>1st round admission process</label></div><div class=\"col-lg-2\"><label>From</label><span>&lt;FirstroundadmissionprocessFrom&gt;</span></div><div class=\"col-lg-2\"><label>To</label><span>&lt;FirstroundadmissionprocessTo&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>11</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>2nd round Entry choice of trades / ITI in the Portal is compulsory for remaining Seats for those who have applied & not got the seats in First Round & thosewho have applied & allotted but not availed the admission in the first round</label></div><div class=\"col-lg-2\"><label>From</label><span>&lt;FromDt2ndRoundEntryChoiceTrade&gt;</span></div><div class=\"col-lg-2\"><label>To</label><span>&lt;ToDt2ndRoundEntryChoiceTrade&gt;</span></div></td></tr>  " +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>12</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Database backup and 2nd round of online seat allotment </label></div><div class=\"col-lg-2\"><label>From</label><span>&lt;FromDtDbBkp2ndRoundOnlineSeat&gt;</span></div><div class=\"col-lg-2\"><label>To</label><span>&lt;ToDtDbBkp2ndRoundOnlineSeat&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>13</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>2nd round admission Process at ITI's</label></div><div class=\"col-lg-2\"><label>From</label><span>&lt;FromDt2ndRoundAdmissionProcess&gt;</span></div><div class=\"col-lg-2\"><label>To</label><span>&lt;ToFDt2ndRoundAdmissionProcess&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>14</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Second list of admitted candidates and vacancy of seats available for 3rd round</label></div><div class=\"col-lg-2\"><span>&lt;Dt2ndAdmittedCand&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>15</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>3rd round Entry choice of trades / ITI in the Portal is compulsory for remaining Seats for those who have applied & not got the seats in Second Round &those who have applied & allotted but not availed the admission in the Previous round</label></div><div class=\"col-lg-2\"><label>From</label><span>&lt;FromDt3rdRoundEntryChoiceTrade&gt;</span></div><div class=\"col-lg-2\"><label>To</label><span>&lt;ToDt3rdRoundEntryChoiceTrade&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>16</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Database backup and 3rd round of online seat allotment</label></div><div class=\"col-lg-2\"><span>&lt;DtDbBkp3rdRoundOnlineSeat&gt;</span></div> </td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>17</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label> 3rd round admission Process at ITI's</label></div><div class=\"col-lg-2\"><label>From</label><span>&lt;FromDt3rdRoundAdmissionProcess&gt;</span></div><div class=\"col-lg-2\"><label>To</label><span>&lt;ToDt3rdRoundAdmissionProcess&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>18</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>List of candidates admitted in 3rd round and also vacancy List of seats available for final round ( All Seats under General Pool of GM)</label></div><div class=\"col-lg-2\"><span>&lt;Dt3rdAdmittedCand&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>19</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Final round Entry choice of trades / ITI in the Portal is compulsory for remaining Seats for those who have applied & not got the seats in Third Round &those who have applied & allotted but not availed the admission in the Previous round</label></div><div class=\"col-lg-2\"><label>From</label><span>&lt;FromDtFinalRoundEntryChoiceTrade&gt;</span></div><div class=\"col-lg-2\"><label>To</label><span>&lt;ToDtFinalRoundEntryChoiceTrade&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>20</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label> Final round seat allotment</label></div><div class=\"col-lg-2\"><span>&lt;DtFinalRoundSeatAllotment&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>21</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label> Admission Final Round</label></div><div class=\"col-lg-2\"><label>From</label><span>&lt;FromDtAdmissionFinalRound&gt;</span></div><div class=\"col-lg-2\"><label>To</label><span>&lt;ToDtAdmissionFinalRound&gt;</span></div></td></tr>" +
								"<tr><td><div class=\"col-lg-1\" style=\"font-size: 13px;\"><label>22</label></div><div class=\"col-lg-6\" style=\"font-size: 13px;\"><label>Commencement of Training</label></div><div class=\"col-lg-2\"><span>&lt;DtCommencementOfTraining&gt;</span></div></td></tr>" +
								"</tbody></table></div></div></div>";


				model = _admissionBll.GetCalendarNotificationBLL(id, calendarId)[0];

				string filename = Path.ChangeExtension(model.Admsn_notif_doc, ".docx");
				string wordpath = Request.PhysicalApplicationPath + filename;

				content = ReadWordFile(wordpath);
				//content = GetCalendarTextFromPDF(model.Admsn_notif_doc);               
				status = _admissionBll.GetNotificationCalEventStatus(calendarId);
			}
			else
			{
				status = 0;
				//Kannada template
				content = "<p style=\"text-align: center; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಕರ್ನಾಟಕ ಸರ್ಕಾರ </p><p style=\"font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ದಿನಾಂಕ : &lt;ಅಧಿಸೂಚನೆ ದಿನಾಂಕ&gt;</p><p style=\"text-align: right; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ ಸಂ.: &lt;ಅಧಿಸೂಚನೆ ಸಂ.&gt;</p><p>&nbsp;</p><p style=\"text-align: center;font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ</p><br>";
			}
			model.content = content;
			model.StatusId = status;
			//read the Calendar detail dropdowns
			model.CourseList = _admissionBll.GetCourseListCalendarBLL();
			model.ApplicantTypeList = _NotifBll.GetApplicantTypeListBLL();
			model.DeptList = _admissionBll.GetDepartmentListDLL();
			model.SessionList = _admissionBll.GetSessionListCalendarBLL();
			model.AdmissionNotifNoList = _admissionBll.GetAdmissionNotifNoListCalendarBLL();
			model.NotifDescList = _admissionBll.GetCalendarNotfyDescListBLL();
			if (calendarId != null)
			{
				model.selectTab = 0;
			}
			else
			{
				model.selectTab = 1;
			}

			return View(model);
			//return View();
		}

		// function to replace the strings
		[HttpPost]
		public ActionResult AdmissionCalendar(AdmissionNotification model, string submit)
		{
			try
			{
				Log.Info("Entered AdmissionCalendar()");
				model.login_id = Convert.ToInt32(Session["RoleId"]);
				model.user_id = Convert.ToInt32(Session["RoleId"]);
				model.CourseList = _admissionBll.GetCourseListCalendarBLL();
				model.ApplicantTypeList = _NotifBll.GetApplicantTypeListBLL();
				model.DeptList = _admissionBll.GetDepartmentListCalendarBLL();
				model.NotifDescList = _admissionBll.GetCalendarNotfyDescListBLL();
				model.SessionList = _admissionBll.GetSessionListCalendarBLL();
				model.AdmissionNotifNoList = _admissionBll.GetAdmissionNotifNoListCalendarBLL();

				if (TempData["Admsn_tentative_calndr_transId"] != null)
					model.Admsn_tentative_calndr_transId = Convert.ToInt32(TempData["Admsn_tentative_calndr_transId"].ToString());

				//if (submit == "Create")
				//{
					if (ModelState.IsValid)
					{
						List<SelectListItem> notifDescr = (List<SelectListItem>)model.NotifDescList.Items;
						model.NotifDescName = notifDescr[model.NotifDescId].Text;

						// create the folder directory wherer notifcation are stored if it doesn't exists
						string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;
						if (!Directory.Exists(DocumentsFolder))
						{
							Directory.CreateDirectory(DocumentsFolder);
						}
						// replace 
						string filename = model.Exam_Notif_Number.Replace("/", "-");
						filename = filename.Replace("-", "_");

						// get full file path
						string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat1, filename);
						string wordpath = string.Format(DocumentsFolder + DocFileNameFormat1, filename);
						string htmlpath = string.Format(DocumentsFolder + HTMLFilename1, filename);
						// replace the date amd notification number notification data
						model.content = ReplaceDataInTemplateCal(model.content, model);

						//Doc save
						Spire.Doc.Document doc = new Spire.Doc.Document();
						doc.AddSection();
						Spire.Doc.Documents.Paragraph para = doc.Sections[0].AddParagraph();
						para.AppendHTML(model.content);
						doc.SaveToFile(wordpath, Spire.Doc.FileFormat.Docx);
						doc.SaveToFile(htmlpath, Spire.Doc.FileFormat.Html);
						PdfDocument pdf = new PdfDocument();
						PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat
						{
							Layout = PdfLayoutType.Paginate,
							FitToPage = Clip.Width,
							LoadHtmlTimeout = 60 * 1000
						};
						htmlLayoutFormat.IsWaiting = true;
						PdfPageSettings setting = new PdfPageSettings();
						setting.Size = PdfPageSize.A4;
						//Thread thread = new Thread(() => { pdf.LoadFromHTML(htmlpath, true, true, true); });
						//thread.SetApartmentState(ApartmentState.STA);
						//thread.Start();
						//thread.Join();
						pdf.SaveToFile(pdfpath, Spire.Pdf.FileFormat.PDF);
						if (System.IO.File.Exists(htmlpath))
						{
							System.IO.File.Delete(htmlpath);
						}

						// convert to pdf
						//// Word2Pdf(wordpath, pdfpath);
						string folderPath = htmlpath.Replace(".html", "_files");
						// remove the word document post creation of respective pdf

						if (System.IO.Directory.Exists(folderPath))
						{
							System.IO.Directory.Delete(folderPath, true);
						}
						doc.Close();
						// now save the notification details to database
						/// model.SavePath = string.Format(UploadFolder + PdfFileNameFormat, model.Exam_Notif_Number);
						model.SavePath = string.Format(UploadFolder + PdfFileNameFormat1, filename);
						model.DeptId = 1;
						var saved = _admissionBll.CreateCalendarNotificationTransBLL(model);
						TempData["Saved"] = saved;
					}
					else
					{
						TempData["Saved"] = "Invalid";
					}
				//}
				//else if (submit == "Save as Draft")
				//{


				//	if (ModelState.IsValid)
				//	{
				//		List<SelectListItem> notifDescr = (List<SelectListItem>)model.NotifDescList.Items;
				//		model.NotifDescName = notifDescr[model.NotifDescId].Text;
				//		// create the folder directory wherer notifcation are stored if it doesn't exists
				//		string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;
				//		if (!Directory.Exists(DocumentsFolder))
				//		{
				//			Directory.CreateDirectory(DocumentsFolder);
				//		}
				//		// replace 
				//		string filename = model.Exam_Notif_Number.Replace("/", "-");
				//		filename = filename.Replace("-", "_");
				//		// get full file path
				//		string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat1, filename);
				//		string wordpath = string.Format(DocumentsFolder + DocFileNameFormat1, filename);
				//		string htmlpath = string.Format(DocumentsFolder + HTMLFilename1, filename);
				//		// replace the date amd notification number notification data
				//		model.content = ReplaceDataInTemplateCal(model.content, model);
				//		//Doc save
				//		Spire.Doc.Document doc = new Spire.Doc.Document();
				//		doc.AddSection();
				//		Spire.Doc.Documents.Paragraph para = doc.Sections[0].AddParagraph();
				//		para.AppendHTML(model.content);
				//		doc.SaveToFile(wordpath, Spire.Doc.FileFormat.Docx);
				//		doc.SaveToFile(htmlpath, Spire.Doc.FileFormat.Html);

				//		PdfDocument pdf = new PdfDocument();
				//		PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat
				//		{
				//			Layout = PdfLayoutType.Paginate,
				//			FitToPage = Clip.Width,
				//			LoadHtmlTimeout = 60 * 1000
				//		};

				//		htmlLayoutFormat.IsWaiting = true;
				//		PdfPageSettings setting = new PdfPageSettings();

				//		setting.Size = PdfPageSize.A4;
				//		//Thread thread = new Thread(() => { pdf.LoadFromHTML(htmlpath, true, true, true); });
				//		//thread.SetApartmentState(ApartmentState.STA);
				//		//thread.Start();
				//		//thread.Join();
				//		pdf.SaveToFile(pdfpath, Spire.Pdf.FileFormat.PDF);
				//		if (System.IO.File.Exists(htmlpath))
				//		{
				//			System.IO.File.Delete(htmlpath);
				//		}
				//		string folderPath = htmlpath.Replace(".html", "_files");
				//		if (System.IO.Directory.Exists(folderPath))
				//		{
				//			System.IO.Directory.Delete(folderPath, true);
				//		}
				//		doc.Close();
				//		model.SavePath = string.Format(UploadFolder + PdfFileNameFormat1, filename);
				//		model.DeptId = 2;
				//		var saved = _admissionBll.CreateCalendarNotificationTransBLL(model);
				//	}
				//	else
				//	{
				//		TempData["Saved"] = "Invalid";
				//	}
				//}
				//else if (submit == "Update")
				//{
				//	if (ModelState.IsValid)
				//	{
				//		List<SelectListItem> notifDescr = (List<SelectListItem>)model.NotifDescList.Items;
				//		model.NotifDescName = notifDescr[model.NotifDescId].Text;

				//		// create the folder directory wherer notifcation are stored if it doesn't exists
				//		string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

				//		if (!Directory.Exists(DocumentsFolder))
				//		{
				//			Directory.CreateDirectory(DocumentsFolder);
				//		}
				//		// replace 
				//		string filename = model.Exam_Notif_Number.Replace("/", "-");
				//		filename = filename.Replace("-", "_");
				//		filename = filename + "_1";
				//		// get full file path
				//		string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat1, filename);
				//		string wordpath = string.Format(DocumentsFolder + DocFileNameFormat1, filename);
				//		string htmlpath = string.Format(DocumentsFolder + HTMLFilename1, filename);

				//		// replace the date amd notification number notification data
				//		model.content = ReplaceDataInTemplateCal(model.content, model);

				//		//Doc save
				//		Spire.Doc.Document doc = new Spire.Doc.Document();
				//		doc.AddSection();
				//		Spire.Doc.Documents.Paragraph para = doc.Sections[0].AddParagraph();
				//		para.AppendHTML(model.content);
				//		doc.SaveToFile(wordpath, Spire.Doc.FileFormat.Docx);
				//		doc.SaveToFile(htmlpath, Spire.Doc.FileFormat.Html);

				//		PdfDocument pdf = new PdfDocument();
				//		PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat
				//		{
				//			Layout = PdfLayoutType.Paginate,
				//			FitToPage = Clip.Width,
				//			LoadHtmlTimeout = 60 * 1000
				//		};
				//		htmlLayoutFormat.IsWaiting = true;
				//		PdfPageSettings setting = new PdfPageSettings();
				//		setting.Size = PdfPageSize.A4;
				//		//Thread thread = new Thread(() => { pdf.LoadFromHTML(htmlpath, true, true, true); });
				//		//thread.SetApartmentState(ApartmentState.STA);
				//		//thread.Start();
				//		//thread.Join();
				//		pdf.SaveToFile(pdfpath, Spire.Pdf.FileFormat.PDF);

				//		if (System.IO.File.Exists(htmlpath))
				//		{
				//			System.IO.File.Delete(htmlpath);
				//		}

				//		// convert to pdf
				//		//// Word2Pdf(wordpath, pdfpath);
				//		string folderPath = htmlpath.Replace(".html", "_files");
				//		// remove the word document post creation of respective pdf
				//		if (System.IO.Directory.Exists(folderPath))
				//		{
				//			System.IO.Directory.Delete(folderPath, true);
				//		}
				//		doc.Close();
				//		// now save the notification details to database
				//		/// model.SavePath = string.Format(UploadFolder + PdfFileNameFormat, model.Exam_Notif_Number);
				//		model.SavePath = string.Format(UploadFolder + PdfFileNameFormat1, filename);
				//		model.DeptId = 3;
				//		var saved = _admissionBll.CreateCalendarNotificationTransBLL(model);
				//		TempData["saved"] = saved;
				//	}
				//	else
				//	{
				//		TempData["saved"] = "Invalid";
				//	}
				//}
				return View(model);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - AdmissionCalendar():" + ex.Message.ToString());
				throw ex;
			}
			//_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			//Utilities.Security.ValidateRequestHeader(Request);


		}

		[HttpPost]
		public ActionResult SaveDraftAdmissionCalendar(AdmissionNotification model)
		{
			try
			{
				model.login_id = Convert.ToInt32(Session["RoleId"]);
				model.user_id = Convert.ToInt32(Session["RoleId"]);
				model.CourseList = _admissionBll.GetCourseListCalendarBLL();
				model.ApplicantTypeList = _NotifBll.GetApplicantTypeListBLL();
				model.DeptList = _admissionBll.GetDepartmentListCalendarBLL();
				model.NotifDescList = _admissionBll.GetCalendarNotfyDescListBLL();
				model.SessionList = _admissionBll.GetSessionListCalendarBLL();
				model.AdmissionNotifNoList = _admissionBll.GetAdmissionNotifNoListCalendarBLL();

				if (TempData["Admsn_tentative_calndr_transId"] != null)
					model.Admsn_tentative_calndr_transId = Convert.ToInt32(TempData["Admsn_tentative_calndr_transId"].ToString());
				if (ModelState.IsValid)
				{
					List<SelectListItem> notifDescr = (List<SelectListItem>)model.NotifDescList.Items;
					model.NotifDescName = notifDescr[model.NotifDescId].Text;

					// create the folder directory wherer notifcation are stored if it doesn't exists
					string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

					if (!Directory.Exists(DocumentsFolder))
					{
						Directory.CreateDirectory(DocumentsFolder);
					}
					// replace 
					string filename = model.Exam_Notif_Number.Replace("/", "-");
					filename = filename.Replace("-", "_");
					// get full file path
					string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat1, filename);
					string wordpath = string.Format(DocumentsFolder + DocFileNameFormat1, filename);
					string htmlpath = string.Format(DocumentsFolder + HTMLFilename1, filename);

					// replace the date amd notification number notification data
					model.content = ReplaceDataInTemplateCal(model.content, model);

					//Doc save
					Spire.Doc.Document doc = new Spire.Doc.Document();
					doc.AddSection();
					Spire.Doc.Documents.Paragraph para = doc.Sections[0].AddParagraph();
					para.AppendHTML(model.content);
					doc.SaveToFile(wordpath, Spire.Doc.FileFormat.Docx);
					doc.SaveToFile(htmlpath, Spire.Doc.FileFormat.Html);

					PdfDocument pdf = new PdfDocument();
					PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat
					{
						Layout = PdfLayoutType.Paginate,
						FitToPage = Clip.Width,
						LoadHtmlTimeout = 60 * 1000
					};
					htmlLayoutFormat.IsWaiting = true;
					PdfPageSettings setting = new PdfPageSettings();
					setting.Size = PdfPageSize.A4;
					//Thread thread = new Thread(() => { pdf.LoadFromHTML(htmlpath, true, true, true); });
					//thread.SetApartmentState(ApartmentState.STA);
					//thread.Start();
					//thread.Join();
					pdf.SaveToFile(pdfpath, Spire.Pdf.FileFormat.PDF);

					if (System.IO.File.Exists(htmlpath))
					{
						System.IO.File.Delete(htmlpath);
					}
					string folderPath = htmlpath.Replace(".html", "_files");
					if (System.IO.Directory.Exists(folderPath))
					{
						System.IO.Directory.Delete(folderPath, true);
					}
					doc.Close();

					model.SavePath = string.Format(UploadFolder + PdfFileNameFormat1, filename);
					model.DeptId = 2;
					var saved = _admissionBll.CreateCalendarNotificationTransBLL(model);
					TempData["Saved"] = "Draft";
				}
				else
				{
					TempData["Saved"] = "Invalid";
				}
				return View("AdmissionCalendar", model);
			}
			catch (Exception ex)
			{

				throw ex;
			}
			//Utilities.Security.ValidateRequestHeader(Request);

		}


		[HttpPost]
		public ActionResult UpdateCalendarNotification(AdmissionNotification model)
		{
			try
			{
				//Utilities.Security.ValidateRequestHeader(Request);
				model.login_id = Convert.ToInt32(Session["RoleId"]);
				model.user_id = Convert.ToInt32(Session["RoleId"]);
				model.CourseList = _admissionBll.GetCourseListCalendarBLL();
				model.ApplicantTypeList = _NotifBll.GetApplicantTypeListBLL();
				model.DeptList = _admissionBll.GetDepartmentListCalendarBLL();
				model.NotifDescList = _admissionBll.GetCalendarNotfyDescListBLL();
				model.SessionList = _admissionBll.GetSessionListCalendarBLL();
				model.AdmissionNotifNoList = _admissionBll.GetAdmissionNotifNoListCalendarBLL();

				if (TempData["Admsn_tentative_calndr_transId"] != null)
					model.Admsn_tentative_calndr_transId = Convert.ToInt32(TempData["Admsn_tentative_calndr_transId"].ToString());
				if (ModelState.IsValid)
				{
					List<SelectListItem> notifDescr = (List<SelectListItem>)model.NotifDescList.Items;
					model.NotifDescName = notifDescr[model.NotifDescId].Text;

					// create the folder directory wherer notifcation are stored if it doesn't exists
					string DocumentsFolder = Request.PhysicalApplicationPath + UploadFolder;

					if (!Directory.Exists(DocumentsFolder))
					{
						Directory.CreateDirectory(DocumentsFolder);
					}
					// replace 
					string filename = model.Exam_Notif_Number.Replace("/", "-");
					filename = filename.Replace("-", "_");
					filename = filename + "_1";
					// get full file path
					string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat1, filename);
					string wordpath = string.Format(DocumentsFolder + DocFileNameFormat1, filename);
					string htmlpath = string.Format(DocumentsFolder + HTMLFilename1, filename);

					// replace the date amd notification number notification data
					model.content = ReplaceDataInTemplateCal(model.content, model);

					//Doc save
					Spire.Doc.Document doc = new Spire.Doc.Document();
					doc.AddSection();
					Spire.Doc.Documents.Paragraph para = doc.Sections[0].AddParagraph();
					para.AppendHTML(model.content);
					doc.SaveToFile(wordpath, Spire.Doc.FileFormat.Docx);
					doc.SaveToFile(htmlpath, Spire.Doc.FileFormat.Html);

					PdfDocument pdf = new PdfDocument();
					PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat
					{
						Layout = PdfLayoutType.Paginate,
						FitToPage = Clip.Width,
						LoadHtmlTimeout = 60 * 1000
					};
					htmlLayoutFormat.IsWaiting = true;
					PdfPageSettings setting = new PdfPageSettings();
					setting.Size = PdfPageSize.A4;
					//Thread thread = new Thread(() => { pdf.LoadFromHTML(htmlpath, true, true, true); });
					//thread.SetApartmentState(ApartmentState.STA);
					//thread.Start();
					//thread.Join();
					pdf.SaveToFile(pdfpath, Spire.Pdf.FileFormat.PDF);

					if (System.IO.File.Exists(htmlpath))
					{
						System.IO.File.Delete(htmlpath);
					}

					// convert to pdf
					//// Word2Pdf(wordpath, pdfpath);
					string folderPath = htmlpath.Replace(".html", "_files");
					// remove the word document post creation of respective pdf
					if (System.IO.Directory.Exists(folderPath))
					{
						System.IO.Directory.Delete(folderPath, true);
					}
					doc.Close();
					// now save the notification details to database
					/// model.SavePath = string.Format(UploadFolder + PdfFileNameFormat, model.Exam_Notif_Number);
					model.SavePath = string.Format(UploadFolder + PdfFileNameFormat1, filename);
					model.DeptId = 3;
					var saved = _admissionBll.CreateCalendarNotificationTransBLL(model);
					TempData["saved"] = saved;
				}
				else
				{
					TempData["saved"] = "Invalid";
				}
				return View("AdmissionCalendar", model);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public ActionResult UpdateAdmissionCalendarDet()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			AdmissionNotification model = new AdmissionNotification();
			if (TempData["Admsn_tentative_calndr_transId"] != null)
				model.Admission_Notif_Id = Convert.ToInt32(TempData["Admsn_tentative_calndr_transId"].ToString());
			int login = Convert.ToInt32(Session["RoleId"].ToString());
			model.GetUpdateNotifDet = _admissionBll.GetUpdateCalendarNtfBLL(login, model.Admission_Notif_Id);
			return View(model);
		}

		public JsonResult GetAdmissionCalendarDetails(int id)
		{
			int login = Convert.ToInt32(Session["RoleId"].ToString());
			var Response = _admissionBll.GetAdmissionCalendarDetailsBLL(id, login);
			Response.RoleId = login;
			Response.login_id = Convert.ToInt32(Session["UserId"].ToString());
			if (Response.ForwardId == login)
			{
				Response.forwardStatus = true;
			}
			if (login <= 7)
			{
				Response.ChangesStatus = true;
			}
			else
			{
				Response.ChangesStatus = false;
			}
			if (login == 1 || login == 2 || login == 4 || login == 6)
			{
				Response.ApprovedStatus = true;
			}
			else
			{
				Response.ApprovedStatus = false;
			}

			if (Response.StatusId == 109 || Response.StatusId == 103)
			{
				Response.ApprovedStatus = false;
			}

			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		//public JsonResult GetAdmissionCalendarDetails(int id)
		//{
		//    int login = Convert.ToInt32(Session["RoleId"].ToString());
		//    var Response = _admissionBll.GetAdmissionCalendarDetailsBLL(id, login);
		//    Response.RoleId = login;
		//    if (Response.ForwardId == login)
		//    {
		//        Response.forwardStatus = true;
		//    }
		//    if (login == 4 || login == 6 || login == 2 || login == 1)
		//    {
		//        if (Response.ForwardId == 4)
		//        {
		//            Response.ApprovedStatus = true;
		//            Response.ChangesStatus = true;
		//        }
		//        else if (Response.ForwardId == 6)
		//        {
		//            Response.ApprovedStatus = true;
		//            Response.ChangesStatus = true;
		//        }
		//        else if (Response.ForwardId == 2)
		//        {
		//            Response.ApprovedStatus = true;
		//            Response.ChangesStatus = true;
		//        }
		//        else if (Response.ForwardId == 1)
		//        {
		//            Response.ApprovedStatus = true;
		//            Response.ChangesStatus = true;
		//        }
		//        else
		//        {
		//            Response.ApprovedStatus = false;
		//            Response.ChangesStatus = false;
		//        }
		//    }

		//    if (Response.StatusId == 109)
		//    {
		//        Response.ApprovedStatus = false;
		//    }
		//    if (Response.StatusId == 103)
		//    {
		//        Response.ApprovedStatus = false;
		//        Response.ChangesStatus = false;
		//    }

		//    return Json(Response, JsonRequestBehavior.AllowGet);
		//}
		public JsonResult GetAdmissionCalendarView()
		{
			List<AdmissionNotification> RemarksList = new List<AdmissionNotification>();
			RemarksList = _admissionBll.GetAdmissionCalendarViewBLL(Convert.ToInt32(Session["RoleId"].ToString()));
			int x = 1;
			foreach (var item in RemarksList)
			{
				item.slno = x;
				x++;
			}
			return Json(RemarksList, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCommentsCalendarFile(int id)
		{
			List<AdmissionNotification> Remarks = new List<AdmissionNotification>();
			Remarks = _admissionBll.GetCommentsCalendarFileBLL(id).OrderByDescending(a => a.createdatetime).ToList(); ;
			int x = 1;
			foreach (var item in Remarks)
			{
				item.slno = x;
				x++;
			}
			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ForwardAdmissionCalendarNotification(int id, int admiNotifId, string remarks)
		{
			//_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			var res = _admissionBll.ForwardAdmissionCalendarNotificationBLL(Convert.ToInt32(Session["UserId"].ToString()), id, admiNotifId, remarks, Convert.ToInt32(Session["RoleId"].ToString()));
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ApproveAdmissionCalendarNotification(int id, int admiNotifId, string remarks)
		{
			//_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			var res = _admissionBll.ApproveAdmissionCalendarNotificationBLL(Convert.ToInt32(Session["UserId"].ToString()), id, admiNotifId, remarks, Convert.ToInt32(Session["RoleId"].ToString()));
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ChangesAdmissionCalendarNotification(int id, int admiNotifId, string remarks)
		{
			//_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			var res = _admissionBll.ChangesAdmissionCalendarNotificationBLL(Convert.ToInt32(Session["RoleId"].ToString()), id, admiNotifId, remarks);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult PublishAdmissionCalNotification(int notificationId)
		{
			//_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.PublishAdmissionCalNotification(notificationId, loginId, Convert.ToInt32(Session["RoleId"].ToString()));
			return Json(res, JsonRequestBehavior.AllowGet);
		}


		public ActionResult SendbackAdmissionCalNotification(int id, int admiNotifId, string remarks)
		{
			//_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			var res = _admissionBll.SendbackAdmissionCalNotification(Convert.ToInt32(Session["UserId"].ToString()), id, admiNotifId, remarks, Convert.ToInt32(Session["RoleId"].ToString()));
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ChangesAdmissionCalNotification(int id, int admiNotifId, string remarks)
		{
			//_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			var res = _admissionBll.ChangesAdmissionCalNotification(Convert.ToInt32(Session["UserId"].ToString()), id, admiNotifId, remarks, Convert.ToInt32(Session["RoleId"].ToString()));
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetRolesCal(int level)
		{
			var Response = _admissionBll.GetRoles(Convert.ToInt32(Session["RoleId"].ToString()), level);
			//Response.RemoveAll(t => t.RoleID == 3);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetApplicantTypeAdmission(int Id)
		{
			var Response = _admissionBll.GetApplicantTypeAdmission(Id);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetAdmissionNtfNumber(string Id)
		{
			int Idd;
			List<SelectListItem> Response = new List<SelectListItem>();
			if (!string.IsNullOrEmpty(Id))
			{
				Idd = Convert.ToInt32(Id);
				List<AdmissionNotification> admissionNtfNumber = _admissionBll.GetAdmissionNtfNumber(Idd);
				admissionNtfNumber.ForEach(x =>
				{
					Response.Add(new SelectListItem
					{
						Text = x.AdmsnNtfNum.ToString(),
						Value = x.Admsn_notif_Id.ToString()
					});
				});
			}
			//var Response = _admissionBll.GetAdmissionNtfNumber(Id);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}


		#endregion Calendar18022021

		#region .. seat Allocation ..

		public ActionResult SeatAllocation()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			return View();
		}

		[HttpGet]
		public ActionResult GetSyllabusType()
		{
			List<SeatAllocation> Syllabus_list = new List<SeatAllocation>();
			Syllabus_list = _admissionBll.GetSyllabusBLL();
			return Json(Syllabus_list, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetYearType()
		{
			List<SeatAllocation> Year_list = new List<SeatAllocation>();
			Year_list = _admissionBll.GetYearTypeBLL();
			return Json(Year_list, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public ActionResult GetExamYear()
		{
			List<SeatAllocation> Year_list = new List<SeatAllocation>();
			Year_list = _admissionBll.GetYearTypeBLL();
			return Json(Year_list, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public ActionResult InsertSeatAllocationData(SeatAllocation objSeatAllocation)
		{
			try
			{
				Log.Info("Entered InsertSeatAllocationData()");

				objSeatAllocation.ModifiedBy = Convert.ToInt32(Session["LoginId"]);
				var StatusListById = _admissionBll.InsertRulesAllocationMasterBLL(objSeatAllocation);
				Log.Info("Left InsertSeatAllocationData()");
				return Json(StatusListById, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - GetStatusList():" + ex.Message.ToString());
				throw ex;
			}
		}

		/*[HttpGet]
		public ActionResult GetSeatAllocationBasedOnExamCourse(int Exam_Year, int CourseId)
		{
				List<SeatAllocation> SeatAllocationVTRules = new List<SeatAllocation>();
				List<SeatAllocation> SeatAllocationRulesHRS = new List<SeatAllocation>();
				List<SeatAllocation> SeatAllocationRulesHYD = new List<SeatAllocation>();
				List<SeatAllocation> SeatAllocationRulesGrade = new List<SeatAllocation>();
				List<SeatAllocation> SeatAllocationOtherRules = new List<SeatAllocation>();

				if (ModelState.IsValid)
				{
						SeatAllocationVTRules = _admissionBll.GetSeatAllocationVerticalBLL(Exam_Year, CourseId);
						SeatAllocationRulesHRS = _admissionBll.GetSeatAllocationHorizontalBLL(Exam_Year, CourseId);
						SeatAllocationRulesHYD = _admissionBll.GetSeatAllocationHydBLL(Exam_Year, CourseId);
						SeatAllocationRulesGrade = _admissionBll.GetSeatAllocationGradeBLL(Exam_Year, CourseId);
						SeatAllocationOtherRules = _admissionBll.GetSeatAllocationOtherRulesBLL(Exam_Year, CourseId);
				}

				return Json(new
				{
						list1 = SeatAllocationVTRules,
						list2 = SeatAllocationRulesHRS,
						list3 = SeatAllocationRulesHYD,
						list4 = SeatAllocationRulesGrade,
						list5 = SeatAllocationOtherRules

				}, JsonRequestBehavior.AllowGet);
		}*/

		[HttpGet]
		public ActionResult GetSeatAllocationData()
		{
			List<SeatAllocation> SeatAllocationVTRules = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesHRS = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesHYD = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesGrade = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationOtherRules = new List<SeatAllocation>();

			if (ModelState.IsValid)
			{
				SeatAllocationVTRules = _admissionBll.GetSeatAllocationVerticalBLL();
				SeatAllocationRulesHRS = _admissionBll.GetSeatAllocationHorizontalBLL();
				SeatAllocationRulesHYD = _admissionBll.GetSeatAllocationHydBLL();
				SeatAllocationRulesGrade = _admissionBll.GetSeatAllocationGradeBLL();
				SeatAllocationOtherRules = _admissionBll.GetSeatAllocationOtherRulesBLL();
			}

			return Json(new
			{
				list1 = SeatAllocationVTRules,
				list2 = SeatAllocationRulesHRS,
				list3 = SeatAllocationRulesHYD,
				list4 = SeatAllocationRulesGrade,
				list5 = SeatAllocationOtherRules

			}, JsonRequestBehavior.AllowGet);
		}



		public ActionResult ExamYear(int ExamYear)
		{
			var Exam_Year = _admissionBll.GetExamYearBLL(ExamYear);
			return Json(Exam_Year, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult UpdateRemarksForSeats(SeatAllocation seatmst)
		{
			seatmst.ModifiedBy = Convert.ToInt32(Session["RoleId"]);
			string returnMsg = _admissionBll.GetSeatupdateVerticalBLL(seatmst);
			return Json(returnMsg);
		}

		#endregion

		#region .. Case Worker

		public ActionResult SeatAllocationInsertUpdateRules()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			return View();
		}

		[HttpGet]
		public ActionResult GetSeatAllocationApprovedBLL(int Exam_Year, int CourseId, string tabNameValue)
		{
			List<SeatAllocation> SeatAllocationVTRules = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesHRS = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesHYD = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesGrade = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationOtherRules = new List<SeatAllocation>();

			if (ModelState.IsValid)
			{
				SeatAllocationVTRules = _admissionBll.GetSeatAllocationVerticalBLL(Exam_Year, CourseId, tabNameValue);
				SeatAllocationRulesHRS = _admissionBll.GetSeatAllocationHorizontalBLL(Exam_Year, CourseId, tabNameValue);
				SeatAllocationRulesHYD = _admissionBll.GetSeatAllocationHydBLL(Exam_Year, CourseId, tabNameValue);
				SeatAllocationRulesGrade = _admissionBll.GetSeatAllocationGradeBLL(Exam_Year, CourseId, tabNameValue);
				SeatAllocationOtherRules = _admissionBll.GetSeatAllocationOtherRulesBLL(Exam_Year, CourseId, tabNameValue);
			}

			return Json(new
			{
				list1 = SeatAllocationVTRules,
				list2 = SeatAllocationRulesHRS,
				list3 = SeatAllocationRulesHYD,
				list4 = SeatAllocationRulesGrade,
				list5 = SeatAllocationOtherRules

			}, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetSeatAllocationDataInsertUpdateRules(int Exam_Year, int CourseId, string tabNameValue)
		{
			List<SeatAllocation> SeatAllocationVTRules = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesHRS = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesHYD = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesGrade = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationOtherRules = new List<SeatAllocation>();

			if (ModelState.IsValid)
			{
				SeatAllocationVTRules = _admissionBll.GetSeatAllocationVerticalBLL(Exam_Year, CourseId, tabNameValue);
				SeatAllocationRulesHRS = _admissionBll.GetSeatAllocationHorizontalBLL(Exam_Year, CourseId, tabNameValue);
				SeatAllocationRulesHYD = _admissionBll.GetSeatAllocationHydBLL(Exam_Year, CourseId, tabNameValue);
				SeatAllocationRulesGrade = _admissionBll.GetSeatAllocationGradeBLL(Exam_Year, CourseId, tabNameValue);
				SeatAllocationOtherRules = _admissionBll.GetSeatAllocationOtherRulesBLL(Exam_Year, CourseId, tabNameValue);
			}

			return Json(new
			{
				list1 = SeatAllocationVTRules,
				list2 = SeatAllocationRulesHRS,
				list3 = SeatAllocationRulesHYD,
				list4 = SeatAllocationRulesGrade,
				list5 = SeatAllocationOtherRules

			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult AllocationDataInsertUpdateRules(SeatAllocation objSeatAllocation)
		{
			try
			{
				Log.Info("Entered InsertSeatAllocationData()");
				Utilities.Security.ValidateRequestHeader(Request);
				objSeatAllocation.ModifiedBy = Convert.ToInt32(Session["RoleId"]);
				var StatusListById = _admissionBll.InsertRulesAllocationMasterBLL(objSeatAllocation);
				Log.Info("Left InsertSeatAllocationData()");
				return Json(StatusListById, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - GetStatusList():" + ex.Message.ToString());
				throw ex;
			}
		}

		[HttpPost]
		public ActionResult RuleAllocationChkExistence(SeatAllocation objSeatAllocation)
		{
			try
			{
				Log.Info("Entered RuleAllocationChkExistence()");
				Utilities.Security.ValidateRequestHeader(Request);
				var StatusListById = _admissionBll.RuleAllocationChkExistenceBLL(objSeatAllocation);
				Log.Info("Left RuleAllocationChkExistence()");
				return Json(StatusListById, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - RuleAllocationChkExistence():" + ex.Message.ToString());
				throw ex;
			}
		}

		#endregion

		#region .. ADD ..

		public ActionResult SeatAllocationReviewView()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			SeatAllocation seatAllocationmodal = new SeatAllocation();
			seatAllocationmodal.FlowId = Convert.ToInt32(Session["RoleId"]);
			List<SeatAllocation> list = _admissionBll.GetSeatAllocationByFlowIdBLL(seatAllocationmodal).ToList();
			seatAllocationmodal.seatAllocationmodel = list;
			return View(seatAllocationmodal);
		}

		public JsonResult getDataseatallocationById(string seatAllocationPopUp)
		{
			try
			{
				List<SeatAllocation> list = new List<SeatAllocation>();
				list = _admissionBll.GetseatallocationListByIdBLL(seatAllocationPopUp);
				return Json(list, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				Log.Error("Error Occured while performing Edit Operation in  BGVerification Details" + e);
				throw e;
			}
		}

		#endregion

		#region .. D/C ..

		public ActionResult SeatAllocationApproveView()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			SeatAllocation seatAllocationmodal = new SeatAllocation();
			seatAllocationmodal.FlowId = Convert.ToInt32(Session["UserId"]);
			List<SeatAllocation> list = _admissionBll.GetSeatAllocationByFlowIdBLL(seatAllocationmodal).ToList();
			seatAllocationmodal.seatAllocationmodel = list;
			return View(seatAllocationmodal);
		}

		#endregion

		#region 

		public JsonResult GetAllocationSeatofRuletoUpdate()
		{
			try
			{
				List<SeatAllocation> listOfSeatAllocation = new List<SeatAllocation>();
				listOfSeatAllocation = _admissionBll.GetAllocationSeatofRuletoUpdateBLL();
				int slnoc = 1;
				foreach (var x in listOfSeatAllocation)
				{
					x.SlNo = slnoc++;
				}
				return Json(listOfSeatAllocation, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Error Occured while performing Edit Operation in  BGVerification Details" + ex);
				throw ex;
			}
		}

		public JsonResult GetApprovedAllocationSeat()
		{
			try
			{
				List<SeatAllocation> listOfSeatAllocation = new List<SeatAllocation>();
				listOfSeatAllocation = _admissionBll.GetApprovedAllocationSeatofRuleBLL();
				int slnoc = 1;
				foreach (var x in listOfSeatAllocation)
				{
					x.SlNo = slnoc++;
				}
				return Json(listOfSeatAllocation, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Error Occured while performing Edit Operation in  BGVerification Details" + ex);
				throw ex;
			}
		}

		public ActionResult GetSeatAllocationById(int Rules_allocation_masterid)
		{
			List<SeatAllocation> SeatAllocationVTRules = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesHRS = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesHYD = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesGrade = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationOtherRules = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRemarksStatus = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationGetExamYear = new List<SeatAllocation>();
			List<CourseMaster> SeatAllocationGetCourseTypes = new List<CourseMaster>();
			List<SeatAllocation> SeatAllocationGetSyllabus = new List<SeatAllocation>();

			SeatAllocationGetExamYear = _admissionBll.GetYearTypeBLL();
			SeatAllocationGetCourseTypes = _masterBll.GetCourseTypes();
			SeatAllocationGetSyllabus = _admissionBll.GetSyllabusBLL();

			if (ModelState.IsValid)
			{
				SeatAllocationVTRules = _admissionBll.GetSeatAllocationVerticalBLL(Rules_allocation_masterid);
				SeatAllocationRulesHRS = _admissionBll.GetSeatAllocationHorizontalBLL(Rules_allocation_masterid);
				SeatAllocationRulesHYD = _admissionBll.GetSeatAllocationHydBLL(Rules_allocation_masterid);
				SeatAllocationRulesGrade = _admissionBll.GetSeatAllocationGradeBLL(Rules_allocation_masterid);
				SeatAllocationOtherRules = _admissionBll.GetSeatAllocationOtherRulesBLL(Rules_allocation_masterid);
				SeatAllocationRemarksStatus = _admissionBll.GetSeatAllocationRemarksStatusBLL(Rules_allocation_masterid);
			}

			return Json(new
			{
				list1 = SeatAllocationVTRules,
				list2 = SeatAllocationRulesHRS,
				list3 = SeatAllocationRulesHYD,
				list4 = SeatAllocationRulesGrade,
				list5 = SeatAllocationOtherRules,
				list6 = SeatAllocationRemarksStatus,
				list7 = SeatAllocationGetExamYear,
				list8 = SeatAllocationGetCourseTypes,
				list9 = SeatAllocationGetSyllabus

			}, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetSeatAllocationApproved(int Rules_allocation_masterid)
		{
			List<SeatAllocation> SeatAllocationVTRules = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesHRS = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesHYD = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationRulesGrade = new List<SeatAllocation>();
			List<SeatAllocation> SeatAllocationOtherRules = new List<SeatAllocation>();

			if (ModelState.IsValid)
			{
				SeatAllocationVTRules = _admissionBll.GetSeatAllocationApprovedVerticalBLL(Rules_allocation_masterid);
				SeatAllocationRulesHRS = _admissionBll.GetSeatAllocationApprovedHorizontalBLL(Rules_allocation_masterid);
				SeatAllocationRulesHYD = _admissionBll.GetSeatAllocationApprovedHydBLL(Rules_allocation_masterid);
				SeatAllocationRulesGrade = _admissionBll.GetSeatAllocationApprovedGradeBLL(Rules_allocation_masterid);
				SeatAllocationOtherRules = _admissionBll.GetSeatAllocationApprovedOtherRulesBLL(Rules_allocation_masterid);
			}

			return Json(new
			{
				list1 = SeatAllocationVTRules,
				list2 = SeatAllocationRulesHRS,
				list3 = SeatAllocationRulesHYD,
				list4 = SeatAllocationRulesGrade,
				list5 = SeatAllocationOtherRules

			}, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCommentDetails(int SeatAllocationId)
		{
			var Remarks = _admissionBll.GetCommentsListBLL(SeatAllocationId);

			int c = 1;
			foreach (var x in Remarks)
			{
				x.SlNo = c++;
			}
			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCommentDetailsRuleofAllocation(int SeatAllocationId)
		{
			var Remarks = _admissionBll.GetCommentsListBLL(SeatAllocationId);

			int c = 1;
			foreach (var x in Remarks)
			{
				x.SlNo = c++;
				if (x.StatusId == 2)
					x.StatusName = "Approved";
			}

			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCommentDetailsApplicant(int SeatAllocationId)
		{
			var Remarks = _admissionBll.GetCommentDetailsApplicantBLL(SeatAllocationId);

			int c = 1;
			foreach (var x in Remarks)
			{
				x.slno = c++;
			}

			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region .. Applicant online Application Form ..

		public ActionResult ApplicantOnlineApplicationForm()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			var getapplicant = _admissionBll.GetApplicantApplicationForm();
			ApplicantApplicationForm ApplicantApplicationFormData = new ApplicantApplicationForm();
			if (getapplicant != null)
			{
				ApplicantApplicationForm rec = new ApplicantApplicationForm
				{
					PhoneNumber = getapplicant.PhoneNumber,
					EmailId = getapplicant.EmailId,
					ApplicantName = getapplicant.ApplicantName
				};
				ViewBag.ApplicantDetails = rec;

			}
			ViewBag.QualificationList = GetAppliedForWhichBasics();
			ViewBag.SyllabusList = GetAppliedForSyllabus();
			ViewBag.SyllabusListCOBSE = GetAppliedForSyllabusCOBSE();
			ViewBag.LocationList = GetLocationList();
			var AssignDataBasedOnTabData = new AssignDataBasedOnTabs();
			return View(AssignDataBasedOnTabData);
		}

		public List<ApplicationForm> GetAppliedForSyllabus()
		{
			List<ApplicationForm> SyllabusList = new List<ApplicationForm>();
			SyllabusList = _admissionBll.GetSyllabusListBLL();
			SyllabusList.RemoveAll(t => t.Syllabus_type_id == 2 || t.Syllabus_type_id == 3 || t.Syllabus_type_id == 6 || t.Syllabus_type_id == 7);
			return SyllabusList;
		}

		public List<ApplicationForm> GetAppliedForSyllabusCOBSE()
		{
			List<ApplicationForm> SyllabusList = new List<ApplicationForm>();
			SyllabusList = _admissionBll.GetSyllabusListBLL();
			SyllabusList.RemoveAll(t => t.Syllabus_type_id == 1 || t.Syllabus_type_id == 4);
			return SyllabusList;
		}

		public List<ApplicationForm> GetAppliedForWhichBasics()
		{
			List<ApplicationForm> QualificationList = new List<ApplicationForm>();
			QualificationList = _admissionBll.GetQualificationListBLL();
			return QualificationList;
		}

		public List<ApplicationForm> GetLocationList()
		{
			List<ApplicationForm> LocationList = new List<ApplicationForm>();
			LocationList = _admissionBll.GetLocationListBLL();
			return LocationList;
		}

		public JsonResult GetReligionList()
		{
			List<ApplicationForm> Religion_list = new List<ApplicationForm>();
			Religion_list = _admissionBll.GetReligionDetailsBLL();
			return Json(Religion_list, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetcoursetypeListbycalendar()
		{
			List<ApplicationForm> course_type = new List<ApplicationForm>();
			course_type = _admissionBll.GetcoursetypeListbycalendar();
			return Json(course_type, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetGenderList()
		{
			List<ApplicationForm> Gender_list = new List<ApplicationForm>();
			Gender_list = _admissionBll.GetGenderDetailsBLL();
			return Json(Gender_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetDocumentApplicationStatus()
		{
			List<ApplicationForm> DocumentApplicationStatus = new List<ApplicationForm>();
			DocumentApplicationStatus = _admissionBll.GetDocumentApplicationStatusBLL();
			return Json(DocumentApplicationStatus, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCategoryList()
		{
			List<ApplicationForm> Category_list = new List<ApplicationForm>();
			Category_list = _admissionBll.GetCategoryListBLL();
			return Json(Category_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetApplicantTypeList()
		{
			List<ApplicationForm> Applicant_list = new List<ApplicationForm>();
			Applicant_list = _admissionBll.GetApplicantTypeListBLL(Convert.ToInt32(Session["RoleId"].ToString()));
			return Json(Applicant_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetReservationsList()
		{
			List<ApplicationForm> Reservation_list = new List<ApplicationForm>();
			Reservation_list = _admissionBll.GetReservationsListBLL();
			return Json(Reservation_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetQualificationList()
		{
			List<ApplicationForm> Qualification_list = new List<ApplicationForm>();
			Qualification_list = _admissionBll.GetQualificationListBLL();
			return Json(Qualification_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetAppliedForBasics()
		{
			List<ApplicationForm> Qualification_list = new List<ApplicationForm>();
			Qualification_list = _admissionBll.GetQualificationListBLL();
			ViewBag.Qualification_list = Qualification_list;
			return Json(Qualification_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetMasterDistrictList()
		{
			var District_list = _admissionBll.GetDistrictMasterListBLL();
			return Json(District_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetTalukMasterList(int Districts)
		{
			var Taluk_list = _admissionBll.GetTalukMasterListBLL(Districts);
			return Json(Taluk_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetITICollegeDetails(int Pincode)
		{
			List<ApplicationForm> College_list = new List<ApplicationForm>();
			College_list = _admissionBll.GetITICollegeDetailsBLL(Pincode);
			return Json(College_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetITICollegeDetailsByDistrictTaluka(int District, int Taluka)
		{
			List<ApplicationForm> College_list = new List<ApplicationForm>();
			College_list = _admissionBll.GetITICollegeDetailsByDistrictTalukaBLL(District, Taluka);
			return Json(College_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetITICollegeTradeDetails(int TradeCode, string qual)
		{
			List<ApplicationForm> College_list = new List<ApplicationForm>();
			College_list = _admissionBll.GetITICollegeTradeDetailsBLL(TradeCode, qual);
			return Json(College_list, JsonRequestBehavior.AllowGet);
		}

		public FileResult DownloadAdmissionDoc(int ApplicationId)
		{
			var data = _admissionBll.GetAdmissionDocumentDetailsBLL(ApplicationId);
			byte[] fileBytes = System.IO.File.ReadAllBytes(data.Photo);
			string fileName = System.IO.Path.GetFileName(data.Photo);
			return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
		}

		[HttpPost]
		public JsonResult GetApplicantRemarksList(int ApplicantTransId)
		{
			Utilities.Security.ValidateRequestHeader(Request);
			List<ApplicantApplicationForm> objApplicantApplicationForm = new List<ApplicantApplicationForm>();
			objApplicantApplicationForm = _admissionBll.GetApplicantRemarksListBLL(ApplicantTransId);
			int c = 1;
			foreach (var item in objApplicantApplicationForm)
			{
				item.slno = c;
				c++;
			}
			return Json(objApplicantApplicationForm, JsonRequestBehavior.AllowGet);
		}

		#region .. Master Data ..

		public JsonResult GetDistrictMasterListBLL()
		{
			var Institute_list = _admissionBll.GetDistrictMasterListBLL();
			return Json(Institute_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetEligibleDateFrmCalenderEvents()
		{
			var res = _admissionBll.GetEligibleDateFrmCalenderEventsBLL();
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetMasterApplicantData()
		{
			ApplicantApplicationForm ApplicantApplicationFormData = new ApplicantApplicationForm();
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			ApplicantApplicationFormData = _admissionBll.GetMasterApplicantDataBLL(loginId, "AP");
			List<ApplicationForm> PreferenceListDet = null;
			List<ApplicationForm> DistrictListDet = null;
			List<ApplicationForm> PreferenceInstituteDet = null;

			if (ApplicantApplicationFormData == null)
			{
				ApplicantApplicationFormData = new ApplicantApplicationForm();
			}

			ApplicantApplicationFormData.GetReligionList = _admissionBll.GetReligionDetailsBLL();
			ApplicantApplicationFormData.GetGenderList = _admissionBll.GetGenderDetailsBLL();
			ApplicantApplicationFormData.GetCategoryList = _admissionBll.GetCategoryListBLL();
			ApplicantApplicationFormData.GetApplicantTypeList = _admissionBll.GetApplicantTypeListBLL(Convert.ToInt32(Session["RoleId"].ToString()));
			ApplicantApplicationFormData.GetCasteList = _admissionBll.GetCasteListBLL();
			ApplicantApplicationFormData.PersonWithDisabilityCategory = _admissionBll.PersonWithDisabilityCategoryBLL();
			ApplicantApplicationFormData.GetReservationList = _admissionBll.GetReservationsListBLL();
			ApplicantApplicationFormData.GetQualificationList = _admissionBll.GetQualificationListBLL();
			ApplicantApplicationFormData.GetApplicableReservations = _admissionBll.GetApplicantReservationListBLL(ApplicantApplicationFormData.ApplicationId);
			ApplicantApplicationFormData.GetDistrictList = _admissionBll.GetDistrictMasterListBLL();
			ApplicantApplicationFormData.GetApplicantDocumentsDetail = _admissionBll.GetDocumentDetailsBLL(ApplicantApplicationFormData.ApplicationId);
			ApplicantApplicationFormData.GetApplicantInstitutePreference = _admissionBll.GetApplicantInstitutePreferenceBLL(ApplicantApplicationFormData.ApplicationId);
			ApplicantApplicationFormData.GetApplicantDocVerfiInstitutePreference = _admissionBll.GetDistrictMasterListBLL();
			ApplicantApplicationFormData.GetOtherBoards = _admissionBll.GetOtherBoardsListBLL();

			int i = 0;
			foreach (var SelectedApplicableReservation in ApplicantApplicationFormData.GetApplicableReservations)
			{
				if (i == 0)
					SelectedApplicableReservation.SelectedReservationId += SelectedApplicableReservation.ReservationId;
				else
					SelectedApplicableReservation.SelectedReservationId += "," + SelectedApplicableReservation.ReservationId;
				ApplicantApplicationFormData.SelectedReservationId += SelectedApplicableReservation.SelectedReservationId;
				i++;
			}
			ApplicantApplicationFormData.SelectedReservationId = ApplicantApplicationFormData.SelectedReservationId;
			foreach (var DocVerfiInstDet in ApplicantApplicationFormData.GetApplicantDocVerfiInstitutePreference)
			{
				DocVerfiInstDet.DocVerfiInstDet = _admissionBll.GetITICollegeDetailsBLL(DocVerfiInstDet.district_lgd_code);
			}

			foreach (var InstitTradeDet in ApplicantApplicationFormData.GetApplicantInstitutePreference)
			{
				InstitTradeDet.TalukDet = _admissionBll.GetTalukMasterListBLL(InstitTradeDet.DistrictId);
				InstitTradeDet.InstituteDet = _admissionBll.GetITICollegeDetailsByDistrictTalukaBLL(InstitTradeDet.DistrictId, InstitTradeDet.TalukaId);
				InstitTradeDet.TradeDet = _admissionBll.GetITICollegeTradeDetailsBLL(InstitTradeDet.InstituteId, ApplicantApplicationFormData.RAppBasics.ToString());
			}
			foreach (var TalukaDet in ApplicantApplicationFormData.GetDistrictList)
			{
				TalukaDet.TalukListDet = _admissionBll.GetTalukMasterListBLL(TalukaDet.district_lgd_code);
			}
            if (ApplicantApplicationFormData.ApplicationMode==0 || ApplicantApplicationFormData.ApplicationMode ==null)
            {
				int RoleId = Convert.ToInt32(Session["RoleId"].ToString());
                if (RoleId==10)
                {
					ApplicantApplicationFormData.ApplicationMode = 1;
					ApplicantApplicationFormData.ApplicantType = 1;

					
				}
			}

            if (!string.IsNullOrEmpty(loginId.ToString()))
            {
				ApplicantApplicationForm userMasterData = new ApplicantApplicationForm();
				userMasterData = _admissionBll.GetUserMasterDataBLL(loginId, "AP");
				ApplicantApplicationFormData.PhoneNumber = userMasterData.ApplicantNumber;
				ApplicantApplicationFormData.EmailId = userMasterData.EmailId;
			}

			if (!string.IsNullOrEmpty(loginId.ToString()))
			{
				ApplicantApplicationForm userMasterData = new ApplicantApplicationForm();
				userMasterData = _admissionBll.GetValidateRDNumberBll("", loginId,0);
				ApplicantApplicationFormData.ApplicantNumber = userMasterData.ApplicantNumber;
			}


			PreferenceListDet = _admissionBll.GetQualificationListBLL();
			DistrictListDet = _admissionBll.GetDistrictMasterListBLL();
			PreferenceInstituteDet = _admissionBll.GetLocationListBLL();
			return Json(new { Resultlist = ApplicantApplicationFormData, pref = PreferenceListDet, dist = DistrictListDet, InstPref = PreferenceInstituteDet }, JsonRequestBehavior.AllowGet);
		}

		#endregion

		public JsonResult GetRDNumverDetails(string RD_Number, int RDNumberType)
		{
			ApplicantApplicationForm userMasterData = new ApplicantApplicationForm();

			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int RoleId = Convert.ToInt32(Session["RoleId"].ToString());
		    userMasterData = _admissionBll.GetValidateRDNumberBll(RD_Number, loginId, RDNumberType);

			return Json(new { Resultlist = userMasterData }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetDocumentIndicatorOptionData(int ApplicationIdFromUI)
		{
			ApplicantApplicationForm objApplicantApplicationFormData = new ApplicantApplicationForm();
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int RoleId = Convert.ToInt32(Session["RoleId"].ToString());

			if (RoleId == 9)    //Checking for ITI Admin
				objApplicantApplicationFormData = _admissionBll.GetMasterApplicantDataBLL(ApplicationIdFromUI, "AP");
			else
				objApplicantApplicationFormData = _admissionBll.GetMasterApplicantDataBLL(loginId, "AP");

			//objApplicantApplicationFormData.GetApplicableReservations = _admissionBll.GetApplicantReservationListBLL(objApplicantApplicationFormData.ApplicationId);
			//int i = 0;
			//foreach (var SelectedApplicableReservation in objApplicantApplicationFormData.GetApplicableReservations)
			//{
			//    if (i == 0)
			//        SelectedApplicableReservation.SelectedReservationId += SelectedApplicableReservation.ReservationId;
			//    else
			//        SelectedApplicableReservation.SelectedReservationId += "," + SelectedApplicableReservation.ReservationId;
			//    objApplicantApplicationFormData.SelectedReservationId += SelectedApplicableReservation.SelectedReservationId;
			//    i++;
			//}
			//objApplicantApplicationFormData.SelectedReservationId = objApplicantApplicationFormData.SelectedReservationId;
			return Json(new { Resultlist = objApplicantApplicationFormData }, JsonRequestBehavior.AllowGet);
		}

		#region .. Save Applicant Data ..

		#region .. Save Documents ..

		[HttpPost]
		public JsonResult ApplicantDocumentDetails(ApplicantDocumentsDetail objApplicantDocumentsDetail)
		{
			ApplicantApplicationForm objApplicantApplicationForm = new ApplicantApplicationForm();
			try
			{
				Utilities.Security.ValidateRequestHeader(Request);
				string documentType = null; int documentTypeContent = 0; string FileNameFormat = null; int DocumentTypeValue = 0; string DocumentRemarks = null; int DocAppId = 0;
				int loginId = Convert.ToInt32(Session["LoginId"].ToString());
				var checkDocFormat = (dynamic)null;

				for (int i = 0; i < 16; i++)
				{
					if (i != 12 && i != 14) //Kasmir Migrants and Land Losers
					{
						if (i == 0)
							checkDocFormat = objApplicantDocumentsDetail.EduCertificatePDF;
						else if (i == 1)
							checkDocFormat = objApplicantDocumentsDetail.CasteCertificatePDF;
						else if (i == 2)
							checkDocFormat = objApplicantDocumentsDetail.RationcardPDF;
						else if (i == 3)
							checkDocFormat = objApplicantDocumentsDetail.IncomePDF;
						else if (i == 4)
							checkDocFormat = objApplicantDocumentsDetail.UIDNumberPDF;
						else if (i == 5)
							checkDocFormat = objApplicantDocumentsDetail.RuralPDF;
						else if (i == 6)
							checkDocFormat = objApplicantDocumentsDetail.KannadaMediumPDF;
						else if (i == 7)
							checkDocFormat = objApplicantDocumentsDetail.DifferentlyAbledPDF;
						else if (i == 8)
							checkDocFormat = objApplicantDocumentsDetail.StudyExemptedPDF;
						else if (i == 9)
							checkDocFormat = objApplicantDocumentsDetail.HyderabadKarnatakaRegionPDF;
						else if (i == 10)
							checkDocFormat = objApplicantDocumentsDetail.HoranaaduGadinaaduKannadigaPDF;
						else if (i == 11)
							checkDocFormat = objApplicantDocumentsDetail.OtherCertificatesPDF;
						else if (i == 12)
							checkDocFormat = objApplicantDocumentsDetail.KashmirMigrantsPDF;
						else if (i == 13)
							checkDocFormat = objApplicantDocumentsDetail.ExservicemanPDF;
						else if (i == 14)
							checkDocFormat = objApplicantDocumentsDetail.LLCertificatePDF;
						else if (i == 15)
							checkDocFormat = objApplicantDocumentsDetail.EWSCertificatePDF;

						if (objApplicantDocumentsDetail.GrievanceId != 0)
						{
							if (i == 7)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.DifferentlyAbledPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.DifferentlyAbledPDF.ContentLength;
									FileNameFormat = "DifferentlyAbledDoc";
								}
								DocumentTypeValue = 8;
								DocumentRemarks = objApplicantDocumentsDetail.DifferentlyAbledRemarks;
								DocAppId = objApplicantDocumentsDetail.DDocAppId;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.DiffAblDocStatus;
							}
							else if (i == 13)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.ExservicemanPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.ExservicemanPDF.ContentLength;
									FileNameFormat = "Exserviceman";
								}
								DocumentTypeValue = 14;
								DocumentRemarks = objApplicantDocumentsDetail.ExservicemanRemarks;
								DocAppId = objApplicantDocumentsDetail.ExSDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.ExSCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.ExserDocStatus;
							}
							else if (i == 15)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.EWSCertificatePDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.EWSCertificatePDF.ContentLength;
									FileNameFormat = "EWSCertificate";
								}
								DocumentTypeValue = 16;
								DocumentRemarks = objApplicantDocumentsDetail.EWSCertificateRemarks;
								DocAppId = objApplicantDocumentsDetail.EWSDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.EWSCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.EWSDocStatus;
							}

							if (i == 7 || i == 13 || i == 15)
							{
								string _path = ""; string UniqueFileName = null;
								if (checkDocFormat != null)
								{
									var supportedTypes = new[] { "pdf" };
									string extension = System.IO.Path.GetExtension(documentType).Substring(1);
									string _FileName = Path.GetFileName(documentType);
									extension = System.IO.Path.GetExtension(documentType).Substring(1);
									UniqueFileName = FileNameFormat + "_" + Guid.NewGuid().ToString() + "." + extension;
									_path = Path.Combine(Server.MapPath("Content/AppDocuments/"), UniqueFileName);
									string _pathCreate = Path.Combine(Server.MapPath("Content/AppDocuments/"));
									if (!Directory.Exists(_pathCreate))
									{
										Directory.CreateDirectory(_pathCreate);
									}
								}

								if (i == 7 && checkDocFormat != null)
									objApplicantDocumentsDetail.DifferentlyAbledPDF.SaveAs(_path);
								else if (i == 13 && checkDocFormat != null)
									objApplicantDocumentsDetail.ExservicemanPDF.SaveAs(_path);
								else if (i == 15 && checkDocFormat != null)
									objApplicantDocumentsDetail.EWSCertificatePDF.SaveAs(_path);

								objApplicantDocumentsDetail.FileName = null;
								if (checkDocFormat != null)
								{
									objApplicantDocumentsDetail.FileName = documentType;
									objApplicantDocumentsDetail.FilePath = "Content/AppDocuments/" + UniqueFileName;
								}
								objApplicantDocumentsDetail.DocumentTypeId = DocumentTypeValue;
								objApplicantDocumentsDetail.ApplicantId = objApplicantDocumentsDetail.ApplicantId;
								objApplicantDocumentsDetail.UpdatedBy = loginId;

								if (objApplicantDocumentsDetail.UploadedByVerfication == 1)
									objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.CreatedBy;
								else
									objApplicantDocumentsDetail.CreatedBy = loginId;

								objApplicantDocumentsDetail.DocAppId = DocAppId;
								objApplicantDocumentsDetail.DocumentRemarks = DocumentRemarks;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.Verified;
								objApplicantApplicationForm.GetApplicantDocumentsDetail = _admissionBll.ApplicantDocumentDetailsBLL(objApplicantDocumentsDetail);
							}
						}
						else if (checkDocFormat != null || objApplicantDocumentsDetail.UploadedByVerfication == 1)
						{
							if (i == 0)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.EduCertificatePDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.EduCertificatePDF.ContentLength;
									FileNameFormat = "EducationDoc";
								}
								DocumentTypeValue = 1;
								DocumentRemarks = objApplicantDocumentsDetail.EduCertificateRemarks;
								DocAppId = objApplicantDocumentsDetail.EDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.ECreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.EduDocStatus;
							}
							else if (i == 1)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.CasteCertificatePDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.CasteCertificatePDF.ContentLength;
									FileNameFormat = "CasteDoc";
								}
								DocumentTypeValue = 2;
								DocumentRemarks = objApplicantDocumentsDetail.CasteCertificateRemarks;
								DocAppId = objApplicantDocumentsDetail.CDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.CCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.CasDocStatus;
							}
							else if (i == 2)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.RationcardPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.RationcardPDF.ContentLength;
									FileNameFormat = "RationDoc";
								}
								DocumentTypeValue = 3;
								DocumentRemarks = objApplicantDocumentsDetail.RationcardRemarks;
								DocAppId = objApplicantDocumentsDetail.RDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.RCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.RationDocStatus;
							}
							else if (i == 3)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.IncomePDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.IncomePDF.ContentLength;
									FileNameFormat = "IncomeDoc";
								}
								DocumentTypeValue = 4;
								DocumentRemarks = objApplicantDocumentsDetail.IncomeRemarks;
								DocAppId = objApplicantDocumentsDetail.IDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.ICreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.IncCerDocStatus;
							}
							else if (i == 4)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.UIDNumberPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.UIDNumberPDF.ContentLength;
									FileNameFormat = "UIDNumberDoc";
								}
								DocumentTypeValue = 5;
								DocumentRemarks = objApplicantDocumentsDetail.UIDNumberRemarks;
								DocAppId = objApplicantDocumentsDetail.UDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.UCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.UIDDocStatus;
							}
							else if (i == 5)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.RuralPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.RuralPDF.ContentLength;
									FileNameFormat = "RuralDoc";
								}
								DocumentTypeValue = 6;
								DocumentRemarks = objApplicantDocumentsDetail.RuralRemarks;
								DocAppId = objApplicantDocumentsDetail.RUDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.RUCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.RUcerDocStatus;
							}
							else if (i == 6)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.KannadaMediumPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.KannadaMediumPDF.ContentLength;
									FileNameFormat = "KannadaMediumDoc";
								}
								DocumentTypeValue = 7;
								DocumentRemarks = objApplicantDocumentsDetail.KannadaMediumRemarks;
								DocAppId = objApplicantDocumentsDetail.KDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.KCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.KanMedCerDocStatus;
							}
							else if (i == 7)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.DifferentlyAbledPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.DifferentlyAbledPDF.ContentLength;
									FileNameFormat = "DifferentlyAbledDoc";
								}
								DocumentTypeValue = 8;
								DocumentRemarks = objApplicantDocumentsDetail.DifferentlyAbledRemarks;
								DocAppId = objApplicantDocumentsDetail.DDocAppId;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.DiffAblDocStatus;
							}
							else if (i == 8)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.StudyExemptedPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.StudyExemptedPDF.ContentLength;
									FileNameFormat = "StudyExemptedDoc";
								}
								DocumentTypeValue = 9;
								DocumentRemarks = objApplicantDocumentsDetail.StudyExemptedRemarks;
								DocAppId = objApplicantDocumentsDetail.ExDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.ExCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.ExCerDocStatus;
							}
							else if (i == 9)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.HyderabadKarnatakaRegionPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.HyderabadKarnatakaRegionPDF.ContentLength;
									FileNameFormat = "HyderabadKarnatakaRegionDoc";
								}
								DocumentTypeValue = 10;
								DocumentRemarks = objApplicantDocumentsDetail.HyderabadKarnatakaRegionRemarks;
								DocAppId = objApplicantDocumentsDetail.HDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.HCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.HyKarDocStatus;
							}
							else if (i == 10)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.HoranaaduGadinaaduKannadigaPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.HoranaaduGadinaaduKannadigaPDF.ContentLength;
									FileNameFormat = "HoranaaduGadinaaduKannadigaDoc";
								}
								DocumentTypeValue = 11;
								DocumentRemarks = objApplicantDocumentsDetail.HoranaaduGadinaaduKannadigaRemarks;
								DocAppId = objApplicantDocumentsDetail.HGKDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.HGKCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.HorKanDocStatus;
							}
							else if (i == 11)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.OtherCertificatesPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.OtherCertificatesPDF.ContentLength;
									FileNameFormat = "OthersDoc";
								}
								DocumentTypeValue = 12;
								DocumentRemarks = objApplicantDocumentsDetail.OtherCertificatesRemarks;
								DocAppId = objApplicantDocumentsDetail.ODocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.OCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.OtherCerDocStatus;
							}
							else if (i == 12)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.KashmirMigrantsPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.KashmirMigrantsPDF.ContentLength;
									FileNameFormat = "KashmirMigrants";
								}
								DocumentTypeValue = 13;
								DocumentRemarks = objApplicantDocumentsDetail.KashmirMigrantsRemarks;
								DocAppId = objApplicantDocumentsDetail.KMDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.KMCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.KasMigDocStatus;
							}
							else if (i == 13)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.ExservicemanPDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.ExservicemanPDF.ContentLength;
									FileNameFormat = "Exserviceman";
								}
								DocumentTypeValue = 14;
								DocumentRemarks = objApplicantDocumentsDetail.ExservicemanRemarks;
								DocAppId = objApplicantDocumentsDetail.ExSDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.ExSCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.ExserDocStatus;
							}
							else if (i == 14)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.LLCertificatePDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.LLCertificatePDF.ContentLength;
									FileNameFormat = "LLCertificate";
								}
								DocumentTypeValue = 15;
								DocumentRemarks = objApplicantDocumentsDetail.LLCertificateRemarks;
								DocAppId = objApplicantDocumentsDetail.LLDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.LLCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.LLCerDocStatus;
							}
							else if (i == 15)
							{
								if (checkDocFormat != null)
								{
									documentType = objApplicantDocumentsDetail.EWSCertificatePDF.FileName;
									documentTypeContent = objApplicantDocumentsDetail.EWSCertificatePDF.ContentLength;
									FileNameFormat = "EWSCertificate";
								}
								DocumentTypeValue = 16;
								DocumentRemarks = objApplicantDocumentsDetail.EWSCertificateRemarks;
								DocAppId = objApplicantDocumentsDetail.EWSDocAppId;
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.EWSCreatedBy;
								objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.EWSDocStatus;
							}

							string _path = ""; string UniqueFileName = null;
							if (checkDocFormat != null)
							{
								var supportedTypes = new[] { "pdf" };
								string extension = System.IO.Path.GetExtension(documentType).Substring(1);
								string _FileName = Path.GetFileName(documentType);
								extension = System.IO.Path.GetExtension(documentType).Substring(1);
								UniqueFileName = FileNameFormat + "_" + Guid.NewGuid().ToString() + "." + extension;
								_path = Path.Combine(Server.MapPath("Content/AppDocuments/"), UniqueFileName);
								string _pathCreate = Path.Combine(Server.MapPath("Content/AppDocuments/"));
								if (!Directory.Exists(_pathCreate))
								{
									Directory.CreateDirectory(_pathCreate);
								}
							}

							if (i == 0 && checkDocFormat != null)
								objApplicantDocumentsDetail.EduCertificatePDF.SaveAs(_path);
							else if (i == 1 && checkDocFormat != null)
								objApplicantDocumentsDetail.CasteCertificatePDF.SaveAs(_path);
							else if (i == 2 && checkDocFormat != null)
								objApplicantDocumentsDetail.RationcardPDF.SaveAs(_path);
							else if (i == 3 && checkDocFormat != null)
								objApplicantDocumentsDetail.IncomePDF.SaveAs(_path);
							else if (i == 4 && checkDocFormat != null)
								objApplicantDocumentsDetail.UIDNumberPDF.SaveAs(_path);
							else if (i == 5 && checkDocFormat != null)
								objApplicantDocumentsDetail.RuralPDF.SaveAs(_path);
							else if (i == 6 && checkDocFormat != null)
								objApplicantDocumentsDetail.KannadaMediumPDF.SaveAs(_path);
							else if (i == 7 && checkDocFormat != null)
								objApplicantDocumentsDetail.DifferentlyAbledPDF.SaveAs(_path);
							else if (i == 8 && checkDocFormat != null)
								objApplicantDocumentsDetail.StudyExemptedPDF.SaveAs(_path);
							else if (i == 9 && checkDocFormat != null)
								objApplicantDocumentsDetail.HyderabadKarnatakaRegionPDF.SaveAs(_path);
							else if (i == 10 && checkDocFormat != null)
								objApplicantDocumentsDetail.HoranaaduGadinaaduKannadigaPDF.SaveAs(_path);
							else if (i == 11 && checkDocFormat != null)
								objApplicantDocumentsDetail.OtherCertificatesPDF.SaveAs(_path);
							else if (i == 12 && checkDocFormat != null)
								objApplicantDocumentsDetail.KashmirMigrantsPDF.SaveAs(_path);
							else if (i == 13 && checkDocFormat != null)
								objApplicantDocumentsDetail.ExservicemanPDF.SaveAs(_path);
							else if (i == 14 && checkDocFormat != null)
								objApplicantDocumentsDetail.LLCertificatePDF.SaveAs(_path);
							else if (i == 15 && checkDocFormat != null)
								objApplicantDocumentsDetail.EWSCertificatePDF.SaveAs(_path);

							objApplicantDocumentsDetail.FileName = null;
							if (checkDocFormat != null)
							{
								objApplicantDocumentsDetail.FileName = documentType;
								objApplicantDocumentsDetail.FilePath = "Content/AppDocuments/" + UniqueFileName;
							}
							objApplicantDocumentsDetail.DocumentTypeId = DocumentTypeValue;
							objApplicantDocumentsDetail.ApplicantId = objApplicantDocumentsDetail.ApplicantId;
							objApplicantDocumentsDetail.UpdatedBy = loginId;

							if (objApplicantDocumentsDetail.UploadedByVerfication == 1)
								objApplicantDocumentsDetail.CreatedBy = objApplicantDocumentsDetail.CreatedBy;
							else
								objApplicantDocumentsDetail.CreatedBy = loginId;

							objApplicantDocumentsDetail.DocAppId = DocAppId;
							objApplicantDocumentsDetail.DocumentRemarks = DocumentRemarks;
							objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.Verified;
							try
							{
								objApplicantApplicationForm.GetApplicantDocumentsDetail = _admissionBll.ApplicantDocumentDetailsBLL(objApplicantDocumentsDetail);
								objApplicantApplicationForm.UpdateMsg = "success";
							}
							catch (Exception ex)
							{
								objApplicantApplicationForm.UpdateMsg = "failed";
							}
						}
					}
				}

				if (objApplicantDocumentsDetail.UploadedByVerfication == 1 && objApplicantDocumentsDetail.SaveAsDraft != 1)
				{
					ApplicationStatusUpdate objApplicationStatusUpdate = new ApplicationStatusUpdate();
					objApplicationStatusUpdate.ApplicantId = objApplicantDocumentsDetail.ApplicantId;
					objApplicationStatusUpdate.ApplStatus = objApplicantDocumentsDetail.VOApplStatus;
					objApplicationStatusUpdate.Remarks = objApplicantDocumentsDetail.VORemarks;
					objApplicationStatusUpdate.FlowId = objApplicantDocumentsDetail.FlowId;
					objApplicationStatusUpdate.ReVerficationStatus = objApplicantDocumentsDetail.ReVerficationStatus;
					objApplicationStatusUpdate.CredatedBy = objApplicantDocumentsDetail.CredatedBy;
					UpdateApplicationDetailsFromVOById(objApplicantDocumentsDetail);
					UpdateApplicationDetailsById(objApplicationStatusUpdate);
				}
			}
			catch (Exception ex)
			{
				objApplicantApplicationForm.UpdateMsg = "failed";
			}

			return Json(objApplicantApplicationForm);
		}

		#endregion

		#region .. Save Institute Data ..

		[HttpPost]
		public JsonResult SaveInstituePreferenceDetails(ApplicantApplicationForm formCollection)
		{
			List<ApplicantInstitutePreference> objApplicantInstitutePreference = new List<ApplicantInstitutePreference>();
			List<ApplicationForm> PreferenceListDet = null;
			List<ApplicationForm> DistrictListDet = null;
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			try
			{
				for (var i = 0; i < formCollection.PreferenceDetId.Count(); i++)
				{
					var model = new ApplicantInstitutePreference();
					model.PreferenceId = formCollection.PreferenceDetId[i];
					//model.PreferenceType = formCollection.PreferenceDetType[i];
					model.DistrictId = formCollection.DistrictDetId[i];
					model.TalukaId = formCollection.TalukaDetId[i];
					model.InstituteId = formCollection.InstituteDetId[i];
					model.TradeId = formCollection.TradeDetId[i];
					model.ApplicantId = formCollection.ApplicationId;
					if (formCollection.InstitutePreferenceId.Count != 0)
						model.InstitutePreferenceId = formCollection.InstitutePreferenceId[i];
					else
						model.InstitutePreferenceId = 0;

					model.CreatedBy = loginId;
					model.ParticipateNextRound = formCollection.ParticipateNextRound;
					objApplicantInstitutePreference = _admissionBll.AddInstituePreferenceDetailsBLL(model).ToList();
					foreach (var InstitTradeDet in objApplicantInstitutePreference)
					{
						InstitTradeDet.TalukDet = _admissionBll.GetTalukMasterListBLL(InstitTradeDet.DistrictId);
						InstitTradeDet.InstituteDet = _admissionBll.GetITICollegeDetailsByDistrictTalukaBLL(InstitTradeDet.DistrictId, InstitTradeDet.TalukaId);
						InstitTradeDet.TradeDet = _admissionBll.GetITICollegeTradeDetailsBLL(InstitTradeDet.InstituteId, formCollection.InstituteStudiedQual);
					}

					PreferenceListDet = _admissionBll.GetQualificationListBLL();
					DistrictListDet = _admissionBll.GetDistrictMasterListBLL();
				}
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception:" + ex.Message.ToString());
			}

			return Json(new { Resultlist = objApplicantInstitutePreference, pref = PreferenceListDet, dist = DistrictListDet }, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region .. Save Applicant, Education,  Address, Document Verification Centre

		[HttpPost]
		public JsonResult InsertApplicantFormDetails(ApplicantApplicationForm objApplicantApplicationForm)
		{
			bool success = false;
			Utilities.Security.ValidateRequestHeader(Request);
			UploadPreferenceType output = new UploadPreferenceType();
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			List<ApplicantApplicationForm> objReturnApplicationForm = new List<ApplicantApplicationForm>();

			try
			{
				HttpPostedFileBase upload = Request.Files["PhotoFile"];
				if (upload.ContentLength == 0)
				{
					ModelState.AddModelError("File", "Please Upload Your file");
				}
				else if (upload.ContentLength > 0)
				{
					string FileNameToCreate = "_UploadPhoto";
					string fileName = upload.FileName; // getting File Name
					string result;

					fileName = Path.GetFileNameWithoutExtension(upload.FileName);

					string fileContentType = upload.ContentType; // getting ContentType
					byte[] tempFileBytes = new byte[upload.ContentLength]; // getting filebytes
					var data = upload.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(upload.ContentLength));
					var types = FileUtility.FileType.Image;  // Setting Image type
															 //  ImageFileExtension imageFileExtension;
					string imageFile1, imageFile11;
					bool isvalid1, isvalid11;
					FileUtility.isValidFile(tempFileBytes, types, fileContentType, out imageFile1, out isvalid1); // Validate Header

					FileUtility.isValidImageFile(tempFileBytes, fileContentType, out imageFile11, out isvalid11);

					isvalid1 = true;
					if (isvalid1 == true)
					{
						int maxFileLength = 1024 * 3000; //FileLength 3 MB 
						if (upload.ContentLength > maxFileLength)
						{
							return this.Json(new { success, message = "Size of pdf Upload file size exceeded max file upload size(3 MB)!" }, JsonRequestBehavior.AllowGet);
						}
						else if (!Enum.IsDefined(typeof(FileUtility.ImageFileExtension), imageFile1))
						{
							return this.Json(new { success, message = "Please upload only image file!" }, JsonRequestBehavior.AllowGet);
						}
						else if (!Enum.IsDefined(typeof(FileUtility.ImageFileExtension), imageFile11))
						{
							return this.Json(new { success, message = "Please upload only image file!" }, JsonRequestBehavior.AllowGet);
						}
						else if (imageFile11 == "")
						{
							return this.Json(new { success, message = "Please upload only image file!" }, JsonRequestBehavior.AllowGet);
						}
						else
						{ }
						Match regex = Regex.Match(fileName, @"[\[\]{}!@#.]");

						if (regex.Success)
						{
							Log.Info("Entered InsertApplicantFormDetails()");
							return this.Json(new { success, message = "Please check uploaded file name!" }, JsonRequestBehavior.AllowGet);
						}
						char[] invalidFileChars = Path.GetInvalidFileNameChars();
						FileUtility.ShowChars(invalidFileChars);

						string UniqueFileName = null;
						UniqueFileName = FileNameToCreate + "." + imageFile11;

						string _path = Path.Combine(@"D:\UploadFile", UniqueFileName);
						//string _path = Path.Combine(Server.MapPath("~/Content/img"), UniqueFileName);
						FileInfo CheckFileName = new FileInfo(_path);

						string _pathCreate = Path.Combine(@"D:\UploadFile");
						if (!Directory.Exists(_pathCreate))
						{
							Directory.CreateDirectory(_pathCreate);
						}
						if (CheckFileName.Exists)
						{
							FileNameToCreate = FileNameToCreate + "_" + DateTime.Now.Ticks;
							UniqueFileName = FileNameToCreate + "." + imageFile11;
						}

						bool IsPermissionGranted = FileUtility.GrantFilePermission();

						//  upload.SaveAs(_path);
						//              UniqueFileName = FileNameFormat + "_" + Guid.NewGuid().ToString() + "." + extension;
						//string _path = Path.Combine(Server.MapPath("Content/AppDocuments/"), UniqueFileName);
						//string _pathCreate = Path.Combine(Server.MapPath("Content/AppDocuments/"));
						if (IsPermissionGranted)
						{
							objApplicantApplicationForm.PhotoFile.SaveAs(_path);
							objApplicantApplicationForm.FileName = UniqueFileName;
							objApplicantApplicationForm.FilePath = _path;
						}
						else
						{
							//	return Json("Uploaded file Folder Permissions are not granted");
							return this.Json(new { success, message = "Uploaded file Folder Permissions are not granted!" }, JsonRequestBehavior.AllowGet);
						}

					}
					else
					{
						return Json("Please check Uploaded file");
					}
				}
				else
				{
					return Json("Please check Uploaded file");
				}
				Log.Info("Entered InsertApplicantFormDetails()");

				if (objApplicantApplicationForm.DOB != null)
				{
					//DateTime dt = DateTime.ParseExact(objApplicantApplicationForm.DOB.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

					//var birthDate = new DateTime(objApplicantApplicationForm.DOB.Value.Day, objApplicantApplicationForm.DOB.Value.Month, objApplicantApplicationForm.DOB.Value.Year);
					//  objApplicantApplicationForm.DOB =birthDate;
					if (objApplicantApplicationForm.DOB > DateTime.Today)
					{
						return Json("Please check Date of Birth");
					}

					//string applicantdob = objApplicantApplicationForm.DOB.ToString("dd-MM-yyyy");
					//bool isValidDOB = DIT.Common.FieldServerValidation.IsDateBeforeOrToday(applicantdob);
					//if (!isValidDOB)
					//{
					//  return Json("Please check Date of Birth");
					//}
				}
				else
				{
					return Json("Please check Date of Birth");
				}
				if (objApplicantApplicationForm.FamilyAnnIncome < 0)
				{
					return Json("Please check Family Annual Income");
				}
				Log.Info("Entered InsertApplicantFormDetails()");
				//objApplicantApplicationForm.RationCard = objApplicantApplicationForm.RationCard != null ? DIT.Utilities.Encryption.DecryptStringAES(objApplicantApplicationForm.RationCard) : "";
				//objApplicantApplicationForm.AadhaarNumber = objApplicantApplicationForm.AadhaarNumber != null ? DIT.Utilities.Encryption.DecryptStringAES(objApplicantApplicationForm.AadhaarNumber) : "";
				objApplicantApplicationForm.AccountNumber = objApplicantApplicationForm.AccountNumber != null ? DIT.Utilities.Encryption.DecryptStringAES(objApplicantApplicationForm.AccountNumber) : "";
				objApplicantApplicationForm.BankName = objApplicantApplicationForm.BankName != null ? DIT.Utilities.Encryption.DecryptStringAES(objApplicantApplicationForm.BankName) : "";
				objApplicantApplicationForm.IFSCCode = objApplicantApplicationForm.IFSCCode != null ? DIT.Utilities.Encryption.DecryptStringAES(objApplicantApplicationForm.IFSCCode) : "";
				objApplicantApplicationForm.ApplicantId = objApplicantApplicationForm.ApplicationId;
				objApplicantApplicationForm.IsActive = true;
				objApplicantApplicationForm.CredatedBy = loginId;
				if (objApplicantApplicationForm.PaymentOptionval == false && objApplicantApplicationForm.DocumentFeeReceiptDetails != "")
				{
					objApplicantApplicationForm.ApplStatus = 5;
					objApplicantApplicationForm.ApplRemarks = "";
				}
				else
				{
					objApplicantApplicationForm.ApplStatus = 18;
					objApplicantApplicationForm.ApplRemarks = "Verification Payment Receipt not Submitted";
				}
				objReturnApplicationForm = _admissionBll.InsertApplicantFormDetailsBLL(objApplicantApplicationForm);

				if (objReturnApplicationForm == null)
				{
					output.flag = 0;
					output.status = "Error occured!";
				}
				else
				{
					output.flag = 1;
					output.status = "Payment of Document Verification fees updated Sucessfully!";
				}


				Log.Info("Left InsertApplicantFormDetails()");
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception:" + ex.Message.ToString());
				output.flag = 0;
				output.status = "Error occured!";
			}
			//return Json(objReturnApplicationForm);
			return Json(new { objReturnApplicationForm, pref = output }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult SaveEducationDetails(ApplicantApplicationForm objApplicantApplicationForm)
		{
			Utilities.Security.ValidateRequestHeader(Request);
			UploadPreferenceType output = new UploadPreferenceType();
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			List<ApplicantApplicationForm> objReturnApplicationForm = new List<ApplicantApplicationForm>();

			try
			{
				Log.Info("Entered SaveEducationDetails()");
				objApplicantApplicationForm.ApplicantId = objApplicantApplicationForm.ApplicationId;
				objApplicantApplicationForm.IsActive = true;
				objApplicantApplicationForm.CredatedBy = loginId;
				if (objApplicantApplicationForm.PaymentOptionval == false && objApplicantApplicationForm.DocumentFeeReceiptDetails != "")
				{
					objApplicantApplicationForm.ApplStatus = 5;
					objApplicantApplicationForm.ApplRemarks = "";
				}
				else
				{
					objApplicantApplicationForm.ApplStatus = 18;
					objApplicantApplicationForm.ApplRemarks = "Verification Payment Receipt not Submitted";
				}
				objReturnApplicationForm = _admissionBll.SaveEducationDetailsBLL(objApplicantApplicationForm);

				if (objReturnApplicationForm == null)
				{
					output.flag = 0;
					output.status = "Error occured!";
				}
				else
				{
					output.flag = 1;
					output.status = "Data Updated Sucessfully!";
				}

				Log.Info("Left SaveEducationDetails()");
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception:" + ex.Message.ToString());
				output.flag = 0;
				output.status = "Error occured!";
			}

			return Json(new { objReturnApplicationForm, pref = output }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult SaveAddressDetails(ApplicantApplicationForm objApplicantApplicationForm)
		{
			Utilities.Security.ValidateRequestHeader(Request);
			UploadPreferenceType output = new UploadPreferenceType();
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			List<ApplicantApplicationForm> objReturnApplicationForm = new List<ApplicantApplicationForm>();

			try
			{
				Log.Info("Entered SaveAddressDetails()");
				objApplicantApplicationForm.ApplicantId = objApplicantApplicationForm.ApplicationId;
				objApplicantApplicationForm.IsActive = true;
				objApplicantApplicationForm.CredatedBy = loginId;
				if (objApplicantApplicationForm.PaymentOptionval == false && objApplicantApplicationForm.DocumentFeeReceiptDetails != "")
				{
					objApplicantApplicationForm.ApplStatus = 5;
					objApplicantApplicationForm.ApplRemarks = "";
				}
				else
				{
					objApplicantApplicationForm.ApplStatus = 18;
					objApplicantApplicationForm.ApplRemarks = "Verification Payment Receipt not Submitted";
				}
				objReturnApplicationForm = _admissionBll.SaveAddressDetailsBLL(objApplicantApplicationForm);

				if (objReturnApplicationForm == null)
				{
					output.flag = 0;
					output.status = "Error occured!";
				}
				else
				{
					output.flag = 1;
					output.status = "Data Updated Sucessfully!";
				}

				Log.Info("Left SaveAddressDetails()");
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception:" + ex.Message.ToString());
				output.flag = 0;
				output.status = "Error occured!";
			}

			return Json(new { objReturnApplicationForm, pref = output }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult SaveInstitueDetails(ApplicantApplicationForm objApplicantApplicationForm)
		{
			Utilities.Security.ValidateRequestHeader(Request);
			UploadPreferenceType output = new UploadPreferenceType();
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			List<ApplicantApplicationForm> objReturnApplicationForm = new List<ApplicantApplicationForm>();

			try
			{
				Log.Info("Entered SaveInstitueDetails()");
				objApplicantApplicationForm.ApplicantId = objApplicantApplicationForm.ApplicationId;
				objApplicantApplicationForm.IsActive = true;
				objApplicantApplicationForm.CredatedBy = loginId;
				if (objApplicantApplicationForm.PaymentOptionval == false && objApplicantApplicationForm.DocumentFeeReceiptDetails != "")
				{
					objApplicantApplicationForm.ApplStatus = 5;
					objApplicantApplicationForm.ApplRemarks = "";
				}
				else
				{
					objApplicantApplicationForm.ApplStatus = 18;
					objApplicantApplicationForm.ApplRemarks = "Verification Payment Receipt not Submitted";
				}
				objReturnApplicationForm = _admissionBll.SaveInstitueDetailsBLL(objApplicantApplicationForm);

				if (objReturnApplicationForm == null)
				{
					output.flag = 0;
					output.status = "Error occured!";
				}
				else
				{
					output.flag = 1;
					output.status = "Data Updated Sucessfully!";
				}

				Log.Info("Left SaveInstitueDetails()");
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception:" + ex.Message.ToString());
				output.flag = 0;
				output.status = "Error occured!";
			}

			return Json(new { objReturnApplicationForm, pref = output }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult CheckNameAvailability(string strName, int ApplicationId, int AadhaarRollNumber)
		{
			var DBVal = _admissionBll.CheckNameAvailabilityBLL(strName, ApplicationId, AadhaarRollNumber);
			return Json(DBVal, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult CheckPhoneNumberAvailability(string strName, int ApplicationId, int AadhaarRollNumber)
		{
			var DBVal = _admissionBll.CheckPhoneNumberAvailabilityBLL(strName, ApplicationId, AadhaarRollNumber);
			return Json(DBVal, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult CheckEmailIdAvailability(string strName, int ApplicationId, int AadhaarRollNumber)
		{
			var DBVal = _admissionBll.CheckEmailIdAvailabilityBll(strName, ApplicationId, AadhaarRollNumber);
			return Json(DBVal, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region ..  Generate Application Reference Number Details

		[HttpPost]
		public JsonResult GenerateApplicationNumber(ApplicantApplicationForm objApplicantApplicationForm)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			objApplicantApplicationForm.CredatedBy = loginId;
			string ApplicantMobileNumber = objApplicantApplicationForm.PhoneNumber;
			string ApplicantName = objApplicantApplicationForm.ApplicantName;

			objApplicantApplicationForm = _admissionBll.GenerateApplicationNumberBLL(objApplicantApplicationForm);
            if (objApplicantApplicationForm!=null)
            {
                try
                {
					string Message = "Dear "+ ApplicantName + ",Application form submitted successfully. Please save the Generated Application No."+ objApplicantApplicationForm.ApplicantNumber + " for future communication. Please get the documents by paying the verification fee  50 at selected Govt ITI." + Environment.NewLine + " From " + Environment.NewLine + " CITE Admission Team";
					string templateid = WebConfigurationManager.AppSettings["NewApplicantRegistrationSMS"];
					//string templateid = "1107165019255858687";
					var OTPSuccuessFailure = SMSHttpPostClient.SendOTPMSG(ApplicantMobileNumber, Message, templateid);
				}
                catch (Exception ex)
                {

                }
            }
			return Json(objApplicantApplicationForm, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region .. Save Payment Details

		[HttpPost]
		public JsonResult InsertPaymentDetails(ApplicantApplicationForm objApplicantApplicationForm)
		{

			//Utilities.Security.ValidateRequestHeader(Request);
			UploadPreferenceType output = new UploadPreferenceType();
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			List<ApplicantApplicationForm> objReturnApplicationForm = new List<ApplicantApplicationForm>();
			objApplicantApplicationForm.CredatedBy = loginId;
			try
			{
				Log.Info("Entered InsertPaymentDetails()");
				objApplicantApplicationForm.ApplicantId = objApplicantApplicationForm.ApplicantId;
				objApplicantApplicationForm.IsActive = true;
				objApplicantApplicationForm.CredatedBy = loginId;
				if (objApplicantApplicationForm.PaymentOptionval == false && (objApplicantApplicationForm.DocumentFeeReceiptDetails != "" && objApplicantApplicationForm.DocumentFeeReceiptDetails != null))
				{
					objApplicantApplicationForm.ApplStatus = 5;
					objApplicantApplicationForm.ApplRemarks = "";
				}
				else
				{
					objApplicantApplicationForm.ApplStatus = 5;
					objApplicantApplicationForm.ApplRemarks = "Verification Payment Receipt not Submitted";
				}
				objReturnApplicationForm = _admissionBll.InsertPaymentDetailsBLL(objApplicantApplicationForm);

				if (objReturnApplicationForm == null)
				{
					output.flag = 0;
					output.status = "There is Error in your Payment of Document verification fee data";
				}
				else
				{
					output.flag = 1;
					output.status = "Payment of Document verification fee collected and Updated Sucessfully!";
				}


				Log.Info("Left InsertPaymentDetails()");
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception:" + ex.Message.ToString());
				output.flag = 0;
				output.status = "Error occured!";
			}
			//return Json(objReturnApplicationForm);
			return Json(new { objReturnApplicationForm, pref = output }, JsonRequestBehavior.AllowGet);
		}


		#endregion

		#region .. Grievance Document Details

		[HttpPost]
		public JsonResult ApplicantGrievanceDetails(GrievanceDocApplData objGrievanceDocApplData)
		{
			ApplicantApplicationForm objApplicantApplicationForm = new ApplicantApplicationForm();
			try
			{
				Utilities.Security.ValidateRequestHeader(Request);
				string documentType = null; int documentTypeContent = 0; string FileNameFormat = null; int DocumentTypeValue = 0; string DocumentRemarks = null; int DocAppId = 0;
				int loginId = Convert.ToInt32(Session["LoginId"].ToString());

				for (int i = 0; i < 16; i++)
				{
					if (i == 0 || i == 1 || i == 4 || i == 5 || i == 7 || i == 9 || i == 13 || i == 15)
					{
						var checkDocFormat = (dynamic)null;
						if (i == 0 && objGrievanceDocApplData.DocumentSet[0] == 1)
							checkDocFormat = objGrievanceDocApplData.EduCertificatePDF;
						else if (i == 1 && objGrievanceDocApplData.DocumentSet[1] == 1)
							checkDocFormat = objGrievanceDocApplData.CasteCertificatePDF;
						else if (i == 4 && objGrievanceDocApplData.DocumentSet[2] == 1)
							checkDocFormat = objGrievanceDocApplData.UIDNumberPDF;
						else if (i == 5 && objGrievanceDocApplData.DocumentSet[3] == 1)
							checkDocFormat = objGrievanceDocApplData.RuralPDF;
						else if (i == 7 && objGrievanceDocApplData.DocumentSet[4] == 1)
							checkDocFormat = objGrievanceDocApplData.DifferentlyAbledPDF;
						else if (i == 9 && objGrievanceDocApplData.DocumentSet[5] == 1)
							checkDocFormat = objGrievanceDocApplData.HyderabadKarnatakaRegionPDF;
						else if (i == 13 && objGrievanceDocApplData.DocumentSet[6] == 1)
							checkDocFormat = objGrievanceDocApplData.ExservicemanPDF;
						else if (i == 15 && objGrievanceDocApplData.DocumentSet[7] == 1)
							checkDocFormat = objGrievanceDocApplData.EWSCertificatePDF;

						if (checkDocFormat != null)
						{
							if (i == 0)
							{
								if (checkDocFormat != null)
								{
									documentType = objGrievanceDocApplData.EduCertificatePDF.FileName;
									documentTypeContent = objGrievanceDocApplData.EduCertificatePDF.ContentLength;
									FileNameFormat = "EducationDoc";
								}
								DocumentTypeValue = 1;
								DocumentRemarks = objGrievanceDocApplData.EduCertificateRemarks;
								DocAppId = objGrievanceDocApplData.EDocAppId;
								objGrievanceDocApplData.CreatedBy = objGrievanceDocApplData.ECreatedBy;
								objGrievanceDocApplData.Verified = objGrievanceDocApplData.EduDocStatus;
							}
							else if (i == 1)
							{
								if (checkDocFormat != null)
								{
									documentType = objGrievanceDocApplData.CasteCertificatePDF.FileName;
									documentTypeContent = objGrievanceDocApplData.CasteCertificatePDF.ContentLength;
									FileNameFormat = "CasteDoc";
								}
								DocumentTypeValue = 2;
								DocumentRemarks = objGrievanceDocApplData.CasteCertificateRemarks;
								DocAppId = objGrievanceDocApplData.CDocAppId;
								objGrievanceDocApplData.CreatedBy = objGrievanceDocApplData.CCreatedBy;
								objGrievanceDocApplData.Verified = objGrievanceDocApplData.CasDocStatus;
							}
							else if (i == 4)
							{
								if (checkDocFormat != null)
								{
									documentType = objGrievanceDocApplData.UIDNumberPDF.FileName;
									documentTypeContent = objGrievanceDocApplData.UIDNumberPDF.ContentLength;
									FileNameFormat = "UIDNumberDoc";
								}
								DocumentTypeValue = 5;
								DocumentRemarks = objGrievanceDocApplData.UIDNumberRemarks;
								DocAppId = objGrievanceDocApplData.UDocAppId;
								objGrievanceDocApplData.CreatedBy = objGrievanceDocApplData.UCreatedBy;
								objGrievanceDocApplData.Verified = objGrievanceDocApplData.UIDDocStatus;
							}
							else if (i == 5)
							{
								if (checkDocFormat != null)
								{
									documentType = objGrievanceDocApplData.RuralPDF.FileName;
									documentTypeContent = objGrievanceDocApplData.RuralPDF.ContentLength;
									FileNameFormat = "RuralDoc";
								}
								DocumentTypeValue = 6;
								DocumentRemarks = objGrievanceDocApplData.RuralRemarks;
								DocAppId = objGrievanceDocApplData.RUDocAppId;
								objGrievanceDocApplData.CreatedBy = objGrievanceDocApplData.RUCreatedBy;
								objGrievanceDocApplData.Verified = objGrievanceDocApplData.RUcerDocStatus;
							}
							else if (i == 7)
							{
								if (checkDocFormat != null)
								{
									documentType = objGrievanceDocApplData.DifferentlyAbledPDF.FileName;
									documentTypeContent = objGrievanceDocApplData.DifferentlyAbledPDF.ContentLength;
									FileNameFormat = "DifferentlyAbledDoc";
								}
								DocumentTypeValue = 8;
								DocumentRemarks = objGrievanceDocApplData.DifferentlyAbledRemarks;
								DocAppId = objGrievanceDocApplData.DDocAppId;
								objGrievanceDocApplData.Verified = objGrievanceDocApplData.DiffAblDocStatus;
							}
							else if (i == 9)
							{
								if (checkDocFormat != null)
								{
									documentType = objGrievanceDocApplData.HyderabadKarnatakaRegionPDF.FileName;
									documentTypeContent = objGrievanceDocApplData.HyderabadKarnatakaRegionPDF.ContentLength;
									FileNameFormat = "HyderabadKarnatakaRegionDoc";
								}
								DocumentTypeValue = 10;
								DocumentRemarks = objGrievanceDocApplData.HyderabadKarnatakaRegionRemarks;
								DocAppId = objGrievanceDocApplData.HDocAppId;
								objGrievanceDocApplData.CreatedBy = objGrievanceDocApplData.HCreatedBy;
								objGrievanceDocApplData.Verified = objGrievanceDocApplData.HyKarDocStatus;
							}
							else if (i == 13)
							{
								if (checkDocFormat != null)
								{
									documentType = objGrievanceDocApplData.ExservicemanPDF.FileName;
									documentTypeContent = objGrievanceDocApplData.ExservicemanPDF.ContentLength;
									FileNameFormat = "Exserviceman";
								}
								DocumentTypeValue = 14;
								DocumentRemarks = objGrievanceDocApplData.ExservicemanRemarks;
								DocAppId = objGrievanceDocApplData.ExSDocAppId;
								objGrievanceDocApplData.CreatedBy = objGrievanceDocApplData.ExSCreatedBy;
								objGrievanceDocApplData.Verified = objGrievanceDocApplData.ExserDocStatus;
							}
							else if (i == 15)
							{
								if (checkDocFormat != null)
								{
									documentType = objGrievanceDocApplData.EWSCertificatePDF.FileName;
									documentTypeContent = objGrievanceDocApplData.EWSCertificatePDF.ContentLength;
									FileNameFormat = "EWSCertificate";
								}
								DocumentTypeValue = 16;
								DocumentRemarks = objGrievanceDocApplData.EWSCertificateRemarks;
								DocAppId = objGrievanceDocApplData.EWSDocAppId;
								objGrievanceDocApplData.CreatedBy = objGrievanceDocApplData.EWSCreatedBy;
								objGrievanceDocApplData.Verified = objGrievanceDocApplData.EWSDocStatus;
							}

							string _path = ""; string UniqueFileName = null;
							if (checkDocFormat != null)
							{
								var supportedTypes = new[] { "pdf" };
								string extension = System.IO.Path.GetExtension(documentType).Substring(1);
								string _FileName = Path.GetFileName(documentType);
								extension = System.IO.Path.GetExtension(documentType).Substring(1);
								UniqueFileName = FileNameFormat + "_" + Guid.NewGuid().ToString() + "." + extension;
								_path = Path.Combine(Server.MapPath("Content/AppDocuments/"), UniqueFileName);
								string _pathCreate = Path.Combine(Server.MapPath("Content/AppDocuments/"));
								if (!Directory.Exists(_pathCreate))
								{
									Directory.CreateDirectory(_pathCreate);
								}
							}

							if (i == 0 && checkDocFormat != null)
								objGrievanceDocApplData.EduCertificatePDF.SaveAs(_path);
							else if (i == 1 && checkDocFormat != null)
								objGrievanceDocApplData.CasteCertificatePDF.SaveAs(_path);
							else if (i == 4 && checkDocFormat != null)
								objGrievanceDocApplData.UIDNumberPDF.SaveAs(_path);
							else if (i == 5 && checkDocFormat != null)
								objGrievanceDocApplData.RuralPDF.SaveAs(_path);
							else if (i == 7 && checkDocFormat != null)
								objGrievanceDocApplData.DifferentlyAbledPDF.SaveAs(_path);
							else if (i == 9 && checkDocFormat != null)
								objGrievanceDocApplData.HyderabadKarnatakaRegionPDF.SaveAs(_path);
							else if (i == 13 && checkDocFormat != null)
								objGrievanceDocApplData.ExservicemanPDF.SaveAs(_path);
							else if (i == 15 && checkDocFormat != null)
								objGrievanceDocApplData.EWSCertificatePDF.SaveAs(_path);

							objGrievanceDocApplData.FileName = null;
							if (checkDocFormat != null)
							{
								objGrievanceDocApplData.FileName = documentType;
								objGrievanceDocApplData.FilePath = "Content/AppDocuments/" + UniqueFileName;
							}
							objGrievanceDocApplData.DocumentTypeId = DocumentTypeValue;
							objGrievanceDocApplData.ApplicantId = objGrievanceDocApplData.ApplicantId;
							objGrievanceDocApplData.UpdatedBy = loginId;

							objGrievanceDocApplData.CreatedBy = loginId;
							objGrievanceDocApplData.DocumentSetInd = 2;

							objGrievanceDocApplData.DocAppId = DocAppId;
							objGrievanceDocApplData.DocumentRemarks = DocumentRemarks;
							objGrievanceDocApplData.Verified = objGrievanceDocApplData.Verified;
							try
							{
								objApplicantApplicationForm.GrievanceDocApplData = _admissionBll.ApplicantGrievanceDocumentDetailsBLL(objGrievanceDocApplData);
								objApplicantApplicationForm.UpdateMsg = "success";
							}
							catch (Exception ex)
							{
								objApplicantApplicationForm.UpdateMsg = "failed";
							}
						}
					}
				}
				objGrievanceDocApplData.LoginId = loginId;
				try
				{
					objApplicantApplicationForm.GrievanceDocApplData = _admissionBll.ApplicantGrievanceDetailsBLL(objGrievanceDocApplData);
					objApplicantApplicationForm.UpdateMsg = "success";
				}
				catch (Exception ex)
				{
					objApplicantApplicationForm.UpdateMsg = "failed";
				}
			}
			catch (Exception ex)
			{
				objApplicantApplicationForm.UpdateMsg = "failed";
			}

			return Json(objApplicantApplicationForm);
		}

		#endregion

		#region .. Save All Data

		[HttpPost]
		public JsonResult InsertCombineApplicantFormDetails(ApplicantCombinedData objApplicantCombinedData)
		{
			UploadPreferenceType output = new UploadPreferenceType();
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			List<ApplicantApplicationForm> objReturnApplicationForm = new List<ApplicantApplicationForm>();

			try
			{
				Utilities.Security.ValidateRequestHeader(Request);
				//General Information Update Starts
				Log.Info("Entered InsertApplicantFormDetails()");
				objApplicantCombinedData.ApplicantGeneralDetails.IsActive = true;
				objApplicantCombinedData.ApplicantGeneralDetails.CredatedBy = loginId;
				objApplicantCombinedData.ApplicantGeneralDetails.ApplStatus = 5;
				objApplicantCombinedData.ApplicantGeneralDetails.ApplRemarks = "Submitted";
				objReturnApplicationForm = _admissionBll.InsertApplicantFormDetailsBLL(objApplicantCombinedData.ApplicantGeneralDetails);

				if (objReturnApplicationForm == null)
				{
					output.flag = 0;
					output.status = "Error occured!";
				}
				else
				{
					output.flag = 1;
					output.status = "Data Updated Sucessfully!";
				}

				Log.Info("Left InsertApplicantFormDetails()");
				//General Information Update Ends

				//Institute Update starts
				List<ApplicantInstitutePreference> objApplicantInstitutePreference = new List<ApplicantInstitutePreference>();
				List<ApplicationForm> PreferenceListDet = null;
				List<ApplicationForm> DistrictListDet = null;

				for (var i = 0; i < objApplicantCombinedData.ApplicantGeneralDetails.PreferenceDetId.Count(); i++)
				{
					var model = new ApplicantInstitutePreference();
					model.PreferenceId = objApplicantCombinedData.ApplicantGeneralDetails.PreferenceDetId[i];
					model.PreferenceType = objApplicantCombinedData.ApplicantGeneralDetails.PreferenceDetType[i];
					model.DistrictId = objApplicantCombinedData.ApplicantGeneralDetails.DistrictDetId[i];
					model.InstituteId = objApplicantCombinedData.ApplicantGeneralDetails.InstituteDetId[i];
					model.TradeId = objApplicantCombinedData.ApplicantGeneralDetails.TradeDetId[i];
					model.ApplicantId = objApplicantCombinedData.ApplicantGeneralDetails.ApplicationId;
					model.InstitutePreferenceId = objApplicantCombinedData.ApplicantGeneralDetails.InstitutePreferenceId[i];
					objApplicantInstitutePreference = _admissionBll.AddInstituePreferenceDetailsBLL(model).ToList();
					foreach (var InstitTradeDet in objApplicantInstitutePreference)
					{
						InstitTradeDet.InstituteDet = _admissionBll.GetITICollegeDetailsBLL(InstitTradeDet.DistrictId);
						InstitTradeDet.TradeDet = _admissionBll.GetITICollegeTradeDetailsBLL(InstitTradeDet.InstituteId, objApplicantCombinedData.ApplicantGeneralDetails.InstituteStudiedQual);
					}

					PreferenceListDet = _admissionBll.GetQualificationListBLL();
					DistrictListDet = _admissionBll.GetDistrictMasterListBLL();
				}
				//Institute Update Ends

				//Document Upate starts
				string documentType = null; int documentTypeContent = 0; string FileNameFormat = null; int DocumentTypeValue = 0;
				var checkDocFormat = (dynamic)null;

				for (int i = 0; i <= 11; i++)
				{
					if (i == 0)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.EduCertificatePDF;
					else if (i == 1)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.CasteCertificatePDF;
					else if (i == 2)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.RationcardPDF;
					else if (i == 3)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.IncomePDF;
					else if (i == 4)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.UIDNumberPDF;
					else if (i == 5)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.RuralPDF;
					else if (i == 6)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.KannadaMediumPDF;
					else if (i == 7)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.DifferentlyAbledPDF;
					else if (i == 8)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.StudyExemptedPDF;
					else if (i == 9)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.HyderabadKarnatakaRegionPDF;
					else if (i == 10)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.HoranaaduGadinaaduKannadigaPDF;
					else if (i == 11)
						checkDocFormat = objApplicantCombinedData.ApplicantDocumentDetails.OtherCertificatesPDF;

					if (checkDocFormat != null)
					{
						if (i == 0)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.EduCertificatePDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.EduCertificatePDF.ContentLength;
							FileNameFormat = "EducationDoc";
							DocumentTypeValue = 1;
						}
						else if (i == 1)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.CasteCertificatePDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.CasteCertificatePDF.ContentLength;
							FileNameFormat = "CasteDoc";
							DocumentTypeValue = 2;
						}
						else if (i == 2)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.RationcardPDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.RationcardPDF.ContentLength;
							FileNameFormat = "RationDoc";
							DocumentTypeValue = 3;
						}
						else if (i == 3)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.IncomePDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.IncomePDF.ContentLength;
							FileNameFormat = "IncomeDoc";
							DocumentTypeValue = 4;
						}
						else if (i == 4)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.UIDNumberPDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.UIDNumberPDF.ContentLength;
							FileNameFormat = "UIDNumberDoc";
							DocumentTypeValue = 5;
						}
						else if (i == 5)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.RuralPDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.RuralPDF.ContentLength;
							FileNameFormat = "RuralDoc";
							DocumentTypeValue = 6;
						}
						else if (i == 6)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.KannadaMediumPDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.KannadaMediumPDF.ContentLength;
							FileNameFormat = "KannadaMediumDoc";
							DocumentTypeValue = 7;
						}
						else if (i == 7)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.DifferentlyAbledPDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.DifferentlyAbledPDF.ContentLength;
							FileNameFormat = "DifferentlyAbledDoc";
							DocumentTypeValue = 8;
						}
						else if (i == 8)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.StudyExemptedPDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.StudyExemptedPDF.ContentLength;
							FileNameFormat = "StudyExemptedDoc";
							DocumentTypeValue = 9;
						}
						else if (i == 9)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.HyderabadKarnatakaRegionPDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.HyderabadKarnatakaRegionPDF.ContentLength;
							FileNameFormat = "HyderabadKarnatakaRegionDoc";
							DocumentTypeValue = 10;
						}
						else if (i == 10)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.HoranaaduGadinaaduKannadigaPDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.HoranaaduGadinaaduKannadigaPDF.ContentLength;
							FileNameFormat = "HoranaaduGadinaaduKannadigaDoc";
							DocumentTypeValue = 11;
						}
						else if (i == 11)
						{
							documentType = objApplicantCombinedData.ApplicantDocumentDetails.OtherCertificatesPDF.FileName;
							documentTypeContent = objApplicantCombinedData.ApplicantDocumentDetails.OtherCertificatesPDF.ContentLength;
							FileNameFormat = "OthersDoc";
							DocumentTypeValue = 12;
						}

						int maxcontentlength = 1024 * 3000;     //3MB
						var supportedTypes = new[] { "pdf" };
						string extension = System.IO.Path.GetExtension(documentType).Substring(1);
						if (!supportedTypes.Contains(extension))
						{
							return Json("Please upload only pdf file");
						}

						if (documentTypeContent > maxcontentlength)
						{
							//ModelState.AddModelError(string.Empty, "Size of pdf Upload file size exceeded max file upload size(3 MB) ");
							return Json("Size of pdf Upload file size exceeded max file upload size(3 MB)");
						}

						string UniqueFileName = null;
						string _FileName = Path.GetFileName(documentType);
						extension = System.IO.Path.GetExtension(documentType).Substring(1);
						UniqueFileName = FileNameFormat + "_" + Guid.NewGuid().ToString() + "." + extension;
						string _path = Path.Combine(Server.MapPath("Content/AppDocuments/"), UniqueFileName);
						string _pathCreate = Path.Combine(Server.MapPath("Content/AppDocuments/"));
						if (!Directory.Exists(_pathCreate))
						{
							Directory.CreateDirectory(_pathCreate);
						}

						if (i == 0)
							objApplicantCombinedData.ApplicantDocumentDetails.EduCertificatePDF.SaveAs(_path);
						else if (i == 1)
							objApplicantCombinedData.ApplicantDocumentDetails.CasteCertificatePDF.SaveAs(_path);
						else if (i == 2)
							objApplicantCombinedData.ApplicantDocumentDetails.RationcardPDF.SaveAs(_path);
						else if (i == 3)
							objApplicantCombinedData.ApplicantDocumentDetails.IncomePDF.SaveAs(_path);
						else if (i == 4)
							objApplicantCombinedData.ApplicantDocumentDetails.UIDNumberPDF.SaveAs(_path);
						else if (i == 5)
							objApplicantCombinedData.ApplicantDocumentDetails.RuralPDF.SaveAs(_path);
						else if (i == 6)
							objApplicantCombinedData.ApplicantDocumentDetails.KannadaMediumPDF.SaveAs(_path);
						else if (i == 7)
							objApplicantCombinedData.ApplicantDocumentDetails.DifferentlyAbledPDF.SaveAs(_path);
						else if (i == 8)
							objApplicantCombinedData.ApplicantDocumentDetails.StudyExemptedPDF.SaveAs(_path);
						else if (i == 9)
							objApplicantCombinedData.ApplicantDocumentDetails.HyderabadKarnatakaRegionPDF.SaveAs(_path);
						else if (i == 10)
							objApplicantCombinedData.ApplicantDocumentDetails.HoranaaduGadinaaduKannadigaPDF.SaveAs(_path);
						else if (i == 11)
							objApplicantCombinedData.ApplicantDocumentDetails.OtherCertificatesPDF.SaveAs(_path);

						objApplicantCombinedData.ApplicantDocumentDetails.FileName = documentType;
						objApplicantCombinedData.ApplicantDocumentDetails.FilePath = "Content/AppDocuments/" + UniqueFileName;
						objApplicantCombinedData.ApplicantDocumentDetails.DocumentTypeId = DocumentTypeValue;
						objApplicantCombinedData.ApplicantDocumentDetails.ApplicantId = objApplicantCombinedData.ApplicantDocumentDetails.ApplicantId;
						objApplicantCombinedData.ApplicantDocumentDetails.DocAppId = DocumentTypeValue;

						objApplicantCombinedData.ApplicantGeneralDetails.GetApplicantDocumentsDetail = _admissionBll.ApplicantDocumentDetailsBLL(objApplicantCombinedData.ApplicantDocumentDetails);
					}
				}
				//Document Update Ends
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception:" + ex.Message.ToString());
				output.flag = 0;
				output.status = "Error occured!";
			}

			return Json(new { objApplicantCombinedData, pref = output }, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#endregion

		#endregion

		#region .. Capture Applicant admission details at ITI Institute  ..

		public JsonResult GetDistrictMasterDivList(int Division)
		{
			var District_list = _admissionBll.GetDistrictMasterDivListBLL(Division);
			return Json(District_list, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetAdmissionRounds()
		{
			List<InstituteWiseAdmission> Year_list = new List<InstituteWiseAdmission>();
			Year_list = _admissionBll.GetAdmissionRoundsBLL();
			return Json(Year_list, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ApplicantAdmissionITIInstitute()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);

			ViewBag.QualificationList = GetAppliedForWhichBasics();
			ViewBag.SyllabusList = GetAppliedForSyllabus();
			ViewBag.SyllabusListCOBSE = GetAppliedForSyllabusCOBSE();
			ViewBag.LocationList = GetLocationList();
			var AssignDataBasedOnTabData = new AssignDataBasedOnTabs();
			return View(AssignDataBasedOnTabData);
		}

		public JsonResult GetDataAllocationFeeDetails(int ApplicationId)
		{
			List<InstituteWiseAdmission> objInstituteWiseAdmission = new List<InstituteWiseAdmission>();
			objInstituteWiseAdmission = _admissionBll.GetDataAllocationFeeDetailsBLL(ApplicationId);

			return Json(new { Resultlist = objInstituteWiseAdmission }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetApplicantDocumentFeeDetails(int ApplicationId)
		{
			List<ApplicantApplicationForm> objInstituteWiseAdmission = new List<ApplicantApplicationForm>();
			objInstituteWiseAdmission = _admissionBll.GetApplicantDocumentFeeDetails(ApplicationId);

			return Json(objInstituteWiseAdmission, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetDataAdmissionApplicants(int SessionYear, int CourseType, int ApplicantType, int RoundOption, int AdmittedorRejected,int ApplicatoinMode)
		{
			int loginId = Convert.ToInt32(Session["LoginId"]);
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			List<InstituteWiseAdmission> dataList = new List<InstituteWiseAdmission>();
			dataList = _admissionBll.GetDataAdmissionApplicantsBLL(SessionYear, CourseType, ApplicantType, RoundOption, AdmittedorRejected, loginId, roleId, ApplicatoinMode);
			int x = 1;
			foreach (var item in dataList)
			{
				item.slno = x;
				x++;
			}
			return Json(dataList, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetDataReconcile(int SessionYear, int CourseType, int ApplicantType, int RoundOption, int AdmittedorRejected, int ApplicatoinMode)
		{
			int loginId = Convert.ToInt32(Session["LoginId"]);
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			List<InstituteWiseAdmission> dataList = new List<InstituteWiseAdmission>();
			List<InstituteWiseAdmission> objInstituteWiseAdmission = new List<InstituteWiseAdmission>();
			List<InstituteWiseAdmission> objInstituteWiseAdmission1 = new List<InstituteWiseAdmission>();
			objInstituteWiseAdmission = _admissionBll.GetDataAdmissionApplicantsBLL(SessionYear, CourseType, ApplicantType, RoundOption, AdmittedorRejected, loginId, roleId,ApplicatoinMode);

			int x = 1;
			for (int i = 0; i < objInstituteWiseAdmission.Count; i++)
			{
				objInstituteWiseAdmission1 = _admissionBll.GetDataAllocationFeeDetailsBLL(objInstituteWiseAdmission[i].ApplicationId);
				dataList.AddRange(objInstituteWiseAdmission1);
				dataList[i].slno = x;
				x++;
			}

			return Json(dataList, JsonRequestBehavior.AllowGet);
		}


		[HttpGet]
		public JsonResult DirectAdmissionApplicantDetails()
		{
			int loginId = Convert.ToInt32(Session["LoginId"]);
			List<InstituteWiseAdmission> dataList = new List<InstituteWiseAdmission>();
			dataList = _admissionBll.DirectAdmissionApplicantDetailsBLL(loginId);
			int x = 1;
			foreach (var item in dataList)
			{
				item.slno = x;
				x++;
			}
			return Json(dataList, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetAdmissionApplicantsDistLogin(AdmissionApplicantsDistLogin objAdmissionApplicantsDistLogin)
		{
			List<InstituteWiseAdmission> dataList = new List<InstituteWiseAdmission>();
			var Id = Convert.ToInt32(Session["LoginId"]);
			dataList = _admissionBll.GetAdmissionApplicantsDistLoginBLL(objAdmissionApplicantsDistLogin, Id);
			int x = 1;
			foreach (var item in dataList)
			{
				item.slno = x;
				x++;
			}
			return Json(dataList, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ApplicantAdmissionITIInstituteRVP()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);

			ViewBag.QualificationList = GetAppliedForWhichBasics();
			ViewBag.SyllabusList = GetAppliedForSyllabus();
			ViewBag.SyllabusListCOBSE = GetAppliedForSyllabusCOBSE();
			ViewBag.LocationList = GetLocationList();
			int LoginId = Convert.ToInt32(Session["LoginID"]);
			ViewBag.UserDivionId = GetUserDivionId(LoginId);
			var AssignDataBasedOnTabData = new AssignDataBasedOnTabs();
			return View(AssignDataBasedOnTabData);
		}

		public int GetUserDivionId(int LoginId)
		{
			int UserDivionId = _admissionBll.GetUserDivionIdBLL(LoginId);
			return UserDivionId;
		}

		//03-07-2021
		public ActionResult GetsendBack()
		{
			var Response = _admissionBll.GetsendBack(Convert.ToInt32(Session["LoginId"].ToString()));
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetForward()
		{
			var Response = _admissionBll.GetForward(Convert.ToInt32(Session["LoginId"].ToString()));
			return Json(Response, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ApprovedRejectedList(InstituteWiseAdmission model)
		{
			var Response = _admissionBll.ApprovedRejectedList(model, Convert.ToInt32(Session["LoginId"].ToString()));
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult SentBackAdmittedList(InstituteWiseAdmission model, int sentId)
		{
			List<InstituteWiseAdmission> list = new List<InstituteWiseAdmission>();
			try
			{
				list = _admissionBll.SentBackAdmittedListBLL(model, Convert.ToInt32(Session["LoginId"].ToString()), sentId);
				return Json(list, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public ActionResult GetforwardAdmittedList(InstituteWiseAdmission model, int ForId)
		{
			var Response = _admissionBll.GetforwardAdmittedListBLL(model, Convert.ToInt32(Session["LoginId"].ToString()), ForId);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetOnClickSendToHierarchy(InstituteWiseAdmission model, int ForId, string TabName)
		{
			var Response = _admissionBll.GetOnClickSendToHierarchyBLL(model, Convert.ToInt32(Session["LoginId"].ToString()), ForId, TabName);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetCommentDetailsRemarks(int ApplicationId)
		{
			var Remarks = _admissionBll.GetCommentDetailsRemarks(Convert.ToInt32(Session["LoginId"].ToString()), ApplicationId);

			int c = 1;
			foreach (var x in Remarks)
			{
				x.slno = c++;
			}
			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetCommentDetailsRemarksById(int ApplicationId)
		{
			var Remarks = _admissionBll.GetCommentDetailsRemarksByIdBLL(ApplicationId);

			int c = 1;
			foreach (var x in Remarks)
			{
				x.slno = c++;
			}

			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult clickAddRemarksTrans(InstituteWiseAdmission model)
		{
			var loginId = Convert.ToInt32(Session["LoginId"]);
			string returnMsg = _admissionBll.GetclickAddRemarksTransBLL(model, loginId);
			return Json(returnMsg);
		}

		#endregion

		#region .. ITI Institute admit the applicants against vacancy seats at Institute level  ..

		public ActionResult ApplicantAdmissionAgainstVacancy()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);

			ViewBag.QualificationList = GetAppliedForWhichBasics();
			ViewBag.SyllabusList = GetAppliedForSyllabus();
			ViewBag.SyllabusListCOBSE = GetAppliedForSyllabusCOBSE();
			ViewBag.LocationList = GetLocationList();
			var AssignDataBasedOnTabData = new AssignDataBasedOnTabs();
			return View(AssignDataBasedOnTabData);
		}

		[HttpGet]
		public ActionResult GetInstituteMaster()
		{
			int LoginId = Convert.ToInt32(Session["LoginId"]);
			ApplicantAdmiAgainstVacancy objApplicantAdmiAgainstVacancy = new ApplicantAdmiAgainstVacancy();
			objApplicantAdmiAgainstVacancy = _admissionBll.GetInstituteMasterBLL(LoginId);
			objApplicantAdmiAgainstVacancy.TradeDetails = _admissionBll.GetInstituteTradeMasterBLL(objApplicantAdmiAgainstVacancy.CollegeId);
			return Json(objApplicantAdmiAgainstVacancy, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetApplIdByApplicationNumber(string ExistChkApplicationNumber)
		{
			int HorizontalVerticalCategory = _admissionBll.GetApplIdByApplicationNumberBLL(ExistChkApplicationNumber);
			return Json(HorizontalVerticalCategory, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetHorizontalVerticalCategory()
		{
			List<HorizontalVerticalCategorycs> HorizontalVerticalCategory = new List<HorizontalVerticalCategorycs>();
			HorizontalVerticalCategory = _admissionBll.GetHorizontalCategoryBLL();
			return Json(HorizontalVerticalCategory, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetVerticalCategory()
		{
			List<HorizontalVerticalCategorycs> HorizontalVerticalCategory = new List<HorizontalVerticalCategorycs>();
			HorizontalVerticalCategory = _admissionBll.GetVerticalCategoryBLL();
			return Json(HorizontalVerticalCategory, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetShiftsDetails()
		{
			List<HorizontalVerticalCategorycs> HorizontalVerticalCategory = new List<HorizontalVerticalCategorycs>();
			HorizontalVerticalCategory = _admissionBll.GetShiftsDetailsBLL();
			return Json(HorizontalVerticalCategory, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetUnitsDetails()
		{
			List<HorizontalVerticalCategorycs> HorizontalVerticalCategory = new List<HorizontalVerticalCategorycs>();
			HorizontalVerticalCategory = _admissionBll.GetUnitsDetailsBLL();
			return Json(HorizontalVerticalCategory, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetITICollegeDetailsMaster(int District, int Taluka)
		{
			List<ApplicationForm> College_list = new List<ApplicationForm>();
			College_list = _admissionBll.GetITICollegeDetailsMasterBLL(District, Taluka);
			return Json(College_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetUnitsShiftsDetails(int CollegeId, int TradeId)
		{
			List<ApplicationForm> Units_Shifts_list = new List<ApplicationForm>();
			Units_Shifts_list = _admissionBll.GetUnitsShiftsDetailsBLL(CollegeId, TradeId);
			return Json(Units_Shifts_list, JsonRequestBehavior.AllowGet);
		}

		public ActionResult UpdateApplicantAdmissionAgainstVacancy(ApplicantAdmiAgainstVacancy objApplicantAdmiAgainstVacancy)
		{
			List<ApplicantAdmiAgainstVacancy> objApplicantAdmiAgainstVacancyList = new List<ApplicantAdmiAgainstVacancy>();
			objApplicantAdmiAgainstVacancyList = _admissionBll.UpdateApplicantAdmissionAgainstVacancyBLL(objApplicantAdmiAgainstVacancy);
			return Json(objApplicantAdmiAgainstVacancyList, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region .. MeritList ..
		public ActionResult GetMeritRoles(int level)
		{
			var Response = _admissionBll.GetMeritRoles(Convert.ToInt32(Session["RoleId"].ToString()), level);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetCommentsMeritListFile(int id)
		{
			List<AdmissionMeritList> Remarks = new List<AdmissionMeritList>();
			Remarks = _admissionBll.GetCommentsMeritListFileBLL(id);
			int x = 1;
			foreach (var item in Remarks)
			{
				item.slno = x;
				x++;
			}
			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}
		public ActionResult AdmissionMeritList()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);

			return View();
		}
		[HttpGet]
		public ActionResult GetRolesForTentativeList(int level)
		{

			//int RoleIdForTentativeList = 1;
			//if (Convert.ToInt32(Session["RoleId"].ToString()) == 13 || Convert.ToInt32(Session["RoleId"].ToString()) == 14)
			//{
			//    RoleIdForTentativeList = 2;
			//}

			var Response = _admissionBll.GetMeritRoles(Convert.ToInt32(Session["RoleId"].ToString()), 1);
			// Response.RemoveAll(t => t.RoleID == 9 || t.RoleID == 10 || t.RoleID == 11 || t.RoleID == 12);

			//if (Convert.ToInt32(Session["RoleId"].ToString()) == 5 )
			//{
			//    Response.RemoveAll(t => t.RoleID == 9 || t.RoleID == 10 || t.RoleID == 11 || t.RoleID == 12 || t.RoleID == 1 || t.RoleID == 2);
			//}

			//if (Convert.ToInt32(Session["RoleId"].ToString()) == 6)
			//{
			//    if (RoleIdForTentativeList == 2)
			//    {
			//        Response.RemoveAll(t => t.RoleID == 13 || t.RoleID == 14 || t.RoleID == 16 || t.RoleID == 17);
			//    }
			//}                 

			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetMeritStatusList()
		{
			try
			{
				Log.Info("Entered GetStatusList()");
				//List<SelectListItem> responseList = _admissionBll.GetMeritStatusListBLL();
				var responseList = _admissionBll.GetMeritStatusListBLL(Convert.ToInt32(Session["RoleId"].ToString()));
				var role_id = Convert.ToInt32(Session["RoleId"]);

				//if (role_id == 2)// Director
				//    responseList.RemoveAll(t => t.Status != 2 && t.Status != 4 && t.Status != 7);
				//else if (role_id == 1)    // Commissioner
				//    responseList.RemoveAll(t => t.Status != 2 && t.Status != 4);
				//else if (role_id != 1 && role_id != 2)         //Apart from Director, Commissioner
				//    responseList.RemoveAll(t => t.Status != 4 && t.Status != 7);
				return Json(responseList, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception - GetStatusList():" + ex.Message.ToString());
				throw ex;
			}
		}

		public JsonResult GetGradationMeritList(int generateId, int AcademicYear, int ApplicantTypeId, int round)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
			MaritList = _admissionBll.GetGradationMeritListBLL(generateId, AcademicYear, ApplicantTypeId, round, Convert.ToInt32(Session["RoleId"].ToString()));
			int x = 1;

			foreach (var item in MaritList)
			{
				item.slno = x;
				item.Rank = x;
				x++;
			}
			return Json(MaritList, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetceckGradationTransTable(int generateId, int AcademicYear, int ApplicantTypeId)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
			MaritList = _admissionBll.GetceckGradationTransTable(generateId, AcademicYear, ApplicantTypeId, Convert.ToInt32(Session["RoleId"].ToString()));

			return Json(MaritList, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetReviewGradationMeritList(int generateId, int AcademicYearReviewId, int ApplicantTypeId)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
			MaritList = _admissionBll.GetReviewGradationMeritListBLL(generateId, AcademicYearReviewId, ApplicantTypeId, Convert.ToInt32(Session["RoleId"].ToString()));
			int x = 1;

			foreach (var item in MaritList)
			{
				item.slno = x;
				//item.Rank = x;
				x++;
			}
			return Json(MaritList, JsonRequestBehavior.AllowGet);
		}
		//Director&Commissioner view
		public JsonResult GetGradationMeritListDir(int generateId, int AcademicYearDC, int ApplicantTypeId)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
			MaritList = _admissionBll.GetGradationMeritListDirBLL(generateId, AcademicYearDC, ApplicantTypeId, Convert.ToInt32(Session["RoleId"].ToString()));
			int x = 1;

			foreach (var item in MaritList)
			{
				item.slno = x;
				//item.Rank = x;
				x++;
			}
			return Json(MaritList, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetGradationList(string rbId)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
			int x = 1;
			if (Convert.ToInt32(Session["RoleId"].ToString()) == 12 || Convert.ToInt32(Session["RoleId"].ToString()) == 5 || Convert.ToInt32(Session["RoleId"].ToString()) == 6 || Convert.ToInt32(Session["RoleId"].ToString()) == 1 || Convert.ToInt32(Session["RoleId"].ToString()) == 2 || Convert.ToInt32(Session["RoleId"].ToString()) == 10)
			{
				int loginId = Convert.ToInt32(Session["LoginId"].ToString());
				MaritList = _admissionBll.GetGradationListBLL(rbId, Convert.ToInt32(Session["RoleId"].ToString()), loginId);
				if (MaritList != null)
				{
					foreach (var item in MaritList)
					{
						item.slno = x;
						// item.Rank = x;
						x++;
					}
				}
			}
			return Json(MaritList, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetIndexGradationMeritList(string rbId)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
			int x = 1;
			MaritList = _admissionBll.GetIndexGradationMeritListBLL(rbId);
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
		//Index Tentative List 
		//public JsonResult GetIndexTentativeGradationMeritList()
		//{
		//    List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
		//    int x = 1;
		//    MaritList = _admissionBll.GetIndexTentativeGradationMeritListBLL();
		//    if (MaritList != null)
		//    {
		//        foreach (var item in MaritList)
		//        {
		//            item.slno = x;
		//            item.Rank = x;
		//            x++;
		//        }
		//    }
		//    return Json(MaritList, JsonRequestBehavior.AllowGet);
		//}
		//public JsonResult GetIndexFinalGradationMeritList()
		//{
		//    List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
		//    int x = 1;
		//    MaritList = _admissionBll.GetIndexFinalGradationMeritListBLL();
		//    if (MaritList != null)
		//    {
		//        foreach (var item in MaritList)
		//        {
		//            item.slno = x;
		//            // item.Rank = x;
		//            x++;
		//        }
		//    }
		//    return Json(MaritList, JsonRequestBehavior.AllowGet);
		//}
		public JsonResult viewMeritList(AdmissionMeritList model, int generateId, int AcademicYear, int ApplicantTypeId, int? DivisionId, int? DistrictId, int round)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
			MaritList = _admissionBll.viewMeritListBll(generateId, AcademicYear, ApplicantTypeId, round, Convert.ToInt32(DivisionId), Convert.ToInt32(DistrictId), Convert.ToInt32(Session["RoleId"].ToString()));
			int x = 1;

			foreach (var item in MaritList)
			{
				item.slno = x;
				//item.Rank = x;
				x++;
			}
			return Json(MaritList, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetGradationListStatus()
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();

			MaritList = _admissionBll.GetGradationListStatusBLL(Convert.ToInt32(Session["RoleId"].ToString()));
			int x = 1;
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

		//Use Case 25 - AD(Review)
		public JsonResult GetGrdationListReviewADNew(int AcademicYearAD, int ApplicantTypeId)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();

			MaritList = _admissionBll.GetGrdationListReviewADNewBLL(Convert.ToInt32(Session["RoleId"].ToString()), AcademicYearAD, ApplicantTypeId);
			int x = 1;
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
		public JsonResult GetGradationMeritAppListADNewId(int AcademicYearAD, int ApplicantTypeId, int ApplicationId)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
			MaritList = _admissionBll.GetGradationMeritAppListADNewIdBLL(AcademicYearAD, ApplicantTypeId, ApplicationId, Convert.ToInt32(Session["RoleId"].ToString()));
			int x = 1;

			foreach (var item in MaritList)
			{
				item.slno = x;
				//item.Rank = x;
				x++;
			}
			return Json(MaritList, JsonRequestBehavior.AllowGet);
		}

		//Use Case 26 - Director/Commissioner (Approve)
		public JsonResult GetGrdationListReviewDirCom(int AcademicYearDC, int ApplicantTypeId)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();

			MaritList = _admissionBll.GetGrdationListReviewDirComBLL(Convert.ToInt32(Session["RoleId"].ToString()), AcademicYearDC, ApplicantTypeId);
			int x = 1;
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
		public JsonResult GetGradationMeritListDirNew(int AcademicYearDC, int ApplicantTypeId, int ApplicationId)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
			MaritList = _admissionBll.GetGradationMeritListDirNewBLL(AcademicYearDC, ApplicantTypeId, ApplicationId, Convert.ToInt32(Session["RoleId"].ToString()));
			int x = 1;

			foreach (var item in MaritList)
			{

				item.slno = x;
				//item.Rank = x;

				x++;
			}
			GetGradationMeritListDirNewRank(MaritList);
			return Json(MaritList, JsonRequestBehavior.AllowGet);
		}

		public void GetGradationMeritListDirNewRank(List<AdmissionMeritList> MaritList)
		{
			MaritList = _admissionBll.GetGradationMeritListDirNewRank(MaritList);
			//return Json(MaritList, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public ActionResult GetGeneratbtnMeritList(List<AdmissionMeritList> lists, string remarks, int round)
		{
			nestedMeritList model = new nestedMeritList();
			model.lists = lists;
			int loginId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.GetGenerateMeritListBLL(model, loginId, remarks, round);

			return Json(res, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetDistrict(int Divisions)
		{
			var District_list = _admissionBll.GetDistricts(Divisions);
			return Json(District_list, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetDivision()
		{
			var District_list = _admissionBll.GetDivision();
			return Json(District_list, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetApplicantType()
		{
			var Applicant_list = _admissionBll.GetApplicantType();
			return Json(Applicant_list, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetApplicationMode()
		{
			var ApplicationMode_list = _admissionBll.GetApplicationMode_Bll();
			return Json(ApplicationMode_list, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetTraineeType()
		{
			var Applicant_list = _admissionBll.GetTraineeType();
			return Json(Applicant_list, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public ActionResult GetGradationType()
		{
			int loginId = Convert.ToInt32(Session["RoleId"].ToString());
			var Gradation_list = _admissionBll.GetGradationType();
			if (Convert.ToInt32(Session["RoleId"].ToString()) == 6 || Convert.ToInt32(Session["RoleId"].ToString()) == 2 || Convert.ToInt32(Session["RoleId"].ToString()) == 1)
			{
				Gradation_list.RemoveAll(t => t.GradationTypeId == 1);
			}

			foreach (var p in Gradation_list)
			{
				p.loginId = loginId;
			}
			return Json(Gradation_list, JsonRequestBehavior.AllowGet);
		}

		//Gradation View Page DropdownList
		[HttpGet]
		public ActionResult GetGradationTypeView()
		{
			int loginId = Convert.ToInt32(Session["RoleId"].ToString());
			var Gradation_list = _admissionBll.GetGradationType();

			foreach (var p in Gradation_list)
			{
				p.loginId = loginId;
			}
			return Json(Gradation_list, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public ActionResult GetAcademicYear()
		{
			List<SeatAllocation> Year_list = new List<SeatAllocation>();
			Year_list = _admissionBll.GetYearTypeBLL();
			return Json(Year_list, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetForwardMeritListDD(AdmissionMeritList model, int id, string remarks, int round)
		{
			var Response = _admissionBll.ForwardMeritListDDBLL(model, Convert.ToInt32(Session["RoleId"].ToString()), id, remarks, round);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public ActionResult GetSendforDirector(AdmissionMeritList model, int id, string remarks)
		{
			var Response = _admissionBll.SendforDirectorBLL(model, Convert.ToInt32(Session["RoleId"].ToString()), id, remarks);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ApproveMeritListDD(AdmissionMeritList model, int id, string remarks)
		{
			var Response = _admissionBll.ApproveMeritListDDBLL(model, Convert.ToInt32(Session["UserId"].ToString()), id, remarks);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}

		public ActionResult PublishMeritList(AdmissionMeritList model, int Status, string remarks)
		{
			var Response = _admissionBll.PublishMeritList(model, Convert.ToInt32(Session["RoleId"].ToString()), Status, remarks);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ChangesMeritList(AdmissionMeritList model, int backId, int Status, string remarks)
		{
			var Response = _admissionBll.ChangesMeritListBLL(model, backId, Convert.ToInt32(Session["RoleId"].ToString()), Status, remarks);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult SentBacktoDDMeritList(AdmissionMeritList model, int Status, int sentId, string remarks)
		{
			var Response = _admissionBll.SentBacktoDDMeritListBLL(model, Convert.ToInt32(Session["RoleId"].ToString()), Status, sentId, remarks);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}

		//Applicant login
		public JsonResult GetApplicantResultMarq()
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();

			MaritList = _admissionBll.GetApplicantResultMarqBLL(Convert.ToInt32(Session["RoleId"].ToString()), Convert.ToInt32(Session["LoginId"].ToString()));
			int x = 1;
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

		public JsonResult GetMeritListstatusPopup(int ApplicationId)
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();

			MaritList = _admissionBll.GetMeritListstatusPopupBLL(Convert.ToInt32(Session["RoleId"].ToString()), Convert.ToInt32(Session["LoginId"].ToString()), ApplicationId);
			int x = 1;
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

		public JsonResult GetPublishcalendarEvents()
		{
			List<AdmissionMeritList> MaritList = new List<AdmissionMeritList>();
			MaritList = _admissionBll.GetPublishcalendarEvents(Convert.ToInt32(Session["RoleId"].ToString()));


			return Json(MaritList, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region  Officer Mapping

		public ActionResult DocumentVerification()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);

			ViewBag.Header = "Verification";
			//Changes For Document Verification tab 
			ViewBag.QualificationList = GetAppliedForWhichBasics();
			ViewBag.SyllabusList = GetAppliedForSyllabus();
			ViewBag.SyllabusListCOBSE = GetAppliedForSyllabusCOBSE();
			ViewBag.LocationList = GetLocationList();
			int LoginId = Convert.ToInt32(Session["LoginID"]);
			ViewBag.UserDivionId = _AffilBll.GetUserDivionIdBLL(LoginId);

			return View();
		}
		public ActionResult ApplicantsAdmissionAllocation()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);

			ViewBag.Header = "Admission";
			return View();
		}
		public ActionResult GetOfficers(int roleId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.GetOfficers(loginId, roleId);
			int c = 1;
			foreach (var item in res)
			{
				item.slno = c;
				c++;
				if (item.Status == true)
					item.StatusName = "Active";
				else
					item.StatusName = "InActive";
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetInstituteId(VerificationOfficer officer)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.GetInstituteId(loginId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult AddOfficer(VerificationOfficer officer)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.AddOfficer(officer, loginId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult EditOfficer(int id)
		{

			var res = _admissionBll.EditOfficer(id);
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult UpdateOfficer(VerificationOfficer officer)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.UpdateOfficer(officer, loginId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult DeleteOfficer(int id)
		{
			var res = _admissionBll.DeleteOfficer(id);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetApplicants(int applicantId, int year, int roleId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = new List<VerificationOfficer>();

			if (roleId == (int)CmnClass.Role.AdmOff)
			{
				var dataList = _admissionBll.GetDataAdmissionApplicantsBLL(year, applicantId, 0, 0, 0, loginId, roleId,0);
				int x = 1;
				foreach (var item in dataList)
				{
					item.slno = x;
					VerificationOfficer verOff = new VerificationOfficer();
					verOff.slno = item.slno;
					//data type change to display year name
					verOff.Session = item.Session;
					verOff.CourseType = item.CourseTypeName;
					verOff.ApplicantNumber = item.ApplicantNumber;
					verOff.ApplicantName = item.ApplicantName;
					verOff.EmailId = item.Email;
					verOff.Gender = item.GenderName;
					verOff.Apdate = item.CreatedOn.ToString();
					verOff.StatusName = item.ApplInstiStatusEx;
					verOff.MobileNumber = item.MobileNumber;

					res.Add(verOff);
					x++;
				}
			}
			else
			{
				res = _admissionBll.GetApplicants(loginId, applicantId, year);
				int c = 1;
				foreach (var item in res)
				{
					item.slno = c;
					c++;
				}
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetVerificationOfficerDetails(int year, int courseType, int roleId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.GetVerificationOfficerDetails(loginId, year, courseType, roleId);
			int c = 1;
			foreach (var item in res)
			{
				item.slno = c;
				c++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetTotalApplicantOfficer(int roleId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.GetTotalApplicantOfficer(loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult MapApplicantToOfficer(int roleId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.MapApplicantToOfficer(loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetInactiveOfficerApplicants(int year, int courseType)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.GetInactiveOfficerApplicants(loginId, year, courseType);
			int c = 1;
			foreach (var item in res)
			{
				item.slno = c;
				c++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ReMapApplicantToOfficer(int roleId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.ReMapApplicantToOfficer(loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetActiveOfficers(int roleId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.GetActiveOfficers(loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult ReMapApplicantIndividualOff(List<Applicants> list)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.ReMapApplicantIndividualOff(loginId, list);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region dhanraj joined in rajgopal task
		public ActionResult GetApplicantsStatusFilter(int year, int courseType, int applicanType, int division_id, int district_lgd_code, int taluk_lgd_code, int InstituteId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.GetApplicantsStatusFilter(loginId, roleId, year, courseType, applicanType,  division_id,  district_lgd_code,  taluk_lgd_code, InstituteId);
			int c = 1;
			foreach (var item in res)
			{
				item.slno = c;
				c++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetApplicantsStatus()
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.GetApplicantsStatus(loginId, roleId);
			int c = 1;
			foreach (var item in res)
			{
				item.slno = c;
				c++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetApplicantsStatusApp(int ApplicantType)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.GetApplicantsStatusBLL(loginId, roleId, ApplicantType);
			int c = 1;
			foreach (var item in res)
			{
				item.slno = c;
				c++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetApplicantsStatusByAppType()
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.GetApplicantsStatus(loginId, roleId);
			int c = 1;
			foreach (var item in res)
			{
				item.slno = c;
				c++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetDataDocumentsVerificationFee(int? year, int? courseType, int? applicanType, int division_id ,int district_lgd_code, int taluk_lgd_code, int InstituteId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.GetDataDocumentsVerificationFee(loginId, roleId, year, courseType, applicanType, division_id, district_lgd_code, taluk_lgd_code, InstituteId);
			int c = 1;
			foreach (var item in res)
			{
				item.slno = c;
				c++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region
		public ActionResult GetDataDocumentsVerificationFeepayment(int? year, int? courseType, int? applicanType, string ApplNo)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			//var res = _admissionBll.GetDataDocumentsVerificationFeepayment(loginId, roleId, year, courseType, applicanType);
			var res = _admissionBll.GetDataDocumentsVerificationFeeNotPaid(loginId, roleId, year, courseType, applicanType, ApplNo);
			int c = 1;
			foreach (var item in res)
			{
				item.slno = c;
				c++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}


		#endregion

		#region
		public ActionResult GetReceiptNumber(int val, int id)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var model = new InstituteWiseAdmission();
			model.CreatedBy = loginId;
			var res = _admissionBll.GetReceiptNumber(model, val, id);
			//int c = 1;
			//foreach (var item in res)
			//{
			//	item.slno = c;
			//	c++;
			//}
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region Grievance
		public ActionResult GrievanceAgainstTentativeGradationList()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);

			ViewBag.QualificationList = GetAppliedForWhichBasics();
			ViewBag.SyllabusList = GetAppliedForSyllabus();
			ViewBag.SyllabusListCOBSE = GetAppliedForSyllabusCOBSE();
			ViewBag.LocationList = GetLocationList();
			int LoginId = Convert.ToInt32(Session["LoginID"]);
			ViewBag.UserDivionId = _AffilBll.GetUserDivionIdBLL(LoginId);
			return View();
		}
		public ActionResult ApplicantRankDetails()
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.ApplicantRankDetails(loginId, roleId);
			int c = 1;
			foreach (var item in res)
			{
				item.slno = c;
				c++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetDocumentTypes()
		{
			var res = _admissionBll.GetDocumentTypes();
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public ActionResult SubmitGrievanceTentative(List<HttpPostedFileBase> list, List<int> fileType, string remarks)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.SubmitGrievanceTentative(list, fileType, loginId, remarks, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetGrievanceTentativeStatus(int course, int year, int division, int district, int applicantType,int taluk,int institute)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			if(roleId==24)
            {
				var div= _AffilBll.GetUserDivionIdBLL(loginId);
				division = div;
			}
			var res = _admissionBll.GetGrievanceTentativeStatus(loginId, roleId, course, year, division, district, applicantType,taluk,institute);
			int c = 1;
			foreach (var item in res)
			{
				item.slno = c;
				c++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult EditApplicantGrievance(int grivanceId)
		{
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.EditApplicantGrievance(grivanceId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public ActionResult VerifyGrievance(List<int> fileType, List<string> status, int grivanceId, string remarks)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.VerifyGrievance(fileType, status, grivanceId, remarks, loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public ActionResult SendForCorrection(List<int> fileType, List<string> status, int grivanceId, string remarks)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.SendForCorrection(fileType, status, grivanceId, remarks, loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public ActionResult UpdateGrievanceTentative(List<HttpPostedFileBase> list, List<int> fileType, List<string> status, string remarks, int grievanceId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.UpdateGrievanceTentative(list, fileType, status, loginId, remarks, grievanceId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetGrievanceGrid()
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.GetGrievanceGrid(loginId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult RejectGrivance(int grivanceId, string remarks)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.RejectGrivance(grivanceId, remarks, loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetGrievanceRemarks(int id)
		{
			var res = _admissionBll.GetGrievanceRemarks(id);
			int c = 1;
			foreach (var itm in res)
			{
				itm.slno = c;
				c++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region .. Verification Officer

		public ActionResult ApplicationVerficationOfficer()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);

			ViewBag.QualificationList = GetAppliedForWhichBasics();
			ViewBag.SyllabusList = GetAppliedForSyllabus();
			ViewBag.LocationList = GetLocationList();
			ViewBag.SyllabusListCOBSE = GetAppliedForSyllabusCOBSE();
			int LoginId = Convert.ToInt32(Session["LoginID"]);
			ViewBag.UserDivionId = _admissionBll.GetUserDivionIdBLL(LoginId);
            var AssignDataBasedOnTabData = new AssignDataBasedOnTabs();
            return View(AssignDataBasedOnTabData);

            //return View();
		}

		public JsonResult GetApplicationDetailsById(int CredatedBy)
		{
			ApplicantApplicationForm ApplicantApplicationFormData = new ApplicantApplicationForm();
			ApplicantApplicationFormData = _admissionBll.GetMasterApplicantDataBLL(CredatedBy, "VO");

			List<ApplicationForm> PreferenceListDet = null;
			List<ApplicationForm> DistrictListDet = null;

			if (ApplicantApplicationFormData == null)
			{
				ApplicantApplicationFormData = new ApplicantApplicationForm();
			}

			ApplicantApplicationFormData.GetReligionList = _admissionBll.GetReligionDetailsBLL();
			ApplicantApplicationFormData.GetGenderList = _admissionBll.GetGenderDetailsBLL();
			ApplicantApplicationFormData.GetCasteList = _admissionBll.GetCasteListBLL();
			ApplicantApplicationFormData.GetCategoryList = _admissionBll.GetCategoryListBLL();
			ApplicantApplicationFormData.GetDocumentApplicationStatus = _admissionBll.GetDocumentApplicationStatusBLL();
			ApplicantApplicationFormData.GetApplicantTypeList = _admissionBll.GetApplicantTypeListBLL(Convert.ToInt32(Session["RoleId"].ToString()));
			ApplicantApplicationFormData.GetReservationList = _admissionBll.GetReservationsListBLL();
			ApplicantApplicationFormData.GetQualificationList = _admissionBll.GetQualificationListBLL();
			ApplicantApplicationFormData.GetApplicableReservations = _admissionBll.GetApplicantReservationListBLL(ApplicantApplicationFormData.ApplicationId);
			ApplicantApplicationFormData.PersonWithDisabilityCategory = _admissionBll.PersonWithDisabilityCategoryBLL();
			ApplicantApplicationFormData.GetDistrictList = _admissionBll.GetDistrictMasterListBLL();
			ApplicantApplicationFormData.GetApplicantDocumentsDetail = _admissionBll.GetDocumentDetailsBLL(ApplicantApplicationFormData.ApplicationId);
			ApplicantApplicationFormData.GetOtherBoards = _admissionBll.GetOtherBoardsListBLL();

			int i = 0;
			foreach (var SelectedApplicableReservation in ApplicantApplicationFormData.GetApplicableReservations)
			{
				if (i == 0)
					SelectedApplicableReservation.SelectedReservationId += SelectedApplicableReservation.ReservationId;
				else
					SelectedApplicableReservation.SelectedReservationId += "," + SelectedApplicableReservation.ReservationId;
				ApplicantApplicationFormData.SelectedReservationId += SelectedApplicableReservation.SelectedReservationId;
				i++;
			}

			foreach (var TalukaDet in ApplicantApplicationFormData.GetDistrictList)
			{
				TalukaDet.TalukListDet = _admissionBll.GetTalukMasterListBLL(TalukaDet.district_lgd_code);
			}

			PreferenceListDet = _admissionBll.GetQualificationListBLL();
			DistrictListDet = _admissionBll.GetDistrictMasterListBLL();
			return Json(new { Resultlist = ApplicantApplicationFormData, pref = PreferenceListDet, dist = DistrictListDet }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetApplicationDocForGriById(int CredatedBy)
		{
			ApplicantApplicationForm ApplicantApplicationFormData = new ApplicantApplicationForm();
			ApplicantApplicationFormData = _admissionBll.GetMasterApplicantDataBLL(CredatedBy, "VO");

			List<ApplicationForm> PreferenceListDet = null;
			List<ApplicationForm> DistrictListDet = null;

			if (ApplicantApplicationFormData == null)
			{
				ApplicantApplicationFormData = new ApplicantApplicationForm();
			}

			ApplicantApplicationFormData.GetReligionList = _admissionBll.GetReligionDetailsBLL();
			ApplicantApplicationFormData.GetGenderList = _admissionBll.GetGenderDetailsBLL();
			ApplicantApplicationFormData.GetCasteList = _admissionBll.GetCasteListBLL();
			ApplicantApplicationFormData.GetCategoryList = _admissionBll.GetCategoryListBLL();
			ApplicantApplicationFormData.GetDocumentApplicationStatus = _admissionBll.GetDocumentApplicationStatusBLL();
			ApplicantApplicationFormData.GetApplicantTypeList = _admissionBll.GetApplicantTypeListBLL(Convert.ToInt32(Session["RoleId"].ToString()));
			ApplicantApplicationFormData.GetReservationList = _admissionBll.GetReservationsListBLL();
			ApplicantApplicationFormData.GetQualificationList = _admissionBll.GetQualificationListBLL();
			ApplicantApplicationFormData.GetApplicableReservations = _admissionBll.GetApplicantReservationListBLL(ApplicantApplicationFormData.ApplicationId);
			ApplicantApplicationFormData.PersonWithDisabilityCategory = _admissionBll.PersonWithDisabilityCategoryBLL();
			ApplicantApplicationFormData.GetDistrictList = _admissionBll.GetDistrictMasterListBLL();
			ApplicantApplicationFormData.GetApplicantDocumentsDetail = _admissionBll.GetDocumentDetailsBLL(ApplicantApplicationFormData.ApplicationId);

			int i = 0;
			foreach (var SelectedApplicableReservation in ApplicantApplicationFormData.GetApplicableReservations)
			{
				if (i == 0)
					SelectedApplicableReservation.SelectedReservationId += SelectedApplicableReservation.ReservationId;
				else
					SelectedApplicableReservation.SelectedReservationId += "," + SelectedApplicableReservation.ReservationId;
				ApplicantApplicationFormData.SelectedReservationId += SelectedApplicableReservation.SelectedReservationId;
				i++;
			}

			foreach (var TalukaDet in ApplicantApplicationFormData.GetDistrictList)
			{
				TalukaDet.TalukListDet = _admissionBll.GetTalukMasterListBLL(TalukaDet.district_lgd_code);
			}

			PreferenceListDet = _admissionBll.GetQualificationListBLL();
			DistrictListDet = _admissionBll.GetDistrictMasterListBLL();
			return Json(new { Resultlist = ApplicantApplicationFormData, pref = PreferenceListDet, dist = DistrictListDet }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult UpdateNextRoundDetailsWithTrans(ApplicantApplicationForm formCollection)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());   //tbl_user_master um_id
			formCollection.VerfOfficer = loginId;
			var Response = _admissionBll.UpdateNextRoundDetailsWithTransBLL(formCollection);
			return Json(Response);
		}

		public JsonResult UpdateApplicationDetailsById(ApplicationStatusUpdate objApplicationStatusUpdate)
		{
			try
			{
				int loginId = Convert.ToInt32(Session["LoginId"].ToString());
				objApplicationStatusUpdate.VerfOfficer = loginId;
				objApplicationStatusUpdate.CreatedBy = loginId;
				var Response = _admissionBll.UpdateApplicationDetailsByIdBLL(objApplicationStatusUpdate);
			}
			catch (Exception ex)
			{

			}
			return Json(Response);
		}

		public JsonResult UpdateApplicationDetailsFromVOById(ApplicantDocumentsDetail objApplicantDocumentsDetail)
		{
			try
			{
				int loginId = Convert.ToInt32(Session["LoginId"].ToString());
				var Response = _admissionBll.UpdateApplicantFormDetailsBLL(objApplicantDocumentsDetail);
			}
			catch (Exception ex)
			{

			}
			return Json(Response);
		}

		public JsonResult UpdateGrievanceApplicationDetailsById(ApplicantDocumentsDetail objApplicantDocumentsDetail)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			objApplicantDocumentsDetail.CreatedBy = loginId;
			var Response = _admissionBll.UpdateGrievanceApplicationDetailsByIdBLL(objApplicantDocumentsDetail);

			if (objApplicantDocumentsDetail.Status != 3)
				ApplicantDocumentDetails(objApplicantDocumentsDetail);
			return Json(Response);
		}
		public JsonResult GetVerificationFeeCmntDetailsById(int ApplicationId)
		{
			var Remarks = _admissionBll.GetVerificationFeeCmntDetailsByIdBLL(ApplicationId);

			int c = 1;
			foreach (var x in Remarks)
			{
				x.slno = c++;
			}

			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Seat Availablity
		public ActionResult SeatAvailability()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);

			return View();
		}
		public ActionResult SeatAvailabilityStatus()
		{
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var Response = _admissionBll.SeatAvailabilityStatus(roleId);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetUserRoles(int level)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var Response = _admissionBll.GetUserRoles(loginId, level, roleId);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetSeatAvailabilityList(int courseCode, string AcademicYear)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var Response = _admissionBll.GetSeatAvailabilityList(loginId, courseCode, AcademicYear);
			int c = 1;
			foreach (var itm in Response)
			{
				itm.Slno = c;
				c++;
			}
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetSeatAvailabilityListAdd(string miscode)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var Response = _admissionBll.GetSeatAvailabilityListAdd(loginId, miscode);
			int c = 1;
			foreach (var itm in Response)
			{
				itm.Slno = c;
				c++;
			}
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetSeatAvailabilityStatus()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			return View();
		}
		public ActionResult GetSeatTypes()
		{
			var Response = _admissionBll.GetSeatTypes();
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetSeatsByTradeIdSeatType(int tradeId)
		{
			var Response = _admissionBll.GetSeatsByTradeIdSeatType(tradeId);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetSeatsBySeatTypeRules(int seattypeId, int tradeId)
		{
			var Response = _admissionBll.GetSeatsBySeatTypeRules(seattypeId, tradeId);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public ActionResult SaveSeatAvailability(List<SeatDetails> seat)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var response = _admissionBll.SaveSeatAvailability(seat, loginId, roleId);
			return Json(response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ForwardSeatAvailability(List<SeatDetails> seat)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var response = _admissionBll.ForwardSeatAvailability(seat, loginId, roleId);
			return Json(response, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetRemarks(int seatId)
		{
			var response = _admissionBll.GetRemarks(seatId);
			int c = 1;
			foreach (var itm in response)
			{
				itm.RoleId = c;
				c++;
			}
			return Json(response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ApproveSeatAvailability(List<SeatDetails> seat)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var response = _admissionBll.ApproveSeatAvailability(seat, loginId, roleId);
			return Json(response, JsonRequestBehavior.AllowGet);
		}
		//public ActionResult RejectSeatAvailability(List<SeatDetails> seat)
		//{
		//    int loginId = Convert.ToInt32(Session["LoginId"].ToString());
		//    var response = _admissionBll.RejectSeatAvailability(seat, loginId);
		//    return Json(response, JsonRequestBehavior.AllowGet);
		//}
		public ActionResult GetSeatViewDetails(int seatId)
		{
			var response = _admissionBll.GetSeatViewDetails(seatId);
			return Json(response, JsonRequestBehavior.AllowGet);
		}

		public ActionResult delUnitShiftDetails(/*List<SeatDetails> seat*/int seatId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var response = _admissionBll.GetdelUnitShiftDetails(seatId, loginId, roleId);
			return Json(response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetdeActiveSeatDetails(int seatId, int TradeItiId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var response = _admissionBll.GetdeActiveSeatDetails(seatId, TradeItiId, loginId, roleId);
			return Json(response, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public ActionResult UpdateSeatAvailability(List<SeatDetails> seat)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var response = _admissionBll.UpdateSeatAvailability(seat[0], loginId, roleId);
			return Json(response, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetRegionDistrictCities()
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var response = _admissionBll.GetRegionDistrictCities(loginId);
			return Json(response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetCitiTaluks(int distilgdCOde)
		{
			var response = _admissionBll.GetTaluks(distilgdCOde);
			return Json(response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetInstitutes(int distilgdCOde)
		{
			var response = _admissionBll.GetInstitutes(distilgdCOde);
			return Json(response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetSeatAvailabilityListStatusFilter(int TabId, int? courseType, int? academicYear, int? division, int? district, int? taluk, int? Institute)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			int Year_id = Convert.ToInt32(academicYear);
			int Course_Id = Convert.ToInt32(courseType);
			int Division_Id = Convert.ToInt32(division);
			int District_Id = Convert.ToInt32(district);
			int taluk_id = Convert.ToInt32(taluk);
			int Insttype_Id = Convert.ToInt32(Institute);

			var response = _admissionBll.GetSeatAvailabilityListStatusFilter(TabId, Course_Id, Year_id, roleId, loginId, Division_Id, District_Id, taluk_id, Insttype_Id);
			int c = 1;
			foreach (var itm in response)
			{
				itm.Slno = c;
				c++;
			}
			return Json(response, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region .. Seat Matrix..
		public ActionResult AdmissionSeatMatrix()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			return View();
		}
		[HttpGet]
		public JsonResult GetGenSeatMatrix(int ApplicantTypeGen, int AcademicYearGen, int RoundGen)
		{
			List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
			seatmatrix = _adseatmatBll.GetGenSeatMatrixBLL(ApplicantTypeGen, AcademicYearGen, RoundGen, Convert.ToInt32(Session["RoleId"].ToString()));
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
		//[HttpGet]
		//public ActionResult AdmissionSeatMatrix()
		//{
		//    seatmatrixmodelNest sList = new seatmatrixmodelNest();

		//    sList.list = new List<seatmatrixmodel>();
		//    sList.seat_list = new List<seatmatrixmodel>();

		//    List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
		//    sList.list = _adseatmatBll.GetGenerateSeatMatrixBLL(Convert.ToInt32(Session["RoleId"].ToString()));

		//    List<seatmatrixmodelNested> divMast = new List<seatmatrixmodelNested>();
		//    divMast = _adseatmatBll.GetGenerateSeatMatrixBLLNested();
		//    //sList.SelectListMatrix = divMast;
		//    foreach (var i in sList.list)
		//    {
		//        if (i != null)
		//        {

		//            divMast = _adseatmatBll.GetGenerateSeatMatrixBLLNested();
		//            sList.seat_list.Add(new seatmatrixmodel { modelseat = i, division_master = divMast });
		//        }
		//    }

		//    return View(sList);
		//    //return Json(sList, JsonRequestBehavior.AllowGet);
		//}


		#region seatmatrix by dhanraj        
		//start generate seat matrix and submit seat matrix
		public ActionResult GetDivisionsInstituteTrades(int year, int appType, int courseType, int round)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _adseatmatBll.GenerateSeaMatrix(year, courseType, round, loginId);
			return PartialView(res);
		}

		[HttpPost]
		public ActionResult SubmitSeatMatrixCollegeWise(List<Tradewideseat> listItem, int collegeId, int year, int appType, int courseType, int round)
		{
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _adseatmatBll.SubmitSeatMatrixCollegeWise(listItem, collegeId, round, year, appType, courseType, loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public ActionResult SubmitSeatMatrix(int year, int appType, int courseType, int round, int role, string remark)
		{
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _adseatmatBll.SubmitSeatMatrix(year, appType, courseType, round, role, loginId, roleId, remark);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		//end generate and submit

		//start update seat matrix
		public ActionResult GetDivisionsInstituteTradesUpdate(int year, int round, int applicantType, int courseId)
		{
			var res = _adseatmatBll.GetDivisionsInstitutesTrades(year, round, applicantType, courseId);
			return PartialView(res);
		}
		[HttpPost]
		public FileResult UploadExcel(int year, int round, int applicantType, int courseId)
		{
			Utilities.Security.ValidateRequestHeader(Request);
			var res = _adseatmatBll.GetDivisionsInstitutesTrades(year, round, applicantType, courseId);

			DataTable dttt = new DataTable();
			bool isfirst = true;
			if (res != null)
			{
				foreach (DivisionUpdateModel dum in res)
				{
					if (dum != null && dum.Institutes != null)
						foreach (InstituteUpdateModel ium in dum.Institutes)
						{
							if (ium != null && ium.Trades != null)
							{
								DataTable dt = ToDataTable(dum.DivisionName, ium.InstituteName, ium.Trades);

								if (dt != null)
								{
									if (isfirst)
									{
										dttt = dt.Clone();
										isfirst = false;
									}
									dttt.Merge(dt);
								}
							}
						}
				}
			}
			//convert datatable to excel sheet
			//make excel sheet downloadable
			//file content to byte format 
			byte[] ba = new byte[] { };
			dttt = null;
			return File(ba, "type", "test");
		}
		private DataTable ToDataTable<T>(string div, string coll, List<T> items)
		{
			DataTable dataTable = new DataTable();
			//Get all the properties
			PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			dataTable.Columns.Add("Division");
			dataTable.Columns.Add("College");
			foreach (PropertyInfo prop in Props)
			{
				//Setting column names as Property names                
				dataTable.Columns.Add(prop.Name);
			}
			if (items != null)
			{
				foreach (T item in items)
				{
					var values = new object[Props.Length + 2];
					for (int i = 0; i < Props.Length + 2; i++)
					{
						if (i == 0)
							values[i] = div;
						else if (i == 1)
							values[i] = coll;
						else
						{
							//inserting property values to datatable rows
							values[i] = Props[i - 2].GetValue(item, null);
						}
					}
					if (values[3].ToString() != "zero") // trade name is zero
						dataTable.Rows.Add(values);
				}
			}
			//put a breakpoint here and check datatable
			return dataTable;
		}
		[HttpPost]
		public ActionResult UpdateSeatMatrixCollegeWise(List<Tradewideseat> listItem, int collegeId, int year, int appType, int courseType, int round)
		{
			Utilities.Security.ValidateRequestHeader(Request);
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _adseatmatBll.UpdateSeatMatrixCollegeWise(listItem, collegeId, round, year, appType, courseType, loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult UpdateSeatMatrix(int year, int appType, int courseType, int round, int role)
		{
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _adseatmatBll.UpdateSeatMatrix(year, appType, courseType, round, role, loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		//end update

		//start forward sendback approve reject seat matrix
		public ActionResult ForwardSendBackApproveSeatMatrix(int round, int year, string remarks, int courseType, int Status, int roleId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int role = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _adseatmatBll.ForwardSendBackApproveSeatMatrix(round, year, remarks, courseType, Status, loginId, roleId, role);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		//end forward sendback approve reject
		//remarks seta matrix
		public ActionResult GetSeatmatrixRemarks(int year, int round, int applId, int courseType)
		{
			var res = _adseatmatBll.GetSeatmatrixRemarks(year, round, applId, courseType);
			int i = 1;
			foreach (var itm in res)
			{
				itm.Slno = i;
				i++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		//end remarks
		//seat matrix publish notification
		[HttpPost]
		public ActionResult GetSeatmatrixNotification()
		{
			var res = _adseatmatBll.GetSeatmatrixNotification();
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		//end seat matrix publish notification
		#endregion
		public JsonResult AdmissionSeatMatrixNested(int round, int instiId, int year, int tradeId)
		{
			var sList = _adseatmatBll.GetGenerateSeatMatrixBLLNested(round, instiId, year, tradeId);
			return Json(sList, JsonRequestBehavior.AllowGet);
		}
		public JsonResult InsertSeatMatrixData(seatmatrixmodelNested sm)
		{
			var smList = _adseatmatBll.InsertSeatMatrixBLL(sm);

			return Json(smList, JsonRequestBehavior.AllowGet);
		}
		//public ActionResult GetInstituteType()
		//{
		//    var Applicant_list = _adseatmatBll.GetInstituteTypeBll();
		//    return Json(Applicant_list, JsonRequestBehavior.AllowGet);
		//}
		//public JsonResult GetTaluk(int DistId)
		//{
		//    List<SelectListItem> list = new List<SelectListItem>();
		//    try
		//    {
		//        list = _adseatmatBll.GetTalukBLL(DistId);
		//    }
		//    catch (Exception ex)
		//    {
		//        Log.Error("Entered Exception:" + ex.Message.ToString());
		//    }

		//    return Json(list, JsonRequestBehavior.AllowGet);
		//}
		//public JsonResult GetviewSeatmatrix(int ApplicantTypeId, int AcademicYear, int Institute)
		//{
		//    List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
		//    seatmatrix = _adseatmatBll.GetviewSeatmatrixBLL(ApplicantTypeId, AcademicYear, Institute, Convert.ToInt32(Session["RoleId"].ToString()));

		//    return Json(seatmatrix, JsonRequestBehavior.AllowGet);
		//}
		//public JsonResult GetGenerateSeatMatrix(int ddlApplicantTypeGen, int ddlAcademicYearGen, int ddlRoundGen)
		//{
		//    List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
		//    seatmatrix = _adseatmatBll.GetGenerateSeatMatrixBLL(ddlApplicantTypeGen, ddlAcademicYearGen, ddlRoundGen, Convert.ToInt32(Session["RoleId"].ToString()));
		//    int x = 1;
		//    if (seatmatrix != null)
		//    {
		//        foreach (var item in seatmatrix)
		//        {
		//            item.slno = x;
		//            x++;
		//        }
		//    }
		//    return Json(seatmatrix, JsonRequestBehavior.AllowGet);
		//}
		//public JsonResult GetSummarySeatMatrix()
		//{
		//    List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
		//    seatmatrix = _adseatmatBll.GetSummarySeatMatrixBLL(Convert.ToInt32(Session["RoleId"].ToString()));
		//    int x = 1;
		//    if (seatmatrix != null)
		//    {
		//        foreach (var item in seatmatrix)
		//        {
		//            item.slno = x;
		//            x++;
		//        }
		//    }
		//    return Json(seatmatrix, JsonRequestBehavior.AllowGet);
		//}
		#endregion

		#region .. Seat Matrix..Added by Sujit
		//public ActionResult AdmissionSeatMatrix()
		//{
		//    List<SM> seatmatrix = new List<SM>();
		//    List<seatmatrixmodel> seatmatrix1 = new List<seatmatrixmodel>();
		//    seatmatrix1 = _adseatmatBll.GetGenerateSeatMatrixBLL(1, 2021, 3, Convert.ToInt32(Session["RoleId"].ToString()));

		//    List<seatmatrixmodelNested> divMast = new List<seatmatrixmodelNested>();
		//    divMast = _adseatmatBll.GetGenerateSeatMatrixBLLNested();
		//    foreach (var i in seatmatrix1)
		//    {
		//        if (i != null)
		//        {
		//            var od = divMast.Where(a => a.iti_college_id.Equals(i.iti_college_id)).ToList();
		//            seatmatrix.Add(new SM { modelseat = i, division_master = od });
		//        }
		//    }

		//    return View(seatmatrix);
		//}
		public ActionResult GetInstituteType()
		{
			var Applicant_list = _adseatmatBll.GetInstituteTypeBll();
			return Json(Applicant_list, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetTaluk(int DistId)
		{
			List<SelectListItem> list = new List<SelectListItem>();
			try
			{
				list = _adseatmatBll.GetTalukBLL(DistId);
			}
			catch (Exception ex)
			{
				Log.Error("Entered Exception:" + ex.Message.ToString());
			}

			return Json(list, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetStatus()
		{
			var Response = _adseatmatBll.GetStatusBLL(Convert.ToInt32(Session["RoleId"].ToString()));
			//Response.RemoveAll(t => t.RoleID == 3);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetviewSeatmatrix(int ApplicantTypeId, int AcademicYear, int Institute)
		{
			List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
			seatmatrix = _adseatmatBll.GetviewSeatmatrixBLL(ApplicantTypeId, AcademicYear, Institute, Convert.ToInt32(Session["RoleId"].ToString()));

			return Json(seatmatrix, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetGenerateSeatMatrix(int ddlApplicantTypeGen, int ddlAcademicYearGen, int ddlRoundGen)
		{
			List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
			seatmatrix = _adseatmatBll.GetGenerateSeatMatrixBLL(ddlApplicantTypeGen, ddlAcademicYearGen, ddlRoundGen, Convert.ToInt32(Session["RoleId"].ToString()));
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


		public JsonResult GetSummarySeatMatrix()
		{
			List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
			seatmatrix = _adseatmatBll.GetSummarySeatMatrixBLL(Convert.ToInt32(Session["RoleId"].ToString()));
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

		public JsonResult GetCheckSummary(int AcademicYearId, int ApplicantTypeId, int InstituteId, int Round, int? DistrictId, int? DivisionId, int? TalukId)
		{
			var seatmatrix = _adseatmatBll.GetCheckSummaryBLL(Convert.ToInt32(Session["RoleId"].ToString()), AcademicYearId, ApplicantTypeId, InstituteId, Round, Convert.ToInt32(DivisionId), Convert.ToInt32(DistrictId), Convert.ToInt32(TalukId));
			seatmatrix.RemoveAt(0);
			return Json(seatmatrix, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetReviewSeatMatrix(int AcademicYearId)
		{
			List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
			seatmatrix = _adseatmatBll.GetReviewSeatMatrixBLL(Convert.ToInt32(Session["RoleId"].ToString()), AcademicYearId);
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
		public JsonResult GetUpdateSeatMatrix(int AcademicYearId)
		{
			var seatmatrix = _adseatmatBll.GetUpdateSeatMatrixBLL(Convert.ToInt32(Session["RoleId"].ToString()), AcademicYearId);
			int x = 1;

			foreach (var item in seatmatrix)
			{
				item.slno = x;
				item.RoleId = Convert.ToInt32(Session["RoleId"].ToString());
				x++;
			}
			return Json(seatmatrix, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetApproveSeatMatrix(int AcademicYearId)
		{
			List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
			seatmatrix = _adseatmatBll.GetApproveSeatMatrixBLL(Convert.ToInt32(Session["RoleId"].ToString()), AcademicYearId);
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
		public JsonResult GetViewSeatMatrixGrid(int ApplicantTypeId, int AcademicYearId, int Round, int? DistrictId, int? DivisionId, int? TalukId, int? InstituteId)
		{
			List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
			seatmatrix = _adseatmatBll.GetViewSeatMatrixGridBLL(Convert.ToInt32(Session["RoleId"].ToString()), ApplicantTypeId, AcademicYearId, Round, Convert.ToInt32(DistrictId), Convert.ToInt32(DivisionId), Convert.ToInt32(TalukId), Convert.ToInt32(InstituteId));
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

		public JsonResult GetViewSeatMatrixData(int ApplicantTypeId, int AcademicYearId, int Round)
		{
			var seatmatrix = _adseatmatBll.GetViewSeatMatrix(ApplicantTypeId, AcademicYearId, Round, Convert.ToInt32(Session["LoginId"].ToString()));
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
		public JsonResult GetUpdateTradeSeatMatrixId(int SeatMaxId)
		{
			List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
			seatmatrix = _adseatmatBll.GetUpdateTradeSeatMatrixBLL(Convert.ToInt32(Session["RoleId"].ToString()), SeatMaxId);
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
		public JsonResult GetReviewTradeSeatMatrixId(int SeatMaxId)
		{
			List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
			seatmatrix = _adseatmatBll.GetReviewTradeSeatMatrixIdBLL(Convert.ToInt32(Session["RoleId"].ToString()), SeatMaxId);
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
		public JsonResult GetAproveTradeSeatMatrixId(int SeatMaxId)
		{
			List<seatmatrixmodel> seatmatrix = new List<seatmatrixmodel>();
			seatmatrix = _adseatmatBll.GetAproveTradeSeatMatrixIdBLL(Convert.ToInt32(Session["RoleId"].ToString()), SeatMaxId);
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
		#endregion



		#region Seat allotment
		public ActionResult SeatAutoAllotment()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			return View();
		}
		public ActionResult GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round)
		{
			int RoleId = Convert.ToInt32(Session["RoleId"].ToString());
			int LoginID = Convert.ToInt32(Session["LoginID"].ToString());
			var Response = _admissionBll.GenerateSeatAutoAllotment(courseType, applicantType, academicYear, round, RoleId, LoginID);
			int c = 1;
			foreach (var itm in Response)
			{
				itm.Slno = c;
				c++;
			}
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public ActionResult ForwardSeatAutoAllotment(List<int> seatAllocDetailId, int roleId, string Remarks, int Status)
		{
			Utilities.Security.ValidateRequestHeader(Request);
			int loginId = Convert.ToInt32(Session["RoleId"].ToString());
			var Response = _admissionBll.ForwardSeatAutoAllotment(seatAllocDetailId, loginId, roleId, Remarks, Status);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetGeneratedSeatAutoAllotmentList(int courseType, int applicantType, int academicYear, int round)
		{
			var Response = _admissionBll.GetGeneratedSeatAutoAllotmentList(courseType, applicantType, academicYear, round);
			int c = 1;
			foreach (var itm in Response)
			{
				itm.Slno = c;
				c++;
			}
			return Json(Response, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ViewSeatAutoAllotment(int allocationId)
		{
			var Response = _admissionBll.ViewSeatAutoAllotment(allocationId);
			int c = 1;
			foreach (var itm in Response)
			{
				itm.Slno = c;
				c++;
			}
			return Json(Response, JsonRequestBehavior.AllowGet);
		}
		#endregion


		#region Seat Allocation Review & Recommand Of Seat Matrix
		public JsonResult GetGenerateSeatMatrix1(int ddlCourseTypesGen, int ddlApplicantTypeGen, int ddlAcademicYearGen, int ddlRoundGen)
		{
			List<ReviewSeatAllocated> seatmatrix = new List<ReviewSeatAllocated>();
			seatmatrix = _admissionBll.GeneratedSeatAllotmentReviewBLL(ddlCourseTypesGen, ddlApplicantTypeGen, ddlAcademicYearGen, ddlRoundGen, Convert.ToInt32(Session["RoleId"].ToString()));
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

		public JsonResult GetSeatMatrixViewDetails(int id)
		{

			// int login = Convert.ToInt32(Session["RoleId"].ToString());
			List<SeatMatrixAllocationDetail> Response = new List<SeatMatrixAllocationDetail>();
			Response = _admissionBll.GeneratedSeatAllotmentReviewListBLL(id);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetSeatMatrixViewList(int courseType, int applicantType, int academicYear, int round)
		{
			try
			{
				List<SeatAutoAllocationModel> listOfSeatAllocation = new List<SeatAutoAllocationModel>();
				listOfSeatAllocation = _admissionBll.GetSeatMatrixViewListBLL(courseType, applicantType, academicYear, round);
				int x = 1;
				if (listOfSeatAllocation != null)
				{
					foreach (var item in listOfSeatAllocation)
					{
						item.Slno = x;
						x++;
					}
				}
				return Json(listOfSeatAllocation, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpPost]
		public ActionResult ForwardSeatAutoAllotmentReview(List<int> seatAllocDetailId, int roleId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var Response = _admissionBll.ForwardSeatAutoAllotmentReviewBLL(seatAllocDetailId, loginId, roleId);
			return Json(Response, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCommentDetailsSeatAllocation(int seatId)
		{
			var Remarks = _admissionBll.GetCommentsListSeatAllocationBLL(seatId);

			int c = 1;
			foreach (var x in Remarks)
			{
				x.slno = c++;
			}

			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}

		#endregion


		#region transfer admission seats
		public ActionResult TransferAdmissionSeat()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			return View();
		}
		public ActionResult GetTrades()
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.GetTrades(loginId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetMisCode(int instiId)
		{
			var res = _admissionBll.GetMisCode(instiId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetAvailableseatsTrades(int instiId)
		{
			var res = _admissionBll.GetAvailableseatsTrades(instiId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetUnits(int insti, int trade)
		{
			var res = _admissionBll.GetUnits(insti, trade);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetShifts(int insti, int trade, int unit)
		{
			var res = _admissionBll.GetShifts(insti, trade, unit);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetDualSystem(int insti, int trade)
		{
			var res = _admissionBll.GetDualSystem(insti, trade);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetTranseferInstitutes(int type, int taluklgd)
		{
			var res = _admissionBll.GetTranseferInstitutes(type, taluklgd);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetAdmittedData(int session, int course, int? trade, int round)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.GetAdmittedData(loginId, session, course, trade, round);
			int x = 1;
			if (res != null)
			{
				foreach (var item in res)
				{
					item.Slno = x;
					x++;
				}
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetRequestedDetails(int session, int course, int trade)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			var res = _admissionBll.GetRequestedDetails(loginId, session, course, trade);
			int x = 1;
			if (res != null)
			{
				foreach (var item in res)
				{
					item.Slno = x;
					x++;
				}
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult SubmitAdmittedData(List<ApplicantTransferModel> seat)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.SubmitAdmittedData(seat, loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetInstituteTypes()
		{
			var res = _admissionBll.GetInstituteTypes();
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetInstituteNames(int type)
		{
			var res = _admissionBll.GetInstituteNames(type);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ForwardAdmittedData(int transSeatId, int status, string remarks, int flowId)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.ForwardAdmittedData(transSeatId, status, remarks, loginId, roleId, flowId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetTransferRemarks(int seatId)
		{
			var res = _admissionBll.GetTransferRemarks(seatId);
			int x = 1;
			foreach (var item in res)
			{
				item.Slno = x;
				x++;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ApproveAdmittedData(int transSeatId, int status, string remarks)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.ApproveAdmittedData(transSeatId, status, remarks, loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult SendBackAdmittedData(int transSeatId, int status, string remarks)
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.SendBackAdmittedData(transSeatId, status, remarks, loginId, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetAdmittedDataStatus(string session = "", string courseType = "", string trade = "")
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.GetAdmittedDataStatus(loginId, roleId);
			if (!string.IsNullOrEmpty(session) && !string.IsNullOrEmpty(courseType) && !string.IsNullOrEmpty(trade))
			{
				res = res?.Where(a => a.Session == Convert.ToInt32(session) && a.CourseType == courseType && a.TradeName == trade).ToList();
			}
			int x = 1;
			if (res != null)
			{
				foreach (var item in res)
				{
					item.Slno = x;
					x++;
				}
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetApplicantTransferbyList(string session = "", string courseType = "", string trade = "")
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.GetApplicantTransferbyList(loginId, roleId);
			if (!string.IsNullOrEmpty(session) && !string.IsNullOrEmpty(courseType) && !string.IsNullOrEmpty(trade))
			{
				res = res?.Where(a => a.Session == Convert.ToInt32(session) && a.CourseType == courseType && a.TradeName == trade).ToList();
			}
			int x = 1;
			if (res != null)
			{
				foreach (var item in res)
				{
					item.Slno = x;
					x++;
				}
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		//dheem
		public ActionResult GetApprovedTransferList(string session = "", string courseType = "", string trade = "")
		{
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			//var res = _admissionBll.GetApplicantTransferbyList(loginId, roleId);
			var res = _admissionBll.GetApprovedTransferbyList(loginId, roleId);
			if (!string.IsNullOrEmpty(session) && !string.IsNullOrEmpty(courseType) && !string.IsNullOrEmpty(trade))
			{
				res = res?.Where(a => a.Session == Convert.ToInt32(session) && a.CourseType == courseType && a.TradeName == trade).ToList();
			}
			int x = 1;
			if (res != null)
			{
				foreach (var item in res)
				{
					item.Slno = x;
					x++;
				}
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetApplicantInstituteDetails(int id)
		{
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.GetApplicantInstituteDetails(id);
			foreach (var itm in res)
			{
				itm.RoleId = roleId;
			}
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult UpdateApplicantInstituteDetails(ApplicantTransferModel tran)
		{
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.UpdateApplicantInstituteDetails(tran, roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		public ActionResult SeatTransferStatus()
		{
			int roleId = Convert.ToInt32(Session["RoleId"].ToString());
			var res = _admissionBll.SeatTransferStatus(roleId);
			return Json(res, JsonRequestBehavior.AllowGet);
		}
		#endregion
		#region Admission register
		public ActionResult Admissionregister()
		{
			_CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
			return View();
		}
		public ActionResult GetViewAdmissionRegisterDemo(int coursetype, int? session, int? division, int? district, int? Institute, int? InstType)
		{
			List<AdmissionRegister> regd = new List<AdmissionRegister>();
			int id = Convert.ToInt32(Session["RoleId"].ToString());
			regd = _admissionBll.GetViewAdmissionRegisterDemo(id, coursetype, session, division, district, Institute, InstType);
			int x = 1;

			foreach (var item in regd)
			{
				item.slno = x;
				x++;
			}
			return Json(regd, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetDistrictsReg()
		{
			var District_list = _admissionBll.GetDistrictsJD();
			return Json(District_list, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public ActionResult GetAppRound(int Roundtype)
		{
			var rounds = _admissionBll.GetAppRound(Roundtype);
			return Json(rounds, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetAppRoundReg(int applicantType, int? Roundtype)
		{
			var rounds = _admissionBll.GetAppRoundReg(applicantType, Roundtype);
			return Json(rounds, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetInstitutesReg(int District)
		{
			var response = _admissionBll.GetInstitutesReg(District);
			return Json(response, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetInstTypeDetails()
		{
			List<AdmissionRegister> regd = new List<AdmissionRegister>();
			regd = _admissionBll.GetInstTypeDetails();
			return Json(regd, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetStateListDetails()
		{
			List<ApplicationForm> regd = new List<ApplicationForm>();
			regd = _admissionBll.GetStateListDetails();
			return Json(regd, JsonRequestBehavior.AllowGet);
		}

		#region comment
		//public ActionResult GetViewAdmissionRegister(int collegeId, int courseType, int academicYear, int NotificationId)
		//{
		//    int loginId = Convert.ToInt32(Session["LoginId"].ToString());
		//    int roleId = Convert.ToInt32(Session["RoleId"].ToString());
		//    var response = _admissionBll.GetViewAdmissionRegister(collegeId, courseType, academicYear, roleId, loginId, NotificationId);
		//    int c = 1;
		//    foreach (var itm in response)
		//    {
		//        itm.slno = c;
		//        c++;
		//    }
		//    return Json(response, JsonRequestBehavior.AllowGet);
		//}
		#endregion comment
		#endregion

		[HttpPost]
		public ActionResult UpdateAdmittedDetails(InstituteWiseAdmission model)
		{
			List<InstituteWiseAdmission> objInstituteWiseAdmission = new List<InstituteWiseAdmission>();
			try
			{
				Utilities.Security.ValidateRequestHeader(Request);
				model.CreatedBy = Convert.ToInt32(Session["LoginId"].ToString());
				if (model.ApplicantNumber == null)
				{
					model.userRole = Session["RoleId"].ToString();
					_admissionBll.DirectAdmissionSeatAllotmentBLL(model);
				}
				objInstituteWiseAdmission = _admissionBll.UpdateAdmittedDetailsBLL(model);

				foreach (var returnData in objInstituteWiseAdmission)
				{
					returnData.UpdateMsg = "Applicant <b>'" + returnData.ApplicantName + "'</b> is <b>'" + (returnData.AdmittedStatus == 6 ? "Admitted" : "Rejected") + "'</b>" + (returnData.AdmittedStatus == 6 ? " to <b> 'InstName' </b>" : "") + " successfully";
				}
			}
			catch (Exception ex)
			{
				foreach (var returnData in objInstituteWiseAdmission)
				{
					returnData.UpdateMsg = "There is error in your data, Updated Failed";
				}
			}
			return Json(objInstituteWiseAdmission, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetAdmissionUnitsDetails()
		{
			List<AdmissionUnitsShifts> AddmissionUnit_list = new List<AdmissionUnitsShifts>();
			AddmissionUnit_list = _admissionBll.GetAdmissionUnitsDetailsDLL();
			return Json(AddmissionUnit_list, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetAdmissionShiftsDetails()
		{
			List<AdmissionUnitsShifts> AddmissionShift_list = new List<AdmissionUnitsShifts>();
			AddmissionShift_list = _admissionBll.GetAdmissionShiftsDetailsDLL();
			return Json(AddmissionShift_list, JsonRequestBehavior.AllowGet);
		}
		public void CheckBrowserAndMenuUl()
		{
			try
			{
				// check direct url
				if (Session["UserId"] == null)
				{
					Response.Redirect("~/Home/Logout", true);
				}
				#region check Menu url
				var response = _LoginService.MenuList(Convert.ToInt32(Session["MenuMappingId"].ToString()));
				var currentpath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
				currentpath = currentpath.Remove(0, 1);
				response.AddRange(CmnClass.GetCmnCustomMenuList(response));
				bool Validmenu = false;
				foreach (var mlist in response)
				{
					Validmenu = mlist.Url.Contains(currentpath);
					if (Validmenu)
					{
						break;
					}
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
						//mobjErrorLog.Err_Handler("UrlReferrer", "NULL", Session["CurrentPage"].ToString());
						Response.Redirect("~/Home/Logout", true);
					}

				}
				#endregion
				var checkLogginstatus = _admissionBll.IsUserLoggedOnElsewhere(Session["UserName"].ToString(), Session["sessionid"].ToString());

				if (checkLogginstatus)
				{
					Response.Redirect("~/Home/Logout", true);
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		[HttpPost]
		public ActionResult GetKutumbaDetails(string KutumbaId)
		{
			try
			{
				
				List<ResultDataList> dataList = new List<ResultDataList>();
				string DeptID = "";
				string APIVersion = "1.0";
				string IsPhotoRequired = "0";
				string ClintCode = "1744991536";
				string BenfId = "";
				string Member_ID = "";
				string Aadhar_No = "";
				string Request_ID = "9999999999";
				string Mobile_No = "";
				string HMACValue = ClintCode + "_" + BenfId + "_" + KutumbaId + "_" + Aadhar_No + "_" + Mobile_No;
				string HMACKey = "b1d1b7a8-bd49-4191-b104-b3c118899e97";
				string HMACResult = HashHMACHex(HMACKey, HMACValue);
				dataList = Get_Family_Data(ClintCode, BenfId, KutumbaId, Aadhar_No, HMACResult, DeptID, APIVersion, IsPhotoRequired, Member_ID, Mobile_No, Request_ID);

				return Json(dataList, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Login", "Login");
			}
		}

		public List<ResultDataList> Get_Family_Data(string ClintCode, string BenfId, string RC_Number, string Aadhar_No, string HMACResult, string DeptID, string APIVersion, string IsPhotoRequired, string Member_ID, string Mobile_No, string Request_ID)
		{
			Memeber_Model _obj = new Memeber_Model();
			ResultDataList data = new ResultDataList();
			List<ResultDataList> Memberlist = new List<ResultDataList>();
			var InputVal = new
			{
				DeptID,
				BenID = BenfId,
				RC_Number,
				Aadhar_No,
				ClientCode = ClintCode,
				HashedMac = HMACResult,
				APIVersion,
				IsPhotoRequired,
				Member_ID,
				Mobile_No,
				Request_ID,
				UIDType= "0"
			};
			
			//string URL = "https://kutumba.karnataka.gov.in/testfidapienc/GetBeneficiaryData";
			string URL = "https://kutumba-services.karnataka.gov.in/testfidapienc1/getbeneficiarydata";
			System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
			client.BaseAddress = new System.Uri(URL);
			string InputJson = JsonConvert.SerializeObject(InputVal);
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
			System.Net.Http.HttpContent content = new StringContent(InputJson, UTF8Encoding.UTF8, "application/json");
			HttpResponseMessage messge = client.PostAsync(URL, content).Result;

			if (messge.IsSuccessStatusCode)
			{
				string result = messge.Content.ReadAsStringAsync().Result;


				var contentJo = (JObject)JsonConvert.DeserializeObject(result);
				
				string EncResultData = contentJo["EncResultData"].ToString();

				string IV = "1231233213214566";
				string HMACKeyDecrypt = "b1d1b7a8bd494191b104b3c118899e97";
				var DecryptData = DecryptString(HMACKeyDecrypt, IV, EncResultData);

				var contentJo1 = (JObject)JsonConvert.DeserializeObject(DecryptData);


				var responce_code = contentJo1["Response_ID"].ToString();
				if (contentJo1["ResultDataList"].ToString() != "")
				{
					var organizationsJArray = contentJo1["ResultDataList"].Value<JArray>();
					Memberlist = organizationsJArray.ToObject<List<ResultDataList>>();
				
				}
				else
				{
					data.data_is_null = "Y";
				}
			}
			else
			{
			}


			return Memberlist;
		}

		[HttpPost]
		public ActionResult Get_SSLC_Details(string SSLC_Reg_No)
		{
			try
			{
				//ResultDataList obj = new ResultDataList();
				string nameMatch = compareTwoString("Annaray", "ಅಣ್", true);
				ResultDataList dataList = new ResultDataList();
				string ClientCode = "1744991536";
				string DeptID = "25";
				string DeptAppID = "0";
				string BenID = SSLC_Reg_No;
				string RC_Number = "";
				string FamilyMemberID = null;
				string FamilyMemberName = null;
				string Request_ID = "1234567890";
				string BenfHashedUID = null;

			    string HMACValue = ClientCode + "_" + BenID + "_" + DeptID + "_" + DeptAppID + "__";
				string HMACKey = "b1d1b7a8-bd49-4191-b104-b3c118899e97";
				string HMACResult = HashHMACHex(HMACKey, HMACValue);

				dataList = GetSSLCDetails(ClientCode, HMACResult, DeptID, DeptAppID, BenID, FamilyMemberID, FamilyMemberName, Request_ID, BenfHashedUID);



				return Json(dataList, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Login", "Login");
			}
		}
		[HttpPost]
		public ActionResult Get_RD_Numbers_Details(string RDNumber)
		{
			try
			{

				ResultDataList dataList = new ResultDataList();
				string ClientCode = "1744991536";
				string DeptID = "23";
				string DeptAppID = "1";
				string BenID = RDNumber;
				string FamilyMemberID = null;
				string FamilyMemberName = null;
				string Request_ID = "1234567890";
				string BenfHashedUID = null;

				string HMACValue = ClientCode + "_" + BenID + "_" + DeptID + "_" + DeptAppID + "__";
				string HMACKey = "b1d1b7a8-bd49-4191-b104-b3c118899e97";
				string HMACResult = HashHMACHex(HMACKey, HMACValue);

				dataList = GetSSLCDetails(ClientCode, HMACResult, DeptID, DeptAppID, BenID, FamilyMemberID, FamilyMemberName, Request_ID, BenfHashedUID);

			


				return Json(dataList, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Login", "Login");
			}
		}

		public ResultDataList GetSSLCDetails(string clientCode, string HMACResult, string DeptID, string DeptAppID, string BenID, string FamilyMemberID, string FamilyMemberName, string Request_ID, string BenfHashedUID)
		{
			ResultDataList data = new ResultDataList();
			List<ResultDataList> Memberlist = new List<ResultDataList>();
			try 
			{ 
			
			var InputVal = new
			{
				ClientCode= clientCode,
				HashedMac = HMACResult,
				DeptID= DeptID,
				DeptAppID= DeptAppID,
				BenID= BenID,
				FamilyMemberID= FamilyMemberID,
				FamilyMemberName= FamilyMemberName,
				Request_ID= Request_ID,
				BenfHashedUID= BenfHashedUID
			};
			
			string URL = "https://kutumba-services.karnataka.gov.in/testfidapienc1/getdeptrawdata";
			System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
			client.BaseAddress = new System.Uri(URL);
			string InputJson = JsonConvert.SerializeObject(InputVal);
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
			System.Net.Http.HttpContent content = new StringContent(InputJson, UTF8Encoding.UTF8, "application/json");
			HttpResponseMessage messge = client.PostAsync(URL, content).Result;

			if (messge.IsSuccessStatusCode)
			{

               
				string result = messge.Content.ReadAsStringAsync().Result;

				var contentJo = (JObject)JsonConvert.DeserializeObject(result);
				string EncResultData = contentJo["EncResultData"].ToString();

				string IV = "1231233213214566";
				string HMACKeyDecrypt = "b1d1b7a8bd494191b104b3c118899e97";
				var DecryptData = DecryptString(HMACKeyDecrypt, IV, EncResultData);

				var contentJo1 = (JObject)JsonConvert.DeserializeObject(DecryptData);




				var responce_code = contentJo1["Response_ID"].ToString();
				if (contentJo1["ResultDataList"].ToString() != "")
				{
					var organizationsJArray = contentJo1["ResultDataList"].Value<JArray>();

					Memberlist = organizationsJArray.ToObject<List<ResultDataList>>();
                    var member_list_with_dob = Memberlist;
                    foreach (var ResultDataList in member_list_with_dob)
                    {
                        data.ApplicantName = ResultDataList.ApplicantName;
                        data.DOB = ResultDataList.DOB;
                        data.Gender = ResultDataList.Gender;
                        data.ApplicantFatherName = ResultDataList.ApplicantFatherName;
                        data.ApplicantMotherName = ResultDataList.ApplicantMotherName;
                        data.SSLC_MaxMarks = ResultDataList.SSLC_MaxMarks;
                        data.SSLC_Secured_Marks = ResultDataList.SSLC_Secured_Marks;
                        data.SSLC_Results = ResultDataList.SSLC_Results;
                        data.SSLC_Percentage = ResultDataList.SSLC_Percentage;
                        data.Applicant_School_Address = ResultDataList.Applicant_School_Address;

                        data.FacilityCode = ResultDataList.FacilityCode;
                        data.DateOfIssue = ResultDataList.DateOfIssue;
                        data.CertificateValidUpto = ResultDataList.CertificateValidUpto;
                        data.AnnualIncome = ResultDataList.AnnualIncome;
                       
                    }
                    data.Member_model_details = member_list_with_dob;

                    if (!string.IsNullOrEmpty(data.CertificateValidUpto))
                    {
						DateTime currentDate = DateTime.Now;
						//DateTime expireDate = Convert.ToDateTime(data.CertificateValidUpto);
						DateTime expireDate = DateTime.ParseExact(data.CertificateValidUpto, "dd/MM/yyyy", null);

						if (currentDate <= expireDate)
						{
							data.RDnumberValidity = "Valid";

						}
						else
						{
							data.RDnumberValidity = "Expired";
						}
					}
					
                }
				else
				{
					data.data_is_null = "Y";
				}
			}
			else
			{

			}

			return data;

			}
            catch (Exception ex)
            {
				return data;
			}

		}




        [HttpPost]
        public JsonResult Get_Family_Details(string Family_Id, string Family_Member_Id, string ResponseId)
        {
            try
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //ExceptionLogging.SendErrorToText(ex);
                throw;
            }

        }


        public string HashHMACHex(string hMACKey, string hMACValue)
		{
			string hashHMACHex = string.Empty;
			try
			{
				ASCIIEncoding encoding = new ASCIIEncoding();
				byte[] keyByte = encoding.GetBytes(hMACKey);
				HMACSHA256 hmacsha1 = new HMACSHA256(keyByte);
				byte[] messageBytes = encoding.GetBytes(hMACValue);
				byte[] hash = HashHMAC(keyByte, messageBytes);
				hashHMACHex = HashEncode(hash);
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
			finally
			{
			}
			return hashHMACHex;
		}
		public byte[] HashHMAC(byte[] key, byte[] message)
		{
			var hash = new HMACSHA256(key);
			return hash.ComputeHash(message);
		}
		public string HashEncode(byte[] hash)
		{
			return Convert.ToBase64String(hash);
		}
		public string DecryptString(string key, string IV, string cipherText)
		{
			byte[] buffer = Convert.FromBase64String(cipherText);
			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(key);
				aes.IV = Encoding.UTF8.GetBytes(IV);
				ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
				using (MemoryStream memoryStream = new MemoryStream(buffer))
				{
					using (CryptoStream cryptoStream = new
				   CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new
					   StreamReader((Stream)cryptoStream))
						{
							return streamReader.ReadToEnd();
						}
					}
				}
			}
		}

		public String compareTwoString(String firstName, String secondName, bool Lenient = true)
		{

			String serviceURL = "https://Namematcher.karnataka.gov.in/NameMatcher/NameMatch";

			string url = serviceURL + "/CompareStrings?FirstName=" + firstName + "&SecondName=" + secondName + "&Lenient=" + Lenient;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.AutomaticDecompression = DecompressionMethods.GZip;

			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())

			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

	}
}