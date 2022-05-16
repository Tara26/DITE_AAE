using Models;
using Models.User;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using System.Web.SessionState;

namespace DIT.Utilities
{
	public class Security
  {
   private static readonly Errorhandling mobjErrorLog = new Errorhandling();
		public static string GetAntiXsrfToken()
		{
			string cookieToken, formToken;
			AntiForgery.GetTokens(null, out cookieToken, out formToken);
			var responseCookie = new HttpCookie("__AJAXAntiXsrfToken")
			{
				HttpOnly = true,
				Value = cookieToken
			};
			if (FormsAuthentication.RequireSSL && HttpContext.Current.Request.IsSecureConnection)
			{
				responseCookie.Secure = true;
			}
			HttpContext.Current.Response.Cookies.Set(responseCookie);

			return formToken;
		}
		public static void ValidateRequestHeader(HttpRequestBase request)
    {
      string cookieToken = "";
      string formToken = "";

      if (request.Headers["RequestVerificationToken"] != null)
      {
        string[] tokens = request.Headers["RequestVerificationToken"].Split(':');
        if (tokens.Length == 2)
        {
          cookieToken = tokens[0].Trim();
          formToken = tokens[1].Trim();
        }
      }
      AntiForgery.Validate(cookieToken, formToken);
		}
	
		public static void CrossBrowser()
		{
      #region CrossBrowser
      // var idurl = Request.RawUrl;
      if (HttpContext.Current.Session["CurrentPage"] == null)
      {
        HttpContext.Current.Session["CurrentPage"] =HttpContext.Current.Request.RawUrl;
      }
      else
      {
				if (HttpContext.Current.Session.IsNewSession && HttpContext.Current.Request.UrlReferrer == null)
				{
          mobjErrorLog.Err_Handler("UrlReferrer", "NULL", HttpContext.Current.Session["CurrentPage"].ToString());
          HttpContext.Current.Response.Redirect("~/Home/Logout", true);
				}
				
      }
      #endregion

      
    }
		//public static bool IsYourLoginStillTrue(string userId, string sid)
		//{
  //    DbConnection context = new DbConnection();

		//	IEnumerable<UserLogin> logins = (from i in context.Logins
		//																where i.LoggedIn == true &&
		//																i.UserId == userId && i.SessionId == sid
		//																select i).AsEnumerable();
		//	return logins.Any();
		//}

		//public static bool IsUserLoggedOnElsewhere(string userId, string sid)
		//{
		//	CapWorxQuikCapContext context = new CapWorxQuikCapContext();

		//	IEnumerable<Logins> logins = (from i in context.Logins
		//																where i.LoggedIn == true &&
		//																i.UserId == userId && i.SessionId != sid
		//																select i).AsEnumerable();
		//	return logins.Any();
		//}

		//public static void LogEveryoneElseOut(string userId, string sid)
		//{
		//	CapWorxQuikCapContext context = new CapWorxQuikCapContext();

		//	IEnumerable<Logins> logins = (from i in context.Logins
		//																where i.LoggedIn == true &&
		//																i.UserId == userId &&
		//																i.SessionId != sid // need to filter by user ID
		//																select i).AsEnumerable();

		//	foreach (Logins item in logins)
		//	{
		//		item.LoggedIn = false;
		//	}

		//	context.SaveChanges();
		//}
		public static void regenerateId()
    {
      //Dim oHTTPContext As HttpContext = HttpContext.Current
      System.Web.SessionState.SessionIDManager manager = new System.Web.SessionState.SessionIDManager();
      HttpContext Context = System.Web.HttpContext.Current;
      string oldId = manager.GetSessionID(Context);
      string newId = manager.CreateSessionID(Context);
      bool isAdd = false, isRedir = false;
      manager.SaveSessionID(Context, newId, out isRedir, out isAdd);
      HttpApplication ctx = (HttpApplication)System.Web.HttpContext.Current.ApplicationInstance;
      HttpModuleCollection mods = ctx.Modules;
      System.Web.SessionState.SessionStateModule ssm = (SessionStateModule)mods.Get("Session");
      System.Reflection.FieldInfo[] fields = ssm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
      SessionStateStoreProviderBase store = null;
      System.Reflection.FieldInfo rqIdField = null, rqLockIdField = null, rqStateNotFoundField = null;
      foreach (System.Reflection.FieldInfo field in fields)
      {
        if (field.Name.Equals("_store")) store = (SessionStateStoreProviderBase)field.GetValue(ssm);
        if (field.Name.Equals("_rqId")) rqIdField = field;
        if (field.Name.Equals("_rqLockId")) rqLockIdField = field;
        if (field.Name.Equals("_rqSessionStateNotFound")) rqStateNotFoundField = field;
      }
      object lockId = rqLockIdField.GetValue(ssm);
      if ((lockId != null) && (oldId != null)) store.ReleaseItemExclusive(Context, oldId, lockId);
      rqStateNotFoundField.SetValue(ssm, true);
      rqIdField.SetValue(ssm, newId);
      // ViewState["dd"] = newId;
    }
  }
}