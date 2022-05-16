using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
   public class tbl_division_master
    {
        [Key]
        public int division_id { get; set; }
        public string division_name { get; set; }
        public bool? division_is_active { get; set; }
        public DateTime? division_created_on { get; set; }
        public int? division_created_by { get; set;}
        public DateTime? division_updated_on { get; set; }
        public int? division_updated_by { get; set; }
        public int? Hyd_NonHyd_region_id { get; set; }
        public string division_kannada_name { get; set; }
    }
}
