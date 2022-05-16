using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class tbl_question_paper_set_trns
    {
        [Key]
        public int qpst_id { get; set; }
        public string subject_id { get; set; }
        public int exam_calendar_id { get; set; }
        public int status_id { get; set; }
        public string qpst_remarks { get; set; }
        public string qpst_file_path { get; set; }
        public string trade_group_id { get; set; }
        public int exam_type_id { get; set; }
        public int trade_year_id { get; set; }
        public int subject_type_id { get; set; }
        public int course_id { get; set; }
        public int trade_type_id { get; set; }
        public int trade_id { get; set; }
        public int qp_set_id { get; set; }
        public int? second_year { get; set; }
        public bool? qpst_is_active { get; set; }
        public Nullable<int> qpst_created_by { get; set; }
        public Nullable<DateTime> qpst_creation_datetime { get; set; }
        public Nullable<int> qpst_updated_by { get; set; }
        public Nullable<DateTime> qpst_updation_datetime { get; set; }
    }

    public class tbl_questionpaper_set
    {
        [Key]
        public int qp_id { get; set; }
        public string qp_set { get; set; }
        public bool is_active { get; set; } 

    }
}
