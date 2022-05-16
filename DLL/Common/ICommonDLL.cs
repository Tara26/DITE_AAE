using Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DLL.Common
{
    public interface ICommonDLL
    {
        void CheckBrowserAndMenuUlDLL(string userId, string menuMappingId, string currPage, string userName, string sessionid, HttpRequestBase Request, HttpResponseBase Response, HttpSessionStateBase Session);
        
        string SendSMSDLL(string Mobileno, string msg);
        bool SendEmailDLL(string receiver, string subject, string msg);
        IEnumerable<tbl_notification_Publish_trans> Get_Notification_PublishDetails_Dll();

        int IsKGIDNumberExistDLL(int um_kgid_number);
        int IsMobileExistDLL(string Mobile);
        int IsEmailExistDLL(string Email);


    }
}
