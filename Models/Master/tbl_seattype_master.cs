using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_seattype_master
    {
        [Key]
        public int Id { get; set; }
        public int seattype_Id { get; set; }
        public int Govt_seats { get; set; }
        public int Management_seats { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }       
    }
}
