using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DLL.Admission;
using Models.Admission;
using Models.Admin;
using Models.AdmissionModel;
using Models.ExamNotification;
using System.Web;
using DLL.Admission.SeatAutoAllocation;
using DLL.Admission.SeatAvailability;
using DLL.Admission.OfficerMapping;
using DLL.Admission.AdmissionNotifications;
using DLL.Admission.Grievances;
using DLL.Admission.InstituteAdmission;
using DLL.Admission.TransferAdmissionSeats;

namespace BLL.Admission
{
    public class AdmissionBLL : IAdmissionBLL
    {
        private readonly IAdmissionDLL _adDll;
        private readonly IAdmissionNotifications _admissionNotif;
        private readonly IOfficerMapping _officerMap;
        private readonly IGrievances _grievance;
        private readonly ISeatAvailability _seatAvailability;
        private readonly ISeatAutoAllocation _seatAllocation;
        private readonly IInstituteAdmission _InstituteAdmission;
        private readonly ITransferAdmissionSeat _transfer;

        public AdmissionBLL()
        {
            this._adDll = new AdmissionDLL();
            this._admissionNotif = new AdmissionNotifications();
            this._officerMap = new OfficerMapping();
            this._grievance = new Grievances();
            this._seatAvailability = new SeatAvailability();
            this._seatAllocation = new SeatAutoAllocation();
            this._InstituteAdmission = new InstituteAdmission();
            this._transfer = new TransferAdmissionSeat();
        }

        public List<SeatAvailability_DD> GetCourseListBLL()
        {
            return _adDll.GetCourseListDLL();
        }

        public List<SeatAvailability_DD> GetDistrictListBLL(int divId)
        {
            return _adDll.GetDistrictListDLL(divId);
        }

        public List<SeatAvailability_DD> GetDivisionListBLL()
        {
            return _adDll.GetDivisionListDLL();
        }

        public List<SeatAvailabilityMaster> GetGridDetailsBLL(int courseId, int DivisionId, int DistrictId, int TalukId)
        {
            return _adDll.GetGridDetails(courseId, DivisionId, DistrictId, TalukId);
        }

        public List<SeatAvailability_DD> GetLoginRoleListBLL()
        {
            return _adDll.GetRoleListDLL();
        }

        public List<SeatAvailability_DD> GetRoleListBLL()
        {
            return _adDll.GetRoleListDLL();
        }

        public List<SeatAvailability_DD> GetTalukListBLL(int distId)
        {
            return _adDll.GetTalukListDLL(distId);
        }


        public List<SeatAvailability_DD> GetUpdateSeatAvailablity(int courseId, int DivisionId, int DistrictId, int TalukaId)
        {
            return _adDll.GetUpdateSeatAvailablityDLL(courseId, DivisionId, DistrictId, TalukaId);
        }

        SelectList IAdmissionBLL.GetLoginRoleListBLL()
        {
            throw new NotImplementedException();
        }

        List<SeatAvailability_DD> IAdmissionBLL.GetNotificationListBLL()
        {
            throw new NotImplementedException();
        }

        public List<SeatAvailabilityMaster> GetTradeNameListBLL()
        {
            return _adDll.GetTradeNameListDLL();
        }

        public List<SeatAvailabilityMaster> GetAdmissionSeatAvailList(SeatAvailabilityMaster modal)
        {
            return this._adDll.GetAdmissionSeatAvailListDLL(modal);
        }

        public List<SeatAvailabilityMaster> GetUnitTradeListBLL()
        {
            return _adDll.GetUnitTradeListDLL();
        }
        public List<SeatAvailabilityMaster> GetTradeIsPPPListBLL()
        {
            return _adDll.GetTradeIsPPPListDLL();
        }
        public List<SeatAvailabilityMaster> GetSysTrainingListBLL()
        {
            return _adDll.GetSysTrainingListDLL();
        }


        public string InsertTradeseatMasterBLL(insertRecordsForTrade model)
        {
            return _adDll.InsertTradeseatMasterDLL(model);
        }

        public string InsertTradeseatTranseBLL(insertRecordsForTrade model)
        {
            return _adDll.InsertTradeseatTranseDLL(model);
        }

        public List<SeatAvailabilityMaster> GetAdmissionSeatAvailListBLL(SeatAvailabilityMaster modal)
        {
            return this._adDll.GetAdmissionSeatAvailListDLL(modal);
        }


        public List<SeatAvailabilityMaster> GetSeatListByIdBLL(string ListId)
        {
            try
            {
                return _adDll.GetSeatListByIdDLL(ListId);
            }
            catch (Exception ex)
            {

                return _adDll.GetSeatListByIdDLL(ListId);
            }
        }

        public SelectList GetStatusListBLL()
        {
            return _adDll.GetStatusListDLL();
        }

        public List<SeatAvailabilityMaster> UpdateRemarksDetailsForSeatBLL(string Remarks, int Status, int TradeITIseatidID)
        {
            return _adDll.UpdateRemarksDetailsForSeatDLL(Remarks, Status, TradeITIseatidID);
        }

        public List<SeatAvailabilityUpdate> GetStatusListByIdBLL(int TradeITISeatid)
        {
            return _adDll.GetStatusListByIdDLL(TradeITISeatid);
        }

        public List<SeatAvailabilityMaster> GetSeatListBLL()
        {
            return _adDll.GetSeatListDLL();
        }

