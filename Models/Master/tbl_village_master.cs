using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
   public class tbl_village_master
    {
        [Key]
        public int village_id { get; set; }
        public int village_lgd_code { get; set; }
        public int? district_code { get; set; }
        public int? taluk_code { get; set; }
        public string village_ename { get; set; }
        public string village_kname { get; set; }
        public string panchayath_name { get; set; }
        public bool? village_is_active { get; set; }
        public DateTime? village_creation_date { get; set; }
        public DateTime? village_updation_date { get; set; }
        public int? village_created_by { get; set; }
        public int? village_updated_by { get; set; }
    }
}
