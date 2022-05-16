using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
   public class tbl_ITI_trade_seat_trans_Excel
    {
        [Key]
        public int ID { get; set; }
        public string MISCode { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string Type { get; set; }
        public int InstituteId { get; set; }
        public string InstituteName { get; set; }
        public int TradeId { get; set; }
        public string TradeName { get; set; }
        public int SeatsPerUnit { get; set; }
        public int Unit { get; set; }
        public int GovtGIASeats { get; set; }
        public int PPPSeats { get; set; }
        public int IMCPrivateManageMent { get; set; }
        public bool IsPPP { get; set; }
        public int Shift { get; set; }
        public bool DualSystemTraining { get; set; }
    }
}
