using Models.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Admission.OfficerMapping
{
    public interface IOfficerMapping
    {
        List<VerificationOfficer> GetOfficers(int loginId, int roleId);
        List<VerificationOfficer> GetInstituteId(int loginId);
        string AddOfficer(VerificationOfficer officer, int loginId);
        VerificationOfficer EditOfficer(int id);
        bool UpdateOfficer(VerificationOfficer officer, int loginId);
        bool DeleteOfficer(int id);
        List<VerificationOfficer> GetApplicants(int loginId, int applicantId, int year);
        List<VerificationOfficer> GetVerificationOfficerDetails(int loginId, int year, int courseType, int roleId);
        VerificationOfficer GetTotalApplicantOfficer(int id, int roleId);
        bool MapApplicantToOfficer(int id, int roleId);
        List<VerificationOfficer> GetInactiveOfficerApplicants(int loginId, int year, int courseType);
        bool ReMapApplicantToOfficer(int loginId, int roleId);
        List<VerificationOfficer> GetActiveOfficers(int loginId, int roleId);
        bool ReMapApplicantIndividualOff(int loginId, List<Applicants> list);
    }
}
