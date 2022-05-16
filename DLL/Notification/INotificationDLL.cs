using Models.ExamNotification;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data;
using Models.ExamCenterMap;

namespace DLL.User
{
	public interface INotificationDLL
	{
		SelectList GetCourseListDLL();
        SelectList SessionListDLL();

        SelectList GetDepartmentListDLL();
		SelectList GetNotificationDescListDLL();
		SelectList GetTradeTypeListDLL();
		SelectList GetTradeListDLL();
		SelectList GetTradeYearListDLL();
		SelectList GetSpecialTradeTypeListDLL();
		SelectList ExamNotificationListDLL();
		SelectList GetExamTypeListDLL();
		SelectList GetExamSemListDLL();
		SelectList GetSubjectTypeListDLL();
		SelectList GetSubjectListDLL();
        //BNM
        SelectList GetExamCentresEmailIDListDLL();
        List<QuestionPaper> getGetSearchModifyQuestionPapersDLL(int CourseTypeID, int TradeTypeID, int TradeID,int @TradeYearID, int ExamTypeID, int ExamSubTypeID, int ExamSubID);
        List<SelectListItem> GetTradeListBasedOnIdDLL(int CourseTypeId, int? TradeTypeId);
		List<SelectListItem> GetTradeTypeListBasedOnIdDLL(int CourseTypeId);
        List<SelectListItem> GetSubjectListBasedOnIdDLL(int? CourseTypeId, int? STradeId, int? subjectType);
        List<SelectListItem> GetSubjectTypeListBasedOnIdDLL(int CourseTypeId);
		string CreateNotificationDetailsDLL(Notification model);
		List<Notification> GetUpdateNotificationDLL(int? notificationId = null);
		List<Models.ExamNotification.Notification> GetNotificationStatusDLL(Notification notification);
		//Models.ExamNotification.Notification GetViewDLL(int id);
		Notification GetViewDLL(int id, int loginid);
        Notification GetViewDLL(int loginId);

        tbl_exam_notification_trans GetnotifyByID(Notification notification);
		int? UpdateEmp(tbl_exam_notification_trans emp);
		List<Models.ExamNotification.Notification> GetNotificationStatus1DLL(Notification modal);
		string CreateTransNotificationDLL(Notification model);
		string SaveExamCalNotificationDLL(ExamCalendarMaster model);
		string ExamCalenderMasterUploadDLL(ExamCalendarMaster model);
		string stringSaveNotifiedSubjectsDLL(ExamCalendarMaster model);
		DataTable ConvertXSLXtoDataTable(string strFilePath);
		SelectList GetLoginRoleListDLL(int? login);
		string CreateCommentsNotificationDLL(Notification model);
		string PublishNotificationDLL(Notification notification);
		tbl_exam_notification_trans GetnotifyByIDofCW(Notification notification);
		int? UpdateStatus(tbl_exam_notification_trans emp);
        List<Notification> GetPublishedFilePathDLL(Notification notification);


		List<ExamCalendarMaster> GetNotificationForApprovalDLL(ExamCalendarMaster model);
		List<subjects> GetSubjectsItemListDLL(int NotificationId);
		string SaveRemarkAndForwardToUserDLL(Notification model);
		List<WorkflowActionDetails> GetCommentRemarksDetailsDLL(int NotificationId);
		string GoBackToModificationDLL(Notification model);
		string ChangesToModificationDLL(Notification model);
		DataTable GetNotificationForSubjectsDLL(int NotificationId, int PublishedId);
		string UpdateNotificationFileDLL(ExamCalendarMaster model);
		List<Notification> GetNotificationListDLL();
		List<ExamCalendarMaster> GetPublishedNotificationDLL(ExamCalendarMaster model);
        List<SelectListItem> GetTradeListBasedOnIdDLL(int TradeTypeId);
		List<SelectListItem> GetTradeYearListBasedOnIdDLL(int TradeId);
		List<Notification> GetCommentsFileDLL(int NotificationId);
		string RejectNotificationDLL(Notification model);
		tbl_exam_notification_mast Getnotify(Notification notification);
		int? UpdateStatusforall(tbl_exam_notification_trans emp);
		List<Notification> GetCommentsListDLL(int NotificationId);
		SelectList GetDivisionListDLL(int? divisionId = null);
		SelectList GetDistrictDLL();
		SelectList GetCollegeDLL();
		SelectList GetDistrictbasedonDivisionIDDLL(int Div_ID);
		SelectList GetCollegetNamebasedonDivisionIDDLL(int Div_ID);
		SelectList GetCollegeCodebasedonCollegeIDDLL(string Col_Code_ID);
		//string SaveMappedDLL(string[] Check_values, Exam_Center ECModel);
		string GetExamdateBasedOnIdDLL(int? SubjectID);
		string stringSendQPDLL(QuestionPaperSets model);
		tbl_exam_notification_trans GetnotifyRoleId(Notification notification);
		tbl_exam_notification_mast GetnotifyByIDMast(int notifnum);
		int? UpdateStatusMast(tbl_exam_notification_mast emp);
			//Models.ExamNotification.Notification GetViewDLL(int id);
        //BNM S
     List<QuestionPaper> getQuestionPaperDll();
        List<QuestionPaperApprovedByHofQC> GetApprovedQPByHofQPCommitteDll();
        // SelectList QPStatusListDLL();
        SelectList GetQPStatusDLL();
        string saveApproveQuestionPaperbyHeadofQPCommitteDll(ApproveQuestionPaperbyHeadofQPCommitte model);

        SelectList GetQuestionPaperSetListDLL();
        //BNM E
        int? LastNotificationNumberDLL(Notification notification);
        List<SelectListItem> GetRemainingSubjectListBasedOnIdDLL(string SubjectID, bool? IsEnable);

        List<tbl_exam_notification_trans> GetNotificationTranslist(Notification notification);
        SelectList GetLoginRoleListForwardDLL(int? login);
        SelectList GetLoginRoleListSendBackDLL(int? login);


        //DSC Key BNM
        string LinkDSCDetailsdll(DSCModel model);
        DSCModelList GetOfficerDSCMappingsdll(int userID);
        bool UpdateDSCStatusdll(DSCModelList list);
        bool ValidateDSCKeyWithUserdll(int officeID, string PublicKey);
        string GetLastFeeDateBLLDll(int ExamNotifyId);
        string GrievanceSaveRemarkAndForwardToUserDLL(TabulateGrievance model);
		SelectList ExamNotificationsListDLL();
        List<Notification> ViewNotificationFileDLL(int id, int? notificationId);
        string UpdateNotificationDocFileDetailsDLL(List<Notification> models);
        int GetNotificationID(Notification notification);
        int? UpdateNotificationDocFile(tbl_exam_notification_trans obj);
        List<Notification> GetRolesDLL(int? Roleid,int? loginID);
        List<tbl_exam_notification_trans> Get_tbl_exam_notification_trans(Notification notification);
        int updateTransactionNotificationDoc(tbl_exam_notification_trans obj);
        Notification GetNotificationtransactiondtlsDLL(int id, int LoginID);
        List<tbl_exam_notification_mast> Get_tbl_exam_notification_mast(Notification notification);
        int updateMasterNotificationPDF(tbl_exam_notification_mast obj);
        List<ExamCalendarMaster> GetPublishedNotificationIDDLL(ExamCalendarMaster model);
		List<string> ExamCenterMailIdDLL();
		SelectList GetLoginRoleListDLL();
		SelectList SubjectTypeDLL();
		SelectList GetApplicantTypeListDLL();
	}

}
