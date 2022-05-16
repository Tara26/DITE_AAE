using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class tbl_user_login
    {
        [Key]
        public int  id { get; set; }
      	public string user_name { get; set; }
        public string password { get; set; }
        public bool? is_active { get; set; }
        public bool?  is_print { get; set; }
        public bool?  is_duplicate { get; set; }
        public string comments_for_reprint { get; set; }
        public DateTime? creation_datetime { get; set; }
        public DateTime? updation_datetime { get; set; }
        public string created_by { get; set; }
        public string updated_by { get; set; }
        public int? print_status { get; set; }
        public int? count { get; set; }
        public string duplicate_reprint_remarks { get; set; }
        public int? approve_reprint { get; set; }
        public int? duplicate_print_approve { get; set; }
        public int? role_id { get; set; }
        public int? division_id { get; set; }
		public string kgid_num { get; set; }
        public string short_name_designation { get; set; }
        public int center_id { get; set; }
    }

    public class tbl_user_roles
    {
        [Key]
        public int user_id { get; set; }
        public string user_role { get; set; }
        public bool is_active { get; set; }
        public Nullable<DateTime> creation_datetime { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public string created_by { get; set; }
        public string updated_by { get; set; }
    }
}
