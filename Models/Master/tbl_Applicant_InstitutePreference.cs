using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Applicant_InstitutePreference
    {
        [Key]
        public int InstitutePreferenceId { get; set; }
        public int ApplicantId { get; set; }
        public int PreferenceId { get; set; }
        public int InstituteId { get; set; }
        public int PreferenceType { get; set; }
        public int TradeId { get; set; }
        public int DistrictId { get; set; }
        public int TalukaId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }
}
