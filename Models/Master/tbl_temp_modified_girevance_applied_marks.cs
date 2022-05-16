using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_temp_modified_girevance_applied_marks
    {
        [Key]
        public int? mdgd_id { get; set; }
        public int? gd_id { get; set; }
        public long mdgd_trainee_roll_num { get; set; }
        public int mdgd_marks { get; set; }
        public string mdgd_marks_words { get; set; }
        public int mdgd_subject_id { get; set; }
        public string mdgd_remarks { get; set; }
        public bool mdgd_is_verified { get; set; }
        public bool  mdgd_is_active { get; set; }
        public int? mdgd_created_by { get; set; }
        public DateTime?  mdgd_creation_datetime { get; set; }
        public int? mdgd_updated_by { get; set; }
        public DateTime? mdgd_updation_datetime { get; set; }
        public int mdgd_status_id { get; set; }
        public int mgd_login_id { get; set; }
    }
}
