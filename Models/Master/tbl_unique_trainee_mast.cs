using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_unique_trainee_mast
    {
        [Key]
        public int ut_id { get; set; }
        public int? ut_ec_id { get; set; }
        public int ut_tfp_id { get; set; }
        public int? ut_attendance_id { get; set; }
        public string unique_identification_code { get; set; }

        public bool? ut_is_active { get; set; }
        public Nullable<DateTime> ut_creation_datetime { get; set; }
        public Nullable<int> ut_created_by { get; set; }
        public Nullable<DateTime> ut_updation_datetime { get; set; }
        public Nullable<int> ut_updated_by { get; set; }
    }
}
