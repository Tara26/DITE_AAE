using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class MenuRoles
    {
        [Key]
        public long Id { get; set; }
        public long MenuId { get; set; }
        public int UserMap_Id { get; set; }
        public DateTime? Created_On { get; set; }
        public int? Created_By { get; set; }
        public bool IsActive { get; set; }
    }
}
