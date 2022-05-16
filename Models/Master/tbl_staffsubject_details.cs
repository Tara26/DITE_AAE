using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_staffsubject_details
    {
        [Key]
        public int Id { get; set; }
        public int Staff_Id { get; set; }
        public int Subject_Id { get; set; }
        public DateTime Created_On { get; set; }
        public bool IsActive { get; set; }

    }
}
