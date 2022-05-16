using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_GrievanceDoc
    {
        [Key]
        public int GrievanceCatId { get; set; }
        public int GrievanceId { get; set; }
        public string DocPath { get; set; }
        public int CatTypeId { get; set; }
        public string DocFileName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
    }
}
