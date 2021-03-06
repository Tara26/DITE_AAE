using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
   public class tbl_SeatMatrix_TradeWise
    {
		[Key]
		public int SeatMaxTradeId { get; set; }
		public int SeatMaxId { get; set; }
		public bool IsActive { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public int TradeId { get; set; }
		public int? GMWH { get; set; }
		public int? GMW { get; set; }
		public int? GMWNH { get; set; }
		public int? GMPD { get; set; }
		public int? GMPDH { get; set; }
		public int? GMPDNH { get; set; }
		public int? GMEXS { get; set; }
		public int? GMEXSH { get; set; }
		public int? GMEXSNH { get; set; }
		public int? GMKM { get; set; }
		public int? GMKMH { get; set; }
		public int? GMKMNH { get; set; }
		public int? GMEWS { get; set; }
		public int? GMEWSH { get; set; }
		public int? GMEWSNH { get; set; }
		public int? GMGP { get; set; }
		public int? GMGPH { get; set; }
		public int? GMGPNH { get; set; }
		public int? SCW { get; set; }
		public int? SCWH { get; set; }
		public int? SCWNH { get; set; }
		public int? SCPD { get; set; }
		public int? SCPDH { get; set; }
		public int? SCPDNH { get; set; }
		public int? SCEXS { get; set; }
		public int? SCEXSH { get; set; }
		public int? SCEXSNH { get; set; }
		public int? SCKM { get; set; }
		public int? SCKMH { get; set; }
		public int? SCKMNH { get; set; }
		public int? SCEWS { get; set; }
		public int? SCEWSH { get; set; }
		public int? SCEWSNH { get; set; }
		public int? SCGP { get; set; }
		public int? SCGPH { get; set; }
		public int? SCGPNH { get; set; }
		public int? STW { get; set; }
		public int? STWH { get; set; }
		public int? STWNH { get; set; }
		public int? STPD { get; set; }
		public int? STPDH { get; set; }
		public int? STPDNH { get; set; }
		public int? STEXS { get; set; }
		public int? STEXSH { get; set; }
		public int? STEXSNH { get; set; }
		public int? STKM { get; set; }
		public int? STKMH { get; set; }
		public int? STKMNH { get; set; }
		public int? STEWS { get; set; }
		public int? STEWSH { get; set; }
		public int? STEWSNH { get; set; }
		public int? STGP { get; set; }
		public int? STGPH { get; set; }
		public int? STGPNH { get; set; }
		public int? C1W { get; set; }
		public int? C1WH { get; set; }
		public int? C1WNH { get; set; }
		public int? C1PD { get; set; }
		public int? C1PDH { get; set; }
		public int? C1PDNH { get; set; }
		public int? C1EXS { get; set; }
		public int? C1EXSH { get; set; }
		public int? C1EXSNH { get; set; }
		public int? C1KM { get; set; }
		public int? C1KMH { get; set; }
		public int? C1KMNH { get; set; }
		public int? C1EWS { get; set; }
		public int? C1EWSH { get; set; }
		public int? C1EWSNH { get; set; }
		public int? C1GP { get; set; }
		public int? C1GPH { get; set; }
		public int? C1GPNH { get; set; }
		public int? TWOAW { get; set; }
		public int? TWOAWH { get; set; }
		public int? TWOAWNH { get; set; }
		public int? TWOAPD { get; set; }
		public int? TWOAPDH { get; set; }
		public int? TWOAPDNH { get; set; }
		public int? TWOAEXS { get; set; }
		public int? TWOAEXSH { get; set; }
		public int? TWOAEXSNH { get; set; }
		public int? TWOAKM { get; set; }
		public int? TWOAKMH { get; set; }
		public int? TWOAKMNH { get; set; }
		public int? TWOAEWS { get; set; }
		public int? TWOAEWSH { get; set; }
		public int? TWOAEWSNH { get; set; }
		public int? TWOAGP { get; set; }
		public int? TWOAGPH { get; set; }
		public int? TWOAGPNH { get; set; }
		public int? TWOBW { get; set; }
		public int? TWOBWH { get; set; }
		public int? TWOBWNH { get; set; }
		public int? TWOBPD { get; set; }
		public int? TWOBPDH { get; set; }
		public int? TWOBPDNH { get; set; }
		public int? TWOBEXS { get; set; }
		public int? TWOBEXSH { get; set; }
		public int? TWOBEXSNH { get; set; }
		public int? TWOBKM { get; set; }
		public int? TWOBKMH { get; set; }
		public int? TWOBKMNH { get; set; }
		public int? TWOBEWS { get; set; }
		public int? TWOBEWSH { get; set; }
		public int? TWOBEWSNH { get; set; }
		public int? TWOBGP { get; set; }
		public int? TWOBGPH { get; set; }
		public int? TWOBGPNH { get; set; }
		public int? THREEAW { get; set; }
		public int? THREEAWH { get; set; }
		public int? THREEAWNH { get; set; }
		public int? THREEAPD { get; set; }
		public int? THREEAPDH { get; set; }
		public int? THREEAPDNH { get; set; }
		public int? THREEAEXS { get; set; }
		public int? THREEAEXSH { get; set; }
		public int? THREEAEXSNH { get; set; }
		public int? THREEAKM { get; set; }
		public int? THREEAKMH { get; set; }
		public int? THREEAKMNH { get; set; }
		public int? THREEAEWS { get; set; }
		public int? THREEAEWSH { get; set; }
		public int? THREEAEWSNH { get; set; }
		public int? THREEAGP { get; set; }
		public int? THREEAGPH { get; set; }
		public int? THREEAGPNH { get; set; }
		public int? THREEBW { get; set; }
		public int? THREEBWH { get; set; }
		public int? THREEBWNH { get; set; }
		public int? THREEBPD { get; set; }
		public int? THREEBPDH { get; set; }
		public int? THREEBPDNH { get; set; }
		public int? THREEBEXS { get; set; }
		public int? THREEBEXSH { get; set; }
		public int? THREEBEXSNH { get; set; }
		public int? THREEBKM { get; set; }
		public int? THREEBKMH { get; set; }
		public int? THREEBKMNH { get; set; }
		public int? THREEBEWS { get; set; }
		public int? THREEBEWSH { get; set; }
		public int? THREEBEWSNH { get; set; }
		public int? THREEBGP { get; set; }
		public int? THREEBGPH { get; set; }
		public int? THREEBGPNH { get; set; }
	}
}
