using DLL.Admission;
using Models.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;


namespace BLL.Admission
{
    public class AdmissionSeatMatrixBLL : IAdmissionSeatMatrixBLL
    {
        private readonly IAdmissionSeatMatrixDLL _adDll;
        public AdmissionSeatMatrixBLL()
        {
            this._adDll = new AdmissionSeatMatrixDLL();
        }

        #region ..Seat Matrix..
        //public List<seatmatrixmodel> GetInstituteTypeBll()
        //{
        //    return _adDll.GetInstituteTypeDll();
        //}

        //public List<SelectListItem> GetTalukBLL(int DistId)
        //{
        //    return _adDll.GetTalukDLL(DistId);
        //}
        //public List<seatmatrixmodel> GetviewSeatmatrixBLL(int ApplicantTypeId, int AcademicYear, int Institute, int id)
        //{
        //    return _adDll.GetviewSeatmatrixDLL(ApplicantTypeId, AcademicYear, Institute, id);
        //}
        //public List<seatmatrixmodel> GetGenerateSeatMatrixBLL(int ddlApplicantTypeGen, int ddlAcademicYearGen, int ddlRoundGen, int id)
        //{
        //    return _adDll.GetGenerateSeatMatrixDLL(ddlApplicantTypeGen, ddlAcademicYearGen, ddlRoundGen, id);
        //}
        //public List<seatmatrixmodel> GetSummarySeatMatrixBLL(int id)
        //{
        //    return _adDll.GetSummarySeatMatrixDLL(id);
        //}

        public List<Tradewideseat> GetGenerateSeatMatrixBLLNested(int round, int instiId, int year, int tradeId)
        {
            return _adDll.GetGenerateSeatMatrixDLLNested(round, instiId, year, tradeId);
        }

        public List<seatmatrixmodelNested> GetGenerateSeatMatrixBLLNested(int CollegeID)
        {
            return _adDll.GetGenerateSeatMatrixDLLNested(CollegeID);
        }

        public string InsertSeatMatrixBLL(seatmatrixmodelNested s)
        {
            return _adDll.InsertSeatMatrixDLL(s);
        }
        #endregion

        #region ..Seat Matrix..Added by Sujit
        public List<seatmatrixmodel> GetInstituteTypeBll()
        {
            return _adDll.GetInstituteTypeDll();
        }

        public List<SelectListItem> GetTalukBLL(int DistId)
        {
            return _adDll.GetTalukDLL(DistId);
        }
        public List<seatmatrixmodel> GetviewSeatmatrixBLL(int ApplicantTypeId, int AcademicYear, int Institute, int id)
        {
            return _adDll.GetviewSeatmatrixDLL(ApplicantTypeId, AcademicYear, Institute, id);
        }
        public List<seatmatrixmodel> GetGenerateSeatMatrixBLL(int ApplicantTypeGen, int AcademicYearGen, int RoundGen, int roleid)
        {
            return _adDll.GetGenerateSeatMatrixDLL(ApplicantTypeGen, AcademicYearGen, RoundGen, roleid);
        }
        public List<DivisionModel> GetDivisions()
        {
            return _adDll.GetDivisions();
        }
        public List<DivisionListModel> GenerateSeaMatrix(int year, int courseType, int round,int loginId)
        {
            return _adDll.GenerateSeaMatrix(year, courseType, round, loginId);
        }
        public bool SubmitSeatMatrixCollegeWise(List<Tradewideseat> listItem,int collegeId, int round, int year, int appType, int courseType, int loginId, int roleId)
        {
            return _adDll.SubmitSeatMatrixCollegeWise(listItem,collegeId, round, year, appType, courseType, loginId, roleId);
        }
        public bool SubmitSeatMatrix(int year, int appType, int courseType, int round, int role, int loginId, int roleId,string remark)
        {
            return _adDll.SubmitSeatMatrix(year, appType, courseType, round, role, loginId, roleId, remark);
        }
        public List<DivisionUpdateModel> GetDivisionsInstitutesTrades(int year, int round, int applicantType,int courseId)
        {
            return _adDll.GetDivisionsInstitutesTrades(year, round, applicantType, courseId);
        }
        public bool UpdateSeatMatrixCollegeWise(List<Tradewideseat> listItem, int collegeId, int round, int year, int appType, int courseType, int loginId, int roleId)
        {
            return _adDll.UpdateSeatMatrixCollegeWise(listItem, collegeId, round, year, appType, courseType, loginId, roleId);
        }
        public bool UpdateSeatMatrix(int year, int appType, int courseType, int round, int role, int loginId, int roleId)
        {
            return _adDll.UpdateSeatMatrix(year, appType, courseType, round, role, loginId, roleId);
        }
        public bool ForwardSendBackApproveSeatMatrix(int round,int year,string remarks,int courseType,int Status, int loginId,int roleId,int role)
        {
            return _adDll.ForwardSendBackApproveSeatMatrix(round, year, remarks, courseType, Status, loginId, roleId, role);
        }

