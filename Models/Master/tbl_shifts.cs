using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_shifts
    {
        [Key]
        public int s_id { get; set; }
        public string shifts { get; set; }

        public bool? s_is_active { get; set; }
        public Nullable<DateTime> s_creation_datetime { get; set; }
        public Nullable<int> s_created_by { get; set; }
        public Nullable<DateTime> s_updation_datetime { get; set; }
        public Nullable<int> s_updated_by { get; set; }
    }
}
