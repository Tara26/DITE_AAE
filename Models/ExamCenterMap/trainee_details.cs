using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ExamCenterMap
{
    class trainee_details
    {
        public int trainee_id { get; set; }
        public string iti_college_code { get; set; }
        public string trainee_name { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> creation_datetime { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<System.DateTime> updation_datetime { get; set; }
        public string trainee_roll_num { get; set; }

    }
}
