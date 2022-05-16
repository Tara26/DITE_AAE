using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class DivisionModel
    {
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public List<InstituteModel> Institutes { get; set; }

    }
    public class InstituteModel
    {
        public int InstituteId { get; set; }
        public string InstituteName { get; set; }
        public List<TradesModel> Trades { get; set; }
    }
    public class TradesModel
    {
        public int TradeId { get; set; }
        public string TradeName { get; set; }
    }
    public class DivisionUpdateModel
    {
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public List<InstituteUpdateModel> Institutes { get; set; }

    }
    public class DivisionListModel
    {
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public List<InstituteListModel> Institutes { get; set; }
        
    }
    public class InstituteUpdateModel
    {
        public int InstituteId { get; set; }
        public string InstituteName { get; set; }
        public int? DivisionId { get; set; }
        public List<Tradewideseat> Trades { get; set; }
        public int Round { get; set; }
        public int TotalSeats { get; set; }
    }
    public class InstituteListModel
    {
        public int InstituteId { get; set; }
        public string InstituteName { get; set; }
        public List<GenerateTradewiseSeat> Trades { get; set; }
        public int TotalSeats { get; set; }
        public int Round { get; set; }
        public string MisCode { get; set; }
        public string DistrictName { get; set; }
        public string TalukName { get; set; }
    }
    public class TradeslistModel
    {
        public int TradeId { get; set; }
        public string TradeName { get; set; }
        public int Seats { get; set; }
    }
    public class VerticleCat
    {
        public int Vcat { get; set; }
        public decimal SeatValue { get; set; }
    }
    public class HorizontalCat
    {
        public int Hcat { get; set; }
        public decimal SeatValue { get; set; }
    }
    public class CollegeTradeSeat
    {
        public int TradeId { get; set; }
        public string TradeName { get; set; }
        public int Seats { get; set; }
        public int CollegeId { get; set; }
        public int Units { get; set; }
        public string TradeYear { get; set; }
    }
    public class Tradewideseat
    {
        public int TradeId { get; set; }
        public string TradeName { get; set; }
        public int Units { get; set; }
        public string TradeYear { get; set; }
        public int Seats { get; set; }
        public int? GMWH { get; set; }
        public int? GMWNH { get; set; }
        public int? GMPDH { get; set; }
        public int? GMPDNH { get; set; }
        public int? GMEXSH { get; set; }
        public int? GMEXSNH { get; set; }
        public int? GMKMH { get; set; }
        public int? GMKMNH { get; set; }
        public int? GMEWSH { get; set; }
        public int? GMEWSNH { get; set; }
        public int? GMGP { get; set; }
        public int? GMGPH { get; set; }
        public int? GMGPNH { get; set; }
        public int? SCWH { get; set; }
        public int? SCWNH { get; set; }
        public int? SCPDH { get; set; }
        public int? SCPDNH { get; set; }
        public int? SCEXSH { get; set; }
        public int? SCEXSNH { get; set; }
        public int? SCKMH { get; set; }
        public int? SCKMNH { get; set; }
        public int? SCEWSH { get; set; }
        public int? SCEWSNH { get; set; }
        public int? SCGP { get; set; }
        public int? SCGPH { get; set; }
        public int? SCGPNH { get; set; }
        public int? STWH { get; set; }
        public int? STWNH { get; set; }
        public int? STPDH { get; set; }
        public int? STPDNH { get; set; }
        public int? STEXSH { get; set; }
        public int? STEXSNH { get; set; }
        public int? STKMH { get; set; }
        public int? STKMNH { get; set; }
        public int? STEWSH { get; set; }
        public int? STEWSNH { get; set; }
        public int? STGP { get; set; }
        public int? STGPH { get; set; }
        public int? STGPNH { get; set; }
        public int? C1WH { get; set; }
        public int? C1WNH { get; set; }
        public int? C1PDH { get; set; }
        public int? C1PDNH { get; set; }
        public int? C1EXSH { get; set; }
        public int? C1EXSNH { get; set; }
        public int? C1KMH { get; set; }
        public int? C1KMNH { get; set; }
        public int? C1EWSH { get; set; }
        public int? C1EWSNH { get; set; }
        public int? C1GP { get; set; }
        public int? C1GPH { get; set; }
        public int? C1GPNH { get; set; }
        public int? TWOAWH { get; set; }
        public int? TWOAWNH { get; set; }
        public int? TWOAPDH { get; set; }
        public int? TWOAPDNH { get; set; }
        public int? TWOAEXSH { get; set; }
        public int? TWOAEXSNH { get; set; }
        public int? TWOAKMH { get; set; }
        public int? TWOAKMNH { get; set; }
        public int? TWOAEWSH { get; set; }
        public int? TWOAEWSNH { get; set; }
        public int? TWOAGP { get; set; }
        public int? TWOAGPH { get; set; }
        public int? TWOAGPNH { get; set; }
        public int? TWOBWH { get; set; }
        public int? TWOBWNH { get; set; }
        public int? TWOBPDH { get; set; }
        public int? TWOBPDNH { get; set; }
        public int? TWOBEXSH { get; set; }
        public int? TWOBEXSNH { get; set; }
        public int? TWOBKMH { get; set; }
        public int? TWOBKMNH { get; set; }
        public int? TWOBEWSH { get; set; }
        public int? TWOBEWSNH { get; set; }
        public int? TWOBGP { get; set; }
        public int? TWOBGPH { get; set; }
        public int? TWOBGPNH { get; set; }
        public int? THREEAWH { get; set; }
        public int? THREEAWNH { get; set; }
        public int? THREEAPDH { get; set; }
        public int? THREEAPDNH { get; set; }
        public int? THREEAEXSH { get; set; }
        public int? THREEAEXSNH { get; set; }
        public int? THREEAKMH { get; set; }
        public int? THREEAKMNH { get; set; }
        public int? THREEAEWSH { get; set; }
        public int? THREEAEWSNH { get; set; }
        public int? THREEAGP { get; set; }
        public int? THREEAGPH { get; set; }
        public int? THREEAGPNH { get; set; }
        public int? THREEBWH { get; set; }
        public int? THREEBWNH { get; set; }
        public int? THREEBPDH { get; set; }
        public int? THREEBPDNH { get; set; }
        public int? THREEBEXSH { get; set; }
        public int? THREEBEXSNH { get; set; }
        public int? THREEBKMH { get; set; }
        public int? THREEBKMNH { get; set; }
        public int? THREEBEWSH { get; set; }
        public int? THREEBEWSNH { get; set; }
        public int? THREEBGP { get; set; }
        public int? THREEBGPH { get; set; }
        public int? THREEBGPNH { get; set; }
    }

    public class GenerateTradewiseSeat
    {
        public int Seats { get; set; }
        public int TradeId { get; set; }
        public string TradeName { get; set; }
        public int Units { get; set; }
        public string TradeYear { get; set; }
        public decimal GMW { get; set; }
        public decimal GMWH { get; set; }
        public decimal GMWNH { get; set; }
        public decimal GMPD { get; set; }
        public decimal GMPDH { get; set; }
        public decimal GMPDNH { get; set; }
        public decimal GMEXS { get; set; }
        public decimal GMEXSH { get; set; }
        public decimal GMEXSNH { get; set; }
        public decimal GMKM { get; set; }
        public decimal GMKMH { get; set; }
        public decimal GMKMNH { get; set; }
        public decimal GMEWS { get; set; }
        public decimal GMEWSH { get; set; }
        public decimal GMEWSNH { get; set; }
        public decimal GMGP { get; set; }
        public decimal GMGPH { get; set; }
        public decimal GMGPNH { get; set; }
        public decimal SCW { get; set; }
        public decimal SCWH { get; set; }
        public decimal SCWNH { get; set; }
        public decimal SCPD { get; set; }
        public decimal SCPDH { get; set; }
        public decimal SCPDNH { get; set; }
        public decimal SCEXS { get; set; }
        public decimal SCEXSH { get; set; }
        public decimal SCEXSNH { get; set; }
        public decimal SCKM { get; set; }
        public decimal SCKMH { get; set; }
        public decimal SCKMNH { get; set; }
        public decimal SCEWS { get; set; }
        public decimal SCEWSH { get; set; }
        public decimal SCEWSNH { get; set; }
        public decimal SCGP { get; set; }
        public decimal SCGPH { get; set; }
        public decimal SCGPNH { get; set; }
        public decimal STW { get; set; }
        public decimal STWH { get; set; }
        public decimal STWNH { get; set; }
        public decimal STPD { get; set; }
        public decimal STPDH { get; set; }
        public decimal STPDNH { get; set; }
        public decimal STEXS { get; set; }
        public decimal STEXSH { get; set; }
        public decimal STEXSNH { get; set; }
        public decimal STKM { get; set; }
        public decimal STKMH { get; set; }
        public decimal STKMNH { get; set; }
        public decimal STEWS { get; set; }
        public decimal STEWSH { get; set; }
        public decimal STEWSNH { get; set; }
        public decimal STGP { get; set; }
        public decimal STGPH { get; set; }
        public decimal STGPNH { get; set; }
        public decimal C1W { get; set; }
        public decimal C1WH { get; set; }
        public decimal C1WNH { get; set; }
        public decimal C1PD { get; set; }
        public decimal C1PDH { get; set; }
        public decimal C1PDNH { get; set; }
        public decimal C1EXS { get; set; }
        public decimal C1EXSH { get; set; }
        public decimal C1EXSNH { get; set; }
        public decimal C1KM { get; set; }
        public decimal C1KMH { get; set; }
        public decimal C1KMNH { get; set; }
        public decimal C1EWS { get; set; }
        public decimal C1EWSH { get; set; }
        public decimal C1EWSNH { get; set; }
        public decimal C1GP { get; set; }
        public decimal C1GPH { get; set; }
        public decimal C1GPNH { get; set; }
        public decimal TWOAW { get; set; }
        public decimal TWOAWH { get; set; }
        public decimal TWOAWNH { get; set; }
        public decimal TWOAPD { get; set; }
        public decimal TWOAPDH { get; set; }
        public decimal TWOAPDNH { get; set; }
        public decimal TWOAEXS { get; set; }
        public decimal TWOAEXSH { get; set; }
        public decimal TWOAEXSNH { get; set; }
        public decimal TWOAKM { get; set; }
        public decimal TWOAKMH { get; set; }
        public decimal TWOAKMNH { get; set; }
        public decimal TWOAEWS { get; set; }
        public decimal TWOAEWSH { get; set; }
        public decimal TWOAEWSNH { get; set; }
        public decimal TWOAGP { get; set; }
        public decimal TWOAGPH { get; set; }
        public decimal TWOAGPNH { get; set; }
        public decimal TWOBW { get; set; }
        public decimal TWOBWH { get; set; }
        public decimal TWOBWNH { get; set; }
        public decimal TWOBPD { get; set; }
        public decimal TWOBPDH { get; set; }
        public decimal TWOBPDNH { get; set; }
        public decimal TWOBEXS { get; set; }
        public decimal TWOBEXSH { get; set; }
        public decimal TWOBEXSNH { get; set; }
        public decimal TWOBKMH { get; set; }
        public decimal TWOBKM { get; set; }
        public decimal TWOBKMNH { get; set; }
        public decimal TWOBEWS { get; set; }
        public decimal TWOBEWSH { get; set; }
        public decimal TWOBEWSNH { get; set; }
        public decimal TWOBGP { get; set; }
        public decimal TWOBGPH { get; set; }
        public decimal TWOBGPNH { get; set; }
        public decimal THREEAW { get; set; }
        public decimal THREEAWH { get; set; }
        public decimal THREEAWNH { get; set; }
        public decimal THREEAPD { get; set; }
        public decimal THREEAPDH { get; set; }
        public decimal THREEAPDNH { get; set; }
        public decimal THREEAEXS { get; set; }
        public decimal THREEAEXSH { get; set; }
        public decimal THREEAEXSNH { get; set; }
        public decimal THREEAKM { get; set; }
        public decimal THREEAKMH { get; set; }
        public decimal THREEAKMNH { get; set; }
        public decimal THREEAEWS { get; set; }
        public decimal THREEAEWSH { get; set; }
        public decimal THREEAEWSNH { get; set; }
        public decimal THREEAGP { get; set; }
        public decimal THREEAGPH { get; set; }
        public decimal THREEAGPNH { get; set; }
        public decimal THREEBW { get; set; }
        public decimal THREEBWH { get; set; }
        public decimal THREEBWNH { get; set; }
        public decimal THREEBPD { get; set; }
        public decimal THREEBPDH { get; set; }
        public decimal THREEBPDNH { get; set; }
        public decimal THREEBEXS { get; set; }
        public decimal THREEBEXSH { get; set; }
        public decimal THREEBEXSNH { get; set; }
        public decimal THREEBKM { get; set; }
        public decimal THREEBKMH { get; set; }
        public decimal THREEBKMNH { get; set; }
        public decimal THREEBEWS { get; set; }
        public decimal THREEBEWSH { get; set; }
        public decimal THREEBEWSNH { get; set; }
        public decimal THREEBGP { get; set; }
        public decimal THREEBGPH { get; set; }
        public decimal THREEBGPNH { get; set; }
    }
    public class HyderbadKarnataka
    {
        public decimal Hydrabad { get; set; }
        public decimal NonHydrabad { get; set; }
    }

    public class RemarkSeat
    {
        public int Slno { get; set; }
        public string Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Remark { get; set; }
        public int? FlowId { get; set; }
        public string Remarks { get; set; }
        public string StatusName { get; set; }
    }

    public class summarySeat
    {
        public decimal GMGPH { get; set; }
        public decimal GMWH { get; set; }
        public decimal GMPDH { get; set; }
        public decimal GMEXSH { get; set; }
        public decimal GMKMH { get; set; }
        public decimal GMEWSH { get; set; }
        //SC
        public decimal SCGPH { get; set; }
        public decimal SCWH { get; set; }
        public decimal SCPDH { get; set; }
        public decimal SCEXSH { get; set; }
        public decimal SCKMH { get; set; }
        public decimal SCEWSH { get; set; }
        //ST
        public decimal STGPH { get; set; }
        public decimal STWH { get; set; }
        public decimal STPDH { get; set; }
        public decimal STEXSH { get; set; }
        public decimal STKMH { get; set; }
        public decimal STEWSH { get; set; }
        //C1
        public decimal C1GPH { get; set; }
        public decimal C1WH { get; set; }
        public decimal C1PDH { get; set; }
        public decimal C1EXSH { get; set; }
        public decimal C1KMH { get; set; }
        public decimal C1EWSH { get; set; }
        //2A
        public decimal TWOAGPH { get; set; }
        public decimal TWOAWH { get; set; }
        public decimal TWOAPDH { get; set; }
        public decimal TWOAEXSH { get; set; }
        public decimal TWOAKMH { get; set; }
        public decimal TWOAEWSH { get; set; }
        //2B
        public decimal TWOBGPH { get; set; }
        public decimal TWOBWH { get; set; }
        public decimal TWOBPDH { get; set; }
        public decimal TWOBEXSH { get; set; }
        public decimal TWOBKMH { get; set; }
        public decimal TWOBEWSH { get; set; }
        //3A
        public decimal THREEAGPH { get; set; }
        public decimal THREEAWH { get; set; }
        public decimal THREEAPDH { get; set; }
        public decimal THREEAEXSH { get; set; }
        public decimal THREEAKMH { get; set; }
        public decimal THREEAEWSH { get; set; }
        //3B
        public decimal THREEBGPH { get; set; }
        public decimal THREEBWH { get; set; }
        public decimal THREEBPDH { get; set; }
        public decimal THREEBEXSH { get; set; }
        public decimal THREEBKMH { get; set; }
        public decimal THREEBEWSH { get; set; }
    }
}
