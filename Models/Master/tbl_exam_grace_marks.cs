using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_exam_grace_marks
    {
        [Key]
        public int grace_marks_id { get; set; }
        public int gm_trainee_id { get; set; }
        public int gm_trade_type_id { get; set; }
        public int gm_trade_id { get; set; }
        public int gm_session_id { get; set; }
        public int gm_cource_type_id { get; set; }
        public int gm_special_type { get; set; }
        public int gm_subjected1 { get; set; }
        public int gm_obtained1 { get; set; }
        public int gm_grace_marks1 { get; set; }
        public int gm_subjected2 { get; set; }
        public int gm_obtained2 { get; set; }
        public int gm_grace_marks2 { get; set; }
        public int? gm_subjected3 { get; set; }
        public int? gm_obtained3 { get; set; }
        public int? gm_grace_marks3 { get; set; }
        public int? gm_subjected4 { get; set; }
        public int? gm_obtained4 { get; set; }
        public int? gm_grace_marks4 { get; set; }
        public int? gm_subjected5 { get; set; }
        public int? gm_obtained5 { get; set; }
        public int? gm_grace_marks5 { get; set; }
        public int? gm_subjected6 { get; set; }
        public int? gm_obtained6 { get; set; }
        public int? gm_grace_marks6 { get; set; }
       
        public bool? gm_is_active { get; set; }
        public int? gm_created_by { get; set; }
        public Nullable<DateTime> gm_creation_datetime { get; set; }
        public int? gm_updated_by { get; set; }
        public Nullable<DateTime> gm_updation_datetime { get; set; }
    }
}
