using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_GradationRank_TransHistory
    {
        [Key]
        public int Gradation_transHis_Id { get; set; }
        public int? Gradation_trans_Id { get; set; }
        public int ApplicantId { get; set; }
        public DateTime TransDate { get; set; }
        public int Rank { get; set; }
        public bool Tentative { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public int FlowId { get; set; }
        public bool Final { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }
}
