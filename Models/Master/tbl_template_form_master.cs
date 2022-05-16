using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_template_form_master
    {
        [Key]
        public int form_id { get; set; }
        public string form_desc { get; set; }
         public string form_pdfpath { get; set; }
         public bool? form_is_active { get; set; }
         public int? form_created_by { get; set; }
         public DateTime? form_creation_datetime { get; set; }
         public int? form_updated_by { get; set; }
         public DateTime? form_updation_datetime { get; set; }
    }
}
