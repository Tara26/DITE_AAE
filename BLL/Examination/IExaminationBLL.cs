using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL
{
    public interface IExaminationBLL
    {
        SelectList GetExamCenterListBLL();

        string GenerateUniqueCodeForTraineeBLL(Examination model);
        string GetSubjectAndTradeBLL(string ExamDate);
        string GeneratePackingSlipBLL(PackingSlip model);
        SelectList GetDisticListBLL(int? DivId);
        SelectList DisticBasedCentersITIClgBLL(int? distic_Id, int? DivId);
    }
}
