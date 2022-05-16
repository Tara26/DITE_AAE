using BLL.ExamNotification;
using Models;
using Models.ExamNotification;
using System;
using Models.ExamCenterMap;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Text;
using Spire.Doc;
using Spire.Doc.Documents;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Table = Spire.Doc.Table;
using TableRow = Spire.Doc.TableRow;
using TableCell = Spire.Doc.TableCell;
using Paragraph = Spire.Doc.Documents.Paragraph;
using Microsoft.Office.Interop;
//using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.Data;
using Spire.Pdf;
using System.Threading;
using Spire.Pdf.HtmlConverter;
using Spire.Pdf.Graphics;
using BLL;

using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
//using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using System.Security.Cryptography.X509Certificates;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using iTextSharp.text.pdf.parser;
using Spire.Pdf.Widget;
using System.Drawing;
using Spire.Pdf.AutomaticFields;
using System.Web.Configuration;
using System.Net.Security;
using System.Net;
using X509Certificate = System.Security.Cryptography.X509Certificates.X509Certificate;
using Newtonsoft.Json;
using Path = System.IO.Path;
using BLL.User;
using DIT.Utilities;
using Models.User;
using BLL.Admission;
using BLL.Common;


//using Microsoft.Office.Interop.Word;

namespace DIT.Controllers
{


    public class NotificationController : Controller
    {

        private readonly INotificationBLL _NotifBll;
        private readonly IMarksBLL _marksBll;

        private readonly IUserBLL _LoginService;
        private readonly IAdmissionBLL _admissionBll;
        private readonly ICommonBLL _CommonBLL;

        private readonly string UploadFolder = ConfigurationManager.AppSettings["TemplateDocumentsPath"];
        private readonly string UploadWordFolder = ConfigurationManager.AppSettings["WordTemplateDocumentsPath"];

        //BNM Question Paper Upload
        private readonly string UploadFolderQP = ConfigurationManager.AppSettings["QPDocumentsPath"];

        // strings to be replaced in template
        private readonly string ReplaceNoticeNumberKannada = "&lt;ಅಧಿಸೂಚನೆ ಸಂ.&gt;";
        private readonly string ReplaceNoticeDateKannada = "&lt;ಅಧಿಸೂಚನೆ ದಿನಾಂಕ&gt;";
        private readonly string ReplaceFeesLastDateKannada = "&lt;ಕೊನೆಯ ದಿನಾಂಕ&gt;";

        private readonly string ReplaceNoticeNumberEnglish = "&lt;Notification Number&gt;";
        private readonly string ReplaceNoticeDateEnglish = "&lt;Notification Date&gt;";
        private readonly string ReplaceFeesLastDateEnglish = "&lt;Last Date&gt;";
        private readonly string ReplaceDescription = "&lt;Description&gt;";

        // strings to generate file name
        private readonly string PdfFileNameFormat = "Notification_{0}.pdf";
        private readonly string DocFileNameFormat = "Notification_{0}.docx"; //changed docx to doc
        private readonly string HTMLFilename = "Notification_{0}.html";


        // function to replace the strings
        private string ReplaceDataInTemplate(string content, Notification model, bool onlyNofificationDate = false)
        {
            if(onlyNofificationDate)
            {
                DateTime dateTime = DateTime.Now;
                string notificationDate = string.Format("{0:D2}/{1:D2}/{2}", dateTime.Day, dateTime.Month, dateTime.Year);
                content = content.Replace(ReplaceNoticeDateKannada, notificationDate);
                content = content.Replace(ReplaceNoticeDateEnglish, notificationDate);
            }
            else
            {
                content = content.Replace(ReplaceNoticeDateKannada, model.Exam_notif_date.ToString("dd:MM:yyyy"));
                content = content.Replace(ReplaceNoticeNumberKannada, model.Exam_Notif_Number);
                content = content.Replace(ReplaceFeesLastDateKannada, ((DateTime)model.fee_pay_last_date).ToString("dd:MM:yyyy"));

                content = content.Replace(ReplaceNoticeDateEnglish, model.Exam_notif_date.ToString("dd:MM:yyyy"));
                content = content.Replace(ReplaceNoticeNumberEnglish, model.Exam_Notif_Number);
                content = content.Replace(ReplaceFeesLastDateEnglish, ((DateTime)model.fee_pay_last_date).ToString("dd:MM:yyyy"));
            }

            return content;
        }
        public NotificationController()
        {
            _marksBll = new MarksBLL();
            _NotifBll = new NotificationBLL();
            this._LoginService = new UserBLL();
            _admissionBll = new AdmissionBLL();
            _CommonBLL = new CommonBLL();
        }

        // GET: Notification
        public ActionResult Index()
        {
      CheckBrowserAndMenuUl();
      return View();
        }

