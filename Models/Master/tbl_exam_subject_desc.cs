using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_exam_subject_desc
    {
        [Key]
        public int esd_id { get; set; }
        public int esd_subject_id { get; set; }
        public string esd_subject_name { get; set; }
        public string esd_pdf_path { get; set; }
        public int esd_trade_id { get; set; }
        public int esd_trade_type_id { get; set; }
        public int esd_sub_trans_id { get; set; }
        public int esd_status { get; set; }
        public int esd_exam_year { get; set; }
        public int? exam_centre_id { get; set; }
        public int? division_id { get; set; }
        public int? district_id { get; set; }
        
        public bool? esd_is_active { get; set; }
        public int? esd_created_by { get; set; }
        public Nullable<DateTime> esd_created_on { get; set; }
        public int? esd_updated_by { get; set; }
        public Nullable<DateTime> esd_updated_on { get; set; }
    }
}
