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

namespace BLL.Admission
{
    public interface IAdmissionBLL
    {
        // List<SeatAvailability_DD> GetCourseListBLL();
        List<SeatAvailability_DD> GetNotificationListBLL();
        List<SeatAvailability_DD> GetCourseListBLL();
        List<SeatAvailability_DD> GetDivisionListBLL();
        List<SeatAvailability_DD> GetDistrictListBLL(int divId);
        List<SeatAvailability_DD> GetTalukListBLL(int distId);
        List<SeatAvailability_DD> GetRoleListBLL();
        SelectList GetLoginRoleListBLL();
        List<SeatAvailabilityMaster> GetGridDetailsBLL(int courseId, int DivisionId, int DistrictId, int TalukId);
        //List<SeatAvailabilityMaster> GetITIAdminGridDetailsBLL(int col_Id);

        List<SeatAvailabilityMaster> GetTradeNameListBLL();

        List<SeatAvailabilityMaster> GetUnitTradeListBLL();

        List<SeatAvailabilityMaster> GetTradeIsPPPListBLL();

        List<SeatAvailabilityMaster> GetSysTrainingListBLL();

        //List<SeatAvailabilityMaster> GetAdmissionSeatAvailList(SeatAvailabilityMaster modal);

        string InsertTradeseatMasterBLL(insertRecordsForTrade model);
        string InsertTradeseatTranseBLL(insertRecordsForTrade model);

