using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class tbl_legacy_marks
	{
		[Key]
		public int lm_id { get; set; }
		public string trainee_roll_num { get; set; }
		public int subject_id { get; set; }
		public int lm_obtained_omr_marks { get; set; }
		public int lm_obtained_sessional_marks { get; set; }
		public int lm_obtained_offline_exam_marks { get; set; }
		public int lm_semester { get; set; }
		public int lm_exam_type { get; set; }
		public bool lm_is_active { get; set; }
		public int lm_created_by { get; set; }
		public DateTime? lm_creation_datetime { get; set; }
		public int lm_updated_by { get; set; }
		public DateTime? lm_updation_datetime { get; set; }
	}


	public class tbl_maximum_and_minimum_marks
	{
		[Key]
		public int mamm_id { get; set; }
		public int mamm_subject_id { get; set; }
		public int maximum_marks { get; set; }
		public int minimum_marks { get; set; }
		public bool mamm_is_active { get; set; }
		public int mamm_created_by { get; set; }
		public DateTime? mamm_creation_datetime { get; set; }
		public int mamm_updated_by { get; set; }
		public DateTime? mamm_updation_datetime { get; set; }

	}


	//public class tbl_iti_trainees_mast
	//{
	//	[Key]
	//	public int iti_trainees_id { get; set; }
	//	public string iti_code { get; set; }
	//	public string iti_trainee_name { get; set; }
	//	public bool iti_trainee_is_active { get; set; }
	//	public int iti_trainee_created_by { get; set; }
	//	public DateTime iti_trainee_creation_datetime { get; set; }
	//	public int iti_trainee_updated_by { get; set; }
	//	public DateTime? iti_trainee_updation_datetime { get; set; }
	//	public int roll_num { get; set; }

	//}


	

	


	public class tbl_subject
	{
		[Key]
		public int subject_id { get; set; }
		public string subject_name { get; set; }
		public int subject_marks_max { get; set; }
		public int? subject_marks_min { get; set; }
		public int subject_total_max { get; set; }
		public int subject_total_min { get; set; }
		public int paper_id { get; set; }
		public bool is_active { get; set; }
		public DateTime? creation_datetime { get; set; }
		public DateTime? updation_datetime { get; set; }
		public string created_by { get; set; }
		public string updated_by { get; set; }
		public int? trade_id { get; set; }
		public int? subject_type_id { get; set; }

	}


	public class tbl_semester
	{
		[Key]
		public int semester_id { get; set; }
		public string semester { get; set; }
		public bool is_active { get; set; }
		public int created_by { get; set; }
		public DateTime? creation_datetime { get; set; }
		public int updated_by { get; set; }
		public DateTime? updation_datetime { get; set; }

	}


	//public class tbl_units
	//{
	//	[Key]
	//	public int u_id { get; set; }
	//	public string units { get; set; }
	//	public bool u_is_active { get; set; }
	//	public int u_created_by { get; set; }
	//	public DateTime? u_creation_datetime { get; set; }
	//	public int u_updated_by { get; set; }
	//	public DateTime? u_updation_datetime { get; set; }

	//}

	//public class tbl_shifts
	//{
	//	[Key]
	//	public int s_id { get; set; }
	//	public string shifts { get; set; }
	//	public bool s_is_active { get; set; }
	//	public int s_created_by { get; set; }
	//	public DateTime? s_creation_datetime { get; set; }
	//	public int s_updated_by { get; set; }
	//	public DateTime? s_updation_datetime { get; set; }

	//}

	public class tbl_code_marks_sheet_master
	{
		[Key]
		public int cms_id { get; set; }
		public int exam_centre_id { get; set; }
		public int cms_ut_id { get; set; }
		public bool cms_is_active { get; set; }
		public int cms_created_by { get; set; }
		public DateTime? cms_creation_datetime { get; set; }
		public int cms_updated_by { get; set; }
		public DateTime? cms_updation_datetime { get; set; }
		public string code_marks_sheet_num { get; set; }

	}

	public class tbl_obtained_exam_marks_master
	{
		[Key]
		public int oem_id { get; set; }
		public int oem_subject_id { get; set; }
		public int oem_code_marks_sheet_id { get; set; }
		public int oem_trainee_roll_num { get; set; }
		public int oem_trade_scheme_id { get; set; }
		public int oem_trade_sector_id { get; set; }
		public int oem_trade_year_id { get; set; }
		public int oem_semester_id { get; set; }
		public int oem_unit_id { get; set; }
		public int oem_shift_id { get; set; }
		public string exam_marks { get; set; }
		public int oem_max_and_min_id { get; set; }
		public string oem_upload_file { get; set; }
		public int oem_status_id { get; set; }
		public string oem_remarks { get; set; }
		public bool oem_is_active { get; set; }
		public int oem_created_by { get; set; }
		public DateTime? oem_creation_datetime { get; set; }
		public int oem_updated_by { get; set; }
		public DateTime? oem_updation_datetime { get; set; }

	}


	//public class tbl_unique_trainee_mast
	//{
	//	[Key]
	//	public int ut_id { get; set; }
	//	public int ut_ec_id { get; set; }
	//	public int ut_tfp_id { get; set; }
	//	public int ut_attendance_id { get; set; }
	//	public bool ut_is_active { get; set; }
	//	public int ut_created_by { get; set; }
	//	public DateTime? ut_creation_datetime { get; set; }
	//	public int ut_updated_by { get; set; }
	//	public DateTime? ut_updation_datetime { get; set; }
	//	public string unique_identification_code { get; set; }

	//}


}
