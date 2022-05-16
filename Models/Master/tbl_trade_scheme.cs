using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_trade_scheme
    {
        [Key]
        public int ts_id { get; set; }
        public string trade_scheme { get; set; }
        public bool ts_is_active { get; set; }
        public int ts_created_by { get; set; }
        public DateTime? ts_creation_datetime { get; set; }
        public int ts_updated_by { get; set; }
        public DateTime? ts_updation_datetime { get; set; }

    }
}
