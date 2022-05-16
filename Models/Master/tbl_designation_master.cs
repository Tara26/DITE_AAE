using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_designation_master
    {
        [Key]
        public int Designation_Id { get; set; }
        public string Designation { get; set; }
        
        public DateTime Created_on { get; set; }
    }
}
