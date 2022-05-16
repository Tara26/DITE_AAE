using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Models.Master
{
    public class tbl_exam_fees_status
    {
        [Key]
        public int exam_fees_id { get; set; }
        public string exam_fees_status { get; set; }
        public bool? efs_is_active { get; set; }
        public int? efs_created_by { get; set; }
        public DateTime? efs_creation_datetime { get; set; }
        public int? efs_updated_by { get; set; }
        public DateTime? efs_updation_datetime { get; set; }

    }
}
