using Models;
using Models.Master;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DBConnection
{
    public class DbConnection : DbContext
    {
        public DbSet<tbl_exam_notification_mast> tbl_exam_notification_mast { get; set; }
        public DbSet<tbl_course_type_mast> tbl_course_type_mast { get; set; }
        public DbSet<tbl_user_login> tbl_user_login { get; set; }
        public DbSet<tbl_user_roles> tbl_user_roles { get; set; }
        public DbSet<tbl_exam_notification_trans> tbl_exam_notification_trans { get; set; }
        public DbSet<tbl_exam_notif_status_mast> tbl_exam_notif_status_mast { get; set; }
        public DbSet<tbl_department_master> tbl_department_master { get; set; }
        public DbSet<tbl_notification_description> tbl_notification_description { get; set; }
        public DbSet<tbl_trade_mast> tbl_trade_mast { get; set; }
        public DbSet<tbl_trade_type_mast> tbl_trade_type_mast { get; set; }
        public DbSet<tbl_exam_type_mast> tbl_exam_type_mast { get; set; }
        public DbSet<tbl_exam_semester_mast> tbl_exam_semester_mast { get; set; }
        public DbSet<tbl_exam_subject_type_mast> tbl_exam_subject_type_mast { get; set; }
        public DbSet<tbl_subject_type> tbl_subject_type { get; set; }
        public DbSet<tbl_exam_subject_mast> tbl_exam_subject_mast { get; set; }
        public DbSet<tbl_exam_calendar_trans> tbl_exam_calendar_trans { get; set; }
        public DbSet<tbl_trade_year_mast> tbl_trade_year_mast { get; set; }
        public DbSet<tbl_exam_subject_trans> tbl_exam_subject_trans { get; set; }
        public DbSet<tbl_Exam_Cal_Notif_mast> tbl_exam_cal_notif_mast { get; set; }
        public DbSet<tbl_exam_cal_notif_trans> tbl_exam_cal_notif_trans { get; set; }
        public DbSet<tbl_comments_transaction> tbl_comments_transaction { get; set; }
        public DbSet<tbl_division_mast> tbl_division_mast { get; set; }
        public DbSet<tbl_attendance_details> tbl_attendance_details { get; set; }
        public DbSet<tbl_block_or_room> tbl_block_or_room { get; set; }
        public DbSet<tbl_designation_master> tbl_designation_master { get; set; }
        public DbSet<tbl_staffdetails_history> tbl_staffdetails_history { get; set; }

        public DbSet<tbl_exam_centers> tbl_exam_centers { get; set; }
        public DbSet<tbl_exam_centre_mapping_trns> tbl_exam_centre_mapping_trns { get; set; }
        public DbSet<tbl_iti_trainees_details> tbl_iti_trainees_details { get; set; }
        public DbSet<tbl_iti_trainees_mast> tbl_iti_trainees_mast { get; set; }
        public DbSet<tbl_taluk_mast> tbl_taluk_mast { get; set; }
        public DbSet<tbl_question_paper_set_trns> tbl_question_paper_set_trns { get; set; }
        public DbSet<tbl_trainee_fee_paid> tbl_trainee_fee_paid { get; set; }
        public DbSet<tbl_unique_trainee_mast> tbl_unique_trainee_mast { get; set; }
        public DbSet<tbl_legacy_marks> tbl_legacy_marks { get; set; }
        public DbSet<tbl_subject> tbl_subject { get; set; }
        public DbSet<tbl_semester> tbl_semester { get; set; }
        public DbSet<tbl_code_marks_sheet_master> tbl_code_marks_sheet_master { get; set; }
        public DbSet<tbl_packaging_slip_mast> tbl_packaging_slip_mast { get; set; }
        public DbSet<tbl_packaging_slip_trans> tbl_packaging_slip_trans { get; set; }

        public DbSet<tbl_ITI_trade_seat_trans_Excel> tbl_ITI_trade_seat_trans_Excel { get; set; }
        public DbSet<tbl_ITI_trade_seat_master> tbl_ITI_trade_seat_master { get; set; }
        public DbSet<tbl_shifts> tbl_shifts { get; set; }
        public DbSet<tbl_seat_type> tbl_seat_type { get; set; }
        public DbSet<tbl_iti_college_details> tbl_iti_college_details { get; set; }
        public DbSet<tbl_Institute_type> tbl_Institute_type { get; set; }
        public DbSet<tbl_ITI_trade_seat_trans> tbl_ITI_trade_seat_trans { get; set; }
        public DbSet<tbl_units> tbl_units { get; set; }
        public DbSet<tbl_ITI_Trade> tbl_ITI_Trade { get; set; }
        public DbSet<tbl_affiliation_uploadexcel> tbl_affiliation_uploadexcel { get; set; }
        public DbSet<tbl_location_type> tbl_location_type { get; set; }
        public DbSet<tbl_Panchayat> tbl_Panchayat { get; set; }
        public DbSet<tbl_CSS> tbl_CSS { get; set; }
        public DbSet<tbl_trade_sector> tbl_trade_sector { get; set; }
        public DbSet<tbl_trade_scheme> tbl_trade_scheme { get; set; }
        public DbSet<tbl_status_master> tbl_status_master { get; set; }
        public DbSet<tbl_ITI_trade_seat_trans_history> tbl_ITI_trade_seat_trans_history { get; set; }
        public DbSet<MenuLists> MenuDetails { get; set; }
        public DbSet<MenuRoles> MenuRoles { get; set; }
        public DbSet<tbl_omr_details_mast> tbl_omr_details_mast { get; set; }
        public DbSet<tbl_attendance_details_trans> tbl_attendance_details_trans { get; set; }
        public DbSet<tbl_Constituency> tbl_Constituencies { get; set; }
        public DbSet<tbl_Village> tbl_Villages { get; set; }
        public DbSet<tbl_admsn_ntf_details> tbl_admsn_ntf_details { get; set; }
        public DbSet<tbl_admsn_ntf_trans> tbl_admsn_ntf_trans { get; set; }
        public DbSet<tbl_admsn_ntf_trans_history> tbl_admsn_ntf_trans_history { get; set; }
        public DbSet<tbl_tentative_calendar_of_events> tbl_tentative_calendar_of_events { get; set; }
        public DbSet<tbl_tentative_calendar_of_events_trans> tbl_tentative_calendar_of_events_trans { get; set; }
        public DbSet<tbl_tentative_calendar_of_events_trans_Hist> tbl_tentative_calendar_of_events_trans_Hist { get; set; }
        public DbSet<tbl_admission_cal_comments_transaction> tbl_admission_cal_comments_transaction { get; set; }
        public DbSet<tbl_admission_comments_transaction> tbl_admission_comments_transaction { get; set; }
        public DbSet<tbl_user_master> tbl_user_master { get; set; }
        public DbSet<tbl_role_master> tbl_role_master { get; set; }
        public DbSet<tbl_role_mapping> tbl_role_mapping { get; set; }
        public DbSet<tbl_department_mast> tbl_department_mast { get; set; }
        public DbSet<tbl_sub_department_master> tbl_sub_department_master { get; set; }
        public DbSet<tbl_user_mapping> tbl_user_mapping { get; set; }
        public DbSet<tbl_VerificationOfficer_Master> tbl_VerificationOfficer_Master { get; set; }
        public DbSet<tbl_Applicant_Detail> tbl_Applicant_Detail { get; set; }
        public DbSet<tbl_Gender> tbl_Gender { get; set; }
        public DbSet<tbl_Appl_Doc_Verifi> tbl_Appl_Doc_Verifi { get; set; }
        public DbSet<Staff_Institute_Detail> Staff_Institute_Detail { get; set; }
        public DbSet<tbl_VerOfficer_Applicant_Mapping> tbl_VerOfficer_Applicant_Mapping { get; set; }
        public DbSet<tbl_ApplicantTrans> tbl_ApplicantTrans { get; set; }
        public DbSet<tbl_Category> tbl_Category { get; set; }
        public DbSet<tbl_GradationRank_Trans> tbl_GradationRank_Trans { get; set; }
        public DbSet<tbl_Document_Applicant> tbl_Document_Applicant { get; set; }
        public DbSet<tbl_DocumentType> tbl_DocumentType { get; set; }
        public DbSet<tbl_Grievance> tbl_Grievance { get; set; }
        public DbSet<tbl_GrievanceCategory> tbl_GrievanceCategory { get; set; }
        public DbSet<tbl_GrievanceDoc> tbl_GrievanceDoc { get; set; }
        public DbSet<tbl_ITI_Trade_ActiveStatus_History> tbl_ITI_Trade_ActiveStatus_History { get; set; }
        public DbSet<tbl_ITI_Institute_His_ActiveStatus> tbl_ITI_Institute_His_ActiveStatus { get; set; }
        public DbSet<tbl_admission_notif_status_mast> tbl_admission_notif_status_mast { get; set; }
        public DbSet<tbl_ITI_Trade_Shift_ActiveStatus_History> tbl_ITI_Trade_Shift_ActiveStatus_History { get; set; }
        public DbSet<tbl_ITI_Trade_Shift_ActiveStatus> tbl_ITI_Trade_Shift_ActiveStatus { get; set; }

        public DbSet<tbl_AdmissionatInstituteFeeDetails> tbl_AdmissionatInstituteFeeDetails { get; set; }
        public DbSet<tbl_Affiliation_documents> tbl_Affiliation_documents { get; set; }

        #region  .. seat allocation ..
        public DbSet<Tbl_rules_allocation_master> Tbl_rules_allocation_master { get; set; }
        public DbSet<tbl_Vertical_rules> tbl_Vertical_rules { get; set; }
        public DbSet<tbl_Vertical_rule_value> tbl_Vertical_rule_value { get; set; }
        public DbSet<Tbl_horizontal_rules> Tbl_horizontal_rules { get; set; }
        public DbSet<tbl_horizontal_rules_values> tbl_horizontal_rules_values { get; set; }
        public DbSet<tbl_Grades> tbl_Grades { get; set; }
        public DbSet<tbl_syllabus_type> tbl_syllabus_type { get; set; }
        public DbSet<tbl_Grade_percentage> tbl_Grade_percentage { get; set; }
        public DbSet<tbl_Grade_percentage_Value> tbl_Grade_percentage_Value { get; set; }
        public DbSet<tbl_HYD_NonHYD_regions> tbl_HYD_NonHYD_regions { get; set; }
        public DbSet<tbl_HYD_NonHYD_candidates> tbl_HYD_NonHYD_candidates { get; set; }
        public DbSet<Tbl_Hyd_kar_rules> Tbl_Hyd_kar_rules { get; set; }
        public DbSet<tbl_HYD_kar_rules_value> tbl_HYD_kar_rules_value { get; set; }
        public DbSet<Tbl_other_rules> Tbl_other_rules { get; set; }
        public DbSet<tbl_other_rules_value> tbl_other_rules_value { get; set; }
        public DbSet<Tbl_rules_allocation_master_history> Tbl_rules_allocation_master_history { get; set; }
        public DbSet<tbl_Year> tbl_Year { get; set; }
        #endregion

        #region .. Applicant Application Form 

        public DbSet<tbl_Religion> tbl_Religion { get; set; }
        public DbSet<tbl_ApplicantType> tbl_ApplicantType { get; set; }
        public DbSet<tbl_TraineeType> tbl_TraineeType { get; set; }
        public DbSet<tbl_OtherBoards_Details> tbl_OtherBoards_Details { get; set; }
        public DbSet<tbl_reservation> tbl_reservation { get; set; }
        public DbSet<tbl_Qualification> tbl_qualification { get; set; }
        public DbSet<tbl_district_master> tbl_district_master { get; set; }
        public DbSet<tbl_district_mast> tbl_district_mast { get; set; }
        public DbSet<tbl_taluk_master> tbl_taluk_master { get; set; }
        public DbSet<tbl_Applicant_InstitutePreference> tbl_Applicant_InstitutePreference { get; set; }
        public DbSet<tbl_Applicant_Reservation> tbl_Applicant_Reservation { get; set; }
        public DbSet<tbl_PersonWithDisabilityCategory> tbl_PersonWithDisabilityCategory { get; set; }
        public DbSet<tbl_ApplicationFormDescStatus> tbl_ApplicationFormDescStatus { get; set; }
        public DbSet<tbl_ApplicantAdmissionRounds> tbl_ApplicantAdmissionRounds { get; set; }
        public DbSet<tbl_Applicant_ITI_Institute_Detail> tbl_Applicant_ITI_Institute_Detail { get; set; }
        public DbSet<tbl_Applicant_ITI_Institute_Detail_Trans> tbl_Applicant_ITI_Institute_Detail_Trans { get; set; }
        public DbSet<tbl_Applicant_Admi_Against_Vacancy> tbl_Applicant_Admi_Against_Vacancy { get; set; }
        public DbSet<tbl_AdmissionatInstituteStatus> tbl_AdmissionatInstituteStatus { get; set; }
        public DbSet<tbl_State> tbl_State { get; set; }
        #endregion

        #region MeritList
        public DbSet<tbl_division_master> tbl_division_master { get; set; }
        public DbSet<tbl_GradationType> tbl_GradationType { get; set; }
        public DbSet<tbl_Caste> tbl_Caste { get; set; }
        public DbSet<tbl_Result> tbl_Result { get; set; }
        public DbSet<tbl_GradationRank_TransHistory> tbl_GradationRank_TransHistory { get; set; }
        #endregion MeritList

        public DbSet<tbl_stafftrade_details> tbl_stafftrade_details { get; set; }
        public DbSet<tbl_staffsubject_details> tbl_staffsubject_details { get; set; }
        public DbSet<tbl_staff_type_master> tbl_staff_type_master { get; set; }
        public DbSet<tbl_StaffYearWise_details> tbl_StaffYearWise_details { get; set; }
        public DbSet<temp_tbl_iti_college_details> temp_Tbl_Iti_College_Details { get; set; }
        public DbSet<temp_tbl_ITI_Trade> temp_Tbl_ITI_Trades { get; set; }
        public DbSet<temp_tbl_ITI_Trade_Shift> temp_Tbl_ITI_Trade_Shifts { get; set; }
        //  public DbSet<tbl_division_master> tbl_Division_Masters { get; set; }
        // public DbSet<tbl_district_master> tbl_District_Masters { get; set; }
        //  public DbSet<tbl_taluk_master> tbl_Taluk_Masters { get; set; }
        public DbSet<tbl_village_master> tbl_Village_Masters { get; set; }
        public DbSet<tbl_ITI_Trade_History> tbl_ITI_Trade_Histories { get; set; }
        public DbSet<tbl_grama_panchayat_master> tbl_Grama_Panchayat_Masters { get; set; }
        public DbSet<tbl_ITI_Institute_ActiveStatus> tbl_ITI_Institute_ActiveStatus { get; set; }

        public DbSet<tbl_ITI_Trade_Shift> tbl_ITI_Trade_Shifts { get; set; }
        public DbSet<tbl_GrievanceHistory> tbl_GrievanceHistory { get; set; }
        public DbSet<tbl_SeatMatrix_Main> tbl_SeatMatrix_Main { get; set; }
        public DbSet<tbl_SeatMatrix_Trans> tbl_SeatMatrix_Trans { get; set; }
        public DbSet<tbl_SeatAllocationDetail_Seatmatrix> tbl_SeatAllocationDetail_Seatmatrix { get; set; }
        public DbSet<tbl_SeatAllocation_SeatMatrix_Trans> tbl_SeatAllocation_SeatMatrix_Trans { get; set; }
        public DbSet<tbl_SeatAllocation_SeatMatrix> tbl_SeatAllocation_SeatMatrix { get; set; }
        public DbSet<tbl_SeatMatrix_TradeWise> tbl_SeatMatrix_TradeWise { get; set; }

        #region Seat Allocation Review & Recommand Of Seat Matrix
        //ram usea case -35 Screen 55&56
        //public DbSet<tbl_SeatAllocation_SeatMatrix> tbl_SeatAllocation_SeatMatrix { get; set; }

        //public DbSet<tbl_SeatAllocationDetail_Seatmatrix> tbl_SeatAllocationDetail_Seatmatrix { get; set; }

        #endregion
        public DbSet<tbl_SeatAvail_status_master> tbl_SeatAvail_status_master { get; set; }
        public DbSet<tbl_Tentative_admsn_eventDetails> tbl_Tentative_admsn_eventDetails { get; set; }
        public DbSet<tbl_transfer_admitted_data> tbl_transfer_admitted_data { get; set; }
        public DbSet<tbl_transfer_admitted_data_trans> tbl_transfer_admitted_data_trans { get; set; }
        public DbSet<tbl_transfer_seat_details> tbl_transfer_seat_details { get; set; }
        public DbSet<tbl_transfer_Institute_Details> tbl_transfer_Institute_Details { get; set; }
        public DbSet<tbl_user_history> tbl_user_history_mast { get; set; }
        public DbSet<tbl_audittrail> tbl_audittrail_mast { get; set; }
        public DbSet<tbl_seattype_master> tbl_seattype_master { get; set; }

        public DbSet<tbl_notification_Publish_trans> tbl_notification_Publish_trans { get; set; }
        public DbSet<tbl_Holiday_mast> tbl_Holiday_mast { get; set; }


        #region Admin
        public DbSet<tbl_LevelMaster> tbl_LevelMaster { get; set; }
        public DbSet<tbl_wing_master> tbl_wing_master { get; set; }
        //public DbSet<tbl_section_master> tbl_section_master { get; set; }
        #endregion
        public DbSet<tbl_ApplicationMode> tbl_ApplicationMode { get; set; }

        #region ExaminationModule
        //Offline Fee Payment BNM
        public DbSet<tbl_feepaid_receipt> tbl_feepaid_receipt { get; set; }
        public DbSet<tbl_grievance_approval> tbl_grievance_approval { get; set; }
        public DbSet<tbl_exam_subject_desc> tbl_exam_subject_desc { get; set; }
        public DbSet<tbl_exam_grace_marks> tbl_exam_grace_marks { get; set; }
        public DbSet<tbl_SaveApproved_QP_by_HofQPC> tbl_SaveApproved_QP_by_HofQPC { get; set; }
        public DbSet<tbl_questionpaper_set> tbl_questionpaper_set { get; set; }
        public DbSet<tbl_Exam_Trainee_Eligibility> tbl_Exam_Trainee_Eligibility { get; set; }
		 public DbSet<tbl_special_trade_types> tbl_special_trade_types { get; set; }
		  public DbSet<tbl_office_emp_mapping> tbl_office_emp_mapping { get; set; }
		   public DbSet<tbl_dsc_mapping_dtls> tbl_dsc_mapping_dtls { get; set; }

       
        #endregion

    }


}




