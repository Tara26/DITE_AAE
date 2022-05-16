using Models.Admin;
using Models.Admission;
using Models.AdmissionModel;
using Models.ExamNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DLL.Admission
{
    public interface IAdmissionDLL
    {

        List<Notification> GetNotificationListDLL();
        List<SeatAvailability_DD> GetCourseListDLL();
        List<SeatAvailability_DD> GetDivisionListDLL();
        List<SeatAvailability_DD> GetDistrictListDLL(int divId);
        List<SeatAvailability_DD> GetTalukListDLL(int distId);
        List<SeatAvailability_DD> GetUpdateSeatAvailablityDLL(int courseId, int DivisionId, int DistrictId, int TalukaId);
        List<SeatAvailability_DD> GetRoleListDLL();
        List<SeatAvailabilityMaster> GetGridDetails(int courseId, int DivisionId, int DistrictId, int TalukId);
        List<SeatAvailabilityMaster> GetAdmissionSeatAvailListDLL(SeatAvailabilityMaster modal);
        List<SeatAvailabilityMaster> GetTradeNameListDLL();
        List<SeatAvailabilityMaster> GetUnitTradeListDLL();
        List<SeatAvailabilityMaster> GetTradeIsPPPListDLL();
        List<SeatAvailabilityMaster> GetSysTrainingListDLL();
        string InsertTradeseatMasterDLL(insertRecordsForTrade model);
        string InsertTradeseatTranseDLL(insertRecordsForTrade model);
        List<SeatAvailabilityMaster> GetSeatListDLL();
        List<SeatAvailabilityMaster> GetSeatListDLL(int CourseTypes, int Divisions, int Districts, int Talukas);
        List<SeatAvailabilityMaster> GetSeatListByIdDLL(string ListId);
        SelectList GetStatusListDLL();
        List<SeatAvailabilityMaster> UpdateRemarksDetailsForSeatDLL(string Remarks, int Status, int TradeITIseatidID);
        List<SeatAvailabilityUpdate> GetStatusListByIdDLL(int TradeITISeatid);
        
        SelectList GetDepartmentListDLL();

        #region calendar
        SelectList GetCourseListCalendarDLL();
        SelectList GetSessionListCalendarDLL();
        SelectList GetAdmissionNotifNoListCalendarDLL();
        SelectList GetDepartmentListCalendarDLL();
        SelectList GetCalendarNotfyDescListDLL();
        List<AdmissionNotification> GetCalendarNotificationDLL(int id, int? calendarId = null);
        string GetCalendarNotificationTransDLL(AdmissionNotification model);
        //List<AdmissionNotification> GetUpdatecalendarDLL(int? calendarId = null);
        List<AdmissionNotification> GetUpdateCalendarNtfDLL(int loginId, int? calendarId = null);
        //List<AdmissionNotification> GetCommentsCalendarFileDLL(int calendarId);

        AdmissionNotification GetAdmissionCalendarDetailsDLL(int id, int loginId);
        List<AdmissionNotification> GetAdmissionCalendarViewDLL(int id);
        List<AdmissionNotification> GetCommentsCalendarFileDLL(int id);
        bool ForwardAdmissionCalendarNotificationDLL(int loginId, int roleId, int admiNotifId, string remarks, int loginRole);
        bool ApproveAdmissionCalendarNotificationDLL(int loginId, int roleId, int admiNotifId, string remarks,int loginRole);
        bool ChangesAdmissionCalendarNotificationDLL(int loginId, int roleId, int admiNotifId, string remarks);
        string PublishAdmissionCalNotification(int notificationId, int loginId,int loginRole);      
        List<AdmissionNotification> AdmissionCalendarNotificationBox();
        bool SendbackAdmissionCalNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole);
        bool ChangesAdmissionCalNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole);

        int GetNotificationCalEventStatus(int? calendarId);
        int GetApplicantTypeAdmission(int Id);
        List<AdmissionNotification> GetAdmissionNtfNumber(int Id);

        #endregion

        #region seatAllocation

        string InsertRulesAllocationMasterDLL(SeatAllocation objSeatAllocation);
        string RuleAllocationChkExistenceDLL(SeatAllocation objSeatAllocation);
        List<InstituteWiseAdmission> GetAdmissionRoundsDLL();
        List<SeatAllocation> GetSyllabusDLL();
        //List<SeatAllocation> GetYearTypeDLL();
        List<SeatAllocation> GetYearTypeDLL(bool isActiveCheck = true);
        List<SeatAllocation> GetSeatAllocationExamCourseDLL();
        List<SeatAllocation> GetSeatAllocationHorizontalDLL();
        List<SeatAllocation> GetseatallocationListByIdDLL(string seatAllocationPopUp);
        List<SeatAllocation> GetseatallocationHydDLL();
        List<SeatAllocation> GetSeatAllocationGradeDLL();
        List<SeatAllocation> GetSeatAllocationOtherRulesDLL();
        List<SeatAllocation> GetExamYearDLL(int ExamYearId);
        string GetSeatupdateVerticalDLL(SeatAllocation modal);
        List<SeatAllocation> GetSeatAllocationVerticalDLL(int Exam_Year, int CourseTypes, string tabNameValue);
        List<SeatAllocation> GetSeatAllocationHorizontalDLL(int Exam_Year, int CourseTypes, string tabNameValue);
        List<SeatAllocation> GetseatallocationHydDLL(int Exam_Year, int CourseTypes, string tabNameValue);
        List<SeatAllocation> GetSeatAllocationGradeDLL(int Exam_Year, int CourseTypes, string tabNameValue);
        List<SeatAllocation> GetSeatAllocationOtherRulesDLL(int Exam_Year, int CourseTypes, string tabNameValue);
        List<SeatAllocation> GetSeatAllocationVerticalDLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationVerticalDLL();
        List<SeatAllocation> GetSeatAllocationHorizontalDLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetseatallocationHydDLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationGradeDLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationOtherRulesDLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetAllocationSeatofRuletoUpdateDLL();
        List<SeatAllocation> GetApprovedAllocationSeatofRuleDLL();
        List<SeatAllocation> GetSeatAllocationDLL(SeatAllocation seatAllocation);
        List<SeatAllocation> GetSeatAllocationApprovedDLL(SeatAllocation seatAllocation);
        List<SeatAllocation> GetSeatAllocationByFlowIdDLL(SeatAllocation seatAllocation);
        List<SeatAllocation> GetSeatAllocationRemarksStatusDLL(int Rules_allocation_masterid);
        List<SelectListItem> GetAllStatusBasedOnUserDLL();

        List<SeatAllocation> GetSeatAllocationApprovedVerticalDLL(int GetSeatAllocationApprovedVerticalDLL);
        List<SeatAllocation> GetSeatAllocationApprovedHorizontalDLL(int GetSeatAllocationApprovedVerticalDLL);
        List<SeatAllocation> GetSeatAllocationApprovedHydDLL(int GetSeatAllocationApprovedVerticalDLL);
        List<SeatAllocation> GetSeatAllocationApprovedGradeDLL(int GetSeatAllocationApprovedVerticalDLL);
        List<SeatAllocation> GetSeatAllocationApprovedOtherRulesDLL(int GetSeatAllocationApprovedVerticalDLL);
        List<SeatAllocation> GetCommentsListDLL(int SeatAllocationId);
        List<ApplicantApplicationForm> GetCommentDetailsApplicantDLL(int SeatAllocationId);

        #endregion
       
        #region .. Application Form for seat allocation ..

        List<ApplicationForm> GetITICollegeDetailsDLL(int PinCode);
        List<ApplicationForm> GetITICollegeDetailsByDistrictTalukaDLL(int District, int Taluka);
        List<ApplicationForm> GetITICollegeTradeDetailsDLL(int TradeCode, string qual);
        List<ApplicationForm> GetReligionDetailsDLL();
        List<ApplicationForm> GetcoursetypeListbycalendar();
        List<ApplicationForm> GetGenderDetailsDLL();
        List<ApplicationForm> GetDocumentApplicationStatusDLL();
        List<ApplicationForm> GetCategoryListDLL();
        List<ApplicationForm> GetApplicantTypeListDLL(int roleId);
        List<ApplicationForm> GetCasteListDLL();
        List<PersonWithDisabilityCategory> PersonWithDisabilityCategoryDLL();
        List<ApplicationForm> GetQualificationListDLL();
        List<ApplicationForm> GetLocationListDLL();
        List<ApplicationForm> GetSyllabusListDLL();
        List<ApplicationForm> GetDistrictMasterListDLL();
        List<ApplicationForm> GetDistrictMasterDivListDLL(int DivisionId);
        List<ApplicationForm> GetTalukMasterListDLL(int distId);
        List<ApplicationForm> GetReservationsListDLL();
        List<ApplicantApplicationForm> GetEligibleDateFrmCalenderEventsDLL();
        List<ApplicantApplicationForm> InsertApplicantFormDetailsDLL(ApplicantApplicationForm objApplicantApplicationForm);
        List<ApplicantDocumentsDetail> UpdateApplicantFormDetailsDLL(ApplicantDocumentsDetail objApplicantDocumentsDetail);
        List<GrievanceDocApplData> ApplicantGrievanceDetailsDLL(GrievanceDocApplData objGrievanceDocApplData);
        List<ApplicantApplicationForm> SaveEducationDetailsDLL(ApplicantApplicationForm objApplicantApplicationForm);
        List<ApplicantApplicationForm> SaveAddressDetailsDLL(ApplicantApplicationForm objApplicantApplicationForm);
        List<ApplicantApplicationForm> SaveInstitueDetailsDLL(ApplicantApplicationForm objApplicantApplicationForm);
        List<ApplicantApplicationForm> InsertPaymentDetailsDLL(ApplicantApplicationForm objApplicantApplicationForm);
        List<ApplicantDocumentsDetail> ApplicantDocumentDetailsDLL(ApplicantDocumentsDetail objApplicantApplicationForm);
        List<GrievanceDocApplData> ApplicantGrievanceDocumentDetailsDLL(GrievanceDocApplData objGrievanceDocApplData);
        List<ApplicantDocumentsDetail> ApplicantDocumentDetailsTransDLL(ApplicantDocumentsDetail objApplicantApplicationForm);
        ApplicantApplicationForm GetMasterApplicantDataDLL(int loginId, string DataFrom);
        ApplicantApplicationForm GetUserMasterDataDLL(int loginId, string DataFrom);
        List<ApplicantDocumentsDetail> GetDocumentDetailsDLL(int loginId);
        List<ApplicantInstitutePreference> GetApplicantInstitutePreferenceDLL(int loginId);
        ApplicantApplicationForm GetAdmissionDocumentDetailsDLL(int ApplicationId);
        List<ApplicantInstitutePreference> AddInstituePreferenceDetailsDLL(ApplicantInstitutePreference objApplicantDocumentsDetail);
        List<ApplicationForm> GetApplicantReservationListDLL(int loginId);
        ApplicantApplicationForm UpdateNextRoundDetailsWithTransDLL(ApplicantApplicationForm formCollection);
        ApplicationStatusUpdate UpdateApplicationDetailsByIdDLL(ApplicationStatusUpdate objApplicationStatusUpdate);
        ApplicantDocumentsDetail UpdateGrievanceApplicationDetailsByIdDLL(ApplicantDocumentsDetail objApplicationStatusUpdate);
        ApplicantApplicationForm GenerateApplicationNumberDLL(ApplicantApplicationForm objApplicantApplicationForm);
        List<ApplicantApplicationForm> GetApplicantRemarksListDLL(int ApplicantTransId);

        #region dhanraj joined in rajgopal task
        List<VerificationOfficer> GetApplicantsStatus(int loginId, int roleId);
        List<VerificationOfficer> GetApplicantsStatusFilter(int loginId, int roleId, int year, int courseType,int applicanType,int division_id, int district_lgd_code, int taluk_lgd_code, int InstituteId);
        List<VerificationOfficer> GetApplicantsStatusDLL(int loginId, int roleId, int ApplicantType);
        List<VerificationOfficer> GetDataDocumentsVerificationFee(int loginId, int roleId, int? year, int? courseType, int? applicanType, int division_id, int district_lgd_code, int taluk_lgd_code, int InstituteId);
        List<VerificationOfficer> GetDataDocumentsVerificationFeepayment(int loginId, int roleId, int? year, int? courseType, int? applicanType);
        List<VerificationOfficer> GetDataDocumentsVerificationFeeNotPaid(int loginId, int roleId, int? year, int? courseType, int? applicanType,string ApplNo);
        List<VerificationOfficer> GetVerificationFeeCmntDetailsByIdDLL(int ApplicationId);
        #endregion

        #endregion

        #region  .. MeritList ..
        List<UserDetails> GetRoles(int id, int level);
        List<AdmissionMeritList> GetCommentsMeritListFileDLL(int id);
        List<UserDetails> GetMeritRoles(int id, int level);
        List<AdmissionMeritList> GetMeritStatusListDLL(int id);
        List<AdmissionMeritList> GetDistricts(int Divisions);
        List<AdmissionMeritList> GetDivision();
        List<AdmissionMeritList> GetApplicantType();

        List<AdmissionMeritList> GetApplicationMode_Dll();

        List<AdmissionMeritList> GetTraineeType();
        List<ApplicationForm> GetOtherBoardsListDLL();
        List<AdmissionMeritList> GetGradationType();       
        List<AdmissionMeritList> GetGradationMeritListDLL(int generateId, int AcademicYear, int ApplicantTypeId, int round, int id);
        List<AdmissionMeritList> GetceckGradationTransTable(int generateId, int AcademicYear, int ApplicantTypeId, int id);
        List<AdmissionMeritList> GetReviewGradationMeritListDLL(int generateId, int AcademicYearReviewId, int ApplicantTypeId, int id);
        List<AdmissionMeritList> GetGradationMeritListDirDLL(int generateId, int AcademicYearDC, int ApplicantTypeId, int id);
        List<AdmissionMeritList> GetGradationListDLL(string rbId, int id, int loginId);
        List<AdmissionMeritList> GetIndexGradationMeritListDLL(string rbId);
        List<AdmissionMeritList> GetIndexTentativeGradationMeritListDLL();
        List<AdmissionMeritList> GetIndexFinalGradationMeritListDLL();
        List<AdmissionMeritList> GetGradationListStatusDLL(int id);
        List<AdmissionMeritList> GetGrdationListReviewDirComDLL(int id, int AcademicYearDC, int ApplicantTypeId);
        List<AdmissionMeritList> GetGrdationListReviewADNewDLL(int id, int AcademicYearAD, int ApplicantTypeId);
        List<AdmissionMeritList> GetGradationMeritListDirNewDLL(int AcademicYearDC, int ApplicantTypeId, int ApplicationId, int id);
        List<AdmissionMeritList> GetGradationMeritListDirNewRank(List<AdmissionMeritList> MaritList);
        List<AdmissionMeritList> GetGradationMeritAppListADNewIdDLL( int AcademicYearAD, int ApplicantTypeId, int ApplicationId, int id);
        List<AdmissionMeritList> GetApplicantResultMarqDLL(int id,int loginId);
        List<AdmissionMeritList> GetMeritListstatusPopupDLL(int id,int loginId, int ApplicationId);
        string GetGenerateMeritListDLL(nestedMeritList model, int loginId,string remarks, int round);
        string ForwardMeritListDDDLL(AdmissionMeritList model, int loginId, int roleId, string remarks, int round);
        List<AdmissionMeritList> SendforDirectorDLL(AdmissionMeritList model, int loginId, int roleId, string remarks);
        string ApproveMeritListDDDLL(AdmissionMeritList model, int loginId, int roleId, string remarks);

        List<AdmissionMeritList> viewMeritListDll(int generateId, int AcademicYear, int ApplicantTypeId, int round, int DivisionId, int DistrictId, int id);
        string PublishMeritList(AdmissionMeritList model, int loginId, int Status,string remarks);
        string ChangesMeritListDLL(AdmissionMeritList model,int backId, int loginId, int Status,string remarks);
        string SentBacktoDDMeritListDLL(AdmissionMeritList model, int loginId, int Status,int sentId, string remarks);
        List<AdmissionMeritList> GetPublishcalendarEvents(int id);
        #endregion

        #region Seat Allocation Review & Recommand Of Seat Matrix
        //ram use case -35 screen 55 &56
        SeatMatrixAllocationDetail GetAdmissionCalendarDetailsDLL1(int id);

        #endregion

        bool CheckNameAvailabilityDLL(string strName, int ApplicationId, int AadhaarRollNumber);
        bool CheckPhoneNumberAvailabilityDLL(string strName, int ApplicationId, int AadhaarRollNumber);
        bool CheckEmailIdAvailabilityDll(string strName, int ApplicationId, int AadhaarRollNumber);

        #region AdmissionRegister
        List<AdmissionRegister> GetViewAdmissionRegisterDemo(int id, int? coursetype, int? session, int? division, int? district, int? Institute, int? InstType);
        List<AdmissionRegister> GetDistrictsJD();
        List<AdmissionRegister> GetAppRound(int roundType); 
        List<AdmissionRegister> GetAppRoundReg(int applicantType,int? roundType);
        List<DistrictTalukDetails> GetInstitutesReg(int District);
        List<AdmissionRegister> GetInstTypeDetails(); 
        List<ApplicationForm> GetStateListDetails();
        #region comment
        //List<AdmissionRegister> GetViewAdmissionRegister(int collegeId, int courseType, int academicYear, int roleId, int loginId,int NotificationId);
        #endregion comment

        #endregion

        List<UserDetails> GetsendBack(int id);
        List<UserDetails> GetForward(int id);
        List<AdmissionUnitsShifts> GetAdmissionUnitsDetailsDLL();
        List<AdmissionUnitsShifts> GetAdmissionShiftsDetailsDLL();
        bool InsertNewApplicantRegistrationDetailsDLL(NewApplicant objNewApplicant);
        bool InsertNewEmployeeRegistrationDetailsDLL(NewEmployee objNewApplicant);
        bool CheckApplicantExistorNot(string ApplicantMobileNo, string email);
    ApplicantApplicationForm GetApplicantApplicationForm();
    bool IsUserLoggedOnElsewhere(string userId, string sid);
  }
}
