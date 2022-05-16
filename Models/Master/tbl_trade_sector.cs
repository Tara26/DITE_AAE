using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_trade_sector
    {
        [Key]
        public int trade_sector_id { get; set; }
        public string trade_sector { get; set; }
        public bool is_active { get; set; }
        public int created_by { get; set; }
        public DateTime? creation_datetime { get; set; }
        public int updated_by { get; set; }
        public DateTime? updation_datetime { get; set; }

    }
}
