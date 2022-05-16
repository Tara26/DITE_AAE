using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission.MasterDataEntry
{
    public class CityDetails
    {
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int DistrictId{ get; set; }
        public string DistrictName{ get; set; }
        public int TalukId { get; set; }
        public string TalukName { get; set; }
    }
}
