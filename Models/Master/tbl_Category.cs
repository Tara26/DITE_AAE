using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CredatedBy { get; set; }
        public int? ParentCategory { get; set; }
        public string Category_desc { get; set; }
    }
}
