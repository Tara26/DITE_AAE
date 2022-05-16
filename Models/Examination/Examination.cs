using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Models
{
    public class Examination
    {
        public int? selectTab { get; set; }
        public int user_id { get; set; }
        public int CourseTypeId { get; set; }
        public int SessionId { get; set; }
        public int SessionId1 { get; set; }
        public int CourseTypeId1 { get; set; }
        public long? RollNumber { get; set; }
        public string TraineeName { get; set; }
        public string CourseName { get; set; }
        public string MISCode { get; set; }
        public string ITIinstitute { get; set; }
        public string UniqueCode { get; set; }
        public int District_dist_id { get; set; }
        public SelectList CourseTypeList { get; set; }
        public SelectList Session_Year_List { get; set; }
        public List<Examination> objUniqueItem { get; set; }
        public SelectList DivisionList { get; set; }
        public int Division_id { get; set; }
        public SelectList DistrictList { get; set; }
        public int district_id { get; set; }
        public SelectList MappedExamCentreLists { get; set; }
        public int centre_id { get; set; }
        public SelectList TradeList { get; set; }
        public int tradeId { get; set; }
        public List<Examination> objUniqueItemOnTrade { get; set; }
        public string sessionYear { get; set; }
        public int session { get; set; }
    }

    public class PackingSlip
    {
        public int? selectTab { get; set; }
        public int user_id { get; set; }
        public int CourseTypeId { get; set; }
        public string CourseName { get; set; }
        public int TradeTypeId { get; set; }
        public int SpecialTradeTypeId { get; set; }
        public int ExamCenterId { get; set; }
        public int TradeId { get; set; }
        public int SessionId { get; set; }
        public int SubjectType { get; set; }
        public int SubjectId { get; set; }
        public DateTime Exam_Date { get; set; }
        public string filepath { get; set; }
        public string SubjectName { get; set; }
        public string Session { get; set; }
        public string TradeName { get; set; }

        public SelectList ExamCenterList { get; set; }
        public SelectList TradeList { get; set; }
        public SelectList TradeTypeList { get; set; }
        public SelectList SpecialTradeTypeList { get; set; }
        public SelectList Session_Year_List { get; set; }
        public SelectList CourseTypeList { get; set; }

        public List<PackingSlip> objPackingSlipItem { get; set; }
    }
	
	public class PracticalEvaluationsheet
    {
        public int? selectTab { get; set; }
        public int user_id { get; set; }
        public int CourseTypeId { get; set; }
        public int ExamCenterId { get; set; }
        public int TradeId { get; set; }
        public int Session { get; set; }
        public int SessionId { get; set; }
        public int TradeTypeId { get; set; }
        public int SubjectId { get; set; }
        public int SpecialTradeTypeId { get; set; }
        public int SubjectType { get; set; }
        public DateTime Exam_Date { get; set; }
        public string filepath { get; set; }
        public string SubjectName { get; set; }
        public string TradeName { get; set; }
        public string CourseName { get; set; }
        public string SpecialSubjectText { get; set; }


        public SelectList ExamCenterList { get; set; }
        public SelectList TradeList { get; set; }
        public SelectList TradeTypeList { get; set; }
        public SelectList SpecialTradeTypeList { get; set; }
        public SelectList Session_Year_List { get; set; }
        public SelectList CourseTypeList { get; set; }

        public List<PracticalEvaluationsheet> objPracticalEvaluationsheetItem { get; set; }
    }

    public class CodeMarksSlip
    {
        public int? selectTab { get; set; }
        public int user_id { get; set; }
        public int CourseTypeId { get; set; }
        public int ExamCenterId { get; set; }
        public int TradeId { get; set; }
        public int SessionId { get; set; }
        public int TradeTypeId { get; set; }
        public int Division_id { get; set; }
        public int District_id { get; set; }
        public int SpecialTradeTypeId { get; set; }
        public int SubjectType { get; set; }
        public int SubjectId { get; set; }
        public DateTime Exam_Date { get; set; }
        public string filepath { get; set; }
        public string SubjectName { get; set; }
        public string TradeName { get; set; }
        public string CourseName { get; set; }
        public string ExamcentreName { get; set; }
        public string DivisionName { get; set; }
        public string DistName { get; set; }

        public SelectList ExamCenterList { get; set; }
        public SelectList TradeList { get; set; }
        public SelectList TradeTypeList { get; set; }
        public SelectList DivisionList { get; set; }
        public SelectList DistrictList { get; set; }
        public SelectList SpecialTradeTypeList { get; set; }
        public SelectList Session_Year_List { get; set; }
        public SelectList CourseTypeList { get; set; }

        public List<CodeMarksSlip> objCodeMarksSlipItem { get; set; }
    }

    public class TabulateGrievance
    {
        public int? selectTab { get; set; }
        public int RoleId { get; set; }
        public int login_id { get; set; }
        public int user_id { get; set; }
        public int CourseTypeId { get; set; }
        public int Division_id { get; set; }
        public int District_dist_id { get; set; }
        public string RollNumber { get; set; }
        public string TraineeName { get; set; }
        public string CollagaName { get; set; }
        public string SubjectName { get; set; }
        public string Marks { get; set; }
        public string Remarks { get; set; }

        public SelectList CourseTypeList { get; set; }
		public SelectList TradeList { get; set; }
		public SelectList KgList { get; set; }
		public SelectList LoginRoleList { get; set; }
        public SelectList DivisionList { get; set; }
        public SelectList DistrictList { get; set; }
        public List<TabulateGrievance> GetApplyedGrievanceTrainee { get; set; }
		public List<TabulateGrievance> GetMappedRetotalingOfficers { get; set; }

        public string retotaling_marks { get; set; }
        public string retotaling_officer_kgid_id { get; set; }
        public string exam_name { get; set; }
        public string college_name { get; set; }
        public string comments { get; set; }

        public long trainee_roll_num { get; set; }
        public int subject_id { get; set; }
        public string exam_marks { get; set; }
        public List<TabulateGrievance> ViewDetails { get; set; }
        public int trainee_id { get; set; }
        public string subject_name { get; set; }
       // public string kgid { get; set; }
        public int trade_type_id { get; set; }
        public int oem_id { get; set; }
        public string trade_name { get; set; }
        public int? status_id { get; set; }
		public string statusdescp { get; set; }


		public string ExamCentre { get; set; }
		public string ExamCentreCode { get; set; }
		public string TradeName { get; set; }
		public string KgOfficer { get; set; }

        public int TradeId { get; set; }
        public int kgId { get; set; }
    }
    public class ExamFeesPayment
    {
        public int? selectTab { get; set; }
        //public int user_id { get; set; }
        public int CourseTypeId { get; set; }
        //public int ExamCenterId { get; set; }
        public int? TradeId { get; set; }
        public int SessionId { get; set; }
        public int? TradeTypeId { get; set; }
        //public int? LastReceiptNumber { get; set; }
        public int ExamTypeId { get; set; }
        public int? trade_type_id { get; set; }
        public int? TradeYearId { get; set; }
        public int? ShiftId { get; set; }
        public int? UnitId { get; set; }

        public int? YearOfAdmission { get; set; }
        public SelectList TradeSubjectList { get; set; }
        public SelectList ExamTypeList { get; set; }
        public SelectList TradeYearList { get; set; }
        public SelectList UnitList { get; set; }
        public SelectList ShiftList { get; set; }

        //public SelectList ExamCenterList { get; set; }
        public SelectList TradeList { get; set; }
        public SelectList TradeTypeList { get; set; }
        //public SelectList DivisionList { get; set; }
        //public SelectList DistrictList { get; set; }
        //public SelectList SpecialTradeTypeList { get; set; }
        public SelectList Session_Year_List { get; set; }
        public SelectList CourseTypeList { get; set; }
        public SelectList ExamNotificationList { get; set; }
        public List<CodeMarksSlip> objCodeMarksSlipItem { get; set; }
        public List<ExamFeesPayment> FeePaymentList { get; set; }
        public List<ExamFeesPayment> FeePaymentReport { get; set; }
        public List<ExamFeesPayment> TraineeAdmitCardList { get; set; }
        public List<ExamFeesPayment> StudentAdmitCardList { get; set; }
        public List<ExamFeesPayment> AdmitCardSubjectDetailsList { get; set; }
        public List<ExamFeesPayment> StudentDetails { get; set; }

        public int TraineeId { get; set; }
        public string TraineeName { get; set; }
        public long TraineeRollNum { get; set; }
        public string CourseName { get; set; }
        public string TradeTypeName { get; set; }
        public string TradeYear { get; set; }
        public string TradeName { get; set; }
        public string FatherName { get; set; }
        public int? ExamFees { get; set; }
        public int FeesPaidAmount { get; set; }
        public bool? RegularRepeter { get; set; }
        public int User_Id { get; set; }
        public int roleId { get; set; }
        public string Eligibility { get; set; }

        public bool? Is_Paid { get; set; }
        public int? TraineeAtt { get; set; }
        public bool? Discharge { get; set; }
        public string ITICollegeName { get; set; }
        public string ReceiptNumber { get; set; }
        public int DifferenceInDays { get; set; }
        public string MISCode { get; set; }
        public string LOCMONYYYY { get; set; }
        public DateTime FeesPaidDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ITICollegeCode { get; set; }
        public int Exam_CalNotif_Number { get; set; }
        public int FeeStatusid { get; set; }
        //public string FeesStatus { get; set; }
        public SelectList FeesStatusList { get; set; }

        public int NCVT_ED_TP_id { get; set; }
        public SelectList NcvtEdTpList { get; set; }
        public int CourseTypeId1 { get; set; }
        public int TradeTypeId1 { get; set; }
        public int SessionId1 { get; set; }
        public int TradeYearId1 { get; set; }
        public int trade_type_id1 { get; set; }
		
        public string ExamCenterName { get; set; }
        public string ExamCenterCode { get; set; }
        public string Res { get; set; }
        public string TradeCode { get; set; }

        public string AdmitCardSubjectDetails { get; set; }
        public int AdmitCardSubjectDetailsId { get; set; }
        public DateTime AdmitCardExamDate { get; set; }
        public DateTime AdmitCardExamTime { get; set; }
    }
    public class ExamQuestionPaper
    {
        public int? selectTab { get; set; }
        public int User_Id { get; set; }
        public int roleId { get; set; }
        public int TradeId { get; set; }
        public SelectList Session_Year_List { get; set; }
        public SelectList CourseTypeList { get; set; }
        public SelectList TradeList { get; set; }
        public SelectList TradeNameList { get; set; }
        public SelectList TradeTypeList { get; set; }
        public SelectList TradeYearList { get; set; }
        public SelectList TradeSubjectList { get; set; }
        public SelectList NcvtEdTpList { get; set; }
        public int qp_id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectIdstring { get; set; }
        public int SubjectTypeId { get; set; }
        public int SubjectTypeId1 { get; set; }
        public SelectList QuestionPaperSet { get; set; }
        public int TradeTypeId { get; set; }
        public int CourseTypeId { get; set; }
        public int TradeYearId { get; set; }
        public int? TradeYearId2 { get; set; }
        public int TradeYearId4 { get; set; }
        public int trade_type_id { get; set; }
        public SelectList SubjectType { get; set; }
        public int NCVT_ED_TP_id { get; set; }
        public string QPFilePath { get; set; }
        public HttpPostedFileBase UploadDocs { get; set; }
        public int TradeTypeId1 { get; set; }
        public int CourseTypeId1 { get; set; }
        public int TradeYearId1 { get; set; }
        public int trade_type_id1 { get; set; }
        public int SubjectId1 { get; set; }
        public int QPSetTransTableId { get; set; }
        public List<ExamQuestionPaper> UploadedQPList { get; set; }
        public List<ExamQuestionPaper> UploadedQPCount { get; set; }
        public List<ExamQuestionPaper> QPReUpload { get; set; }
        public string CourseName { get; set; }
        public string TradeTypeName { get; set; }
        public string TradeYear { get; set; }
        public string TradeName { get; set; }
        public string QuestionPaperSetName { get; set; }
        public string SubjectName { get; set; }
        public string SubjectTypeName { get; set; }
        public string filename { get; set; }
        public DateTime? QPUploadedDate { get; set; }
        public int CourseTypeId5 { get; set; }
        public int TradeTypeId5 { get; set; }
        public int TradeYearId5 { get; set; }
        public int TradeYearId6 { get; set; }
        public int trade_type_id5 { get; set; }
        public int SubjectTypeId5 { get; set; }
        public int SubjectId5 { get; set; }
        public int CourseTypeId7 { get; set; }
        public int TradeTypeId7 { get; set; }
        public int TradeYearId7 { get; set; }
        public int TradeYearId9 { get; set; }
        public int trade_type_id7 { get; set; }
        public int SubjectTypeId7 { get; set; }
        public int SubjectId7 { get; set; }
        public int QPStatus { get; set; }
        public bool? SentToEC { get; set; }
        public int UnusedQPCount { get; set; }
        public int UsedQPCount { get; set; }
        public int ReqQpToUpload { get; set; }
        public bool? IsQPSentToExamCenter { get; set; }
        public int TotalQpUploadedForEachTrade { get; set; }
        public string Comments { get; set; }
        public string creationdatetime { get; set; }
        public int user_name { get; set; }
        public string By { get; set; }
        public string Status { get; set; }
        public List<ExamQuestionPaper> CommentList { get; set; }
        public bool result { get; set; }
    }
}
