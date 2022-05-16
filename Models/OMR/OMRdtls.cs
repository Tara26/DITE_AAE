using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Models.OMR
{
    public class OMRdtls
    {
        
            public int divId { get; set; }
           
            public int attenId { get; set; }
            public int ExamCntrId { get; set; }
            public int User_Id { get; set; }
            public int TradeId { get; set; }
            public int TradeTypeId { get; set; }
            public int sublectId { get; set; }
            public string divDesc { get; set; }
        //// public string UploadDocs { get; set; }
        //public string ExamCntrDesc { get; set; }
        public SelectList divisionList { get; set; }
        public SelectList ExamCentrList { get; set; }
        public SelectList SubjectList { get; set; }
        public HttpPostedFileBase UploadPdf { get; set; }
        public HttpPostedFileBase UploadXcel { get; set; }

        //public DateTime? ExamDate { get; set; }
        public DateTime? ExamDate { get; set; }
        public string Day { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string TradeYear { get; set; }
        public string Subject { get; set; }
        public string BlockNo { get; set; }
        //public int? Trainee_Roll_Num { get; set; }
        public string attendSavePath { get; set; }
        public string attendExcelPath { get; set; }
        public string InvigilatorSacannedCopy { get; set; }
        public string InvigilatorDairyEditableCopy { get; set; }
        public string SelectCheckBox { get; set; }

        public List<OMR.OMRdtls> OMRdtlsList { get; set; }
        public int? ECT_Id { get; set; }
        public DateTime? ECT_ExamDate { get; set; }
        public DateTime? ECT_Exam_Start_Time { get; set; }
        public DateTime? ECT_Exam_End_Time { get; set; }
        public string TradeYr { get; set; }
        public int? ECT_Sub_Id { get; set; }
        public int? StatusId { get; set; }




    }
}
