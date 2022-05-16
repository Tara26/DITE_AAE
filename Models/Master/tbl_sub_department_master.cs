using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_sub_department_master
    {
        [Key]
        public int sub_dept_id { get; set; }
        public int dept_id { get; set; }
        public string sub_dept_description { get; set; }
        public int division_id { get; set; }
        public int district_lgd_code { get; set; }
        public int taluk_lgd_code { get; set; }
        public bool is_active { get; set; }
        public int created_by { get; set; }
        public DateTime creation_datetime { get; set; }
        public int updated_by { get; set; }
        public DateTime updation_datetime { get; set; }
    }
}
