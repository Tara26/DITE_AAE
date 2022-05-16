using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_AdmissionatInstituteFeeDetails
    {
        [Key]
        public int AdmissionatInstituteFeeId { get; set; }
        public int InstituteFee { get; set; }
        public string InstituteType { get; set; }
        public bool IsActive { get; set; }
        public int? Institute_type_id { get; set; }
        public int? location_id { get; set; }
        public int? trade_type_id { get; set; }
    }
}
