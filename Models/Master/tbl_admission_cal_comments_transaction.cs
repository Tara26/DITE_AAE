using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_admission_cal_comments_transaction
    {
        [Key]
        public int comments_transaction_id { get; set; }
        public string comments_transaction_desc { get; set; }
        public int notification_id { get; set; }
        public int module_id { get; set; }
        public int status_id { get; set; }
        public int login_id { get; set; }
        public bool ct_is_active { get; set; }
        public int ct_created_by { get; set; }
        public int is_published { get; set; }

        public Nullable<DateTime> ct_created_on { get; set; }
        public int ct_updated_by { get; set; }
        public Nullable<DateTime> ct_updated_on { get; set; }
    }
}