        List<SeatAvailabilityMaster> GetAdmissionSeatAvailListBLL(SeatAvailabilityMaster modal);
        List<SeatAvailabilityMaster> GetAdmissionSeatAvailList(SeatAvailabilityMaster modal);
        List<SeatAvailabilityMaster> GetSeatListBLL(int CourseTypes, int Divisions, int Districts, int Talukas);
        List<SeatAvailabilityMaster> GetSeatListBLL();
        List<SeatAvailabilityMaster> GetSeatListByIdBLL(string ListId);
        SelectList GetStatusListBLL();
        List<SeatAvailabilityMaster> UpdateRemarksDetailsForSeatBLL(string Remarks, int Status, int TradeITIseatidID);
        List<SeatAvailabilityUpdate> GetStatusListByIdBLL(int TradeITISeatid);
        #region admission notification
        List<Notification> GetComments(int id);
        List<AdmissionNotification> GetAdmissionNotification(int id);
        Notification GetAdmissionNotificationDetails(int id, int loginId);
        List<UserDetails> GetRoles(int id,int level);
        bool ForwardAdmissionNotification(int loginId, int roleId, int admiNotifId,string remarks,int loginRole, string filePathName);
        bool SendbackAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole);
        string ConvertWordToPDF(bool toPDF, string wordpath, string pdfpath);
        bool ApproveAdmissionNotification(int loginId, int roleId, int admiNotifId,string remarks, int loginRole, string SavePath);
        bool ChangesAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole);
        string PublishNotification(int notificationId, int loginId, int loginRole);
        string CreateAdmissionNotificationDetailsBLL(AdmissionNotification model);
        string ConvertUploadedAdmsnNotifToPDFBLL(AdmissionNotification model, string PdfFileNameFormat, string DocFileNameFormat, string DocumentsFolder);
        List<AdmissionNotification> GetUpdateNotificationBLL(int id, int? notificationId = null);
        int GetNotificationStatus(int? notificationId);
        List<AdmissionNotification> GetAdmissionNotificationBox();
        #endregion

        #region calendar
        SelectList GetCourseListCalendarBLL();
        SelectList GetSessionListCalendarBLL();
        SelectList GetAdmissionNotifNoListCalendarBLL();
        SelectList GetDepartmentListCalendarBLL();
        SelectList GetCalendarNotfyDescListBLL();
        List<AdmissionNotification> GetCalendarNotificationBLL(int id, int? calendarId = null);
        string CreateCalendarNotificationTransBLL(AdmissionNotification model);
        //List<AdmissionNotification> GetUpdateCalendarBLL(int? calendarId = null);
        List<AdmissionNotification> GetUpdateCalendarNtfBLL(int loginId, int? calendarId = null);

        //List<AdmissionNotification> GetCommentsCalendarFileBLL(int calendarId);
        AdmissionNotification GetAdmissionCalendarDetailsBLL(int id, int loginId);
        List<AdmissionNotification> GetAdmissionCalendarViewBLL(int id);
        List<AdmissionNotification> GetCommentsCalendarFileBLL(int id);
        bool ForwardAdmissionCalendarNotificationBLL(int loginId, int roleId, int admiNotifId, string remarks, int loginRole);
        bool ApproveAdmissionCalendarNotificationBLL(int loginId, int roleId, int admiNotifId, string remarks, int loginRole);
        bool ChangesAdmissionCalendarNotificationBLL(int loginId, int roleId, int admiNotifId, string remarks);
        SelectList GetDepartmentListDLL();
        string PublishAdmissionCalNotification(int notificationId, int loginId, int loginRole);
        List<AdmissionNotification> AdmissionCalendarNotificationBox();
        bool SendbackAdmissionCalNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole);
        bool ChangesAdmissionCalNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole);

        int GetNotificationCalEventStatus(int? calendarId);
        int GetApplicantTypeAdmission(int Id);
        List<AdmissionNotification> GetAdmissionNtfNumber(int Id);
        #endregion calendar

        #region seatAllocation

        string InsertRulesAllocationMasterBLL(SeatAllocation objSeatAllocation);
        List<SeatAllocation> GetSyllabusBLL();
        // List<SeatAllocation> GetYearTypeBLL();

        /// <summary>
        /// Method to featch years list based on condition,
        /// by default is_active true,
        /// send "false" param value to fetch all records.
        /// </summary>
        /// <param name="isActiveCheck">default value is true</param>
        /// <returns>list of years</returns>
        List<SeatAllocation> GetYearTypeBLL(bool isActiveCheck = true);
        List<SeatAllocation> GetSeatAllocationExamCourseBLL();
        List<SeatAllocation> GetSeatAllocationVerticalBLL();
        List<SeatAllocation> GetSeatAllocationHorizontalBLL();
        List<SeatAllocation> GetseatallocationListByIdBLL(string seatAllocationPopUp);
        List<SeatAllocation> GetSeatAllocationHydBLL();
        List<SeatAllocation> GetSeatAllocationGradeBLL();
        List<SeatAllocation> GetSeatAllocationOtherRulesBLL();
        List<SeatAllocation> GetExamYearBLL(int ExamYearId);
        string GetSeatupdateVerticalBLL(SeatAllocation modal);
        List<SeatAllocation> GetSeatAllocationVerticalBLL(int Exam_Year, int CourseTypes, string tabNameValue);
        List<SeatAllocation> GetSeatAllocationHorizontalBLL(int Exam_Year, int CourseTypes, string tabNameValue);
        List<SeatAllocation> GetSeatAllocationHydBLL(int Exam_Year, int CourseTypes, string tabNameValue);
        List<SeatAllocation> GetSeatAllocationGradeBLL(int Exam_Year, int CourseTypes, string tabNameValue);
        List<SeatAllocation> GetSeatAllocationOtherRulesBLL(int Exam_Year, int CourseTypes, string tabNameValue);

        List<SeatAllocation> GetAllocationSeatofRuletoUpdateBLL();
        List<SeatAllocation> GetApprovedAllocationSeatofRuleBLL();
        List<SeatAllocation> GetSeatAllocationVerticalBLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationHorizontalBLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationHydBLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationGradeBLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationOtherRulesBLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationBLL(SeatAllocation seatAllocation);
        List<SeatAllocation> GetSeatAllocationByFlowIdBLL(SeatAllocation seatAllocation);
        List<SeatAllocation> GetSeatAllocationApprovedBLL(SeatAllocation seatAllocation);
        List<SelectListItem> GetAllStatusBasedOnUserBLL();
        List<SeatAllocation> GetSeatAllocationRemarksStatusBLL(int Rules_allocation_masterid);

        List<SeatAllocation> GetSeatAllocationApprovedVerticalBLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationApprovedHorizontalBLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationApprovedHydBLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationApprovedGradeBLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetSeatAllocationApprovedOtherRulesBLL(int Rules_allocation_masterid);
        List<SeatAllocation> GetCommentsListBLL(int SeatAllocationId);
        List<ApplicantApplicationForm> GetCommentDetailsApplicantBLL(int SeatAllocationId);
        #endregion

        #region .. Institute Admission ..

        List<InstituteWiseAdmission> GetAdmissionRoundsBLL();
        List<InstituteWiseAdmission> GetDataAdmissionApplicantsBLL(int Session, int CourseType, int ApplicantType, int RoundOption, int AdmittedorRejected, int LoginId, int roleId,int ApplicatoinMode);
        List<InstituteWiseAdmission> DirectAdmissionApplicantDetailsBLL(int LoginId);
        List<InstituteWiseAdmission> GetAdmissionApplicantsDistLoginBLL(AdmissionApplicantsDistLogin objAdmissionApplicantsDistLogin, int Id);

        List<InstituteWiseAdmission> GetDataAllocationFeeDetailsBLL(int ApplicationId);
        List<ApplicantApplicationForm> GetApplicantDocumentFeeDetails(int ApplicationId);

        List<InstituteWiseAdmission> GeneratePaymentReceiptBLL(InstituteWiseAdmission model);
        List<InstituteWiseAdmission> UpdateAdmittedDetailsBLL(InstituteWiseAdmission model);
        List<InstituteWiseAdmission> GetCommentDetailsApplicantInstituteBLL(int ApplicationId);
        List<InstituteWiseAdmission> GeneratePaymentReceiptPDFBLL(int ApplicationId);
        List<InstituteWiseAdmission> GenerateDocumentPaymentReceiptPDFBLL(int ApplicationId);
        List<ApplicantDocumentsDetail> GetApplicantDocStatusDLL(int ApplicationId); 
        List<InstituteWiseAdmission> GenerateAdmissionAcknowledgementPDFBLL(int ApplicationId);

        List<ApplicantDocumentsDetail> ApplicantDocumentDetailsInsBLL(ApplicantDocumentsDetail objApplicantDocumentsDetail);
        List<ApplicantDocumentsDetail> ApplicantUpdateInstituteBLL(ApplicantApplicationForm objApplicantApplicationForm);

        //03-07-2021
        List<UserDetails> GetsendBack(int id);
        int GetUserDivionIdBLL(int LoginId);
        List<UserDetails> GetForward(int id);
        string ApprovedRejectedList(InstituteWiseAdmission model, int loginId);
        List<InstituteWiseAdmission> SentBackAdmittedListBLL(InstituteWiseAdmission model, int loginId, int sentId);
        string GetforwardAdmittedListBLL(InstituteWiseAdmission model, int loginId, int ForId);
        string GetOnClickSendToHierarchyBLL(InstituteWiseAdmission model, int loginId, int ForId, string TabName);
        List<InstituteWiseAdmission> GetCommentDetailsRemarksByIdBLL(int ApplicationId);
        List<InstituteWiseAdmission> GetCommentDetailsRemarks(int loginId, int ApplicationId);
        string GetclickAddRemarksTransBLL(InstituteWiseAdmission modal, int loginId);

        #endregion

        #region officer
        List<VerificationOfficer> GetOfficers(int loginId, int roleId);
        List<VerificationOfficer> GetInstituteId(int loginId);
        string AddOfficer(VerificationOfficer officer, int loginId);
        VerificationOfficer EditOfficer(int id);
        bool UpdateOfficer(VerificationOfficer officer, int loginId);
        bool DeleteOfficer(int id);
        List<VerificationOfficer> GetApplicants(int loginId, int applicantId, int year);
        List<VerificationOfficer> GetVerificationOfficerDetails(int loginId,int year, int courseType, int roleId);
        VerificationOfficer GetTotalApplicantOfficer(int id, int roleId);
        bool MapApplicantToOfficer(int id, int roleId);
        List<VerificationOfficer> GetInactiveOfficerApplicants(int loginId, int year, int courseType);
        bool ReMapApplicantToOfficer(int loginId, int roleId);
        List<VerificationOfficer> GetActiveOfficers(int loginId, int roleId);
        bool ReMapApplicantIndividualOff(int loginId, List<Applicants> list);
        List<VerificationOfficer> GetVerificationFeeCmntDetailsByIdBLL(int ApplicationId);
        #endregion

        #region dhanraj joined in rajgopal task
        List<VerificationOfficer> GetApplicantsStatus(int loginId,int roleId);
        List<VerificationOfficer> GetApplicantsStatusFilter(int loginId, int roleId, int year, int courseType,int applicanType , int division_id, int district_lgd_code, int taluk_lgd_code, int InstituteId);
        List<VerificationOfficer> GetApplicantsStatusBLL(int loginId,int roleId,int ApplicantType);
         List<VerificationOfficer> GetDataDocumentsVerificationFee(int loginId, int roleId, int? year, int? courseType, int? applicanType, int division_id, int district_lgd_code, int taluk_lgd_code, int InstituteId);

        List<VerificationOfficer> GetDataDocumentsVerificationFeepayment(int loginId, int roleId, int? year, int? courseType, int? applicanType);
        List<VerificationOfficer> GetDataDocumentsVerificationFeeNotPaid(int loginId, int roleId, int? year, int? courseType, int? applicanType, string ApplNo);
        #endregion

        #region Grievance
        List<VerificationOfficer>  ApplicantRankDetails(int loginId, int roleId);
        List<VerificationOfficer> GetDocumentTypes();
        string SubmitGrievanceTentative(List<HttpPostedFileBase> list,List<int> fileType, int loginId,string remarks, int roleId);
        List<VerificationOfficer> GetGrievanceTentativeStatus(int loginId,int roleId, int course, int year, int division, int district,int applicantType,int taluk,int institute);
        VerificationOfficer EditApplicantGrievance(int grivanceId,int roleId);
        bool VerifyGrievance(List<int> fileType, List<string> status, int grievanceId,string remark,int loginId, int roleId);
        bool SendForCorrection(List<int> fileType, List<string> status, int grievanceId, string remark,int loginId,int roleId);
        bool UpdateGrievanceTentative(List<HttpPostedFileBase> list, List<int> fileType, List<string> status, int loginId, string remarks, int grievanceId, int roleId);
        VerificationOfficer GetGrievanceGrid(int loginId);
        bool RejectGrivance(int grivanceId, string remarks,int loginId, int roleId);
        List<VerificationOfficer> GetGrievanceRemarks(int id);
        #endregion

        #region .. Application Form for seat allocation ..

        List<ApplicationForm> GetITICollegeDetailsBLL(int Pincode);
        List<ApplicationForm> GetITICollegeDetailsByDistrictTalukaBLL(int District, int Taluka);
        List<ApplicationForm> GetITICollegeTradeDetailsBLL(int TradeCode, string qual);
        List<ApplicationForm> GetReligionDetailsBLL();
        List<ApplicationForm> GetcoursetypeListbycalendar();
        List<ApplicationForm> GetGenderDetailsBLL();
        List<ApplicationForm> GetDocumentApplicationStatusBLL();
        List<ApplicationForm> GetCategoryListBLL();
        List<ApplicationForm> GetApplicantTypeListBLL(int roleId);
        List<ApplicationForm> GetQualificationListBLL();
        List<ApplicationForm> GetLocationListBLL();
        List<ApplicationForm> GetCasteListBLL();
        List<PersonWithDisabilityCategory> PersonWithDisabilityCategoryBLL();
        List<ApplicationForm> GetSyllabusListBLL();
        List<ApplicationForm> GetReservationsListBLL();
        List<ApplicationForm> GetDistrictMasterListBLL();
        List<ApplicationForm> GetDistrictMasterDivListBLL(int Division);
        List<ApplicantApplicationForm> GetEligibleDateFrmCalenderEventsBLL();
        List<ApplicationForm> GetTalukMasterListBLL(int distId);
        List<ApplicantApplicationForm> InsertApplicantFormDetailsBLL(ApplicantApplicationForm objApplicantApplicationForm);
        List<ApplicantDocumentsDetail> UpdateApplicantFormDetailsBLL(ApplicantDocumentsDetail objApplicantDocumentsDetail);
        List<ApplicantApplicationForm> SaveEducationDetailsBLL(ApplicantApplicationForm objApplicantApplicationForm);
        List<ApplicantApplicationForm> SaveAddressDetailsBLL(ApplicantApplicationForm objApplicantApplicationForm);
        List<ApplicantApplicationForm> SaveInstitueDetailsBLL(ApplicantApplicationForm objApplicantApplicationForm);
        List<ApplicantApplicationForm> InsertPaymentDetailsBLL(ApplicantApplicationForm objApplicantApplicationForm);
        List<ApplicantDocumentsDetail> ApplicantDocumentDetailsBLL(ApplicantDocumentsDetail objApplicantDocumentsDetail);
        List<ApplicantDocumentsDetail> ApplicantDocumentDetailsTransBLL(ApplicantDocumentsDetail objApplicantDocumentsDetail);
        List<GrievanceDocApplData> ApplicantGrievanceDetailsBLL(GrievanceDocApplData objApplicantDocumentsDetail);
        List<GrievanceDocApplData> ApplicantGrievanceDocumentDetailsBLL(GrievanceDocApplData objApplicantDocumentsDetail);
        ApplicantApplicationForm GetMasterApplicantDataBLL(int loginId, string DataFrom);
        ApplicantApplicationForm GetUserMasterDataBLL(int loginId, string DataFrom);
        ApplicantApplicationForm GetAdmissionDocumentDetailsBLL(int ApplicationId);
        ApplicantApplicationForm UpdateNextRoundDetailsWithTransBLL(ApplicantApplicationForm formCollection);
        List<ApplicantInstitutePreference> AddInstituePreferenceDetailsBLL(ApplicantInstitutePreference objApplicantInstitutePreference);
        List<ApplicantDocumentsDetail> GetDocumentDetailsBLL(int loginId);
        List<ApplicantInstitutePreference> GetApplicantInstitutePreferenceBLL(int loginId);
        List<ApplicationForm> GetApplicantReservationListBLL(int loginId);
        List<ApplicantApplicationForm> GetApplicantRemarksListBLL(int ApplicantTransId);

        ApplicationStatusUpdate UpdateApplicationDetailsByIdBLL(ApplicationStatusUpdate objApplicationStatusUpdate);
        ApplicantDocumentsDetail UpdateGrievanceApplicationDetailsByIdBLL(ApplicantDocumentsDetail objApplicationStatusUpdate);
        ApplicantApplicationForm GenerateApplicationNumberBLL(ApplicantApplicationForm objApplicantApplicationForm);

        #endregion

        #region .. Admission Against Vacancy seat at Institute level 

        int GetApplIdByApplicationNumberBLL(string ExistChkApplicationNumber);

        ApplicantAdmiAgainstVacancy GetInstituteMasterBLL(int LoginId);
        List<HorizontalVerticalCategorycs> GetInstituteTradeMasterBLL(int CollegeId);
        List<HorizontalVerticalCategorycs> GetHorizontalCategoryBLL();
        List<HorizontalVerticalCategorycs> GetVerticalCategoryBLL();
        List<HorizontalVerticalCategorycs> GetShiftsDetailsBLL();
        List<HorizontalVerticalCategorycs> GetUnitsDetailsBLL();
        List<ApplicationForm> GetITICollegeDetailsMasterBLL(int District, int Taluka);
        List<ApplicationForm> GetUnitsShiftsDetailsBLL(int CollegeId, int TradeId);
        List<ApplicantAdmiAgainstVacancy> UpdateApplicantAdmissionAgainstVacancyBLL(ApplicantAdmiAgainstVacancy objApplicantAdmiAgainstVacancyList);

        #endregion


        #region .. MeritList ..
        List<AdmissionMeritList> GetCommentsMeritListFileBLL(int id);
        List<UserDetails> GetMeritRoles(int id, int level);
        List<AdmissionMeritList> GetMeritStatusListBLL(int id);
        List<AdmissionMeritList> GetDistricts(int Divisions);
        List<AdmissionMeritList> GetDivision();
        List<AdmissionMeritList> GetApplicantType();
        List<AdmissionMeritList> GetApplicationMode_Bll();

        List<AdmissionMeritList> GetTraineeType();
        List<ApplicationForm> GetOtherBoardsListBLL();
        List<AdmissionMeritList> GetGradationType();       
        List<AdmissionMeritList> GetGradationMeritListBLL(int generateId, int AcademicYear, int ApplicantTypeId, int round, int id); 
        List<AdmissionMeritList> GetceckGradationTransTable(int generateId, int AcademicYear, int ApplicantTypeId, int id);
        List<AdmissionMeritList> GetReviewGradationMeritListBLL(int generateId, int AcademicYearReviewId, int ApplicantTypeId, int id);
        List<AdmissionMeritList> GetGradationMeritListDirBLL(int generateId, int AcademicYearDC, int ApplicantTypeId, int id);
        List<AdmissionMeritList> GetGradationListBLL(string rbId, int id, int loginId);
        List<AdmissionMeritList> GetIndexGradationMeritListBLL(string rbId);
        List<AdmissionMeritList> GetIndexTentativeGradationMeritListBLL();
        List<AdmissionMeritList> GetIndexFinalGradationMeritListBLL();
        List<AdmissionMeritList> GetGradationListStatusBLL(int id);
        List<AdmissionMeritList> GetGrdationListReviewDirComBLL(int id,int AcademicYearDC, int ApplicantTypeId);
        List<AdmissionMeritList> GetGrdationListReviewADNewBLL(int id,int AcademicYearAD, int ApplicantTypeId);
        List<AdmissionMeritList> GetGradationMeritListDirNewBLL(int AcademicYearDC, int ApplicantTypeId,int ApplicationId, int id);
        List<AdmissionMeritList> GetGradationMeritListDirNewRank(List<AdmissionMeritList> MaritList);
        List<AdmissionMeritList> GetGradationMeritAppListADNewIdBLL(int AcademicYearAD, int ApplicantTypeId,int ApplicationId, int id);
        List<AdmissionMeritList> GetApplicantResultMarqBLL(int id,int loginId);
        List<AdmissionMeritList> GetMeritListstatusPopupBLL(int id,int loginId, int ApplicationId);
        List<AdmissionMeritList> viewMeritListBll(int generateId, int AcademicYear, int ApplicantTypeId, int round, int DivisionId, int DistrictId, int id);
        string GetGenerateMeritListBLL(nestedMeritList model, int loginId,string remarks, int round);
        string ForwardMeritListDDBLL(AdmissionMeritList model, int loginId, int roleId, string remarks, int round);
        List<AdmissionMeritList> SendforDirectorBLL(AdmissionMeritList model, int loginId, int roleId, string remarks);
        string ApproveMeritListDDBLL(AdmissionMeritList model, int loginId, int roleId, string remarks);
        string PublishMeritList(AdmissionMeritList model, int loginId, int Status, string remarks);
        string ChangesMeritListBLL(AdmissionMeritList model,int backId ,int loginId, int Status, string remarks);
        string SentBacktoDDMeritListBLL(AdmissionMeritList model, int loginId, int Status,int sentId, string remarks);
        List<AdmissionMeritList> GetPublishcalendarEvents(int id);
        #endregion

        #region usecase 1-3 Seat Availablity
        List<SeatDetails> SeatAvailabilityStatus(int roleId);
        List<UserDetails> GetUserRoles(int id, int level,int roleId);
        List<SeatAvailabilty> GetSeatAvailabilityList(int loginId, int courseCode, string AcademicYear);
        List<SeatAvailabilty> GetSeatAvailabilityListAdd(int loginId,string miscode);
        List<SeatAvailabilty>  GetSeatTypes();
        int GetSeatsByTradeIdSeatType(int tradeId);
        List<SeatAvailabilty> GetSeatsBySeatTypeRules(int seattypeId, int tradeId);
        string SaveSeatAvailability(List<SeatDetails> seat, int loginId, int roleId);        
        bool ForwardSeatAvailability(List<SeatDetails> seat, int loginId, int roleId);        
        List<SeatDetails> GetRemarks(int seatId);
        bool ApproveSeatAvailability(List<SeatDetails> seat, int loginId,int roleId);
        //bool RejectSeatAvailability(List<SeatDetails> seat, int loginId);
        List<SeatAvailabilty> GetSeatViewDetails(int seatId);
        bool GetdelUnitShiftDetails(int seatId, int loginId, int roleId);
        bool GetdeActiveSeatDetails(int seatId,int TradeItiId, int loginId, int roleId);
        bool UpdateSeatAvailability(SeatDetails seat, int loginId,int roleId);
        List<DistrictTalukDetails> GetRegionDistrictCities(int loginId);
        List<DistrictTalukDetails> GetTaluks(int distilgdCOde);
        List<DistrictTalukDetails> GetInstitutes(int distilgdCOde);
        List<SeatAvailabilty> GetSeatAvailabilityListStatusFilter(int TabId, int Course_Id, int Year_id, int roleId, int loginId, int Division_Id, int District_Id, int taluk_id, int Insttype_Id);
        //List<SeatAvailabilty> GetIndexseatavailabilityList();
        #endregion

        #region Seat allotment
        List<SeatAutoAllocationModel> GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round,int RoleId, int LoginID);
        bool ForwardSeatAutoAllotment(List<int> seat,int loginId,int roleId, string Remarks, int Status);
        List<SeatAutoAllocationModel> GetGeneratedSeatAutoAllotmentList(int courseType, int applicantType, int academicYear, int round);
        List<SeatAutoAllocationModel> ViewSeatAutoAllotment(int allocationId);
        #endregion

        
        string RuleAllocationChkExistenceBLL(SeatAllocation objSeatAllocation);

        #region Seat Allocatment Review & Recommand Of Seat Matrix
        //ram - Use case -35 Screen-55&56
        List<ReviewSeatAllocated> GeneratedSeatAllotmentReviewBLL(int ddlCourseTypesGen, int ddlApplicantTypeGen, int ddlAcademicYearGen, int ddlRoundGen, int id);

        List<SeatMatrixAllocationDetail> GeneratedSeatAllotmentReviewListBLL(int id);

        List<SeatAutoAllocationModel> GetSeatMatrixViewListBLL(int courseType, int applicantType, int academicYear, int roun);

        bool ForwardSeatAutoAllotmentReviewBLL(List<int> seat, int loginId, int roleId);

        List<SeatMatrixAllocationDetail> GetCommentsListSeatAllocationBLL(int seatId);
        string UpdtApplITIInstDetailsBLL(int ApplicantId, int LoginID, int roleID, int? allocId);
        string DirectAdmissionSeatAllotmentBLL(InstituteWiseAdmission model);
        #endregion

        bool CheckNameAvailabilityBLL(string strName, int ApplicationId, int AadhaarRollNumber);
        bool CheckPhoneNumberAvailabilityBLL(string strName, int ApplicationId, int AadhaarRollNumber); 
        bool CheckEmailIdAvailabilityBll(string strName, int ApplicationId, int AadhaarRollNumber);

        #region AdmissionRegister
        List<AdmissionRegister> GetViewAdmissionRegisterDemo(int id, int? coursetype, int? session, int? division, int? district, int? Institute,int? InstType);
        List<AdmissionRegister> GetDistrictsJD();
        List<AdmissionRegister> GetAppRound(int roundype);
        List<AdmissionRegister> GetAppRoundReg(int applicantType,int? roundype);
        List<DistrictTalukDetails> GetInstitutesReg(int District);
        List<AdmissionRegister> GetInstTypeDetails(); 
        List<ApplicationForm> GetStateListDetails();
        #region comment
        //List<AdmissionRegister> GetViewAdmissionRegister(int collegeId, int courseType, int academicYear, int roleId, int loginId, int NotificationId);
        #endregion comment

        #endregion

        #region tansfer admission seats
        List<ApplicantTransferModel> GetAdmittedData(int id, int session, int course, int? trade, int round);
        List<ApplicantTransferModel> GetRequestedDetails(int id, int session, int course, int trade);
        List<Trades> GetTrades(int loginId);
        string GetMisCode(int instiId);
        List<Trades> GetAvailableseatsTrades(int instiId);
        List<Trades> GetUnits(int instiId,int trade);
        List<Trades> GetShifts(int instiId,int trade, int unit);
        string GetDualSystem(int insti, int trade);
        List<Trades> GetTranseferInstitutes(int type, int taluklgd);
        string SubmitAdmittedData(List<ApplicantTransferModel> seat, int loginId, int roleId);
        List<Trades> GetInstituteTypes();
        List<Trades> GetInstituteNames(int type);
        bool ForwardAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId, int flowId);
        List<ApplicantTransferModel> GetTransferRemarks(int seatId);
        bool ApproveAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId);
        bool SendBackAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId);
        List<ApplicantTransferModel> GetAdmittedDataStatus(int id,int roleId);
        List<ApplicantTransferModel> GetApplicantTransferbyList(int id,int roleId);
        List<ApplicantTransferModel> GetApprovedTransferbyList(int loginId, int roleId);
        List<ApplicantTransferModel> GetApplicantInstituteDetails(int id);
        bool UpdateApplicantInstituteDetails(ApplicantTransferModel tran,int roleId);
        List<SeatDetails> SeatTransferStatus(int roleId);
        #endregion

        List<AdmissionUnitsShifts> GetAdmissionUnitsDetailsDLL();
        List<AdmissionUnitsShifts> GetAdmissionShiftsDetailsDLL();
        bool InsertNewApplicantRegistrationDetailsBLL(NewApplicant objNewApplicant);
        bool InsertNewEmployeeRegistrationDetailsBLL(NewEmployee objNewApplicant);
        bool CheckApplicantExistorNot(string ApplicantMobileNo, string email);
    ApplicantApplicationForm GetApplicantApplicationForm();
    bool IsUserLoggedOnElsewhere(string userId, string sid);

        string GetReceiptNumber(InstituteWiseAdmission model, int ApplID, int rcpt);
        ApplicantApplicationForm GetValidateRDNumberBll(string RD_Number,int loginId, int RDNumberType);
    }
}
