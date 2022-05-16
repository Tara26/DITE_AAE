using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_trade_type_mast
    {
        [Key]
        public int trade_type_id { get; set; }
        public string trade_type_name { get; set; }
        public int trade_type_course_id { get; set; }

        public bool? trade_type_is_active { get; set; }
        public Nullable<DateTime> trade_type_creation_datetime { get; set; }
        public Nullable<int> trade_type_created_by { get; set; }
        public Nullable<DateTime> trade_type_updation_datetime { get; set; }
        public Nullable<int> trade_type_updated_by { get; set; }
    }
}
