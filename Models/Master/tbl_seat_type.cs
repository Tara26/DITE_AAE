using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_seat_type
    {

        [Key]
        public int Seat_type_id { get; set; }
        public string SeatType { get; set; }

        public bool? IsActive { get; set; }      
        public Nullable<DateTime> CreatedOn { get; set; }
    
    }
}
