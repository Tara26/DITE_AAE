using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models.Admission
{
    public class insertRecordsForTrade
    {
        public string trade_name { get; set; }
        public int Units { get; set; }
        public bool IsPPP { get; set; }
        public bool DualSystemTraining { get; set; }
        public int Trade_ITI_Id { get; set; }
        public int UnitId { get; set; }
                   
        public int ShiftId { get; set; }
        public int SeatsPerUnit { get; set; }
        public int SeatsTypeId { get; set; }         
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Nullable<int> ModifedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int Govt_Gia_seats { get; set; }
        public int PPP_seats { get; set; }
        public int Management_seats { get; set; }

        public int Trade_ITI_seat_trans_id { get; set; }
        public int Trade_ITI_seat_Id { get; set; }
        public DateTime? Trans_Date { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public int FlowId { get; set; }
    }


    public class SeatAvailabilityMaster
    {
        public int division_id { get; set; }
        public string division_name { get; set; }
        public int dist_id { get; set; }
        public string dist_name { get; set; }
        public int SlNo { get; set; }

        public int Trade_ITI_seat_id { get; set; }
        public int Trade_ITI_Id { get; set; }
        //public int ITICode { get; set; }
        public int UnitId { get; set; }
        public int ShiftId { get; set; }
        public int SeatsPerUnit { get; set; }
        public int SeatsTypeId { get; set; }
        public bool IsPPP { get; set; }
        public bool DualSystemTraining { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ModifedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int RoleId { get; set; }
        //tbl_iti_college_details_Admission
        public string iti_college_code { get; set; }
        public int district_id { get; set; }
        public int taluk_id { get; set; }
        public string village_or_town { get; set; }
        public string college_address { get; set; }
        public int location_id { get; set; }
        public int css_code { get; set; }
        public string iti_college_name { get; set; }
        public Nullable<decimal> geo { get; set; }
        public string file_ref_no { get; set; }
        public bool? is_active { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<DateTime> creation_datetime { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public int iti_college_id { get; set; }
        public string email_id { get; set; }
        public string phone_num { get; set; }
        public string MISCode { get; set; }
        public string Address { get; set; }
        public string Constituency { get; set; }
        public string BuildUpArea { get; set; }
        public Nullable<DateTime> AffiliationDate { get; set; }
        public int NoOfShifts { get; set; }
        public int Insitute_TypeId { get; set; }
        public int Units { get; set; }
        public string UploadAffiliationDoc { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public string SeatType { get; set; }
        public string shifts { get; set; }
        public int u_id { get; set; }
        public string units { get; set; }

        public int Division_id { get; set; }
        public string Division_name { get; set; }
        public bool? Division_is_active { get; set; }
        public int? Division_created_by { get; set; }
        public DateTime? Division_creation_datetime { get; set; }
        public int Division_updated_by { get; set; }
        public SelectList DivisionList { get; set; }

        public int District_dist_id { get; set; }
        public string District_dist_name { get; set; }
        public int Dist_division_id { get; set; }
        public bool Dist_is_active { get; set; }
        public int Dist_created_by { get; set; }
        public int Dist_id_creation_datetime { get; set; }
        public int Dist_updated_by { get; set; }
        public int Dist_updation_datetime { get; set; }
        public int trade_id { get; set; }
        public string trade_name { get; set; }

        public int user_id { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }

        //public int TradeCode { get; set; }
        public SelectList DistrictList { get; set; }
        public List<SeatAvailabilityMaster> seatAvailabilityMaster { get; set; }
        public SelectList StatusList { get; set; }
        public SelectList LoginRoleList { get; set; }
        public SelectList TalukList { get; set; }
        public int? PPP_seats { get; set; }
        public int? IMCPrivateManageMent { get; set; }
        public string Shift { get; set; }
    }

    public class SeatAvailabilityTrans
    {
        public int Trade_ITI_seat_trans_id { get; set; }
        public int Trade_ITI_seat_Id { get; set; }
        public DateTime? Trans_Date { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public int CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int FlowId { get; set; }
    }

    public class tblunits
    {     
        public int u_id { get; set; }
        public string units { get; set; }
        public bool? u_is_active { get; set; }
        public Nullable<int> u_created_by { get; set; }
        public Nullable<DateTime> u_creation_datetime { get; set; }
        public Nullable<int> u_updated_by { get; set; }
        public Nullable<DateTime> u_updation_datetime { get; set; }


    }

    //public class tbl_iti_college_details_Admission
    //{
       
       

    //}

    public class tbl_shifts
    {   
        public int s_id { get; set; }
        public string shifts { get; set; }
        public bool? s_is_active { get; set; }
        public Nullable<DateTime> s_creation_datetime { get; set; }
        public Nullable<int> s_created_by { get; set; }
        public Nullable<DateTime> s_updation_datetime { get; set; }
        public Nullable<int> s_updated_by { get; set; }
    }

    public class tbl_seat_type
    {
        public int Seat_type_id { get; set; }
        public string SeatType { get; set; }

        public bool? IsActive { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
    }

    public class tbl_ITI_Trade
    {
       
        public int Trade_ITI_id { get; set; }
        public int TradeCode { get; set; }

        public int ITICode { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

    }

    public class tbl_trade_mast
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

    }

}
