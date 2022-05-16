using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Document_Applicant
    {
        [Key]
        public int DocAppId { get; set; }
        public int ApplicantId { get; set; }
        public int DocumentTypeId { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Remarks { get; set; }
        public int Verified { get; set; }
        public int DocumentSet { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
