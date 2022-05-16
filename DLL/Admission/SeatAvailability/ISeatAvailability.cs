using Models.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Admission.SeatAvailability
{
    public interface ISeatAvailability
    {
        List<SeatDetails> SeatAvailabilityStatus(int roleId);
        List<UserDetails> GetUserRoles(int id, int level,int roleId);
        List<SeatAvailabilty> GetSeatAvailabilityList(int loginId, int courseCode, string AcademicYear);
        List<SeatAvailabilty> GetSeatAvailabilityListAdd(int loginId,string miscode);
        List<SeatAvailabilty> GetSeatTypes();
        int GetSeatsByTradeIdSeatType(int tradeId); 
        List<SeatAvailabilty> GetSeatsBySeatTypeRules(int seattypeId, int tradeId);
        string SaveSeatAvailability(List<SeatDetails> seat, int loginId, int roleId);       
        bool ForwardSeatAvailability(List<SeatDetails> seat, int loginId, int roleId);        
        List<SeatDetails> GetRemarks(int seatId);
        bool ApproveSeatAvailability(List<SeatDetails> seat, int loginId,int roleId);
        //bool RejectSeatAvailability(List<SeatDetails> seat, int loginId);
        List<SeatAvailabilty> GetSeatViewDetails(int seatId);
        bool GetdelUnitShiftDetails(int seatId, int loginId, int roleId);
        bool GetdeActiveSeatDetails(int seatId,int TradeItiId, int loginId, int roleId);
        bool UpdateSeatAvailability(SeatDetails seat, int loginId, int roleId);
        List<DistrictTalukDetails> GetRegionDistrictCities(int loginId);
        List<DistrictTalukDetails> GetTaluks(int distilgdCOde);
        List<DistrictTalukDetails> GetInstitutes(int distilgdCOde);
        List<SeatAvailabilty> GetSeatAvailabilityListStatusFilter(int TabId, int Course_Id, int Year_id, int roleId, int loginId, int Division_Id, int District_Id, int taluk_id, int Insttype_Id);
        //List<SeatAvailabilty> GetIndexseatavailabilityList();
    }
}
