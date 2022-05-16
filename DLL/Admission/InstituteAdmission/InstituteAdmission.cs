using DLL.DBConnection;
using Models.Admission;
using Models.Master;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.Globalization;
using Spire.Pdf;
using Spire.Pdf.HtmlConverter;
using Spire.Pdf.Graphics;
using System.Transactions;

namespace DLL.Admission.InstituteAdmission
{
    public class InstituteAdmission : IInstituteAdmission
    {
        private readonly DbConnection _db = new DbConnection();

        public List<InstituteWiseAdmission> GetDataAdmissionApplicantsDLL(int Session, int CourseType, int ApplicantType, int RoundOption, int AdmittedorRejected, int LoginId, int roleId, int ApplicatoinMode)
        {
            try
            {
                int allocId = _db.tbl_SeatAllocation_SeatMatrix.Where(a => (Session != 0 ? a.AcademicYear == Session : true) && (CourseType != 0 ? a.CourseTypeId == CourseType : true) && (ApplicantType != 0 ? a.ApplicantType == ApplicantType : true) && (RoundOption !=0 ? a.Round == RoundOption : true))
                    .Select(a => a.AllocationId).Take(1).FirstOrDefault();
                int collegeId = 0;
                int officerId = 0;

                if (roleId == (int)CmnClass.Role.AdmOff)
                {
                    collegeId = _db.tbl_VerificationOfficer_Master.Where(a => a.UserMasterId == LoginId).Select(a => a.InstituteId).Take(1).FirstOrDefault();
                    officerId = _db.tbl_VerificationOfficer_Master.Where(a => a.UserMasterId == LoginId).Select(a => a.Officer_Id).Take(1).FirstOrDefault();
                }
                if (collegeId == 0 && allocId != 0)
                {
                    collegeId = _db.Staff_Institute_Detail.Where(a => a.UserId == LoginId).Select(a => a.InstituteId).Take(1).FirstOrDefault();
                }
                var res = (from taid in _db.tbl_Applicant_ITI_Institute_Detail
                           join tsadm in _db.tbl_SeatAllocationDetail_Seatmatrix on taid.ApplicationId equals tsadm.ApplicantId
                           join tad in _db.tbl_Applicant_Detail on taid.ApplicationId equals tad.ApplicationId
                           join tcd in _db.tbl_iti_college_details on tsadm.InstituteId equals tcd.iti_college_id
                           join tdm in _db.tbl_division_master on tcd.division_id equals tdm.division_id
                           //join tsm in _db.tbl_AdmissionatInstituteStatus on taid.AdmittedStatus equals tsm.StatusId
                           join tsms in _db.tbl_AdmissionatInstituteStatus on taid.AdmittedStatus equals tsms.StatusId
                           join tit in _db.tbl_Institute_type on tcd.Insitute_TypeId equals tit.Institute_type_id
                           join sid in _db.Staff_Institute_Detail on tsadm.InstituteId equals sid.InstituteId
                           join sasm in _db.tbl_SeatAllocation_SeatMatrix on tsadm.AllocationId equals sasm.AllocationId
                           join tdim in _db.tbl_district_master on tcd.district_id equals tdim.district_lgd_code
                           join ttm in _db.tbl_taluk_master on tcd.taluk_id equals ttm.taluk_lgd_code
                           join tr in _db.tbl_Religion on tad.Religion equals tr.Religion_Id
                           join tc in _db.tbl_Category on tad.Category equals tc.CategoryId
                           join th in _db.Tbl_horizontal_rules on tsadm.HorizontalId equals th.Horizontal_rules_id
                           //join tv in _db.tbl_Vertical_rules on tsadm.VerticalId equals tv.Vertical_rules_id
                           join tq in _db.tbl_qualification on tad.Qualification equals tq.QualificationId
                           join ttrm in _db.tbl_trade_mast on tsadm.TradeId equals ttrm.trade_id
                           join tu in _db.tbl_units on tsadm.UnitId equals tu.u_id
                           join ts in _db.tbl_shifts on tsadm.ShiftId equals ts.s_id
                           join tg in _db.tbl_Gender on tad.Gender equals tg.Gender_Id
                           join ty in _db.tbl_Year on sasm.AcademicYear equals ty.YearID
                           join tctm in _db.tbl_course_type_mast on sasm.CourseTypeId equals tctm.course_id
                           //join tssm in _db.tbl_SeatAllocation_SeatMatrix on taid.AllocationId equals tssm.AllocationId
                           join tvoam in _db.tbl_VerOfficer_Applicant_Mapping on tsadm.ApplicantId equals tvoam.ApplicantId into tvoam1 from tvoam
                           in tvoam1.DefaultIfEmpty()
                           join vof in _db.tbl_VerificationOfficer_Master on tvoam.VerifiedOfficer equals vof.Officer_Id 
                           
                           join ttp in _db.tbl_TraineeType on taid.TraineeType equals ttp.TraineeTypeId into ttp1
                           from ttp in ttp1.DefaultIfEmpty()
                           join am in _db.tbl_ApplicationMode on tad.ApplicationMode equals am.ApplicationModeId
                           where collegeId != 0 ? officerId != 0 ? tvoam.VerifiedOfficer == officerId : tsadm.InstituteId == collegeId : taid.ApplicantITIInstituteId == collegeId
                           && taid.IsActive == true && (AdmittedorRejected != 0 ? taid.AdmittedStatus == AdmittedorRejected : true)
                           && (RoundOption != 0 ? sasm.Round == RoundOption : true) && (tsadm.AllocationId == allocId && taid.AllocationId == allocId)
                           && (ApplicatoinMode != 0 ? tad.ApplicationMode == ApplicatoinMode : true)
                           //orderby tsadm.RankNumber
                           select new InstituteWiseAdmission
                           {
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               ApplicantRank = tsadm.RankNumber,
                               ApplicationId = tsadm.ApplicantId,
                               DivisionName = tdm.division_name,
                               MISCode = tcd.MISCode,
                               InstituteName = tcd.iti_college_name,
                               InstituteType = tit.Institute_type,
                               AdmittedStatusEx = tsms.StatusName,
                               ApplInstiStatusEx = tsms.StatusName,
                               AdmittedStatus = taid.AdmittedStatus,
                               AdmissionRegistrationNumber = taid.AdmissionRegistrationNumber,
                               ApplInstiStatus = taid.ApplInstiStatus,
                               //For Excel Data
                               DistrictName = tdim.district_ename,
                               TalukId = tcd.taluk_id,
                               TalukName = ttm.taluk_ename,
                               StateRegistrationNumber = taid.StateRegistrationNumber,
                               DateOfBirth = tad.DOB.ToString(),
                               GenderName = tg.Gender,
                               MobileNumber = tad.PhoneNumber,
                               Email = tad.EmailId,
                               AadharNumber = tad.AadhaarNumber,
                               FathersName = tad.FathersName,
                               MothersName = tad.MothersName,
                               ReligionName = tr.Religion,
                               MinorityCategory = tad.MinorityCategory,
                               CategoryName = tc.Category,
                               HorizontalCategory = th.Horizontal_rules,
                               VerticalCategory = tc.Category,
                               TraineeType = ttp.TraineeType,
                               Qualification = tq.Qualification,
                               RationCardNo = tad.RationCard,
                               IncomeCertificateNo = "N/A",
                               CasteCertNum = "N/A",
                               AccountNumber = tad.AccountNumber,
                               TradeDuration = ttrm.trade_duration,
                               TradeName = ttrm.trade_name,
                               Units = tu.units,
                               Shifts = ts.shifts,
                               DualType = taid.DualType,
                               AdmTime = taid.AdmisionTime.ToString(),
                               AdmisionFee = taid.AdmisionFee,
                               AdmFeePaidStatus = taid.AdmFeePaidStatus,
                               ReceiptNumber = taid.ReceiptNumber,
                               ApplicantITIInstituteId = tcd.iti_college_id,
                               Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),//tad.ApplyYear,
                               CourseType = tctm.course_id,
                               CourseTypeName = tctm.course_type_name,
                               CreatedOn = tsadm.CreatedOn.ToString(),
                               ApplicationMode= am.ApplicationMode,
                               OfficerName=vof.Name
                           }
                    ).Distinct().ToList();

                if (AdmittedorRejected == 1)
                    res = res.Where(a => a.AdmittedStatus == 1 || a.AdmittedStatus == 2).ToList();
                else if (AdmittedorRejected != 0)
                    res = res.Where(a => a.AdmittedStatus == AdmittedorRejected && a.ApplInstiStatus == 6 || a.ApplInstiStatus == 3 || a.ApplInstiStatus == 9).ToList();
                //res = res.Where(a => a.ApplInstiStatus == 4 || a.ApplInstiStatus ==9).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InstituteWiseAdmission> DirectAdmissionApplicantDetailsDLL(int LoginId)
        {
            try
            {
                /* var res = (from taid in _db.tbl_Applicant_ITI_Institute_Detail
                           join tsadm in _db.tbl_SeatAllocationDetail_Seatmatrix on taid.ApplicationId equals tsadm.ApplicantId
                           join tad in _db.tbl_Applicant_Detail on taid.ApplicationId equals tad.ApplicationId
                           join tcd in _db.tbl_iti_college_details on tsadm.InstituteId equals tcd.iti_college_id
                           join tdm in _db.tbl_division_master on tcd.division_id equals tdm.division_id
                           join tsm in _db.tbl_AdmissionatInstituteStatus on taid.AdmittedStatus equals tsm.StatusId
                           join tsms in _db.tbl_AdmissionatInstituteStatus on taid.AdmittedStatus equals tsms.StatusId
                           join sid in _db.Staff_Institute_Detail on tsadm.InstituteId equals sid.InstituteId
                           where sid.UserId == LoginId && taid.IsActive == true && taid.CreatedBy == LoginId && tad.AgainstVacancyInd == 1
                           select new InstituteWiseAdmission
                           {
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               ApplicantRank = tsadm.RankNumber,
                               ApplicationId = tsadm.ApplicantId,
                               DivisionName = tdm.division_name,
                               MISCode = tcd.MISCode,
                               InstituteName = tcd.iti_college_name,
                               AdmittedStatusEx = tsms.StatusName,
                               ApplInstiStatusEx = tsm.StatusName,
                               AdmittedStatus = taid.AdmittedStatus,
                               AdmissionRegistrationNumber = taid.AdmissionRegistrationNumber,
                               ApplInstiStatus = taid.ApplInstiStatus
                           }
                    ).ToList();
                */

                var res = (from tad in _db.tbl_Applicant_Detail
                           join taiid in _db.tbl_Applicant_ITI_Institute_Detail on tad.ApplicationId equals taiid.ApplicationId
                           where taiid.AdmissionTypeID == 1
                                select new InstituteWiseAdmission
                           {
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               ApplicationId = tad.ApplicationId,
                                    // AdmittedStatusEx = "Click on Edit"
                                    AdmittedStatusEx = taiid.AdmittedStatus == 6 ?"Applicant Admitted" : "Rejected the Admission"

                                }
                    ).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InstituteWiseAdmission> GetDataAllocationFeeDetailsDLL(int ApplicationId)
        {
            try
            {
                var res = (from taid in _db.tbl_Applicant_ITI_Institute_Detail
                           join tsadm in _db.tbl_SeatAllocationDetail_Seatmatrix on taid.ApplicationId equals tsadm.ApplicantId
                           join tad in _db.tbl_Applicant_Detail on taid.ApplicationId equals tad.ApplicationId
                           join tcd in _db.tbl_iti_college_details on tsadm.InstituteId equals tcd.iti_college_id
                           join tdm in _db.tbl_division_master on tcd.division_id equals tdm.division_id
                           join tdim in _db.tbl_district_master on tcd.district_id equals tdim.district_lgd_code
                           join ttm in _db.tbl_taluk_master on tcd.taluk_id equals ttm.taluk_lgd_code
                           join tit in _db.tbl_Institute_type on tcd.Insitute_TypeId equals tit.Institute_type_id
                           join tvr in _db.tbl_Vertical_rules on tsadm.VerticalId equals tvr.Vertical_rules_id
                           join thr in _db.Tbl_horizontal_rules on tsadm.HorizontalId equals thr.Horizontal_rules_id
                           join ttradem in _db.tbl_trade_mast on tsadm.TradeId equals ttradem.trade_id
                           join tshifts in _db.tbl_shifts on tsadm.ShiftId equals tshifts.s_id
                           join tunits in _db.tbl_units on tsadm.UnitId equals tunits.u_id
                           join taifd in _db.tbl_AdmissionatInstituteFeeDetails on tcd.Insitute_TypeId equals taifd.Institute_type_id
                           join tsm in _db.tbl_AdmissionatInstituteStatus on taid.AdmittedStatus equals tsm.StatusId
                           where taid.IsActive == true && tad.ApplicationId == ApplicationId
                           select new InstituteWiseAdmission
                           {
                               ApplicationId = tsadm.ApplicantId,
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               ApplicantRank = tsadm.RankNumber,
                               DivisionId = tdm.division_id,
                               DivisionName = tdm.division_name,
                               DistrictName = tdim.district_ename,
                               DistrictId = tdim.district_lgd_code,
                               TalukName = ttm.taluk_ename,
                               TalukId = ttm.taluk_lgd_code,
                               MISCode = tcd.MISCode,
                               ApplyYear = tad.ApplyYear,
                               ApplyMonth = tad.ApplyMonth,
                               FathersName = tad.FathersName,
                               MothersName = tad.MothersName,
                               DOB = tad.DOB,
                               Gender = tad.Gender,
                               Email = tad.EmailId,
                               InstituteName = tcd.iti_college_name,
                               InstituteType = tit.Institute_type,
                               TradeCode = ttradem.trade_code,
                               TradeDuration = ttradem.trade_duration,
                               TradeName = ttradem.trade_name,
                               VerticalCategory = tvr.Vertical_Rules,
                               HorizontalCategory = thr.Horizontal_rules,
                               Units = tshifts.shifts,
                               Shifts = tunits.units,
                               DualType = taid.DualType,
                               TraineeTypeId = taid.TraineeType,
                               ITIUnderPPP = taid.ITIUnderPPP,
                               AdmisionTime = taid.AdmisionTime,
                               AdmisionFee = taifd.InstituteFee,
                               ApplInstiStatusEx = tsm.StatusName,
                               AdmittedStatus = taid.AdmittedStatus,
                               ReceiptNumber = taid.ReceiptNumber,
                               PaymentDate = taid.PaymentDate,
                               PaymentMode = taid.PaymentMode,
                               PaymentStatus = taid.PaymentStatus,
                               PaymentInd = taid.PaymentInd,
                               AdmFeePaidStatus = taid.AdmFeePaidStatus,
                               AdmissionRegistrationNumber = taid.AdmissionRegistrationNumber,
                               StateRegistrationNumber = taid.StateRegistrationNumber,
                               RollNumber = taid.RollNumber,
                               AdmissionTypeID = taid.AdmissionTypeID,
                               Remarks = taid.Remarks

                           }
                    ).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ApplicantApplicationForm> GetApplicantDocumentFeeDetails(int ApplicationId)
        {
            try
            {
               var resData = (from a in _db.tbl_Applicant_Detail
                           join ts in _db.tbl_VerOfficer_Applicant_Mapping on a.ApplicationId equals ts.ApplicantId
                           where a.ApplicationId == ApplicationId && a.IsActive == true
                           select new ApplicantApplicationForm
                           {
                               ApplicationId = a.ApplicationId,
                               ApplyMonth = a.ApplyMonth,
                               ApplyYear = a.ApplyYear,
                               ApplicantName = a.ApplicantName,
                               FathersName = a.FathersName,
                               ParentsOccupation = a.ParentsOccupation,
                               Photo = a.Photo,
                               DOB = a.DOB,
                               Gender = a.Gender,
                               Percentage = a.Percentage,
                               MothersName = a.MothersName,
                               Religion = a.Religion,
                               Category = a.Category,
                               MinorityCategory = a.MinorityCategory,
                               Caste = a.Caste,
                               studiedMathsScience = a.studiedMathsScience,
                               FamilyAnnIncome = a.FamilyAnnIncome,
                               ApplicantType = a.ApplicantType,
                               Qualification = a.Qualification,
                               ApplicantBelongTo = a.ApplicantBelongTo,
                               AppliedBasic = a.AppliedBasic,
                               TenthBoard = a.TenthBoard,
                               InstituteStudiedQual = a.InstituteStudiedQual,
                               MaxMarks = a.MaxMarks,
                               MinMarks = a.MinMarks,
                               ResultQual = a.ResultQual,
                               CommunicationAddress = a.CommunicationAddress,
                               DistrictId = a.DistrictId,
                               TalukaId = a.TalukaId,
                               Pincode = a.Pincode,
                               SameAdd = a.SameAdd,
                               PermanentAddress = a.PermanentAddress,
                               PDistrict = a.PDistrict,
                               PTaluk = a.PTaluk,
                               PPinCode = a.PPinCode,
                               PhoneNumber = a.PhoneNumber,
                               FatherPhoneNumber = a.FatherPhoneNumber,
                               EmailId = a.EmailId,
                               DocVeriInstituteFilter = a.DocVeriInstituteFilter,
                               DocVeriInstituteDistrict = a.DocVeriInstituteDistrict,
                               DocVerificationCentre = a.DocVerificationCentre,
                               PaymentOptionval = a.PaymentOptionval,
                               //DocumentFeeReceiptDetails = a.DocumentFeeReceiptDetails,
                               DocumentFeeReceiptDetails = ts.DocVeriFeeReceiptNumber,
                               FlowId = a.FlowId,
                               AssignedVO = a.AssignedVO,
                               DocVeriFeePymtDate = ts.DocVeriFeePymtDate,
                               ApplicantNumber = a.ApplicantNumber,
                               DocVeriFee = ts.DocVeriFee,
                               TreasuryReceiptNo = ts.Treasury_Receipt_No


                           }).ToList();

                return resData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InstituteWiseAdmission> GeneratePaymentReceiptDLL(InstituteWiseAdmission model)
        {
            var curyear = DateTime.Now.ToString("yy");

            try
            {
                using (var transaction = new TransactionScope())
                {

                    #region .. State Registration Number logic ..

                    string GenStRegiNumber = "";

                    //To find the Institute ID
                    int InstituteId = (from tad in _db.Staff_Institute_Detail
                                       where tad.UserId == model.UserLoginId && tad.IsActive == true
                                       select tad.InstituteId).FirstOrDefault();


                    //To get MISCode, Division , Institute 
                    var GetInstituteDetails = (from tad in _db.tbl_iti_college_details
                                               where tad.iti_college_id == InstituteId
                                               select new InstituteWiseAdmission
                                               {
                                                   MISCode = tad.MISCode,
                                                   division_id = tad.division_id,
                                                   Insitute_TypeId = tad.Insitute_TypeId
                                               }).ToList();


                    if (GetInstituteDetails.Count > 0 )
                    {
                        foreach (var ExistingAssignedVOval in GetInstituteDetails)
                        {
                            if (ExistingAssignedVOval.division_id == 1)
                                GenStRegiNumber = Convert.ToString(CmnClass.stRegNumbStDiv.KA)
                                    + Convert.ToString(CmnClass.stRegNumbStDiv.B);
                            else if (ExistingAssignedVOval.division_id == 2)
                                GenStRegiNumber = Convert.ToString(CmnClass.stRegNumbStDiv.KA)
                                    + Convert.ToString(CmnClass.stRegNumbStDiv.M);
                            else if (ExistingAssignedVOval.division_id == 3)
                                GenStRegiNumber = Convert.ToString(CmnClass.stRegNumbStDiv.KA)
                                    + Convert.ToString(CmnClass.stRegNumbStDiv.H);
                            else if (ExistingAssignedVOval.division_id == 4)
                                GenStRegiNumber = Convert.ToString(CmnClass.stRegNumbStDiv.KA)
                                    + Convert.ToString(CmnClass.stRegNumbStDiv.K);

                            if (ExistingAssignedVOval.Insitute_TypeId == 1 || ExistingAssignedVOval.Insitute_TypeId == 3)
                                GenStRegiNumber += CmnClass.stRegNumbStDiv.G +
                                    curyear + (Convert.ToString(ExistingAssignedVOval.MISCode)).Substring(2, 8);
                            else
                                GenStRegiNumber += CmnClass.stRegNumbStDiv.P +
                                    curyear + (Convert.ToString(ExistingAssignedVOval.MISCode)).Substring(2, 8);

                            break;
                        }
                    }

                    //To get the all Reg # from the DB, to make the check digit
                    var StateRegistrationNumberDet = _db.tbl_Applicant_ITI_Institute_Detail.Where(m => m.StateRegistrationNumber
                    .Contains(GenStRegiNumber) && m.IsActive == true).Select(x => x.StateRegistrationNumber).ToList();

                    string ToChkExistingRec = "";
                    if (StateRegistrationNumberDet.Count > 0)
                    {
                        foreach (var ExistingAssignedStval in StateRegistrationNumberDet)
                        {
                            ToChkExistingRec = ExistingAssignedStval;
                        }
                    }

                    //To Generate the Check Digits
                    string ToGenCheckDigits = ""; int ToGenCheckDigits1 = 1;
                    if (ToChkExistingRec != "")
                    {
                        ToChkExistingRec = StateRegistrationNumberDet.Max();
                        ToGenCheckDigits = ToChkExistingRec.Substring(14, 4).ToString();
                        if (ToGenCheckDigits.Length != 4)
                        {
                            ToGenCheckDigits = ToGenCheckDigits.PadLeft(4, '0');
                        }
                        ToGenCheckDigits1 = Int32.Parse(ToGenCheckDigits);
                        ToGenCheckDigits1++;
                    }
                    ToChkExistingRec = ToGenCheckDigits1.ToString().PadLeft(4, '0');

                    #endregion


                    #region .. Admission Registration Number Logic ..

                    string AdmGenStRegiNumber = "";
                    if (GetInstituteDetails.Count > 0)
                    {
                        foreach (var ExistingAssignedVOval in GetInstituteDetails)
                        {
                            if (ExistingAssignedVOval.division_id == 1)
                                AdmGenStRegiNumber = (Convert.ToString(ExistingAssignedVOval.MISCode))
                                    + Convert.ToString(CmnClass.stRegNumbStDiv.B) + curyear + model.TradeName.Substring(0, 3);
                            else if (ExistingAssignedVOval.division_id == 2)
                                AdmGenStRegiNumber = (Convert.ToString(ExistingAssignedVOval.MISCode))
                                    + Convert.ToString(CmnClass.stRegNumbStDiv.M) + curyear + model.TradeName.Substring(0, 3);
                            else if (ExistingAssignedVOval.division_id == 3)
                                AdmGenStRegiNumber = (Convert.ToString(ExistingAssignedVOval.MISCode))
                                    + Convert.ToString(CmnClass.stRegNumbStDiv.H) + curyear + model.TradeName.Substring(0, 3);
                            else if (ExistingAssignedVOval.division_id == 4)
                                AdmGenStRegiNumber = (Convert.ToString(ExistingAssignedVOval.MISCode))
                                    + Convert.ToString(CmnClass.stRegNumbStDiv.K) + curyear + model.TradeName.Substring(0, 3);

                            break;
                        }
                    }

                    //To get the all Admission Reg # from the DB, to make the check digit
                    var AdmissionRegistrationNumberDet = _db.tbl_Applicant_ITI_Institute_Detail.Where(m => m.AdmissionRegistrationNumber
                    .Contains(AdmGenStRegiNumber) && m.IsActive == true).Select(x => x.AdmissionRegistrationNumber).ToList();

                    string AdmToChkExistingRec = "";
                    if (AdmissionRegistrationNumberDet.Count > 0)
                    {
                        foreach (var ExistingAssignedStval in AdmissionRegistrationNumberDet)
                        {
                            AdmToChkExistingRec = ExistingAssignedStval;
                        }
                    }

                    //To Generate the Check Digits
                    string AdmToGenCheckDigits = ""; int AdmToGenCheckDigits1 = 1;
                    if (AdmToChkExistingRec != "")
                    {
                        AdmToChkExistingRec = StateRegistrationNumberDet.Max();
                        AdmToGenCheckDigits = AdmToChkExistingRec.Substring(14, 4).ToString();
                        if (AdmToGenCheckDigits.Length != 4)
                        {
                            AdmToGenCheckDigits = AdmToGenCheckDigits.PadLeft(4, '0');
                        }
                        AdmToGenCheckDigits1 = Int32.Parse(AdmToGenCheckDigits);
                        AdmToGenCheckDigits1++;
                    }
                    AdmToChkExistingRec = AdmToGenCheckDigits1.ToString().PadLeft(4, '0');

                    #endregion

                    #region .. Update in tbl_Applicant_ITI_Institute_Detail ..

                    var ExistingRecordForUpdate = (from tad in _db.tbl_Applicant_ITI_Institute_Detail
                                                   where tad.ApplicationId == model.ApplicationId
                                                   select new { tad.ApplicantITIInstituteId, tad.AdmFeePaidStatus}).Take(1).FirstOrDefault();

                    if (ExistingRecordForUpdate.AdmFeePaidStatus != 1)
                    {
                        var update_query = _db.tbl_Applicant_ITI_Institute_Detail.Where(s => s.ApplicationId == model.ApplicationId).FirstOrDefault();
                        update_query.ApplicantITIInstituteId = ExistingRecordForUpdate.ApplicantITIInstituteId;
                        update_query.AdmisionFee = model.PaymentAmount;
                        update_query.AdmisionTime = model.AdmisionTime;
                        update_query.ITIUnderPPP = model.ITIUnderPPP;
                        update_query.DualType = model.DualType;
                        update_query.TraineeType = model.TraineeTypeId;
                        if (model.ClickToPay == 1)
                        {
                            update_query.AdmFeePaidStatus = 1;
                            update_query.PaymentDate = DateTime.Now;
                            update_query.ReceiptNumber = "DITE-PAY-REC-ONLINE-001";
                            update_query.PaymentMode = "WALLET";
                            update_query.PaymentStatus = "Success";
                            update_query.AdmissionRegistrationNumber = "DITE-ADM-REG-ONLINE-001";
                            update_query.StateRegistrationNumber = "DITE-ST-REG-ONLINE-001";
                            update_query.RollNumber = "DITE-ROLL-ONLINE-001";
                            update_query.PaymentInd = 1;
                        }
                        else
                        {
                            update_query.AdmFeePaidStatus = model.AdmFeePaidStatus;
                            update_query.PaymentDate = DateTime.Now;
                            update_query.StateRegistrationNumber = GenStRegiNumber + ToChkExistingRec;
                            update_query.AdmissionRegistrationNumber = AdmGenStRegiNumber + AdmToChkExistingRec;
                            update_query.ReceiptNumber = GetReceiptNumber(model,0,0);
                            update_query.PaymentStatus = "Success";
                            update_query.PaymentMode = "CASH";
                            update_query.RollNumber = GetRollNumber(model, GetInstituteDetails.Select(a => Convert.ToInt32(a.division_id)).Take(1).FirstOrDefault());
                            update_query.PaymentInd = 0;
                        }
                        _db.SaveChanges();

                        #endregion

                        #region .. Update in tbl_Applicant_ITI_Institute_Detail_Trans ..


                        tbl_Applicant_ITI_Institute_Detail_Trans objtbl_Applicant_ITI_Institute_Detail_Trans = new tbl_Applicant_ITI_Institute_Detail_Trans();
                        objtbl_Applicant_ITI_Institute_Detail_Trans.ApplicantITIInstituteId = ExistingRecordForUpdate.ApplicantITIInstituteId;
                        objtbl_Applicant_ITI_Institute_Detail_Trans.ApplicationId = model.ApplicationId;
                        objtbl_Applicant_ITI_Institute_Detail_Trans.AdmittedStatus = model.AdmittedStatus;
                        objtbl_Applicant_ITI_Institute_Detail_Trans.Remarks = model.Remarks;
                        objtbl_Applicant_ITI_Institute_Detail_Trans.FlowId = model.CreatedBy;
                        objtbl_Applicant_ITI_Institute_Detail_Trans.CreatedBy = model.CreatedBy;
                        objtbl_Applicant_ITI_Institute_Detail_Trans.CreatedOn = DateTime.Now;

                        _db.tbl_Applicant_ITI_Institute_Detail_Trans.Add(objtbl_Applicant_ITI_Institute_Detail_Trans);
                        _db.SaveChanges();

                        #endregion

                        transaction.Complete();
                    }
                }

                var res = (from taid in _db.tbl_Applicant_ITI_Institute_Detail
                           join tad in _db.tbl_Applicant_Detail on taid.ApplicationId equals tad.ApplicationId
                           join ttp in _db.tbl_TraineeType on taid.TraineeType equals ttp.TraineeTypeId into ttp1
                           from ttp in ttp1.DefaultIfEmpty()

                           where taid.IsActive == true && tad.ApplicationId == model.ApplicationId
                           select new InstituteWiseAdmission
                           {
                               ApplicationId = tad.ApplicationId,
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               ReceiptNumber = taid.ReceiptNumber,
                               PaymentDate = taid.PaymentDate,
                               AdmisionFee = taid.AdmisionFee,
                               AdmisionTime = taid.AdmisionTime,
                               PaymentMode = taid.PaymentMode,
                               PaymentStatus = taid.PaymentStatus,
                               AdmFeePaidStatus = taid.AdmFeePaidStatus,
                               AdmittedStatus = taid.AdmittedStatus,
                               AdmissionRegistrationNumber = taid.AdmissionRegistrationNumber,
                               StateRegistrationNumber = taid.StateRegistrationNumber,
                               RollNumber = taid.RollNumber,
                               PaymentInd = taid.PaymentInd,
                               DualType = taid.DualType,
                               TraineeType = ttp.TraineeType,
                               ITIUnderPPP = taid.ITIUnderPPP
                           }
                    ).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InstituteWiseAdmission> UpdateAdmittedDetailsDLL(InstituteWiseAdmission model)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    #region .. Update in tbl_Applicant_ITI_Institute_Detail ..

                    int? PaymentInd = (from tum in _db.tbl_Applicant_ITI_Institute_Detail
                                       where tum.ApplicationId == model.ApplicationId
                                       select tum.PaymentInd).Take(1).FirstOrDefault();

                    var update_query = _db.tbl_Applicant_ITI_Institute_Detail.Where(s => s.ApplicationId == model.ApplicationId).FirstOrDefault();
                    update_query.AdmisionFee = model.PaymentAmount;
                    update_query.AdmisionTime = model.AdmisionTime != null ? model.AdmisionTime : DateTime.Now.Date;
                    update_query.ITIUnderPPP = model.ITIUnderPPP;
                    update_query.DualType = model.DualType;
                    update_query.TraineeType = model.TraineeTypeId;
                    update_query.AdmittedStatus = model.AdmittedStatus;
                    update_query.ApplInstiStatus = model.ApplInstiStatus;
                    update_query.Remarks = model.Remarks;
                    update_query.AdmissionTypeID = model.AdmissionTypeID;
                    update_query.Shiftid = model.Shiftid;
                    update_query.Unitid = model.Unitid;
                    update_query.AdmFeePaidStatus = model.AdmFeePaidStatus;
                    update_query.FlowId = model.CreatedBy;



                    _db.SaveChanges();

                    #endregion

                    #region .. Update in tbl_Applicant_ITI_Institute_Detail_Trans ..

                    int ExistingRecordForUpdate = (from tad in _db.tbl_Applicant_ITI_Institute_Detail
                                                   where tad.ApplicationId == model.ApplicationId
                                                   select tad.ApplicantITIInstituteId).Take(1).FirstOrDefault();

                    tbl_Applicant_ITI_Institute_Detail_Trans objtbl_Applicant_ITI_Institute_Detail_Trans = new tbl_Applicant_ITI_Institute_Detail_Trans();
                    objtbl_Applicant_ITI_Institute_Detail_Trans.ApplicantITIInstituteId = ExistingRecordForUpdate;
                    objtbl_Applicant_ITI_Institute_Detail_Trans.ApplicationId = model.ApplicationId;
                    objtbl_Applicant_ITI_Institute_Detail_Trans.AdmittedStatus = model.AdmittedStatus;
                    objtbl_Applicant_ITI_Institute_Detail_Trans.ApplInstiStatus = model.ApplInstiStatus;
                    objtbl_Applicant_ITI_Institute_Detail_Trans.Remarks = model.Remarks;
                    objtbl_Applicant_ITI_Institute_Detail_Trans.FlowId = model.CreatedBy;
                    objtbl_Applicant_ITI_Institute_Detail_Trans.CreatedBy = model.CreatedBy;
                    objtbl_Applicant_ITI_Institute_Detail_Trans.CreatedOn = DateTime.Now;

                    _db.tbl_Applicant_ITI_Institute_Detail_Trans.Add(objtbl_Applicant_ITI_Institute_Detail_Trans);
                    _db.SaveChanges();

                    #endregion

                    transaction.Complete();
                }

                var res = (from taid in _db.tbl_Applicant_ITI_Institute_Detail
                           join tad in _db.tbl_Applicant_Detail on taid.ApplicationId equals tad.ApplicationId

                           where taid.IsActive == true && tad.ApplicationId == model.ApplicationId
                           select new InstituteWiseAdmission
                           {
                               ApplicationId = tad.ApplicationId,
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               ReceiptNumber = taid.ReceiptNumber,
                               PaymentDate = taid.PaymentDate,
                               AdmisionFee = taid.AdmisionFee,
                               AdmisionTime = taid.AdmisionTime,
                               PaymentMode = taid.PaymentMode,
                               PaymentStatus = taid.PaymentStatus,
                               AdmFeePaidStatus = taid.AdmFeePaidStatus,
                               AdmittedStatus = taid.AdmittedStatus,
                               AdmissionRegistrationNumber = taid.AdmissionRegistrationNumber,
                               StateRegistrationNumber = taid.StateRegistrationNumber,
                               RollNumber = taid.RollNumber,
                               PaymentInd = taid.PaymentInd,
                           }
                    ).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InstituteWiseAdmission> GeneratePaymentReceiptPDFDLL(int ApplicationId)
        {
            try
            {
                var res = (from taid in _db.tbl_Applicant_ITI_Institute_Detail
                           join tad in _db.tbl_Applicant_Detail on taid.ApplicationId equals tad.ApplicationId
                           join tsadsm in _db.tbl_SeatAllocationDetail_Seatmatrix on taid.ApplicationId equals tsadsm.ApplicantId
                           join tdm in _db.tbl_trade_mast on tsadsm.TradeId equals tdm.trade_id
                           join ticd in _db.tbl_iti_college_details on tsadsm.InstituteId equals ticd.iti_college_id

                           where taid.IsActive == true && tad.ApplicationId == ApplicationId
                           select new InstituteWiseAdmission
                           {
                               ApplicationId = tad.ApplicationId,
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               ReceiptNumber = taid.ReceiptNumber,
                               PaymentDate = taid.PaymentDate,
                               AdmisionFee = taid.AdmisionFee,
                               PaymentMode = taid.PaymentMode,
                               PaymentStatus = taid.PaymentStatus,
                               TradeName = tdm.trade_name,
                               InstituteName = ticd.iti_college_name,
                               MISCode = ticd.MISCode
                           }
                    ).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InstituteWiseAdmission> GenerateDocumentPaymentReceiptPDFBLL(int ApplicationId)
        {
            try
            {
                var res = (from taid in _db.tbl_Applicant_Detail
                           join tad in _db.tbl_VerOfficer_Applicant_Mapping on taid.ApplicationId equals tad.ApplicantId
                           join it in _db.tbl_iti_college_details on taid.DocVerificationCentre equals it.iti_college_id
                           join tq in _db.tbl_qualification on taid.Qualification equals tq.QualificationId


                           where taid.IsActive == true && tad.ApplicantId== ApplicationId
                           select new InstituteWiseAdmission
                           {
                               ApplicationId = tad.ApplicantId,
                               ApplicantNumber = taid.ApplicantName,
                               ApplicantName = taid.ApplicantName,
                               ReceiptNumber = tad.DocVeriFeeReceiptNumber,
                               PaymentDate = tad.DocVeriFeePymtDate,
                               AdmisionFee = tad.DocVeriFee,
                               PaymentMode = "Offline",
                               PaymentStatus = tad.DocVeriFeePymtStatus.ToString(),
                               Qualification=tq.Qualification,
                               CategoryName=taid.CategoryName,
                               InstituteName=it.iti_college_name,
                               MISCode=it.MISCode

                               
                           }
                    ).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApplicantDocumentsDetail> GetApplicantDocStatusDLL(int ApplicantId)
        {
            var resData = (from a in _db.tbl_Document_Applicant
                           join tad in _db.tbl_DocumentType on a.DocumentTypeId equals tad.DocumentTypeId
                           where a.ApplicantId == ApplicantId && a.Verified != 0
                       select new ApplicantDocumentsDetail
                       {
                           FileName = tad.DocumentType,
                           FilePath = a.FilePath,
                           DocumentTypeId = a.DocumentTypeId,
                           ApplicantId = a.ApplicantId,
                           Verified = a.Verified,
                           DocumentRemarks = a.Remarks

                       }).ToList();
            return resData;
        }

        public List<InstituteWiseAdmission> GenerateAdmissionAcknowledgementPDFDLL(int ApplicationId)
        {
            try
            {
                var res = (from tad in _db.tbl_Applicant_Detail
                           join tq in _db.tbl_qualification on tad.Qualification equals tq.QualificationId

                           where tad.IsActive == true && tad.ApplicationId == ApplicationId
                           select new InstituteWiseAdmission
                           {
                               //ApplicationId = tad.ApplicationId,
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               CategoryName = tad.CategoryName,
                               Qualification = tq.Qualification,
                               CreatedOn = tad.CreatedOn.ToString()
                           }
                    ).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApplicantDocumentsDetail> ApplicantDocumentDetailsInsDLL(ApplicantDocumentsDetail objApplicantApplicationForm)
        {
            UploadPreferenceType output = new UploadPreferenceType();
            List<ApplicantDocumentsDetail> resData = new List<ApplicantDocumentsDetail>();

            try
            {
                using (var transaction = new TransactionScope())
                {
                    int ExistingRecordForUpdate = 0;
                    int ApplicationId = objApplicantApplicationForm.ApplicantId;
                    int DocAppId = objApplicantApplicationForm.DocAppId;

                    if (DocAppId != 0)
                    {
                        ExistingRecordForUpdate = (from tad in _db.tbl_Document_Applicant
                                                   where tad.ApplicantId == ApplicationId && tad.DocumentTypeId == objApplicantApplicationForm.DocumentTypeId &&
                                                   tad.DocAppId == objApplicantApplicationForm.DocAppId
                                                   select tad.DocAppId).Take(1).FirstOrDefault();
                    }

                    if (ExistingRecordForUpdate == 0)
                    {
                        tbl_Document_Applicant objtbl_Document_Applicant = new tbl_Document_Applicant();
                        objtbl_Document_Applicant.ApplicantId = objApplicantApplicationForm.ApplicantId;
                        objtbl_Document_Applicant.DocumentTypeId = objApplicantApplicationForm.DocumentTypeId;
                        if (objApplicantApplicationForm.FileName != null)
                        {
                            objtbl_Document_Applicant.FileName = objApplicantApplicationForm.FileName;
                            objtbl_Document_Applicant.FilePath = objApplicantApplicationForm.FilePath;
                        }
                        if (objApplicantApplicationForm.Verified != 0)
                        {
                            objtbl_Document_Applicant.Verified = objApplicantApplicationForm.Verified;
                        }
                        objtbl_Document_Applicant.CreatedOn = DateTime.Now;
                        objtbl_Document_Applicant.CreatedBy = objApplicantApplicationForm.CreatedBy;
                        objtbl_Document_Applicant.UpdatedBy = objApplicantApplicationForm.UpdatedBy;
                        if (objApplicantApplicationForm.DocumentRemarks != null)
                        {
                            objtbl_Document_Applicant.Remarks = objApplicantApplicationForm.DocumentRemarks;
                        }
                        objtbl_Document_Applicant.DocumentSet = 3;
                        _db.tbl_Document_Applicant.Add(objtbl_Document_Applicant);
                        _db.SaveChanges();

                        DocAppId = (from tad in _db.tbl_Document_Applicant
                                    orderby tad.CreatedOn descending
                                    select tad.DocAppId).Take(1).FirstOrDefault();

                    }
                    else
                    {
                        tbl_Document_Applicant objtbl_Document_Applicant = new tbl_Document_Applicant();
                        var update_query = _db.tbl_Document_Applicant.Where(s => s.ApplicantId == objApplicantApplicationForm.ApplicantId &&
                                s.DocumentTypeId == objApplicantApplicationForm.DocumentTypeId && s.DocAppId == objApplicantApplicationForm.DocAppId).FirstOrDefault();

                        update_query.ApplicantId = objApplicantApplicationForm.ApplicantId;
                        update_query.DocumentTypeId = objApplicantApplicationForm.DocumentTypeId;
                        if (objApplicantApplicationForm.FileName != null)
                        {
                            update_query.FileName = objApplicantApplicationForm.FileName;
                            update_query.FilePath = objApplicantApplicationForm.FilePath;
                        }

                        if (objApplicantApplicationForm.Verified != 0)
                        {
                            update_query.Verified = objApplicantApplicationForm.Verified;
                        }
                        update_query.CreatedBy = objApplicantApplicationForm.CreatedBy;
                        update_query.UpdatedBy = objApplicantApplicationForm.UpdatedBy;
                        update_query.DocumentSet = 3;
                        if (objApplicantApplicationForm.DocumentRemarks != null)
                        {
                            update_query.Remarks = objApplicantApplicationForm.DocumentRemarks;
                        }
                        _db.SaveChanges();
                    }

                    if (objApplicantApplicationForm.FileName != null)
                    {
                        int? ExistingAssignedVO = (from tad in _db.tbl_Applicant_Detail
                                                   where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                                   select tad.AssignedVO).Take(1).FirstOrDefault();

                        tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                        objtbl_ApplicantTrans.ApplicantId = objApplicantApplicationForm.ApplicantId;
                        objtbl_ApplicantTrans.VerfOfficer = objApplicantApplicationForm.UpdatedBy;
                        objtbl_ApplicantTrans.Remark = objApplicantApplicationForm.Remarks;
                        if (objApplicantApplicationForm.Verified == 1)
                        {
                            objtbl_ApplicantTrans.Status = objApplicantApplicationForm.Verified;
                            objtbl_ApplicantTrans.DocumentWiseStatus = objApplicantApplicationForm.Verified;
                            objtbl_ApplicantTrans.ReVerficationStatus = true;
                        }
                        else
                        {
                            objtbl_ApplicantTrans.Status = 5;
                            objtbl_ApplicantTrans.DocumentWiseStatus = 5;
                            objtbl_ApplicantTrans.ReVerficationStatus = false;
                        }
                        objtbl_ApplicantTrans.TransDate = DateTime.Now;
                        objtbl_ApplicantTrans.IsActive = 1;
                        objtbl_ApplicantTrans.Remark = objApplicantApplicationForm.DocumentRemarks;
                        objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                        objtbl_ApplicantTrans.DocumentName = objApplicantApplicationForm.DocumentTypeId;
                        objtbl_ApplicantTrans.DocumentWiseRemarks = objApplicantApplicationForm.DocumentRemarks;
                        objtbl_ApplicantTrans.CreatedBy = objApplicantApplicationForm.UpdatedBy;
                        objtbl_ApplicantTrans.FlowId = objApplicantApplicationForm.UpdatedBy;

                        _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                        _db.SaveChanges();
                    }

                    resData = (from a in _db.tbl_Document_Applicant
                               where a.ApplicantId == ApplicationId
                               select new ApplicantDocumentsDetail
                               {
                                   FileName = a.FileName,
                                   FilePath = a.FilePath,
                                   DocumentTypeId = a.DocumentTypeId,
                                   ApplicantId = a.ApplicantId,
                                   Verified = a.Verified,
                                   DocumentRemarks = a.Remarks

                               }).ToList();

                    transaction.Complete();
                    resData.Add(new ApplicantDocumentsDetail { ErrorOccuredMsg = "Proper Data Format", ErrorOccuredInd = 0, UpdateMsg = "success" });
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                resData.Clear();
                resData.Add(new ApplicantDocumentsDetail { ErrorOccuredMsg = "Error in Data Format", ErrorOccuredInd = 1, UpdateMsg = "failed" });
            }
            return resData;
        }

        public int GetUserDivionIdDLL(int LoginId)
        {
            int res = 0;
            try
            {
                res = _db.tbl_user_master.Where(a => a.um_id == LoginId).Select(b => (int)b.um_div_id).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public List<ApplicantDocumentsDetail> ApplicantUpdateInstituteDLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            var resData = (dynamic)null;

            try
            {
                using (var transaction = new TransactionScope())
                {
                    #region .. Updating in tbl_Applicant_Details ..

                    int ApplicationId = objApplicantApplicationForm.ApplicantId;

                    var update_query_Appl = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicantApplicationForm.ApplicantId && s.IsActive == true).FirstOrDefault();
                    update_query_Appl.ApplicantBelongTo = objApplicantApplicationForm.ApplicantBelongTo;
                    update_query_Appl.PhysicallyHanidcapInd = objApplicantApplicationForm.PhysicallyHanidcapInd;
                    update_query_Appl.PhysicallyHanidcapType = objApplicantApplicationForm.PhysicallyHanidcapType;
                    update_query_Appl.KanndaMedium = objApplicantApplicationForm.KanndaMedium;
                    update_query_Appl.ExemptedFromStudyCertificate = objApplicantApplicationForm.ExemptedFromStudyCertificate;
                    update_query_Appl.HyderabadKarnatakaRegion = objApplicantApplicationForm.HyderabadKarnatakaRegion;
                    update_query_Appl.HoraNadu_GadiNadu_Kannidagas = objApplicantApplicationForm.HoraNadu_GadiNadu_Kannidagas;
                    update_query_Appl.HyderabadKarnatakaRegion = objApplicantApplicationForm.HyderabadKarnatakaRegion;
                    update_query_Appl.ApplRemarks = objApplicantApplicationForm.Remarks;

                    _db.SaveChanges();

                    #endregion

                    tbl_Applicant_Reservation objtbl_Applicant_Reservation = new tbl_Applicant_Reservation();
                    if (objApplicantApplicationForm.ApplicableReservations != null)
                    {
                        var remove_query = _db.tbl_Applicant_Reservation.Where(s => s.ApplicantId == objApplicantApplicationForm.ApplicantId && s.IsActive == true).ToList();
                        foreach (var item in remove_query)
                        {
                            _db.tbl_Applicant_Reservation.Remove(item);
                        }
                        foreach (int ReservationApp in objApplicantApplicationForm.ApplicableReservations)
                        {
                            objtbl_Applicant_Reservation.ApplicantId = objApplicantApplicationForm.ApplicantId;
                            objtbl_Applicant_Reservation.ReservationId = ReservationApp;
                            objtbl_Applicant_Reservation.IsActive = true;
                            objtbl_Applicant_Reservation.CreatedOn = DateTime.Now;
                            _db.tbl_Applicant_Reservation.Add(objtbl_Applicant_Reservation);
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        if (objApplicantApplicationForm.ExserDocStatus == 3)
                        {
                            var remove_query = _db.tbl_Applicant_Reservation.Where(s => s.ApplicantId == objApplicantApplicationForm.ApplicantId && s.ReservationId == 2 && s.IsActive == true).ToList();
                            foreach (var item in remove_query)
                            {
                                _db.tbl_Applicant_Reservation.Remove(item);
                            }
                        }
                        if (objApplicantApplicationForm.EWSDocStatus == 3)
                        {
                            var remove_query = _db.tbl_Applicant_Reservation.Where(s => s.ApplicantId == objApplicantApplicationForm.ApplicantId && s.ReservationId == 5 && s.IsActive == true).ToList();
                            foreach (var item in remove_query)
                            {
                                _db.tbl_Applicant_Reservation.Remove(item);
                            }
                        }
                        _db.SaveChanges();
                    }


                    #region Updating in tbl_ApplicantTrans

                    var ExistingAssignedVO = (from tad in _db.tbl_Applicant_Detail
                                              where tad.ApplicationId == objApplicantApplicationForm.ApplicantId && tad.IsActive == true
                                              select new ApplicationStatusUpdate
                                              {
                                                  FlowId = tad.FlowId,
                                                  AssignedVO = tad.AssignedVO,
                                                  ApplDescStatus = tad.ApplDescStatus,
                                                  ApplStatusS = tad.ApplStatus
                                              }).ToList();

                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                    objtbl_ApplicantTrans.ApplicantId = objApplicantApplicationForm.ApplicantId;
                    objtbl_ApplicantTrans.CreatedBy = objApplicantApplicationForm.CredatedBy;
                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                    objtbl_ApplicantTrans.IsActive = 1;
                    objtbl_ApplicantTrans.FinalSubmitInd = 1;
                    objtbl_ApplicantTrans.ReVerficationStatus = false;
                    objtbl_ApplicantTrans.Remark = "Documents and appropriate Indicator Updated at ITI Admin Level";
                    if (ExistingAssignedVO.Count > 0)
                    {
                        foreach (var ExistingAssignedVOval in ExistingAssignedVO)
                        {
                            objtbl_ApplicantTrans.Status = ExistingAssignedVOval.ApplStatusS;
                            objtbl_ApplicantTrans.VerfOfficer = ExistingAssignedVOval.AssignedVO;
                            objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                            objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                            objtbl_ApplicantTrans.FlowId = ExistingAssignedVOval.FlowId;
                        }
                    }
                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                    _db.SaveChanges();

                    #endregion

                    objApplicantApplicationForm.UpdateMsg = "success";

                    resData = (from a in _db.tbl_Applicant_Detail
                               where a.ApplicationId == ApplicationId && a.IsActive == true
                               select new GrievanceDocApplData
                               {
                                   ApplicantId = a.ApplicationId
                               }).ToList();

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                objApplicantApplicationForm.UpdateMsg = "failed";
                resData = objApplicantApplicationForm;
            }

            return resData;
        }

        public List<InstituteWiseAdmission> GetCommentDetailsApplicantInstituteDLL(int SeatAllocationId)
        {
            var res = (from taiidt in _db.tbl_Applicant_ITI_Institute_Detail_Trans
                       join tat in _db.tbl_Applicant_Detail on taiidt.ApplicationId equals tat.ApplicationId
                       join tur in _db.tbl_user_master on taiidt.CreatedBy equals tur.um_id
                       join turs in _db.tbl_user_master on taiidt.FlowId equals turs.um_id
                       join tais in _db.tbl_AdmissionatInstituteStatus on taiidt.AdmittedStatus equals tais.StatusId
                       where taiidt.ApplicationId == SeatAllocationId
                       orderby taiidt.CreatedOn descending
                       select new InstituteWiseAdmission
                       {
                           ApplicationId = taiidt.ApplicationId,
                           CreatedOn = taiidt.CreatedOn.ToString(),
                           userRole = tur.um_name,
                           ForwardedTo = turs.um_name,
                           StatusName = tais.StatusName,
                           Remarks = (taiidt.Remarks == "" || taiidt.Remarks == null ? "NA" : taiidt.Remarks)
                       }).ToList();

            return res;
        }

        #region .. Division Officer Login ..

        public List<InstituteWiseAdmission> GetAdmissionApplicantsDistLoginDLL(AdmissionApplicantsDistLogin objAdmissionApplicantsDistLogin, int Id)
        {
            try
            {
                List<InstituteWiseAdmission> res = null;

                int roleLevel = (from tum in _db.tbl_user_master
                                 join tump in _db.tbl_user_mapping on tum.um_id equals tump.um_id
                                 join trm in _db.tbl_role_master on tump.role_id equals trm.role_id
                                 where tum.um_id == Id

                                 select (int)trm.role_Level).Take(1).FirstOrDefault();

                if (roleLevel != 0)   //JD2
                {
                    res = (from taid in _db.tbl_Applicant_ITI_Institute_Detail
                           join tsadm in _db.tbl_SeatAllocationDetail_Seatmatrix on taid.ApplicationId equals tsadm.ApplicantId
                           join tad in _db.tbl_Applicant_Detail on taid.ApplicationId equals tad.ApplicationId
                           join tcd in _db.tbl_iti_college_details on tsadm.InstituteId equals tcd.iti_college_id
                           join tdm in _db.tbl_division_master on tcd.division_id equals tdm.division_id
                           join tdim in _db.tbl_district_master on tcd.district_id equals tdim.district_lgd_code
                           join tsm in _db.tbl_AdmissionatInstituteStatus on taid.ApplInstiStatus equals tsm.StatusId
                           join tsms in _db.tbl_AdmissionatInstituteStatus on taid.AdmittedStatus equals tsms.StatusId
                           join ttm in _db.tbl_taluk_master on tcd.taluk_id equals ttm.taluk_lgd_code
                           join tr in _db.tbl_Religion on tad.Religion equals tr.Religion_Id
                           join tc in _db.tbl_Category on tad.Category equals tc.CategoryId
                           join th in _db.Tbl_horizontal_rules on tsadm.HorizontalId equals th.Horizontal_rules_id
                           join tv in _db.tbl_Vertical_rules on tsadm.VerticalId equals tv.Vertical_rules_id
                           join tq in _db.tbl_qualification on tad.Qualification equals tq.QualificationId
                           join ttrm in _db.tbl_trade_mast on tsadm.TradeId equals ttrm.trade_id
                           join tu in _db.tbl_units on tsadm.UnitId equals tu.u_id
                           join ts in _db.tbl_shifts on tsadm.ShiftId equals ts.s_id
                           join tg in _db.tbl_Gender on tad.Gender equals tg.Gender_Id
                           join tssm in _db.tbl_SeatAllocation_SeatMatrix on taid.AllocationId equals tssm.AllocationId
                           join ttp in _db.tbl_TraineeType on taid.TraineeType equals ttp.TraineeTypeId into ttp1
                           from ttp in ttp1.DefaultIfEmpty()
                           join tit in _db.tbl_Institute_type on tcd.Insitute_TypeId equals tit.Institute_type_id
                           join tat in _db.tbl_ApplicantType on tssm.ApplicantType equals tat.ApplicantTypeId
                           join tct in _db.tbl_course_type_mast on tssm.CourseTypeId equals tct.course_id
                           join tar in _db.tbl_ApplicantAdmissionRounds on tssm.Round equals tar.ApplicantAdmissionRoundsId

                           where taid.IsActive == true && taid.AdmittedStatus == objAdmissionApplicantsDistLogin.AdmittedorRejected //&& taid.ApplInstiStatus == 4
                           && tssm.AcademicYear == objAdmissionApplicantsDistLogin.Session
                           && tssm.CourseTypeId == objAdmissionApplicantsDistLogin.CourseType
                           && (objAdmissionApplicantsDistLogin.ApplicantType != 0  ? tssm.ApplicantType == objAdmissionApplicantsDistLogin.ApplicantType : true)
                           && (objAdmissionApplicantsDistLogin.RoundOption != 0 ? tssm.Round == objAdmissionApplicantsDistLogin.RoundOption : true)

                           select new InstituteWiseAdmission
                           {
                               DistrictId = (int)tcd.district_id,
                               DistrictName = tdim.district_ename,
                               TalukId = tcd.taluk_id,
                               TalukName = ttm.taluk_ename,
                               ApplicantNumber = tad.ApplicantNumber,
                               StateRegistrationNumber = taid.StateRegistrationNumber,
                               ApplicantName = tad.ApplicantName,
                               DateOfBirth = tad.DOB.ToString(),
                               GenderName = tg.Gender,
                               MobileNumber = tad.PhoneNumber,
                               Email = tad.EmailId,
                               AadharNumber = tad.AadhaarNumber,
                               FathersName = tad.FathersName,
                               MothersName = tad.MothersName,
                               ReligionName = tr.Religion,
                               MinorityCategory = tad.MinorityCategory,
                               CategoryName = tc.Category,
                               HorizontalCategory = th.Horizontal_rules,
                               VerticalCategory = tv.Vertical_Rules,
                               TraineeType = ttp.TraineeType,
                               Qualification = tq.Qualification,
                               RationCardNo = tad.RationCard,
                               IncomeCertificateNo = "N/A",
                               CasteCertNum = "N/A",
                               AccountNumber = tad.AccountNumber,
                               TradeName = ttrm.trade_name,
                               Units = tu.units,
                               Shifts = ts.shifts,
                               DualType = taid.DualType,
                               AdmTime = taid.AdmisionTime.ToString(),
                               AdmisionFee = taid.AdmisionFee,
                               AdmFeePaidStatus = taid.AdmFeePaidStatus,
                               ReceiptNumber = taid.ReceiptNumber,
                               ApplicantRank = tsadm.RankNumber,
                               ApplicationId = tsadm.ApplicantId,
                               division_id = tdm.division_id,
                               DivisionName = tdm.division_name,
                               MISCode = tcd.MISCode,
                               InstituteType = tit.Institute_type,
                               CourseTypeName = tct.course_type_name,
                               ApplicantType = tat.ApplicantType,
                               RoundList = tar.RoundList,
                               ApplicantITIInstituteId = tcd.iti_college_id,
                               InstituteName = tcd.iti_college_name,
                               AdmittedStatusEx = tsms.StatusName,
                               ApplInstiStatusEx = tsm.StatusName,
                               AdmissionRegistrationNumber = taid.AdmissionRegistrationNumber,
                               ColumnCheck = taid.ColumnCheck
                           }
                       ).ToList();

                    if (objAdmissionApplicantsDistLogin.Division != 0)
                    {
                        res = res.Where(a => a.division_id == objAdmissionApplicantsDistLogin.Division).ToList();
                    }
                    if (objAdmissionApplicantsDistLogin.District != 0)
                    {
                        res = res.Where(a => a.DistrictId == objAdmissionApplicantsDistLogin.District).ToList();
                    }
                    if (objAdmissionApplicantsDistLogin.Taluk != 0)
                    {
                        res = res.Where(a => a.TalukId == objAdmissionApplicantsDistLogin.Taluk).ToList();
                    }
                    if (objAdmissionApplicantsDistLogin.ITIInstitute != 0)
                    {
                        res = res.Where(a => a.ApplicantITIInstituteId == objAdmissionApplicantsDistLogin.ITIInstitute).ToList();
                    }
                }
                else
                {
                    res = (from taid in _db.tbl_Applicant_ITI_Institute_Detail
                           join tsadm in _db.tbl_SeatAllocationDetail_Seatmatrix on taid.ApplicationId equals tsadm.ApplicantId
                           join tad in _db.tbl_Applicant_Detail on taid.ApplicationId equals tad.ApplicationId
                           join tcd in _db.tbl_iti_college_details on tsadm.InstituteId equals tcd.iti_college_id
                           join tdm in _db.tbl_division_master on tcd.division_id equals tdm.division_id
                           join tsm in _db.tbl_AdmissionatInstituteStatus on taid.ApplInstiStatus equals tsm.StatusId
                           join tsms in _db.tbl_AdmissionatInstituteStatus on taid.AdmittedStatus equals tsms.StatusId
                           where taid.IsActive == true && taid.AdmittedStatus == objAdmissionApplicantsDistLogin.AdmittedorRejected /*&& taid.ApplInstiStatus != 5 && taid.ApplInstiStatus != 9 &&taid.ApplInstiStatus==9*/
                           && tcd.iti_college_id == objAdmissionApplicantsDistLogin.ITIInstitute

                           //orderby tsadm.RankNumber
                           select new InstituteWiseAdmission
                           {
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               ApplicantRank = tsadm.RankNumber,
                               ApplicationId = tsadm.ApplicantId,
                               DivisionName = tdm.division_name,
                               MISCode = tcd.MISCode,
                               InstituteName = tcd.iti_college_name,
                               AdmittedStatusEx = tsms.StatusName,
                               ApplInstiStatusEx = tsm.StatusName,
                               AdmissionRegistrationNumber = taid.AdmissionRegistrationNumber,
                               ColumnCheck = taid.ColumnCheck
                           }
                       ).ToList();
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public List<HorizontalVerticalCategorycs> GetHorizontalCategoryDLL()
        {
            var res = (from a in _db.Tbl_horizontal_rules
                       where a.IsActive == true
                       select new HorizontalVerticalCategorycs
                       {
                           Horizontal_rules_id = a.Horizontal_rules_id,
                           Horizontal_rules = a.Horizontal_rules
                       }).ToList();
            return res;
        }

        public int GetApplIdByApplicationNumberDLL(string ExistChkApplicationNumber)
        {
            int res = 0;
            res = _db.tbl_Applicant_Detail.Where(a => a.ApplicantNumber == ExistChkApplicationNumber && a.IsActive == true)
                .Select(y => y.ApplicationId).FirstOrDefault();

            return res;
        }

        public List<HorizontalVerticalCategorycs> GetVerticalCategoryDLL()
        {
            var res = (from a in _db.tbl_Vertical_rules
                       where a.IsActive == true
                       select new HorizontalVerticalCategorycs
                       {
                           Vertical_Rules = a.Vertical_Rules,
                           Vertical_rules_id = a.Vertical_rules_id
                       }).ToList();
            return res;
        }

        public List<HorizontalVerticalCategorycs> GetShiftsDetailsDLL()
        {
            var res = (from a in _db.tbl_shifts
                       where a.s_is_active == true
                       select new HorizontalVerticalCategorycs
                       {
                           s_id = a.s_id,
                           shifts = a.shifts
                       }).ToList();
            return res;
        }

        public List<HorizontalVerticalCategorycs> GetUnitsDetailsDLL()
        {
            var res = (from a in _db.tbl_units
                       where a.u_is_active == true
                       select new HorizontalVerticalCategorycs
                       {
                           units = a.units,
                           u_id = a.u_id
                       }).ToList();
            return res;
        }
        public List<ApplicationForm> GetITICollegeDetailsMasterDLL(int District, int Taluka)
        {
            var res = (from ticd in _db.tbl_iti_college_details
                       where ticd.district_id == District && ticd.taluk_id == Taluka
                       select new ApplicationForm
                       {
                           iti_college_code = ticd.iti_college_id,
                           iti_college_name = ticd.iti_college_name
                       }).ToList();

            return res;
        }

        public ApplicantAdmiAgainstVacancy GetInstituteMasterDLL(int LoginId)
        {
            int InstitueID = _db.Staff_Institute_Detail.Where(a => a.UserId == LoginId).Select(b => b.InstituteId).FirstOrDefault();

            var res = (dynamic)null;
            res = (from ticd in _db.tbl_iti_college_details
                   join tdm in _db.tbl_district_master on ticd.district_id equals tdm.district_lgd_code
                   join tdmm in _db.tbl_division_master on ticd.division_id equals tdmm.division_id
                   join ttm in _db.tbl_taluk_master on ticd.taluk_id equals ttm.taluk_lgd_code
                   join tit in _db.tbl_Institute_type on ticd.Insitute_TypeId equals tit.Institute_type_id
                   join tctm in _db.tbl_course_type_mast on ticd.CourseCode equals tctm.course_id
                   join taifd in _db.tbl_AdmissionatInstituteFeeDetails on ticd.Insitute_TypeId equals taifd.Institute_type_id

                   where ticd.iti_college_id == InstitueID
                   select new ApplicantAdmiAgainstVacancy
                   {
                       CollegeId = ticd.iti_college_id,
                       Division = tdmm.division_id,
                       DivisionName = tdmm.division_name,
                       District = tdm.district_lgd_code,
                       DistrictName = tdm.district_ename,
                       Taluk = ttm.taluk_lgd_code,
                       TalukName = ttm.taluk_ename,
                       ITIInstituteName = ticd.iti_college_name,
                       InstituteTypeDet = tit.Institute_type,
                       MISCode = ticd.MISCode,
                       CourseTypeDet = tctm.course_type_name,
                       TuitionFee = taifd.InstituteFee


                   }).FirstOrDefault();

            return res;
        }

        public List<HorizontalVerticalCategorycs> GetInstituteTradeMasterDLL(int CollegeId)
        {
            var res = (dynamic)null;
            res = (from ticd in _db.tbl_iti_college_details
                   join tdm in _db.tbl_ITI_Trade on ticd.iti_college_id equals tdm.ITICode
                   join tdmm in _db.tbl_trade_mast on tdm.TradeCode equals tdmm.trade_id

                   where ticd.iti_college_id == CollegeId && ticd.is_active == true
                   select new HorizontalVerticalCategorycs
                   {
                       TradeName = tdmm.trade_id,
                       TradeNameDet = tdmm.trade_name

                   }).ToList();

            return res;
        }

        public List<ApplicationForm> GetUnitsShiftsDetailsDLL(int CollegeId, int TradeId)
        {
            var res = (from a in _db.tbl_iti_college_details
                       join e in _db.tbl_Institute_type on a.Insitute_TypeId equals e.Institute_type_id
                       join f in _db.tbl_course_type_mast on a.CourseCode equals f.course_id
                       join g in _db.tbl_ITI_Trade on a.iti_college_id equals g.ITICode
                       join h in _db.tbl_trade_mast on g.TradeCode equals h.trade_id
                       join i in _db.tbl_ITI_Trade_Shifts on g.Trade_ITI_id equals i.ITI_Trade_Id
                       join j in _db.tbl_units on i.Units equals j.u_id
                       join k in _db.tbl_shifts on i.Shift equals k.s_id

                       where a.iti_college_id == CollegeId && g.TradeCode == TradeId
                       select new ApplicationForm
                       {
                           UnitsId = j.u_id,
                           UnitsValue = j.units,
                           ShiftId = k.s_id,
                           ShiftValue = k.shifts
                       }).ToList();

            return res;
        }

        public List<ApplicantAdmiAgainstVacancy> UpdateApplicantAdmissionAgainstVacancyDLL(ApplicantAdmiAgainstVacancy objApplicantAdmiAgainstVacancyList)
        {
            #region .. Updating in tbl_Applicant_Details ..

            int ExistingRecordForUpdate = (from tad in _db.tbl_Applicant_Admi_Against_Vacancy
                                           where tad.ApplicantId == objApplicantAdmiAgainstVacancyList.ApplicantId
                                           select tad.ApplicantId).Take(1).FirstOrDefault();

            int ApplicationId = objApplicantAdmiAgainstVacancyList.ApplicantId;

            if (ExistingRecordForUpdate == 0)
            {
                tbl_Applicant_Admi_Against_Vacancy objtbl_Applicant_Admi_Against_Vacancy = new tbl_Applicant_Admi_Against_Vacancy();
                objtbl_Applicant_Admi_Against_Vacancy.AcademicMonths = objApplicantAdmiAgainstVacancyList.AcademicMonths;
                objtbl_Applicant_Admi_Against_Vacancy.AcademicYear = objApplicantAdmiAgainstVacancyList.AcademicYear;
                objtbl_Applicant_Admi_Against_Vacancy.DirectAdmission = objApplicantAdmiAgainstVacancyList.DirectAdmission;
                objtbl_Applicant_Admi_Against_Vacancy.ApplicationNumber = objApplicantAdmiAgainstVacancyList.ApplicationNumber;
                objtbl_Applicant_Admi_Against_Vacancy.StateRegistrationNumber = objApplicantAdmiAgainstVacancyList.StateRegistrationNumber;
                objtbl_Applicant_Admi_Against_Vacancy.Division = objApplicantAdmiAgainstVacancyList.Division;
                objtbl_Applicant_Admi_Against_Vacancy.District = objApplicantAdmiAgainstVacancyList.District;
                objtbl_Applicant_Admi_Against_Vacancy.Taluk = objApplicantAdmiAgainstVacancyList.Taluk;
                objtbl_Applicant_Admi_Against_Vacancy.InstituteType = objApplicantAdmiAgainstVacancyList.InstituteType;
                objtbl_Applicant_Admi_Against_Vacancy.ITIInstitute = objApplicantAdmiAgainstVacancyList.ITIInstitute;
                objtbl_Applicant_Admi_Against_Vacancy.CourseType = objApplicantAdmiAgainstVacancyList.CourseType;
                objtbl_Applicant_Admi_Against_Vacancy.AdmissionVerticalCategory = objApplicantAdmiAgainstVacancyList.AdmissionVerticalCategory;
                objtbl_Applicant_Admi_Against_Vacancy.AdmissionHorizontalCategory = objApplicantAdmiAgainstVacancyList.AdmissionHorizontalCategory;
                objtbl_Applicant_Admi_Against_Vacancy.TraineeType = objApplicantAdmiAgainstVacancyList.TraineeType;
                objtbl_Applicant_Admi_Against_Vacancy.TradeName = objApplicantAdmiAgainstVacancyList.TradeName;
                objtbl_Applicant_Admi_Against_Vacancy.Units = objApplicantAdmiAgainstVacancyList.Units;
                objtbl_Applicant_Admi_Against_Vacancy.Shift = objApplicantAdmiAgainstVacancyList.Shift;
                objtbl_Applicant_Admi_Against_Vacancy.DualSystem = objApplicantAdmiAgainstVacancyList.DualSystem;
                objtbl_Applicant_Admi_Against_Vacancy.AdmisionTime = objApplicantAdmiAgainstVacancyList.AdmisionTime;
                objtbl_Applicant_Admi_Against_Vacancy.TuitionFee = objApplicantAdmiAgainstVacancyList.TuitionFee;
                objtbl_Applicant_Admi_Against_Vacancy.TuitionFeePaidStatus = objApplicantAdmiAgainstVacancyList.TuitionFeePaidStatus;
                objtbl_Applicant_Admi_Against_Vacancy.PaymentDate = objApplicantAdmiAgainstVacancyList.PaymentDate;
                objtbl_Applicant_Admi_Against_Vacancy.ReceiptNumber = objApplicantAdmiAgainstVacancyList.ReceiptNumber;
                objtbl_Applicant_Admi_Against_Vacancy.AdmissionStatus = objApplicantAdmiAgainstVacancyList.AdmissionStatus;

                _db.tbl_Applicant_Admi_Against_Vacancy.Add(objtbl_Applicant_Admi_Against_Vacancy);
                _db.SaveChanges();
            }
            else
            {
                var update_query_Appl = _db.tbl_Applicant_Admi_Against_Vacancy.Where(s => s.ApplicantId == objApplicantAdmiAgainstVacancyList.ApplicantId && s.IsActive == true).FirstOrDefault();
                update_query_Appl.AcademicMonths = objApplicantAdmiAgainstVacancyList.AcademicMonths;
                update_query_Appl.AcademicYear = objApplicantAdmiAgainstVacancyList.AcademicYear;
                update_query_Appl.DirectAdmission = objApplicantAdmiAgainstVacancyList.DirectAdmission;
                update_query_Appl.ApplicationNumber = objApplicantAdmiAgainstVacancyList.ApplicationNumber;
                update_query_Appl.StateRegistrationNumber = objApplicantAdmiAgainstVacancyList.StateRegistrationNumber;
                update_query_Appl.Division = objApplicantAdmiAgainstVacancyList.Division;
                update_query_Appl.District = objApplicantAdmiAgainstVacancyList.District;
                update_query_Appl.Taluk = objApplicantAdmiAgainstVacancyList.Taluk;
                update_query_Appl.InstituteType = objApplicantAdmiAgainstVacancyList.InstituteType;
                update_query_Appl.ITIInstitute = objApplicantAdmiAgainstVacancyList.ITIInstitute;
                update_query_Appl.CourseType = objApplicantAdmiAgainstVacancyList.CourseType;
                update_query_Appl.AdmissionVerticalCategory = objApplicantAdmiAgainstVacancyList.AdmissionVerticalCategory;
                update_query_Appl.AdmissionHorizontalCategory = objApplicantAdmiAgainstVacancyList.AdmissionHorizontalCategory;
                update_query_Appl.TraineeType = objApplicantAdmiAgainstVacancyList.TraineeType;
                update_query_Appl.TradeName = objApplicantAdmiAgainstVacancyList.TradeName;
                update_query_Appl.Units = objApplicantAdmiAgainstVacancyList.Units;
                update_query_Appl.Shift = objApplicantAdmiAgainstVacancyList.Shift;
                update_query_Appl.DualSystem = objApplicantAdmiAgainstVacancyList.DualSystem;
                update_query_Appl.AdmisionTime = objApplicantAdmiAgainstVacancyList.AdmisionTime;
                update_query_Appl.TuitionFee = objApplicantAdmiAgainstVacancyList.TuitionFee;
                update_query_Appl.TuitionFeePaidStatus = objApplicantAdmiAgainstVacancyList.TuitionFeePaidStatus;
                update_query_Appl.PaymentDate = objApplicantAdmiAgainstVacancyList.PaymentDate;
                update_query_Appl.ReceiptNumber = objApplicantAdmiAgainstVacancyList.ReceiptNumber;
                update_query_Appl.AdmissionStatus = objApplicantAdmiAgainstVacancyList.AdmissionStatus;

                _db.SaveChanges();
            }

            var res = (from taid in _db.tbl_Applicant_Admi_Against_Vacancy
                       where taid.IsActive == true
                       select new ApplicantAdmiAgainstVacancy
                       {
                           ApplicantId = taid.ApplicantId,
                           AcademicMonths = taid.AcademicMonths,
                           AcademicYear = taid.AcademicYear,
                           DirectAdmission = taid.DirectAdmission,
                           ApplicationNumber = taid.ApplicationNumber,
                           StateRegistrationNumber = taid.StateRegistrationNumber,
                           Division = taid.Division,
                           District = taid.District,
                           Taluk = taid.Taluk,
                           InstituteType = taid.InstituteType,
                           ITIInstitute = taid.ITIInstitute,
                           CourseType = taid.CourseType,
                           AdmissionVerticalCategory = taid.AdmissionVerticalCategory,
                           AdmissionHorizontalCategory = taid.AdmissionHorizontalCategory,
                           TraineeType = taid.TraineeType,
                           TradeName = taid.TradeName,
                           Units = taid.Units,
                           Shift = taid.Shift,
                           DualSystem = taid.DualSystem,
                           AdmisionTime = taid.AdmisionTime,
                           TuitionFee = taid.TuitionFee,
                           TuitionFeePaidStatus = taid.TuitionFeePaidStatus,
                           PaymentDate = taid.PaymentDate,
                           ReceiptNumber = taid.ReceiptNumber,
                           AdmissionStatus = taid.AdmissionStatus
                       }
                    ).ToList();

            return res;

            #endregion
        }

        //03-07-2021

        public string ApprovedRejectedList(InstituteWiseAdmission model, int loginId)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    string[] Listarray = model.checkboxes_value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in Listarray)
                    {
                        int? ChkAppId = Convert.ToInt32(item);
                        var list = (from t in _db.tbl_Applicant_ITI_Institute_Detail.Where(x => x.ApplicationId == ChkAppId && x.ApplInstiStatus == 4)
                                    select new InstituteWiseAdmission
                                    {
                                        ApplicantITIInstituteId = t.ApplicantITIInstituteId,
                                        ApplicationId = t.ApplicationId,
                                        AdmittedStatus = t.AdmittedStatus,
                                        IsActive = t.IsActive,
                                        Remarks = t.Remarks,
                                        FlowId = t.FlowId,
                                        ApplInstiStatus = t.ApplInstiStatus
                                    }).Distinct().ToList();

                        foreach (var p in list)
                        {
                            if (list != null)
                            {
                                tbl_Applicant_ITI_Institute_Detail ITIlist = _db.tbl_Applicant_ITI_Institute_Detail.Where(x => x.ApplicantITIInstituteId == p.ApplicantITIInstituteId).FirstOrDefault();
                                ITIlist.ApplInstiStatus = 9;
                                ITIlist.FlowId = 9;
                                _db.SaveChanges();

                                tbl_Applicant_ITI_Institute_Detail_Trans ITITrans = new tbl_Applicant_ITI_Institute_Detail_Trans();
                                ITITrans.ApplicantITIInstituteId = p.ApplicantITIInstituteId;
                                ITITrans.ApplicationId = p.ApplicationId;
                                ITITrans.AdmittedStatus = p.AdmittedStatus;
                                ITITrans.FlowId = 9;
                                ITITrans.Remarks = p.Remarks;
                                ITITrans.IsActive = p.IsActive;
                                ITITrans.CreatedOn = DateTime.Now;
                                ITITrans.CreatedBy = loginId;
                                ITITrans.ApplInstiStatus = 9;
                                _db.tbl_Applicant_ITI_Institute_Detail_Trans.Add(ITITrans);
                                _db.SaveChanges();
                            }
                            else
                            {
                                return "No Records Founds";
                            }
                        }
                    }
                    transaction.Complete();
                    return "success";
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    throw ex;
                }
            }
        }
        public List<InstituteWiseAdmission> SentBackAdmittedListDLL(InstituteWiseAdmission model, int loginId, int sentId)
        {

            try
            {
                using (var transaction = new TransactionScope())
                {
                    string[] Listarray = model.checkboxes_value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in Listarray)
                    {
                        int? ChkAppId = Convert.ToInt32(item);
                        var list = (from t in _db.tbl_Applicant_ITI_Institute_Detail.Where(x => x.ApplicationId == ChkAppId && x.ApplInstiStatus == 4)
                                    select new InstituteWiseAdmission
                                    {
                                        ApplicantITIInstituteId = t.ApplicantITIInstituteId,
                                        ApplicationId = t.ApplicationId,
                                        AdmittedStatus = t.AdmittedStatus,
                                        IsActive = t.IsActive,
                                        Remarks = t.Remarks,
                                        FlowId = t.FlowId,
                                        ApplInstiStatus = t.ApplInstiStatus
                                    }).Distinct().ToList();

                        foreach (var p in list)
                        {
                            if (list != null && p.ApplInstiStatus == 4)
                            {
                                tbl_Applicant_ITI_Institute_Detail ITIlist = _db.tbl_Applicant_ITI_Institute_Detail.Where(x => x.ApplicantITIInstituteId == p.ApplicantITIInstituteId).FirstOrDefault();
                                ITIlist.ApplInstiStatus = 9;
                                ITIlist.FlowId = sentId;
                                _db.SaveChanges();

                                tbl_Applicant_ITI_Institute_Detail_Trans ITITrans = new tbl_Applicant_ITI_Institute_Detail_Trans();
                                ITITrans.ApplicantITIInstituteId = p.ApplicantITIInstituteId;
                                ITITrans.ApplicationId = p.ApplicationId;
                                ITITrans.AdmittedStatus = p.AdmittedStatus;
                                ITITrans.FlowId = sentId;
                                ITITrans.Remarks = p.Remarks;
                                ITITrans.IsActive = p.IsActive;
                                ITITrans.CreatedOn = DateTime.Now;
                                ITITrans.CreatedBy = loginId;
                                ITITrans.ApplInstiStatus = 9;
                                _db.tbl_Applicant_ITI_Institute_Detail_Trans.Add(ITITrans);
                                _db.SaveChanges();
                            }
                            //else
                            //{
                            //    return "No Records Founds";
                            //}
                        }
                    }

                    transaction.Complete();
                }
                var res = (from taid in _db.tbl_Applicant_ITI_Institute_Detail
                           join tad in _db.tbl_Applicant_Detail on taid.ApplicationId equals tad.ApplicationId

                           where taid.IsActive == true && taid.ApplInstiStatus == 4 && taid.AdmittedStatus == 6
                           select new InstituteWiseAdmission
                           {
                               ApplicationId = taid.ApplicationId,
                               //ApplicantNumber = taid.ApplicantNumber,
                               //ApplicantName = taid.ApplicantName,
                               ReceiptNumber = taid.ReceiptNumber,
                               PaymentDate = taid.PaymentDate,
                               AdmisionFee = taid.AdmisionFee,
                               AdmisionTime = taid.AdmisionTime,
                               PaymentMode = taid.PaymentMode,
                               PaymentStatus = taid.PaymentStatus,
                               AdmFeePaidStatus = taid.AdmFeePaidStatus,
                               AdmittedStatus = taid.AdmittedStatus,
                               AdmissionRegistrationNumber = taid.AdmissionRegistrationNumber,
                               StateRegistrationNumber = taid.StateRegistrationNumber,
                               RollNumber = taid.RollNumber,
                               PaymentInd = taid.PaymentInd
                           }
                   ).ToList();

                return res;
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw ex;
            }

        }
        public string GetforwardAdmittedListDLL(InstituteWiseAdmission model, int loginId, int ForId)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    string[] Listarray = model.checkboxes_value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in Listarray)
                    {
                        int? ChkAppId = Convert.ToInt32(item);
                        var list = (from t in _db.tbl_Applicant_ITI_Institute_Detail.Where(x => x.ApplicationId == ChkAppId && x.ApplInstiStatus == 4)
                                    select new InstituteWiseAdmission
                                    {
                                        ApplicantITIInstituteId = t.ApplicantITIInstituteId,
                                        ApplicationId = t.ApplicationId,
                                        AdmittedStatus = t.AdmittedStatus,
                                        IsActive = t.IsActive,
                                        Remarks = t.Remarks,
                                        FlowId = t.FlowId,
                                        ApplInstiStatus = t.ApplInstiStatus
                                    }).Distinct().ToList();

                        foreach (var p in list)
                        {
                            if (list != null && p.ApplInstiStatus == 4)
                            {
                                tbl_Applicant_ITI_Institute_Detail ITIlist = _db.tbl_Applicant_ITI_Institute_Detail.Where(x => x.ApplicantITIInstituteId == p.ApplicantITIInstituteId).FirstOrDefault();
                                ITIlist.ApplInstiStatus = 7;
                                ITIlist.FlowId = ForId;
                                _db.SaveChanges();

                                tbl_Applicant_ITI_Institute_Detail_Trans ITITrans = new tbl_Applicant_ITI_Institute_Detail_Trans();
                                ITITrans.ApplicantITIInstituteId = p.ApplicantITIInstituteId;
                                ITITrans.ApplicationId = p.ApplicationId;
                                ITITrans.AdmittedStatus = p.AdmittedStatus;
                                ITITrans.FlowId = ForId;
                                ITITrans.Remarks = p.Remarks;
                                ITITrans.IsActive = p.IsActive;
                                ITITrans.CreatedOn = DateTime.Now;
                                ITITrans.CreatedBy = loginId;
                                ITITrans.ApplInstiStatus = 7;
                                _db.tbl_Applicant_ITI_Institute_Detail_Trans.Add(ITITrans);
                                _db.SaveChanges();
                            }
                            else
                            {
                                return "No Records Founds";
                            }
                        }
                    }

                    transaction.Complete();

                    return "success";
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    throw ex;
                }
            }
        }
        public List<InstituteWiseAdmission> GetCommentDetailsRemarks(int loginId, int ApplicationId)
        {
            var res = (from tram in _db.tbl_Applicant_ITI_Institute_Detail_Trans
                       join tat in _db.tbl_Applicant_ITI_Institute_Detail on tram.ApplicantITIInstituteId equals tat.ApplicantITIInstituteId
                       join tafds in _db.tbl_AdmissionatInstituteStatus on tram.ApplInstiStatus equals tafds.AdmissionatInstituteStatusId
                       join Apl in _db.tbl_Applicant_Detail on tat.ApplicationId equals Apl.ApplicationId
                       //join tur in _db.tbl_user_master on tram.CreatedBy equals tur.um_id
                       join turs in _db.tbl_user_master on tram.FlowId equals turs.um_id
                       join tursF in _db.tbl_user_master on tram.CreatedBy equals tursF.um_id
                       where Apl.ApplicationId == ApplicationId
                       orderby tram.CreatedOn descending
                       select new InstituteWiseAdmission
                       {
                           ApplicationId = tram.ApplicationId,
                           CreatedOn = tram.CreatedOn.ToString(),
                           userRole = tursF.um_name,
                           ForwardedTo = turs.um_name,
                           ApplInstiStatus = tram.ApplInstiStatus,
                           StatusName = tafds.StatusName,
                           //ApplDescription = tafds.ApplDescription,
                           Remarks = (tram.Remarks == "" || tram.Remarks == null ? "NA" : tram.Remarks)
                       }).Distinct().ToList();

            return res;
        }
        public List<InstituteWiseAdmission> GetCommentDetailsRemarksByIdDLL(int ApplicationId)
        {

            var res = (from t in _db.tbl_Applicant_ITI_Institute_Detail.Where(x => x.ApplicationId == ApplicationId)
                       join tum in _db.tbl_user_master on t.FlowId equals tum.um_id
                           //orderby t.CreatedOn descending
                       select new InstituteWiseAdmission
                       {
                           ApplicantITIInstituteId = t.ApplicantITIInstituteId,
                           AdmTime = (t.AdmisionTime == null ? "NA" : t.AdmisionTime.ToString()),
                           ApplicationId = t.ApplicationId,
                           AdmittedStatus = t.AdmittedStatus,
                           IsActive = t.IsActive,
                           //Remarks = t.Remarks,
                           Remarks = (t.Remarks == "" || t.Remarks == null ? "NA" : t.Remarks),
                           FlowId = t.FlowId,
                           ApplInstiStatus = t.ApplInstiStatus,
                           userRole = tum.um_name
                       }).Distinct().ToList();
            //}).Take(1).ToList();
            return res;
        }
        public string GetclickAddRemarksTransDLL(InstituteWiseAdmission model, int loginId)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    //string[] Listarray = model.checkboxes_value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    //foreach (var item in Listarray)
                    //{
                    // int? ChkAppId = Convert.ToInt32(item);
                    var list = (from t in _db.tbl_Applicant_ITI_Institute_Detail.Where(x => x.ApplicationId == model.ApplicationId)
                                    //orderby t.CreatedOn descending
                                select new InstituteWiseAdmission
                                {
                                    ApplicantITIInstituteId = t.ApplicantITIInstituteId,
                                    ApplicationId = t.ApplicationId,
                                    AdmittedStatus = t.AdmittedStatus,
                                    IsActive = t.IsActive,
                                    //Remarks = t.Remarks,
                                    FlowId = t.FlowId,
                                    ApplInstiStatus = t.ApplInstiStatus
                                }).Take(1).ToList();

                    foreach (var p in list)
                    {
                        if (list != null)
                        {
                            tbl_Applicant_ITI_Institute_Detail ITIlist = _db.tbl_Applicant_ITI_Institute_Detail.Where(x => x.ApplicantITIInstituteId == p.ApplicantITIInstituteId).FirstOrDefault();
                            //ITIlist.ApplInstiStatus = 9;
                            ITIlist.Remarks = model.AddRemark;
                            ITIlist.FlowId = loginId;
                            _db.SaveChanges();

                            tbl_Applicant_ITI_Institute_Detail_Trans ITITrans = new tbl_Applicant_ITI_Institute_Detail_Trans();
                            ITITrans.ApplicantITIInstituteId = p.ApplicantITIInstituteId;
                            ITITrans.ApplicationId = p.ApplicationId;
                            ITITrans.AdmittedStatus = p.AdmittedStatus;
                            ITITrans.FlowId = loginId;
                            ITITrans.Remarks = model.AddRemark;
                            ITITrans.IsActive = p.IsActive;
                            ITITrans.CreatedOn = DateTime.Now;
                            ITITrans.CreatedBy = loginId;
                            ITITrans.ApplInstiStatus = p.ApplInstiStatus;
                            _db.tbl_Applicant_ITI_Institute_Detail_Trans.Add(ITITrans);
                            _db.SaveChanges();
                        }
                        else
                        {
                            return "No Records Founds";
                        }
                    }
                    transaction.Complete();

                    //}
                    return "success";
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    throw ex;
                }
            }
        }

        public string GetOnClickSendToHierarchyDLL(InstituteWiseAdmission model, int loginId, int ForId, string TabName)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var list = (from t in _db.tbl_Applicant_ITI_Institute_Detail.Where(x => x.AdmittedStatus == 6 && x.ApplInstiStatus == 6 || x.ApplInstiStatus == 9)
                                select new InstituteWiseAdmission
                                {
                                    ApplicantITIInstituteId = t.ApplicantITIInstituteId,
                                    ApplicationId = t.ApplicationId,
                                    AdmittedStatus = t.AdmittedStatus,
                                    IsActive = t.IsActive,
                                    Remarks = t.Remarks,
                                    FlowId = t.FlowId,
                                    ApplInstiStatus = t.ApplInstiStatus
                                }).Distinct().ToList();

                    foreach (var p in list)
                    {
                        if (list != null)
                        {
                            tbl_Applicant_ITI_Institute_Detail ITIlist = _db.tbl_Applicant_ITI_Institute_Detail.Where(x => x.ApplicantITIInstituteId == p.ApplicantITIInstituteId).FirstOrDefault();
                            ITIlist.ApplInstiStatus = 4;
                            ITIlist.FlowId = ForId;
                            _db.SaveChanges();

                            tbl_Applicant_ITI_Institute_Detail_Trans ITITrans = new tbl_Applicant_ITI_Institute_Detail_Trans();
                            ITITrans.ApplicantITIInstituteId = p.ApplicantITIInstituteId;
                            ITITrans.ApplicationId = p.ApplicationId;
                            ITITrans.AdmittedStatus = p.AdmittedStatus;
                            ITITrans.FlowId = ForId;
                            ITITrans.Remarks = p.Remarks;
                            ITITrans.IsActive = p.IsActive;
                            ITITrans.CreatedOn = DateTime.Now;
                            ITITrans.CreatedBy = loginId;
                            ITITrans.ApplInstiStatus = 4;
                            _db.tbl_Applicant_ITI_Institute_Detail_Trans.Add(ITITrans);
                            _db.SaveChanges();
                        }
                        else
                        {
                            return "No Records Founds";
                        }
                    }
                    transaction.Complete();

                    return "success";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //public List<InstituteWiseAdmission> GetCommentDetailsRemarks(int loginId)
        //{
        //    using (var transaction = new TransactionScope())
        //    {
        //        try
        //        {

        //          var list = (from t in _db.tbl_Applicant_ITI_Institute_Detail
        //                      select new InstituteWiseAdmission
        //                      {
        //                          ApplicantITIInstituteId = t.ApplicantITIInstituteId,
        //                          ApplicationId = t.ApplicationId,
        //                          AdmittedStatus = t.AdmittedStatus,
        //                          IsActive = t.IsActive,
        //                          Remarks = t.Remarks,
        //                          FlowId = t.FlowId,
        //                          ApplInstiStatus = t.ApplInstiStatus
        //                      }).Distinct().ToList();



        //            transaction.Complete();

        //            return list;
        //        }
        //        catch (Exception ex)
        //        {
        //            //transaction.Rollback();
        //            throw ex;
        //        }
        //    }
        //}

        public string GetReceiptNumberGen(InstituteWiseAdmission model, int ApplID, int rcpt)
        {
           return  GetReceiptNumber(model, ApplID, rcpt);
        }
            private string GetReceiptNumber(InstituteWiseAdmission model, int ApplID, int rcpt)
        {
            var curyear = DateTime.Now.ToString("yy");
            List<string> InstituteReceiptNum = new List<string>();
            var miscode = "";
            if (model.MISCode!=null)
            {
                 InstituteReceiptNum = _db.tbl_Applicant_ITI_Institute_Detail.Where(m => m.ReceiptNumber
                                      .Contains(model.MISCode.Substring(0, 2) + model.MISCode.Substring(model.MISCode.Length - 4) + curyear + CmnClass.FeePaymentReceiptText.Admission)
                                      && m.IsActive == true).Select(x => x.ReceiptNumber).ToList();
            }
            
           else
            {
                if(model.CreatedBy!=0)
                {
                    int InstitueID = _db.tbl_VerificationOfficer_Master.Where(a => a.UserMasterId == model.CreatedBy).Select(b => b.InstituteId).FirstOrDefault();
                     miscode = _db.tbl_iti_college_details.Where(a => a.iti_college_id == InstitueID).Select(c => c.MISCode).FirstOrDefault();
                }
                 InstituteReceiptNum = _db.tbl_VerOfficer_Applicant_Mapping.Where(m => m.DocVeriFeeReceiptNumber
                                      .Contains(miscode.Substring(0, 2) + miscode.Substring(miscode.Length - 4) + CmnClass.FeePaymentReceiptText.DocVerify + curyear)
                                      ).Select(x => x.DocVeriFeeReceiptNumber).ToList();
            }

            string ToChkExistingRec = "";
            if (InstituteReceiptNum.Count > 0)
            {
                foreach (var ExistingAssignedReceiptNum in InstituteReceiptNum)
                {
                    ToChkExistingRec = ExistingAssignedReceiptNum;
                }
            }

            string ToGenCheckDigits = ""; int ToGenCheckDigits1 = 1;

            if (ToChkExistingRec != "")
            {
                ToChkExistingRec = InstituteReceiptNum.Max();
                ToChkExistingRec = ToChkExistingRec.Substring(8, 4).ToString();
                if (ToChkExistingRec.Length != 4)
                {
                    ToChkExistingRec = ToGenCheckDigits.PadLeft(4, '0');
                }
                ToGenCheckDigits1 = Int32.Parse(ToChkExistingRec);
                ToGenCheckDigits1++;
            }
            ToChkExistingRec = ToGenCheckDigits1.ToString().PadLeft(4, '0');

            string receiptNum = "";
            if(model.MISCode!=null)
            {
                receiptNum = model.MISCode.Substring(0, 2) + model.MISCode.Substring(model.MISCode.Length - 4);
                receiptNum = receiptNum + CmnClass.FeePaymentReceiptText.Admission;
            }
            else
            {
                receiptNum = miscode.Substring(0, 2) + miscode.Substring(miscode.Length - 4);
                receiptNum = receiptNum + CmnClass.FeePaymentReceiptText.DocVerify;
            }
            
            receiptNum = receiptNum + curyear;
            
            receiptNum = receiptNum + ToChkExistingRec;
            return receiptNum;
        }

        private string GetRollNumber(InstituteWiseAdmission model, int DivisionId)
        {
            int NumOfChars = 3;
            model.DistrictId = Convert.ToInt32(_db.tbl_Applicant_Detail.Where(a => a.ApplicationId == model.ApplicationId).Select(a => a.DistrictId).Take(1).FirstOrDefault());
            string rollNum = "NIL";
            if (model.CourseType == (int)CmnClass.courseType.SCVT)
            {
                rollNum = Convert.ToString(DivisionId);
                rollNum = rollNum + model.DistrictId;

                rollNum = rollNum + model.MISCode.Substring(0, NumOfChars);
                rollNum = rollNum + model.MISCode.Substring(model.MISCode.Length - NumOfChars);
                rollNum = rollNum + model.TradeName.Substring(0, 2);

                var lstRollNum = _db.tbl_Applicant_ITI_Institute_Detail.Where(m => m.RollNumber
                             .Contains(rollNum) && m.IsActive == true).Select(x => x.ReceiptNumber).ToList();

                string ToChkExistingRec = "";
                if (lstRollNum.Count > 0)
                {
                    foreach (var ExistingAssignedRollNum in lstRollNum)
                    {
                        ToChkExistingRec = ExistingAssignedRollNum;
                    }
                }

                string ToGenCheckDigits = ""; int ToGenCheckDigits1 = 1;

                if (ToChkExistingRec != "")
                {
                    ToChkExistingRec = lstRollNum.Max();
                    ToChkExistingRec = ToChkExistingRec.Substring(8, 4).ToString();
                    if (ToChkExistingRec.Length != 4)
                    {
                        ToChkExistingRec = ToGenCheckDigits.PadLeft(NumOfChars, '0');
                    }
                    ToGenCheckDigits1 = Int32.Parse(ToChkExistingRec);
                    ToGenCheckDigits1++;
                }
                ToChkExistingRec = ToGenCheckDigits1.ToString().PadLeft(NumOfChars, '0');

                rollNum = rollNum + ToChkExistingRec;

            }
            return rollNum;
        }


        public ApplicantApplicationForm GetValidateRDNumberDll(string RD_Number, int loginId, int RDNumberType)
        {
            ApplicantApplicationForm result = new ApplicantApplicationForm();
            //var res = (from a in _db.tbl_Applicant_Detail where a.CredatedBy == loginId && a.Caste_Categaory_Income_RD == RD_Number && a.IsActive == true select a).FirstOrDefault();
            //result.Caste_RD_No = res.Caste_Categaory_Income_RD;
            //result.EconomicWeaker_RD_No = res.Economic_Weaker_Section_RD;
            //result.HYD_Karnataka_RD_No = res.Hyderabada_Karnataka_Region_RD;
            //result.ApplicantNumber = res.ApplicantNumber;

            if (RDNumberType == 0)
            {
                var res = (from a in _db.tbl_Applicant_Detail where a.CredatedBy == loginId  && a.IsActive == true select a).FirstOrDefault();
                
                if (res!=null)
                {
                    result.ApplicantNumber = res.ApplicantNumber;
                }
            }

            if (RDNumberType==1)
            {
                var res = (from a in _db.tbl_Applicant_Detail where a.CredatedBy == loginId && a.Caste_Categaory_Income_RD == RD_Number && a.IsActive == true select a).FirstOrDefault();
            }
            if (RDNumberType == 2)
            {
                var res = (from a in _db.tbl_Applicant_Detail where a.CredatedBy == loginId && a.Economic_Weaker_Section_RD == RD_Number && a.IsActive == true select a).FirstOrDefault();
            }
            if (RDNumberType == 3)
            {
                var res = (from a in _db.tbl_Applicant_Detail where a.CredatedBy == loginId && a.Hyderabada_Karnataka_Region_RD == RD_Number && a.IsActive == true select a).FirstOrDefault();
            }

            return result;
        }

    }
}
