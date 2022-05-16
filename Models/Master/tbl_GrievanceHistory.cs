using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_GrievanceHistory
    {
        [Key]
        public int GrievanceHisId { get; set; }
        public int GrievanceId { get; set; }
        public int ApplicationId { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public string DocFileName { get; set; }
        public string DocPath { get; set; }
        public int? DocTypeId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
