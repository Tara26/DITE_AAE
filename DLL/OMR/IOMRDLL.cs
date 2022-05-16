using Models.OMR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DLL.OMR
{
    public interface IOMRDLL
    {
        SelectList GetDivisionListDLL();
        SelectList GetCenterListDLL();
        
        SelectList GetSubjectListDLL();
        string CreateOMRDetailsDLL(List<OMRdtls> models);
        SelectList GetCenterListDLL(int? DivId);
        List<OMRdtls> getSubjectDtlsDLL(int subjectId);
    }
}
