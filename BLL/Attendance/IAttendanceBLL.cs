using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;
using Models.AttendanceDetails;

namespace BLL.Attendance
{
    public  interface IAttendanceBLL
    {
        SelectList GetDivisionListBLL();
        SelectList GetCenterListBLL();
        string ExamAttendanceUploadBLL(AtendanceDet model);
        List<AtendanceDet> getAttendanceDetBLL(int divId);
        //List<AtendanceDet> getAttendanceDtlsBLL();
        List<AtendanceDet> getAttendanceDtlsBLL(AtendanceDet attendanceDet);
        string CreateAttendanceDetailsBLL(List<AtendanceDet> models);
        string UpdateAttendanceDetailsBLL(List<AtendanceDet> models);
        List<AtendanceDet> getModifyAttendanceDetBLL(int divId);
        List<AtendanceDet> getTraineeDtlsByRollNoBLL(int rollNo);
        int? ApproveStatusBLL(AtendanceDet model);
        SelectList GetLoginRoleListBLL();
        List<AtendanceDet> GetAttendanceDtlsByLoginIdBLL(AtendanceDet modal);
        List<AtendanceDet> AttendanceDtlsBLL(AtendanceDet attendanceDet);
        AtendanceDet ViewAttendanceDtlsBLL(AtendanceDet attendanceDet);
        string UpdateCWStatusAttendanceBLL(AtendanceDet attendanceDtls);

    }
}
