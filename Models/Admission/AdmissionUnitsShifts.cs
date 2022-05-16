using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class AdmissionUnitsShifts
    {

        public int s_id { get; set; }
        public string shifts { get; set; }
        public int u_id { get; set; }
        public string units { get; set; }
        public bool? is_active { get; set; }
        public int? created_by { get; set; }
        public DateTime? creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updation_datetime { get; set; }
    }
}
