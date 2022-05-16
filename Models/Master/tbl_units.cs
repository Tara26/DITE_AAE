using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_units
    {
        [Key]
        public int u_id { get; set; }
        public string units { get; set; }

        public bool? u_is_active { get; set; }
        public Nullable<int> u_created_by { get; set; }
        public Nullable<DateTime> u_creation_datetime { get; set; }
        public Nullable<int> u_updated_by { get; set; }
        public Nullable<DateTime> u_updation_datetime { get; set; }
       

    }
}
