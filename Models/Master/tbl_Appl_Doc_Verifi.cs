using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Appl_Doc_Verifi
    {
        [Key]
        public int ApplDocVerifiID { get; set; }
        public string VerificationStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
