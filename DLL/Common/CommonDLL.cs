using DLL.Admission;
using DLL.User;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Spire.Xls;
using System.Data;
using System.Security;
using System.Security.Permissions;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Net.Mail;
using Models.Master;
using DLL.DBConnection;
using System.Globalization;

namespace DLL.Common
{
    public class CommonDLL : ICommonDLL
    {
		private readonly IUserDLL _LoginServiceDLL;
		private readonly IAdmissionDLL _admissionDll;
        private readonly DbConnection _db = new DbConnection();
		public CommonDLL()
        {
			this._LoginServiceDLL = new UserDLL();
			this._admissionDll = new AdmissionDLL();
		}
        #region Security Audit
        public void CheckBrowserAndMenuUlDLL(string userId, string menuMappingId, string currPage, string userName, string sessionid, HttpRequestBase Request, HttpResponseBase Response, HttpSessionStateBase Session)
		{
			try
			{
				// check direct url
				if (userId == null)
				{
					Response.Redirect("~/Home/Logout", true);
				}
				#region check Menu url
				var response = _LoginServiceDLL.MenuList(Convert.ToInt32(menuMappingId));
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
				if (currPage == null)
				{
					currPage = "";//Request.RawUrl;
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
				var checkLogginstatus = _admissionDll.IsUserLoggedOnElsewhere(userName, sessionid);

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
        #endregion

        public string SendSMSDLL(string Mobileno, string message)
        {
            //string strSendSMSUrl = string.Empty;
            //string strOutput = string.Empty;
            //string smsPushURL1 = ConfigurationManager.AppSettings["smsPushURL1"].ToString();
            string username = ConfigurationManager.AppSettings["username"].ToString();
            string password = ConfigurationManager.AppSettings["password"].ToString();
            string senderid = ConfigurationManager.AppSettings["senderid"].ToString();
            string secureKey = ConfigurationManager.AppSettings["secureKey"].ToString();
            string templateid = ConfigurationManager.AppSettings["templateid"].ToString();
            string retAPIValue = "";

            retAPIValue = sendSingleSMS(username, password, senderid, Mobileno, message, secureKey, templateid);
            return retAPIValue;
        }

        public String sendSingleSMS(String username, String password, String senderid, String mobileNo, String message, String secureKey, String templateid)
        {
            Stream dataStream;
            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT");
            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 1;
            ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0;Windows 98; DigExt)";
            request.Method = "POST";
            System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();
            String encryptedPassword = encryptedPasswod(password);
            String NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
            String smsservicetype = "singlemsg";
            //For single message.
            String query = "username=" + HttpUtility.UrlEncode(username.Trim()) + "&password=" + HttpUtility.UrlEncode(encryptedPassword) + "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) + "&content=" + HttpUtility.UrlEncode(message.Trim()) + "&mobileno=" + HttpUtility.UrlEncode(mobileNo) + "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) + "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) + "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());
            byte[] byteArray = Encoding.ASCII.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            String Status = ((HttpWebResponse)response).StatusDescription;
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

        protected String encryptedPasswod(String password)
        {
            byte[] encPwd = Encoding.UTF8.GetBytes(password);
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
            byte[] pp = sha1.ComputeHash(encPwd);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in pp)
            {
                sb.Append(b.ToString("x2"));

            }
            return sb.ToString();
        }

        protected String hashGenerator(String Username, String sender_id, String message, String secure_key)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
            byte[] sec_key = sha1.ComputeHash(genkey);
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }

