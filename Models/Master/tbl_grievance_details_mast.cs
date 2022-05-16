using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
	public class tbl_grievance_details_mast
	{
        [Key]
        public int? gd_id {get; set;}
		public int gd_er_id { get; set; }
		public int gd_subject_id { get; set; }
		public int gd_ec_id { get; set; }
		public long gd_trainee_roll_num { get; set; }
		public string upload_grievance_proof { get; set; }
		public int gd_status_id { get; set; }
		public string gd_remarks { get; set; }
		public bool gd_is_active { get; set; }
		public int gd_created_by { get; set; }
		public DateTime? gd_creation_datetime { get; set; }
		public int gd_updated_by { get; set; }
		public DateTime? gd_updation_datetime { get; set; }
		public bool? gd_payment_status { get; set; }
        public string gd_receipt_number { get; set; }
        public string gd_application_number { get; set; }
        public decimal gd_retotalling_fee { get; set; }
        public int exam_notif_status_id { get; set; }
        public int? gd_login_id { get; set; }
        public string gr_upload_pdf_file { get; set; }
        public long? treasury_receipt_num { get; set; }
        public string ITI_MisCode { get; set; }

    }

	public class tbl_exam_result_mast
	{
        [Key]
		public int er_id { get; set; }
		public int er_omrm_id { get; set; }
		public int er_obtained_marks_id { get; set; }
		public int er_obtained_exam_marks_id { get; set; }
		public int er_rs_id { get; set; }
		public int er_status_id { get; set; }
		public string er_remarks { get; set; }
		public bool er_is_active { get; set; }
		public int er_created_by { get; set; }
		public DateTime? er_creation_datetime { get; set; }
		public int er_updated_by { get; set; }
		public DateTime? er_updation_datetime { get; set; }
	}

    public class tbl_grievance_results_master
    {
        [Key]
        public int grm_id { get; set; }
        public int grm_gd_id { get; set; }
        public int grm_er_id { get; set; }
        public string upload_proof { get; set; }
        public int grm_rs_id { get; set; }
        public int grm_status_id { get; set; }
        public string grm_remarks { get; set; }
        public bool grm_is_active { get; set; }
        public int grm_created_by { get; set; }
        public DateTime? grm_creation_datetime { get; set; }
        public int grm_updated_by { get; set; }
        public DateTime? grm_updation_datetime { get; set; }
    }
   
}
