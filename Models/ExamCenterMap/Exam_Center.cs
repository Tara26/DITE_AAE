using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Exam_Center
    {
        public int ec_id { get; set; }
        public string ec_name { get; set; }
        public string ec_email_id { get; set; }
        public string ec_phone_num { get; set; }
        public Nullable<int> division_id { get; set; }
        public Nullable<int> district_id { get; set; }
        public Nullable<int> taluk_id { get; set; }
        public Nullable<bool> ec_is_active { get; set; }
        public Nullable<int> ec_created_by { get; set; }
        public Nullable<System.DateTime> ec_creation_datetime { get; set; }
        public Nullable<int> ec_updated_by { get; set; }
        public Nullable<System.DateTime> ec_updation_datetime { get; set; }
        public Nullable<int> ec_status_id { get; set; }
        public string ec_remarks { get; set; }
        public string ec_code { get; set; }


    }
}
