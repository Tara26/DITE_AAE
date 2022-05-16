using Models.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DLL.Admission.Admission_Notification
{
   public interface IAdmissionNotificationDLL
    {
        string CreateAdmissionNotificationDetailsDLL(AdmissionNotification model);
        List<AdmissionNotification> GetUpdateNotificationDLL(int id,int? notificationId = null);
        List<AdmissionNotification> GetAdmissionNotificationBox();
        SelectList GetDepartmentListDLL();
    }
}
