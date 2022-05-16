using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class MenuLists
    {
        public long Id { get; set; }
        public string MenuName { get; set; }
        public long? ParentMenu { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public int? MenuOrder { get; set; }
    }
}
