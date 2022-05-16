using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_ApplicationFormDescStatus
    {
        [Key]
        public int ApplicationFormDescStatus_id { get; set; }
        public string ApplDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
