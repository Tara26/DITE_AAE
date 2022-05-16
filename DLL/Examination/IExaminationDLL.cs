using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DLL
{
    public interface IExaminationDLL
    {
        SelectList GetExamCenterListDLL();

        string GenerateUniqueCodeForTraineeDLL(Examination model);
        string GetSubjectAndTradeDLL(string ExamDate);
        string GeneratePackingSlipDLL(PackingSlip model);
        SelectList GetDisticListDLL(int? DivId);
        SelectList DisticBasedCentersITIClgDLL(int? distic_Id, int? DivId);
    }
}
