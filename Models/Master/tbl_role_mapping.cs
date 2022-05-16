using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_role_mapping
    {
        [Key]
        public int rm_id { get; set; }
        public int role_id { get; set; }
        public int division_id { get; set; }
        public int dept_id { get; set; }
        public int sub_dept_id { get; set; }
        public bool rm_create { get; set; }
        public bool rm_edit { get; set; }
        public bool rm_delete { get; set; }
        public bool rm_view { get; set; }
        public bool rm_is_active { get; set; }
        public int rm_created_by { get; set; }
        public DateTime rm_creation_time { get; set; }
        public int rm_wing_id { get; set; }
    }
}
