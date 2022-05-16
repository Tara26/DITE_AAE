using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_admission_notif_status_mast
    {
        [Key]
        public int statusId{ get; set; }
        public string statusDesc { get; set; }
        public bool isActive { get; set; }
        public DateTime createdOn { get; set; }
        public int createdBy { get; set; }
    }
}
