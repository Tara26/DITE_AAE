using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models.ExamNotification;
using Models;
using System.Data;

namespace BLL.ExamNotification
{
    public interface INotificationBLL
    {
        SelectList GetCourseListBLL();
        SelectList GetApplicantTypeListBLL();
        SelectList GetDepartmentListBLL();
        SelectList GetNotificationDescListBLL();
        SelectList GetTradeTypeListBLL();
        SelectList GetTradeListBLL();
        SelectList GetTradeYearListBLL();
SelectList SessionListBLL();
 SelectList GetSpecialTradeTypeListBLL();
        SelectList ExamNotificationListBLL();
        SelectList GetExamTypeListBLL();
        SelectList GetExamSemListBLL();
        SelectList SubjectTypeBLL();
        SelectList GetSubjectTypeListBLL();
        SelectList GetSubjectListBLL();
        //BNM
        SelectList GetExamCentresEmailIDListBLL();
        //BNM for question paper set population
        SelectList GetQuestionPaperSetListBLL();
        //BNM for question paper Modify Search
        List<QuestionPaper> getGetSearchModifyQuestionPapersBLL(int CourseTypeID, int TradeTypeID, int TradeID,int @TradeYearID, int ExamTypeID, int ExamSubTypeID, int ExamSubID);

        List<SelectListItem> GetTradeListBasedOnIdBLL(int CourseTypeId, int? TradeTypeId);
        List<SelectListItem> GetTradeTypeListBasedOnIdBLL(int CourseTypeId);
        //List<SelectListItem> GetSubjectListBasedOnIdBLL(int CourseTypeId, int? SubjectTypeId);
		List<SelectListItem> GetSubjectListBasedOnIdBLL(int? CourseTypeId, int? STradeId, int? subjectType);
        List<SelectListItem> GetSubjectTypeListBasedOnIdBLL(int CourseTypeId);
        string CreateNotificationDetailsBLL(Models.ExamNotification.Notification model);
        List<Notification> GetUpdateNotificationBLL(int? notificationId = null);
        List<Models.ExamNotification.Notification> GetNotificationStatusBLL(Notification model);
		//Models.ExamNotification.Notification GetViewBLL(int id);
		Notification GetViewBLL(int id, int loginId);
		//	Models.ExamNotification.Notification GetViewBLL(int id);
		
		int? UpdateTransStatusBLL(Notification notification);
		List<Notification> GetNotificationStatus1BLL(Notification modal);

        string SaveExamCalNotificationBLL(ExamCalendarMaster model);
        string ExamCalenderMasterUploadBLL(ExamCalendarMaster model);
        SelectList GetLoginRoleListBLL();
		SelectList GetLoginRoleListBLL(int? login);

        string PublishNotificationBLL(Notification notification);
		string UpdateCommentsTransStatusBLL(Notification notification);
		string UpdateCWStatusBLL(Notification notification);
        string UpdateCWStatusBLL_WithDSC(Notification notification);
        string SaveNotifiedSubjectsBLL(ExamCalendarMaster model);
		List<ExamCalendarMaster> GetNotificationForApprovalBLL(ExamCalendarMaster model);
        List<subjects> GetSubjectsItemListBLL(int NotificationId);
        string SaveRemarkAndForwardToUserBLL(Notification model);
		string GoBackToModificationBLL(Notification model);
        string ChangesToModificationBLL(Notification model);
        List<WorkflowActionDetails> GetCommentRemarksDetailsBLL(int NotificationId);
        DataTable GetNotificationForSubjectsBLL(int NotificationId, int PublishedId);
        string UpdateNotificationFileBLL(ExamCalendarMaster model);
        List<Notification> GetNotificationListBLL();
        List<ExamCalendarMaster> GetPublishedNotificationBLL(ExamCalendarMaster model);
        List<SelectListItem> GetTradeYearListBasedOnIdBLL(int TradeId);
        List<Notification> GetCommentsFileBLL(int NotificationId);
		 string RejectNotificationBLL(Notification model);
		 List<Notification> GetCommentsListBLL(int NotificationId);
		 //SelectList GetDivisionListBLL();
		  SelectList GetDivisionListBLL(int? divisionId = null);
        SelectList GetDistrictBLL();
        SelectList GetCollegeBLL(); 
         SelectList GetDistrictbasedonDivisionIDBLL(int Div_ID);
        SelectList GetCollegetNamebasedonDivisionIDBLL(int Div_ID);
        SelectList  GetCollegeCodebasedonCollegeIDBLL(string Col_Code_ID);
        //string SaveMappedBLL(string[] Check_values, Exam_Center ECModel);
		List<SelectListItem> GetTradeListBasedOnIdBLL(int TradeTypeId);        
        string GetExamdateBasedOnIdBLL(int? SubjectID);
		string SaveSendQPBLL(QuestionPaperSets model);
		string RejectBLL(Notification notification);
        List<QuestionPaper> getQuestionPaperBll();

        //Below is to fetch the  Approved QP By HofQP Committe

        List<QuestionPaperApprovedByHofQC> GetApprovedQPByHofQPCommitteBll();
        //BNM
        SelectList QPStatusListBLL();

        string saveApproveQuestionPaperbyHeadofQPCommitteBll(ApproveQuestionPaperbyHeadofQPCommitte model);
        int? LastNotificationNumberBLL(Notification notification);
        List<SelectListItem> GetRemainingSubjectListBasedOnIdBLL(string SubjectID, bool? IsEnable);

        //28April2021 BNM
        string UpdateCommentsTransStatus1BLL(Notification notification);
        SelectList GetLoginRoleListForwardBLL(int? login);
        SelectList GetLoginRoleListSendBackBLL(int? login);

        //DSC Key BNM 01May2021 BNM
        string LinkDSCDetailsbll(DSCModel model);
        DSCModelList GetOfficerDSCMappingsbll(int userID);
        bool UpdateDSCStatusbll(DSCModelList list);
        bool ValidateDSCKeyWithUserbll(int officeID, string PublicKey);
        string GetLastFeeDateBLL(int ExamNotifyId);

        string GrievanceSaveRemarkAndForwardToUserBLL(TabulateGrievance model);
        SelectList ExamNotificationsListBLL();
        List<Notification> GetPublishedFilePathBLL(Notification notification);
        List<Notification> ViewNotificationFileBLL(int id, int? notificationId);

        int? UpdateNotificationDocFileDetailsBLL(List<Notification> models);
        List<Notification> GetRollsBLL(int? Roleid,int? loginID);
        Notification GetNotificationtransactiondtlsBLL(int id, int LoginID);

        List<ExamCalendarMaster> GetPublishedNotificationIDBLL(ExamCalendarMaster model);
        List<string> ExamCenterMailIdBLL();
       
    }
}
