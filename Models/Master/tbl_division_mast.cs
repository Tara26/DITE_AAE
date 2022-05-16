using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_division_mast
    {
        [Key]
        public int division_id { get; set; }
        public string division_name { get; set; }
        public bool? is_active { get; set; }
        public int? created_by { get; set; }
        public DateTime? creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updation_datetime { get; set; }
    }
}
