using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models.Admission;

namespace BLL.Admission
{
    public interface IAdmissionNotificationBLL
    {
        string CreateAdmissionNotificationDetailsBLL(AdmissionNotification model);
        List<AdmissionNotification> GetUpdateNotificationBLL(int id,int? notificationId = null);
        List<AdmissionNotification> GetAdmissionNotificationBox();
        SelectList GetDepartmentListDLL();
    }
}
