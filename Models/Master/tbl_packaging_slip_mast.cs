using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_packaging_slip_mast
    {
        [Key]
        public int ps_id { get; set; }
        public string packing_slip_code { get; set; }
        public int ps_trade_id { get; set; }
        public int ps_subject_id { get; set; }
        public int ps_center_id { get; set; }

        public bool ps_is_active { get; set; }
        public int ps_created_by { get; set; }
        public DateTime? ps_creation_datetime { get; set; }
        public int ps_updated_by { get; set; }
        public DateTime? ps_updation_datetime { get; set; }
    }
}
