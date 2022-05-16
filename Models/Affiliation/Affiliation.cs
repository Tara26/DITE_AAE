using Models.Affiliation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class UploadAffiliation
    {
        public int flag { get; set; }
        public string status { get; set; }
    }

    public class AffiliationCollegeCourseType
    {
        public string course_name { get; set; }
        public int course_id { get; set; }
    }

    public class AffiliationCollegeDivision
    {
        public string division_name { get; set; }
        public int division_id { get; set; }
    }

    public class AffiliationCollegeDistrict
    {
        public string district { get; set; }
        public int district_id { get; set; }
    }

    public class AffiliationCollegeTrades
    {
        public string trade_code { get; set; }
        public int trade_id { get; set; }
    }

    public class AffiliationInstitutionType
    {
        public int institute_id { get; set; }
        public string insti_name { get; set; }
    }

    public class AffiliationLocationType
    {
        public int location_id { get; set; }
        public string location_name { get; set; }
    }

    public class AffiliationTaluk
    {
        public int taluk_id { get; set; }
        public string taluk_name { get; set; }
    }

    public class AffiliationConstiteuncy
    {
        public int consti_id { get; set; }
        public string consti_name { get; set; }
    }

    public class AffiliationPanchayat
    {
        public int pancha_id { get; set; }
        public string pancha_name { get; set; }
    }

    public class TradeActiveandDeactiveStatus
    {
        public int itiID { get; set; }
        public string status { get; set; }
        public string active { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }

        public string fileName { get; set; }
        public string filePath { get; set; }
        public int userID { get; set; }
        public string ActDeActRemarks { get; set; }
        public int? Flowid { get; set; }
        public int? StatusId { get; set; }
        public int? clgId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string status1 { get; set; }
        public bool IsActive { get; set; }
        public HttpPostedFileBase ActiveImageFile { get; set; }

        public string AffiliateFilePath { get; set; }
        public string AffiliateFileName { get; set; }
        public string DeaffiliateOrderNo { get; set; }
        public Nullable<DateTime> DeffiliateDate { get; set; }

        public Nullable<DateTime> AffiliateDate { get; set; }
        public string AffiliateOrderNo { get; set; }
    }

    public class AffiliationCollegeDetails
    {
        public string AidedUnaidedTrade { get; set; }
        public string name_of_iti { get; set; }
        public string mis_code { get; set; }
        public string type_of_iti { get; set; }
        public string trade { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string taluka { get; set; }
        public string panchayat { get; set; }
        public string village { get; set; }
        public string constituency { get; set; }
        public string build_up_area { get; set; }
        public string geo_location { get; set; }
        public string address { get; set; }
        public string location_type { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public DateTime? affiliation_date { get; set; }
        public string no_trades { get; set; }
        public int? no_units { get; set; }
        public int? no_shifts { get; set; }
        public string css_code { get; set; }
        public int? division_id { get; set; }
        public int? trade_id { get; set; }
        public int iti_college_id { get; set; }
        public int? type_of_iti_id { get; set; }
        public int? location_type_id { get; set; }
        public List<int> list_trades { get; set; }
        public int? dist_id { get; set; }
        public int? taluk_id { get; set; }
        public int? consti_id { get; set; }
        public int? pancha_id { get; set; }
        public int? village_id { get; set; }
        public int? css_code_id { get; set; }
        public string FileUploadPath { get; set; }
        public string date { get; set; }
        public int CreatedBy { get; set; }
        public string status { get; set; }
        public string remarks { get; set; }
        public bool isSelect { get; set; }
        public List<SelectListItem> trades { get; set; }
        public List<AffiliationTrade> trades_list { get; set; }
        public int? Pincode { get; set; }
        public List<int> list_units { get; set; }
        public List<bool> is_uploads { get; set; }
        public List<int> list_it_trade_id { get; set; }
        public int trade_iti_id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? status_id { get; set; }
        public int flag { get; set; }
        public int? flow_id { get; set; }
        public int? CreatedByChk { get; set; }
        public bool en_edit { get; set; }
        public List<int> list_keys { get; set; }
        public List<int> list_shift_id { get; set; }
        public List<TradeShift> shifts { get; set; }
        public string units { get; set; }
        public string Website { get; set; }
        public DateTime? AffiliationOrderNoDate { get; set; }
        public int? Scheme { get; set; }
        public string AffiliationOrderNo { get; set; }
        public string order_no_date { get; set; }
        public string scheme_name { get; set; }
        public int? course_code { get; set; }
        public string course_name { get; set; } 
        public string division { get; set; }
        public string sector { get; set; }
        public string trade_code { get; set; }
        public int? batch_size { get; set; }
        public int? color_flag { get; set; }
        public string zipcode { get; set; }
        public bool isUploaded { get; set; }
        public string trade_type { get; set; }
        public string duration { get; set; }
        public List<ActiveDeactiveTradeHistory> activeDeactiveTradeHistories { get; set; }
        public string Activefilepath { get; set; }
        public int? ITI_Trade_ShiftId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsActiveNew { get; set; }
        public bool? ActiveDeactive { get; set; }
        public List<ActiveDeactiveTradeHistory> TradeShiftDetails { get; set; }
        public int Slno { get; set; }
        public int? Trade_ITI_id { get; set; }
        public int ApprovalFlowId { get; set; }
        public string Pubdate { get; set; }
        //public string TStatus { get; set; }

        //ram
        public string nooftrades { get; set; }
        public int? NoofTrades { get; set; }


        public string DeactivateOrderNo { get; set; }
        public string DeactivateDate { get; set; }

        public string ActivateDate { get; set; }
        public string ActivateOrderNo { get; set; }
        public string ActivateFileName { get; set; }
        public string ActivateFilePath { get; set; }

        public string AffiliateFilePath { get; set; }
        public string AffiliateFileName { get; set; }
        public string NewAffiliationOrderNo { get; set; }
        public string NewAffiliationOrderNoDate { get; set; }
        public string UploadTradeAffiliationDoc { get; set; }
        public string AffiliationFilePath { get; set; }
        public int? tshift { get; set; }
        public string Filename { get; set; }
        public List<AffiliationDocuments> AffiliationDocs { get; set; }
        public AffiliationDocuments AffiliationDoc { get; set; }
        public List<AffiliationDocuments> TradeWiseAffiliationDoc { get; set; }

    }

    public class AffiliationCollegeDetails1
    {
        public int? trade_iti_id_publish { get; set; }
        public string status { get; set; }
    }


    public class AffiliationCollegeDetailsTest
    {
        public string name_of_iti { get; set; }

        public int iti_college_id { get; set; }
        public int? type_of_iti_id { get; set; }

        public int? location_type_id { get; set; }

        public string mis_code { get; set; }
        public int? division_id { get; set; }

        public int? dist_id { get; set; }
        public int? taluk_id { get; set; }

        public int? consti_id { get; set; }

        public string build_up_area { get; set; }

        public string address { get; set; }
        public string geo_location { get; set; }

        public string email { get; set; }

        public DateTime? affiliation_date { get; set; }

        public string phone_number { get; set; }

        public int? no_units { get; set; }

        public string date { get; set; }

        public int? Pincode { get; set; }
        public int? course_code { get; set; }
        public string Website { get; set; }

        public string AffiliationOrderNo { get; set; }
        public int? Scheme { get; set; }
        public DateTime AffiliationOrderNoDate { get; set; }

        public string order_no_date { get; set; }
        public List<int> list_trades { get; set; }

        public List<int> list_units { get; set; }
        public List<int> list_keys { get; set; }
        public List<string> list_type { get; set; }

        public List<AffiliationTrade> trades_list { get; set; }

        public int? pancha_id { get; set; }

        public int? village_id { get; set; }

        public int? no_shifts { get; set; }
        public string FileUploadPath { get; set; }

        public int? color_flag { get; set; }

        public int CreatedBy { get; set; }

        //add new fields
        public int trade_iti_id { get; set; }

        public List<TradeShift> shifts { get; set; }

        public int? trade_id { get; set; }

        public int NoofTrades { get; set; }

        public string AidedUnaidedTrade { get; set; }
        public string NewAffiliationOrderNo { get; set; }
        public Nullable<DateTime> NewAffiliationOrderNoDate { get; set; }
        public string UploadTradeAffiliationDoc { get; set; }
        public int? Flag { get; set; }
        public bool IsActive { get; set; }
        public int? flow_id { get; set; }
        public string remarks { get; set; }
    }



    public class AffiliationCollegeDetailsTest1
    {
        public string name_of_iti { get; set; }

        public int iti_college_id { get; set; }
        public int? type_of_iti_id { get; set; }

        public int? location_type_id { get; set; }

        public string mis_code { get; set; }
        public int? division_id { get; set; }

        public int? dist_id { get; set; }
        public int? taluk_id { get; set; }

        public int? consti_id { get; set; }

        public string build_up_area { get; set; }

        public string address { get; set; }
        public string geo_location { get; set; }

        public string email { get; set; }

        public DateTime? affiliation_date { get; set; }

        public string phone_number { get; set; }

        public int? no_units { get; set; }

        public string date { get; set; }

        public int? Pincode { get; set; }
        public int? course_code { get; set; }
        public string Website { get; set; }

        public string AffiliationOrderNo { get; set; }
        public int? Scheme { get; set; }
        public DateTime AffiliationOrderNoDate { get; set; }

        public string order_no_date { get; set; }
        public List<int> list_trades { get; set; }

        public List<int> list_units { get; set; }
        public List<int> list_keys { get; set; }

        public List<AffiliationTrade> trades_list { get; set; }

        public int? pancha_id { get; set; }

        public int? village_id { get; set; }

        public int? no_shifts { get; set; }
        public string FileUploadPath { get; set; }

        public int? color_flag { get; set; }

        public int CreatedBy { get; set; }

        //add new fields
        public int trade_iti_id { get; set; }

        public List<TradeShift> shifts { get; set; }

        public string trade { get; set; }

        public string district { get; set; }

        public string taluka { get; set; }

        public string status { get; set; }
        public int? status_id { get; set; }

        public DateTime? CreatedOn { get; set; }

        public bool isUploaded { get; set; }

        public int? trade_id { get; set; }

        public int? NoofTrades { get; set; }
    }



    public class AffiliationNested
    {
        public List<AffiliationCollegeDetails> list { get; set; }
        public List<AffiliationCollegeDetails> pubs { get; set; }
        public List<AffiliationCollegeDetails> pubs_list { get; set; }
        public List<AffiliationCollegeDetails> aff_list { get; set; }
        public List<AffiliationCollegeDetails> upl_list { get; set; }
        public AffiliationCollegeDetails aff { get; set; }
        public List<SelectListItem> inst_list { get; set; }
        public List<SelectListItem> loca_type_list { get; set; }
        public List<SelectListItem> trades_list { get; set; }
        public List<SelectListItem> div_list { get; set; }
        public List<SelectListItem> dist_list { get; set; }
        public List<SelectListItem> taluk_list { get; set; }
        public List<SelectListItem> consti_list { get; set; }
        public List<SelectListItem> pancha_list { get; set; }
        public List<SelectListItem> village_list { get; set; }
        public List<SelectListItem> css_code_list { get; set; }
        public List<ActiveandDeactiveDeatils> list1 { get; set; }
        public List<TradeHistory> his_list { get; set; }
        public List<SelectListItem> cou_list { get; set; }
        public List<SelectListItem> schem_list { get; set; }

        //Affiliation Inst
        public List<DeAffiliateInstitute> Cwinst_list { get; set; }
        public List<DeAffiliateInstitute> Osinst_list { get; set; }
        public List<DeAffiliateInstitute> ADDDinst_list { get; set; }
        public List<DeAffiliateInstitute> Approvedinst_list { get; set; }
        public List<ActiveandDeactiveDeatils> ViewADDetails { get; set; }

        public List<ActiveandDeactiveUnitsDeatils> Cwunits_list { get; set; }
        public List<ActiveandDeactiveUnitsDeatils> Osunit_list { get; set; }
        public List<ActiveandDeactiveUnitsDeatils> ADDDunit_list { get; set; }
        public List<ActiveandDeactiveUnitsDeatils> ViewUnitDetails { get; set; }
        public List<ActiveandDeactiveUnitsDeatils> adTrade_list { get; set; }
        public AffiliationDocuments Affiliationdoc { get; set; }



    }


    public class ToPublishRecords
    {
        public List<AffiliationCollegeDetails1> pubs_list_topublish { get; set; }
    }



    public class AffiliationTrade
    {
        public string trade_name { get; set; }
        public int? trade_id { get; set; }
        public int? units { get; set; }
        public string file_upload_path { get; set; }
        public int trade_iti_id { get; set; }
        public bool is_uploaded { get; set; }
        public string remarks { get; set; }
        public int? status_id { get; set; }
        public int? flow_id { get; set; }
        public bool en_edit { get; set; }
        public List<TradeShift> list { get; set; }
        public int? sessionKey { get; set; }
        public int CreatedBy { get; set; }
        public bool is_published { get; set; }
        public string trade_code { get; set; }
        public bool ISActive { get; set; }
        public string type { get; set; }

        public string tradetype { get; set; }
        public string NewAffiliationOrderNo { get; set; }
        public Nullable<DateTime> NewAffiliationOrderNoDate { get; set; }
        public string name_of_iti { get; set; }

        public int iti_college_id { get; set; }
        public int? type_of_iti_id { get; set; }

        public int? location_type_id { get; set; }

        public string mis_code { get; set; }
        public int? division_id { get; set; }

        public int? dist_id { get; set; }
        public int? taluk_id { get; set; }

        public int? consti_id { get; set; }

        public string build_up_area { get; set; }

        public string address { get; set; }
        public string geo_location { get; set; }

        public string email { get; set; }

        public DateTime? affiliation_date { get; set; }

        public string phone_number { get; set; }

        public int? no_units { get; set; }

        public string date { get; set; }

        public int? Pincode { get; set; }
        public int? course_code { get; set; }
        public string Website { get; set; }
        public int? Scheme { get; set; }
        public DateTime AffiliationOrderNoDate { get; set; }

        public string order_no_date { get; set; }
        public int? pancha_id { get; set; }

        public int? village_id { get; set; }
        public string AffiliationOrderNo { get; set; }
        public string AidedUnaidedTrade { get; set; }
    }

    public class TradeHistory
    {
        public int Trade_ITI_His_id { get; set; }
        public int Trade_ITI_id { get; set; }
        public int TradeCode { get; set; }
        public int? ITICode { get; set; }
        public int? StatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int? Unit { get; set; }
        public int? FlowId { get; set; }
        public bool IsActive { get; set; }
        public string FileUploadPath { get; set; }
        public string TradeName { get; set; }
        public string Status { get; set; }
        public string Flow_user { get; set; }
        public string date { get; set; }
        public string created_by { get; set; }
        public string sent_by { get; set; }
    }

    public class TradeShift
    {
        public int ITI_Trade_Shift_Id { get; set; }
        public int ITI_Trade_Id { get; set; }
        public int Units { get; set; }
        public int Shift { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string IsPPP { get; set; }
        public string Dual_System { get; set; }
        public string st_unit { get; set; }
        public string st_shift { get; set; }
        public int? new_add { get; set; }
        public int? Status { get; set; }
        public string TotalUnits { get; set; }
        public string TotalShift { get; set; }
    }

    public class TradeShiftSessions
    {
       public List<TradeShift> list { get; set; }
       public int sessionKey { get; set; }
    }

    public class NestedTradeSession
    {
        public List<TradeShiftSessions> sessions { get; set; } 
    }

    public class Trade
    {
        public int trade_id { get; set; }
        public string trade_name { get; set; }
        public int trade_type_id { get; set; }
        public int trade_course_id { get; set; }

        public bool? trdae_is_active { get; set; }
        public Nullable<DateTime> trade_creation_date { get; set; }
        public Nullable<int> trade_created_by { get; set; }
        public Nullable<DateTime> trade_updation_date { get; set; }
        public Nullable<int> trade_updated_by { get; set; }
        public string trade_code { get; set; }
        public int? trade_unit { get; set; }
        public int? trade_seating { get; set; }
        public string trade_Mini_Qualification { get; set; }
        public string trade_duration { get; set; }
        public int? sector_id { get; set; }
        public string sector { get; set; }
        public string trade_type { get; set; }

        public string AidedUnaidedTrade { get; set; }

    }

    public class MisCodes
    {
        public List<SelectListItem> list { get; set; }
        public int total_count { get; set; }
        public bool Ismore { get; set; }
    }
    public class ActiveDeactiveTradeHistory
    {
        public int Trade_ITI_His_id { get; set; }
        public int Trade_ITI_id { get; set; }
        public int TradeCode { get; set; }
        public int? ITICode { get; set; }
        public int? StatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? Unit { get; set; }
        public int? FlowId { get; set; }
        public bool IsActive { get; set; }
        public string FileUploadPath { get; set; }
        public string TradeName { get; set; }
        public string Status { get; set; }
        public string Flow_user { get; set; }
        public string date { get; set; }
        public string FilePath { get; set; }
        public string ActDeActRemarks { get; set; }
        public string CreatedUser { get; set; }
        public string User { get; set; }
        public string ActiveFilepath { get; set; }


    }
}
