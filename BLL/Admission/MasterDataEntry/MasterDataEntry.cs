using DLL.Admission.MasterDataEntry;
using Models.Admission.MasterDataEntry;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Admission.MasterDataEntry
{
    public class MasterDataEntry: IMasterDataEntry
    {
        private readonly IMasterDataEntryDll _masterDll;

        public MasterDataEntry()
        {
            this._masterDll = new MasterDataEntryDll();
        }

        public List<CourseMaster> GetCourseTypes()
        {
            return _masterDll.GetCourseTypes();
        }

        //public DataTable ConvertXSLXtoDataTable(string strFilePath)
        //{
        //    Workbook workbook = new Workbook();
        //    workbook.LoadFromFile(strFilePath);
        //    Worksheet sheet = workbook.Worksheets[0];
        //    DataTable dt = sheet.ExportDataTable();
        //    dt = dt.AsEnumerable().Where(row => !row.ItemArray.All(f => f is null || String.IsNullOrWhiteSpace(f.ToString()))).CopyToDataTable();
        //    return dt;
        //}

        public int UpdateDetails(DataTable dt, int CourseId,int userId)
        {
            var result = _masterDll.UpdateDetails(dt, CourseId, userId);
            return result;
        }
                
        public List<MasterData> GetSeatAvailability()
        {
            var result = _masterDll.GetSeatAvailability();
            return result;
        }
        
        public List<CityDetails> GetDivisionTypes()
        {
            var result = _masterDll.GetDivisionTypes();
            return result;
        }
        public List<CityDetails> GetDistricts(int divisionId)
        {
            var result = _masterDll.GetDistricts(divisionId);
            return result;
        }
        public List<CityDetails> GetTaluks(int districtId)
        {
            var result = _masterDll.GetTaluks(districtId);
            return result;
        }
        public List<MasterData> GetAailableSeats()
        {
            var result = _masterDll.GetAailableSeats();
            return result;
        }
    }
}
