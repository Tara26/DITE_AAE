using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_ITI_Trade_Shift
    {
        [Key]
        public int ITI_Trade_Shift_Id { get; set; }
        public int ITI_Trade_Id { get; set; }
        public int Units { get; set; }
        public int Shift { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string IsPPP { get; set; }
        public string Dual_System { get; set; }
        public int? Status { get; set; }
    }
}
