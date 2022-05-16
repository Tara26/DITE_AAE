using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_obtained_sessional_marks_master
    {
        [Key]
        public int osm_id { get; set; }
        public int osm_subject_id { get; set; }
        public int? osm_sessional_test_id { get; set; }
        public long? trainee_roll_num { get; set; }
        public int? osm_trade_scheme_id { get; set; }
        public int? osm_trade_sector_id { get; set; }
        public int? osm_trade_year_id { get; set; }
        public int? osm_semester_id { get; set; }
        public int? osm_unit_id { get; set; }
        public int? osm_shift_id { get; set; }
        public int? sessional_marks { get; set; }
        public int? osm_max_and_min_id { get; set; }
        public string osm_upload_file { get; set; }
        public int? osm_status_id { get; set; }
        public string osm_remarks { get; set; }
        public bool? osm_is_active { get; set; }
        public int? osm_created_by { get; set; }
        public DateTime? osm_creation_datetime { get; set; }
        public int? osm_updated_by { get; set; }
        public DateTime? osm_updation_datetime { get; set; }
    }

    public class tbl_obtained_sessional_marks_trans
    {
        [Key]
        public int osmt_id { get; set; }
        public int? osmt_osm_id { get; set; }
        public int? osmt_subject_id { get; set; }
        public int? osmt_sessional_test_id { get; set; }
        public long? trainee_roll_num { get; set; }
        public int? osmt_trade_scheme_id { get; set; }
        public int? osmt_trade_sector_id { get; set; }
        public int? osmt_trade_year_id { get; set; }
        public int? osmt_semester_id { get; set; }
        public int? osmt_unit_id { get; set; }
        public int? osmt_shift_id { get; set; }
        public int? sessional_marks { get; set; }
        public int? osmt_max_and_min_id { get; set; }
        public string osmt_upload_file { get; set; }
        public int? osmt_status_id { get; set; }
        public string osmt_remarks { get; set; }
        public bool? osmt_is_active { get; set; }
        public int? osmt_created_by { get; set; }
        public DateTime? osmt_creation_datetime { get; set; }
        public int? osmt_updated_by { get; set; }
        public DateTime? osmt_updation_datetime { get; set; }
    }

    public class tbl_sessional_marks_details
    {
        [Key]
        public string smd_trainee_name { get; set; }
        public string smd_rollnumber { get; set; }
        public int? smd_trainee_id { get; set; }
        public float? smd_trade_theory_sessional_marks { get; set; }
        public float? smd_trade_practice_sessional_marks { get; set; }
        public float? smd_engineering_drawing_marks { get; set; }
        public float? smd_workshop_calculation_drawing_marks { get; set; }
        public bool? smd_is_active { get; set; }
        public int? smd_created_by { get; set; }
        public DateTime? smd_created_on { get; set; }
        public int? smd_updated_by { get; set; }
        public DateTime? smd_updated_on { get; set; }
    }


}