        public List<SeatAvailabilityMaster> GetSeatListBLL(int CourseTypes, int Divisions, int Districts, int Talukas)
        {
            return _adDll.GetSeatListDLL(CourseTypes, Divisions, Districts, Talukas);
        }
        #region admission notification
        public List<Notification> GetComments(int id)
        {
            return this._admissionNotif.GetComments(id);
        }
        public List<AdmissionNotification> GetAdmissionNotification(int id)
        {
            return this._admissionNotif.GetAdmissionNotification(id);
        }
        public Notification GetAdmissionNotificationDetails(int id, int loginId)
        {
            return _admissionNotif.GetAdmissionNotificationDetails(id, loginId);
        }
        public List<UserDetails> GetRoles(int id, int level)
        {
            return _admissionNotif.GetRoles(id, level);
        }
        public bool ForwardAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole, string filePathName)
        {
            return _admissionNotif.ForwardAdmissionNotification(loginId, roleId, admiNotifId, remarks, loginRole, filePathName);
        }
        public bool SendbackAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            return _admissionNotif.SendbackAdmissionNotification(loginId, roleId, admiNotifId, remarks, loginRole);
        }
        public string ConvertWordToPDF(bool toPDF, string wordpath, string pdfpath)
        {
            return _admissionNotif.ConvertWordToPDF(toPDF, wordpath, pdfpath);
        }
        public bool ApproveAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole, string SavePath)
        {
            return _admissionNotif.ApproveAdmissionNotification(loginId, roleId, admiNotifId, remarks, loginRole, SavePath);
        }
        public bool ChangesAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            return _admissionNotif.ChangesAdmissionNotification(loginId, roleId, admiNotifId, remarks, loginRole);
        }
        public string PublishNotification(int notificationId, int loginId, int loginRole)
        {
            return _admissionNotif.PublishNotification(notificationId, loginId, loginRole);
        }
        public string CreateAdmissionNotificationDetailsBLL(AdmissionNotification model)
        {
            return _admissionNotif.CreateAdmissionNotificationDetailsDLL(model);
        }
        public string ConvertUploadedAdmsnNotifToPDFBLL(AdmissionNotification model, string PdfFileNameFormat, string DocFileNameFormat, string DocumentsFolder)
        {
            return _admissionNotif.ConvertUploadedAdmsnNotifToPDFDLL(model, PdfFileNameFormat, DocFileNameFormat, DocumentsFolder);
        }
        public List<AdmissionNotification> GetUpdateNotificationBLL(int id, int? notificationId = null)
        {
            return _admissionNotif.GetUpdateNotificationDLL(id, notificationId);
        }
        public int GetNotificationStatus(int? notificationId)
        {
            return _admissionNotif.GetNotificationStatus(notificationId);
        }
        public List<AdmissionNotification> GetAdmissionNotificationBox()
        {
            return _admissionNotif.GetAdmissionNotificationBox();
        }
        #endregion

        public SelectList GetDepartmentListDLL()
        {
            return _adDll.GetDepartmentListDLL();
        }

        #region calendar
        public SelectList GetCourseListCalendarBLL()
        {
            return _adDll.GetCourseListCalendarDLL();
        }
        public SelectList GetSessionListCalendarBLL()
        {
            return _adDll.GetSessionListCalendarDLL();
        }
        public SelectList GetAdmissionNotifNoListCalendarBLL()
        {
            return _adDll.GetAdmissionNotifNoListCalendarDLL();
        }
        public SelectList GetDepartmentListCalendarBLL()
        {
            return _adDll.GetDepartmentListCalendarDLL();
        }

        public SelectList GetCalendarNotfyDescListBLL()
        {
            return _adDll.GetCalendarNotfyDescListDLL();
        }
        public List<AdmissionNotification> GetCalendarNotificationBLL(int id, int? calendarId = null)
        {
            return _adDll.GetCalendarNotificationDLL(id, calendarId);
        }

        public string CreateCalendarNotificationTransBLL(AdmissionNotification model)
        {
            return _adDll.GetCalendarNotificationTransDLL(model);
        }
        public List<AdmissionNotification> GetUpdateCalendarNtfBLL(int loginId, int? calendarId = null)
        {
            return _adDll.GetUpdateCalendarNtfDLL(loginId, calendarId);
        }


        //public List<AdmissionNotification> GetCommentsCalendarFileBLL(int calendarId)
        //{
        //    return _adDll.GetCommentsCalendarFileDLL(calendarId);
        //}

        //public List<AdmissionNotification> GetUpdateCalendarBLL(int? calendarId = null)
        //{
        //    return _adDll.GetUpdatecalendarDLL(calendarId);
        //}

        public AdmissionNotification GetAdmissionCalendarDetailsBLL(int id, int loginId)
        {
            return _adDll.GetAdmissionCalendarDetailsDLL(id, loginId);
        }

        public List<AdmissionNotification> GetAdmissionCalendarViewBLL(int id)
        {
            return _adDll.GetAdmissionCalendarViewDLL(id);
        }
        public List<AdmissionNotification> GetCommentsCalendarFileBLL(int id)
        {
            return this._adDll.GetCommentsCalendarFileDLL(id);
        }
        public bool ForwardAdmissionCalendarNotificationBLL(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            return _adDll.ForwardAdmissionCalendarNotificationDLL(loginId, roleId, admiNotifId, remarks, loginRole);
        }
        public bool ApproveAdmissionCalendarNotificationBLL(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            return _adDll.ApproveAdmissionCalendarNotificationDLL(loginId, roleId, admiNotifId, remarks, loginRole);
        }
        public bool ChangesAdmissionCalendarNotificationBLL(int loginId, int roleId, int admiNotifId, string remarks)
        {
            return _adDll.ChangesAdmissionCalendarNotificationDLL(loginId, roleId, admiNotifId, remarks);
        }
        public string PublishAdmissionCalNotification(int notificationId, int loginId, int loginRole)
        {
            return _adDll.PublishAdmissionCalNotification(notificationId, loginId, loginRole);
        }
        public List<AdmissionNotification> AdmissionCalendarNotificationBox()
        {
            return _adDll.AdmissionCalendarNotificationBox();
        }
        public bool SendbackAdmissionCalNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            return _adDll.SendbackAdmissionCalNotification(loginId, roleId, admiNotifId, remarks, loginRole);
        }
        public bool ChangesAdmissionCalNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            return _adDll.ChangesAdmissionCalNotification(loginId, roleId, admiNotifId, remarks, loginRole);
        }

        public int GetNotificationCalEventStatus(int? calendarId)
        {
            return _adDll.GetNotificationCalEventStatus(calendarId);
        }
        public int GetApplicantTypeAdmission(int Id)
        {
            return _adDll.GetApplicantTypeAdmission(Id);
        }
        public List<AdmissionNotification> GetAdmissionNtfNumber(int Id)
        {
            return _adDll.GetAdmissionNtfNumber(Id);
        }

        #endregion calendar

        #region seatAllocation

        public string InsertRulesAllocationMasterBLL(SeatAllocation objSeatAllocation)
        {
            return _adDll.InsertRulesAllocationMasterDLL(objSeatAllocation);
        }

        public List<SeatAllocation> GetSyllabusBLL()
        {
            return _adDll.GetSyllabusDLL();
        }

        //public List<SeatAllocation> GetYearTypeBLL()
        //{
        //    return _adDll.GetYearTypeDLL();
        //}
        public List<SeatAllocation> GetYearTypeBLL(bool isActiveCheck = true)
        {
            return _adDll.GetYearTypeDLL(isActiveCheck);
        }
        public List<SeatAllocation> GetSeatAllocationExamCourseBLL()
        {
            return _adDll.GetSeatAllocationExamCourseDLL();
        }



        public List<SeatAllocation> GetSeatAllocationVerticalBLL()
        {
            return _adDll.GetSeatAllocationVerticalDLL();
        }

        public List<SeatAllocation> GetSeatAllocationHorizontalBLL()
        {
            return _adDll.GetSeatAllocationHorizontalDLL();
        }

        public List<SeatAllocation> GetseatallocationListByIdBLL(string seatAllocationPopUp)
        {
            return this._adDll.GetseatallocationListByIdDLL(seatAllocationPopUp);

        }
        public List<SeatAllocation> GetSeatAllocationHydBLL()
        {
            return this._adDll.GetseatallocationHydDLL();
        }

        public List<SeatAllocation> GetSeatAllocationGradeBLL()
        {
            return this._adDll.GetSeatAllocationGradeDLL();
        }

        public List<SeatAllocation> GetSeatAllocationOtherRulesBLL()
        {
            return this._adDll.GetSeatAllocationOtherRulesDLL();
        }

        public List<SeatAllocation> GetExamYearBLL(int ExamYearId)
        {
            return _adDll.GetExamYearDLL(ExamYearId);
        }

        public string GetSeatupdateVerticalBLL(SeatAllocation modal)
        {
            return this._adDll.GetSeatupdateVerticalDLL(modal);
        }

        public List<SeatAllocation> GetSeatAllocationVerticalBLL(int Exam_Year, int CourseTypes, string tabNameValue)
        {
            return _adDll.GetSeatAllocationVerticalDLL(Exam_Year, CourseTypes, tabNameValue);
        }
        public List<SeatAllocation> GetSeatAllocationHorizontalBLL(int Exam_Year, int CourseTypes, string tabNameValue)
        {
            return _adDll.GetSeatAllocationHorizontalDLL(Exam_Year, CourseTypes, tabNameValue);
        }
        public List<SeatAllocation> GetSeatAllocationHydBLL(int Exam_Year, int CourseTypes, string tabNameValue)
        {
            return _adDll.GetseatallocationHydDLL(Exam_Year, CourseTypes, tabNameValue);
        }
        public List<SeatAllocation> GetSeatAllocationGradeBLL(int Exam_Year, int CourseTypes, string tabNameValue)
        {
            return _adDll.GetSeatAllocationGradeDLL(Exam_Year, CourseTypes, tabNameValue);
        }
        public List<SeatAllocation> GetSeatAllocationOtherRulesBLL(int Exam_Year, int CourseTypes, string tabNameValue)
        {
            return _adDll.GetSeatAllocationOtherRulesDLL(Exam_Year, CourseTypes, tabNameValue);
        }


        public List<SeatAllocation> GetAllocationSeatofRuletoUpdateBLL()
        {
            return _adDll.GetAllocationSeatofRuletoUpdateDLL();
        }

        public List<SeatAllocation> GetApprovedAllocationSeatofRuleBLL()
        {
            return _adDll.GetApprovedAllocationSeatofRuleDLL();
        }


        public List<SeatAllocation> GetSeatAllocationVerticalBLL(int Rules_allocation_masterid)
        {
            return _adDll.GetSeatAllocationVerticalDLL(Rules_allocation_masterid);
        }
        public List<SeatAllocation> GetSeatAllocationHorizontalBLL(int Rules_allocation_masterid)
        {
            return _adDll.GetSeatAllocationHorizontalDLL(Rules_allocation_masterid);
        }
        public List<SeatAllocation> GetSeatAllocationHydBLL(int Rules_allocation_masterid)
        {
            return _adDll.GetseatallocationHydDLL(Rules_allocation_masterid);
        }
        public List<SeatAllocation> GetSeatAllocationGradeBLL(int Rules_allocation_masterid)
        {
            return _adDll.GetSeatAllocationGradeDLL(Rules_allocation_masterid);
        }
        public List<SeatAllocation> GetSeatAllocationOtherRulesBLL(int Rules_allocation_masterid)
        {
            return _adDll.GetSeatAllocationOtherRulesDLL(Rules_allocation_masterid);
        }
        public List<SeatAllocation> GetSeatAllocationBLL(SeatAllocation seatAllocation)
        {
            return _adDll.GetSeatAllocationDLL(seatAllocation);
        }
        public List<SeatAllocation> GetSeatAllocationByFlowIdBLL(SeatAllocation seatAllocation)
        {
            return _adDll.GetSeatAllocationByFlowIdDLL(seatAllocation);
        }
        public List<SeatAllocation> GetSeatAllocationApprovedBLL(SeatAllocation seatAllocation)
        {
            return _adDll.GetSeatAllocationApprovedDLL(seatAllocation);
        }
        public List<SelectListItem> GetAllStatusBasedOnUserBLL()
        {
            return _adDll.GetAllStatusBasedOnUserDLL();
        }
        public List<SeatAllocation> GetSeatAllocationRemarksStatusBLL(int Rules_allocation_masterid)
        {
            return _adDll.GetSeatAllocationRemarksStatusDLL(Rules_allocation_masterid);
        }

        public List<SeatAllocation> GetSeatAllocationApprovedVerticalBLL(int Rules_allocation_masterid)
        {
            return _adDll.GetSeatAllocationApprovedVerticalDLL(Rules_allocation_masterid);
        }
        public List<SeatAllocation> GetSeatAllocationApprovedHorizontalBLL(int Rules_allocation_masterid)
        {
            return _adDll.GetSeatAllocationApprovedHorizontalDLL(Rules_allocation_masterid);
        }
        public List<SeatAllocation> GetSeatAllocationApprovedHydBLL(int Rules_allocation_masterid)
        {
            return _adDll.GetSeatAllocationApprovedHydDLL(Rules_allocation_masterid);
        }
        public List<SeatAllocation> GetSeatAllocationApprovedGradeBLL(int Rules_allocation_masterid)
        {
            return _adDll.GetSeatAllocationApprovedGradeDLL(Rules_allocation_masterid);
        }
        public List<SeatAllocation> GetSeatAllocationApprovedOtherRulesBLL(int Rules_allocation_masterid)
        {
            return _adDll.GetSeatAllocationApprovedOtherRulesDLL(Rules_allocation_masterid);
        }

        public List<SeatAllocation> GetCommentsListBLL(int SeatAllocationId)
        {
            return this._adDll.GetCommentsListDLL(SeatAllocationId);
        }

        public List<ApplicantApplicationForm> GetCommentDetailsApplicantBLL(int SeatAllocationId)
        {
            return this._adDll.GetCommentDetailsApplicantDLL(SeatAllocationId);
        }

        #endregion

        #region .. Institute Admission ..

        public List<InstituteWiseAdmission> GetAdmissionRoundsBLL()
        {
            return _adDll.GetAdmissionRoundsDLL();
        }

        public List<InstituteWiseAdmission> GetDataAdmissionApplicantsBLL(int Session, int CourseType, int ApplicantType, int RoundOption, int AdmittedorRejected, int LoginId, int roleId,int ApplicatoinMode)
        {
            return _InstituteAdmission.GetDataAdmissionApplicantsDLL(Session, CourseType, ApplicantType, RoundOption, AdmittedorRejected, LoginId, roleId, ApplicatoinMode);
        }
        public List<InstituteWiseAdmission> DirectAdmissionApplicantDetailsBLL(int LoginId)
        {
            return _InstituteAdmission.DirectAdmissionApplicantDetailsDLL(LoginId);
        }
        public List<InstituteWiseAdmission> GetAdmissionApplicantsDistLoginBLL(AdmissionApplicantsDistLogin objAdmissionApplicantsDistLogin, int Id)
        {
            return _InstituteAdmission.GetAdmissionApplicantsDistLoginDLL(objAdmissionApplicantsDistLogin, Id);

        }
        public List<InstituteWiseAdmission> GetDataAllocationFeeDetailsBLL(int ApplicationId)
        {
            return _InstituteAdmission.GetDataAllocationFeeDetailsDLL(ApplicationId);
        }
        public List<ApplicantApplicationForm> GetApplicantDocumentFeeDetails(int ApplicationId)
        {
            var x =  _InstituteAdmission.GetApplicantDocumentFeeDetails(ApplicationId);
            foreach (var item in x)
            {
                if (item.DocVeriFeePymtDate != null)
                {
                    item.DocVeriFeePymtDatestring = item.DocVeriFeePymtDate.Value.ToString("dd/MM/yyyy");
                }
            }
            return x;
        }
        
        public List<InstituteWiseAdmission> GeneratePaymentReceiptBLL(InstituteWiseAdmission model)
        {
            return _InstituteAdmission.GeneratePaymentReceiptDLL(model);
        }

        public List<InstituteWiseAdmission> UpdateAdmittedDetailsBLL(InstituteWiseAdmission model)
        {
            return _InstituteAdmission.UpdateAdmittedDetailsDLL(model);
        }

        public List<InstituteWiseAdmission> GetCommentDetailsApplicantInstituteBLL(int ApplicationId)
        {
            return _InstituteAdmission.GetCommentDetailsApplicantInstituteDLL(ApplicationId);
        }

        public List<InstituteWiseAdmission> GeneratePaymentReceiptPDFBLL(int ApplicationId)
        {
            return _InstituteAdmission.GeneratePaymentReceiptPDFDLL(ApplicationId);
        }
        public List<InstituteWiseAdmission> GenerateDocumentPaymentReceiptPDFBLL(int ApplicationId)
        {
            return _InstituteAdmission.GenerateDocumentPaymentReceiptPDFBLL(ApplicationId);
        }
        public List<ApplicantDocumentsDetail> GetApplicantDocStatusDLL(int ApplicationId)
        {
            return _InstituteAdmission.GetApplicantDocStatusDLL(ApplicationId);
        }
        
        public List<InstituteWiseAdmission> GenerateAdmissionAcknowledgementPDFBLL(int ApplicationId)
        {
            return _InstituteAdmission.GenerateAdmissionAcknowledgementPDFDLL(ApplicationId);
        }
        public List<ApplicantDocumentsDetail> ApplicantDocumentDetailsInsBLL(ApplicantDocumentsDetail objApplicantDocumentsDetail)
        {
            return _InstituteAdmission.ApplicantDocumentDetailsInsDLL(objApplicantDocumentsDetail);
        }
        public List<ApplicantDocumentsDetail> ApplicantUpdateInstituteBLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            return _InstituteAdmission.ApplicantUpdateInstituteDLL(objApplicantApplicationForm);
        }

        //03-07-2021
        public List<UserDetails> GetsendBack(int id)
        {
            return _adDll.GetsendBack(id);
        }
        public int GetUserDivionIdBLL(int LoginId)
        {
            return _InstituteAdmission.GetUserDivionIdDLL(LoginId);
        }
        public List<UserDetails> GetForward(int id)
        {
            return _adDll.GetForward(id);
        }
        public string ApprovedRejectedList(InstituteWiseAdmission model, int loginId)
        {
            return _InstituteAdmission.ApprovedRejectedList(model, loginId);
        }
        public List<InstituteWiseAdmission> SentBackAdmittedListBLL(InstituteWiseAdmission model, int loginId, int sentId)
        {
            return _InstituteAdmission.SentBackAdmittedListDLL(model, loginId, sentId);
        }
        public string GetforwardAdmittedListBLL(InstituteWiseAdmission model, int loginId, int ForId)
        {
            return _InstituteAdmission.GetforwardAdmittedListDLL(model, loginId, ForId);
        }
        public string GetOnClickSendToHierarchyBLL(InstituteWiseAdmission model, int loginId, int ForId, string TabName)
        {
            return _InstituteAdmission.GetOnClickSendToHierarchyDLL(model, loginId, ForId, TabName);
        }
        public List<InstituteWiseAdmission> GetCommentDetailsRemarksByIdBLL(int ApplicationId)
        {
            return _InstituteAdmission.GetCommentDetailsRemarksByIdDLL(ApplicationId);
        }
        public List<InstituteWiseAdmission> GetCommentDetailsRemarks(int loginId, int ApplicationId)
        {
            return _InstituteAdmission.GetCommentDetailsRemarks(loginId, ApplicationId);
        }
        public string GetclickAddRemarksTransBLL(InstituteWiseAdmission modal, int loginId)
        {
            return _InstituteAdmission.GetclickAddRemarksTransDLL(modal, loginId);
        }


        #endregion

        #region officer
        public List<VerificationOfficer> GetOfficers(int loginId, int roleId)
        {
            return _officerMap.GetOfficers(loginId, roleId);
        }
        public List<VerificationOfficer> GetInstituteId(int loginId)
        {
            return _officerMap.GetInstituteId(loginId);
        }
        public string AddOfficer(VerificationOfficer officer, int loginId)
        {
            return _officerMap.AddOfficer(officer, loginId);
        }
        public VerificationOfficer EditOfficer(int id)
        {
            return _officerMap.EditOfficer(id);
        }
        public bool UpdateOfficer(VerificationOfficer officer, int loginId)
        {
            return _officerMap.UpdateOfficer(officer, loginId);
        }
        public bool DeleteOfficer(int id)
        {
            return _officerMap.DeleteOfficer(id);
        }
        public List<VerificationOfficer> GetApplicants(int loginId, int applicantId, int year)
        {
            return _officerMap.GetApplicants(loginId, applicantId, year);
        }
        public List<VerificationOfficer> GetVerificationOfficerDetails(int loginId, int year, int courseType, int roleId)
        {
            return _officerMap.GetVerificationOfficerDetails(loginId, year, courseType, roleId);
        }
        public VerificationOfficer GetTotalApplicantOfficer(int id, int roleId)
        {
            return _officerMap.GetTotalApplicantOfficer(id, roleId);
        }
        public bool MapApplicantToOfficer(int id, int roleId)
        {
            return _officerMap.MapApplicantToOfficer(id, roleId);
        }
        public List<VerificationOfficer> GetInactiveOfficerApplicants(int loginId, int year, int courseType)
        {
            return _officerMap.GetInactiveOfficerApplicants(loginId, year, courseType);
        }
        public bool ReMapApplicantToOfficer(int loginId, int roleId)
        {
            return _officerMap.ReMapApplicantToOfficer(loginId, roleId);
        }
        public List<VerificationOfficer> GetActiveOfficers(int loginId, int roleId)
        {
            return _officerMap.GetActiveOfficers(loginId, roleId);
        }
        public bool ReMapApplicantIndividualOff(int loginId, List<Applicants> list)
        {
            return _officerMap.ReMapApplicantIndividualOff(loginId, list);
        }
        #endregion

        #region dhanraj joined in rajgopal task
        public List<VerificationOfficer> GetApplicantsStatus(int loginId, int roleId)
        {
            return _adDll.GetApplicantsStatus(loginId, roleId);
        }
        public List<VerificationOfficer> GetApplicantsStatusFilter(int loginId, int roleId, int year, int courseType, int applicanType, int division_id, int district_lgd_code, int taluk_lgd_code, int InstituteId)
        {
            return _adDll.GetApplicantsStatusFilter(loginId, roleId, year, courseType, applicanType,  division_id,  district_lgd_code,  taluk_lgd_code, InstituteId);
        }
        public List<VerificationOfficer> GetApplicantsStatusBLL(int loginId, int roleId, int ApplicantType)
        {
            return _adDll.GetApplicantsStatusDLL(loginId, roleId, ApplicantType);

        }
        public List<VerificationOfficer> GetDataDocumentsVerificationFee(int loginId, int roleId, int? year, int? courseType, int? applicanType, int division_id, int district_lgd_code, int taluk_lgd_code, int InstituteId)
        {
            return _adDll.GetDataDocumentsVerificationFee(loginId, roleId, year, courseType, applicanType, division_id , district_lgd_code,  taluk_lgd_code,  InstituteId);
        }
        public List<VerificationOfficer> GetDataDocumentsVerificationFeepayment(int loginId, int roleId, int? year, int? courseType, int? applicanType)
        {
            return _adDll.GetDataDocumentsVerificationFeepayment(loginId, roleId, year, courseType, applicanType);
        }
        public List<VerificationOfficer> GetDataDocumentsVerificationFeeNotPaid(int loginId, int roleId, int? year, int? courseType, int? applicanType,string ApplNo)
        {
            return _adDll.GetDataDocumentsVerificationFeeNotPaid(loginId, roleId, year, courseType, applicanType,ApplNo);
        }
        
        public string GetReceiptNumber(InstituteWiseAdmission model, int ApplID, int rcpt )
        {
            return _InstituteAdmission.GetReceiptNumberGen(model, ApplID, rcpt);
        }
        public List<VerificationOfficer> GetVerificationFeeCmntDetailsByIdBLL(int ApplicationId)
        {
            return _adDll.GetVerificationFeeCmntDetailsByIdDLL(ApplicationId);
        }
        #endregion

        #region Grievances
        public List<VerificationOfficer> ApplicantRankDetails(int loginId, int roleId)
        {
            return _grievance.ApplicantRankDetails(loginId, roleId);
        }
        public List<VerificationOfficer> GetDocumentTypes()
        {
            return _grievance.GetDocumentTypes();
        }
        public string SubmitGrievanceTentative(List<HttpPostedFileBase> list, List<int> fileType, int loginId, string remarks, int roleId)
        {
            return _grievance.SubmitGrievanceTentative(list, fileType, loginId, remarks, roleId);
        }
        public List<VerificationOfficer> GetGrievanceTentativeStatus(int loginId, int roleId, int course, int year, int division, int district, int applicantType, int taluk, int institute)
        {
            return _grievance.GetGrievanceTentativeStatus(loginId, roleId, course, year, division, district, applicantType,taluk,institute);
        }
        public VerificationOfficer EditApplicantGrievance(int grivanceId, int roleId)
        {
            return _grievance.EditApplicantGrievance(grivanceId, roleId);
        }
        public bool VerifyGrievance(List<int> fileType, List<string> status, int grievanceId, string remark, int loginId, int roleId)
        {
            return _grievance.VerifyGrievance(fileType, status, grievanceId, remark, loginId, roleId);
        }
        public bool SendForCorrection(List<int> fileType, List<string> status, int grievanceId, string remark, int loginId, int roleId)
        {
            return _grievance.SendForCorrection(fileType, status, grievanceId, remark, loginId, roleId);
        }
        public bool UpdateGrievanceTentative(List<HttpPostedFileBase> list, List<int> fileType, List<string> status, int loginId, string remarks, int grievanceId, int roleId)
        {
            return _grievance.UpdateGrievanceTentative(list, fileType, status, loginId, remarks, grievanceId, roleId);
        }
        public VerificationOfficer GetGrievanceGrid(int loginId)
        {
            return _grievance.GetGrievanceGrid(loginId);
        }
        public bool RejectGrivance(int grivanceId, string remarks, int loginId, int roleId)
        {
            return _grievance.RejectGrivance(grivanceId, remarks, loginId, roleId);
        }
        public List<VerificationOfficer> GetGrievanceRemarks(int id)
        {
            return _grievance.GetGrievanceRemarks(id);
        }
        #endregion

        #region .. MeritList ..
        public List<AdmissionMeritList> GetCommentsMeritListFileBLL(int id)
        {
            return this._adDll.GetCommentsMeritListFileDLL(id);
        }
        public List<UserDetails> GetMeritRoles(int id, int level)
        {
            return _adDll.GetMeritRoles(id, level);
        }
        public List<AdmissionMeritList> GetMeritStatusListBLL(int id)
        {
            return _adDll.GetMeritStatusListDLL(id);
        }
        public List<AdmissionMeritList> GetDistricts(int Divisions)
        {
            return _adDll.GetDistricts(Divisions);
        }
        public List<AdmissionMeritList> GetDivision()
        {
            return _adDll.GetDivision();
        }
        public List<AdmissionMeritList> GetApplicantType()
        {
            return _adDll.GetApplicantType();
        }
        public List<AdmissionMeritList> GetApplicationMode_Bll()
        {
            return _adDll.GetApplicationMode_Dll();
        }
        public List<AdmissionMeritList> GetTraineeType()
        {
            return _adDll.GetTraineeType();
        }
        public List<ApplicationForm> GetOtherBoardsListBLL()
        {
            return _adDll.GetOtherBoardsListDLL();
        }
        public List<AdmissionMeritList> GetGradationType()
        {
            return _adDll.GetGradationType();
        }

        public List<AdmissionMeritList> GetGradationMeritListBLL(int generateId, int AcademicYear, int ApplicantTypeId, int round , int id)
        {
            return _adDll.GetGradationMeritListDLL(generateId, AcademicYear, ApplicantTypeId, round, id);
        }  
        public List<AdmissionMeritList> GetceckGradationTransTable(int generateId, int AcademicYear, int ApplicantTypeId, int id)
        {
            return _adDll.GetceckGradationTransTable(generateId, AcademicYear, ApplicantTypeId, id);
        }
        public List<AdmissionMeritList> GetReviewGradationMeritListBLL(int generateId, int AcademicYearReviewId, int ApplicantTypeId, int id)
        {
            return _adDll.GetReviewGradationMeritListDLL(generateId, AcademicYearReviewId, ApplicantTypeId, id);
        }
        public List<AdmissionMeritList> GetGradationMeritListDirBLL(int generateId, int AcademicYearDC, int ApplicantTypeId, int id)
        {
            return _adDll.GetGradationMeritListDirDLL(generateId, AcademicYearDC, ApplicantTypeId, id);
        }
        public List<AdmissionMeritList> GetGradationListBLL(string rbId, int id, int loginId)
        {
            return _adDll.GetGradationListDLL(rbId, id, loginId);
        }
        public List<AdmissionMeritList> GetIndexGradationMeritListBLL(string rbId)
        {
            return _adDll.GetIndexGradationMeritListDLL(rbId);
        }
        public List<AdmissionMeritList> GetIndexTentativeGradationMeritListBLL()
        {
            return _adDll.GetIndexTentativeGradationMeritListDLL();
        }
        public List<AdmissionMeritList> GetIndexFinalGradationMeritListBLL()
        {
            return _adDll.GetIndexFinalGradationMeritListDLL();
        }
        public List<AdmissionMeritList> GetGradationListStatusBLL(int id)
        {
            return _adDll.GetGradationListStatusDLL(id);
        }
        public List<AdmissionMeritList> GetGrdationListReviewDirComBLL(int id, int AcademicYearDC, int ApplicantTypeId)
        {
            return _adDll.GetGrdationListReviewDirComDLL(id, AcademicYearDC, ApplicantTypeId);
        }
        public List<AdmissionMeritList> GetGrdationListReviewADNewBLL(int id, int AcademicYearAD, int ApplicantTypeId)
        {
            return _adDll.GetGrdationListReviewADNewDLL(id, AcademicYearAD, ApplicantTypeId);
        }
        public List<AdmissionMeritList> GetGradationMeritListDirNewBLL(int AcademicYearDC, int ApplicantTypeId, int ApplicationId, int id)
        {
            return _adDll.GetGradationMeritListDirNewDLL(AcademicYearDC, ApplicantTypeId, ApplicationId, id);
        }
        public List<AdmissionMeritList> GetGradationMeritListDirNewRank(List<AdmissionMeritList> MaritList)
        {
            return _adDll.GetGradationMeritListDirNewRank(MaritList);
        }
        public List<AdmissionMeritList> GetGradationMeritAppListADNewIdBLL(int AcademicYearAD, int ApplicantTypeId, int ApplicationId, int id)
        {
            return _adDll.GetGradationMeritAppListADNewIdDLL(AcademicYearAD, ApplicantTypeId, ApplicationId, id);
        }
        public List<AdmissionMeritList> GetApplicantResultMarqBLL(int id, int loginId)
        {
            return _adDll.GetApplicantResultMarqDLL(id, loginId);
        }
        public List<AdmissionMeritList> GetMeritListstatusPopupBLL(int id, int loginId, int ApplicationId)
        {
            return _adDll.GetMeritListstatusPopupDLL(id, loginId, ApplicationId);
        }
        public string GetGenerateMeritListBLL(nestedMeritList model, int loginId, string remarks, int round)
        {
            return _adDll.GetGenerateMeritListDLL(model, loginId, remarks, round);
        }

        public string ForwardMeritListDDBLL(AdmissionMeritList model, int loginId, int roleId, string remarks, int round)
        {
            return _adDll.ForwardMeritListDDDLL(model, loginId, roleId, remarks, round);
        }
        public List<AdmissionMeritList> SendforDirectorBLL(AdmissionMeritList model, int loginId, int roleId, string remarks)
        {
            return _adDll.SendforDirectorDLL(model, loginId, roleId, remarks);
        }
        public string ApproveMeritListDDBLL(AdmissionMeritList model, int loginId, int roleId, string remarks)
        {
            return _adDll.ApproveMeritListDDDLL(model, loginId, roleId, remarks);
        }

        public List<AdmissionMeritList> viewMeritListBll(int generateId, int AcademicYear, int ApplicantTypeId, int round, int DivisionId, int DistrictId, int id)
        {
            return _adDll.viewMeritListDll(generateId, AcademicYear, ApplicantTypeId, round, DivisionId, DistrictId, id);
        }
        public string PublishMeritList(AdmissionMeritList model, int loginId, int Status, string remarks)
        {
            return _adDll.PublishMeritList(model, loginId, Status, remarks);
        }
        public string ChangesMeritListBLL(AdmissionMeritList model, int backId, int loginId, int Status, string remarks)
        {
            return _adDll.ChangesMeritListDLL(model, backId, loginId, Status, remarks);
        }
        public string SentBacktoDDMeritListBLL(AdmissionMeritList model, int loginId, int Status, int sentId, string remarks)
        {
            return _adDll.SentBacktoDDMeritListDLL(model, loginId, Status, sentId, remarks);
        }
        public List<AdmissionMeritList> GetPublishcalendarEvents(int id)
        {
            return _adDll.GetPublishcalendarEvents(id);
        }
        #endregion 

        #region .. Application Form for seat allocation ..

        public List<ApplicationForm> GetITICollegeDetailsBLL(int PinCode)
        {
            return this._adDll.GetITICollegeDetailsDLL(PinCode);
        }

        public List<ApplicationForm> GetITICollegeDetailsByDistrictTalukaBLL(int District, int Taluka)
        {
            return this._adDll.GetITICollegeDetailsByDistrictTalukaDLL(District, Taluka);
        }

        public List<ApplicationForm> GetITICollegeTradeDetailsBLL(int TradeCode, string qual)
        {
            return this._adDll.GetITICollegeTradeDetailsDLL(TradeCode, "");
        }

        public List<ApplicationForm> GetReligionDetailsBLL()
        {
            return this._adDll.GetReligionDetailsDLL();
        }
        public List<ApplicationForm> GetcoursetypeListbycalendar()
        {
            return this._adDll.GetcoursetypeListbycalendar();
        }

        public List<ApplicationForm> GetGenderDetailsBLL()
        {
            return this._adDll.GetGenderDetailsDLL();
        }

        public List<ApplicationForm> GetDocumentApplicationStatusBLL()
        {
            return this._adDll.GetDocumentApplicationStatusDLL();
        }

        public List<ApplicationForm> GetCategoryListBLL()
        {
            return this._adDll.GetCategoryListDLL();
        }

        public List<ApplicationForm> GetApplicantTypeListBLL(int roleId)
        {
            return this._adDll.GetApplicantTypeListDLL(roleId);
        }

        public List<ApplicationForm> GetCasteListBLL()
        {
            return this._adDll.GetCasteListDLL();
        }

        public List<PersonWithDisabilityCategory> PersonWithDisabilityCategoryBLL()
        {
            return this._adDll.PersonWithDisabilityCategoryDLL();
        }
        public List<ApplicationForm> GetReservationsListBLL()
        {
            return this._adDll.GetReservationsListDLL();
        }

        public List<ApplicationForm> GetSyllabusListBLL()
        {
            return this._adDll.GetSyllabusListDLL();
        }

        public List<ApplicationForm> GetQualificationListBLL()
        {
            return this._adDll.GetQualificationListDLL();
        }

        public List<ApplicationForm> GetDistrictMasterListBLL()
        {
            return this._adDll.GetDistrictMasterListDLL();
        }

        public List<ApplicationForm> GetDistrictMasterDivListBLL(int DivisionId)
        {
            return this._adDll.GetDistrictMasterDivListDLL(DivisionId);
        }

        public List<ApplicantApplicationForm> GetEligibleDateFrmCalenderEventsBLL()
        {
            return this._adDll.GetEligibleDateFrmCalenderEventsDLL();
        }

        public ApplicantApplicationForm GenerateApplicationNumberBLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            return this._adDll.GenerateApplicationNumberDLL(objApplicantApplicationForm);
        }

        public List<ApplicationForm> GetApplicantReservationListBLL(int loginId)
        {
            return this._adDll.GetApplicantReservationListDLL(loginId);
        }
        public List<HorizontalVerticalCategorycs> GetHorizontalCategoryBLL()
        {
            return this._InstituteAdmission.GetHorizontalCategoryDLL();
        }

        public int GetApplIdByApplicationNumberBLL(string ExistChkApplicationNumber)
        {
            return this._InstituteAdmission.GetApplIdByApplicationNumberDLL(ExistChkApplicationNumber);
        }

        public List<HorizontalVerticalCategorycs> GetShiftsDetailsBLL()
        {
            return this._InstituteAdmission.GetShiftsDetailsDLL();
        }

        public List<HorizontalVerticalCategorycs> GetUnitsDetailsBLL()
        {
            return this._InstituteAdmission.GetUnitsDetailsDLL();
        }

        public List<HorizontalVerticalCategorycs> GetVerticalCategoryBLL()
        {
            return this._InstituteAdmission.GetVerticalCategoryDLL();
        }

        public List<ApplicationForm> GetITICollegeDetailsMasterBLL(int District, int Taluka)
        {
            return this._InstituteAdmission.GetITICollegeDetailsMasterDLL(District, Taluka);
        }

        public ApplicantAdmiAgainstVacancy GetInstituteMasterBLL(int LoginId)
        {
            return this._InstituteAdmission.GetInstituteMasterDLL(LoginId);
        }

        public List<HorizontalVerticalCategorycs> GetInstituteTradeMasterBLL(int CollegeId)
        {
            return this._InstituteAdmission.GetInstituteTradeMasterDLL(CollegeId);
        }
        public List<ApplicationForm> GetUnitsShiftsDetailsBLL(int CollegeId, int TradeId)
        {
            return this._InstituteAdmission.GetUnitsShiftsDetailsDLL(CollegeId, TradeId);
        }

        public List<ApplicantAdmiAgainstVacancy> UpdateApplicantAdmissionAgainstVacancyBLL(ApplicantAdmiAgainstVacancy objApplicantAdmiAgainstVacancyList)
        {
            return this._InstituteAdmission.UpdateApplicantAdmissionAgainstVacancyDLL(objApplicantAdmiAgainstVacancyList);
        }

        public List<ApplicationForm> GetLocationListBLL()
        {
            return _adDll.GetLocationListDLL();
        }

        public List<ApplicationForm> GetTalukMasterListBLL(int distId)
        {
            return _adDll.GetTalukMasterListDLL(distId);
        }

        public List<ApplicantApplicationForm> InsertApplicantFormDetailsBLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            return _adDll.InsertApplicantFormDetailsDLL(objApplicantApplicationForm);
        }

        public List<ApplicantDocumentsDetail> UpdateApplicantFormDetailsBLL(ApplicantDocumentsDetail objApplicantDocumentsDetail)
        {
            return _adDll.UpdateApplicantFormDetailsDLL(objApplicantDocumentsDetail);
        }

        public List<ApplicantApplicationForm> SaveEducationDetailsBLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            return _adDll.SaveEducationDetailsDLL(objApplicantApplicationForm);
        }

        public List<ApplicantApplicationForm> SaveAddressDetailsBLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            return _adDll.SaveAddressDetailsDLL(objApplicantApplicationForm);
        }

        public List<ApplicantApplicationForm> SaveInstitueDetailsBLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            return _adDll.SaveInstitueDetailsDLL(objApplicantApplicationForm);
        }

        public List<ApplicantApplicationForm> InsertPaymentDetailsBLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            return _adDll.InsertPaymentDetailsDLL(objApplicantApplicationForm);
        }

        public List<ApplicantDocumentsDetail> ApplicantDocumentDetailsBLL(ApplicantDocumentsDetail objApplicantDocumentsDetail)
        {
            return _adDll.ApplicantDocumentDetailsDLL(objApplicantDocumentsDetail);
        }


        public List<ApplicantDocumentsDetail> ApplicantDocumentDetailsTransBLL(ApplicantDocumentsDetail objApplicantDocumentsDetail)
        {
            return _adDll.ApplicantDocumentDetailsTransDLL(objApplicantDocumentsDetail);
        }

        public List<GrievanceDocApplData> ApplicantGrievanceDetailsBLL(GrievanceDocApplData objGrievanceDocApplData)
        {
            return _adDll.ApplicantGrievanceDetailsDLL(objGrievanceDocApplData);
        }

        public List<GrievanceDocApplData> ApplicantGrievanceDocumentDetailsBLL(GrievanceDocApplData objGrievanceDocApplData)
        {
            return _adDll.ApplicantGrievanceDocumentDetailsDLL(objGrievanceDocApplData);
        }

        public ApplicantApplicationForm GetMasterApplicantDataBLL(int loginId, string DataFrom)
        {
            return _adDll.GetMasterApplicantDataDLL(loginId, DataFrom);
        }
        public ApplicantApplicationForm GetUserMasterDataBLL(int loginId, string DataFrom)
        {
            return _adDll.GetUserMasterDataDLL(loginId, DataFrom);
        }
        public List<ApplicantDocumentsDetail> GetDocumentDetailsBLL(int loginId)
        {
            return _adDll.GetDocumentDetailsDLL(loginId);
        }

        public List<ApplicantInstitutePreference> GetApplicantInstitutePreferenceBLL(int loginId)
        {
            return _adDll.GetApplicantInstitutePreferenceDLL(loginId);
        }

        public ApplicantApplicationForm GetAdmissionDocumentDetailsBLL(int ApplicationId)
        {
            return _adDll.GetAdmissionDocumentDetailsDLL(ApplicationId);
        }

        public List<ApplicantInstitutePreference> AddInstituePreferenceDetailsBLL(ApplicantInstitutePreference objApplicantInstitutePreference)
        {
            return _adDll.AddInstituePreferenceDetailsDLL(objApplicantInstitutePreference);
        }

        public ApplicantApplicationForm UpdateNextRoundDetailsWithTransBLL(ApplicantApplicationForm objApplicantInstitutePreference)
        {
            return _adDll.UpdateNextRoundDetailsWithTransDLL(objApplicantInstitutePreference);
        }

        public ApplicationStatusUpdate UpdateApplicationDetailsByIdBLL(ApplicationStatusUpdate objApplicationStatusUpdate)
        {
            return _adDll.UpdateApplicationDetailsByIdDLL(objApplicationStatusUpdate);
        }

        public ApplicantDocumentsDetail UpdateGrievanceApplicationDetailsByIdBLL(ApplicantDocumentsDetail objApplicantDocumentsDetail)
        {
            return _adDll.UpdateGrievanceApplicationDetailsByIdDLL(objApplicantDocumentsDetail);
        }

        public List<ApplicantApplicationForm> GetApplicantRemarksListBLL(int ApplicantTransId)
        {
            return this._adDll.GetApplicantRemarksListDLL(ApplicantTransId);
        }

        #endregion

        #region .. 

        #endregion



        #region usecase 1-3 Seat Availablity
        public List<SeatDetails> SeatAvailabilityStatus(int roleId)
        {
            return _seatAvailability.SeatAvailabilityStatus(roleId);
        }
        public List<UserDetails> GetUserRoles(int id, int level, int roleId)
        {
            return _seatAvailability.GetUserRoles(id, level, roleId);
        }
        public List<SeatAvailabilty> GetSeatAvailabilityList(int loginId, int courseCode, string AcademicYear)
        {
            return _seatAvailability.GetSeatAvailabilityList(loginId, courseCode, AcademicYear);
        }
        public List<SeatAvailabilty> GetSeatAvailabilityListAdd(int loginId,string miscode)
        {
            return _seatAvailability.GetSeatAvailabilityListAdd(loginId, miscode);
        }
        public List<SeatAvailabilty> GetSeatTypes()
        {
            return _seatAvailability.GetSeatTypes();
        }
        public int GetSeatsByTradeIdSeatType(int tradeId)
        {
            return _seatAvailability.GetSeatsByTradeIdSeatType(tradeId);
        } 
        public List<SeatAvailabilty> GetSeatsBySeatTypeRules(int seattypeId, int tradeId)
        {
            return _seatAvailability.GetSeatsBySeatTypeRules(seattypeId, tradeId);
        }
        public string SaveSeatAvailability(List<SeatDetails> seat, int loginId, int roleId)
        {
            return _seatAvailability.SaveSeatAvailability(seat, loginId, roleId);
        }
        public bool ForwardSeatAvailability(List<SeatDetails> seat, int loginId, int roleId)
        {
            return _seatAvailability.ForwardSeatAvailability(seat, loginId, roleId);
        }
        public List<SeatDetails> GetRemarks(int seatId)
        {
            return _seatAvailability.GetRemarks(seatId);
        }
        public bool ApproveSeatAvailability(List<SeatDetails> seat, int loginId, int roleId)
        {
            return _seatAvailability.ApproveSeatAvailability(seat, loginId, roleId);
        }
        //public bool RejectSeatAvailability(List<SeatDetails> seat, int loginId)
        //{
        //    return _seatAvailability.RejectSeatAvailability(seat, loginId);
        //}
        public List<SeatAvailabilty> GetSeatViewDetails(int seatId)
        {
            var res = _seatAvailability.GetSeatViewDetails(seatId);
            foreach (var itm in res)
            {
                if (itm.DualSystem == true)
                {
                    itm.DualSystemTraining = "dual";
                }
                else
                {
                    itm.DualSystemTraining = "regular";
                }
            }
            return res;
        }
        public bool GetdelUnitShiftDetails(int seatId, int loginId, int roleId)
        {
            return _seatAvailability.GetdelUnitShiftDetails(seatId, loginId, roleId);              
        }
        public bool GetdeActiveSeatDetails(int seatId,int TradeItiId, int loginId, int roleId)
        {
            return _seatAvailability.GetdeActiveSeatDetails(seatId, TradeItiId, loginId, roleId);              
        }
        public bool UpdateSeatAvailability(SeatDetails seat, int loginId, int roleId)
        {
            return _seatAvailability.UpdateSeatAvailability(seat, loginId, roleId);
        }
        public List<DistrictTalukDetails> GetRegionDistrictCities(int loginId)
        {
            return _seatAvailability.GetRegionDistrictCities(loginId);
        }
        public List<DistrictTalukDetails> GetTaluks(int distilgdCOde)
        {
            return _seatAvailability.GetTaluks(distilgdCOde);
        }
        public List<DistrictTalukDetails> GetInstitutes(int distilgdCOde)
        {
            return _seatAvailability.GetInstitutes(distilgdCOde);
        }
        public List<SeatAvailabilty> GetSeatAvailabilityListStatusFilter(int TabId, int Course_Id, int Year_id, int roleId, int loginId, int Division_Id, int District_Id, int taluk_id, int Insttype_Id)
        {
            return _seatAvailability.GetSeatAvailabilityListStatusFilter(TabId, Course_Id, Year_id, roleId, loginId, Division_Id, District_Id, taluk_id, Insttype_Id);
        }
        //public List<SeatAvailabilty> GetIndexseatavailabilityList()
        //{
        //    return _seatAvailability.GetIndexseatavailabilityList();
        //}
        #endregion

        #region Seat allotment
        public List<SeatAutoAllocationModel> GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round, int RoleId, int LoginID)
        {
            //if (round == 1)
            {
                return _seatAllocation.Round1GenerateSeatAutoAllotment(courseType, applicantType, academicYear, round, RoleId, LoginID);
            }
            /*else if (round == 2)
            {
                return _seatAllocation.Round1GenerateSeatAutoAllotment(courseType, applicantType, academicYear, round, RoleId, LoginID);
                
            }
            else if (round == 3 || round == 4 || round == 5)
            {
                return _seatAllocation.Round3_4GenerateSeatAutoAllotment(courseType, applicantType, academicYear, round, RoleId);             
            }
            else
            {
                return _seatAllocation.Round6GenerateSeatAutoAllotment(courseType, applicantType, academicYear, round, RoleId);
            }*/

        }
        public bool ForwardSeatAutoAllotment(List<int> seat, int loginId, int roleId, string Remarks, int Status)
        {
            return _seatAllocation.ForwardSeatAutoAllotment(seat, loginId, roleId, Remarks, Status);
        }
        public List<SeatAutoAllocationModel> GetGeneratedSeatAutoAllotmentList(int courseType, int applicantType, int academicYear, int round)
        {
            return _seatAllocation.GetGeneratedSeatAutoAllotmentList(courseType, applicantType, academicYear, round);
        }
        public List<SeatAutoAllocationModel> ViewSeatAutoAllotment(int allocationId)
        {
            return _seatAllocation.ViewSeatAutoAllotment(allocationId);
        }
        #endregion


        public string RuleAllocationChkExistenceBLL(SeatAllocation objSeatAllocation)
        {
            return _adDll.RuleAllocationChkExistenceDLL(objSeatAllocation);
        }

        #region Seat Allocation Review & Recommand Of Seat Matrix
        //ram use case-35

        public List<ReviewSeatAllocated> GeneratedSeatAllotmentReviewBLL(int ddlCourseTypesGen, int ddlApplicantTypeGen, int ddlAcademicYearGen, int ddlRoundGen, int id)
        {
            return _seatAllocation.GenerateSeatAllotmentReviewDLL(ddlCourseTypesGen, ddlApplicantTypeGen, ddlAcademicYearGen, ddlRoundGen, id);
        }

        public List<SeatMatrixAllocationDetail> GeneratedSeatAllotmentReviewListBLL(int id)
        {
            return _seatAllocation.GeneratedSeatAllotmentReviewListDLL(id);
        }

        public List<SeatAutoAllocationModel> GetSeatMatrixViewListBLL(int courseType, int applicantType, int academicYear, int round)
        {
            return _seatAllocation.GetSeatMatrixViewListDLL(courseType, applicantType, academicYear, round);
        }

        public bool ForwardSeatAutoAllotmentReviewBLL(List<int> seat, int loginId, int roleId)
        {
            return _seatAllocation.ForwardSeatAutoAllotmentReviewDLL(seat, loginId, roleId);
        }
        public List<SeatMatrixAllocationDetail> GetCommentsListSeatAllocationBLL(int SeatAllocationId)
        {
            return _seatAllocation.GetCommentsListSeatAllocationDLL(SeatAllocationId);
        }
        public string UpdtApplITIInstDetailsBLL(int ApplicantId, int LoginID,int roleID, int? allocId)
        {
            return _seatAllocation.UpdtApplITIInstDetailsDLL(ApplicantId, LoginID, roleID, allocId);
        }
        public string DirectAdmissionSeatAllotmentBLL(InstituteWiseAdmission model)
        {
            return _seatAllocation.DirectAdmissionSeatAllotmentDLL(model);
        }

        #endregion

        public bool CheckNameAvailabilityBLL(string strName, int ApplicationId, int AadhaarRollNumber)
        {
            return _adDll.CheckNameAvailabilityDLL(strName, ApplicationId, AadhaarRollNumber);
        }
        public bool CheckPhoneNumberAvailabilityBLL(string strName, int ApplicationId, int AadhaarRollNumber)
        {
            return _adDll.CheckPhoneNumberAvailabilityDLL(strName, ApplicationId, AadhaarRollNumber);
        }
        public bool CheckEmailIdAvailabilityBll(string strName, int ApplicationId, int AadhaarRollNumber)
        {
            return _adDll.CheckEmailIdAvailabilityDll(strName, ApplicationId, AadhaarRollNumber);
        }

        #region AdmissionRegister
        public List<AdmissionRegister> GetViewAdmissionRegisterDemo(int id, int? coursetype, int? session, int? division, int? district, int? Institute,int? InstType)
        {
            return _adDll.GetViewAdmissionRegisterDemo(id, coursetype, session, division, district, Institute, InstType);
        }
        public List<AdmissionRegister> GetDistrictsJD()
        {
            return _adDll.GetDistrictsJD();
        }
        public List<AdmissionRegister> GetAppRound(int roundType)
        {
            return _adDll.GetAppRound(roundType);
        }
        public List<AdmissionRegister> GetAppRoundReg(int applicantType, int? roundType)
        {
            return _adDll.GetAppRoundReg(applicantType, roundType);
        }
        public List<DistrictTalukDetails> GetInstitutesReg(int District)
        {
            return _adDll.GetInstitutesReg(District);
        }
        public List<AdmissionRegister> GetInstTypeDetails()
        {
            return this._adDll.GetInstTypeDetails();
        }
        public List<ApplicationForm> GetStateListDetails()
        {
            return this._adDll.GetStateListDetails();
        }
        #region comment
        //public List<AdmissionRegister> GetViewAdmissionRegister(int collegeId, int courseType, int academicYear, int roleId, int loginId, int NotificationId)
        //{
        //    return _adDll.GetViewAdmissionRegister(collegeId, courseType, academicYear, roleId, loginId, NotificationId);
        //}
        #endregion comment

        #endregion
        #region Transfer admission Seat
        public List<ApplicantTransferModel> GetAdmittedData(int loginId, int session, int course, int? trade, int round)
        {
            return _transfer.GetAdmittedData(loginId, session, course, trade, round);
        }

        public List<ApplicantTransferModel> GetRequestedDetails(int loginId, int session, int course, int trade)
        {
            return _transfer.GetRequestedDetails(loginId, session, course, trade); ;
        }
        public List<Trades> GetTrades(int loginId)
        {
            return _transfer.GetTrades(loginId);
        }
        public string GetMisCode(int instiId)
        {
            return _transfer.GetMisCode(instiId);
        }
        public List<Trades> GetAvailableseatsTrades(int instiId)
        {
            return _transfer.GetAvailableseatsTrades(instiId);
        }
        public List<Trades> GetUnits(int instiId, int trade)
        {
            return _transfer.GetUnits(instiId, trade);
        }
        public List<Trades> GetShifts(int instiId, int trade, int unit)
        {
            return _transfer.GetShifts(instiId, trade, unit);
        }
        public string GetDualSystem(int insti, int trade)
        {
            return _transfer.GetDualSystem(insti, trade);
        }
        public List<Trades> GetTranseferInstitutes(int type, int taluklgd)
        {
            return _transfer.GetTranseferInstitutes(type, taluklgd);
        }
        public string SubmitAdmittedData(List<ApplicantTransferModel> seat, int loginId, int roleId)
        {
            return _transfer.SubmitAdmittedData(seat, loginId, roleId);
        }
        public List<Trades> GetInstituteTypes()
        {
            return _transfer.GetInstituteTypes();
        }
        public List<Trades> GetInstituteNames(int type)
        {
            return _transfer.GetInstituteNames(type);
        }
        public bool ForwardAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId, int flowId)
        {
            return _transfer.ForwardAdmittedData(transSeatId, status, remarks, loginId, roleId, flowId);
        }
        public List<ApplicantTransferModel> GetTransferRemarks(int seatId)
        {
            return _transfer.GetTransferRemarks(seatId);
        }
        public bool ApproveAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId)
        {
            return _transfer.ApproveAdmittedData(transSeatId, status, remarks, loginId, roleId);
        }
        public bool SendBackAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId)
        {
            return _transfer.ApproveAdmittedData(transSeatId, status, remarks, loginId, roleId);
        }
        public List<ApplicantTransferModel> GetAdmittedDataStatus(int loginId, int roleId)
        {
            return _transfer.GetAdmittedDataStatus(loginId, roleId);
        }
        public List<ApplicantTransferModel> GetApplicantTransferbyList(int loginId, int roleId)
        {
            return _transfer.GetApplicantTransferbyList(loginId, roleId);
        }
        public List<ApplicantTransferModel> GetApprovedTransferbyList(int loginId, int roleId)
        {
            return _transfer.GetApprovedTransferbyList(loginId, roleId);
        }
        public List<ApplicantTransferModel> GetApplicantInstituteDetails(int id)
        {
            return _transfer.GetApplicantInstituteDetails(id);
        }
        public bool UpdateApplicantInstituteDetails(ApplicantTransferModel tran, int roleId)
        {
            return _transfer.UpdateApplicantInstituteDetails(tran, roleId);
        }
        public List<SeatDetails> SeatTransferStatus(int roleId)
        {
            return _transfer.SeatTransferStatus(roleId);
        }
        #endregion

        public List<AdmissionUnitsShifts> GetAdmissionUnitsDetailsDLL()
        {
            return _adDll.GetAdmissionUnitsDetailsDLL();
        }
        public List<AdmissionUnitsShifts> GetAdmissionShiftsDetailsDLL()
        {
            return _adDll.GetAdmissionShiftsDetailsDLL();
        }
        public bool InsertNewApplicantRegistrationDetailsBLL(NewApplicant objNewApplicant)
        {
          return _adDll.InsertNewApplicantRegistrationDetailsDLL(objNewApplicant);
        }
        public bool InsertNewEmployeeRegistrationDetailsBLL(NewEmployee objNewApplicant)
        {
            return _adDll.InsertNewEmployeeRegistrationDetailsDLL(objNewApplicant);
        }
        public bool CheckApplicantExistorNot(string ApplicantMobileNo, string email)
        {
          return _adDll.CheckApplicantExistorNot(ApplicantMobileNo, email);
        }
    public ApplicantApplicationForm GetApplicantApplicationForm()
    {
      return _adDll.GetApplicantApplicationForm();
    }
    public bool IsUserLoggedOnElsewhere(string userId, string sid)
    {
      return _adDll.IsUserLoggedOnElsewhere(userId, sid);
    }

        public ApplicantApplicationForm GetValidateRDNumberBll(string RD_Number, int loginId, int RDNumberType)
        {
            return _InstituteAdmission.GetValidateRDNumberDll(RD_Number, loginId, RDNumberType);
        }
    }
}
