using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_Exam_Cal_Notif_mast
    {
        [Key]
        public int ecn_id { get; set; }
        public int exam_notif_id { get; set; }
        public string ecn_desc { get; set; }
        public DateTime ecn_Date { get; set; }
        public string ecn_type { get; set; }
        public int exam_cal_status_id { get; set; }
        public int? login_id { get; set; }
        public int ecn_is_published { get; set; }
        public int? course_type_id { get; set; }
        public string exam_notif_file_path { get; set; }


        public bool? ecn_is_active { get; set; }
        public Nullable<DateTime> ecn_creation_datetime { get; set; }
        public Nullable<int> ecn_created_by { get; set; }
        public Nullable<DateTime> ecn_updation_datetime { get; set; }
        public Nullable<int> ecn_updated_by { get; set; }
    }

    public class tbl_Exam_Cal_Notif_trans
    {
        [Key]
        public int ecnt_id { get; set; }
        public int exam_cal_notif_id { get; set; }
        public int exam_cal_notif_status_id { get; set; }
        public Nullable<DateTime> trans_date { get; set; }
        public bool? is_active { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<DateTime> creation_datetime { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public Nullable<int> updated_by { get; set; }
    }
}
