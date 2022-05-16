using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class AdmissionApplicantsDistLogin
    {
        public int Session { get; set; }
        public int CourseType { get; set; }
        public int ApplicantType { get; set; }
        public int RoundOption { get; set; }
        public int District { get; set; }
        public int Taluk { get; set; }
        public int ITIInstitute { get; set; }
        public int AdmittedorRejected { get; set; }
        public int Division { get; set; }
    }
}
