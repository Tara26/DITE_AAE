using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Admission;
using Models;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.UI;
using System.Text;
using iTextSharp.tool.xml;
using BLL.Admission;
using System.Configuration;
using BLL.User;

namespace DIT.Controllers
{
	public class PaymentPDFGenerationController : Controller
	{
		private readonly IAdmissionBLL _admissionBll;
		private IUserBLL _LoginService;
		string ErrorLogpath = ConfigurationManager.AppSettings["logPath"].ToString();
		Errorhandling mobjErrorLog = new Errorhandling();
		public PaymentPDFGenerationController()
		{
			this._LoginService = new UserBLL();
			_admissionBll = new AdmissionBLL();
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
		// GET: PaymentPDFGeneration
		public ActionResult Index()
		{
			CheckBrowserAndMenuUl();
			return View();
		}

		[HttpPost]
		public JsonResult InsertApplicantFormDetails(ApplicantApplicationForm objApplicantApplicationForm)
		{
			UploadPreferenceType output = new UploadPreferenceType();
			int loginId = Convert.ToInt32(Session["LoginId"].ToString());
			List<ApplicantApplicationForm> objReturnApplicationForm = new List<ApplicantApplicationForm>();

			try
			{
				Utilities.Security.ValidateRequestHeader(Request);
				string documentType = null; int documentTypeContent = 0; string FileNameFormat = null;
				if (objApplicantApplicationForm.PhotoFile != null)
				{
					documentType = objApplicantApplicationForm.PhotoFile.FileName;
					documentTypeContent = objApplicantApplicationForm.PhotoFile.ContentLength;
					FileNameFormat = "PhotoImg";

					int maxcontentlength = 1024 * 3000;     //3MB
					var supportedTypes = new[] { "jpg" };
					string extension = System.IO.Path.GetExtension(documentType).Substring(1);

					if (documentTypeContent > maxcontentlength)
					{
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
					objApplicantApplicationForm.PhotoFile.SaveAs(_path);
					objApplicantApplicationForm.FileName = documentType;
					objApplicantApplicationForm.FilePath = "Content/AppDocuments/" + UniqueFileName;
				}

				objApplicantApplicationForm.ApplicantId = objApplicantApplicationForm.ApplicationId;
				objApplicantApplicationForm.IsActive = true;
				objApplicantApplicationForm.CredatedBy = loginId;
				objReturnApplicationForm = _admissionBll.InsertApplicantFormDetailsBLL(objApplicantApplicationForm);

				if (objReturnApplicationForm == null)
				{
					output.flag = 0;
					output.status = "Error occured!";
				}
				else
				{
					output.flag = 1;
					output.status = "Basicaly !";
				}
			}
			catch (Exception ex)
			{
				output.flag = 0;
				output.status = "Error occured!";
			}

			return Json(new { objReturnApplicationForm, pref = output }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult GeneratePaymentReceiptPDFData(InstituteWiseAdmission model)
		{
			List<InstituteWiseAdmission> objInstituteWiseAdmission = new List<InstituteWiseAdmission>();
			try
			{
				Utilities.Security.ValidateRequestHeader(Request);
				model.UserLoginId = Convert.ToInt32(Session["LoginId"]);
				var role_id = Convert.ToInt32(Session["RoleId"]);
				model.TradeName = model.TradeName.ToUpper();
				if (model.ApplicantNumber == null)
				{
					_admissionBll.UpdtApplITIInstDetailsBLL(model.ApplicationId, model.UserLoginId, role_id, model.AllocationId);
				}
				objInstituteWiseAdmission = _admissionBll.GeneratePaymentReceiptBLL(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return Json(objInstituteWiseAdmission, JsonRequestBehavior.AllowGet);
		}

		public void GeneratePaymentReceiptPDF(int ApplicationId, int DocAdmFeeFlag)
		{
			List<InstituteWiseAdmission> objInstituteWiseAdmission = new List<InstituteWiseAdmission>();
			if(DocAdmFeeFlag==1)
            {
				objInstituteWiseAdmission = _admissionBll.GenerateDocumentPaymentReceiptPDFBLL(ApplicationId); 

			}
			else
            {
				objInstituteWiseAdmission = _admissionBll.GeneratePaymentReceiptPDFBLL(ApplicationId);
			}
			
			
			using (StringWriter sw = new StringWriter())
			{
				using (HtmlTextWriter hw = new HtmlTextWriter(sw))
				{
					StringBuilder sb = new StringBuilder();

					//Generate Invoice (Bill) Header.
					string html = System.IO.File.ReadAllText(Server.MapPath("~/Content/Receipt.html.db"));
					sb.Append(html);

					//Export HTML String as PDF.
					sb.Replace("#img", Server.MapPath("~/Content/frontend/images/Seal_of_Karnataka.png"));
					sb.Replace("#TitleText", "Admission");
					sb.Replace("#ReceiptNum", objInstituteWiseAdmission[0].ReceiptNumber);
					sb.Replace("#ApplicantName", objInstituteWiseAdmission[0].ApplicantName);
					sb.Replace("#Trade", objInstituteWiseAdmission[0].TradeName);
					sb.Replace("#FeeAmt", objInstituteWiseAdmission[0].AdmisionFee.ToString() + " ( " + GetAmtInWords((int)objInstituteWiseAdmission[0].AdmisionFee) + " only )");
					sb.Replace("#LOCMONYYYY", "BNG" + " " + DateTime.Today.ToString("MMM") + " " + DateTime.Today.Year.ToString());
					sb.Replace("#InstituteName", objInstituteWiseAdmission[0].InstituteName);
					sb.Replace("#MisCode", objInstituteWiseAdmission[0].MISCode);
					sb.Replace("#Date", objInstituteWiseAdmission[0].PaymentDate.ToString());
					sb.Replace("#AppTrainText", "Applicant");

					sb.Append(sb.ToString());

					sb.Replace("Student", "Office", sb.Length / 2, sb.Length / 2);    // Replace the second occurrence of Student as Office copy

					StringReader sr = new StringReader(sb.ToString());
					Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

					PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
					pdfDoc.Open();
					XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);

					pdfDoc.Close();
					Response.ContentType = "application/pdf";
					Response.AddHeader("content-disposition", "attachment;filename=" + objInstituteWiseAdmission[0].ApplicantNumber + "-" + objInstituteWiseAdmission[0].ApplicationId + ".pdf");
					Response.Cache.SetCacheability(HttpCacheability.NoCache);
					Response.Write(pdfDoc);
					Response.End();
				}
			}
		}

		public void GenerateAdmissionAcknowledgementPDF(int ApplicationId)
		{
			List<InstituteWiseAdmission> objInstituteWiseAdmission = new List<InstituteWiseAdmission>();
			
			
				objInstituteWiseAdmission = _admissionBll.GeneratePaymentReceiptPDFBLL(ApplicationId);
			


			using (StringWriter sw = new StringWriter())
			{
				using (HtmlTextWriter hw = new HtmlTextWriter(sw))
				{
					StringBuilder sb = new StringBuilder();

					//Generate Invoice (Bill) Header.
					string html = System.IO.File.ReadAllText(Server.MapPath("~/Content/Receipt.html.db"));
					sb.Append(html);

					//Export HTML String as PDF.
					sb.Replace("#img", Server.MapPath("~/Content/frontend/images/Seal_of_Karnataka.png"));
					sb.Replace("#TitleText", "Admission");
					sb.Replace("#ReceiptNum", objInstituteWiseAdmission[0].ReceiptNumber);
					sb.Replace("#ApplicantName", objInstituteWiseAdmission[0].ApplicantName);
					sb.Replace("#Trade", objInstituteWiseAdmission[0].TradeName);
					sb.Replace("#FeeAmt", objInstituteWiseAdmission[0].AdmisionFee.ToString() + " ( " + GetAmtInWords((int)objInstituteWiseAdmission[0].AdmisionFee) + " only )");
					sb.Replace("#LOCMONYYYY", "BNG" + " " + DateTime.Today.ToString("MMM") + " " + DateTime.Today.Year.ToString());
					sb.Replace("#InstituteName", objInstituteWiseAdmission[0].InstituteName);
					sb.Replace("#MisCode", objInstituteWiseAdmission[0].MISCode);
					sb.Replace("#Date", objInstituteWiseAdmission[0].PaymentDate.ToString());
					sb.Replace("#AppTrainText", "Applicant");

					sb.Append(sb.ToString());

					sb.Replace("Student", "Office", sb.Length / 2, sb.Length / 2);    // Replace the second occurrence of Student as Office copy

					StringReader sr = new StringReader(sb.ToString());
					Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

					PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
					pdfDoc.Open();
					XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);

					pdfDoc.Close();
					Response.ContentType = "application/pdf";
					Response.AddHeader("content-disposition", "attachment;filename=" + objInstituteWiseAdmission[0].ApplicantNumber + "-" + objInstituteWiseAdmission[0].ApplicationId + ".pdf");
					Response.Cache.SetCacheability(HttpCacheability.NoCache);
					Response.Write(pdfDoc);
					Response.End();
				}
			}
		}

		[HttpPost]
		public ActionResult UpdateAdmittedDetails(InstituteWiseAdmission model)
		{
			List<InstituteWiseAdmission> objInstituteWiseAdmission = new List<InstituteWiseAdmission>();
			try
			{
				Utilities.Security.ValidateRequestHeader(Request);
				model.CreatedBy = Convert.ToInt32(Session["LoginId"].ToString());
				objInstituteWiseAdmission = _admissionBll.UpdateAdmittedDetailsBLL(model);

				foreach (var returnData in objInstituteWiseAdmission)
				{
					returnData.UpdateMsg = "Applicant Admitted Details updated successfully";
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
							_path = _path.Replace("PaymentPDFGeneration", "Admission");
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
						objApplicantDocumentsDetail.CreatedBy = loginId;
						objApplicantDocumentsDetail.DocAppId = DocAppId;
						objApplicantDocumentsDetail.DocumentRemarks = DocumentRemarks;
						objApplicantDocumentsDetail.Verified = objApplicantDocumentsDetail.Verified;
						try
						{
							if (checkDocFormat != null)
							{
								objApplicantApplicationForm.GetApplicantDocumentsDetail = _admissionBll.ApplicantDocumentDetailsInsBLL(objApplicantDocumentsDetail);
								objApplicantApplicationForm.UpdateMsg = "success";
							}
						}
						catch (Exception ex)
						{
							objApplicantApplicationForm.UpdateMsg = "failed";
						}
					}
				}

				#region .. Update Documents indicator in tbl_Applicant_Detail ..

				try
				{
					objApplicantApplicationForm.CredatedBy = Convert.ToInt32(Session["LoginId"].ToString());
					objApplicantApplicationForm.GetApplicantDocumentsDetail = _admissionBll.ApplicantUpdateInstituteBLL(objApplicantApplicationForm);
					objApplicantApplicationForm.UpdateMsg = "success";
				}
				catch (Exception ex)
				{
					objApplicantApplicationForm.UpdateMsg = "failed";
				}

				#endregion
			}
			catch (Exception ex)
			{
				objApplicantApplicationForm.UpdateMsg = "failed";
			}

			return Json(objApplicantApplicationForm);
		}

		public JsonResult GetCommentDetailsApplicant(int SeatAllocationId)
		{
			var Remarks = _admissionBll.GetCommentDetailsApplicantInstituteBLL(SeatAllocationId);
			int SiNoCnt = 1;
			foreach (var AddSiNo in Remarks)
			{
				AddSiNo.slno = SiNoCnt++;
			}
			return Json(Remarks, JsonRequestBehavior.AllowGet);
		}
		public void GenerateApplicantAdmissionAcknowledgementPDF(int ApplicationId)
		{
			List<InstituteWiseAdmission> objInstituteWiseAdmission = new List<InstituteWiseAdmission>();
			List<ApplicantDocumentsDetail> appdoc = new List<ApplicantDocumentsDetail>();
			ApplicantApplicationForm ApplicantApplicationFormData = new ApplicantApplicationForm();
			//objInstituteWiseAdmission = _admissionBll.GenerateAdmissionAcknowledgementPDFBLL(ApplicationId);
			objInstituteWiseAdmission = _admissionBll.GenerateDocumentPaymentReceiptPDFBLL(ApplicationId);
			 appdoc = _admissionBll.GetApplicantDocStatusDLL(ApplicationId);
			ApplicantApplicationFormData.GetApplicantInstitutePreference = _admissionBll.GetApplicantInstitutePreferenceBLL(ApplicationId);

			using (StringWriter sw = new StringWriter())
			{
				using (HtmlTextWriter hw = new HtmlTextWriter(sw))
				{
					StringBuilder sb = new StringBuilder();

					//Generate Invoice (Bill) Header.
					string html = System.IO.File.ReadAllText(Server.MapPath("~/Content/AdmissionAcknowledgement.html.db"));

					string toReplace = "<tr><td> #ListOfInstitutes </td></tr>";
					string toReplace1 = "<tr><td> #ListOfDocuments </td></tr>";
					int index = html.IndexOf(toReplace);

					string replace = "";
					int sino = 0;
					html = html.Replace(toReplace, "");


					foreach (var InstitTradeDet in ApplicantApplicationFormData.GetApplicantInstitutePreference)
					{
						sino = sino + 1;
						string strTr = "";
						InstitTradeDet.InstituteDet = _admissionBll.GetITICollegeDetailsByDistrictTalukaBLL(InstitTradeDet.DistrictId, InstitTradeDet.TalukaId);
						InstitTradeDet.TradeDet = _admissionBll.GetITICollegeTradeDetailsBLL(InstitTradeDet.InstituteId, ApplicantApplicationFormData.InstituteStudiedQual);
						if (sino % 2 != 0)
						{
							strTr = "<tr>";
						}
						replace = strTr + "<td>" + sino + "</td><td>" + InstitTradeDet.InstituteName/*.Where(a => a.iti_college_code == InstitTradeDet.InstituteId).Select(a => a.iti_college_name).FirstOrDefault()*/ + "</td><td>" +
								InstitTradeDet.TradeName/*TradeDet.Where(a => a.trade_id == InstitTradeDet.TradeId).Select(a => a.trade_name).FirstOrDefault()*/ + "</td>";
						if (sino % 2 == 0 || sino == ApplicantApplicationFormData.GetApplicantInstitutePreference.Count)
						{
							replace = replace + "</tr>";
						}
						html = html.Insert(index, replace);
						index = index + replace.Length + 1;
					}
					int index1 = html.IndexOf(toReplace1);
					html = html.Replace(toReplace1, "");
					replace = "";
					sino = 0;


					foreach (var AplDocTradeDet in appdoc)
					{
						sino = sino + 1;
						string strTr = "";
						//InstitTradeDet.InstituteDet = _admissionBll.GetITICollegeDetailsByDistrictTalukaBLL(InstitTradeDet.DistrictId, InstitTradeDet.TalukaId);
						//InstitTradeDet.TradeDet = _admissionBll.GetITICollegeTradeDetailsBLL(InstitTradeDet.InstituteId, ApplicantApplicationFormData.InstituteStudiedQual);
						if (sino % 2 != 0)
						{
							strTr = "<tr>";
						}
						if(AplDocTradeDet.Verified==15)
						{
							AplDocTradeDet.UpdateMsg = "Approved";
						}
						replace = strTr + "<td>" + sino + "</td><td>" + AplDocTradeDet.FileName/*.Where(a => a.iti_college_code == InstitTradeDet.InstituteId).Select(a => a.iti_college_name).FirstOrDefault()*/ + "</td><td>" +
								AplDocTradeDet.UpdateMsg/*TradeDet.Where(a => a.trade_id == InstitTradeDet.TradeId).Select(a => a.trade_name).FirstOrDefault()*/ + "</td>";
						if (sino % 2 == 0 || sino == appdoc.Count)
						{
							replace = replace + "</tr>";
						}
						
						html = html.Insert(index1, replace);
						index1 = index1 + replace.Length + 1;
					}

					sb.Append(html);

					//Export HTML String as PDF.
					sb.Replace("#img", Server.MapPath("~/Content/frontend/images/Seal_of_Karnataka.png"));
					sb.Replace("#ApplicationNo.", objInstituteWiseAdmission[0].ApplicantNumber);
					sb.Replace("#ApplicantName", objInstituteWiseAdmission[0].ApplicantName);
					sb.Replace("#Category", objInstituteWiseAdmission[0].CategoryName);
					sb.Replace("#Qualification", objInstituteWiseAdmission[0].Qualification);
					sb.Replace("#Date", objInstituteWiseAdmission[0].PaymentDate.ToString());
					sb.Replace("#Receipt", objInstituteWiseAdmission[0].ReceiptNumber);

					sb.Append(sb.ToString());

					sb.Replace("Student", "Office", sb.Length / 2, sb.Length / 2);    // Replace the second occurrence of Student as Office copy

					StringReader sr = new StringReader(sb.ToString());
					Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

					PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
					pdfDoc.Open();
					XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);

					pdfDoc.Close();
					Response.ContentType = "application/pdf";
					Response.AddHeader("content-disposition", "attachment;filename=" + objInstituteWiseAdmission[0].ApplicantNumber + "-" + ApplicationId + ".pdf");
					Response.Cache.SetCacheability(HttpCacheability.NoCache);
					Response.Write(pdfDoc);
					Response.End();
				}
			}
		}
		public string GetAmtInWords(int amt)
		{
			string strAmt = "";

			if (amt == 0)
				return "zero";

			if ((amt / 1000) > 0)
			{
				strAmt += GetAmtInWords(amt / 1000) + " Thousand ";
				amt %= 1000;
			}

			if ((amt / 100) > 0)
			{
				strAmt += GetAmtInWords(amt / 100) + " Hundred ";
				amt %= 100;
			}

			if (amt > 0)
			{
				if (strAmt != "")
					strAmt += "and ";

				var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
				var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

				if (amt < 20)
					strAmt += unitsMap[amt];
				else
				{
					strAmt += tensMap[amt / 10];
					if ((amt % 10) > 0)
						strAmt += "-" + unitsMap[amt % 10];
				}
			}

			return strAmt;
		}

		private string GetDivisionInitials()
		{
			string divName = "";

			return divName;
		}
	}
}