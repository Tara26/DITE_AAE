using DLL.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using System.Web.Mvc;
using Models;
using Models.AttendanceDetails;

namespace BLL.Attendance
{
    public class AttendanceBLL : IAttendanceBLL
    {
        private readonly IAttendanceDLL _attendDll;
        public AttendanceBLL()
        {
            this._attendDll = new AttendanceDLL();
        }

        public SelectList GetDivisionListBLL()
        {
            return _attendDll.GetDivisionListDLL();
        }
        public SelectList GetCenterListBLL()
        {
            return _attendDll.GetCenterListDLL();
        }

        public string ExamAttendanceUploadBLL(AtendanceDet model)
        {
            return _attendDll.ExamAttendanceUploadDLL(model);
        }

        public List<AtendanceDet> getAttendanceDetBLL(int divId)
        {
            return _attendDll.getAttendanceDetDLL(divId);
        }
        //public List<AtendanceDet> getAttendanceDtlsBLL()
        //{
        //    return _attendDll.getAttendanceDtlsDLL();
        //}

        public List<AtendanceDet> getAttendanceDtlsBLL(AtendanceDet attendanceDet)
        {
            return _attendDll.getAttendanceDtlsDLL(attendanceDet);
        }
        public List<AtendanceDet> AttendanceDtlsBLL(AtendanceDet attendanceDet)
        {
            return _attendDll.AttendanceDtlsDLL(attendanceDet);
        }

        public AtendanceDet ViewAttendanceDtlsBLL(AtendanceDet attendanceDet)
        {
            return _attendDll.ViewAttendanceDtlsDLL(attendanceDet);
        }

        public string CreateAttendanceDetailsBLL(List<AtendanceDet> models)

        {
            return _attendDll.CreateAttendanceDetailsDLL(models);
        }

        public string UpdateAttendanceDetailsBLL(List<AtendanceDet> models)

        {
            return _attendDll.UpdateAttendanceDetailsDLL(models);
        }

        public List<AtendanceDet> getModifyAttendanceDetBLL(int divId)
        {
            return _attendDll.getModifyAttendanceDetDLL(divId);
        }

        public List<AtendanceDet> getTraineeDtlsByRollNoBLL(int rollNo)
        {
            return _attendDll.getTraineeDtlsByRollNoDLL(rollNo);
        }

        public List<AtendanceDet> GetAttendanceDtlsByLoginIdBLL(AtendanceDet modal)
        {
            return this._attendDll.GetAttendanceDtlsByLoginIdDLL(modal);
        }


        public int? ApproveStatusBLL(AtendanceDet model)
        {
            var stdDetails = this._attendDll.GetAttendByID(model);
            //empDetails.exam_notif_status_id = 101;
            stdDetails.exam_notif_status_id = 101;
            stdDetails.updation_datetime = DateTime.Now;
            int? res1 = this._attendDll.UpdateStatus(stdDetails);
            string res = _attendDll.CreateTransAttendenceDLL(stdDetails, model);
            return res1;

        }
        public SelectList GetLoginRoleListBLL()
        {
            return _attendDll.GetLoginRoleListDLL();
        }

        public string UpdateCWStatusAttendanceBLL(AtendanceDet attendanceDtls)
        {
            //var UpdateExamMaster = this._notifDll.GetnotifyByIDMast(notification.Exam_Notif_Id);
            //UpdateExamMaster.login_id = notification.RoleId;
            //UpdateExamMaster.status_id = notification.updatestatusCW;
            //int? a = this._notifDll.UpdateStatusMast(UpdateExamMaster);

                var updateStatus = this._attendDll.Getnotify(attendanceDtls); // CHANGE STATUS FOR ROLE ID
                updateStatus.exam_notif_status_id = attendanceDtls.updatestatusCW;
                int? res1 = this._attendDll.UpdateStatusDLL(updateStatus);

                var updateStatusJD = this._attendDll.GetnotifyLoginID(attendanceDtls);// CHANGE THE STATUS FOR LOGIN ID
                updateStatusJD.exam_notif_status_id = attendanceDtls.updatestatusCW;
                int? res2 = this._attendDll.UpdateStatusDLL(updateStatusJD);
            
            return "Saved";
        }


    }
}
