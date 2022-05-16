using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_obtained_exam_marks_after_grievance
    {
        [Key]
        public int oemag_id { get; set; }
        public int oemag_subject_type_id { get; set; }
        public long oemag_trainee_roll_num { get; set; }
        public int oemag_course_id { get; set; }
        public int oemag_trade_type_id { get; set; }
        public int oemag_trade_year_id { get; set; }
        public int oemag_trade_id { get; set; }
        public int oemag_exam_year { get; set; }
        public int oemag_max_and_min_id { get; set; }
        public int oemag_new_exam_marks_obtained { get; set; }
        public string oemag_new_exam_marks_in_words { get; set; }
        public int oemag_new_grace_mark_obtained { get; set; }
        public string oemag_new_grace_mark_in_words { get; set; }
        public int oemag_new_total_mark_obtained { get; set; }
        public string oemag_new_total_mark_in_words { get; set; }
        public int oemag_created_by { get; set; }
        public DateTime oemag_creation_datetime { get; set; }
        public int oemag_updated_by { get; set; }
        public DateTime oemag_updation_datetime { get; set; }
        public bool oemag_is_active { get; set; }
        public bool is_pass { get; set; }
        //public int oemag_status_id { get; set; }

    }
}
