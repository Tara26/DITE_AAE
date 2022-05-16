using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_grievance_approval
    {
        [Key]
        public int ga_id { get; set; }
        public int ga_exam_year { get; set; }
        public int ga_status { get; set; }
        public int ga_login_id { get; set; }
        public int ga_Cource_type { get; set; }

        public bool ga_is_active { get; set; }
        public int? ga_created_by { get; set; }
        public DateTime? ga_creation_datetime { get; set; }
        public int? ga_updated_by { get; set; }
        public DateTime? ga_updation_datetime { get; set; }
    }
}
