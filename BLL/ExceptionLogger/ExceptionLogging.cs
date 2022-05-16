using System;
using System.IO;
using System.Web;
using context = System.Web.HttpContext;
namespace BLL.ExceptionLogger
{
    public static class ExceptionLogging
    {
        public static void SendErrorToText(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;
            String errorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            String Eerrormsg = ex.GetType().Name.ToString();
            String Extype = ex.GetType().ToString();
            String Exurl = context.Current.Request.Url.ToString();
            String errorLocation = ex.ToString();
            try
            {
                string filepath = context.Current.Server.MapPath("~/ExceptionDetailsFile/");  //Text File Path
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + errorlineNo + line + "Error Message:" + " " + Eerrormsg + line + "Exception Type:" + " " + Extype + line + "Error Location :" + " " + errorLocation + line + " Error Page Url:" + " " + Exurl + line + "User Host IP:" + " " + line;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
    public class LoggerModel
    {
        public void ErrorHandler(string ErrorMsg, int LineNo)
        {
            var relativePath = "LogFile.txt";
            string absolutePath = Path.Combine(HttpRuntime.AppDomainAppPath, relativePath);
            string Text = string.Empty;
            if (!System.IO.File.Exists(absolutePath))
            {
                using (FileStream fs = System.IO.File.Create(absolutePath))
                {

                    Text = "Errors: " + Environment.NewLine + Environment.NewLine;
                    System.IO.File.AppendAllText(absolutePath, Text);
                }
            }
            Text = Environment.NewLine + "Error : " + Environment.NewLine + "Error Message : " + ErrorMsg + Environment.NewLine + "Line No : " + LineNo + Environment.NewLine + "Date and Time : " + DateTime.Now + Environment.NewLine + "Page Title : dashboard" + "----------------------------------------------------------------------------------------------";

            System.IO.File.AppendAllText(absolutePath, Text);

        }
    }

}

