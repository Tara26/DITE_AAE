using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class SeatMatrixAllocationDetail
    {
        public int AllocationDetailId { get; set; }

        public int? AllocationId { get; set; }

        public int? RankNumber { get; set; }

        public int? ApplicantId { get; set; }

        public int? InstituteId { get; set; }

        public int? TradeId { get; set; }

        public int? HorizontalId { get; set; }

        public int? VerticalId { get; set; }

        public int? HyrNonHydrId { get; set; }

        public int? PreferenceNum { get; set; }

        public int? Status { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
        public string InstituteName { get; set; }
        public string TradeName { get; set; }
        //Institute Type
        public int Institute_typeId { get; set; }
        public string InstituteType { get; set; }
        //District Master
        public int district_id { get; set; }
        public string district_ename { get; set; }
        public string district_kname { get; set; }
        public int district_lgd_code { get; set; }
        public int? state_code { get; set; }
        public bool? dis_is_active { get; set; }
        public DateTime? dis_creation_datetime { get; set; }
        public DateTime? dis_updation_datetime { get; set; }
        public int? dis_created_by { get; set; }
        public int? dis_updated_by { get; set; }

        //Division Master
        public int division_id { get; set; }
        public string division_name { get; set; }
        public bool? division_is_active { get; set; }
        public DateTime? division_created_on { get; set; }
        public int? division_created_by { get; set; }
        public DateTime? division_updated_on { get; set; }
        public int? division_updated_by { get; set; }
        public string division_kannada_name { get; set; }
        public decimal MarksObtained_1 { get; set; }

        //tbl_iti_college_details
        public string iti_college_code { get; set; }
        public int? taluk_id { get; set; }
        public int? village_or_town { get; set; }
        public string college_address { get; set; }
        public int? location_id { get; set; }
        public int? css_code { get; set; }
        public string iti_college_name { get; set; }
        public string geo { get; set; }
        public string file_ref_no { get; set; }
        public bool is_active { get; set; }
        public int? created_by { get; set; }
        public DateTime? creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updation_datetime { get; set; }
        public int iti_college_id { get; set; }
        public string email_id { get; set; }
        public string phone_num { get; set; }
        public string MISCode { get; set; }
        public string Address { get; set; }
        public int? Constituency { get; set; }
        public string BuildUpArea { get; set; }
        public DateTime? AffiliationDate { get; set; }
        public int? NoOfShifts { get; set; }
        public int? Insitute_TypeId { get; set; }
        public int? Units { get; set; }
        public string UploadAffiliationDoc { get; set; }
        public int? StatusId { get; set; }
        public string Remarks { get; set; }
        public int? Panchayat { get; set; }
        public int? CourseCode { get; set; }
        public int? PinCode { get; set; }
        public bool ActiveDeActive { get; set; }
        public string Website { get; set; }
        public DateTime? AffiliationOrderNoDate { get; set; }
        public int? Scheme { get; set; }
        public string AffiliationOrderNo { get; set; }
        public int? color_flag { get; set; }

        //tbl_ITI_trade_seat_master
        public int Trade_ITI_seatId { get; set; }
        public int Trade_ITIId { get; set; }
        public int UnitId { get; set; }
        public int ShiftId { get; set; }
        public int SeatsPerUnit { get; set; }
        public int SeatsTypeId { get; set; }
        public bool IsPPP { get; set; }
        public bool DualSystemTraining { get; set; }
        //public DateTime CreatedOn { get; set; }
        // public int CreatedBy { get; set; }
        public int? ModifedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? Govt_Gia_seats { get; set; }
        public int? PPP_seats { get; set; }
        public int? Management_seats { get; set; }

        //tbl_ITI_Trade
        public int? TradeCode { get; set; }
        public int? ITICode { get; set; }
        public int? Unit { get; set; }
        public int? FlowId { get; set; }
        public string FileUploadPath { get; set; }
        public string ActivrFilePath { get; set; }
        public string ActiveFileName { get; set; }
        public string AllottedCategory { get; set; }
        public string AllottedGroup { get; set; }
        public int? PreferenceNumber { get; set; }
        public string LocalStatus  { get; set; }
        public string FirstName  { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public int slno { get; set; }

        public string userRole { get; set; }
        public string ForwardedTo { get; set; }
        public string StatusName { get; set; }
        public string CommentsCreatedOn { get; set; }
    }
}
