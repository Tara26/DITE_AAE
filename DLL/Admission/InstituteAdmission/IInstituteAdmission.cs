using Models.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DLL.Admission.InstituteAdmission
{
    public interface IInstituteAdmission
    {
        List<InstituteWiseAdmission> GetDataAdmissionApplicantsDLL(int Session, int CourseType, int ApplicantType, int RoundOption, int AdmittedorRejected, int LoginId, int roleId, int ApplicatoinMode);
        List<InstituteWiseAdmission> DirectAdmissionApplicantDetailsDLL(int LoginId);
        List<InstituteWiseAdmission> GetDataAllocationFeeDetailsDLL(int ApplicationId);
        List<ApplicantApplicationForm> GetApplicantDocumentFeeDetails(int ApplicationId); 
        List<InstituteWiseAdmission> GeneratePaymentReceiptDLL(InstituteWiseAdmission model);
        List<InstituteWiseAdmission> UpdateAdmittedDetailsDLL(InstituteWiseAdmission model);
        List<InstituteWiseAdmission> GetCommentDetailsApplicantInstituteDLL(int ApplicationId);
        List<InstituteWiseAdmission> GeneratePaymentReceiptPDFDLL(int ApplicationId);
        List<InstituteWiseAdmission> GenerateDocumentPaymentReceiptPDFBLL(int ApplicationId);
        List<ApplicantDocumentsDetail> GetApplicantDocStatusDLL(int ApplicationId);

        List<InstituteWiseAdmission> GenerateAdmissionAcknowledgementPDFDLL(int ApplicationId);
        List<ApplicantDocumentsDetail> ApplicantDocumentDetailsInsDLL(ApplicantDocumentsDetail objApplicantApplicationForm);
        List<ApplicantDocumentsDetail> ApplicantUpdateInstituteDLL(ApplicantApplicationForm objApplicantApplicationForm);
        int GetUserDivionIdDLL(int LoginId);
        List<InstituteWiseAdmission> GetAdmissionApplicantsDistLoginDLL(AdmissionApplicantsDistLogin objAdmissionApplicantsDistLogin, int Id);
        List<HorizontalVerticalCategorycs> GetHorizontalCategoryDLL();
        int GetApplIdByApplicationNumberDLL(string ExistChkApplicationNumber);
        List<HorizontalVerticalCategorycs> GetVerticalCategoryDLL();
        List<HorizontalVerticalCategorycs> GetShiftsDetailsDLL();
        List<HorizontalVerticalCategorycs> GetUnitsDetailsDLL();
        List<ApplicationForm> GetITICollegeDetailsMasterDLL(int District, int Taluka);
        ApplicantAdmiAgainstVacancy GetInstituteMasterDLL(int LoginId);
        List<HorizontalVerticalCategorycs> GetInstituteTradeMasterDLL(int CollegeId);
        List<ApplicationForm> GetUnitsShiftsDetailsDLL(int CollegeId, int TradeId);
        List<ApplicantAdmiAgainstVacancy> UpdateApplicantAdmissionAgainstVacancyDLL(ApplicantAdmiAgainstVacancy objApplicantAdmiAgainstVacancyList);

        //03-07-2021
        string ApprovedRejectedList(InstituteWiseAdmission model, int loginId);
        List<InstituteWiseAdmission> SentBackAdmittedListDLL(InstituteWiseAdmission model, int loginId, int sentId);
        string GetforwardAdmittedListDLL(InstituteWiseAdmission model, int loginId, int ForId);
        string GetOnClickSendToHierarchyDLL(InstituteWiseAdmission model, int loginId, int ForId, string TabName);
        List<InstituteWiseAdmission> GetCommentDetailsRemarksByIdDLL(int ApplicationId);
        List<InstituteWiseAdmission> GetCommentDetailsRemarks(int loginId, int ApplicationId);

        string GetclickAddRemarksTransDLL(InstituteWiseAdmission modal, int loginId);

        string GetReceiptNumberGen(InstituteWiseAdmission model,int ApplID, int rcpt);
        ApplicantApplicationForm GetValidateRDNumberDll(string RD_Number, int loginId, int RDNumberType);

    }
}
