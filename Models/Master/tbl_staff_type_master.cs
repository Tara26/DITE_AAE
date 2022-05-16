using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_staff_type_master
    {
        [Key]
        public int Staff_type_id { get; set; }
        public string Staff_Type { get; set; }
        public bool isactive { get; set; }
        public DateTime Created_on { get; set; }
    }
}
