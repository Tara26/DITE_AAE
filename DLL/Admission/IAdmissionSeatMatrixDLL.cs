using Models.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DLL.Admission
{
    public interface IAdmissionSeatMatrixDLL
    {
        #region ..Seat Matrix..
        //List<seatmatrixmodel> GetInstituteTypeDll();

        //List<SelectListItem> GetTalukDLL(int DistId);
        //List<seatmatrixmodel> GetviewSeatmatrixDLL(int ApplicantTypeId, int AcademicYear, int Institute, int id);
        //List<seatmatrixmodel> GetGenerateSeatMatrixDLL(int ddlApplicantTypeGen, int ddlAcademicYearGen, int ddlRoundGen, int id);
        //List<seatmatrixmodel> GetSummarySeatMatrixDLL(int id);

        List<Tradewideseat> GetGenerateSeatMatrixDLLNested(int round, int instiId, int year, int tradeId);

        List<seatmatrixmodelNested> GetGenerateSeatMatrixDLLNested(int CollegeID);

        string InsertSeatMatrixDLL(seatmatrixmodelNested s);
        #endregion

        #region ..Seat Matrix..Added by Sujit
        List<seatmatrixmodel> GetInstituteTypeDll();
        //List<AdmissionMeritList> GetDivisionSeatM();
        List<SelectListItem> GetTalukDLL(int DistId);
        List<seatmatrixmodel> GetviewSeatmatrixDLL(int ApplicantTypeId, int AcademicYear, int Institute, int id);
        List<seatmatrixmodel> GetGenerateSeatMatrixDLL(int ApplicantTypeGen, int AcademicYearGen, int RoundGen, int roleid);
        List<DivisionModel> GetDivisions();
        
        
        
        List<seatmatrixmodel> GetGenSeatMatrixDLL(int ApplicantTypeGen, int AcademicYearGen, int RoundGen, int roleid);
        List<seatmatrixmodel> GetSummarySeatMatrixDLL(int id);
        List<summarySeat> GetCheckSummaryDLL(int id, int AcademicYearId, int ApplicantTypeId, int InstituteId, int Round, int DistrictId, int DivisionId, int TalukId);
        List<seatmatrixmodel> GetReviewSeatMatrixDLL(int id, int AcademicYearId);
        List<seatmatrixmodel> GetUpdateSeatMatrixDLL(int id, int AcademicYearId);
        List<seatmatrixmodel> GetApproveSeatMatrixDLL(int id, int AcademicYearId);
        List<seatmatrixmodel> GetViewSeatMatrixGridDLL(int id, int ApplicantTypeId, int AcademicYearId, int Round, int DistrictId, int DivisionId, int TalukId, int InstituteId);
        List<seatmatrixmodel> GetViewSeatMatrix( int ApplicantTypeId, int AcademicYearId, int Round, int loginId);
        List<seatmatrixmodel> GetUpdateTradeSeatMatrixDLL(int id,int SeatMaxId);
        List<seatmatrixmodel> GetReviewTradeSeatMatrixIdDLL(int id,int SeatMaxId);
        List<seatmatrixmodel> GetAproveTradeSeatMatrixIdDLL(int id,int SeatMaxId);
        List<seatmatrixmodel> GetStatusDLL(int id);
        //List<seatmatrixmodelNested> GetGenerateSeatMatrixDLLNested();
        #endregion

        #region by dhanraj
        List<DivisionListModel> GenerateSeaMatrix(int year, int courseType, int round,int loginId);
        bool SubmitSeatMatrixCollegeWise(List<Tradewideseat> listItem,int collegeId, int round, int year, int appType, int courseType, int loginId, int roleId);
        bool UpdateSeatMatrixCollegeWise(List<Tradewideseat> listItem, int collegeId, int round, int year, int appType, int courseType, int loginId, int roleId);
        bool UpdateSeatMatrix(int year, int appType, int courseType, int round, int role, int loginId, int roleId);
        bool ForwardSendBackApproveSeatMatrix(int round,int  year, string remarks,int  courseType,int  Status,int  loginId,int roleId, int role);
        List<RemarkSeat> GetSeatmatrixRemarks(int year, int round, int applId, int courseType);
        int GetSeatmatrixNotification();
        bool SubmitSeatMatrix(int year, int appType, int courseType, int round, int role, int loginId, int roleId,string remark);
        List<DivisionUpdateModel> GetDivisionsInstitutesTrades(int year, int round, int applicantType, int courseId);
        #endregion
    }
}
