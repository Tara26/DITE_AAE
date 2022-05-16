using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class tbl_Panchayat
    {
        [Key]
        public int PanchayatId { get; set; }
        public string Panchayat { get; set; }
        public int TalukCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
