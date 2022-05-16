using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_taluk_master
    {       
        [Key]
        public int taluk_lgd_code { get; set; }
        public int taluk_id { get; set; }
        public string taluk_ename { get; set; }
        public string taluk_kname { get; set; }        
        public int district_lgd_code { get; set; }
        public bool taluk_is_active { get; set; }
        public DateTime taluk_creation_datetime { get; set; }
        public DateTime taluk_updation_datetime { get; set; }
        public int? taluk_created_by { get; set; }
        public int? taluk_updated_by { get; set; }
    }
}
