using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Religion
    {
        [Key]
        public int Religion_Id { get; set; }
        public string Religion { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CredatedBy { get; set; }
    }
}