        /// <summary>
        /// get action method for creating notification
        /// </summary>
        /// <param name="notificationId">holds the notification id for updating notifcation, otherwise it will be null</param>
        /// <returns></returns>
        public ActionResult CreateNotification(int? notificationId, int? selectedTab)
        {
            //CheckBrowserAndMenuUl();
            _CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            
            // create model object  
            Notification model = new Notification();
            int loginId = Convert.ToInt32(Session["LoginId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            string content = string.Empty;
            model.Exam_notif_date = DateTime.Now;
            model.fee_pay_last_date= DateTime.Now;
            //Kannada template
            //content = "<p style=\"text-align: center; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಕರ್ನಾಟಕ ಸರ್ಕಾರ </p><p style=\"font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ದಿನಾಂಕ : &lt;ಅಧಿಸೂಚನೆ ದಿನಾಂಕ&gt;</p><p style=\"text-align: right; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ ಸಂ.: &lt;ಅಧಿಸೂಚನೆ ಸಂ.&gt;</p><p>&nbsp;</p><p style=\"text-align: center;font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ</p><br>";
            content = "<h2 style=\"text-align: center;\"><span style=\"font-family: 'times new roman', times, serif;\">Government of Karnataka</span></h2>\r\n<p>&nbsp;</p>\r\n<p class=\"MsoNormal\" style=\"margin-bottom: 0cm; line-height: normal; mso-layout-grid-align: none; text-autospace: none;\"><span style=\"font-size: 12pt; font-family: 'times new roman', times, serif;\">&lt;Notification Number&gt;</span></p>\r\n<p class=\"MsoNormal\" style=\"text-align: right; line-height: 1;\" align=\"right\"><span style=\"font-family: 'times new roman', times, serif;\">Industrial training and employment</span></p>\r\n<p class=\"MsoNormal\" style=\"text-align: right; line-height: 1;\" align=\"right\"><span style=\"font-family: 'times new roman', times, serif;\">Skill House, Dairy Circle,</span></p>\r\n<p class=\"MsoNormal\" style=\"text-align: right; line-height: 1;\" align=\"right\"><span style=\"font-family: 'times new roman', times, serif;\">Bannerghatta Road, Bangalore-560 029,</span></p>\r\n<p class=\"MsoNormal\" style=\"text-align: right; line-height: 1;\" align=\"right\"><span style=\"font-family: 'times new roman', times, serif;\">Date: &lt;Notification Date&gt;</span></p>\r\n<p class=\"MsoNormal\" style=\"text-align: center;\" align=\"center\"><span style=\"font-family: 'times new roman', times, serif;\">NOTIFICATION</span></p>\r\n<p class=\"MsoNormal\" style=\"text-align: center;\" align=\"center\"><span style=\"font-family: 'times new roman', times, serif;\">Topic:&lt;Description&gt;</span></p>";
           // content = "<p style=\"text-align: center; \">Government of Karnataka</p><p>Notification Date: &lt;Notification Date&gt;</p><p style=\"text-align: right;\">Notification Number : &lt;Notification Number&gt;</p><p>&nbsp;</p><p style=\"text-align: center;\">NOTIFICATION</p><br>";

            if (notificationId != null)
            {
                model = _NotifBll.GetUpdateNotificationBLL(notificationId)[0];
                string DocumentsFolder = GetUploadFolderPath(); 
                //Replace the Notification number slash by hipen
                string fileName = model.Exam_Notif_Number.Replace(@"/", "_");
                string wordpath = string.Format(DocumentsFolder + DocFileNameFormat, fileName);
                content = ReadWordFile(wordpath);
                // replace the template date and notification number
                //content = ReplaceDataInTemplate(content, model);
            }
            else
            {
                content = ReplaceDataInTemplate(content, model,true);
            }
            model.content = content;
            //read the notification detail dropdowns
            model.DeptId = 1;
            model.NotifDescId = 1;
            model.CourseList = _NotifBll.GetCourseListBLL();
            model.DeptList = _NotifBll.GetDepartmentListBLL();
            model.NotifDescList = _NotifBll.GetNotificationDescListBLL();

            //read last notification id
            model.LastNotigicationId = _NotifBll.LastNotificationNumberBLL(model);
            foreach (var department in model.DeptList)
            {
                if (department.Value == model.DeptId.ToString())
                {
                    model.DeptName = department.Text;
                    break;
                }
            }
            foreach (var notificationDescription in model.NotifDescList)
            {
                if (notificationDescription.Value == model.DeptId.ToString())
                {
                    model.Exam_Notif_Desc = notificationDescription.Text;
                    break;
                }
            }
            //for Landing page Notification ststus
            model.selectTab = 0;
            if (selectedTab != null)
            {
                model.selectTab = selectedTab;
            }
            return View(model);
        }

        /// <summary>
        /// Function to convert docx  to pdf file
        /// </summary>
        /// <param name="wordFile">word file path</param>
        /// <param name="pdfFile">pdf file path</param>
        /// <returns></returns>
        //private JsonResult Word2Pdf(string wordFile, string pdfFile)
        //{
        //    try
        //    {
        //        // Create a new Microsoft Word application object
        //        Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
        //        // C# doesn't have optional arguments so we'll need a dummy value
        //        object oMissing = System.Reflection.Missing.Value;
        //        word.Visible = false;
        //        word.ScreenUpdating = false;
        //        // Cast as Object for word Open method

        //        Object filename = (Object)wordFile;
        //        // Use the dummy value as a placeholder for optional arguments
        //        Microsoft.Office.Interop.Word.Document doc = word.Documents.Open(ref filename, ref oMissing,
        //            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //            ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        //        doc.Activate();

        //        object outputFileName = pdfFile;
        //        object fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;

        //        // Save document into PDF Format
        //        doc.SaveAs(ref outputFileName,
        //             ref fileFormat, ref oMissing, ref oMissing,
        //             ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //             ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //             ref oMissing, ref oMissing, ref oMissing, ref oMissing);

        //        // Close the Word document, but leave the Word application open.
        //        // doc has to be cast to type _Document so that it will find the
        //        // correct Close method. 

        //        object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
        //        ((Microsoft.Office.Interop.Word._Document)doc).Close(ref saveChanges, ref oMissing, ref oMissing);
        //        doc = null;

        //        // word has to be cast to type _Application so that it will find
        //        // the correct Quit method.
        //        ((Microsoft.Office.Interop.Word._Application)word).Quit(ref oMissing, ref oMissing, ref oMissing);
        //        word = null;

                

        //        return Json("success");

        //    }
        //    catch (Exception ex)
        //    {
        //        var relativePath = "LogFile.txt";
        //      //  string absolutePath = Path.Combine(HttpRuntime.AppDomainAppPath, relativePath);
        //        string Text = " "+ ex.Message;
        //        if (!System.IO.File.Exists(absolutePath))
        //        {
        //            using (FileStream fs = System.IO.File.Create(absolutePath))
        //            {
        //                Text = "Errors: " + Environment.NewLine + Environment.NewLine;
        //                System.IO.File.AppendAllText(absolutePath, Text);
        //                fs.Close();
        //            }
        //        }
        //        Text = ex.ToString();//"fkdkg"

        //        System.IO.File.AppendAllText(absolutePath, Text);

        //        return Json("Failed");
        //    }
        //}

     
        private string ReadWordFile(string wordFile)
        {
            string result = string.Empty;
            try
            {
                if(System.IO.File.Exists(wordFile))
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
            catch(Exception ex)
            {
                
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetUploadFolderPath()
        {
            string uploadFolderPath = Request.PhysicalApplicationPath + UploadFolder;
            uploadFolderPath = uploadFolderPath.Replace("//", "\\");
            uploadFolderPath = uploadFolderPath.Replace("\\\\", "\\");
            if (!System.IO.Directory.Exists(uploadFolderPath))
            {
                System.IO.Directory.CreateDirectory(uploadFolderPath);
            }
            return uploadFolderPath;

        }

        /// <summary>
        /// Post method to create or update notification
        /// </summary>
        /// <param name="model">Notification model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateNotification(Notification model)
        {
      Utilities.Security.ValidateRequestHeader(Request);
      model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.user_id = Convert.ToInt32(Session["UserId"]);
            model.CourseList = _NotifBll.GetCourseListBLL();
            model.DeptList = _NotifBll.GetDepartmentListBLL();
            model.NotifDescList = _NotifBll.GetNotificationDescListBLL();

            if (ModelState.IsValid)
            {
                List<SelectListItem> notifDescr = (List<SelectListItem>)model.NotifDescList.Items;
                model.NotifDescName = notifDescr[model.NotifDescId].Text;

                // create the folder directory wherer notifcation are stored if it doesn't exists

                string DocumentsFolder = GetUploadFolderPath();               

                //Replace the Notification number slash by hipen
                string fileName = model.Exam_Notif_Number.Replace(@"/", "_");

                // get full file path
                string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat, fileName);
                string wordpath = string.Format(DocumentsFolder + DocFileNameFormat, fileName);
                string htmlpath = string.Format(DocumentsFolder + HTMLFilename, fileName);//vidhya Changes

                // replace the date amd notification number notification data
                model.content = ReplaceDataInTemplate(model.content, model);


                //Doc save
                Spire.Doc.Document doc = new Spire.Doc.Document();
                doc.AddSection();
                Spire.Doc.Documents.Paragraph para = doc.Sections[0].AddParagraph();
                para.AppendHTML(model.content);
                doc.SaveToFile(wordpath, Spire.Doc.FileFormat.Docx);// changed Docx to doc
                doc.SaveToFile(htmlpath, Spire.Doc.FileFormat.Html);


                Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
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

                PdfUnitConvertor unitCvtr = new PdfUnitConvertor();

                PdfMargins margin = new PdfMargins();

                margin.Top = unitCvtr.ConvertUnits(2.54f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

                margin.Bottom = margin.Top;

                margin.Left = unitCvtr.ConvertUnits(3.17f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

                margin.Right = margin.Left;

                DrawPageNumber(pdf.Pages, margin);


                pdf.SaveToFile(pdfpath, Spire.Pdf.FileFormat.PDF);
                pdf.Close();
                //Pagination number 
                //Spire.Pdf.PdfDocument pdfDoc = new Spire.Pdf.PdfDocument();
                //pdfDoc.LoadFromFile(pdfpath);

                //PdfUnitConvertor unitCvtr = new PdfUnitConvertor();

                //PdfMargins margin = new PdfMargins();

                //margin.Top = unitCvtr.ConvertUnits(2.54f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

                //margin.Bottom = margin.Top;

                //margin.Left = unitCvtr.ConvertUnits(3.17f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

                //margin.Right = margin.Left;

                //DrawPageNumber(pdfDoc.Pages, margin);

                ////Befor Save
                //pdfDoc.SaveToFile(pdfpath, Spire.Pdf.FileFormat.PDF);
                //pdfDoc.Close();

                if (System.IO.File.Exists(htmlpath))
                {
                    System.IO.File.Delete(htmlpath);
                }

                // remove temp html folder
                 string folderPath = htmlpath.Replace(".html", "_files");
                if (System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.Delete(folderPath, true);
                }

                doc.Close();
                // convert to pdf
             // Word2Pdf(wordpath, pdfpath);

                //Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                //wordDocument = appWord.Documents.Open(wordpath);
                //wordDocument.ExportAsFixedFormat(pdfpath, WdExportFormat.wdExportFormatPDF);

                //   public Microsoft.Office.Interop.Word.Document wordDocument { get; set; }

                // remove the word document post creation of respective pdf
                //if(System.IO.File.Exists(wordpath))
                //{
                //    System.IO.File.Delete(wordpath);
                //}

                // now save the notification details to database
                model.SavePath = string.Format(UploadFolder + PdfFileNameFormat, fileName);

                var saved = _NotifBll.CreateNotificationDetailsBLL(model);
                if (saved == "Saved")
                {
                    if(model.Exam_Notif_Id !=0 )
                    {
                        TempData["Saved"] = "Updated";
                    }
                    else
                    {
                        TempData["Saved"] = "Created";
                        if (model.Action == 0)
                        {
                            TempData["Saved"] = "Draft";
                        }
                    }
                    
                    model.selectTab = 1;
                }
                
                TempData["NotificationNumber"] = model.Exam_Notif_Number;
            }
            else
            {
                TempData["Saved"] = "Invalid";
            }
            return View(model);
        }
        public Microsoft.Office.Interop.Word.Document wordDocument { get; set; }

        private static void DrawPageNumber(PdfPageCollection collection, PdfMargins margin)

        {

            foreach (PdfPageBase page in collection)

            {

                PdfBrush brush = PdfBrushes.Black;

                PdfTrueTypeFont font = new PdfTrueTypeFont(new System.Drawing.Font("Arial", 10f, FontStyle.Bold), true);

                PdfStringFormat format = new PdfStringFormat(PdfTextAlignment.Left);

                int x = Convert.ToInt32(page.Canvas.ClientSize.Width / 2);

                int y = Convert.ToInt32(page.Canvas.ClientSize.Height - margin.Bottom);

                System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(x, y, 50, 20);

                PdfPageNumberField field = new PdfPageNumberField();

                field.Font = font;

                field.Brush = brush;

                field.StringFormat = format;

                field.Bounds = bounds;

                field.Draw(page.Canvas);

            }

        }


        //public ActionResult UpdateNotificationDet()
        //{
        //    CheckBrowserAndMenuUl();
        //    Notification model = new Notification();
        //    model.GetUpdateNotifDet = _NotifBll.GetUpdateNotificationBLL();
        //    return View(model);
        //}

        public ActionResult Notificationstatus()
		{
      //CheckBrowserAndMenuUl();
      Notification modal = new Notification();
            List<Notification> notifications = new List<Notification>();

            modal.login_id = Convert.ToInt32(Session["LoginId"]);
            List<Notification> list = _NotifBll.GetNotificationStatusBLL(modal).ToList();
            modal.LoginRoleList = _NotifBll.GetLoginRoleListForwardBLL(modal.login_id);
            modal.notifications = list;
           modal.notifications = _NotifBll.GetNotificationStatusBLL(modal);
            return View(modal);
        }

        [HttpPost]
        public ActionResult GetDetails(int ID)
        {
            try
            {
                //Notification modal = new Notification();
                int loginId = Convert.ToInt32(Session["LoginId"]);
                Notification details = this._NotifBll.GetViewBLL(ID, loginId);
                Notification transactiondetails = this._NotifBll.GetNotificationtransactiondtlsBLL(ID, loginId);
                //details.comcount = _NotifBll.GetCommentsListBLL(ID).Count();
               string fileName = details.Exam_Notif_Number.Replace(@"/", "_");
                string PdfFileName = Path.GetFileNameWithoutExtension(details.SavePath);
                string DocFileName = Path.GetFileNameWithoutExtension(transactiondetails.DocSavePath);
                details.filename = PdfFileName;
                details.Docfilename = DocFileName;
                //fileName.details =

                //modal.employeelist = details;
                return Json(details, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        [HttpPost]
        public string UpdateTransStatus(int Id, int Status, int Loginid, string comments)
        {
            Notification model = new Notification();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.Exam_Notif_Id = Id;
            model.exam_notif_status_id = Status;
            model.RoleId = Loginid;
            model.comments = comments;
            int? updateemployee = this._NotifBll.UpdateTransStatusBLL(model);
            string res = updateemployee.ToString();
            return res;
        }

        [HttpPost]
        public JsonResult UpdateCommentTransStatus(int Id, int Status, int Loginid, string comments)
        {
            Notification model = new Notification();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.Exam_Notif_Id = Id;
            model.exam_notif_status_id = Status;
            model.RoleId = Loginid;
            model.comments = comments;
            string updateemployee = this._NotifBll.UpdateCommentsTransStatusBLL(model);
            //string res = updateemployee;
            return Json(updateemployee, JsonRequestBehavior.AllowGet);
        }

        //Admission and Affiliation and Examination Module Integration in progress

        [HttpPost]
        public JsonResult PublishNotification(int Id, int Status)
        {
            string fileName = "";
            Notification model = new Notification();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.Exam_Notif_Id = Id;
            model.exam_notif_status_id = Status;
            //  model.RoleId = Loginid;
            
            string updateStatus = this._NotifBll.PublishNotificationBLL(model);
            string res = updateStatus.ToString();
            //Send Notification Pdf to All ITI Institute
            List<string> lstEmailIds = _NotifBll.ExamCenterMailIdBLL();
            var emailIds = String.Join(",", lstEmailIds);
            //foreach(var addres in lstEmailIds)
            //{
            //    mailMessagePlainText.To.Add(new MailAddress(address.Trim(), ""));
            //}
            
            var filepath = _NotifBll.GetPublishedFilePathBLL(model);
            if(filepath!=null)
            {
                foreach(var file_path in filepath)
                {
                    var file = "";
                    file = file_path.SavePath;
                    fileName = file.Substring(21);
                }
            }
            try
            {
                byte[] file = System.IO.File.ReadAllBytes(GetUploadFolderPath() + "\\" + fileName);
                using (MemoryStream memory = new MemoryStream(file))
                {
                    if(!SendEmailWithAttachment(emailIds, "HI, Please Find The Attached Notification File for your reference ", "Notification File", memory))
                    {
                        res = "Failed";
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public bool SendEmailWithAttachment(string to_Address, string Body, string Subject, MemoryStream AttachmentFilePath)
        {
            bool bSuccess = false;
            try
            {
                
                string emailID = WebConfigurationManager.AppSettings["EmailID"];
                string Password = WebConfigurationManager.AppSettings["MailPassword"];
                string AppHost = WebConfigurationManager.AppSettings["Host"];
                var AppPort = (WebConfigurationManager.AppSettings["Port"]);
                string subject = Subject;
                string body = Body;

                var smtp = new SmtpClient
                {

                    Host = AppHost,
                    Port = int.Parse(AppPort),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailID, Password)
                };

                ServicePointManager.ServerCertificateValidationCallback += delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(emailID, to_Address);
                message.Subject = subject;
                message.Body = body;
                message.Attachments.Add(new Attachment(AttachmentFilePath, "NB Bond", "application/pdf"));
                {

                    smtp.Send(message);
                }
                bSuccess = true;
            }

            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                //Logger.LogMessage(TracingLevel.INFO, "Mail with attachment  - " + message);

            }
            return bSuccess;
        }

        public ActionResult Notificationstatus1()
		{
      CheckBrowserAndMenuUl();
            Notification modal = new Notification();
            modal.RoleId = Convert.ToInt32(Session["LoginId"]);
            // modal.LoginRoleList = _NotifBll.GetLoginRoleListBLL(modal.RoleId);
            modal.LoginRoleList = _NotifBll.GetLoginRoleListForwardBLL(modal.RoleId);
            modal.LoginRoleListSendBack = _NotifBll.GetLoginRoleListSendBackBLL(modal.RoleId);
            //modal.user_role = Session["UserRole"].ToString();
            List<Notification> list = _NotifBll.GetNotificationStatus1BLL(modal).ToList();
            modal.notifications = list;
            return View(modal);
        }

        public ActionResult ExamCalenderMasterEntry(ExamCalendarMaster model)
        {
      CheckBrowserAndMenuUl();
            model.user_id = Convert.ToInt32(Session["LoginId"]);
            model.RoleId = Convert.ToInt32(Session["UserRoleId"]);
            //model.role_id = Convert.ToInt32(Session["UserId"]);
            model.CourseTypeList = _NotifBll.GetCourseListBLL();
            model.ExamSemList = _NotifBll.GetExamSemListBLL();
            model.ExamTypeList = _NotifBll.GetExamTypeListBLL();
            //model.SubjectList = _NotifBll.GetSubjectListBLL();
            model.SubjectTypeList = _NotifBll.GetSubjectTypeListBLL();
            model.SubjectType = _NotifBll.SubjectTypeBLL();
            model.LoginRoleListSendBack = _NotifBll.GetLoginRoleListSendBackBLL(model.user_id);
            //model.TradeList = _NotifBll.GetTradeListBLL();
            //model.TradeTypeList = _NotifBll.GetTradeTypeListBLL();
            model.TradeYearList = _NotifBll.GetTradeYearListBLL();
            model.SpecialTradeTypeList = _NotifBll.GetSpecialTradeTypeListBLL();
            model.ExamNotificationList = _NotifBll.ExamNotificationListBLL();
            model.NotificationForApprovalList = _NotifBll.GetNotificationForApprovalBLL(model).ToList();
            //var SubjectList = _NotifBll.GetRemainingSubjectListBasedOnIdBLL(null, true);
            //BNM
            //model.ExamCentresEmailList = _NotifBll.GetExamCentresEmailIDListBLL();
            //model.NotificationForApprovalForModificationList = _NotifBll.GetNotificationForApprovalBLL(model).Where(x=>x.status_id == 102).ToList();
            //model.NotificationForApprovedList = _NotifBll.GetNotificationForApprovalBLL(model).Where(x => x.status_id == 103).ToList();
            //model.PublishedNotificationForModificationList = _NotifBll.GetPublishedNotificationBLL(model).ToList();
            model.LoginRoleList = _NotifBll.GetLoginRoleListForwardBLL(model.user_id);
            //ViewBag.mySkills = _NotifBll.GetSubjectListBLL();

            if (model.CourseName != null || model.ExamSemName != null || model.ExamTypeName != null
                || model.SubjectName != null || model.SubjectTypeName != null || model.TradeName != null || model.TradeTypeName != null)
            {

                var res = _NotifBll.SaveExamCalNotificationBLL(model);
            }
            model.selectTab = 1;

            List<SelectListItem> SubjectsList = new List<SelectListItem>();
            var SubjList = new SelectListItem()
            {

                Value = null,
                Text = "Select Subject",

            };
            SubjectsList.Insert(0, SubjList);
            ViewBag.mySkills = new SelectList(SubjectsList, "Value", "Text");

            return View(model);
        }

        [HttpPost]
        public JsonResult ExamCalenderMasterUpload(ExamCalendarMaster model)
        {
      Utilities.Security.ValidateRequestHeader(Request);
      model.user_id = Convert.ToInt32(Session["LoginId"]);
            //model.role_id = Convert.ToInt32(Session["UserId"]);
            var res = _NotifBll.ExamCalenderMasterUploadBLL(model);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTradeList(int CourseTypeId, int? TradeTypeId)
        {
            var TradeList = _NotifBll.GetTradeListBasedOnIdBLL(CourseTypeId, TradeTypeId);
            return Json(TradeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubjectList(int CourseTypeId, int? STradeId, int? subjectType)
        {
            var SubjectList = _NotifBll.GetSubjectListBasedOnIdBLL(CourseTypeId, STradeId, subjectType);
            return Json(SubjectList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubjectTypeList(int CourseTypeId)
        {
            var SubjectList = _NotifBll.GetSubjectTypeListBasedOnIdBLL(CourseTypeId);
            return Json(SubjectList, JsonRequestBehavior.AllowGet);
        }

        //public FileResult GetReport()
        //{
        //    string ReportURL = AppDomain.CurrentDomain.BaseDirectory + "/Content/Template/Notification_ೂೋೀೋಲರಸ್ಪೀೀಸೋಕೀ7734.pdf";
        //    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
        //    return File(FileBytes, "application/pdf");
        //}



        [HttpPost]
        public JsonResult BackToCW(int Id, int Status, int Loginid, string comments, int StatusforCW)
        {
      Utilities.Security.ValidateRequestHeader(Request);
            Notification model = new Notification();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.Exam_Notif_Id = Id;
            model.exam_notif_status_id = Status;
            model.RoleId = Loginid;
            model.updatestatusCW = StatusforCW;
            model.comments = comments;
            string updateemployee = this._NotifBll.UpdateCWStatusBLL(model);
            //string res = updateemployee.ToString();
            model.res = updateemployee.ToString();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveNotifiedSubjects(ExamCalendarMaster model)
        {
            model.user_id = Convert.ToInt32(Session["LoginId"]);
            var res = _NotifBll.SaveNotifiedSubjectsBLL(model);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubjectsItemList(int NotificationId)
        {
            var srList = _NotifBll.GetSubjectsItemListBLL(NotificationId);
            return Json(srList, JsonRequestBehavior.AllowGet);
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

        public JsonResult GoBackToModification(Notification model)
        {
            model.user_id = Convert.ToInt32(Session["LoginId"]);
            //model.role_id = Convert.ToInt32(Session["UserId"]);
            var Remarks = _NotifBll.GoBackToModificationBLL(model);
            return Json(Remarks, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangesToModification(Notification model)
        {
            model.user_id = Convert.ToInt32(Session["LoginId"]);
            //model.role_id = Convert.ToInt32(Session["UserId"]);
            var Remarks = _NotifBll.ChangesToModificationBLL(model);
            return Json(Remarks, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SendingQuestionPaers(QuestionPaperSets model)
        {
      CheckBrowserAndMenuUl();
            model.user_id = Convert.ToInt32(Session["LoginId"]);
            model.CourseTypeList = _NotifBll.GetCourseListBLL();
            model.ExamSemList = _NotifBll.GetExamSemListBLL();
            model.ExamTypeList = _NotifBll.GetExamTypeListBLL();
            model.SubjectList = _NotifBll.GetSubjectListBLL();
            model.SubjectTypeList = _NotifBll.GetSubjectTypeListBLL();
            model.TradeList = _NotifBll.GetTradeListBLL();
            model.TradeTypeList = _NotifBll.GetTradeTypeListBLL();
            model.TradeYearList = _NotifBll.GetTradeYearListBLL();
            //BNM for question paper set population
            model.QuestionPaperSet = _NotifBll.GetQuestionPaperSetListBLL();
            //BNM
            model.ExamCentresEmailList = _NotifBll.GetExamCentresEmailIDListBLL();
            // model.NotificationForApprovalList = _NotifBll.GetNotificationForApprovalBLL(model).ToList();

            //if (model.Course != null || model.ExamSemName != null || model.ExamTypeName != null
            //    || model.SubjectName != null || model.SubjectTypeName != null || model.TradeName != null || model.TradeTypeName != null)
            //{
            //    var res = _NotifBll.SaveExamCalNotificationBLL(model);
            //}
            model.selectTab = 0;
            return View(model);
        }


        //BNM
        //     Notification modal = new Notification();
        //     List<Notification> notifications = new List<Notification>();

        //     modal.login_id = Convert.ToInt32(Session["LoginId"]);
        //List<Notification> list = _NotifBll.GetNotificationStatusBLL(modal).ToList();
        //     modal.LoginRoleList = _NotifBll.GetLoginRoleListBLL();
        //modal.notifications = list;
        //         //modal.notifications = _NotifBll.GetNotificationStatusBLL();
        //         return View(modal);
        //   [HttpGet]
        public ActionResult GetSearchModifyQuestionPapers()//(int CourseTypeID, int TradeTypeID, int TradeID, int ExamTypeID, int ExamSubTypeID, int ExamSubID)
        {
            ViewModel mymodel = new ViewModel();

            mymodel.questionPaperSets = SearchModifyPopulateDDL();
            mymodel.questionPaperList = SearchModifyQuestionPapers(100, 14, 13, 4, 9, 9, 23); //(100, 14, 13, 4, 9, 9, 23);//  (0,0,0,0,0,0,0);
            return View(mymodel);
        }

        public JsonResult GetSearchModifyQuestionPapers1(int CourseTypeID, int TradeTypeID, int TradeID, int @TradeYearID, int ExamTypeID, int ExamSubTypeID, int ExamSubID)
        {
            ViewModel mymodel = new ViewModel();

            mymodel.questionPaperSets = SearchModifyPopulateDDL();
            mymodel.questionPaperList = SearchModifyQuestionPapers(CourseTypeID, TradeTypeID, TradeID, @TradeYearID, ExamTypeID, ExamSubTypeID, ExamSubID);
            return Json(mymodel, JsonRequestBehavior.AllowGet);
        }

        public List<QuestionPaper> SearchModifyQuestionPapers(int CourseTypeID, int TradeTypeID, int TradeID, int @TradeYearID, int ExamTypeID, int ExamSubTypeID, int ExamSubID)
        {
            List<QuestionPaper> subjects = new List<QuestionPaper>();
            // int CourseTypeID; int TradeTypeID = 1; int TradeID = 1; int ExamTypeID = 1; int ExamSubTypeID = 1; int ExamSubID = 1;
            //    CourseTypeID = 1;
            var model = _NotifBll.getGetSearchModifyQuestionPapersBLL(CourseTypeID, TradeTypeID, TradeID, @TradeYearID, ExamTypeID, ExamSubTypeID, ExamSubID);
            //   var userData = _LoginService.GetSubjectsBll();
            return model;
        }
        public QuestionPaperSets SearchModifyPopulateDDL()
        {
            QuestionPaperSets mymodel = new QuestionPaperSets();
            // mymodel.questionPaperSets.user_id = Convert.ToInt32(Session["LoginId"]);
            mymodel.CourseTypeList = _NotifBll.GetCourseListBLL();
            mymodel.ExamSemList = _NotifBll.GetExamSemListBLL();
            mymodel.ExamTypeList = _NotifBll.GetExamTypeListBLL();
            mymodel.SubjectList = _NotifBll.GetSubjectListBLL();
            mymodel.SubjectTypeList = _NotifBll.GetSubjectTypeListBLL();
            mymodel.TradeList = _NotifBll.GetTradeListBLL();
            mymodel.TradeTypeList = _NotifBll.GetTradeTypeListBLL();
            mymodel.TradeYearList = _NotifBll.GetTradeYearListBLL();
            //BNM for question paper set population
            mymodel.QuestionPaperSet = _NotifBll.GetQuestionPaperSetListBLL();
            //BNM
            mymodel.ExamCentresEmailList = _NotifBll.GetExamCentresEmailIDListBLL();
            return mymodel;
        }

        public ActionResult Edit_GetSearchModifyQuestionPapers1(int ID)
        {
            ViewModel mymodel = new ViewModel();
            mymodel.questionPaperList = mymodel.questionPaperList.Where(s => s.QPSTID == ID);//.FirstOrDefault();
            mymodel.questionPaperSets.CourseTypeList = _NotifBll.GetCourseListBLL();
            // ViewBag.EntidadList = SearchModifyPopulateDDL();
            //    ViewBag.EntidadList = _NotifBll.GetCourseListBLL();
            // mymodel.questionPaperSets.CourseTypeList = _NotifBll.GetCourseListBLL().Select(m => new SelectListItem { Value = m.Value, Text = m.Text }).FirstOrDefault();
            //  ViewBag.CourseID = _NotifBll.GetCourseListBLL().Select(m => new SelectListItem { Value = m.Value, Text = m.Text }).FirstOrDefault();
            //  ViewBag.CourseID = _NotifBll.GetCourseListBLL();
            //   ViewBag.CourseID = new SelectList(mymodel.questionPaperSets.CourseTypeList, "Value", "Text", mymodel.questionPaperSets.course_id);
            // ViewBag.CourseID = new SelectList(_NotifBll.GetCourseListBLL(), "CourseID", "CourseName", mymodel.questionPaperSets.course_id);
            //mymodel.questionPaperSets = SearchModifyPopulateDDL();
            //     mymodel.questionPaperList = SearchModifyQuestionPapers(CourseTypeID, TradeTypeID, TradeID, @TradeYearID, ExamTypeID, ExamSubTypeID, ExamSubID);
            // mymodel.questionPaperList = SearchModifyQuestionPapers(100, 14, 13, 4, 9, 9, 22);

            //ViewBag.DDU = new SelectList(mymodel.questionPaperSets.CourseTypeList, "Value", "Text", std.CourseType);
            return View(mymodel);
        }

        //Status of QuestionPaper Sets Tab :
        public ActionResult QPSStatus()
        {

            return View();
        }


        public JsonResult PublishCalTimeTableNotification(int NotificationId, int PublishedId, int FromReg)
        {
            System.Data.DataTable dtsubjects = _NotifBll.GetNotificationForSubjectsBLL(NotificationId, PublishedId);

            ExamCalendarMaster model = new ExamCalendarMaster();
            model.NotificationId = NotificationId;
            model.PublishedNotificationForModificationList = _NotifBll.GetPublishedNotificationIDBLL(model);
            string examtimeTable = "Exam Time Table";
            
            if (model.PublishedNotificationForModificationList.Count>0 && model.PublishedNotificationForModificationList!=null)
            {
                examtimeTable = "Revised Exam Time Table";
            }



            string rootpath = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Notifications";
            if (!Directory.Exists(rootpath))
            {
                Directory.CreateDirectory(rootpath);
            }

            StringBuilder headerSb = new StringBuilder();// For Common data header
            StringBuilder DynamicSb = new StringBuilder();//For Dynamic data 
            StringBuilder footerSb = new StringBuilder();// For Common data footer
            string PdfdbPath = string.Empty;

            StringBuilder Final = new StringBuilder();
            StringBuilder FinalStringBuilder = new StringBuilder();
            StringBuilder finalNotsixiterated = new StringBuilder();
            string actulalhtmlBody = @"<table class='mce-item-table' style='border: 1px dashed rgb(187, 187, 187); color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'>
    <tbody>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='697'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><strong><span style='font-size: 9px;'>&nbsp;GOVERNMENT OF KARNATAKA</span></strong></p>
            </td>
        </tr>
    </tbody>
</table>
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'>&nbsp;</span></p>
<table class='mce-item-table' style='border: 1px dashed rgb(187, 187, 187); color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'>
    <tbody>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='587'>
                <p><span style='font-size: 9px;'>No.ITE/TRG/TTC/CTS-6/CR-06/2020-21/Exam_Notify_Number</span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='302'>
                <p><span style='font-size: 9px;'>&nbsp;</span></p>
            </td>
        </tr>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='587'>
                <p><span style='font-size: 9px;'>&nbsp;</span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='302'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'>Industrial Training and Employment</span></p>
            </td>
        </tr>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='587'>
                <p><span style='font-size: 9px;'>&nbsp;</span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='302'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'>Kaushalya Bhavan, Bannerghatta Road</span></p>
            </td>
        </tr>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='587'>
                <p><span style='font-size: 9px;'>&nbsp;</span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='302'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'>Near Dairy Cricle, Hosur Road</span></p>
            </td>
        </tr>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='587'>
                <p><span style='font-size: 9px;'>&nbsp;</span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='302'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'>Bangalore-560 029,&nbsp;</span></p>
            </td>
        </tr>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='587'>
                <p><span style='font-size: 9px;'>&nbsp;</span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='302'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'>Dated:Exam_Notif_date</span></p>
            </td>
        </tr>
    </tbody>
</table>
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'><strong>&nbsp;</strong></span></p>
<table border='1' style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'>
    <tbody>
        <tr data-mce-style='height: 35px;' style='height: 35px;'>
            <td colspan='5' data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='707'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong><u>STATE TRADE TEST OF SCVT UNDER CTS AUGUST/SEPTEMBER - Exam_Notif_year</u></strong></span></p>
            </td>
        </tr>
        <tr data-mce-style='height: 31px;' style='height: 31px;'>
            <td colspan='5' data-mce-style='height: 31px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 31px;' width='707'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong><u> examtimeTable </u></strong></span></p>
            </td>
        </tr>
        <tr data-mce-style='height: 21px;' style='height: 21px;'>
            <td colspan='5' data-mce-style='height: 21px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 21px;' width='707'>
                <p><span style='font-size: 9px;'><strong><u><br></u></strong></span></p>
            </td>
        </tr>
        <tr data-mce-style='height: 35px;' style='height: 35px;'>
            <td data-mce-style='height: 83px;' rowspan='2' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 83px;' width='67'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong>DATE</strong></span></p>
            </td>
            <td data-mce-style='height: 83px;' rowspan='2' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 83px;' width='70'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong>DAY</strong></span></p>
            </td>
            <td data-mce-style='height: 83px;' rowspan='2' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 83px;' width='78'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong>TIME OF COMMEN CEMENT</strong></span></p>
            </td>
            <td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='193'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong>SUBJECTS</strong></span></p>
            </td>
            <td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='304'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong>SUBJECTS</strong></span></p>
            </td>
        </tr>
        <tr data-mce-style='height: 48px;' style='height: 48px;'>
            <td data-mce-style='height: 48px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 48px;' width='193'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong>(Regular Trades)</strong></span></p>
            </td>
            <td data-mce-style='height: 48px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 48px;' width='304'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong>(Special trades - Sugar, Fire, Real Estate, Water Technology)</strong></span></p>
            </td>
        </tr>";
            string htmlBody = actulalhtmlBody.Replace("Exam_Notify_Number", dtsubjects.Rows.Count > 0 ? dtsubjects.Rows[0]["NotificationNumber"].ToString() : "");
            string datet = Convert.ToString(dtsubjects.Rows.Count > 0 ? dtsubjects.Rows[0]["CreationDate"] : "");
            string yearExam = Convert.ToString(dtsubjects.Rows.Count > 0 ? dtsubjects.Rows[0]["CreationDate"] : "");
            DateTime myDateTime = DateTime.Now;
            string year = myDateTime.Year.ToString();
           
            
            string htmlactualBody1 = htmlBody.Replace("Exam_Notif_date", datet);
            string htmlactualBody2 = htmlactualBody1.Replace("Exam_Notif_year", year);
            string htmlactualBody3 = htmlactualBody2.Replace("examtimeTable", examtimeTable);
            string FooterBody = @"</tbody>
</table>
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'>&nbsp;</span></p>
<table class='mce-item-table' style='border: 1px dashed rgb(187, 187, 187); color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'>
    <tbody>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='707'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong>(LUNCH BREAK FROM 01.00 PM TO 01.30 PM For the Practical exam only)</strong></span></p>
            </td>
        </tr>
    </tbody>
</table>
<br />
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'>&nbsp;</span></p>
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'>&nbsp;</span></p>
<table class='mce-item-table' style='border: 1px dashed rgb(187, 187, 187); color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'>
    <tbody>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='486'>
                <p><span style='font-size: 9px;'><strong>&nbsp;</strong></span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='250'>
                <p><span style='font-size: 9px;'><strong>&nbsp;</strong></span></p>
            </td>
        </tr>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='486'>
                <p><span style='font-size: 9px;'><strong>&nbsp;</strong></span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='250'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong>Commissioner</strong></span></p>
            </td>
        </tr>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='486'>
                <p><span style='font-size: 9px;'><strong>&nbsp;</strong></span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='250'>
                <p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'><strong>Industrial Training and Employment</strong></span></p>
            </td>
        </tr>
    </tbody>
</table>
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'>&nbsp;</span></p>
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'><strong>&nbsp;</strong></span></p>
<table class='mce-item-table' style='border: 1px dashed rgb(187, 187, 187); color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'>
    <tbody>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='58'>
                <p><span style='font-size: 9px;'>Copy to:-</span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='649'>
                <p><span style='font-size: 9px;'><strong>&nbsp;</strong></span></p>
            </td>
        </tr>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='58'>
                <p><span style='font-size: 9px;'>1)</span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='649'>
                <p><span style='font-size: 9px;'>&nbsp;The Joint Director (Trg). Divisional office Bangalore / Mysore / Hubli/ Kalburgi for information and circulate among all Govt. &amp; Pvt ITI&apos;s of your division at the earliest.</span></p>
            </td>
        </tr>
        <tr>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='58'>
                <p><span style='font-size: 9px;'>2)</span></p>
            </td>
            <td style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; border: 1px dashed rgb(187, 187, 187);' width='649'>
                <p><span style='font-size: 9px;'>&nbsp;The Principal, ITI............................................... with instruction to circulate among all the attached ITI.</span></p>
                <p><span style='font-size: 9px;'>&nbsp;</span></p>
            </td>
        </tr>
    </tbody>
</table>
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'><strong>&nbsp;</strong></span></p>
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'>&nbsp;</span></p>
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'>&nbsp;</span></p>
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'>&nbsp;</span></p>
<p style='color: rgb(0, 0, 0); font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-thickness: initial; text-decoration-style: initial; text-decoration-color: initial;'><span style='font-size: 9px;'>&nbsp;</span></p>";

            for (int i = 0; i < dtsubjects.Rows.Count; i++)
            {
               DateTime hours= Convert.ToDateTime(dtsubjects.Rows[i]["PTime"]);
                var ss = hours.ToString("hh:mm tt");
                //tr data to loop
                //DynamicSb.Append("<tr data-mce-style='height: 35px;' style='height: 35px;'><td td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='67'><p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'>" + dtsubjects.Rows[i]["ExamDate"].ToString() + "</span></p></td><td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='70'><p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'>" + dtsubjects.Rows[i]["ExamDay"].ToString() + "</span></p></td><td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='78'><p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'>" + dtsubjects.Rows[i]["ETime"].ToString() + "</span></p></td><td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='193'><p><span style='font-size: 9px;'>&nbsp;" + dtsubjects.Rows[i]["SubjectType"].ToString() + "</span></p></td><td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='304'><p><span style='font-size: 9px;'>&nbsp;" + dtsubjects.Rows[i]["Subject"].ToString() + "</span></p></td></tr>");
                DynamicSb.Append("<tr data-mce-style='height: 35px;' style='height: 35px;'><td td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='67'><p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'>" + dtsubjects.Rows[i]["ExamDate"].ToString() + "</span></p></td><td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='70'><p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'>" + dtsubjects.Rows[i]["ExamDay"].ToString() + "</span></p></td><td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='78'><p data-mce-style='text-align: center;' style='text-align: center;'><span style='font-size: 9px;'>" + ss + "</span></p></td><td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='193'><p><span style='font-size: 9px;'>&nbsp;" + dtsubjects.Rows[i]["SubjectType"].ToString() + "</span></p></td><td data-mce-style='height: 35px;' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; height: 35px;' width='304'><p><span style='font-size: 9px;'>&nbsp;" + dtsubjects.Rows[i]["Subject"].ToString() + "</span></p></td></tr>");
            }
            //headerSb.Append(htmlactualBody1);
            headerSb.Append(htmlactualBody3);
            footerSb.Append(FooterBody);

            iTextSharp.text.Document document = new iTextSharp.text.Document();
            iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(rootpath + "/" + NotificationId.ToString() + ".pdf", FileMode.Create));
            document.Open();

            FinalStringBuilder.Append(headerSb);
            FinalStringBuilder.Append(DynamicSb);
            FinalStringBuilder.Append(footerSb);
            DynamicSb.Clear();

            iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
            hw.Parse(new StringReader(FinalStringBuilder.ToString()));
            document.Close();

            //ExamCalendarMaster model = new ExamCalendarMaster();

            PdfdbPath = "~//Content//Notifications" + "//" + NotificationId.ToString() + ".pdf";

            model.user_id = Convert.ToInt32(Session["LoginId"]);
            model.PdfPath = PdfdbPath;
            model.NotificationId = NotificationId;
            model.FromReg = FromReg;

            var rea = _NotifBll.UpdateNotificationFileBLL(model);
            return Json(rea, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTradeListQP(int TradeTypeId)
        {
            var TradeList = _NotifBll.GetTradeListBasedOnIdBLL(TradeTypeId);
            return Json(TradeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTradeYearList(int TradeId)
        {
            var TradeYearList = _NotifBll.GetTradeYearListBasedOnIdBLL(TradeId);
            return Json(TradeYearList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCommentFileDetails(int NotificationId)
        {

            var Remarks = _NotifBll.GetCommentsFileBLL(NotificationId);
            return Json(Remarks, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult RejectNotification(Notification model)
        //{
        //    model.user_id = Convert.ToInt32(Session["LoginId"]);
        //    //model.role_id = Convert.ToInt32(Session["UserId"]);
        //    var res = _NotifBll.RejectNotificationBLL(model);
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetCommentDetails(int NotificationId)
        {
            Notification model = new Notification();
            //var Remarks = _NotifBll.GetCommentsListBLL(NotificationId);
            model.NotificationList = _NotifBll.GetCommentsListBLL(NotificationId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTradeTypeList(int CourseTypeId)
        {
            var TradeList = _NotifBll.GetTradeTypeListBasedOnIdBLL(CourseTypeId);
            return Json(TradeList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExamCenterMapping(ExamcenterMapping model)
        {
            model.Course_Type_List = _NotifBll.GetCourseListBLL();
            model.DivisionList = _NotifBll.GetDivisionListBLL();
            model.DistrictList = _NotifBll.GetDistrictBLL();
            model.CollegeLists = _NotifBll.GetCollegeBLL();
            return View(model);
        }

        public JsonResult GetDistrictbasedonDivisionID(string Div_ID_Res)
        {
            var Dist_list = _NotifBll.GetDistrictbasedonDivisionIDBLL(Convert.ToInt32(Div_ID_Res));
            return Json(Dist_list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCollegetNamebasedonDivisionID(string Dist_ID_Res)
        {
            var Col_list = _NotifBll.GetCollegetNamebasedonDivisionIDBLL(Convert.ToInt32(Dist_ID_Res));
            return Json(Col_list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCollegeCodebasedonCollegeID(string Col_Code_ID)
        {

            var Col_Code_Result = _NotifBll.GetCollegeCodebasedonCollegeIDBLL(Col_Code_ID);
            return Json(Col_Code_Result, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult SaveMappedData(string[] Check_values, Exam_Center ECModel)

        //{

        //    var Data_mapped = _NotifBll.SaveMappedBLL(Check_values, ECModel);
        //    return Json(Data_mapped, JsonRequestBehavior.AllowGet);
        //}

        public string GetExamDate(int? SubjectId)
        {
            var setExamdate = _NotifBll.GetExamdateBasedOnIdBLL(SubjectId);
            return setExamdate;



        }

        [HttpPost]
        public JsonResult SaveSendQP(QuestionPaperSets model)
        {
            var res = "";
            try
            {
                model.user_id = Convert.ToInt32(Session["LoginId"]);

                ////Use Namespace called :  System.IO  
                //string FileName = Path.GetFileNameWithoutExtension(model.qpst_file_path.FileName);

                ////To Get File Extension  
                //string FileExtension = Path.GetExtension(model.qpst_file_path.FileName);

                ////Add Current Date To Attached File Name  
                //FileName = DateTime.Now.ToString("yyyyMMdd HHMM") + "-" + FileName.Trim() + FileExtension;

                ////Get Upload path from Web.Config file AppSettings.  
                //string UploadPath = ConfigurationManager.AppSettings["QPDocumentsPath"].ToString();

                ////Its Create complete path to store in server.  
                //model.ImagePath = UploadPath + FileName;

                ////To copy and save file into server.  

                //model.qpst_file_path.SaveAs(model.ImagePath);

                
               // model.qpst_file_path = string.Format(UploadFolderQP ,model.ImagePath );


                res = _NotifBll.SaveSendQPBLL(model);

                //  return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }




        public JsonResult Reject(int Id, int Status, int Loginid, string comments, int StatusforCW)
        {
            Notification model = new Notification();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.Exam_Notif_Id = Id;
            model.exam_notif_status_id = Status;
            model.RoleId = Loginid;
            model.updatestatusCW = StatusforCW;
            model.comments = comments;
            string updateemployee = this._NotifBll.RejectBLL(model);
            string res = updateemployee.ToString();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRemainingSubjectList(string SubjectID, bool? IsEnable)
        {
            var SubjectList = _NotifBll.GetRemainingSubjectListBasedOnIdBLL(SubjectID, IsEnable);
            return Json(SubjectList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCommentTransStatus1(int Id, int Status, int Loginid, string comments)
        {

            Notification model = new Notification();
            model.login_id = Convert.ToInt32(Session["LoginId"]);
            model.Exam_Notif_Id = Id;
            model.exam_notif_status_id = Status;
            model.RoleId = Loginid;
            model.comments = comments;
            string updateemployee = this._NotifBll.UpdateCommentsTransStatus1BLL(model);
            //string res = updateemployee;
            return Json(updateemployee, JsonRequestBehavior.AllowGet);
        }


        //DSC Key Implemetation
        //step -1 DSC Key Registration
        public ActionResult RegisterDSCDetails()
        {
            DSCModel dSCModel = new DSCModel();

            //  OfficeLocation officeLocation = new OfficeLocation();
            //Convert.ToInt32(Session["UserId"])
            //int userID = 0;
            //if (Session["UserId"] != null)
            //{
            //    userID = Convert.ToInt32(Session["UserId"]);//int.Parse(Session["wdd_login_id"].ToString());

            //    //if (_approvalFlowBLL.GetUserRolesbll(userID, 7) == false)
            //    //{
            //    //    //send to access denied page.
            //    //    return RedirectToAction("AccessDenied", "Account");
            //    //}
            // //   officeLocation = _approvalFlowBLL.GetUserOfficeLocationbll(userID);
            //}
            //else
            //{
            //    //send back to login page
            //    return RedirectToAction("Login", "Account");
            //}

            return View(dSCModel);
        }

        [HttpPost]
        public ActionResult RegisterDSCDetails(DSCModel model)
        {
            //  OfficeLocation officeLocation = new OfficeLocation();
            int userID = 0;
            //if (Session["wdd_login_id"] != null)
            //{
            //    userID = int.Parse(Session["wdd_login_id"].ToString());
            userID = Convert.ToInt32(Session["UserId"]);
            //    if (_approvalFlowBLL.GetUserRolesbll(userID, 7) == false)
            //    {
            //        //send to access denied page.
            //        return RedirectToAction("AccessDenied", "Account");
            //    }
            //    officeLocation = _approvalFlowBLL.GetUserOfficeLocationbll(userID);
            //}
            //else
            //{
            //    //send back to login page
            //    return RedirectToAction("Login", "Account");
            //}
            if (ModelState.IsValid)
            {
                model.userID = userID;
                model.userID = Convert.ToInt32(Session["UserId"]); ;
                string status = _NotifBll.LinkDSCDetailsbll(model);

                if (status == "true")
                {
                    ViewBag.Status = "Success";
                }
            }
            return View(model);
        }

        //Step -2 Reviewing the Linked DSC key by Admin  -- DEO will approve the DSC

        public ActionResult ReviewDSCDetails()
        {
            // OfficeLocation officeLocation = new OfficeLocation();
            int userID = 0;
            if (Session["wdd_login_id"] != null)
            {
                userID = int.Parse(Session["UserId"].ToString());

                //if (_approvalFlowBLL.GetUserRolesbll(userID, 8) == false)
                //{
                //    //send to access denied page.
                //    return RedirectToAction("AccessDenied", "Account");
                //}
                //officeLocation = _approvalFlowBLL.GetUserOfficeLocationbll(userID);
            }
            else
            {
                //send back to login page
                //  return RedirectToAction("Login", "Account");
            }

            DSCModelList model = new DSCModelList();

            model = _NotifBll.GetOfficerDSCMappingsbll(userID);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReviewDSCDetails(DSCModelList model, string submit)
        {
            ViewBag.Status = "";
            //OfficeLocation officeLocation = new OfficeLocation();
            int userID = 0;
            if (Session["wdd_login_id"] != null)
            {
                userID = int.Parse(Session["UserId"].ToString());

                //if (_approvalFlowBLL.GetUserRolesbll(userID, 8) == false)
                //{
                //    //send to access denied page.
                //    return RedirectToAction("AccessDenied", "Account");
                //}
                //officeLocation = _approvalFlowBLL.GetUserOfficeLocationbll(userID);
            }
            else
            {
                //send back to login page
                // return RedirectToAction("Login", "Account");
            }

            //DSCModelList model = new DSCModelList();

            if (model.dSCModels.Where(x => x.ischecked == true && x.remarks == null || x.remarks == "").Count() > 0)
            {
                ModelState.AddModelError("Remarks", "Please fill remarks for checked items");
            }

            if (model.dSCModels.Where(x => x.ischecked == true).Count() == 0)
            {
                ModelState.AddModelError("Remarks", "Please check atleast one record");
            }

            if (ModelState.IsValid && model.dSCModels.Where(x => x.ischecked == true).Count() > 0)
            {
                foreach (DSCModel dSCModel in model.dSCModels.Where(x => x.ischecked == true))
                {
                    dSCModel.status = submit;
                }

                bool status = _NotifBll.UpdateDSCStatusbll(model);
                ViewBag.Status = status == true ? "Success" : "Failed";
            }

            model = _NotifBll.GetOfficerDSCMappingsbll(userID);

            return View(model);
        }

        // [HttpPost]
        //public JsonResult PDFSign(string certClient, SelCertAttribs SelCertAttribs)
        public JsonResult PDFSign(string certClient, SelCertAttribs SelCertAttribs, string notifIDDesc, int notifID, int Status, int Id, int Loginid, string comments, int StatusforCW)
        {
            string replaceSlash = notifIDDesc.Replace(@"/", "_");
            //Get the path for Signing the Approved PDF here Commissioner
            string pdfpath2 = @"//Content//Template//" + "Notification_" + replaceSlash;
            string withoutSlashpath = Server.MapPath(pdfpath2);
            string pdfpath1 = @"//Content//Template//" + "Notification_" + replaceSlash + ".pdf";
            //  string pdfPathfromDB = _NotifBll.GetOfficerDSCMappingsbll(userID);
            string pdfpath = Server.MapPath(pdfpath1);
            // string file2 = Server.MapPath("~/dam.txt");
            // string file3 = Server.MapPath("~/foldername/dam.txt");
            // string file4 = Server.MapPath("~/folder") + @"\dam.txt";
            X509CertificateParser cp = new X509CertificateParser();

            //Get Sertifiacte
            X509Certificate2 certClient1 = null;
            X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            st.Open(OpenFlags.MaxAllowed);
            //X509Certificate2Collection collection = X509Certificate2UI.SelectFromCollection(st.Certificates,
            //    "Please choose certificate:", "", X509SelectionFlag.SingleSelection);
            //if (collection.Count > 0)
            //{
            //    certClient1 = collection[0];
            //}
            st.Close();
            //Get Cert Chain
            IList<X509Certificate> chain1 = new List<X509Certificate>();
            X509Chain x509Chain1 = new X509Chain();

            x509Chain1.Build(certClient1);

            foreach (X509ChainElement x509ChainElement in x509Chain1.ChainElements)
            {
                //chain1.Add(DotNetUtilities.FromX509Certificate(x509ChainElement.Certificate));
            }


            //string filename = @"C:\\sample.pdf";
            //   string filename1 = @"C:\\sample.pdf";

            //PdfReader inputPdf = new PdfReader(filename);
            PdfReader inputPdf = new PdfReader(pdfpath);
            //  FileStream signedPdf = new FileStream(File_rename(filename, "_signed"), FileMode.Create);
            // FileStream signedPdf = new FileStream(@"D:\\BNM\Sample_signed.pdf", FileMode.Create);

            string concatSignedpath = withoutSlashpath + "_Signed" + ".pdf";
            string concatSignedpathSaveinDB = pdfpath2 + "_Signed" + ".pdf";
            //string pdfpathduplicate = pdfpath;

            //Below is for _Signed  File
            FileStream signedPdf = new FileStream(concatSignedpath, FileMode.Create);
            //FileStream signedPdf = new FileStream((concatSignedpath);//, FileMode.Create);
            // FileStream signedPdf = new FileStream(pdfpath, FileMode.OpenOrCreate);
            //FileStream signedPdf = new FileStream(pdfpath, FileMode.Open, FileAccess.Write);
            PdfStamper pdfStamper = PdfStamper.CreateSignature(inputPdf, signedPdf, '\0');

            IExternalSignature externalSignature = new X509Certificate2Signature(certClient1, "SHA-1");

            PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;

            //signatureAppearance.SignatureGraphic = Image.GetInstance(pathToSignatureImage);
            //signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(0, 00, 250, 150), inputPdf.NumberOfPages, "Signature");
            signatureAppearance.Reason = "Approved";
            signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(0, 00, 200, 100), inputPdf.NumberOfPages, "Signature");

            signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
            //Signature is stamping in the below line of code
           // MakeSignature.SignDetached(signatureAppearance, externalSignature, chain1, null, null, null, 0,
              //  CryptoStandard.CMS);
            inputPdf.Close();
            pdfStamper.Close();
            //---------------------------
            Notification notif = new Notification();

            // notif.Exam_Notif_Id = notifID;
            notif.getSignedPDFPath = concatSignedpathSaveinDB;
            //notif.exam_notif_status_id = Status;
            notif.login_id = Convert.ToInt32(Session["LoginId"]);
            notif.Exam_Notif_Id = Id;
            notif.exam_notif_status_id = Status;
            notif.RoleId = Loginid;
            notif.updatestatusCW = StatusforCW;
            notif.comments = comments;
            //notif.SavePath = concatSignedpathSaveinDB;
            //string updateemployee = this._NotifBll.UpdateCWStatusBLL(notif);
            string updateemployee = this._NotifBll.UpdateCWStatusBLL_WithDSC(notif);

            notif.res = updateemployee;
            // return View();
            // return Json(JsonRequestBehavior.AllowGet);
            return Json(notif, JsonRequestBehavior.AllowGet);
        }

        public string GetFileForSigning(RequestFile requestFile)
        {
            Image_convert_model _file_obj = new Image_convert_model();
            byte[] binFile = null;
            try
            {
                string RefID = "123";
                string filename = System.Web.Hosting.HostingEnvironment.MapPath("~/UploadedFiles/sample.pdf");
                string pdfFilePath = filename;
                byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);

                string strBytes = Convert.ToBase64String(bytes);

                _file_obj = new Image_convert_model
                {
                    File_Name = "Sample.pdf",
                    File_bytes = strBytes,
                    File_token = "",
                    RefID = RefID,
                    RefType = "1",
                    DSC_user_name = ""
                };

                return _file_obj.File_bytes;

                ////string filename = "~/UploadedFiles/sample.pdf";
                ////BinaryReader binReader = new BinaryReader(System.IO.File.Open(System.Web.Hosting.HostingEnvironment.MapPath(filename), FileMode.Open, FileAccess.Read));
                ////binReader.BaseStream.Position = 0;
                ////binFile = binReader.ReadBytes(Convert.ToInt32(binReader.BaseStream.Length));
                ////binReader.Close();

                //System.IO.FileStream _FileStream = new System.IO.FileStream(System.Web.Hosting.HostingEnvironment.MapPath(filename), System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //System.IO.BinaryReader _BinaryReader = new System.IO.BinaryReader(_FileStream);
                //long _TotalBytes = new System.IO.FileInfo(System.Web.Hosting.HostingEnvironment.MapPath(filename)).Length;
                //binFile = _BinaryReader.ReadBytes((Int32)_TotalBytes);
                //_FileStream.Close();
                //_FileStream.Dispose();
                //_BinaryReader.Close();


            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                int lineNo = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                //return Request.CreateResponse(HttpStatusCode.OK, _responce_model, Configuration.Formatters.JsonFormatter);
            }
            return "";
        }

        public string UploadSignedFile(Image_convert_model _Model)
        {
            File_Responce_model _responce_model = new File_Responce_model();
            try
            {
                _Model.File_Path = Server.MapPath("~/Content/SignedPDF/");

                if (_Model.File_Name != "" && _Model.File_bytes != "")
                {
                    string serverFileName = GenerateUniqueCode(5);
                    string filePathSigned = "/Content/SignedPDF/" + serverFileName + "_Signed.pdf";
                    byte[] imageBytes = Convert.FromBase64String(_Model.File_bytes);
                    string FileName = serverFileName + "_Signed.pdf";
                    string path = _Model.File_Path;
                    string imgPath = System.IO.Path.Combine(path, FileName);
                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }
                    System.IO.File.WriteAllBytes(imgPath, imageBytes);


                    _responce_model.Status = true;
                    _responce_model.Message = "success"; ;
                    _responce_model.return_reponce = "File Upload successfully.";
                }
                else
                {
                    _responce_model.Status = false;
                    _responce_model.Message = "failed"; ;
                    _responce_model.return_reponce = "File unable to upload.";
                }

                return "true";
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                int lineNo = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                return "false";
            }
        }

        public string GenerateUniqueCode(int num)
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            characters += alphabets + small_alphabets + numbers;
            string otp = string.Empty;
            for (int i = 0; i < num; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
        public JsonResult GetLastFeeDate(int ExamNotifyId)
        {
            var SubjectList = _NotifBll.GetLastFeeDateBLL(ExamNotifyId);
            return Json(SubjectList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PostNotificationData(Notification data)
        {

            
            int loginId = Convert.ToInt32(Session["LoginId"]);
            int roleId = Convert.ToInt32(Session["RoleId"].ToString());
            int userId = Convert.ToInt32(Session["UserId"]);
            string DocumentsFolder = GetUploadFolderPath();
            string filePathNotif = string.Empty;
            string DocdbPathNotif = string.Empty;
            data.login_id = loginId;
            data.user_id = userId;
            //var notifNumber = data.Exam_Notif_Number.Replace(@"/", "_");
            string filename = "";
            if (data.UploadPdf != null)
            {
                filename = Path.GetFileNameWithoutExtension(data.UploadPdf.FileName).Replace("/", "_");
               // string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat, filename);
               //// string wordpath = string.Format(DocumentsFolder + DocFileNameFormat, data.UploadPdf.FileName);
               // string wordpath = string.Format(DocumentsFolder + DocFileNameFormat, filename);

               
                if (!Directory.Exists(DocumentsFolder))
                {
                    Directory.CreateDirectory(DocumentsFolder);
                }
                // upload Notification file copy
                
         
                filename = filename + "-" + GetRoles(roleId, loginId);
                filename = filename.Replace("-", "_");
                string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat, filename);
                string wordpath = string.Format(DocumentsFolder + DocFileNameFormat, filename);

                if (data != null && !string.IsNullOrEmpty(data.UploadPdf.FileName) && (data.UploadPdf.ContentLength > 0))
                {
                    data.UploadPdf.SaveAs(wordpath);
                }

                
                Document document = new Document();
                document.LoadFromFile(wordpath);
                //string extension = System.IO.Path.GetExtension(pdfpath);
                //string filePathWithoutExt = pdfpath.Substring(0, pdfpath.Length - extension.Length);
                //Convert Word to PDF

                 document.SaveToFile(pdfpath, Spire.Doc.FileFormat.PDF);

                data.SavePath = string.Format(UploadFolder + PdfFileNameFormat, filename);
                
            }
            
            var saved = _NotifBll.CreateNotificationDetailsBLL(data);

            return Json(saved, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewNotificationFile(int id, HttpPostedFileBase file)
        {
            Notification model = new Notification();

           // _CommonBLL.CheckBrowserAndMenuUlBLL(Convert.ToString(Session["UserId"]), Convert.ToString(Session["MenuMappingId"]), Convert.ToString(Session["CurrentPage"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["sessionid"]), Request, Response, Session);
            //int roleId = Convert.ToInt32(Session["RoleId"].ToString());
            int loginID = Convert.ToInt32(Session["LoginId"].ToString());
            //var res = _NotifBll.GetUpdateNotificationBLL();
            model.NotificationList = _NotifBll.ViewNotificationFileBLL(loginID, id);

           // string fileName = model.Exam_Notif_Number.Replace(@"/", "_");
           foreach(var files in model.NotificationList)
            {
                string PdfFileName = Path.GetFileNameWithoutExtension(files.SavePath);
                string DocFileName = Path.GetFileNameWithoutExtension(files.DocSavePath);
                files.filename = PdfFileName;
                files.Docfilename = DocFileName;
            }
           
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveNotificationDocFile(FormCollection formCollection)
        {
            // Notification model = new Notification();
            int? res = 0;
            // get the files from view
            HttpFileCollectionBase files = Request.Files;
            List<Notification> models = new List<Notification>();

            //AtendanceDet models = new AtendanceDet();


            // get all the row data
            foreach (var key in formCollection.AllKeys)
            {
                //Deserialize JSON to class
                Notification model = JsonConvert.DeserializeObject<Notification>(formCollection[key]);
                //model.User_Id = Convert.ToInt32(Session["LoginId"]);
                for (int index = 0; index < files.Count; index++)
                {
                    // assign file to be uploaded
                    if (model.SavePath == files[index].FileName)
                    {
                        model.UploadDoc = files[index];
                    }
                    
                }
                models.Add(model);

                model.role_id = Convert.ToInt32(Session["UserRoleId"].ToString());
                model.login_id = Convert.ToInt32(Session["LoginId"].ToString());
                
                string filename =ConvertUploadedAdmsnNotifToPDFDLL(model, PdfFileNameFormat, DocFileNameFormat, Request.PhysicalApplicationPath + UploadFolder);
                model.DocSavePath = string.Format(UploadFolder + DocFileNameFormat, filename);
                model.SavePath = string.Format(UploadFolder + PdfFileNameFormat, filename);
                res = _NotifBll.UpdateNotificationDocFileDetailsBLL(models);
            }

            
           
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public string ConvertUploadedAdmsnNotifToPDFDLL(Notification model, string PdfFileNameFormat, string DocFileNameFormat, string DocumentsFolder)
        {
            if (!Directory.Exists(DocumentsFolder))
            {
                Directory.CreateDirectory(DocumentsFolder);
            }
            int? roleID = model.role_id;
            int? loginID = model.login_id;
            string filename = Path.GetFileNameWithoutExtension(model.UploadDoc.FileName).Replace("/", "_");
            filename = filename + "-" + GetRoles(roleID, loginID);
            filename = filename.Replace("-", "_");
            string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat, filename);
            string wordpath = string.Format(DocumentsFolder + DocFileNameFormat, filename);
            model.UploadDoc.SaveAs(wordpath);

            if (model.login_id == 2)
            {
                Document document = new Document();
                document.LoadFromFile(wordpath);

                //Convert Word to PDF
                document.SaveToFile(pdfpath, Spire.Doc.FileFormat.PDF);
            }
            return filename;
        }
        public string GetRoles(int? roleID,int? loginID)
        {
            var  roleName = _NotifBll.GetRollsBLL(roleID, loginID);
            string RoleDesc = "";
            foreach (var role in roleName)
            {
                RoleDesc = role.shrotName;
            }
            return RoleDesc;
        }

        public void CheckBrowserAndMenuUl()
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

    }
    public class SelCertAttribs
    {
        public string CertThumbPrint
        {
            get;
            set;
        }

        public string eMail
        {
            get;
            set;
        }

        public DateTime ExpDate
        {
            get;
            set;
        }

        public string PublicKey
        {
            get;
            set;
        }
        public string PrivateKey { get; set; }

        public string SelCertSubject
        {
            get;
            set;
        }

        public DateTime ValidFrom
        {
            get;
            set;
        }
        public object issuerName { get; internal set; }

        public SelCertAttribs()
        {
        }
    }

    public class Image_convert_model
    {
        public string File_Name { get; set; }
        public string File_bytes { get; set; }
        public string File_Path { get; set; }
        public string File_token { get; set; }
        public string RefID { get; set; }
        public string RefType { get; set; }
        public string DSC_user_name { get; set; }
    }

    public class RequestFile
    {
        public string RefID { get; set; }
        public string RefType { get; set; }
    }
    public class File_Responce_model
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string return_reponce { get; set; }
    }


}