        public List<RemarkSeat> GetSeatmatrixRemarks(int year, int round, int applId,int courseType)
        {
            return _adDll.GetSeatmatrixRemarks(year,round, applId, courseType);
        }
        public int GetSeatmatrixNotification()
        {
            return _adDll.GetSeatmatrixNotification();
        }
        public List<seatmatrixmodel> GetGenSeatMatrixBLL(int ApplicantTypeGen, int AcademicYearGen, int RoundGen, int roleid)
        {
            return _adDll.GetGenSeatMatrixDLL(ApplicantTypeGen, AcademicYearGen, RoundGen, roleid);
        }
        public List<seatmatrixmodel> GetSummarySeatMatrixBLL(int id)
        {
            return _adDll.GetSummarySeatMatrixDLL(id);
        }
        public List<summarySeat> GetCheckSummaryBLL(int id, int AcademicYearId, int ApplicantTypeId, int InstituteId, int Round, int DistrictId, int DivisionId, int TalukId)
        {
            return _adDll.GetCheckSummaryDLL(id, AcademicYearId, ApplicantTypeId, InstituteId, Round, DistrictId, DivisionId, TalukId);
        }
        public List<seatmatrixmodel> GetReviewSeatMatrixBLL(int id, int AcademicYearId)
        {
            return _adDll.GetReviewSeatMatrixDLL(id, AcademicYearId);
        }
        public List<seatmatrixmodel> GetUpdateSeatMatrixBLL(int id, int AcademicYearId)
        {
            return _adDll.GetUpdateSeatMatrixDLL(id, AcademicYearId);
        }
        public List<seatmatrixmodel> GetApproveSeatMatrixBLL(int id, int AcademicYearId)
        {
            return _adDll.GetApproveSeatMatrixDLL(id, AcademicYearId);
        }
          public List<seatmatrixmodel> GetViewSeatMatrixGridBLL(int id, int ApplicantTypeId, int AcademicYearId, int Round, int DistrictId, int DivisionId, int TalukId, int InstituteId)
        {
            return _adDll.GetViewSeatMatrixGridDLL(id,ApplicantTypeId, AcademicYearId,Round, DistrictId, DivisionId, TalukId, InstituteId);
        }
        public List<seatmatrixmodel> GetViewSeatMatrix( int ApplicantTypeId, int AcademicYearId, int Round, int loginId)
        {
            return _adDll.GetViewSeatMatrix( ApplicantTypeId, AcademicYearId, Round, loginId);
        }
        public List<seatmatrixmodel> GetUpdateTradeSeatMatrixBLL(int id,int SeatMaxId)
        {
            return _adDll.GetUpdateTradeSeatMatrixDLL(id, SeatMaxId);
        }
        public List<seatmatrixmodel> GetReviewTradeSeatMatrixIdBLL(int id,int SeatMaxId)
        {
            return _adDll.GetReviewTradeSeatMatrixIdDLL(id, SeatMaxId);
        }
        public List<seatmatrixmodel> GetAproveTradeSeatMatrixIdBLL(int id,int SeatMaxId)
        {
            return _adDll.GetAproveTradeSeatMatrixIdDLL(id, SeatMaxId);
        }
        public List<seatmatrixmodel> GetStatusBLL(int id)
        {
            return _adDll.GetStatusDLL(id);
        }
        //public List<seatmatrixmodelNested> GetGenerateSeatMatrixBLLNested()
        //{
        //    return _adDll.GetGenerateSeatMatrixDLLNested();
        //}
        #endregion
             
    }
}
