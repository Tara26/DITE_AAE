using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_omr_details_mast
    {
            [Key]
            public int omr_id { get; set; }
            public long? omr_sheet_number { get; set; }
            public long? omr_trainee_roll_num { get; set; }
            public int omr_block_id { get; set; }
            public int omr_exam_cal_notif_id { get; set; }
        public int omr_exam_calendar_id { get; set; }
        
            public int omr_division_id { get; set; }
            public int omr_exam_centre_id { get; set; }
            public int omr_status_id { get; set; }
            public string omr_remarks { get; set; }
            public bool? omr_is_active { get; set; }
            public int? omr_created_by { get; set; }
            public DateTime? omr_creation_datetime { get; set; }
            public int? omr_updated_by { get; set; } 
            public DateTime?  omr_updation_datetime { get; set; }
            public int? omr_year { get; set; }
            public int omr_exam_notif_status_id { get; set; }
            public int? omr_login_id { get; set; }
            public int? omr_trade_id { get; set; }
            public int? omr_trade_type_id { get; set; }
            public int? omr_exam_subject_type_id { get; set; }
        public DateTime omr_exam_date { get; set; }
        public string omr_invigilator_name { get; set; }

    }

    public class tbl_omr_details_trans
    {
        [Key]
        public int omrt_id { get; set; }
        public int omr_id { get; set; }
        public long? omrt_sheet_number { get; set; }
        public long? omrt_trainee_roll_num { get; set; }
        public int omrt_block_id { get; set; }
        public int omrt_exam_cal_notif_id { get; set; }
        public int omrt_division_id { get; set; }
        public int omrt_exam_centre_id { get; set; }
        public int omrt_status_id { get; set; }
        public string omrt_remarks { get; set; }
        public bool? omrt_is_active { get; set; }
        public int? omrt_created_by { get; set; }
        public DateTime? omrt_creation_datetime { get; set; }
        public int? omrt_updated_by { get; set; }
        public DateTime? omrt_updation_datetime { get; set; }
        public int? omrt_year { get; set; }
        public int? omrt_exam_notif_status_id { get; set; }
        public int?  omrt_login_id { get; set; }
        public int? omr_trade_id { get; set; }
        public int? omr_trade_type_id { get; set; }
        public int? omr_exam_subject_type_id { get; set; }
        public DateTime omr_exam_date { get; set; }


    }

    public class tbl_omr_marks_master
    {
        [Key]
        public int omrm_marks_id { get; set; }
        public int? omrm_omr_id { get; set; }
        public int? omrm_marks { get; set; }
        public int? omrm_status_id { get; set; }
        public string omrm_remarks { get; set; }
        public bool? omrm_is_active { get; set; }
        public int? omrm_created_by { get; set; }
        
        public DateTime? omrm_creation_datetime { get; set; }
        public int? omrm_updated_by { get; set; }
        public DateTime? omrm_updation_datetime { get; set; }
        public int? omrm_subject_type_id { get; set; }

    }

    public class tbl_omr_marks_details
    {
        [Key]
        public int omd_id { get; set; }
        public long? omd_sheet_number { get; set; }
        public int? omd_trainee_roll_num { get; set; }
        public int omd_block_id { get; set; }
        public int omd_exam_cal_notif_id { get; set; }
        public int omd_division_id { get; set; }
        public int omd_exam_centre_id { get; set; }
        public int omd_status_id { get; set; }
        public string omd_remarks { get; set; }
        public bool? omd_is_active { get; set; }
        public int? omd_created_by { get; set; }
        public DateTime? omd_creation_datetime { get; set; }
        public int? omd_updated_by { get; set; }
        public DateTime? omd_updation_datetime { get; set; }
        public int? omd_year { get; set; }
        public int omd_exam_notif_status_id { get; set; }
        public int? omd_login_id { get; set; }
        public int? omd_trade_id { get; set; }
        public int? omd_trade_type_id { get; set; }
        public int? omd_exam_subject_type_id { get; set; }
        public DateTime omd_exam_date { get; set; }

    }
}
