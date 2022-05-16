using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class seatmatrixmodel
    {
        public int slno { get; set; }
        public string InstituteName { get; set; }
        public string TradeName { get; set; }
        public int? Govtseat { get; set; }
        public int? GovtPPP { get; set; }
        public int? Private { get; set; }
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string CourseTypeName { get; set; }
        public string SeatName { get; set; }
        public string Duration { get; set; }

        //Application Type
        public int ApplicantTypeId { get; set; }
        public string ApplicantTypeDdl { get; set; }

        //Institute Type
        public int Institute_typeId { get; set; }
        public string InstituteType { get; set; }

        //District Master
        public int? district_id { get; set; }
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
        public int? division_id { get; set; }
        public string division_name { get; set; }
        public bool? division_is_active { get; set; }
        public DateTime? division_created_on { get; set; }
        public int? division_created_by { get; set; }
        public DateTime? division_updated_on { get; set; }
        public int? division_updated_by { get; set; }
        public string division_kannada_name { get; set; }
        public decimal MarksObtained_1 { get; set; }

        //Taluka Master
        public int taluk_lgd_code { get; set; }
        public string taluk_ename { get; set; }
        public bool taluk_is_active { get; set; }

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
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
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
        public bool IsActive { get; set; }
        public string FileUploadPath { get; set; }
        public string ActivrFilePath { get; set; }
        public string ActiveFileName { get; set; }

        //Rules For Allocations
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ResGroupId { get; set; }
        public string GroupName { get; set; }
        public decimal GovtAidedseats { get; set; }
        public string LocalStatus { get; set; }
        public decimal verticalRule { get; set; }
        public decimal HorizonRule { get; set; }
        public int? TotalSeats { get; set; }

        //All Category column
        //GM
        public decimal GMWomenH { get; set; }
        public decimal GMWomenNH { get; set; }
        public decimal GMPwdH { get; set; }
        public decimal GMPwdNH { get; set; }
        public decimal GMExSH { get; set; }
        public decimal GMExSNH { get; set; }
        public decimal GMkmH { get; set; }
        public decimal GMkmNH { get; set; }
        public decimal GMllH { get; set; }
        public decimal GMllNH { get; set; }
        public decimal GMKasMH { get; set; }
        public decimal GMKasMNH { get; set; }
        public decimal GMEwsH { get; set; }
        public decimal GMEwsNH { get; set; }
        public decimal GMgpH { get; set; }
        public decimal GMgpNH { get; set; }

        //SC
        public decimal SCWomenH { get; set; }
        public decimal SCWomenNH { get; set; }
        public decimal SCPwdH { get; set; }
        public decimal SCPwdNH { get; set; }
        public decimal SCExSH { get; set; }
        public decimal SCExSNH { get; set; }
        public decimal SCkmH { get; set; }
        public decimal SCkmNH { get; set; }
        public decimal SCllH { get; set; }
        public decimal SCllNH { get; set; }
        public decimal SCKasMH { get; set; }
        public decimal SCKasMNH { get; set; }
        public decimal SCEwsH { get; set; }
        public decimal SCEwsNH { get; set; }
        public decimal SCgpH { get; set; }
        public decimal SCgpNH { get; set; }

        //ST
        public decimal STWomenH { get; set; }
        public decimal STWomenNH { get; set; }
        public decimal STPwdH { get; set; }
        public decimal STPwdNH { get; set; }
        public decimal STExSH { get; set; }
        public decimal STExSNH { get; set; }
        public decimal STkmH { get; set; }
        public decimal STkmNH { get; set; }
        public decimal STllH { get; set; }
        public decimal STllNH { get; set; }
        public decimal STKasMH { get; set; }
        public decimal STKasMNH { get; set; }
        public decimal STEwsH { get; set; }
        public decimal STEwsNH { get; set; }
        public decimal STgpH { get; set; }
        public decimal STgpNH { get; set; }

        //C1
        public decimal C1WomenH { get; set; }
        public decimal C1WomenNH { get; set; }
        public decimal C1PwdH { get; set; }
        public decimal C1PwdNH { get; set; }
        public decimal C1ExSH { get; set; }
        public decimal C1ExSNH { get; set; }
        public decimal C1kmH { get; set; }
        public decimal C1kmNH { get; set; }
        public decimal C1llH { get; set; }
        public decimal C1llNH { get; set; }
        public decimal C1KasMH { get; set; }
        public decimal C1KasMNH { get; set; }
        public decimal C1EwsH { get; set; }
        public decimal C1EwsNH { get; set; }
        public decimal C1gpH { get; set; }
        public decimal C1gpNH { get; set; }


        //IIA
        public decimal IIAWomenH { get; set; }
        public decimal IIAWomenNH { get; set; }
        public decimal IIAPwdH { get; set; }
        public decimal IIAPwdNH { get; set; }
        public decimal IIAExSH { get; set; }
        public decimal IIAExSNH { get; set; }
        public decimal IIAkmH { get; set; }
        public decimal IIAkmNH { get; set; }
        public decimal IIAllH { get; set; }
        public decimal IIAllNH { get; set; }
        public decimal IIAKasMH { get; set; }
        public decimal IIAKasMNH { get; set; }
        public decimal IIAEwsH { get; set; }
        public decimal IIAEwsNH { get; set; }
        public decimal IIAgpH { get; set; }
        public decimal IIAgpNH { get; set; }


        //IIB
        public decimal IIBWomenH { get; set; }
        public decimal IIBWomenNH { get; set; }
        public decimal IIBPwdH { get; set; }
        public decimal IIBPwdNH { get; set; }
        public decimal IIBExSH { get; set; }
        public decimal IIBExSNH { get; set; }
        public decimal IIBkmH { get; set; }
        public decimal IIBkmNH { get; set; }
        public decimal IIBllH { get; set; }
        public decimal IIBllNH { get; set; }
        public decimal IIBKasMH { get; set; }
        public decimal IIBKasMNH { get; set; }
        public decimal IIBEwsH { get; set; }
        public decimal IIBEwsNH { get; set; }
        public decimal IIBgpH { get; set; }
        public decimal IIBgpNH { get; set; }

        //IIIA
        public decimal IIIAWomenH { get; set; }
        public decimal IIIAWomenNH { get; set; }
        public decimal IIIAPwdH { get; set; }
        public decimal IIIAPwdNH { get; set; }
        public decimal IIIAExSH { get; set; }
        public decimal IIIAExSNH { get; set; }
        public decimal IIIAkmH { get; set; }
        public decimal IIIAkmNH { get; set; }
        public decimal IIIAllH { get; set; }
        public decimal IIIAllNH { get; set; }
        public decimal IIIAKasMH { get; set; }
        public decimal IIIAKasMNH { get; set; }
        public decimal IIIAEwsH { get; set; }
        public decimal IIIAEwsNH { get; set; }
        public decimal IIIAgpH { get; set; }
        public decimal IIIAgpNH { get; set; }


        //IIIB
        public decimal IIIBWomenH { get; set; }
        public decimal IIIBWomenNH { get; set; }
        public decimal IIIBPwdH { get; set; }
        public decimal IIIBPwdNH { get; set; }
        public decimal IIIBExSH { get; set; }
        public decimal IIIBExSNH { get; set; }
        public decimal IIIBkmH { get; set; }
        public decimal IIIBkmNH { get; set; }
        public decimal IIIBllH { get; set; }
        public decimal IIIBllNH { get; set; }
        public decimal IIIBKasMH { get; set; }
        public decimal IIIBKasMNH { get; set; }
        public decimal IIIBEwsH { get; set; }
        public decimal IIIBEwsNH { get; set; }
        public decimal IIIBgpH { get; set; }
        public decimal IIIBgpNH { get; set; }


        // calculation 
        public decimal VerticalRuleCal { get; set; }
        public int Vertical_rulesid { get; set; }
        public decimal RuleValue { get; set; }
        public string Vertical_Rules { get; set; }
        public int Rules_allocation_masterid { get; set; }
        public decimal? TotalSeatMatrix { get; set; }

        //tbl_SeatMatrix_Main
        public int SeatMaxId { get; set; }
        public int AcademicYear { get; set; }
        public int InstituteId { get; set; }
        public int ApplicantType { get; set; }
        public int Round { get; set; }
        public int Status { get; set; }
        public int CourseTypeId { get; set; }

        public DateTime AcademicYeardate { get; set; }

        //tbl_course_type_mast
        public int courseid { get; set; }
        public string coursetypename { get; set; }

        //tbl_status_master
        public string StatusName { get; set; }

        //tbl_SeatMatrix_TradeWise
        public int SeatMaxTradeId { get; set; }
        public int TradeId { get; set; }
        public decimal GMWH { get; set; }
        public decimal GMWNH { get; set; }
        public decimal GMPDH { get; set; }
        public decimal GMPDNH { get; set; }
        public decimal GMEXSH { get; set; }
        public decimal GMEXSNH { get; set; }
        public decimal GMKMH { get; set; }
        public decimal GMKMNH { get; set; }
        public decimal GMEWSH { get; set; }
        public decimal GMEWSNH { get; set; }
        public decimal GMGPH { get; set; }
        public decimal GMGPNH { get; set; }
        public decimal SCWH { get; set; }
        public decimal SCWNH { get; set; }
        public decimal SCPDH { get; set; }
        public decimal SCPDNH { get; set; }
        public decimal SCEXSH { get; set; }
        public decimal SCEXSNH { get; set; }
        public decimal SCKMH { get; set; }
        public decimal SCKMNH { get; set; }
        public decimal SCEWSH { get; set; }
        public decimal SCEWSNH { get; set; }
        public decimal SCGPH { get; set; }
        public decimal SCGPNH { get; set; }
        public decimal STWH { get; set; }
        public decimal STWNH { get; set; }
        public decimal STPDH { get; set; }
        public decimal STPDNH { get; set; }
        public decimal STEXSH { get; set; }
        public decimal STEXSNH { get; set; }
        public decimal STKMH { get; set; }
        public decimal STKMNH { get; set; }
        public decimal STEWSH { get; set; }
        public decimal STEWSNH { get; set; }
        public decimal STGPH { get; set; }
        public decimal STGPNH { get; set; }
        public decimal C1WH { get; set; }
        public decimal C1WNH { get; set; }
        public decimal C1PDH { get; set; }
        public decimal C1PDNH { get; set; }
        public decimal C1EXSH { get; set; }
        public decimal C1EXSNH { get; set; }
        public decimal C1KMH { get; set; }
        public decimal C1KMNH { get; set; }
        public decimal C1EWSH { get; set; }
        public decimal C1EWSNH { get; set; }
        public decimal C1GPH { get; set; }
        public decimal C1GPNH { get; set; }
        public decimal TWOAWH { get; set; }
        public decimal TWOAWNH { get; set; }
        public decimal TWOAPDH { get; set; }
        public decimal TWOAPDNH { get; set; }
        public decimal TWOAEXSH { get; set; }
        public decimal TWOAEXSNH { get; set; }
        public decimal TWOAKMH { get; set; }
        public decimal TWOAKMNH { get; set; }
        public decimal TWOAEWSH { get; set; }
        public decimal TWOAEWSNH { get; set; }
        public decimal TWOAGPH { get; set; }
        public decimal TWOAGPNH { get; set; }
        public decimal TWOBWH { get; set; }
        public decimal TWOBWNH { get; set; }
        public decimal TWOBPDH { get; set; }
        public decimal TWOBPDNH { get; set; }
        public decimal TWOBEXSH { get; set; }
        public decimal TWOBEXSNH { get; set; }
        public decimal TWOBKMH { get; set; }
        public decimal TWOBKMNH { get; set; }
        public decimal TWOBEWSH { get; set; }
        public decimal TWOBEWSNH { get; set; }
        public decimal TWOBGPH { get; set; }
        public decimal TWOBGPNH { get; set; }
        public decimal THREEAWH { get; set; }
        public decimal THREEAWNH { get; set; }
        public decimal THREEAPDH { get; set; }
        public decimal THREEAPDNH { get; set; }
        public decimal THREEAEXSH { get; set; }
        public decimal THREEAEXSNH { get; set; }
        public decimal THREEAKMH { get; set; }
        public decimal THREEAKMNH { get; set; }
        public decimal THREEAEWSH { get; set; }
        public decimal THREEAEWSNH { get; set; }
        public decimal THREEAGPH { get; set; }
        public decimal THREEAGPNH { get; set; }
        public decimal THREEBWH { get; set; }
        public decimal THREEBWNH { get; set; }
        public decimal THREEBPDH { get; set; }
        public decimal THREEBPDNH { get; set; }
        public decimal THREEBEXSH { get; set; }
        public decimal THREEBEXSNH { get; set; }
        public decimal THREEBKMH { get; set; }
        public decimal THREEBKMNH { get; set; }
        public decimal THREEBEWSH { get; set; }
        public decimal THREEBEWSNH { get; set; }
        public decimal THREEBGPH { get; set; }
        public decimal THREEBGPNH { get; set; }

        //New Calculation Property 
        //GM
        public decimal checkGeneralPoolGM { get; set; }
        public decimal checkWomenGM { get; set; }
        public decimal checkPhysicallyHandiGM { get; set; }
        public decimal checkExServiceGM { get; set; }
        public decimal checkKanMediumGM { get; set; }
        public decimal checkEwsGM { get; set; }

        public decimal checkGeneralPoolGMTotal { get; set; }
        public decimal checkWomenGMTotal { get; set; }
        public decimal checkPhysicallyHandiGMTotal { get; set; }
        public decimal checkExServiceGMTotal { get; set; }
        public decimal checkKanMediumGMTotal { get; set; }
        public decimal checkEwsGMTotal { get; set; }

        //SC
        public decimal checkGeneralPoolSC { get; set; }
        public decimal checkWomenSC { get; set; }
        public decimal checkPhysicallyHandiSC { get; set; }
        public decimal checkExServiceSC { get; set; }
        public decimal checkKanMediumSC { get; set; }
        public decimal checkEwsSC { get; set; }

        public decimal checkGeneralPoolSCTotal { get; set; }
        public decimal checkWomenSCTotal { get; set; }
        public decimal checkPhysicallyHandiSCTotal { get; set; }
        public decimal checkExServiceSCTotal { get; set; }
        public decimal checkKanMediumSCTotal { get; set; }
        public decimal checkEwsSCTotal { get; set; }

        //ST
        public decimal checkGeneralPoolST { get; set; }
        public decimal checkWomenST { get; set; }
        public decimal checkPhysicallyHandiST { get; set; }
        public decimal checkExServiceST { get; set; }
        public decimal checkKanMediumST { get; set; }
        public decimal checkEwsST { get; set; }

        public decimal checkGeneralPoolSTTotal { get; set; }
        public decimal checkWomenSTTotal { get; set; }
        public decimal checkPhysicallyHandiSTTotal { get; set; }
        public decimal checkExServiceSTTotal { get; set; }
        public decimal checkKanMediumSTTotal { get; set; }
        public decimal checkEwsSTTotal { get; set; }   
        
        //C1
        public decimal checkGeneralPoolC1 { get; set; }
        public decimal checkWomenC1 { get; set; }
        public decimal checkPhysicallyHandiC1 { get; set; }
        public decimal checkExServiceC1 { get; set; }
        public decimal checkKanMediumC1 { get; set; }
        public decimal checkEwsC1{ get; set; }

        public decimal checkGeneralPoolC1Total { get; set; }
        public decimal checkWomenC1Total { get; set; }
        public decimal checkPhysicallyHandiC1Total { get; set; }
        public decimal checkExServiceC1Total { get; set; }
        public decimal checkKanMediumC1Total { get; set; }
        public decimal checkEwsC1Total { get; set; } 
        
        //IIA
        public decimal checkGeneralPoolIIA { get; set; }
        public decimal checkWomenIIA { get; set; }
        public decimal checkPhysicallyHandiIIA { get; set; }
        public decimal checkExServiceIIA { get; set; }
        public decimal checkKanMediumIIA { get; set; }
        public decimal checkEwsIIA { get; set; }

        public decimal checkGeneralPoolIIATotal { get; set; }
        public decimal checkWomenIIATotal { get; set; }
        public decimal checkPhysicallyHandiIIATotal { get; set; }
        public decimal checkExServiceIIATotal { get; set; }
        public decimal checkKanMediumIIATotal { get; set; }
        public decimal checkEwsIIATotal { get; set; }

        //IIB
        public decimal checkGeneralPoolIIB { get; set; }
        public decimal checkWomenIIB { get; set; }
        public decimal checkPhysicallyHandiIIB { get; set; }
        public decimal checkExServiceIIB { get; set; }
        public decimal checkKanMediumIIB { get; set; }
        public decimal checkEwsIIB { get; set; }

        public decimal checkGeneralPoolIIBTotal { get; set; }
        public decimal checkWomenIIBTotal { get; set; }
        public decimal checkPhysicallyHandiIIBTotal { get; set; }
        public decimal checkExServiceIIBTotal { get; set; }
        public decimal checkKanMediumIIBTotal { get; set; }
        public decimal checkEwsIIBTotal { get; set; }

        //IIIA
        public decimal checkGeneralPoolIIIA { get; set; }
        public decimal checkWomenIIIA { get; set; }
        public decimal checkPhysicallyHandiIIIA { get; set; }
        public decimal checkExServiceIIIA { get; set; }
        public decimal checkKanMediumIIIA { get; set; }
        public decimal checkEwsIIIA { get; set; }

        public decimal checkGeneralPoolIIIATotal { get; set; }
        public decimal checkWomenIIIATotal { get; set; }
        public decimal checkPhysicallyHandiIIIATotal { get; set; }
        public decimal checkExServiceIIIATotal { get; set; }
        public decimal checkKanMediumIIIATotal { get; set; }
        public decimal checkEwsIIIATotal { get; set; }
        
        //IIIB
        public decimal checkGeneralPoolIIIB { get; set; }
        public decimal checkWomenIIIB { get; set; }
        public decimal checkPhysicallyHandiIIIB { get; set; }
        public decimal checkExServiceIIIB { get; set; }
        public decimal checkKanMediumIIIB { get; set; }
        public decimal checkEwsIIIB { get; set; }

        public decimal checkGeneralPoolIIIBTotal { get; set; }
        public decimal checkWomenIIIBTotal { get; set; }
        public decimal checkPhysicallyHandiIIIBTotal { get; set; }
        public decimal checkExServiceIIIBTotal { get; set; }
        public decimal checkKanMediumIIIBTotal { get; set; }
        public decimal checkEwsIIIBTotal { get; set; }


        public seatmatrixmodel modelseat { get; set; }
        public List<seatmatrixmodelNested> division_master { get; set; }
    }

    public class seatmatrixmodelNest
    {
        public List<seatmatrixmodel> list { get; set; }
        public List<seatmatrixmodel> seat_list { get; set; }

        public List<seatmatrixmodelNested> SelectListMatrix { get; set; }

    }

    public class seatmatrixmodelNested
    {
        public int slno { get; set; }
        public string TradeName { get; set; }
        public string ApplicantTypeDdl { get; set; }

        public string iti_college_name { get; set; }
        public int iti_college_id { get; set; }
        public int GMWomenH { get; set; }
        public int GMWomenNH { get; set; }
        public int GMPwdH { get; set; }
        public int GMPwdNH { get; set; }
        public int GMExSH { get; set; }
        public int GMExSNH { get; set; }
        public int GMkmH { get; set; }
        public int GMkmNH { get; set; }
        public int GMllH { get; set; }
        public int GMllNH { get; set; }
        public int GMKasMH { get; set; }
        public int GMKasMNH { get; set; }
        public int GMEwsH { get; set; }
        public int GMEwsNH { get; set; }
        public int GMgpH { get; set; }
        public int GMgpNH { get; set; }

        //SC
        public int SCWomenH { get; set; }
        public int SCWomenNH { get; set; }
        public int SCPwdH { get; set; }
        public int SCPwdNH { get; set; }
        public int SCExSH { get; set; }
        public int SCExSNH { get; set; }
        public int SCkmH { get; set; }
        public int SCkmNH { get; set; }
        public int SCllH { get; set; }
        public int SCllNH { get; set; }
        public int SCKasMH { get; set; }
        public int SCKasMNH { get; set; }
        public int SCEwsH { get; set; }
        public int SCEwsNH { get; set; }
        public int SCgpH { get; set; }
        public int SCgpNH { get; set; }

        //ST
        public int STWomenH { get; set; }
        public int STWomenNH { get; set; }
        public int STPwdH { get; set; }
        public int STPwdNH { get; set; }
        public int STExSH { get; set; }
        public int STExSNH { get; set; }
        public int STkmH { get; set; }
        public int STkmNH { get; set; }
        public int STllH { get; set; }
        public int STllNH { get; set; }
        public int STKasMH { get; set; }
        public int STKasMNH { get; set; }
        public int STEwsH { get; set; }
        public int STEwsNH { get; set; }
        public int STgpH { get; set; }
        public int STgpNH { get; set; }

        //C1
        public int C1WomenH { get; set; }
        public int C1WomenNH { get; set; }
        public int C1PwdH { get; set; }
        public int C1PwdNH { get; set; }
        public int C1ExSH { get; set; }
        public int C1ExSNH { get; set; }
        public int C1kmH { get; set; }
        public int C1kmNH { get; set; }
        public int C1llH { get; set; }
        public int C1llNH { get; set; }
        public int C1KasMH { get; set; }
        public int C1KasMNH { get; set; }
        public int C1EwsH { get; set; }
        public int C1EwsNH { get; set; }
        public int C1gpH { get; set; }
        public int C1gpNH { get; set; }


        //IIA
        public int IIAWomenH { get; set; }
        public int IIAWomenNH { get; set; }
        public int IIAPwdH { get; set; }
        public int IIAPwdNH { get; set; }
        public int IIAExSH { get; set; }
        public int IIAExSNH { get; set; }
        public int IIAkmH { get; set; }
        public int IIAkmNH { get; set; }
        public int IIAllH { get; set; }
        public int IIAllNH { get; set; }
        public int IIAKasMH { get; set; }
        public int IIAKasMNH { get; set; }
        public int IIAEwsH { get; set; }
        public int IIAEwsNH { get; set; }
        public int IIAgpH { get; set; }
        public int IIAgpNH { get; set; }


        //IIB
        public int IIBWomenH { get; set; }
        public int IIBWomenNH { get; set; }
        public int IIBPwdH { get; set; }
        public int IIBPwdNH { get; set; }
        public int IIBExSH { get; set; }
        public int IIBExSNH { get; set; }
        public int IIBkmH { get; set; }
        public int IIBkmNH { get; set; }
        public int IIBllH { get; set; }
        public int IIBllNH { get; set; }
        public int IIBKasMH { get; set; }
        public int IIBKasMNH { get; set; }
        public int IIBEwsH { get; set; }
        public int IIBEwsNH { get; set; }
        public int IIBgpH { get; set; }
        public int IIBgpNH { get; set; }

        //IIIA
        public int IIIAWomenH { get; set; }
        public int IIIAWomenNH { get; set; }
        public int IIIAPwdH { get; set; }
        public int IIIAPwdNH { get; set; }
        public int IIIAExSH { get; set; }
        public int IIIAExSNH { get; set; }
        public int IIIAkmH { get; set; }
        public int IIIAkmNH { get; set; }
        public int IIIAllH { get; set; }
        public int IIIAllNH { get; set; }
        public int IIIAKasMH { get; set; }
        public int IIIAKasMNH { get; set; }
        public int IIIAEwsH { get; set; }
        public int IIIAEwsNH { get; set; }
        public int IIIAgpH { get; set; }
        public int IIIAgpNH { get; set; }


        //IIIB
        public int IIIBWomenH { get; set; }
        public int IIIBWomenNH { get; set; }
        public int IIIBPwdH { get; set; }
        public int IIIBPwdNH { get; set; }
        public int IIIBExSH { get; set; }
        public int IIIBExSNH { get; set; }
        public int IIIBkmH { get; set; }
        public int IIIBkmNH { get; set; }
        public int IIIBllH { get; set; }
        public int IIIBllNH { get; set; }
        public int IIIBKasMH { get; set; }
        public int IIIBKasMNH { get; set; }
        public int IIIBEwsH { get; set; }
        public int IIIBEwsNH { get; set; }
        public int IIIBgpH { get; set; }
        public int IIIBgpNH { get; set; }

    }
}
