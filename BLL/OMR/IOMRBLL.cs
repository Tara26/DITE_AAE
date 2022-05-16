using Models.OMR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL.OMR
{
    public  interface IOMRBLL
    {
        SelectList GetDivisionListBLL();
        SelectList GetCenterListBLL();
        SelectList GetSubjectListBLL();
        List<OMRdtls> getSubjectDtlsBLL(int subjectId);
        SelectList GetCenterListBLL(int? DivId);
        string CreateOMRDetailsBLL(List<OMRdtls> models);
    }
}
