using Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ExamCenterMap
{
    public class Exam_Center_Map_Trans
    {
        public int esmt_id { get; set; }
        public Nullable<int> exam_centre_id { get; set; }
        public Nullable<int> iti_college_id { get; set; }
        public Nullable<int> trainee_roll_num { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> creation_datetime { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<System.DateTime> updation_datetime { get; set; }
        public Nullable<int> esmt_status_id { get; set; }
        public string esmt_remarks { get; set; }

        public virtual tbl_iti_college_details tbl_iti_college_details { get; set; }
        public virtual tbl_iti_trainees_details tbl_iti_trainees_details { get; set; }
    }
}
