using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_status_master
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public bool IsActive { get; set; }
    }
}
