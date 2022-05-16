using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_packaging_slip_trans
    {
        [Key]
        public int ps_trans_Id { get; set; }
        public int ps_id { get; set; }
        public int trainee_id { get; set; }
        public int slno { get; set; }
        
        public bool ps_is_active { get; set; }
        public int ps_created_by { get; set; }
        public DateTime? ps_created_on { get; set; }
        public int ps_updated_by { get; set; }
        public DateTime? ps_updated_on { get; set; }
    }
}
