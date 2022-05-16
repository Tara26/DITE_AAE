using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
   public class tbl_exam_notification_mast
    {
        [Key]
        public int exam_notif_id { get; set; }
        public string exam_notif_number { get; set; }
        public int course_id { get; set; }
        public int department_id { get; set; }
        //public string exam_notif_desc { get; set; }
        public int notif_type_id { get; set; }
        public string exam_notif_file_path { get; set; }
        public Nullable<DateTime> exam_notif_date { get; set; }
        public string exam_notif_type { get; set; }
        //public Nullable<DateTime> fee_pay_last_date { get; set; }
        public DateTime fee_pay_last_date { get; set; }
        public Nullable<DateTime> appli_from_last_date { get; set; }
        public Nullable<DateTime> trainee_name_eval_last_date { get; set; }
        public Nullable<DateTime> princ_sub_last_date { get; set; }

        public Nullable<DateTime> jd_sub_last_date { get; set; }
        public int? appli_charges_fee { get; set; }
        public int? exam_regular_fee { get; set; }
        public int? exam_repeater_fee { get; set; }
        public int? penalty_after10days_fee { get; set; }
        public Nullable<int> penalty_after30days_fee { get; set; }
        // public Nullable<DateTime> appli_from_last_date { get; set; }
        public int? penalty_after40days_fee { get; set; }
        public bool? is_active { get; set; }
        public Nullable<DateTime> creation_datetime { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<int> updated_by { get; set; }
		public int status_id { get; set; }
		public int? login_id { get; set; }
        public string  notif_description { get; set; }
        //To get the Signed PDF Path BNM  DSC
        public string signed_exam_notif_file_path { get; set; }
    }

    public class tbl_course_type_mast
    {
        [Key]
        public int course_id { get; set; }
        public string course_type_name { get; set; }
        public bool? is_active { get; set; }
        public Nullable<DateTime> creation_datetime { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<int> updated_by { get; set; }

    }
	
		/// <summary>
	/// Exam calender notification Master
	/// </summary>
	public class tbl_exam_cal_notif_mast
	{
		[Key]
		public int course_id { get; set; }
		public string course_type_name { get; set; }
		public string ecn_desc { get; set; }
		public DateTime? ecn_date { get; set; }
		public string ecn_type { get; set; }
		public char ecn_is_active { get; set; }
		public int ecn_created_by { get; set; }
		public int ecn_updated_by { get; set; }
		public Nullable<DateTime> ecn_creation_datetime { get; set; }
		public Nullable<DateTime> ecn_updation_datetime { get; set; }

	}

	/// <summary>
	/// Exam calender notification status master
	/// </summary>

	public class tbl_exam_cal_notif_status_mast
	{
		[Key]
		public int ecns_id { get; set; }
		public string ecns_desc { get; set; }
		public char ecns_is_active { get; set; }
		public int ecns_created_by { get; set; }
		public int ecns_updated_by { get; set; }
		public Nullable<DateTime> ecns_creation_datetime { get; set; }
		public Nullable<DateTime> ecns_updation_datetime { get; set; }
		
	}

	/// <summary>
	/// Exam calender notification transaction
	/// </summary>

	public class tbl_exam_cal_notif_trans
	{
		[Key]
		public int ecnt_id { get; set; }
		public int exam_cal_notif_id { get; set; }
		public int exam_cal_notif_status_id { get; set; }
		public Nullable<DateTime> trans_date { get; set; }
		public bool? is_active { get; set; }
		public int? created_by { get; set; }
		public int? updated_by { get; set; }
		public Nullable<DateTime> creation_datetime { get; set; }
		public Nullable<DateTime> updation_datetime { get; set; }

	}


	/// <summary>
	/// Exam calendar master
	/// </summary>
	public class tbl_exam_calendar_mast
	{
		[Key]
		public int exam_calender_id { get; set; }
		public string exam_calender_desc { get; set; }
		public int exam_type_id { get; set; }
		public char exam_calender_is_active { get; set; }
		public int exam_calender_created_by { get; set; }
		public int exam_calender_updated_by { get; set; }
		public Nullable<DateTime> exam_calender_creation_datetime { get; set; }
		public Nullable<DateTime> exam_calender_updation_datetime { get; set; }

	}

	/// <summary>
	/// 
	/// </summary>
	public class tbl_exam_calendar_trans
	{
		[Key]
		public int ect_id { get; set; }
		public int ect_exam_calendar_id { get; set; }
		public string ect_exam_desc { get; set; }
		public int ect_exam_type_id { get; set; }
		public int ect_course_id { get; set; }
		public int ect_trade_id { get; set; }
		public int ect_exam_year { get; set; }
		public Nullable<DateTime> ect_exam_date { get; set; }
		public Nullable<DateTime> ect_exam_start_time { get; set; }
		public Nullable<DateTime> ect_exam_end_time { get; set; }
		public int ect_exam_semester_id { get; set; }
		public int ect_subject_id { get; set; }
		public int ect_trade_type_id { get; set; }
		public int ect_trade_year_id { get; set; }
		public int ect_subject_type_id { get; set; }
		public char ect_is_active { get; set; }
		public int ect_created_by { get; set; }
		public int ect_updated_by { get; set; }
		public Nullable<DateTime> ect_creation_datetime { get; set; }
		public Nullable<DateTime> ect_updation_datetime { get; set; }
	}


	/// <summary>
	/// Exam notification history transaction
	/// </summary>
	public class tbl_exam_notif_history_trans
	{
		[Key]
		public int exam_notif_hist_seq_no { get; set; }
		public int exam_notif_id { get; set; }
		public int exam_notif_status_id { get; set; }
		public Nullable<DateTime> trans_date { get; set; }
		public char is_active { get; set; }
		public Nullable<DateTime> creation_datetime { get; set; }
		public Nullable<DateTime> updation_datetime { get; set; }
		public int intcreated_by { get; set; }
		public int updated_by { get; set; }
	}

	/// <summary>
	/// Exam notification Status master
	/// </summary>
	public class tbl_exam_notif_status_mast
	{
		[Key]
		public int exam_notif_status_id { get; set; }
		public string exam_notif_status_desc { get; set; }
		public char is_active { get; set; }
		public Nullable<DateTime> creation_datetime { get; set; }
		public Nullable<DateTime> updation_datetime { get; set; }
		public int created_by { get; set; }
		public int updated_by { get; set; }
	}


	/// <summary>
	/// Exam notification transaction
	/// </summary>
	public class tbl_exam_notification_trans
	{
		[Key]
		public int exam_notif_trans_id { get; set; }
		public int exam_notif_id { get; set; }
		public int exam_notif_status_id { get; set; }
		public Nullable<DateTime> trans_date { get; set; }
		public bool? is_active { get; set; }
		public Nullable<DateTime> creation_datetime { get; set; }
		public Nullable<DateTime> updation_datetime { get; set; }
		public int? created_by { get; set; }
		public int? updated_by { get; set; }
        public int? login_id { get; set; }
        public string exam_notif_doc_file_path { get; set; }

        public string signed_exam_notif_file_path { get; set; }
    }

	/// <summary>
	/// Exam semester master
	/// </summary>
	public class tbl_exam_semester_mast
	{
		[Key]
		public int exam_semester_id { get; set; }
		public string exam_semester_desc { get; set; }
		public bool? exam_semester_is_active { get; set; }
		public int exam_semester_created_by { get; set; }
		public int exam_semester_updated_by { get; set; }
		public Nullable<DateTime> exam_semester_creation_datetime { get; set; }
		public Nullable<DateTime> exam_semester_updation_datetime { get; set; }

	}

	/// <summary>
	/// Exam subject master
	/// </summary>
	public class tbl_exam_subject_mast
	{
		[Key]
		public int exam_subject_id { get; set; }
		public string exam_subject_desc { get; set; }
		public int exam_subject_type_id { get; set; }
		public int exam_course_id { get; set; }
		public int trade_special_type { get; set; }
		public int subject_type_id { get; set; }
		public int? exam_subject_trade_id { get; set; }
		public bool? exam_subject_is_active { get; set; }
		public int? exam_subject_created_by { get; set; }
		public int? exam_subject_updated_by { get; set; }
		public Nullable<DateTime> exam_subject_creation_datetime { get; set; }
		public Nullable<DateTime> exam_subject_updation_datetime { get; set; }
	}

	/// <summary>
	/// Exam subject type master
	/// </summary>
	public class tbl_exam_subject_type_mast
	{
		[Key]
		public int exam_subject_type_id { get; set; }
		public string exam_subject_type_desc { get; set; }
        public int course_id { get; set; }
        public bool? exam_subject_type_is_active { get; set; }
		public int? exam_subject_type_created_by { get; set; }
		public int? exam_subject_type_updated_by { get; set; }
		public Nullable<DateTime> exam_subject_type_creation_datetime { get; set; }
		public Nullable<DateTime> exam_subject_type_updation_datetime { get; set; }
        public int subject_type_id { get; set; }

    }


    /// <summary>
    /// Exam type master
    /// </summary>
    public class tbl_exam_type_mast
	{
		[Key]
		public int exam_type_id { get; set; }
		public string exam_type_name { get; set; }
		public bool? exam_type_is_active { get; set; }
		public int exam_type_created_by { get; set; }
		public int exam_type_updated_by { get; set; }
		public Nullable<DateTime> exam_type_creation_datetime { get; set; }
		public Nullable<DateTime> exam_type_updation_datetime { get; set; }
	}

    /// <summary>
    /// User Roles
    /// </summary>
    /// 
    //tbl_department_master
    public class tbl_department_master
    {
        [Key]
        public int dept_id { get; set; }
        public string dept_description { get; set; }
        public char dept_is_active { get; set; }
        public int dept_created_by { get; set; }
        public int dept_creation_datetime { get; set; }
        public Nullable<DateTime> dept_updated_by { get; set; }
        public Nullable<DateTime> dept_updation_datetime { get; set; }
    }

    /// <summary>
	/// Notification Description
	/// </summary>
	public class tbl_notification_description
    {
        [Key]
        public int notif_decr_id { get; set; }
        public string notification_description { get; set; }
        public bool notif_decr_is_active { get; set; }
        public int notif_decr_created_by { get; set; }
        public Nullable<DateTime> notif_decr_creation_datetime { get; set; }
        public int notif_decr_updated_by { get; set; }
        public Nullable<DateTime> notif_decr_updation_datetime { get; set; }
    }

	/// <summary>
	/// Comments table
	/// </summary>
	

    //public class tbl_division_mast
    //{
    //    [Key]
    //    public int division_id { get; set; }
    //    public string division_name { get; set; }
    //    public bool? is_active { get; set; }
    //    public int? created_by { get; set; }
    //    public DateTime? creation_datetime { get; set; }
    //    public int updated_by { get; set; }
    //    //public SelectList DivisionList { get; set; }updation_datetime
    //    public DateTime? updation_datetime { get; set; }
    //}

    //public class tbl_district_mast
    //{
    //    [Key]
    //    public int dist_id { get; set; }
    //    public string dist_name { get; set; }

    //    public int? division_id { get; set; }
    //    public bool? is_active { get; set; }
    //    public int? created_by { get; set; }
    //    public DateTime? creation_datetime { get; set; }
        
    //    public int? updated_by { get; set; }

    //    public DateTime? updation_datetime { get; set; }
    //}
    //public class tbl_iti_college_details
    //{
    //    [Key]
    //    public int iti_college_id { get; set; }
    //    public string iti_college_code { get; set; }
    //    public int district_id { get; set; }
    //    public int taluk_id { get; set; }
    //    public string village_or_town { get; set; }
    //    public string college_address { get; set; }
    //    public int location_id { get; set; }
    //    public int css_code { get; set; }
    //    public string iti_college_name { get; set; }
    //    public int grading { get; set; }
    //    public Decimal geo { get; set; }
    //    public string file_ref_no { get; set; }
    //    public bool is_active { get; set; }

    //    public DateTime creation_datetime { get; set; }
    //    public int updated_by { get; set; }
       
    //    public DateTime updation_datetime { get; set; }

    //}
    public class tbl_exam_centers
    {
        [Key]
        public int ec_id { get; set; }
        public string ec_name { get; set; }
        public string ec_email_id { get; set; }
        public string ec_phone_num { get; set; }
        public Nullable<int> division_id { get; set; }
        public Nullable<int> district_id { get; set; }
        public Nullable<bool> ec_is_active { get; set; }
        public Nullable<int> ec_created_by { get; set; }
        public Nullable<System.DateTime> ec_creation_datetime { get; set; }
        public Nullable<int> ec_updated_by { get; set; }
        public Nullable<System.DateTime> ec_updation_datetime { get; set; }
        public Nullable<int> ec_status_id { get; set; }
        public string ec_remarks { get; set; }
        public string ec_code { get; set; }
        public int? ec_center_code { get; set; }

        //tara Commented
        //  public int taluk_id { get; set; }


    }
    public class tbl_exam_centre_mapping_mast
    {
        [Key]
        public int ecmm_id { get; set; }
        public Nullable<int> exam_centre_id { get; set; }
        public Nullable<int> iti_college_id { get; set; }
        public Nullable<int> trainee_id { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> creation_datetime { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public Nullable<int> ecmm_status_id { get; set; }
        public string ecmm_remarks { get; set; }
        public string exam_centre_mapping_number { get; set; }
        public Nullable<int> course_id { get; set; }

        public Nullable<int> login_id { get; set; }
        public Nullable<int> division_id { get; set; }

        public string ec_name { get; set; }
        public string college_name { get; set; }
        public Nullable<int> district_id { get; set; }
        public int  exam_notif_status_id { get; set; }
        public int session_year { get; set; }
        //public Nullable<DateTime> remapped_datetime { get; set; }
        public Nullable<int> remapped_status_id { get; set; }

    }

    public class tbl_exam_centre_mapping_trans
    {
        [Key]
        public int ecmt_id { get; set; }
        public int? ecmm_id { get; set; }
        public int? exam_centre_id { get; set; }
        public int? iti_college_id { get; set; }
        public int? division_id { get; set; }
        public int? distict_id { get; set; }
        public int? status_id { get; set; }
        public int? login_id { get; set; }
        public bool? ecmt_is_active { get; set; }
        public int? ecmt_created_by { get; set; }
        public Nullable<DateTime> ecmt_creation_datetime { get; set; }
        public int? ecmt_updated_by { get; set; }
        public Nullable<DateTime> ecmt_updation_datetime { get; set; }


    }
    public class tbl_iti_trainees_details
    {
        [Key]
        public int trainee_id { get; set; }
        public string iti_college_code { get; set; }
        public string trainee_name { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> creation_datetime { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<System.DateTime> updation_datetime { get; set; }
        public String trainee_roll_num { get; set; }
        public int course_id { get; set; }
        public int trade_type_id { get; set; }
        public int trade_id { get; set; }
        public int trade_year { get; set; }
        public string Father_name { get; set; }
        public bool? regular_repeater { get; set; }
        public int? attendance { get; set; }
        public bool? discharged { get; set; }
        public bool? is_paid { get; set; }
        public int exam_fees_paid { get; set; }
        public int? shift_id { get; set; }
        public int? year_of_admission { get; set; }
        public int? unit_id { get; set; }

        //public int shift { get; set; }
        //public int year_of_admission { get; set; }
        //public int unit { get; set; }
        public Nullable<System.DateTime> date_of_birth { get; set; }

    }
    public class tbl_iti_trainees_mast
    {
        [Key]
        public int iti_trainees_id { get; set; }
        public string iti_code { get; set; }
        public string iti_trainee_name { get; set; }
        public Nullable<bool> iti_trainee_is_active { get; set; }
        public Nullable<int> iti_trainee_created_by { get; set; }
        public Nullable<System.DateTime> iti_trainee_creation_datetime { get; set; }
        public Nullable<int> iti_trainee_updated_by { get; set; }
        public Nullable<System.DateTime> iti_trainee_updation_datetime { get; set; }
        public Nullable<int> roll_num { get; set; }
    }
    public class tbl_taluk_mast
    {
        [Key]
        public int taluk_id { get; set; }
        public string taluk_name { get; set; }
        public Nullable<int> district_id { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> creation_datetime { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<System.DateTime> updation_datetime { get; set; }

        //public virtual tbl_district_mast tbl_district_mast { get; set; }
    }

	public class tbl_subject_type
	{
		[Key]
		public int st_id { get; set; }
		public string subject_type { get; set; }
		public bool? st_is_active { get; set; }
		public int st_created_by { get; set; }
		public Nullable<DateTime> st_creation_datetime { get; set; }
		public int st_updated_by { get; set; }
		public Nullable<DateTime> st_updation_datetime { get; set; }
	}

    public class tbl_exam_centre_mapping_history
    {
        [Key]
        public int ecmh_id { get; set; }
        public int? exam_centre_id { get; set; }
        public int? iti_college_id { get; set; }
        public bool is_active { get; set; }
        public int created_by { get; set; }
        public Nullable<DateTime> creation_datetime { get; set; }
        public int updated_by { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public int ecmh_status_id { get; set; }
        public int division_id { get; set; }
        public int? district_id { get; set; }
        public int? login_id { get; set; }
    }

    public class tbl_comments_transaction
    {
        [Key]
        public int comments_transaction_id { get; set; }
        public string comments_transaction_desc { get; set; }
        public int notification_id { get; set; }
        public int module_id { get; set; }
        public int status_id { get; set; }
        public int? login_id { get; set; }
        public bool ct_is_active { get; set; }
        public int? ct_created_by { get; set; }
        public int is_published { get; set; }

        public Nullable<DateTime> ct_created_on { get; set; }
        public int ct_updated_by { get; set; }
        public Nullable<DateTime> ct_updated_on { get; set; }
    }

    //DSC Key 01May2021 BNM
    public class tbl_office_emp_mapping
    {
        [Key]
        public int office_emp_map_id { get; set; }
        public int office_id { get; set; }
        public int emp_id { get; set; }
        public bool? oem_is_active { get; set; }
        public DateTime? oem_created_on { get; set; }
        public DateTime? oem_updated_on { get; set; }
        public int? oem_created_by { get; set; }
        public int? oem_updated_by { get; set; }
    }
    public class tbl_dsc_mapping_dtls
    {
        [Key]
        public int dmd_regn_no { get; set; }
        public string dmd_serial_number { get; set; }
        public string dmd_certifying_authority { get; set; }
        public string dmd_public_key { get; set; }
        public DateTime dmd_date_of_expiry { get; set; }
        public int dmd_emp_id { get; set; }
        public string dmd_name { get; set; }
        public string dmd_place { get; set; }
        public string dmd_email { get; set; }
        public string dmd_phone_no { get; set; }
        public bool? dmd_is_approved { get; set; }
        public int dmd_approved_by { get; set; }
        public bool? dmd_is_active { get; set; }
        public DateTime? dmd_created_on { get; set; }
        public DateTime? dmd_updated_on { get; set; }
        public int? dmd_created_by { get; set; }
        public int? dmd_updated_by { get; set; }
        public string dmd_remarks { get; set; }
    }

    public class tbl_district_mast
    {
        [Key]
        public int dist_id { get; set; }
        public string dist_name { get; set; }

        public int? division_id { get; set; }
        public bool? is_active { get; set; }
        public int? created_by { get; set; }
        public DateTime? creation_datetime { get; set; }

        public int? updated_by { get; set; }

        public DateTime? updation_datetime { get; set; }
    }
}
