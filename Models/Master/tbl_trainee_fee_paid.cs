using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_trainee_fee_paid
    {
        [Key]
        public int tfp_id { get; set; }
        public int trainee_id { get; set; }
        public bool? fee_paid_status { get; set; }

        public bool? is_active { get; set; }
        public Nullable<DateTime> creation_datetime { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public Nullable<int> updated_by { get; set; }
    }

    public class tbl_feepaid_receipt
    {
        [Key]
        public int id { get; set; }
        public int trainee_id { get; set; }
        public string receipt_number { get; set; }
        public bool is_active { get; set; }
        public DateTime receipt_issued_date { get; set; }
    }
}
