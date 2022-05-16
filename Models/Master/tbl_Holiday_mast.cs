using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Holiday_mast
    {
        [Key]
        public int h_id { get; set; }
        public DateTime h_date { get; set; }
        public string h_day { get; set; }
        public string h_name { get; set; }
        public bool h_is_active { get; set; }
        public DateTime h_creation_datetime { get; set; }

    }
}
