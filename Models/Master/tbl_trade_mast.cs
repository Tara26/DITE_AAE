using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_trade_mast
    {
        [Key]
        public int trade_id { get; set; }
        public string trade_name { get; set; }
        public int trade_type_id { get; set; }
        public int trade_course_id { get; set; }

        public bool? trdae_is_active { get; set; }
        public Nullable<DateTime> trade_creation_date { get; set; }
        public Nullable<int> trade_created_by { get; set; }
        public Nullable<DateTime> trade_updation_date { get; set; }
        public Nullable<int> trade_updated_by { get; set; }
        public string trade_code { get; set; }
        public int? trade_unit { get; set; }
        public int? trade_seating { get; set; }
        public string trade_Mini_Qualification { get; set; }
        public string trade_duration { get; set; }
        public int? sector_id { get; set; }

        public string AidedUnaidedTrade { get; set; }

    }
}
