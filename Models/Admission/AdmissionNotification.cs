using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Models.Admission
{
    public class AdmissionNotification
    {
        public int Admission_Notif_Id { get; set; }

        [Required(ErrorMessage = "Notification Number is required")]
        public string Exam_Notif_Number { get; set; }
        public string Exam_Notif_Desc { get; set; }
        [Required(ErrorMessage = "Notification Date is required")]
        public DateTime Exam_notif_date { get; set; }
        public string exam_notif_type { get; set; }
        public DateTime? fee_pay_last_date { get; set; }
        public DateTime? appli_from_last_date { get; set; }
        public DateTime? trainee_name_eval_last_date { get; set; }
        public DateTime? princ_sub_last_date { get; set; }
        public DateTime? jd_sub_last_date { get; set; }
        public int? appli_charges_fee { get; set; }
        public int? exam_regular_fee { get; set; }
        public int? exam_repeater_fee { get; set; }
        public int? penalty_after10days_fee { get; set; }
        public int? penalty_after30days_fee { get; set; }
        public int? penalty_after40days_fee { get; set; }
        public bool? is_active { get; set; }
        public DateTime? creation_datetime { get; set; }
        public DateTime? updation_datetime { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public int role_id { get; set; }
        public int login_id { get; set; }
        public int updatestatusCW { get; set; }
        //Course Type Master
        [Required(ErrorMessage = "Course Type is required")]
        public int CourseTypeId { get; set; }
        [Required(ErrorMessage = "Notification Type is required")]
        public int? applicantTypeId { get; set; }
        public int DeptId { get; set; }
        public int NotifDescId { get; set; }
        public string NotifDescName { get; set; }
        public string CourseTypeName { get; set; }
        public string applicantTypeName { get; set; }
        public string DeptName { get; set; }
        public int slno { get; set; }
        public SelectList CourseList { get; set; }
        public SelectList ApplicantTypeList { get; set; }

        public SelectList DeptList { get; set; }
        public SelectList LoginRoleList { get; set; }
        public SelectList NotifDescList { get; set; }
        public string NotifDesc { get; set; }
        [Required(ErrorMessage = "Please enter the Email ID")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please enter correct Email address")]
        public string NtfEmailId { get; set; }
        public List<AdmissionNotification> notifications { get; set; }
        public List<AdmissionNotification> GetUpdateNotifDet { get; set; }
        public List<AdmissionNotification> GeAdmissionNotifDet { get; set; }
        public List<AdmissionNotification> NotificationList { get; set; }
        public bool toPDF { get; set; }
        public HttpPostedFileBase file { get; set; }

        //Status Type Master
        public int? exam_notif_status_id { get; set; }
        public string exam_notif_status_desc { get; set; }
        public int PublishedId { get; set; }
        public string createdatetime { get; set; }
        //tbl_Exam_Notification_Trans

        public int? Exam_Notif_Trans_Id { get; set; }
        // public int Exam_Notif_Id { get; set; }


        public int user_id { get; set; }
        public string user_role { get; set; }
        public int RoleId { get; set; }
        public int SelectedRoleId { get; set; }
        public string user_name { get; set; }
        [AllowHtml]
        public string content { get; set; }
        public string SavePath { get; set; }
        public int? selectTab { get; set; }
        public string comments { get; set; }
        public int modeuleId { get; set; }
        public int IsForward { get; set; }
        public string creationdatetime { get; set; }
        public string createdbyuser { get; set; }
        public string updatedbyuser { get; set; }
        public string RecordLevel { get; set; }

        public int? StatusId { get; set; }
        //public int SlNo { get; set; }

        #region Calendar
        //calendar
        public int Admsn_tentative_calndr_transId { get; set; }
        public int Tentative_admsn_evnt_clndr_Id { get; set; }
        public DateTime Trans_date { get; set; }
        public int? Status_id { get; set; }
        public string Remarks { get; set; }
        public int? FlowId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string RoleDesc { get; set; }

        public int Tentative_admsn_evnt_clndrId { get; set; }
        public DateTime? Notification_Date { get; set; }
        public int? CourseId { get; set; }
        public string Admsn_ntf_num { get; set; }
        //public int? Notif_decrid { get; set; }
        public string Admsn_notif_doc { get; set; }
        public int session { get; set; }
        public int admissionNotificationId { get; set; }
        public int YearID { get; set; }
        public string Year { get; set; }
        public string sessionYear { get; set; }
        public string admissionNotificationNumber { get; set; }
        public string statusRoleDescription { get; set; }
        //tbl_Tentative_admsn_eventDetails
        public int Calendar_EventId { get; set; }
        public DateTime? FromDt_DateofNotification { get; set; }
        public DateTime? ToDt_DateofNotification { get; set; }
        public DateTime? FromDt_Tentatviveseat { get; set; }
        public DateTime? ToDt_Tentatviveseat { get; set; }
        public DateTime? FromDt_ApplDocVerif { get; set; }
        public DateTime? ToDt_ApplDocVerif { get; set; }
        public DateTime? VacancySeat_nxtrnd { get; set; }
        public DateTime? FromDt_Fnlseatmtrx { get; set; }
        public DateTime? ToDt_Fnlseatmtrx { get; set; }
        public DateTime? FromDt_TntGrlistbyAppl { get; set; }
        public DateTime? ToDt_TntGrlistbyAppl { get; set; }
        public DateTime? FromDt_AplGrVRoffcr { get; set; }
        public DateTime? ToDt_AplGrVRoffcr { get; set; }
        public DateTime? Publishgrdlist1Rnd { get; set; }
        public DateTime? Publishgrdlist2Rnd { get; set; }
        public DateTime? Publishgrdlist3Rnd { get; set; }
        public DateTime? Rnd2seatAllotment { get; set; }
        public DateTime? FromDt_ApplyingOnlineApplicationForm { get; set; }
        public DateTime FromDt_ApplyingOnlineApplicationForm_pdf { get; set; }
        public DateTime? ToDt_ApplyingOnlineApplicationForm { get; set; }
        public DateTime ToDt_ApplyingOnlineApplicationForm_pdf { get; set; }
        public DateTime? FromDt_DocVerificationPeriod { get; set; }
        public DateTime FromDt_DocVerificationPeriod_pdf { get; set; }
        public DateTime? ToDt_DocVerificationPeriod { get; set; }
        public DateTime ToDt_DocVerificationPeriod_pdf { get; set; }


        public DateTime? Dt_DisplayEigibleVerifiedlist { get; set; }
        public DateTime Dt_DisplayEigibleVerifiedlist_pdf { get; set; }
        public DateTime? Dt_DBBackupSeatMatrixFInalByDept { get; set; }
        public DateTime Dt_DBBackupSeatMatrixFInalByDept_pdf { get; set; }
        public DateTime? Dt_DisplayTentativeGradation { get; set; }
        public DateTime Dt_DisplayTentativeGradation_pdf { get; set; }
        public DateTime? Dt_DisplayFinalGradationList { get; set; }
        public DateTime Dt_DisplayFinalGradationList_pdf { get; set; }
        public DateTime? Dt_1stListSeatAllotment { get; set; }
        public DateTime Dt_1stListSeatAllotment_pdf { get; set; }
        public DateTime? FromDt_1stRoundAdmissionProcess { get; set; }
        public DateTime FromDt_1stRoundAdmissionProcess_pdf { get; set; }
        public DateTime? ToDt_1stRoundAdmissionProcess { get; set; }
        public DateTime ToDt_1stRoundAdmissionProcess_pdf { get; set; }

        public DateTime? Dt_1stListAdmittedCand { get; set; }
        public DateTime Dt_1stListAdmittedCand_pdf { get; set; }
        public DateTime? FromDt_2ndRoundEntryChoiceTrade { get; set; }
        public DateTime FromDt_2ndRoundEntryChoiceTrade_pdf { get; set; }
        public DateTime? ToDt_2ndRoundEntryChoiceTrade { get; set; }
        public DateTime ToDt_2ndRoundEntryChoiceTrade_pdf { get; set; }
        public DateTime? FromDt_DbBkp2ndRoundOnlineSeat { get; set; }
        public DateTime FromDt_DbBkp2ndRoundOnlineSeat_pdf { get; set; }
        public DateTime? ToDt_DbBkp2ndRoundOnlineSeat { get; set; }
        public DateTime ToDt_DbBkp2ndRoundOnlineSeat_pdf { get; set; }


        public DateTime? FromDt_2ndRoundAdmissionProcess { get; set; }
        public DateTime FromDt_2ndRoundAdmissionProcess_pdf { get; set; }
        public DateTime? ToFDt_2ndRoundAdmissionProcess { get; set; }
        public DateTime ToFDt_2ndRoundAdmissionProcess_pdf { get; set; }
        public DateTime? Dt_2ndAdmittedCand { get; set; }
        public DateTime Dt_2ndAdmittedCand_pdf { get; set; }
        public DateTime? FromDt_3rdRoundEntryChoiceTrade { get; set; }
        public DateTime FromDt_3rdRoundEntryChoiceTrade_pdf { get; set; }
        public DateTime? ToDt_3rdRoundEntryChoiceTrade { get; set; }
        public DateTime ToDt_3rdRoundEntryChoiceTrade_pdf { get; set; }

        public DateTime? Dt_DbBkp3rdRoundOnlineSeat { get; set; }
        public DateTime Dt_DbBkp3rdRoundOnlineSeat_pdf { get; set; }
        public DateTime? FromDt_3rdRoundAdmissionProcess { get; set; }
        public DateTime FromDt_3rdRoundAdmissionProcess_pdf { get; set; }
        public DateTime? ToDt_3rdRoundAdmissionProcess { get; set; }
        public DateTime ToDt_3rdRoundAdmissionProcess_pdf { get; set; }
        public DateTime? Dt_3rdAdmittedCand { get; set; }
        public DateTime Dt_3rdAdmittedCand_pdf { get; set; }

        public DateTime? FromDt_FinalRoundEntryChoiceTrade { get; set; }
        public DateTime FromDt_FinalRoundEntryChoiceTrade_pdf { get; set; }
        public DateTime? ToDt_FinalRoundEntryChoiceTrade { get; set; }
        public DateTime ToDt_FinalRoundEntryChoiceTrade_pdf { get; set; }
        public DateTime? Dt_FinalRoundSeatAllotment { get; set; }
        public DateTime Dt_FinalRoundSeatAllotment_pdf { get; set; }
        public DateTime? FromDt_AdmissionFinalRound { get; set; }
        public DateTime FromDt_AdmissionFinalRound_pdf { get; set; }
        public DateTime? ToDt_AdmissionFinalRound { get; set; }
        public DateTime ToDt_AdmissionFinalRound_pdf { get; set; }
        public DateTime? Dt_CommencementOfTraining { get; set; }
        public DateTime Dt_CommencementOfTraining_pdf { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? Dt_Notification { get; set; }
        [Required(ErrorMessage = "Admission Notification No required ")]
        public int Admsn_notif_Id { get; set; }
        public string AdmsnNtfNum { get; set; }
        //******//
        public SelectList SessionList { get; set; }
        public SelectList AdmissionNotifNoList { get; set; }


        public List<AdmissionNotification> GetUpdateCalendarfDet { get; set; }
        public int ForwardId { get; set; }
        public bool forwardStatus { get; set; }
        public bool SendbackStatus { get; set; }
        public bool ApprovedStatus { get; set; }
        public bool PublishStatus { get; set; }
        public bool ChangesStatus { get; set; }
        public string Role { get; set; }

        //Status Type Master                                
        public int Admsn_tentative_calndr_transHisId { get; set; }
        public string To { get; set; }
        public string Status { get; set; }
        public string FromUser { get; set; }

        //public string role_DescShortForm { get; set; }
        #endregion Calendar
    }
}
