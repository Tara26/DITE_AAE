using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_transfer_seat_details
    {
        [Key]
        public int Id { get; set; }
        public int Admit_id { get; set; }
        public int DivisionId { get; set; }
        public int DistrictId { get; set; }
        public int TalukId { get; set; }
        public int MISITICode { get; set; }
        public int InstituteType { get; set; }
        public int IstituteId { get; set; }
        public int TradeId { get; set; }
        public int Unit { get; set; }
        public int Shift { get; set; }
        public int DualSystem { get; set; }

    }
}
