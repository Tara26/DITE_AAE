using Models.Admission;
using Models.ExamNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DLL.Admission.AdmissionNotifications
{
    public interface IAdmissionNotifications
    {
        string CreateAdmissionNotificationDetailsDLL(AdmissionNotification model);
        string ConvertUploadedAdmsnNotifToPDFDLL(AdmissionNotification model, string PdfFileNameFormat, string DocFileNameFormat, string DocumentsFolder);
        List<AdmissionNotification> GetUpdateNotificationDLL(int id, int? notificationId = null);
        int GetNotificationStatus(int? notificationId);
        List<AdmissionNotification> GetAdmissionNotificationBox();
        List<Notification> GetComments(int id);
        List<AdmissionNotification> GetAdmissionNotification(int id);
        Notification GetAdmissionNotificationDetails(int id, int loginId);
        List<UserDetails> GetRoles(int id, int level);
        bool ForwardAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole, string filePathName);
        bool SendbackAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole);
        string ConvertWordToPDF(bool toPDF, string wordpath, string pdfpath);
        bool ApproveAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole, string SavePath);
        bool ChangesAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole);
        string PublishNotification(int notificationId, int loginId, int loginRole);
    }
}
