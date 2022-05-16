using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_district_master
    {
        [Key]
        public int district_lgd_code { get; set; }
        public string district_ename { get; set; }
        //public int dis_is_active { get; set; }
        public int division_id { get; set; }
        public string dist_name { get; set; }
        public bool? is_active { get; set; }
        public int? created_by { get; set; }
        public DateTime? creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updation_datetime { get; set; }

        public int district_id { get; set; }
        //public int district_ename { get; set; }
        public string district_kname { get; set; }
        //public int district_lgd_code { get; set; }
        public int state_code { get; set; }
        //public int division_id { get; set; }
        public bool dis_is_active { get; set; }
        public DateTime dis_creation_datetime { get; set; }
        public DateTime dis_updation_datetime { get; set; }
        public int dis_created_by { get; set; }
        public int dis_updated_by { get; set; }
}
}
