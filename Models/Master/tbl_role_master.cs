using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_role_master
    {
        [Key]
        public int role_id { get; set; }
        public string role_description { get; set; }
        public string role_DescShortForm { get; set; }
        public int? role_seniority_no { get; set; }
        public bool? role_is_active { get; set; }
        public int? role_created_by { get; set; }
        public DateTime? role_creation_datetime { get; set; }
        public int? role_updated_by { get; set; }
        public DateTime? role_updation_datetime { get; set; }
        public int? role_Level { get; set; }
    }
}
