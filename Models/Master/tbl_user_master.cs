using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_user_master
    {
        [Key]
        public int um_id { get; set; }
        public string um_name { get; set; }
        public string um_email_id { get; set; }
        public string um_mobile_no { get; set; }
        public string um_password { get; set; }
        public bool um_is_active { get; set; }
        public int? um_created_by { get; set; }
        public DateTime? um_creation_datetime { get; set; }
        public int? um_updated_by { get; set; }
        public int? um_div_id { get; set; }
        public DateTime? um_updation_datetime { get; set; }
        public int? um_kgid_number { get; set; }
        public DateTime? um_dob { get; set; }
        public string um_photo { get; set; }
        public string um_father_name { get; set; }
        public int um_gender { get; set; }
        public string um_designation { get; set; }
        public int role_id { get; set; }
    }
}
