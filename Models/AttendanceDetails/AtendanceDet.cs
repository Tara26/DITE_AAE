using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Models.AttendanceDetails
{
    public class AtendanceDet
    {
        public int divId { get; set; }
        // public string divDesc{ get; set; }
        public int attenId { get; set; }
        public int ExamCntrId { get; set; }
        public int User_Id { get; set; }
        public int TradeId { get; set; }
        public int TradeTypeId { get; set; }
        public int sublectId { get; set; }
        public string divDesc { get; set; }
        // public string UploadDocs { get; set; }
        public string ExamCntrDesc { get; set; }
        public SelectList divisionList { get; set; }
        public SelectList ExamCentrList { get; set; }
        public HttpPostedFileBase UploadPdf { get; set; }
        public HttpPostedFileBase UploadXcel { get; set; }
        //public DateTime? ExamDate { get; set; }
        public string ExamDate { get; set; }
        public string Day { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string TradeYear { get; set; }
        public string Subject { get; set; }
        public string BlockNo { get; set; }
        public int? Trainee_Roll_Num { get; set; }
        public string attendSavePath { get; set; }
        public string attendExcelPath { get; set; }
        public string InvigilatorSacannedCopy { get; set; }
        public string InvigilatorDairyEditableCopy { get; set; }
        public string SelectCheckBox { get; set; }
        public string status_desc { get; set; }
        public int RoleId { get; set; }
        public int status_id { get; set; }
        public int? login_id { get; set; }
        public SelectList LoginRoleList { get; set; }
        public int Exam_Notif_Id { get; set; }
        public int exam_notif_status_id { get; set; }
        public List<AtendanceDet> AttendanceDetList { get; set; }
        public List<AtendanceDet> attendanceList { get; set; }
        public int? ECT_Id { get; set; }
        public DateTime? ECT_ExamDate { get; set; }
        public DateTime? ECT_Exam_Start_Time { get; set; }
        public DateTime? ECT_Exam_End_Time { get; set; }
        public string TradeYr { get; set; }
        public int? ECT_Sub_Id { get; set; }
        public int? StatusId { get; set; }
        public string comments { get; set; }
        public int SelectedRoleId { get; set; }
        public int exam_status_id { get; set; }
        public int updatestatusCW { get; set; }
        public string attendance { get; set; }
        public int AdditionalSheetNo { get; set; }
        public int AnswerSheetNo { get; set; }
        public int ExamDuration { get; set; }
        public int ITI_trainees_Id { get; set; }
        public int Block_Id { get; set; }
        public int Procedure_or_Drawing_Sheet_No { get; set; }
        public int Status_Id { get; set; }
        public bool Is_Active {get;set;}
        public int created_by { get; set; }
        public DateTime creation_datetime { get; set; }
        public int updated_by { get; set; }
        public DateTime updation_datetime { get; set; }
        public string adt_remarks { get; set; }
    }

    public class OMRSheetDetails
    {
        public int division_Id { get; set; }
        public int Exam_Cntr_Id { get; set; }
        public int User_Id { get; set; }
        public int Trade_Id { get; set; }
        public int Trade_Type_Id { get; set; }
        public int subject_Id { get; set; }
        public DateTime Exam_Date { get; set; }
        public string EExam_Date { get; set; }
        public string Exam_Start_date { get; set; }
        public string Exam_End_date { get; set; }

        public string Day { get; set; }
        public string TradeYr { get; set; }
        public string Subject { get; set; }
        public int RoomNo { get; set; }

        public SelectList Division_List { get; set; }
        public SelectList Exam_Centr_List { get; set; }
        public List<OMRSheetDetails> OMRSheetDetailsList { get; set; }
    }
}
