using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Affiliation
{
   public class AffiliationDocuments
    {
        public int Id { get; set; }
        public int? Institute_id { get; set; }

        public int? Trade_Id { get; set; }
        public int? Unit { get; set; }
        public int? Shift { get; set; }
        public string Status { get; set; }
        public int? Flag { get; set; }
        public bool IsActive { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int? Size { get; set; }
        public DateTime? Created_On { get; set; }
        public string AffiliationOrder_Number { get; set; }
        public string Affiliation_date { get; set; }
        public string Trade_Name { get; set; }
    }
}
