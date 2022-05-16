using Models.Admission.MasterDataEntry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Admission.MasterDataEntry
{
    public interface IMasterDataEntryDll
    {
        int UpdateDetails(DataTable dt, int CourseId,int userId);       
        List<CourseMaster> GetCourseTypes();
        List<MasterData> GetSeatAvailability();
        List<CityDetails> GetDivisionTypes();
        List<CityDetails> GetDistricts(int divisionId);
        List<CityDetails> GetTaluks(int districtId);
        List<MasterData> GetAailableSeats();
    }
}
