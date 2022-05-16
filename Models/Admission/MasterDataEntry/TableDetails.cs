using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission.MasterDataEntry
{
    public class TableDetails
    {
        public string FileName { get; set; }
        public DataTable Table { get; set; }
        public DataTable Table1 { get; set; }
        public int ScreenId { get; set; }
    }
}
