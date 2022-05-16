using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_GradationType
    {
        [Key]
        public int GradationTypeId { get; set; }
        public string GradationType { get; set; }     
        public bool IsActive { get; set; }     
        public DateTime CreatedOn { get; set; }
        public int CredatedBy { get; set; }
    }
}
