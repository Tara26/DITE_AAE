using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
   public class temp_tbl_iti_college_details
    {
        public string iti_college_code { get; set; }
        public string district { get; set; }
        public string taluk { get; set; }
        public string village { get; set; }
        public string college_address { get; set; }
        public string location { get; set; }
        public string css_code { get; set; }
        public string iti_college_name { get; set; }
        public string geo { get; set; }
        public string file_ref_no { get; set; }
        public bool is_active { get; set; }
        public int? created_by { get; set; }
        public DateTime? creation_datetime { get; set; }
        [Key]
        public int iti_college_id_temp { get; set; }
        public string email_id { get; set; }
        public string phone_num { get; set; }
        public string MISCode { get; set; }
        public string Address { get; set; }
        public string Constituency { get; set; }
        public string BuildUpArea { get; set; }
        public string AffiliationDate { get; set; }
        public string NoOfShifts { get; set; }
        public string Insitute_Type { get; set; }
        public string Units { get; set; }
        public int? StatusId { get; set; }
        public string Remarks { get; set; }
        public string Panchayat { get; set; }
        public string CourseCode { get; set; }
        public string division { get; set; }
        public string PinCode { get; set; }
        public string Website { get; set; }
        public string AffiliationOrderNoDate { get; set; }
        public string Scheme { get; set; }
        public string AffiliationOrderNo { get; set; }
        public string NoofTrades { get; set; }
        public string AidedUnaidedTrade { get; set; }

       
    }

    public class temp_tbl_ITI_Trade
    {
        [Key]
        public int  Trade_ITI_id_temp { get; set; }
        public string TradeCode { get; set; }
        public int? ITICode { get; set; }
        public int? StatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsUploaded { get; set; }

        public string NoofTrades { get; set; }

    }

    public class temp_tbl_ITI_Trade_Shift
    {
        [Key]
        public int ITI_Trade_Shift_Id_temp { get; set; }
        public int ITI_Trade_Id_temp { get; set; }
        public string Units { get; set; }
        public string Shift  { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string IsPPP { get; set; }
        public string Dual_System { get; set; }

    }
}
