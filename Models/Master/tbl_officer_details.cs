using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models.Master
{
    public class tbl_officer_details
    {
        [Key]
        public int officer_id { get; set; }
        public string designation_title { get; set; }
        public bool is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_on { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_on { get; set; }
        public SelectList Officer_List { get; set; }
    }
}
