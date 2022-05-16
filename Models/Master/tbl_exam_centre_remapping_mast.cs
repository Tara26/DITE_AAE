using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_exam_centre_remapping_mast
    {
        [Key]
        public int ecrm_id { get; set; }
        public Nullable<int> exam_centre_id { get; set; }
        public Nullable<int> iti_college_id { get; set; }
        public Nullable<int> trainee_id { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> creation_datetime { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public Nullable<int> ecmm_status_id { get; set; }
        public string ecmm_remarks { get; set; }
        public string exam_centre_mapping_number { get; set; }
        public Nullable<int> course_id { get; set; }

        public Nullable<int> login_id { get; set; }
        public Nullable<int> division_id { get; set; }

        public string ec_name { get; set; }
        public string college_name { get; set; }
        public Nullable<int> district_id { get; set; }
        public int exam_notif_status_id { get; set; }
        public int session_year { get; set; }

    }
}
