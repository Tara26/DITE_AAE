using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_reservation
    {
        [Key]
        public int ReservationId { get; set; }
        public string Reservations { get; set; }
        public int IsActive { get; set; }
    }
}
