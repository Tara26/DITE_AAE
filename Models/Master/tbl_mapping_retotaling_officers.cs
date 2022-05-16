using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_mapping_retotaling_officers
    {
        [Key]
        public int id { get; set; }
        public int kgid_id { get; set; }
        public int exam_year { get; set; }
        public int trade_id { get; set; }
        public bool? is_active { get; set; }
        public int? created_by { get; set; }
        public DateTime? creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updation_datetime { get; set; }
    }

    public class tbl_RetotallingOfficer_Map
    {
        [Key]
        public int ID { get; set; }
        public string Kgid_number { get; set; }
        public int session_year { get; set; }
        public string unit_description { get; set; }
        public string Name_Of_Officer { get; set; }
        public string Designation { get; set; }
        
        public int unit_id { get; set; }
        
        public bool? isActive { get; set; }
        public int? created_by { get; set; }
        public DateTime? creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updation_datetime { get; set; }

    }
}
