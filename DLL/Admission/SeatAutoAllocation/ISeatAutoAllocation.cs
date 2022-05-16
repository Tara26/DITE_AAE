using Models.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Admission.SeatAutoAllocation
{
    public interface ISeatAutoAllocation
    {
        #region Generate Seat Auto allocation
        List<SeatAutoAllocationModel> Round1GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round, int RoleId, int LoginID);
        List<SeatAutoAllocationModel> Round2GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round, int RoleId);
        List<SeatAutoAllocationModel> Round3_4GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round, int RoleId);
        List<SeatAutoAllocationModel> Round5GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round, int RoleId);
        List<SeatAutoAllocationModel> Round6GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round, int RoleId);
        string DirectAdmissionSeatAllotmentDLL(InstituteWiseAdmission model);
        bool ForwardSeatAutoAllotment(List<int> allocationId, int loginId, int roleId, string Remarks, int Status);
        #endregion

        #region update seat auto allocation
        List<SeatAutoAllocationModel> GetGeneratedSeatAutoAllotmentList(int courseType, int applicantType, int academicYear, int round);
        List<SeatAutoAllocationModel> ViewSeatAutoAllotment(int allocationId);
        #endregion

        #region Seat Allocation Review & Recommand Of Seat Matrix
        //ram Use Case-35 & Screen 55 &56
        List<ReviewSeatAllocated> GenerateSeatAllotmentReviewDLL(int ddlCourseTypesGen, int ddlApplicantTypeGen, int ddlAcademicYearGen, int ddlRoundGen, int id);

        List<SeatMatrixAllocationDetail> GeneratedSeatAllotmentReviewListDLL(int id);

        List<SeatAutoAllocationModel> GetSeatMatrixViewListDLL(int courseType, int applicantType, int academicYear, int round);

        bool ForwardSeatAutoAllotmentReviewDLL(List<int> allocationId, int loginId, int roleId);

        List<SeatMatrixAllocationDetail> GetCommentsListSeatAllocationDLL(int SeatAllocationId);
        string UpdtApplITIInstDetailsDLL(int ApplicantId, int LoginID,int roleId, int? allocId);

        #endregion
    }
}
