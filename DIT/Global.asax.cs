using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Models.Logs;
using static DLL.Common.FileUtility;

namespace DIT
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            // GlobalFilters.Filters.Add(new ValidateCustomAntiForgeryTokenAttribute());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
            PreSendRequestHeaders += Application_PreSendRequestHeaders;
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("X-Frame option", "deny");
            Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate,pre-check=0,post-check=0,s-maxage=0");
            Response.AddHeader("X-XSS-Protection", "1; mode=block");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            // Response.ClearHeaders();

            Response.Cache.AppendCacheExtension("no-cache, no-store, max-age=0, must-revalidate,pre-check=0,post-check=0,s-maxage=0");
            Response.AppendHeader("Pragma", "no-cache");
            Response.AppendHeader("Expires", "0");
        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());
        }
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Headers.Remove("Server");
            //HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            //HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            //HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            //            
        }
        void Session_Start(object sender, EventArgs e)
        {
        }
        protected void Session_Start()
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            string date = DateTime.Today.Date.ToShortDateString();
            string dir = "~/Content/Error/" + date;
            string path = System.Web.HttpContext.Current.Server.MapPath(dir + "/Error.log");
            string directory = System.Web.HttpContext.Current.Server.MapPath(dir);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string pathjson = System.Web.HttpContext.Current.Server.MapPath(dir + "/Error.json");
            string urlstring = System.Web.HttpContext.Current.Request.RawUrl;
            string url = System.Web.HttpContext.Current.Request.Url.Authority;
            string host = System.Web.HttpContext.Current.Request.Url.Scheme;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToLocalTime().ToString("F"));
            sb.AppendLine("Request: " + System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
            sb.AppendLine("Source File: " + System.Web.HttpContext.Current.Request.RawUrl);
            sb.AppendLine("Inner Exception: " + System.Web.HttpContext.Current.Server.GetLastError().InnerException);
            sb.AppendLine("Message: " + System.Web.HttpContext.Current.Server.GetLastError().Message);
            sb.AppendLine("Exception: " + System.Web.HttpContext.Current.Server.GetLastError());
            sb.AppendLine("------------------------------------------------------------" + Environment.NewLine);
            File.AppendAllText(path, sb.ToString());
            bool fileCheck = File.Exists(pathjson);
            if (fileCheck == false)
            {
                List<LogFile> _loglist = new List<LogFile>();
                _loglist.Add(new LogFile
                {
                    _date = DateTime.Now.ToLocalTime().ToString("F"),
                    _request = System.Web.HttpContext.Current.Request.Url.AbsoluteUri,
                    _sourcefile = System.Web.HttpContext.Current.Request.RawUrl,
                    _message = System.Web.HttpContext.Current.Server.GetLastError().Message,
                    _exception = System.Web.HttpContext.Current.Server.GetLastError().ToString(),
                    _stacktrace = System.Web.HttpContext.Current.Server.GetLastError().StackTrace
                });
                string jsondata = new JavaScriptSerializer().Serialize(_loglist);
                File.WriteAllText(pathjson, jsondata);
            }
            else
            {
                string jsonPath = dir + "\\Error.json";
                ReadJsonFile _rjf = new ReadJsonFile();
                List<LogFile> log = _rjf.ReadJson<LogFile>(jsonPath);
                log.Add(new LogFile
                {
                    _date = DateTime.Now.ToLocalTime().ToString("F"),
                    _request = HttpContext.Current.Request.Url.AbsoluteUri,
                    _sourcefile = HttpContext.Current.Request.RawUrl,
                    _message = HttpContext.Current.Server.GetLastError().Message,
                    _exception = HttpContext.Current.Server.GetLastError().ToString(),
                    _stacktrace = HttpContext.Current.Server.GetLastError().StackTrace
                });
                string jsondata = new JavaScriptSerializer().Serialize(log);
                File.WriteAllText(pathjson, jsondata);
            }
            var httpContext = ((HttpApplication)sender).Context;
            httpContext.Response.Clear();
            httpContext.ClearError();
        }
    }
}
