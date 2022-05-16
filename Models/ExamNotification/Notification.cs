using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Models.ExamNotification
{
    public class Notification
    {
        public string filename { get; set; }
        public string Docfilename { get; set; }
        public int Exam_Notif_Id { get; set; }
		//[Required(ErrorMessage ="Please Enter the Notification Number")]
        public string Exam_Notif_Number { get; set; }
        public int Exam_Cal_Notif_Number { get; set; }
        public int applicantTypeId { get; set; }
        public string Exam_Notif_Desc { get; set; }
		public DateTime Exam_notif_date { get; set; }
        public string exam_notif_type { get; set; }
        public DateTime fee_pay_last_date { get; set; }
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
        public int? role_id { get; set; }
        public int? login_id { get; set; }
		public int updatestatusCW { get; set; }
        public HttpPostedFileBase UploadPdf { get; set; }
        public HttpPostedFileBase UploadDoc { get; set; }
        public string NotificationSavePath { get; set; }

        public string To { get; set; }
        public string FromUser { get; set; }

        //Course Type Master
        public int CourseTypeId { get; set; }
        public int DeptId { get; set; }
        public int NotifDescId { get; set; }
        public string NotifDescName { get; set; }
        public string getSignedPDFPath { get; set; }
        public string CourseTypeName { get; set; }
        public string DeptName { get; set; }

        public SelectList CourseList { get; set; }

        public SelectList DeptList { get; set; }
		public SelectList LoginRoleList { get; set; }
        public SelectList NotifDescList { get; set; }
        public List<Notification> notifications { get; set; }
        public List<Notification> GetUpdateNotifDet { get; set; }
        public List<Notification> NotificationList { get; set; }

        //Status Type Master
        public int exam_notif_status_id { get; set; }
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
        public string DocSavePath { get; set; }
        public int? selectTab { get; set; }
		 public string comments { get; set; } 
		public int modeuleId { get; set; }
		public int IsForward { get; set; }
        public string creationdatetime { get; set; }
		public string createdbyuser { get; set; }
		public string updatedbyuser { get; set; }
		public string RecordLevel { get; set; }

        public int Admsn_notif_Id { get; set; }
        public int SlNo { get; set; }
        public string CourseType { get; set; }
        public string DepartmentName { get; set; }
        public string NotificationNumber{ get; set; }
        public string Description{ get; set; }
        public DateTime? AddDate{ get; set; }
        public string AddDateString { get; set; }
        public string Status{ get; set; }
        public int ForwardId { get; set; }
        public bool forwardStatus { get; set; }
        public bool SendbackStatus{ get; set; }
        public bool ApprovedStatus{ get; set; }
        public int StatusId { get; set; }
        public bool PublishStatus { get; set; }
        public int flowId { get; set; }
        public bool ChangesStatus { get; set; }
        public string Role { get; set; }
		public string By { get; set; }
		public string res { get; set; }
        public string shrotName { get; set; }

        public int? LastNotigicationId { get; set; }
        public int Action { get; set; }
        public int comcount { get; set; }
        public int id { get; set; }
        public SelectList LoginRoleSendBackList { get; set; }
        public SelectList LoginRoleListSendBack { get; set; }
        public int SelectedSBID { get; set; }
    }

    public class ExamCalendarMaster 
    {
        //[Required(ErrorMessage = "Course is Required")]
        public int Course { get; set; }
        public int CourseTypeId { get; set; }
        //[Required(ErrorMessage = "Trade Type is Required")]
        public string TradeTypeName { get; set; }
        public int TradeTypeId { get; set; }
        //[Required(ErrorMessage = "Trade is Required")]
        public string TradeName { get; set; }
        public int TradeId { get; set; }
        public int STradeId { get; set; }
        public int IsDraft { get; set; }
        public string TradeYearName { get; set; }
        public int TradeYearId { get; set; }
        public int SujectTtypeId { get; set; }
        //[Required(ErrorMessage = "Trade Session is Required")]
        public int TradeSessionId { get; set; }
        //[Required(ErrorMessage = "Exam Type is Required")]
        public string ExamTypeName { get; set; }
        public int ExamTypeId { get; set; }
        //[Required(ErrorMessage = "Exam semester is Required")]
        public string ExamYear { get; set; }
        public string ExamSemName { get; set; }
        public int ExamSemId { get; set; }
        //[Required(ErrorMessage = "Subject Type is Required")]
        public string SubjectTypeName { get; set; }
        public int SubjectTypeId { get; set; }
        public int SelectedSBID { get; set; }
        //[Required(ErrorMessage = "Subject is Required")]
        public string SubjectName { get; set; }
        public int SubjectId { get; set; }
        public int user_id { get; set; }
        public int? Login_Id { get; set; }
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Exam Duration must be numeric")]
        //[Required(ErrorMessage = "Exam Duration is Required")]
        public int Duration { get; set; }
        //[Required(ErrorMessage = "Exam Start Date is Required")]
        public DateTime NExam_date { get; set; }
        public DateTime Exam_Start_date { get; set; }
        //[Required(ErrorMessage = "Exam End Date is Required")]
        public DateTime Exam_End_date { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public int ectId { get; set; }
        public int comcount { get; set; }
        public string status { get; set; }
        public string Remarks { get; set; }
        public HttpPostedFileBase UploadDocs { get; set; }
        public int RoleId { get; set; }
        public int? selectTab { get; set; }
        public int status_id { get; set; }
        public string PdfPath { get; set; }
        public string DeptName { get; set; }
        public string CourseName { get; set; }
        public int FromReg { get; set; }


        public string Exam_CalNotif_Number { get; set; }
        public string Notif_Number { get; set; }
        public int Notif_Number_Id { get; set; }
        public string Exam_CalNotif_Description { get; set; }
        public string Notif_Description { get; set; }
        public string Notif_Status { get; set; }
        public string Notif_Present { get; set; }
        public string By { get; set; }
        public DateTime Exam_Date { get; set; }
        public DateTime EExam_Date { get; set; }
        public DateTime Notif_Date { get; set; }
        public int NotificationId { get; set; }
        public int PublishedId { get; set; }

        public SelectList CourseTypeList { get; set; }
        public SelectList TradeTypeList { get; set; }
        public SelectList ExamNotificationList { get; set; }
        public SelectList TradeList { get; set; }
        public SelectList TradeYearList { get; set; }
        public SelectList SubjectType { get; set; }
        public SelectList SpecialTradeTypeList { get; set; }
        public SelectList TradeSessionList { get; set; }
        public SelectList ExamTypeList { get; set; }
        public SelectList ExamSemList { get; set; }
        public SelectList SubjectTypeList { get; set; }
        public SelectList SubjectList { get; set; }
        public SelectList LoginRoleList { get; set; }
        public SelectList LoginRoleListSendBack { get; set; }
        //BNM
        public SelectList ExamCentresEmailList { get; set; }

        public NotifiationListItem NotifiationListList { get; set; }
        public List<ExamCalendarMaster> NotificationForApprovalList { get; set; }
        public List<ExamCalendarMaster> NotificationForApprovalForModificationList { get; set; }
        public List<ExamCalendarMaster> NotificationForApprovedList { get; set; }
        public List<ExamCalendarMaster> PublishedNotificationForModificationList { get; set; }
        public QuestionPaperSetsListItem QuestionPaperSetsList { get; set; }

        [Required(ErrorMessage = "Please Upload File")]
        [Display(Name = "Upload File")]        
        public HttpPostedFileBase file { get; set; }
    }

    public class QuestionPaperSets
    {
        public int qpst_id { get; set; }
        public int subject_name { get; set; }
        public int exam_calendar_id { get; set; }
        public int status_id { get; set; }
        public string qpst_remarks { get; set; }
        // public string qpst_file_path { get; set; } BNM Commented
        public HttpPostedFile qpst_file_path { get; set; }
        public string ImagePath { get; set; }

        public string exam_type_name { get; set; }
        public int trade_year_name { get; set; }
        public int subject_type_name { get; set; }
        public string course_name { get; set; }
        public string trade_type_name { get; set; }
        public int user_id { get; set; }

        public string subject_id { get; set; }
        public int exam_type_id { get; set; }
        public int trade_year_id { get; set; }
        public int subject_type_id { get; set; }
        public int course_id { get; set; }
        public int trade_type_id { get; set; }
        public int trade_id { get; set; }
        //BNM
        public int ec_id { get; set; }
        public string ExamDate { get; set; }

        public SelectList CourseTypeList { get; set; }
        public SelectList TradeTypeList { get; set; }
        public SelectList TradeList { get; set; }
        public SelectList TradeYearList { get; set; }
        public SelectList TradeSessionList { get; set; }
        public SelectList ExamTypeList { get; set; }
        public SelectList ExamSemList { get; set; }
        public SelectList SubjectTypeList { get; set; }
        public SelectList SubjectList { get; set; }
        public SelectList LoginRoleList { get; set; }
        //BNM
        public SelectList ExamCentresEmailList { get; set; }
        //BNM for question paper set population
        public SelectList QuestionPaperSet { get; set; }
        public int qp_id { get; set; }

        public QuestionPaperSetsListItem QuestionPaperSetsList { get; set; }
        public int selectTab { get; set; }
    }

    //BNM S 
    public class QuestionPaper
    {
        public int QPSTID { get; set; }
        public string CourseType { get; set; }
    public string TradeType { get; set; }
    public string Trade { get; set; }
    public string TradeYear { get; set; }
    public string ExamType { get; set; }
    public string SubjectType { get; set; }
    public string Subject { get; set; }
    public DateTime? ExamDate { get; set; }
    public string QP { get; set; }

        public SelectList QPStatusList { get; set; }
        //  public IEnumerable<QuestionPaperItem> QuestionPaperList { get; set; }
    }

    public class QuestionPaperApprovedByHofQC
    {
        public int QPSTID { get; set; }
        public string CourseType { get; set; }
        public string TradeType { get; set; }
        public string Trade { get; set; }
        public string TradeYear { get; set; }
        public string ExamType { get; set; }
        public string SubjectType { get; set; }
        public string Subject { get; set; }
        public string ExamDate { get; set; }
        public string QP { get; set; }

        public SelectList QPStatusList { get; set; }
        //  public IEnumerable<QuestionPaperItem> QuestionPaperList { get; set; }
    }

    public class QuestionPaperItem
    {
        //  public List<QuestionPaper> objQuestionPaperItem { get; set; }
        public SelectList QPStatusList { get; set; }
        public IEnumerable<QuestionPaper> questionPaperItem { get; set; }
        public IEnumerable<QuestionPaperApprovedByHofQC> questionPaperApprovedByHofQC { get; set; }

        public IEnumerable<QuestionPaper> questionPaperItem1 { get; set; }

        public IEnumerable<ApproveQuestionPaperbyHeadofQPCommitte> approveQuestionPaperbyHeadofQPCommitteItem { get; set; }
    }

    public class ViewModel
    {
        // public IEnumerable<AdmitCard> admitCard { get; set; }
        public IEnumerable<QuestionPaper> questionPaperList { get; set; }
        public QuestionPaperSets questionPaperSets { get; set; }
        public IEnumerable<QuestionPaperSets> questionPaperSetsList { get; set; }
    //public IEnumerable<Subjects> subjects { get; set; }
    }


    public class ApproveQuestionPaperbyHeadofQPCommitte
    {
        public string CourseType { get; set; }
        public string TradeType { get; set; }
        public string Trade { get; set; }
        public string TradeYear { get; set; }
        public string ExamType { get; set; }
        public string SubjectType { get; set; }
        public string Subject { get; set; }
        public string ExamDate { get; set; }
        public string QP { get; set; }
        public string UploadedFile { get; set; }
        public string isSelected { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

        public List<ApproveQuestionPaperbyHeadofQPCommitte> ApproveQuestionPaperbyHeadofQPCommitteList { get; set; }

  //      public List<ApproveQuestionPaperbyHeadofQPCommitteItem> ApproveQuestionPaperbyHeadofQPCommitteList { get; set; }
    }


    public class ApproveQuestionPaperbyHeadofQPCommitteItem
    {
        public string approveQuestionPaperbyHeadofQPCommitteItem { get; set; }
    }


    //BNM E
    public class QuestionPaperSetsListItem
    {
        public List<sendQP> objQuestionPaperSetsItem { get; set; }
    }




    public class sendQP
    {
        public int qpst_id { get; set; }
        public string ExamDate { get; set; }
        public int CourseTypeId { get; set; }
        public int TradeTypeId { get; set; }
        public int TradeId { get; set; }
        public int TradeYearId { get; set; }
        public int ExamTypeId { get; set; }
        public string SubjectId { get; set; }
        public int SubjectTypeId { get; set; }
        //BNM
        public string QuestionPaper { get; set; }
        public string UploadFiles { get; set; }
        //public string FilePath { get; set; }

    }

    //Customized data annotation validator for uploading file
    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int MaxContentLength = 1024 * 1024 * 3; //3 MB
            string[] AllowedFileExtensions = new string[] { ".doc", ".docx", ".pdf" };

            var file = value as HttpPostedFileBase;

            if (file == null)
                return false;
            else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Please upload file of type: " + string.Join(", ", AllowedFileExtensions);
                return false;
            }
            else if (file.ContentLength > MaxContentLength)
            {
                ErrorMessage = "Your file is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "MB";
                return false;
            }
            else
                return true;
        }
    }

    public class subjects
    {
        public int est_Id { get; set; }
        public DateTime Exam_Date { get; set; }
        public DateTime Exam_Start_Time { get; set; }
        public DateTime Exam_End_Time { get; set; }
        public string ExamDate { get; set; }
        public string ExamDay { get; set; }
        public string STime { get; set; }
        public string ETime { get; set; }
        public int SubjectTypeID { get; set; }
        public int SujectTtypeId { get; set; }
        public int CourseTypeId { get; set; }
        public int TradeTypeId { get; set; }
        public int TradeId { get; set; }
        public int TradeYearId { get; set; }
        public int ExamTypeId { get; set; }
        public int ExamSemId { get; set; }
        public string SubjectId { get; set; }
        public int ExamYear { get; set; }
        public string SubjectType { get; set; }
        public string Subject { get; set; }
        public int NotificationId { get; set; }
        public int NotificationNumber { get; set; }
        public string CreationDate { get; set; }
        public DateTime PTime { get; set; }
        public string currentYear { get; set; }
        public int statusID { get; set; }

    }

    public class NotifiationListItem
    {
        public List<subjects> objSubjectsItem { get; set; }
    }

    public class WorkflowActionDetails
    {
        public int NotificationId { get; set; }
        public string DateInfo { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string By { get; set; }

    }


    //DSC Key 01/05/2021
    public class DSCModel
    {
        public int ID { get; set; }
        public string DSCName { get; set; }
        public string Place { get; set; }
        public string SerialNo { get; set; }
        public string issuerEmail { get; set; }
        public string ExpiryDate { get; set; }
        public string ValidFromDate { get; set; }
        public string PublicKey { get; set; }
        public string PhoneNumber { get; set; }
        public string certifyingAuthority { get; set; }
        public int DSCKeyId { get; set; }
        public int userID { get; set; }
        public string userName { get; set; }

        public bool ischecked { get; set; }
        public string remarks { get; set; }
        public string status { get; set; }

        public string designation { get; set; }
        //public string district { get; set; }
        //public string taluka { get; set; }
        //public string hobli { get; set; }
    }

    public class DSCModelList
    {
        public List<DSCModel> dSCModels { get; set; }
    }
}
