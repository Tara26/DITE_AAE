using DLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL
{
    public class ExaminationBLL : IExaminationBLL
    {
        private readonly IExaminationDLL _examDll;
        public ExaminationBLL()
        {
           _examDll = new ExaminationDLL();
        }

        public SelectList GetExamCenterListBLL()
        {
            return _examDll.GetExamCenterListDLL();
        }

        public string GenerateUniqueCodeForTraineeBLL(Examination model)
        {
            return _examDll.GenerateUniqueCodeForTraineeDLL(model);
        }

        public string GetSubjectAndTradeBLL(string ExamDate)
        {
            return _examDll.GetSubjectAndTradeDLL(ExamDate);
        }

        public string GeneratePackingSlipBLL(PackingSlip model)
        {
            return _examDll.GeneratePackingSlipDLL(model);
        }

        public SelectList GetDisticListBLL(int? DivId)
        {
            return _examDll.GetDisticListDLL(DivId);
        }

        public SelectList DisticBasedCentersITIClgBLL(int? distic_Id, int? DivId)
        {
            return _examDll.DisticBasedCentersITIClgDLL(distic_Id, DivId);
        }
    }
}
