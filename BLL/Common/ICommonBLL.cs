using Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL.Common
{
    public interface ICommonBLL
    {
        void CheckBrowserAndMenuUlBLL(string userId, string menuMappingId, string currPage, string userName, string sessionid, HttpRequestBase Request, HttpResponseBase Response, HttpSessionStateBase Session);
        string SendSMSBLL(string Mobileno, string msg);
        bool SendEmailBLL(string receiver, string subject, string msg);

        IEnumerable<tbl_notification_Publish_trans> Get_Notification_PublishDetails_Bll();

        int IsKGIDNumberExistBLL(int um_kgid_number);
        int IsMobileExistBLL(string Mobile);

        int IsEmailExistBLL(string Email);



    }
}
