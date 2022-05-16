using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_PersonWithDisabilityCategory
    {
        [Key]
        public int PersonWithDisabilityCategoryId { get; set; }
        public string DisabilityName { get; set; }
        public bool IsActive { get; set; } 
    }
}
