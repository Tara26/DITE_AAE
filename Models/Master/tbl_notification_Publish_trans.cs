using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_notification_Publish_trans
    {
        [Key]
        public int np_trans_id { get; set; }

        public string np_module_name { get; set; }

        public int? np_module_id { get; set; }

        public string np_module_path { get; set; }

        public bool? np_is_active { get; set; }

        public string np_creation_datetime { get; set; }

        public int? np_created_by { get; set; }

        public DateTime? np_updatation_datetime { get; set; }

    }

}
