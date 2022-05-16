using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;
using Models.AttendanceDetails;

namespace DLL.Attendance
{
    public interface IAttendanceDLL
    {
        SelectList GetDivisionListDLL();
        SelectList GetCenterListDLL();
        string ExamAttendanceUploadDLL(AtendanceDet model);
        List<AtendanceDet> getAttendanceDetDLL(int id);
        string CreateAttendanceDetailsDLL(List<AtendanceDet> models);
        string UpdateAttendanceDetailsDLL(List<AtendanceDet> models);
        List<AtendanceDet> getModifyAttendanceDetDLL(int divId);
		List<OMRSheetDetails> GetOMREntryDLL(int id);
		 //List<AtendanceDet> getAttendanceDtlsDLL();
        List<AtendanceDet> getAttendanceDtlsDLL(AtendanceDet attendanceDet);
        List<AtendanceDet> getTraineeDtlsByRollNoDLL(int rollNo);
       // int? ApproveStatusDLL(AtendanceDet model);
        string CreateTransAttendenceDLL(tbl_attendance_details_trans model, AtendanceDet role);
       // string CreateTransAttendenceDLL(AtendanceDet model);
        tbl_attendance_details_trans GetAttendByID(AtendanceDet model);
        int? UpdateEmp(tbl_attendance_details emp);
        SelectList GetLoginRoleListDLL();
        List<AtendanceDet> GetAttendanceDtlsByLoginIdDLL(AtendanceDet modal);
        List<AtendanceDet> AttendanceDtlsDLL(AtendanceDet attendanceDet);
        AtendanceDet ViewAttendanceDtlsDLL(AtendanceDet attendanceDet);
        //string UpdateCWStatusAttendanceDLL(AtendanceDet attendanceDtls);
        tbl_attendance_details_trans Getnotify(AtendanceDet attendanceDtls);
        tbl_attendance_details_trans GetnotifyLoginID(AtendanceDet attendanceDtls);
        int UpdateStatusDLL(tbl_attendance_details_trans obj);
        int? UpdateStatus(tbl_attendance_details_trans emp);
    }
}
