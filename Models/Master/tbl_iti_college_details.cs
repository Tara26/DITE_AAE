using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_iti_college_details
    {
        public string iti_college_code { get; set; }
        public int? division_id { get; set; }
        public int? district_id { get; set; }
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
        [Key]
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

        public int? NoofTrades { get; set; }

        public string AidedUnaidedTrade { get; set; }
        public Nullable<DateTime> NewAffiliationOrderNoDate { get; set; }
        public string NewAffiliationOrderNo { get; set; }
        public string UploadTradeAffiliationDoc { get; set; }
    }
}
