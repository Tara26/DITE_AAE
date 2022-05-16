using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_trade_year_mast
    {
        [Key]
        public int trade_year_id { get; set; }
        public string trade_year_name { get; set; }

        public int trade_id { get; set; }
        public int trade_type_id { get; set; }

        public bool? trade_year_is_active { get; set; }
        public Nullable<DateTime> trade_year_creation_datetime { get; set; }
        public Nullable<int> trade_year_created_by { get; set; }
        public Nullable<DateTime> trade_year_updation_datetime { get; set; }
        public Nullable<int> trade_year_updated_by { get; set; }
    }
}
