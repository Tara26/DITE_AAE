using Models.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DLL.Admission.Grievances
{
    public interface IGrievances
    {
        List<VerificationOfficer> ApplicantRankDetails(int loginId, int roleId);
        List<VerificationOfficer> GetDocumentTypes();
        string SubmitGrievanceTentative(List<HttpPostedFileBase> list, List<int> fileType, int loginId, string remarks, int roleId);
        List<VerificationOfficer> GetGrievanceTentativeStatus(int loginId, int roleId, int course, int year, int division, int district,int applicantType, int taluk, int institute);
        VerificationOfficer EditApplicantGrievance(int grivanceId, int roleId);
        bool VerifyGrievance(List<int> fileType, List<string> status, int grievanceId, string remark, int loginId, int roleId);
        bool SendForCorrection(List<int> fileType, List<string> status, int grievanceId, string remark, int loginId, int roleId);
        bool UpdateGrievanceTentative(List<HttpPostedFileBase> list, List<int> fileType, List<string> status, int loginId, string remarks, int grievanceId, int roleId);
        VerificationOfficer GetGrievanceGrid(int loginId);
        bool RejectGrivance(int grivanceId, string remarks, int loginId, int roleId);
        List<VerificationOfficer> GetGrievanceRemarks(int id);
    }
}
