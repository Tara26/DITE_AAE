using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_exam_subject_trans
    {
        [Key]
        public int est_id { get; set; }
        public DateTime est_exam_date { get; set; }
        public DateTime est_exam_start_date { get; set; }
        public DateTime exam_end_date { get; set; }
        public int? est_exam_subject_type_id { get; set; }
        public string est_exam_subject_id { get; set; }
        public int est_transactionid { get; set; }
        public int est_course_id { get; set; }
        public int est_trade_id { get; set; }
        public int est_trade_type_id { get; set; }
        public int est_exam_type_id { get; set; }
        public int est_trade_year_id { get; set; }
        public int est_subject_type_id { get; set; }
        public int exam_semester_id { get; set; }
        public int est_exam_year { get; set; }
        //public int exam_year { get; set; }
        public string est_file_path { get; set; }
        public string code_marksslip_filepath { get; set; }
        public string practical_evaluation_sheet_filepath { get; set; }

        public bool? est_is_active { get; set; }
        public Nullable<DateTime> est_creation_datetime { get; set; }
        public Nullable<int> est_created_by { get; set; }
        public Nullable<DateTime> est_updation_datetime { get; set; }
        public Nullable<int> est_updated_by { get; set; }

    }

    public class tbl_exam_centre_mapping_trns
    {
        [Key]
        public int ecmt_id { get; set; }
        public int? ecmm_id { get; set; }
        public int? exam_centre_id { get; set; }
        public int? iti_college_id { get; set; }
        public int? division_id { get; set; }
        public int? distict_id { get; set; }
        public int? status_id { get; set; }
        public int? login_id { get; set; }
        public bool? ecmt_is_active { get; set; }
        public int? ecmt_created_by { get; set; }
        public Nullable<DateTime> ecmt_creation_datetime { get; set; }
        public int? ecmt_updated_by { get; set; }
        public Nullable<DateTime> ecmt_updation_datetime { get; set; }


    }
}
