using Models.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL.Admission
{
    public interface IAdmissionSeatMatrixBLL
    {

        #region ..SeatMatrix..
        //List<seatmatrixmodel> GetInstituteTypeBll();
        //List<AdmissionMeritList> GetDivisionSeatM();
        //List<SelectListItem> GetTalukBLL(int DistId);
        //List<seatmatrixmodel> GetviewSeatmatrixBLL(int ApplicantTypeId, int AcademicYear, int Institute, int id);
        
        List<seatmatrixmodel> GetGenSeatMatrixBLL(int ApplicantTypeGen, int AcademicYearGen, int RoundGen, int roleid);
        List<seatmatrixmodel> GetGenerateSeatMatrixBLL(int ApplicantTypeGen, int AcademicYearGen, int RoundGen, int roleid);
        //List<seatmatrixmodel> GetSummarySeatMatrixBLL(int id);

        List<Tradewideseat> GetGenerateSeatMatrixBLLNested(int round, int instiId, int year, int tradeId);
        List<seatmatrixmodelNested> GetGenerateSeatMatrixBLLNested(int CollegeID);

        string InsertSeatMatrixBLL(seatmatrixmodelNested s);
        #endregion

        #region ..SeatMatrix..Added by Sujit
        List<seatmatrixmodel> GetInstituteTypeBll();
        //List<AdmissionMeritList> GetDivisionSeatM();
        List<SelectListItem> GetTalukBLL(int DistId);
        List<seatmatrixmodel> GetviewSeatmatrixBLL(int ApplicantTypeId, int AcademicYear, int Institute, int id);
        //List<seatmatrixmodel> GetGenerateSeatMatrixBLL(int ddlApplicantTypeGen, int ddlAcademicYearGen, int ddlRoundGen, int id);
        List<seatmatrixmodel> GetSummarySeatMatrixBLL(int id);
        List<summarySeat> GetCheckSummaryBLL(int id, int AcademicYearId, int ApplicantTypeId, int InstituteId, int Round, int DistrictId, int DivisionId, int TalukId);
        List<seatmatrixmodel> GetReviewSeatMatrixBLL(int id, int AcademicYearId);
        List<seatmatrixmodel> GetUpdateSeatMatrixBLL(int id, int AcademicYearId);
        List<seatmatrixmodel> GetApproveSeatMatrixBLL(int id, int AcademicYearId);
        List<seatmatrixmodel> GetViewSeatMatrixGridBLL(int id, int ApplicantTypeId, int AcademicYearId, int Round, int DistrictId, int DivisionId, int TalukId, int InstituteId);
        List<seatmatrixmodel> GetViewSeatMatrix( int ApplicantTypeId, int AcademicYearId, int Round, int loginId);
        List<seatmatrixmodel> GetUpdateTradeSeatMatrixBLL(int id, int SeatMaxId);
        List<seatmatrixmodel> GetReviewTradeSeatMatrixIdBLL(int id,int SeatMaxId);
        List<seatmatrixmodel> GetAproveTradeSeatMatrixIdBLL(int id,int SeatMaxId);

        List<seatmatrixmodel> GetStatusBLL(int id);
        //List<seatmatrixmodel> GetGenerateSeatMatrixBLL(int ddlApplicantTypeGen, int ddlAcademicYearGen, int ddlRoundGen, int id);
        //List<seatmatrixmodelNested> GetGenerateSeatMatrixBLLNested();
        #endregion

        #region by dhanraj
        List<DivisionModel> GetDivisions();
        bool SubmitSeatMatrix(int year, int appType, int courseType, int round, int role, int loginId, int roleId,string remark);
        List<DivisionUpdateModel> GetDivisionsInstitutesTrades(int year, int round, int applicantType, int courseId);

        List<DivisionListModel> GenerateSeaMatrix(int year, int courseType, int round,int loginId);
        bool SubmitSeatMatrixCollegeWise(List<Tradewideseat> listItem, int collegeId, int round, int year, int appType, int  courseType, int  loginId, int roleId);
        bool UpdateSeatMatrixCollegeWise(List<Tradewideseat> listItem, int collegeId, int round, int year, int appType, int courseType, int loginId, int roleId);
        bool UpdateSeatMatrix(int year, int appType, int courseType, int round, int role, int loginId, int roleId);
        bool ForwardSendBackApproveSeatMatrix(int round,int year,string remarks,int courseType,int Status,int loginId,int roleId, int role);
        List<RemarkSeat> GetSeatmatrixRemarks(int year, int round, int applId, int courseType);
        int GetSeatmatrixNotification();
        #endregion
    }
}
