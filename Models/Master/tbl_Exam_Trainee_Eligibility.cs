using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Exam_Trainee_Eligibility
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int RegularOrRepeater { get; set; }
        public int AttendenceEligibility { get; set; }
        public int NoOfAttempts { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int IsActive { get; set; }
        public int RoleID { get; set; }
        public Int32 Trainee_ID { get; set; }
    }

    public class tbl_Exam_Fee_RegularAndRepeater
    {
        [Key]
        public int RegularOrRepeater { get; set; }
        public int Regular_ExamFee { get; set; }
        public int Repeater_ExamFee { get; set; }
        public int LateFee_10Days { get; set; }
        public int LateFee_20Days { get; set; }
        public int LateFee_30Days { get; set; }
    }

    //public class tbl_exam_centre_mapping_mast
    //{
    //    [Key]
    //    public int ecmm_id { get; set; }
    //    public int exam_centre_id { get; set; }
    //    public int iti_college_id { get; set; }
    //    public int trainee_id { get; set; }
    //    public int is_active { get; set; }
    //    public int created_by { get; set; }
    //    public DateTime creation_datetime { get; set; }
    //    public int updated_by { get; set; }
    //    public DateTime updation_datetime { get; set; }
    //    public int ecmm_status_id { get; set; }
    //    public string ecmm_remarks { get; set; }
    //    public string exam_centre_mapping_number { get; set; }
    //    public int course_id { get; set; }
    //    public int login_id { get; set; }
    //    public int division_id { get; set; }
    //    public string ec_name { get; set; }
    //    public string college_name { get; set; }
    //    public int district_id { get; set; }
    //    public int exam_notif_status_id { get; set; }
    //}


    public class tbl_SaveApproved_QP_by_HofQPC
    {
        [Key]
        public int Id { get; set; }
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
    }

}
