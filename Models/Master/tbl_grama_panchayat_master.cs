using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class tbl_grama_panchayat_master
    {
        [Key]
        public int gp_id { get; set; }
        public string grama_panchayat_name { get; set; }
        public int gp_lgd_code { get; set; }
        public int? taluk_lgd_code { get; set; }
        public bool? gp_is_active { get; set; }
        public int? gp_created_by { get; set; }
        public DateTime? gp_created_on { get; set; }
        public int? gp_updated_by { get; set; }
        public DateTime? gp_updated_on { get; set; }
    }
}