        public bool SendEmailDLL(string receiver, string subject, string message)
        {
            bool isSent = false;
            message = message.Replace("/n", "<br />");
            if (ConfigurationManager.AppSettings["Environment"] == "Development") {
                receiver = ConfigurationManager.AppSettings["toEmail"].ToString();
            }
            
            try
            {
                string fromEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
                MailMessage msg = new MailMessage();

                msg.From = new MailAddress(fromEmail, "ITI Admission Cell");
                msg.To.Add(receiver);
                msg.Subject = subject;
                msg.IsBodyHtml = true;
                msg.Body = message;
                msg.Priority = MailPriority.High;

                SmtpClient client = new SmtpClient();
                client.Host = ConfigurationManager.AppSettings["smtpServer"];
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
                client.UseDefaultCredentials = true;
                client.EnableSsl = false;
                client.Timeout = 100000;
                client.Credentials = new NetworkCredential(fromEmail, ConfigurationManager.AppSettings["smtpPass"]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(msg);
                isSent = true;
                return isSent;

            }
            catch (Exception ex)
            {
                // mobjErrorLog.Err_Handler(ex.Message, "Home", "SendEmail");
            }
            return isSent;
        }


        public void NotificationPublish(int ModelId)

        {
             var ModuleData = _db.tbl_notification_Publish_trans.Where(a => a.np_module_id == ModelId ).FirstOrDefault();
            if (ModuleData == null)
                ModuleData = new tbl_notification_Publish_trans();
            DateTime TodayDate =  DateTime.Now;
            //pubtrans.np_module_name = ModelName;
            //pubtrans.np_module_id = ModelId;
            //pubtrans.np_module_path = "";
            //pubtrans.np_is_active = true;

            ModuleData.np_creation_datetime = TodayDate.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            //_db.tbl_notification_Publish_trans.Add(pubtrans);
            _db.SaveChanges();
        }

        
             public IEnumerable<tbl_notification_Publish_trans> Get_Notification_PublishDetails_Dll()
       {
            string dateValue = _db.tbl_notification_Publish_trans.Max(p => p.np_creation_datetime);
            var MaxPublishDate = (from n in _db.tbl_notification_Publish_trans
                                  where n.np_is_active == true && n.np_creation_datetime == dateValue && n.np_creation_datetime != null

                                  select n).ToList();

            return MaxPublishDate;
        }

          public  int IsKGIDNumberExistDLL(int um_kgid_number)
         {
            int res;
            var quert_result = (from n in _db.tbl_user_master
                                where n.um_kgid_number == um_kgid_number
                                orderby n.um_id descending
                                select n).FirstOrDefault();
            if (quert_result != null)
            {
                res = 1;
            }
            else
            {
                res = 0;
            }
            return res;
        }


        public int IsMobileExistDLL(string Mobile)
        {
            int res;
            var quert_result = (from n in _db.tbl_user_master
                                where n.um_mobile_no == Mobile
                                orderby n.um_id descending
                                select n).FirstOrDefault();
            if (quert_result != null)
            {
                res = 1;
            }
            else
            {
                res = 0;
            }
            return res;
        }

        public int IsEmailExistDLL(string Email)
        {
            int res;
            var quert_result = (from n in _db.tbl_user_master
                                where n.um_email_id == Email
                                orderby n.um_id descending
                                select n).FirstOrDefault();
            if (quert_result != null)
            {
                res = 1;
            }
            else
            {
                res = 0;
            }
            return res;
        }

    }
    internal class MyPolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint srvPoint, System.Security.Cryptography.X509Certificates.X509Certificate certificate, WebRequest request, int certificateProblem)
        {
            return true;
        }
    }

    public static class FileUtility
    {
        public static DataTable ConvertXSLXtoDataTable(string strFilePath)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(strFilePath);
            Worksheet sheet = workbook.Worksheets[0];
            DataTable dt = sheet.ExportDataTable();
            dt = dt.AsEnumerable().Where(row => !row.ItemArray.All(f => f is null || String.IsNullOrWhiteSpace(f.ToString()))).CopyToDataTable();
            return dt;
        }
        #region " Validations for File Types"

        public enum ImageFileExtension
        {
            none = 0,
            jpg = 1,
            jpeg = 2,
            bmp = 3,
            gif = 4,
            png = 5
        }

        private enum VideoFileExtension
        {
            none = 0,
            wmv = 1,
            mpg = 2,
            mpeg = 3,
            mp4 = 4,
            avi = 5,
            flv = 6
        }

        private enum PDFFileExtension
        {
            none = 0,
            PDF = 1
        }

        public enum FileType
        {
            Image = 1,
            Video = 2,
            PDF = 3,
            Text = 4,
            DOC = 5,
            DOCX = 6,
            PPT = 7,
        }

        public static bool XLMimeType(string MimeType, string ext)
        {
            bool isValid = false;

            if (MimeType == "application/x-msexcel" && (ext == ".xlsx" || ext == ".xls"))
            {
                isValid = true;
            }
            else if (MimeType == "application/x-excel" && (ext == ".xlsx" || ext == ".xls"))
            {
                isValid = true;
            }
            else if (MimeType == "application/vnd.ms-excel" && (ext == ".xlsx" || ext == ".xls"))
            {
                isValid = true;
            }
            else if (MimeType == "application/excel" && (ext == ".xlsx" || ext == ".xls"))
            {
                isValid = true;
            }

            return isValid;
        }

        public static void isValidFile(byte[] bytFile, FileType flType, String FileContentType, out string imageFileExtension1, out bool isvalid)
        {
            bool isvalid1 = false;
            string imageFileExtension = "";
            if (flType == FileType.Image)
            {
                isValidImageFile(bytFile, FileContentType, out imageFileExtension, out isvalid1);
            }
            else { }
            //else if (flType == FileType.Video)
            //{
            //    isvalid = isValidVideoFile(bytFile, FileContentType, imageFileExtension);
            //}
            //else if (flType == FileType.PDF)
            //{
            //    isvalid = isValidPDFFile(bytFile, FileContentType, imageFileExtension);
            //}

            //  return isvalid;

            imageFileExtension1 = imageFileExtension;
            isvalid = isvalid1;
        }

        public static void isValidImageFile(byte[] bytFile, String FileContentType, out string imageFileExtension, out bool isvalid1)
        {
            bool isvalid = false;
            string filetype = "";
            byte[] chkBytejpg = { 255, 216, 255, 224 };
            byte[] chkBytebmp = { 66, 77 };
            byte[] chkBytegif = { 71, 73, 70, 56 };
            byte[] chkBytepng = { 137, 80, 78, 71 };


            ImageFileExtension imgfileExtn = ImageFileExtension.none;

            if (FileContentType.Contains("jpg") | FileContentType.Contains("jpeg"))
            {
                imgfileExtn = ImageFileExtension.jpg;
            }
            else if (FileContentType.Contains("png"))
            {
                imgfileExtn = ImageFileExtension.png;
            }
            else if (FileContentType.Contains("bmp"))
            {
                imgfileExtn = ImageFileExtension.bmp;
            }
            else if (FileContentType.Contains("gif"))
            {
                imgfileExtn = ImageFileExtension.gif;
            }

            if (imgfileExtn == ImageFileExtension.jpg || imgfileExtn == ImageFileExtension.jpeg)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkBytejpg[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }


            if (imgfileExtn == ImageFileExtension.png)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkBytepng[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }


            if (imgfileExtn == ImageFileExtension.bmp)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 1; i++)
                    {
                        if (bytFile[i] == chkBytebmp[i])
                        {
                            j = j + 1;
                            if (j == 2)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }

            if (imgfileExtn == ImageFileExtension.gif)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 1; i++)
                    {
                        if (bytFile[i] == chkBytegif[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }

            filetype = Convert.ToString(imgfileExtn);
            imageFileExtension = filetype;
            isvalid1 = isvalid;
            //return isvalid;
        }

        public static string ImgExtension(byte[] bytFile, String FileContentType)
        {

            byte[] chkBytejpg = { 255, 216, 255, 224 };
            byte[] chkBytebmp = { 66, 77 };
            byte[] chkBytegif = { 71, 73, 70, 56 };
            byte[] chkBytepng = { 137, 80, 78, 71 };


            ImageFileExtension imgfileExtn = ImageFileExtension.none;

            if (FileContentType.Contains("jpg") | FileContentType.Contains("jpeg"))
            {
                imgfileExtn = ImageFileExtension.jpg;
            }
            else if (FileContentType.Contains("png"))
            {
                imgfileExtn = ImageFileExtension.png;
            }
            else if (FileContentType.Contains("bmp"))
            {
                imgfileExtn = ImageFileExtension.bmp;
            }
            else if (FileContentType.Contains("gif"))
            {
                imgfileExtn = ImageFileExtension.gif;
            }


            return imgfileExtn.ToString();
        }

        private static bool isValidVideoFile(byte[] bytFile, String FileContentType)
        {
            byte[] chkBytewmv = { 48, 38, 178, 117 };
            byte[] chkByteavi = { 82, 73, 70, 70 };
            byte[] chkByteflv = { 70, 76, 86, 1 };
            byte[] chkBytempg = { 0, 0, 1, 186 };
            byte[] chkBytemp4 = { 0, 0, 0, 20 };
            bool isvalid = false;

            VideoFileExtension vdofileExtn = VideoFileExtension.none;
            if (FileContentType.Contains("wmv"))
            {
                vdofileExtn = VideoFileExtension.wmv;
            }
            else if (FileContentType.Contains("mpg") || FileContentType.Contains("mpeg"))
            {
                vdofileExtn = VideoFileExtension.mpg;
            }
            else if (FileContentType.Contains("mp4"))
            {
                vdofileExtn = VideoFileExtension.mp4;
            }
            else if (FileContentType.Contains("avi"))
            {
                vdofileExtn = VideoFileExtension.avi;
            }
            else if (FileContentType.Contains("flv"))
            {
                vdofileExtn = VideoFileExtension.flv;
            }

            if (vdofileExtn == VideoFileExtension.wmv)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkBytewmv[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }
            else if ((vdofileExtn == VideoFileExtension.mpg || vdofileExtn == VideoFileExtension.mpeg) & isvalid)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkBytempg[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }
            else if (vdofileExtn == VideoFileExtension.mp4 & isvalid)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkBytemp4[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }
            else if (vdofileExtn == VideoFileExtension.avi & isvalid)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkByteavi[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }
            else if (vdofileExtn == VideoFileExtension.flv & isvalid)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkByteflv[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }

            return isvalid;

        }

        public static bool isValidPDFFile(byte[] bytFile, String FileContentType)
        {
            byte[] chkBytepdf = { 37, 80, 68, 70 };
            bool isvalid = false;

            PDFFileExtension pdffileExtn = PDFFileExtension.none;
            if (FileContentType.Contains("pdf"))
            {
                pdffileExtn = PDFFileExtension.PDF;
            }

            if (pdffileExtn == PDFFileExtension.PDF)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkBytepdf[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }

            return isvalid;
        }

        public static bool isValidDimension(byte[] bytFile, int maxRequiredWidth, int maxRequiredHeight)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(bytFile), true, true))
            {
                if (image.Width == maxRequiredWidth && image.Height == maxRequiredHeight)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        #endregion
        public static void ShowChars(char[] charArray)
        {
            Console.WriteLine("Char\tHex Value");
            // Display each invalid character to the console.
            foreach (char someChar in charArray)
            {
                if (Char.IsWhiteSpace(someChar))
                {
                    Console.WriteLine(",\t{0:X4}", (int)someChar);
                }
                else
                {
                    Console.WriteLine("{0:c},\t{1:X4}", someChar, (int)someChar);
                }
            }
        }
        public static bool GrantFilePermission()
        {
            bool IsGranted = false;
            FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.Read, "C:\\Users\\rubam\\source\\UploadFile_r");
            f2.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, "C:\\Users\\rubam\\source\\SMSAPI.txt");
            try
            {
                f2.Demand();
                IsGranted = true;
            }
            catch (SecurityException s)
            {
                IsGranted = false;
                //Console.WriteLine(s.Message);
            }
            return IsGranted;
        }

        public class ReadJsonFile
        {
            public List<T> ReadJson<T>(string path)
            {
                string json = "";
                string appendPath = "";
                List<T> _data = new List<T>();
                try
                {
                    appendPath = HttpContext.Current.Request.PhysicalApplicationPath + path;
                    json = System.IO.File.ReadAllText(appendPath);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    _data = ser.Deserialize<List<T>>(json);
                }
                catch (Exception ex)
                {
                    //ExceptionLogging.SendErrorToText(ex);
                    //throw;

                }

                return _data;
            }
        }

      
    }
}
