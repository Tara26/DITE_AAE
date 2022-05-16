using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_ApplicantAdmissionRounds
    {
        [Key]
        public int ApplicantAdmissionRoundsId { get; set; }
        public string RoundList { get; set; }
        public bool IsActive { get; set; }
    }
}
