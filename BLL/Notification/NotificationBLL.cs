using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL;
using DLL.User;
using System.Web.Mvc;
using Models.ExamNotification;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace BLL.ExamNotification
{
    public class NotificationBLL : INotificationBLL
    {
        private readonly INotificationDLL _notifDll;

        public NotificationBLL()
        {
            this._notifDll = new NotificationDLL();
        }
        //BNM
        public SelectList GetExamCentresEmailIDListBLL()
        {
            return _notifDll.GetExamCentresEmailIDListDLL();
        }
        public SelectList GetCourseListBLL()
        {
            return _notifDll.GetCourseListDLL();
        }
        public SelectList GetApplicantTypeListBLL()
        {
            return _notifDll.GetApplicantTypeListDLL();
        }
        public SelectList SessionListBLL()
        {
            return _notifDll.SessionListDLL();
        }

        public SelectList GetTradeTypeListBLL()
        {
            return _notifDll.GetTradeTypeListDLL();
        }

        public SelectList GetTradeListBLL()
        {
            return _notifDll.GetTradeListDLL();
        }

        public SelectList GetTradeYearListBLL()
        {
            return _notifDll.GetTradeYearListDLL();
        }
		
		public SelectList ExamNotificationListBLL()
        {
            return _notifDll.ExamNotificationListDLL();
        }

        public SelectList GetExamTypeListBLL()
        {
            return _notifDll.GetExamTypeListDLL();
        }

        public SelectList GetExamSemListBLL()
        {
            return _notifDll.GetExamSemListDLL();
        }

        public SelectList GetSubjectTypeListBLL()
        {
            return _notifDll.GetSubjectTypeListDLL();
        }

        public SelectList SubjectTypeBLL()
        {
            return _notifDll.SubjectTypeDLL();
        }
        public SelectList GetSubjectListBLL()
        {
            return _notifDll.GetSubjectListDLL();
        }

        public SelectList GetDepartmentListBLL()
        {
           return _notifDll.GetDepartmentListDLL();
        }

        public SelectList GetNotificationDescListBLL()
        {
            return _notifDll.GetNotificationDescListDLL();
        }

        public string CreateNotificationDetailsBLL(Notification model)
        {
            return _notifDll.CreateNotificationDetailsDLL(model);
        }

        public List<Notification> GetNotificationStatusBLL(Notification model)
        {
            return this._notifDll.GetNotificationStatusDLL(model); 
        }

        public List<ExamCalendarMaster> GetNotificationForApprovalBLL(ExamCalendarMaster model)
        {
            return this._notifDll.GetNotificationForApprovalDLL(model);
        }

        public List<ExamCalendarMaster> GetPublishedNotificationBLL(ExamCalendarMaster model)
        {
            return this._notifDll.GetPublishedNotificationDLL(model);
        }

        public Notification GetViewBLL(int id, int loginId)
		{
            return this._notifDll.GetViewDLL(id, loginId);			

		}
		
		 public Notification GetViewBLL(int id)
		{
            return this._notifDll.GetViewDLL(id);			

		}
		
		 public Notification GetNotificationtransactiondtlsBLL(int id,int LoginID)
        {
            return this._notifDll.GetNotificationtransactiondtlsDLL(id, LoginID);

        }

		public int? UpdateTransStatusBLL(Notification notification)
		{
				var empDetails = this._notifDll.GetnotifyByID(notification);
            empDetails.exam_notif_status_id = 101;
			
				int? res1 = this._notifDll.UpdateEmp(empDetails);
			string res = _notifDll.CreateTransNotificationDLL(notification);
			var Details = this._notifDll.GetnotifyRoleId(notification);
			var UpdateExamMaster = this._notifDll.GetnotifyByIDMast(Details.exam_notif_id);
			UpdateExamMaster.login_id = Details.login_id;
            UpdateExamMaster.status_id = 113;// Details.exam_notif_status_id;
            UpdateExamMaster.updation_datetime = DateTime.Now;
            int? a = this._notifDll.UpdateStatusMast(UpdateExamMaster);
            //if (notification.login_id == 2)
            //{
                string res2 = _notifDll.CreateCommentsNotificationDLL(notification);
            //}
            return res1;

        }

        public string PublishNotificationBLL(Notification notification)
        {
            var res = this._notifDll.PublishNotificationDLL(notification);
            var updateStatus = this._notifDll.Getnotify(notification);
            updateStatus.status_id = notification.exam_notif_status_id;
            int? res2 = _notifDll.UpdateStatusMast(updateStatus);

            var translist = this._notifDll.GetNotificationTranslist(notification);
            tbl_exam_notification_trans obj = new tbl_exam_notification_trans();
            foreach (var item in translist)
            {
                obj.exam_notif_trans_id = item.exam_notif_trans_id;
                obj.exam_notif_status_id = notification.exam_notif_status_id;
                int? res1 = this._notifDll.UpdateStatusforall(obj);
            }
            return res;
        }

        public List<Notification> GetPublishedFilePathBLL(Notification notification)
        {
            return this._notifDll.GetPublishedFilePathDLL(notification);
           
        }

        public List<Notification> GetNotificationStatus1BLL(Notification modal)
        {
            return this._notifDll.GetNotificationStatus1DLL(modal);
        }

        public string SaveExamCalNotificationBLL(ExamCalendarMaster model)
        {
            return _notifDll.SaveExamCalNotificationDLL(model);
        }

        public string ExamCalenderMasterUploadBLL(ExamCalendarMaster model)
        {
            return _notifDll.ExamCalenderMasterUploadDLL(model);
        }

        public string SaveNotifiedSubjectsBLL(ExamCalendarMaster model)
        {
            return _notifDll.stringSaveNotifiedSubjectsDLL(model);
        }

        public List<SelectListItem> GetTradeListBasedOnIdBLL(int CourseTypeId, int? TradeTypeId)
        {
            return _notifDll.GetTradeListBasedOnIdDLL(CourseTypeId, TradeTypeId);
        }

        public List<SelectListItem> GetTradeTypeListBasedOnIdBLL(int CourseTypeId)
        {
            return _notifDll.GetTradeTypeListBasedOnIdDLL(CourseTypeId);
        }

        //public List<SelectListItem> GetSubjectListBasedOnIdBLL(int CourseTypeId, int? SubjectTypeId)
        //{
        //    return _notifDll.GetSubjectListBasedOnIdDLL(CourseTypeId, SubjectTypeId);
        //}
		
		public List<SelectListItem> GetSubjectListBasedOnIdBLL(int? CourseTypeId, int? STradeId, int? subjectType)
        {
            return _notifDll.GetSubjectListBasedOnIdDLL(CourseTypeId, STradeId, subjectType);
        }

        public List<SelectListItem> GetSubjectTypeListBasedOnIdBLL(int CourseTypeId)
        {
            return _notifDll.GetSubjectTypeListBasedOnIdDLL(CourseTypeId);
        }

        public SelectList GetLoginRoleListBLL()
        {
            return _notifDll.GetLoginRoleListDLL();
        }

        public SelectList GetLoginRoleListBLL(int? login)
        {
            return _notifDll.GetLoginRoleListDLL(login);
        }

        public List<Notification> GetUpdateNotificationBLL(int? notificationId = null)
        {
            return _notifDll.GetUpdateNotificationDLL(notificationId);
        }

        public string UpdateCommentsTransStatusBLL(Notification notification)
        {
            //int? update = UpdateTransStatusBLL(notification);
            var empDetails = this._notifDll.GetnotifyByID(notification);
            int? res1 = this._notifDll.UpdateEmp(empDetails);
            string res2 = _notifDll.CreateTransNotificationDLL(notification);
            var Details = this._notifDll.GetnotifyRoleId(notification);
            Details.exam_notif_status_id = 101;
            Details.updated_by = notification.login_id;
            Details.updation_datetime = DateTime.Now;
            var rest = _notifDll.UpdateStatus(Details);
            var UpdateExamMaster = this._notifDll.GetnotifyByIDMast(Details.exam_notif_id);
            UpdateExamMaster.login_id = notification.RoleId;
            UpdateExamMaster.status_id = 101;
            UpdateExamMaster.updation_datetime = DateTime.Now;
            int? a = this._notifDll.UpdateStatusMast(UpdateExamMaster);

            string res = _notifDll.CreateCommentsNotificationDLL(notification);
            return res;
        }

        public string UpdateCWStatusBLL(Notification notification)
        {
            var UpdateExamMaster = this._notifDll.GetnotifyByIDMast(notification.Exam_Notif_Id);
            UpdateExamMaster.login_id = notification.RoleId;
            UpdateExamMaster.status_id = notification.updatestatusCW;
            UpdateExamMaster.updation_datetime = DateTime.Now;
            int? a = this._notifDll.UpdateStatusMast(UpdateExamMaster);

            //if (notification.updatestatusCW != 102)
            //{
                var updateStatus = this._notifDll.GetNotificationTranslist(notification);
                tbl_exam_notification_trans obj = new tbl_exam_notification_trans();
                foreach (var item in updateStatus)
                {
                    obj.exam_notif_trans_id = item.exam_notif_trans_id;
                    obj.exam_notif_status_id = notification.updatestatusCW;
                    obj.updated_by = notification.login_id;
                    int? res1 = this._notifDll.UpdateStatusforall(obj);
                }
           // }
    //        else
    //        {
    //            var updateStatus = this._notifDll.GetnotifyByID(notification);
				//updateStatus.exam_notif_status_id = 112;// notification.updatestatusCW;
				////updateStatus.exam_notif_status_id = 102;
				//updateStatus.updated_by = notification.login_id;
    //            int? result = this._notifDll.UpdateStatus(updateStatus);
    //            var Details = this._notifDll.GetnotifyRoleId(notification);
    //            Details.exam_notif_status_id = notification.updatestatusCW;
    //            Details.updated_by = notification.login_id;
    //            var rest = _notifDll.UpdateStatus(Details);

    //        }

            string res = _notifDll.CreateCommentsNotificationDLL(notification);
            return res;
        }
        public string UpdateCWStatusBLL_WithDSC(Notification notification)
        {
            var UpdateExamMaster = this._notifDll.GetnotifyByIDMast(notification.Exam_Notif_Id);
            UpdateExamMaster.login_id = notification.RoleId;
            UpdateExamMaster.status_id = notification.updatestatusCW;
            UpdateExamMaster.updation_datetime = DateTime.Now;
            UpdateExamMaster.signed_exam_notif_file_path = notification.getSignedPDFPath;
            // UpdateExamMaster.exam_notif_file_path = notification.getSignedPDFPath;
            int? a = this._notifDll.UpdateStatusMast(UpdateExamMaster);

            //if (notification.updatestatusCW == 110)
            //{

            //}
            var updateStatus = this._notifDll.GetNotificationTranslist(notification);
            tbl_exam_notification_trans obj = new tbl_exam_notification_trans();
            foreach (var item in updateStatus)
            {
                obj.exam_notif_trans_id = item.exam_notif_trans_id;
                obj.exam_notif_status_id = notification.updatestatusCW;
                obj.updated_by = notification.login_id;
                int? res1 = this._notifDll.UpdateStatusforall(obj);
            }


            string res = _notifDll.CreateCommentsNotificationDLL(notification);
            return res;
        }

        public string RejectBLL(Notification notification)
        {
            //var updateStatus = this._notifDll.GetnotifyByID(notification);
            var updateStatus = this._notifDll.GetnotifyRoleId(notification);
            updateStatus.exam_notif_status_id = notification.updatestatusCW;
            updateStatus.updated_by = notification.login_id;
            var sentback = this._notifDll.GetnotifyByID(notification);
            sentback.exam_notif_status_id = 109;
            int? result1 = this._notifDll.UpdateStatus(sentback);
            sentback.updated_by = notification.login_id;
            int? result = this._notifDll.UpdateStatus(updateStatus);

            string res = _notifDll.CreateCommentsNotificationDLL(notification);

            var Details = this._notifDll.GetnotifyRoleId(notification);
            var UpdateExamMaster = this._notifDll.GetnotifyByIDMast(Details.exam_notif_id);
            //UpdateExamMaster.login_id = Details.login_id;
            //UpdateExamMaster.status_id = Details.exam_notif_status_id;
            UpdateExamMaster.login_id = notification.RoleId;
            UpdateExamMaster.status_id = notification.updatestatusCW;
            int? a = this._notifDll.UpdateStatusMast(UpdateExamMaster);


            if (result == 1)
            {
                return "Saved";
            }
            //updateStatus.exam_notif_status_id = notification.updatestatusCW;
            //int? result = this._notifDll.UpdateStatus(updateStatus);
            //var empDetails = this._notifDll.GetnotifyByIDofCW(notification);
            //empDetails.exam_notif_status_id = notification.updatestatusCW;
            //int? res1 = this._notifDll.UpdateStatus(empDetails);
            return "Failed";
        }
        public List<subjects> GetSubjectsItemListBLL(int NotificationId)
        {
            return _notifDll.GetSubjectsItemListDLL(NotificationId);
        }

        public string SaveRemarkAndForwardToUserBLL(Notification model)
        {
            return _notifDll.SaveRemarkAndForwardToUserDLL(model);
        }

        public List<WorkflowActionDetails> GetCommentRemarksDetailsBLL(int NotificationId)
        {
            return _notifDll.GetCommentRemarksDetailsDLL(NotificationId);
        }

        public string GoBackToModificationBLL(Notification model)
        {
            return _notifDll.GoBackToModificationDLL(model);
        }

        public string ChangesToModificationBLL(Notification model)
        {
            return _notifDll.ChangesToModificationDLL(model);
        }

        //public DataTable GetNotificationForSubjectsBLL(int NotificationId, int PublishedId)
        //{
        //    return _notifDll.GetNotificationForSubjectsDLL(NotificationId, PublishedId);
        //}

        public string UpdateNotificationFileBLL(ExamCalendarMaster model)
        {
            return _notifDll.UpdateNotificationFileDLL(model);
        }

        public List<Notification> GetNotificationListBLL()
        {
            return _notifDll.GetNotificationListDLL();
        }

        public List<SelectListItem> GetTradeYearListBasedOnIdBLL(int TradeId)
        {
            return _notifDll.GetTradeYearListBasedOnIdDLL(TradeId);
        }

        public List<Notification> GetCommentsFileBLL(int NotificationId)
        {
            return _notifDll.GetCommentsFileDLL(NotificationId);
        }

        public string RejectNotificationBLL(Notification model)
        {
            return _notifDll.RejectNotificationDLL(model);
        }

        public List<Notification> GetCommentsListBLL(int NotificationId)
        {
            return this._notifDll.GetCommentsListDLL(NotificationId);
        }

        public SelectList GetDivisionListBLL(int? divisionId = null)
        {
            return _notifDll.GetDivisionListDLL(divisionId);
        }
        public SelectList GetDistrictBLL()
        {
            return _notifDll.GetDistrictDLL();
        }
        public SelectList GetCollegeBLL()
        {
            return _notifDll.GetCollegeDLL();
        }
        public SelectList GetDistrictbasedonDivisionIDBLL(int Div_ID)
        {
            return _notifDll.GetDistrictbasedonDivisionIDDLL(Div_ID);
        }

        public SelectList GetCollegetNamebasedonDivisionIDBLL(int Dist_ID)
        {
            return _notifDll.GetCollegetNamebasedonDivisionIDDLL(Dist_ID);
        }
        public SelectList GetCollegeCodebasedonCollegeIDBLL(string Col_Code_ID)
        {
            return _notifDll.GetCollegeCodebasedonCollegeIDDLL(Col_Code_ID);
        }

        //public string SaveMappedBLL(string[] Check_values, Exam_Center ECModel)
        //{
        //    return _notifDll.SaveMappedDLL(Check_values, ECModel);
        //}

        public DataTable GetNotificationForSubjectsBLL(int NotificationId, int PublishedId)
        {
            return _notifDll.GetNotificationForSubjectsDLL(NotificationId, PublishedId);
        }

        public string SaveSendQPBLL(QuestionPaperSets model)
        {
            return _notifDll.stringSendQPDLL(model);
        }

        public List<SelectListItem> GetTradeListBasedOnIdBLL(int TradeTypeId)
        {
            return _notifDll.GetTradeListBasedOnIdDLL(TradeTypeId);
        }

        public string GetExamdateBasedOnIdBLL(int? SubjectID)
        {
            return _notifDll.GetExamdateBasedOnIdDLL(SubjectID);
        }
        //BNM S
        public List<QuestionPaper> getQuestionPaperBll()
        // public Ienum<QuestionPaper> getQuestionPaperBll()
        {
            return _notifDll.getQuestionPaperDll();
        }

        
            public List<QuestionPaperApprovedByHofQC> GetApprovedQPByHofQPCommitteBll()
        // public Ienum<QuestionPaper> getQuestionPaperBll()
        {
            return _notifDll.GetApprovedQPByHofQPCommitteDll();
        }



        public SelectList QPStatusListBLL()
        {
            return _notifDll.GetQPStatusDLL();
        }

        public string saveApproveQuestionPaperbyHeadofQPCommitteBll(ApproveQuestionPaperbyHeadofQPCommitte model)
        // public Ienum<QuestionPaper> getQuestionPaperBll()
        {
            return _notifDll.saveApproveQuestionPaperbyHeadofQPCommitteDll(model);
        }


        public SelectList GetQuestionPaperSetListBLL()
        {
            return _notifDll.GetQuestionPaperSetListDLL();
        }


        public List<QuestionPaper> getGetSearchModifyQuestionPapersBLL(int CourseTypeID, int TradeTypeID, int TradeID, int @TradeYearID, int ExamTypeID, int ExamSubTypeID, int ExamSubID)
        {
            return _notifDll.getGetSearchModifyQuestionPapersDLL(CourseTypeID, TradeTypeID, TradeID, @TradeYearID, ExamTypeID, ExamSubTypeID, ExamSubID);
        }

        //BNM E

        public int? LastNotificationNumberBLL(Notification notification)
        {
            return _notifDll.LastNotificationNumberDLL(notification);
        }

        public SelectList GetSpecialTradeTypeListBLL()
        {
            return _notifDll.GetSpecialTradeTypeListDLL();
        }

        public List<SelectListItem> GetRemainingSubjectListBasedOnIdBLL(string SubjectID, bool? IsEnable)
        {
            return _notifDll.GetRemainingSubjectListBasedOnIdDLL(SubjectID, IsEnable);
        }

        public SelectList GetLoginRoleListForwardBLL(int? login)
        {
            return _notifDll.GetLoginRoleListForwardDLL(login);
        }


        public SelectList GetLoginRoleListSendBackBLL(int? login)
        {
            return _notifDll.GetLoginRoleListSendBackDLL(login);
        }


        public string UpdateCommentsTransStatus1BLL(Notification notification)
        {
            //int? update = UpdateTransStatusBLL(notification);
            var empDetails = this._notifDll.GetnotifyByID(notification);
            int? res1 = this._notifDll.UpdateEmp(empDetails);
            string res2 = _notifDll.CreateTransNotificationDLL(notification);
            var Details = this._notifDll.GetnotifyRoleId(notification);
            Details.exam_notif_status_id = 109;
            Details.updated_by = notification.login_id;
            var rest = _notifDll.UpdateStatus(Details);
            var UpdateExamMaster = this._notifDll.GetnotifyByIDMast(Details.exam_notif_id);
            UpdateExamMaster.login_id = notification.RoleId;
            UpdateExamMaster.status_id = 109;
            int? a = this._notifDll.UpdateStatusMast(UpdateExamMaster);
            string res = _notifDll.CreateCommentsNotificationDLL(notification);
            return res;
        }

        //DSC Key BNM 01May2021
        public string LinkDSCDetailsbll(DSCModel model)
        {
            return _notifDll.LinkDSCDetailsdll(model);
        }

        public DSCModelList GetOfficerDSCMappingsbll(int userID)
        {
            return _notifDll.GetOfficerDSCMappingsdll(userID);
        }

        public bool UpdateDSCStatusbll(DSCModelList list)
        {
            return _notifDll.UpdateDSCStatusdll(list);
        }

        public bool ValidateDSCKeyWithUserbll(int officeID, string PublicKey)
        {
            return _notifDll.ValidateDSCKeyWithUserdll(officeID, PublicKey);
        }

        public string GetLastFeeDateBLL(int ExamNotifyId)
        {
            return _notifDll.GetLastFeeDateBLLDll(ExamNotifyId);
        }

        public string GrievanceSaveRemarkAndForwardToUserBLL(TabulateGrievance model)
        {
            return _notifDll.GrievanceSaveRemarkAndForwardToUserDLL(model);
        }
        public SelectList ExamNotificationsListBLL()
        {
            return _notifDll.ExamNotificationsListDLL();
        }
        public List<Notification> ViewNotificationFileBLL(int id, int? notificationId)
        {
            return _notifDll.ViewNotificationFileDLL(id, notificationId);
        }

        public int? UpdateNotificationDocFileDetailsBLL(List<Notification> models)
        {
            Notification notification = new Notification();
            
            var res1 = 0;
            foreach (var model in models)
            {
                notification.Exam_Notif_Number = model.Exam_Notif_Number;
                notification.login_id = model.login_id;
                notification.SavePath = model.SavePath;
                notification.DocSavePath = model.DocSavePath;
                
            }

            notification.Exam_Notif_Id = _notifDll.GetNotificationID(notification);
           
            //transaction table
            List<tbl_exam_notification_trans> translist = _notifDll.Get_tbl_exam_notification_trans(notification);
            tbl_exam_notification_trans obj1 = new tbl_exam_notification_trans();
            foreach (var item in translist)
            {
                
                obj1.login_id = notification.login_id;
                obj1.exam_notif_id = notification.Exam_Notif_Id;
                obj1.updated_by = notification.login_id;
                obj1.exam_notif_doc_file_path = notification.DocSavePath;
                obj1.exam_notif_status_id = item.exam_notif_status_id;
                res1 = _notifDll.updateTransactionNotificationDoc(obj1);
            }

            //master table
            if(notification.login_id == 2)
            {
                List<tbl_exam_notification_mast> mastlist = _notifDll.Get_tbl_exam_notification_mast(notification);
                tbl_exam_notification_mast mastobj1 = new tbl_exam_notification_mast();
                foreach (var item in mastlist)
                {

                    mastobj1.login_id = notification.login_id;
                    mastobj1.exam_notif_id = notification.Exam_Notif_Id;
                    mastobj1.updated_by = notification.login_id;
                    mastobj1.exam_notif_file_path = notification.SavePath;
                    res1 = _notifDll.updateMasterNotificationPDF(mastobj1);
                }

            }

            return res1;
        }

        public List<Notification> GetRollsBLL(int? Roleid,int? loginID)
        {
            return _notifDll.GetRolesDLL(Roleid,loginID);
        }

        public List<ExamCalendarMaster> GetPublishedNotificationIDBLL(ExamCalendarMaster model)
        {
            return this._notifDll.GetPublishedNotificationIDDLL(model);
        }

        public List<string> ExamCenterMailIdBLL()
        {
            var res = _notifDll.ExamCenterMailIdDLL();
            return res;
        }
    }
}
