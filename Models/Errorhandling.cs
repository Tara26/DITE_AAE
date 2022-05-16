using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Models
{
  public class Errorhandling
  {
    private string lstrFileBulk;
    private StreamWriter objFile;



    public Errorhandling()
    {
      lstrFileBulk = ConfigurationManager.AppSettings["LogPath"].ToString();
   
    }
    public bool DbLog_Handler(string lstrPageName, string lstrFunctionName, string lstrDbMessage, string SP)
    {
      string lStrDay = DateTime.Now.ToString("dd");
      string lStrMonth = DateTime.Now.ToString("MM");
      string lStrYear = DateTime.Now.ToString("yyyy");
      string lStrDate = "DLL_" + lStrDay + "-" + lStrMonth + "-" + lStrYear + DateTime.Now.Hour + ".ael";
      string lstrFolderName = null;
      string pkeyIP = null;
      string pkeyvalPageName = null;
      string pkeyvalFunctionName = null;
      string LoginSessionID = null;
      string UserId = null;

      try
      {
        pkeyvalPageName = DateTime.Now + lstrPageName;
        pkeyvalFunctionName = lstrFunctionName;
        //lstrDbMessage = lstrDbMessage ;
        pkeyIP = DateTime.Now.ToString() + " & " + HttpContext.Current.Request.UserHostAddress.ToString();

        if (System.Web.HttpContext.Current.Session["newId"] != null)
        {
          LoginSessionID = System.Web.HttpContext.Current.Session["newId"].ToString();
        }
        else if (System.Web.HttpContext.Current.Session["sessionid"] != null)
        {
          LoginSessionID = System.Web.HttpContext.Current.Session["sessionid"].ToString();
        }
        if (System.Web.HttpContext.Current.Session["UserId"] != null)
        {
          UserId = System.Web.HttpContext.Current.Session["UserId"].ToString();
        }

        lstrFolderName = lstrFileBulk + lStrDay + "-" + lStrMonth + "-" + lStrYear + "\\";
        //"E:/ERRORLOG/BulkSMS/"
        FileStream fs = default(FileStream);
        FileInfo keyFile = new FileInfo(lstrFolderName + lStrDate);
        DirectoryInfo Dinfo = new DirectoryInfo(lstrFolderName);
        if (Dinfo.Exists == false)
        {
          Dinfo.Create();
        }
        //If key already Present delete it and recreate it 
        if (!keyFile.Exists)
        {
          fs = new FileStream(lstrFolderName + lStrDate, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        }
        else
        {
          fs = new FileStream(lstrFolderName + lStrDate, FileMode.Append, FileAccess.Write, FileShare.None);
        }
        StreamWriter Twriter = new StreamWriter(fs, System.Text.Encoding.Default);
        Twriter.WriteLine();
        Twriter.WriteLine("************************************************************");
        Twriter.WriteLine(" User ID       :" + UserId);
        Twriter.WriteLine(" SessionID     :" + LoginSessionID);
        Twriter.WriteLine(" SP            :" + SP);
        Twriter.WriteLine(" Values        :" + lstrDbMessage);
        Twriter.WriteLine(" Page          :" + lstrPageName);
        Twriter.WriteLine(" Date & IP     :" + pkeyIP);
        Twriter.WriteLine(" Method        :" + pkeyvalFunctionName);
        Twriter.WriteLine("************************************************************");
        Twriter.WriteLine();
        Twriter.Flush();
        Twriter.Close();
        Twriter = null;
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }
   
    public bool Err_Handler(string lstrMessage, string lstrPageName, string lstrFunctionName)
    {
      // mobjErrorLog.Err_Handler(lstrMessage, lstrPageName, lstrFunctionName)
      string lStrDay = DateTime.Now.ToString("dd");
      string lStrMonth = DateTime.Now.ToString("MM");
      string lStrYear = DateTime.Now.ToString("yyyy");
      string lStrDate = lStrDay + "-" + lStrMonth + "-" + lStrYear + DateTime.Now.Hour + ".ael";
      string lstrFolderName = null;
      string pkeyIP = null;
      string pkeyval = null;
      string pkeyvalPageName = null;
      string pkeyvalFunctionName = null;
      try
      {
        pkeyval = DateTime.Now + lstrMessage.ToString();
        pkeyvalPageName = lstrPageName;
        pkeyvalFunctionName = lstrFunctionName;
        lstrFolderName = lstrFileBulk + lStrDay + "-" + lStrMonth + "-" + lStrYear + "\\";
        FileStream fs = default(FileStream);
        FileInfo keyFile = new FileInfo(lstrFolderName + lStrDate);
        DirectoryInfo Dinfo = new DirectoryInfo(lstrFolderName);

        if (Dinfo.Exists == false)
        {
          Dinfo.Create();
        }
        pkeyIP = DateTime.Now.ToString() + " & " + HttpContext.Current.Request.UserHostAddress.ToString();
        //If key already Present delete it and recreate it 
        if (!keyFile.Exists)
        {
          fs = new FileStream(lstrFolderName + lStrDate, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        }
        else
        {
          fs = new FileStream(lstrFolderName + lStrDate, FileMode.Append, FileAccess.Write, FileShare.None);
        }
        StreamWriter Twriter = new StreamWriter(fs, System.Text.Encoding.Default);
        Twriter.WriteLine();
        Twriter.WriteLine("************************************************************");
        Twriter.WriteLine(pkeyval);
        Twriter.WriteLine(pkeyvalPageName);
        Twriter.WriteLine(pkeyIP);
        Twriter.WriteLine(pkeyvalFunctionName);
        Twriter.WriteLine("************************************************************");
        Twriter.WriteLine();
        Twriter.Flush();
        Twriter.Close();
        Twriter = null;
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }
  
    public bool LogWrite(string lstrLog)
    {
      string lStrDay = DateTime.Now.ToString("dd");
      string lStrMonth = DateTime.Now.ToString("MM");
      string lStrYear = DateTime.Now.ToString("yyyy");
      string lstrfilename = System.Configuration.ConfigurationManager.AppSettings["LOGimps"].ToString() + lStrDay + "-" + lStrMonth + "-" + lStrYear + "\\";
      try
      {
        string lstrFile = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Hour + ".tic";




        objFile = File.AppendText(lstrfilename + lstrFile);
        objFile.WriteLine("Date Stamp : " + DateTime.Now.Day + "-" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
        objFile.WriteLine("Log Message : " + lstrLog);
        objFile.WriteLine("_________________________________________________");
        objFile.Close();
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }

    }
    public void DisplayMessage(string lstrText, Page pageName)
    {
      try
      {
        string lstrScript = null;
        lstrScript = "alert('" + lstrText.ToString() + "');";
        ScriptManager.RegisterStartupScript(pageName, pageName.GetType(), "Message Information", lstrScript, true);
      }
      catch (Exception ex)
      {
        Err_Handler(ex.Message, "ErrorLog", "DisplayMessage");
      }
    }
  }
}