using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DLL.Common;
using Models.Master;

namespace BLL.Common
{
    public class CommonBLL : ICommonBLL
    {
        private readonly ICommonDLL _CommonDll;

        public CommonBLL()
        {
            this._CommonDll = new CommonDLL();
        }
        public void CheckBrowserAndMenuUlBLL(string userId, string menuMappingId, string currPage, string userName, string sessionid, HttpRequestBase Request, HttpResponseBase Response, HttpSessionStateBase Session)
        {
            _CommonDll.CheckBrowserAndMenuUlDLL(userId, menuMappingId, currPage, userName, sessionid, Request, Response, Session);
        }
        public string SendSMSBLL(string Mobileno, string msg)
        {
            return _CommonDll.SendSMSDLL(Mobileno, msg);
        }
        public bool SendEmailBLL(string receiver, string subject, string msg)
        {
            return _CommonDll.SendEmailDLL(receiver, subject, msg);
        }
        public IEnumerable<tbl_notification_Publish_trans>Get_Notification_PublishDetails_Bll()
        {
            var _subcategory_1_Data = _CommonDll.Get_Notification_PublishDetails_Dll();
            return _subcategory_1_Data;
        }

        public int IsKGIDNumberExistBLL(int um_kgid_number)
        {
            try
            {
                var _data = _CommonDll.IsKGIDNumberExistDLL(um_kgid_number);
                return _data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int IsMobileExistBLL(string Mobile)
        {
            try
            {
                var _data = _CommonDll.IsMobileExistDLL(Mobile);
                return _data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int IsEmailExistBLL(string Email)

        {
            try
            {
                var _data = _CommonDll.IsEmailExistDLL(Email);
                return _data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
