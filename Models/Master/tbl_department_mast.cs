using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_department_mast
    {
        [Key]
        public int dept_id { get; set; }
        public string dept_description { get; set; }
        public string dept_type { get; set; }
        public bool dept_is_active { get; set; }
        public int dept_created_by { get; set; }
        public DateTime dept_creation_datetime { get; set; }
        public int dept_updated_by { get; set; }
        public DateTime dept_updation_datetime { get; set; }
    }
}
