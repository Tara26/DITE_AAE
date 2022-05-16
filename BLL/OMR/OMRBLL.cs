using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.OMR;
using Models.OMR;
using System.Web.Mvc;

namespace BLL.OMR
{
    public class OMRBLL :IOMRBLL
    {
        private readonly IOMRDLL _omrDll;
        public OMRBLL()
        {
            this._omrDll = new OMRDLL();
        }

        public SelectList GetDivisionListBLL()
        {
            return _omrDll.GetDivisionListDLL();
        }
        public SelectList GetCenterListBLL()
        {
            return _omrDll.GetCenterListDLL();

        }

        public SelectList GetSubjectListBLL()
        {
            return _omrDll.GetSubjectListDLL();
        }

        public string CreateOMRDetailsBLL(List<OMRdtls> models)
        {
            return _omrDll.CreateOMRDetailsDLL(models);
        }
        public List<OMRdtls> getSubjectDtlsBLL(int subjectId)
        {
            return _omrDll.getSubjectDtlsDLL(subjectId);
        }

        public SelectList GetCenterListBLL(int? DivId)
        {
            return _omrDll.GetCenterListDLL(DivId);
        }
    }
}
