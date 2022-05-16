using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_user_mapping
    {
        [Key]
        public int user_map_id { get; set; }
        public int role_id { get; set; }
        public int um_id { get; set; }
        public int dept_id { get; set; }
        public int? sub_dept_id { get; set; }
        public bool is_active { get; set; }
        public int? created_by { get; set; }
        public DateTime creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public DateTime updation_datetime { get; set; }
    }
}
