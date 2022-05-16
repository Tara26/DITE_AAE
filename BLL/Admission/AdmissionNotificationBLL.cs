using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DLL.Admission.Admission_Notification;
using Models.Admission;

namespace BLL.Admission
{
    public class AdmissionNotificationBLL : IAdmissionNotificationBLL
    {
        private readonly IAdmissionNotificationDLL _adDll;
        public AdmissionNotificationBLL()
        {
            this._adDll = new AdmissionNotificationDLL();
        }

        public string CreateAdmissionNotificationDetailsBLL(AdmissionNotification model)
        {
            return _adDll.CreateAdmissionNotificationDetailsDLL(model);
        }
        public List<AdmissionNotification> GetUpdateNotificationBLL(int id,int? notificationId = null)
        {
            return _adDll.GetUpdateNotificationDLL(id, notificationId);
        }
        public List<AdmissionNotification> GetAdmissionNotificationBox()
        {
            return _adDll.GetAdmissionNotificationBox();
        }
        public SelectList GetDepartmentListDLL()
        {
            return _adDll.GetDepartmentListDLL();
        }
    }
}
