using DLL.DBConnection;
using Models.Admission;
using Models.Master;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Models.SMS;
using DLL.Common;
using Models;

namespace DLL.Admission.Grievances
{
    public class Grievances : IGrievances
    {
        private readonly DbConnection _db = new DbConnection();

        public List<VerificationOfficer> ApplicantRankDetails(int loginId, int roleId)
        {
            try
            {
                var res = (from aa in _db.tbl_Applicant_Detail
                           join bb in _db.tbl_Gender on aa.Gender equals bb.Gender_Id
                           join cc in _db.tbl_Category on aa.Category equals cc.CategoryId
                           join dd in _db.tbl_location_type on aa.ApplicantBelongTo equals dd.location_id
                           join ee in _db.tbl_GradationRank_Trans on aa.ApplicationId equals ee.ApplicantId
                           join tt in _db.tbl_VerOfficer_Applicant_Mapping on aa.ApplicationId equals tt.ApplicantId
                           //join vv in _db.tbl_Applicant_Reservation on aa.ApplicationId equals vv.ApplicantId
                           //join mm in _db.tbl_reservation on vv.ReservationId equals mm.ReservationId
                           join gg in _db.tbl_Result on aa.ResultQual equals gg.ResultId
                           join zz in _db.tbl_qualification on aa.Qualification equals zz.QualificationId
                           join xc in _db.tbl_GradationRank_TransHistory on aa.ApplicationId equals xc.ApplicantId
                           where aa.IsActive == true && aa.CredatedBy == loginId && ee.Tentative == true
                           select new VerificationOfficer
                           {
                               ApplicationId = aa.ApplicationId,
                               ApplicantNumber = aa.ApplicantNumber,
                               ApplicantName = aa.ApplicantName,
                               FatherName = aa.FathersName,
                               Gender = bb.Gender,
                               DOB = aa.DOB,
                               Category = cc.Category,
                               MaxMarks = aa.MaxMarks,
                               MarksObtained = aa.MarksObtained,
                               Result = gg.Result,
                               RuralUrban = dd.location_name,
                               Rank = xc.Rank,
                               DiffAbled = aa.PhysicallyHanidcapInd,
                               Percentage = aa.Percentage,
                               Qualification = zz.Qualification,
                               HydKar=aa.HyderabadKarnatakaRegion==true?"Yes":"No",
                               KannadaMedium=aa.KanndaMedium==true?"Yes":"No"
                           }
               ).Distinct().ToList();
                foreach (var itm in res)
                {
                    if (itm.DiffAbled == true)
                        itm.DiffrentAbled = "Yes";
                    else
                        itm.DiffrentAbled = "No";

                    var resid = (from qq in _db.tbl_Applicant_Reservation
                                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                                 where qq.ApplicantId == itm.ApplicationId && xx.ReservationId == 2
                                 select xx.ReservationId).FirstOrDefault();
                    if (resid != 0)
                        itm.ExService = "Yes";
                    else
                        itm.ExService = "No";

                    var ewsid = (from qq in _db.tbl_Applicant_Reservation
                                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                                 where qq.ApplicantId == itm.ApplicationId && xx.ReservationId == 5
                                 select xx.ReservationId).FirstOrDefault();
                    if (ewsid != 0)
                        itm.EconomicWeekerSec = "Yes";
                    else
                        itm.EconomicWeekerSec = "No";

                    itm.Weightage = itm.MarksObtained / 10;
                    decimal? weight = itm.MarksObtained + itm.Weightage;
                    if (weight > itm.MaxMarks)
                        itm.TotalMarks = itm.MaxMarks;
                    else
                        itm.TotalMarks = weight;
                }
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<VerificationOfficer> GetDocumentTypes()
        {
            try
            {
                var res = (from aa in _db.tbl_GrievanceCategory where aa.IsActive==true 
                           select new VerificationOfficer
                           {
                               docTypeId = aa.CategoryId,
                               docType = aa.CategoryName
                           }
                    ).ToList();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string SubmitGrievanceTentative(List<HttpPostedFileBase> list, List<int> fileType, int loginId, string remarks, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //string applicant_name = _db.tbl_user_master.Where(x => x.um_id == loginId).Select(y => y.um_name).FirstOrDefault();
                    //string substr = applicant_name.Substring(0, 2);
                    var doc = _db.tbl_Applicant_Detail.Where(x => x.CredatedBy == loginId).FirstOrDefault();
                    string substr = doc.ApplicantName.Substring(0, 2);
                    string grivance_no = substr + doc.ApplicantNumber.Substring(doc.ApplicantNumber.Length - 4);
                    string itiCollegeName = _db.tbl_iti_college_details.Where(x => x.iti_college_id == doc.DocVerificationCentre).Select(a => a.iti_college_name).FirstOrDefault();
                    if (fileType.Count == list.Count && fileType.Count != 0)
                    {
                        tbl_Grievance gr = new tbl_Grievance();
                        gr.ApplicationId = doc.ApplicationId;
                        gr.Status = 11;
                        gr.Remarks = remarks;
                        gr.IsActive = true;
                        gr.CreatedBy = loginId;
                        gr.CreatedOn = DateTime.Now;
                        gr.GrievanceRefNumber = grivance_no;
                        _db.tbl_Grievance.Add(gr);
                        _db.SaveChanges();

                        var gri = _db.tbl_Grievance.Where(x => x.CreatedBy == loginId).OrderByDescending(x => x.GrievanceId).Select(y => y.GrievanceId).Take(1).FirstOrDefault();
                        tbl_GrievanceHistory gh = new tbl_GrievanceHistory();
                        gh.GrievanceId = Convert.ToInt32(gri);
                        gh.ApplicationId = doc.ApplicationId;
                        gh.Status = 11;
                        gh.Remarks = remarks;
                        gh.IsActive = true;
                        gh.CreatedBy = roleId;
                        gh.CreatedOn = DateTime.Now;
                        _db.tbl_GrievanceHistory.Add(gh);

                        #region .. Updating in tbl_Applicant_Detail table ..

                        int tadapplicationId = gh.ApplicationId;
                        int ApplStatusToUpdate = 11;
                        var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == tadapplicationId && s.IsActive == true).FirstOrDefault();
                        update_query.ApplDescStatus = 9;                //Grievance Submitted
                        update_query.ApplStatus = ApplStatusToUpdate;

                        _db.SaveChanges();

                        var FlowIDExisting = (from tad in _db.tbl_ApplicantTrans
                                              where tad.ApplicantId == tadapplicationId && tad.IsActive == 1
                                              orderby tad.ApplicantTransId descending
                                              select new ApplicantTransDetails
                                              {
                                                  VerfOfficer = tad.VerfOfficer,
                                                  CreatedBy = tad.CreatedBy,
                                                  ReVerficationStatus = tad.ReVerficationStatus,
                                                  FinalSubmitInd = tad.FinalSubmitInd,
                                                  FlowId = tad.FlowId,
                                                  AssignedVO = tad.AssignedVO
                                              }).ToList();

                        tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                        objtbl_ApplicantTrans.ApplicantId = tadapplicationId;
                        objtbl_ApplicantTrans.Remark = gh.Remarks;
                        objtbl_ApplicantTrans.ApplDescStatus = 9;
                        objtbl_ApplicantTrans.Status = ApplStatusToUpdate;
                        objtbl_ApplicantTrans.CreatedBy = loginId;
                        if (FlowIDExisting.Count > 0)
                        {
                            foreach (var ExistingAssignedVOval in FlowIDExisting)
                            {
                                objtbl_ApplicantTrans.VerfOfficer = ExistingAssignedVOval.VerfOfficer;
                                objtbl_ApplicantTrans.ReVerficationStatus = ExistingAssignedVOval.ReVerficationStatus;
                                objtbl_ApplicantTrans.FinalSubmitInd = ExistingAssignedVOval.FinalSubmitInd;
                                objtbl_ApplicantTrans.FlowId = ExistingAssignedVOval.AssignedVO;
                                objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                                objtbl_ApplicantTrans.FinalSubmitInd = 1;
                                break;
                            }
                        }

                        objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                        objtbl_ApplicantTrans.TransDate = DateTime.Now;
                        objtbl_ApplicantTrans.IsActive = 1;
                        _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                        _db.SaveChanges();


                        #endregion

                        foreach (var ss in fileType.Zip(list, (filetype, file) => new { filetype, file }))
                        {
                            _db.SaveChanges();
                            if (ss.file != null)
                            {
                                string documentfolder = System.Web.HttpContext.Current.Server.MapPath(" ");
                                string pathstr = string.Concat(documentfolder.Reverse().Skip(10).Reverse()) + "//Content//Uploads//ApplicantGrievance//";
                                var uploadRootFolderInput = pathstr;
                                string UniqueFileName = null;
                                var extension = System.IO.Path.GetExtension(ss.file.FileName).Substring(1);
                                UniqueFileName = System.IO.Path.GetFileNameWithoutExtension(ss.file.FileName) + "_" + Guid.NewGuid().ToString() + "." + extension;
                                if (!Directory.Exists(uploadRootFolderInput))
                                {
                                    Directory.CreateDirectory(uploadRootFolderInput);
                                }
                                string path = Path.Combine(uploadRootFolderInput, Path.GetFileName(UniqueFileName));
                                ss.file.SaveAs(path);
                                string loc = "/Content/Uploads/ApplicantGrievance/" + UniqueFileName;
                                if (doc != null)
                                {
                                    tbl_GrievanceDoc gd = new tbl_GrievanceDoc();
                                    gd.GrievanceId = Convert.ToInt32(gri);
                                    gd.DocFileName = ss.file.FileName;
                                    gd.DocPath = loc;
                                    gd.CatTypeId = ss.filetype;
                                    gd.CreatedBy = loginId;
                                    gd.CreatedOn = DateTime.Now;
                                    gd.IsActive = true;
                                    _db.tbl_GrievanceDoc.Add(gd);
                                }
                            }
                            else
                            {
                                return "failed";
                            }
                        }
                        _db.SaveChanges();
                        transaction.Commit();
                   
                        string templateid = WebConfigurationManager.AppSettings["NewApplicantOTPRegistration"];
                        var OTPSuccuessFailure = SMSHttpPostClient.SendOTPMSG(Convert.ToString(doc.PhoneNumber), string.Format(CmnClass.EmailAndMobileMsgs.EmailGRVSubjectMOB, doc.ApplicantName,grivance_no), templateid);
                        //smsresponse = _CommonBLL.SendSMSBLL(TOMobileNumber, string.Format(Message, objApplicant.Name, Session["generatedMobileOTP"].ToString()));
                        //smsresponse = sendSMS(TOMobileNumber, string.Format(Message, objApplicant.Name, Session["generatedMobileOTP"].ToString()));
                        var emailresponse = true;
                        CommonDLL cd = new CommonDLL();
                        emailresponse = cd.SendEmailDLL(doc.EmailId, "Grievance Confirmation", string.Format(CmnClass.EmailAndMobileMsgs.EmailGRVSubjectMOB,doc.ApplicantName ,grivance_no));


                        return grivance_no + " - " + itiCollegeName;
                    }
                    else
                    {
                        return "failed";
                    }


                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
        public List<VerificationOfficer> GetGrievanceTentativeStatus(int loginId, int roleId, int course, int year, int division, int district, int applicantType, int taluk, int institute)
        {
            try
            {
                int AcademicYearReviewId = 0;
                if (year!=0)
                {
                    string yr = _db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault();
                    yr = yr.Split('-')[1];
                     AcademicYearReviewId = Convert.ToInt32(yr);
                }
                
                if (roleId == 10)
                {
                    var res = (from aa in _db.tbl_Grievance
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicationId equals bb.ApplicationId
                               join cc in _db.tbl_GradationRank_Trans on aa.ApplicationId equals cc.ApplicantId
                               join dd in _db.tbl_status_master on aa.Status equals dd.StatusId
                               join vv in _db.tbl_iti_college_details on bb.DocVerificationCentre equals vv.iti_college_id
                               join zz in _db.tbl_course_type_mast on vv.CourseCode equals zz.course_id
                               where aa.CreatedBy == loginId && cc.Tentative == true
                               select new VerificationOfficer
                               {
                                   ApplicationId = bb.ApplicationId,
                                   GrievanceId = aa.GrievanceId,
                                   ApplicantNumber = bb.ApplicantNumber,
                                   ApplicantName = bb.ApplicantName,
                                   Rank = cc.Rank,
                                   StatusId = aa.Status,
                                   StatusName = dd.StatusName,
                                   Remarks = aa.Remarks,
                                   RoleId = roleId,
                                   CourseType = zz.course_type_name,
                                   Year = bb.ApplyYear,
                                   GrievanceRefNumber = aa.GrievanceRefNumber
                               }).ToList();

                    var from_date = _db.tbl_Tentative_admsn_eventDetails.OrderByDescending(y => y.Calendar_EventId).Select(x => x.FromDt_DocVerificationPeriod).FirstOrDefault();
                    var to_date = _db.tbl_Tentative_admsn_eventDetails.OrderByDescending(y => y.Calendar_EventId).Select(x => x.ToDt_DocVerificationPeriod).FirstOrDefault();
                    if (from_date != null)
                    {
                        DateTime from_date1 = (DateTime)from_date;
                        DateTime to_date1 = (DateTime)to_date;
                        foreach (var item in res)
                        {
                            item.From = from_date1.ToString("yyyy,MM,d");
                            item.To = to_date1.ToString("yyyy,MM,d");
                        }
                    }


                    return res;
                }
                else if (roleId == 12)
                {
                    if (course == 0)
                    {
                        var res = (from aa in _db.tbl_Grievance
                                   join bb in _db.tbl_Applicant_Detail on aa.ApplicationId equals bb.ApplicationId
                                   join cc in _db.tbl_GradationRank_Trans on aa.ApplicationId equals cc.ApplicantId
                                   join dd in _db.tbl_status_master on aa.Status equals dd.StatusId
                                   join ee in _db.tbl_VerOfficer_Applicant_Mapping on aa.ApplicationId equals ee.ApplicantId
                                   join ff in _db.tbl_VerificationOfficer_Master on ee.VerifiedOfficer equals ff.Officer_Id
                                   join vv in _db.tbl_iti_college_details on bb.DocVerificationCentre equals vv.iti_college_id
                                   join zz in _db.tbl_course_type_mast on vv.CourseCode equals zz.course_id
                                   where aa.IsActive == true && ff.UserMasterId == loginId
                                   select new VerificationOfficer
                                   {
                                       ApplicationId = bb.ApplicationId,
                                       GrievanceId = aa.GrievanceId,
                                       ApplicantNumber = bb.ApplicantNumber,
                                       ApplicantName = bb.ApplicantName,
                                       Rank = cc.Rank,
                                       StatusId = aa.Status,
                                       //StatusName = aa.Status == 11 ? "Grievance Review Pending" : dd.StatusName,
                                       StatusName = dd.StatusName,
                                       Remarks = aa.Remarks,
                                       RoleId = roleId,
                                       CourseType = zz.course_type_name,
                                       Year = bb.ApplyYear,
                                       GrievanceRefNumber = aa.GrievanceRefNumber
                                   }).ToList();
                        return res;
                    }
                    else
                    {
                        var res = (from aa in _db.tbl_Grievance
                                   join bb in _db.tbl_Applicant_Detail on aa.ApplicationId equals bb.ApplicationId
                                   join cc in _db.tbl_GradationRank_Trans on aa.ApplicationId equals cc.ApplicantId
                                   join dd in _db.tbl_status_master on aa.Status equals dd.StatusId
                                   join ee in _db.tbl_VerOfficer_Applicant_Mapping on aa.ApplicationId equals ee.ApplicantId
                                   join ff in _db.tbl_VerificationOfficer_Master on ee.VerifiedOfficer equals ff.Officer_Id
                                   join vv in _db.tbl_iti_college_details on bb.DocVerificationCentre equals vv.iti_college_id
                                   join zz in _db.tbl_course_type_mast on vv.CourseCode equals zz.course_id
                                   where aa.IsActive == true && ff.UserMasterId == loginId && bb.ApplyYear == year && vv.CourseCode == course && bb.ApplicantType == applicantType
                                   select new VerificationOfficer
                                   {
                                       ApplicationId = bb.ApplicationId,
                                       GrievanceId = aa.GrievanceId,
                                       ApplicantNumber = bb.ApplicantNumber,
                                       ApplicantName = bb.ApplicantName,
                                       Rank = cc.Rank,
                                       StatusId = aa.Status,
                                       //StatusName = aa.Status == 11 ? "Grievance Review Pending" : dd.StatusName,
                                       StatusName = dd.StatusName,
                                       Remarks = aa.Remarks,
                                       RoleId = roleId,
                                       CourseType = zz.course_type_name,
                                       Year = bb.ApplyYear,
                                       GrievanceRefNumber = aa.GrievanceRefNumber
                                   }).ToList();
                        return res;
                    }
                }
                else
                {
                    if (applicantType == 0 && division == 0)
                    {
                        var res = (from aa in _db.tbl_Grievance
                                   join bb in _db.tbl_Applicant_Detail on aa.ApplicationId equals bb.ApplicationId
                                   join cc in _db.tbl_GradationRank_Trans on aa.ApplicationId equals cc.ApplicantId
                                   join dd in _db.tbl_status_master on aa.Status equals dd.StatusId
                                   join vv in _db.tbl_iti_college_details on bb.DocVerificationCentre equals vv.iti_college_id
                                   join zz in _db.tbl_course_type_mast on vv.CourseCode equals zz.course_id
                                   join ve in _db.tbl_VerificationOfficer_Master on bb.AssignedVO equals ve.UserMasterId                                  
                                   join dis in _db.tbl_district_master on bb.DistrictId equals dis.district_lgd_code
                                   join div in _db.tbl_division_master on vv.division_id equals div.division_id
                                   join tlk in _db.tbl_taluk_master on vv.taluk_id equals tlk.taluk_lgd_code
                                   select new VerificationOfficer
                                   {
                                       ApplicationId = bb.ApplicationId,
                                       GrievanceId = aa.GrievanceId,
                                       ApplicantNumber = bb.ApplicantNumber,
                                       ApplicantName = bb.ApplicantName,
                                       Rank = cc.Rank,
                                       //StatusName = aa.Status == 11 ? "Grievance Review Pending" : dd.StatusName,
                                       StatusName = dd.StatusName,
                                       Remarks = aa.Remarks,
                                       RoleId = roleId,
                                       CourseType = zz.course_type_name,
                                       Year = bb.ApplyYear,
                                       GrievanceRefNumber = aa.GrievanceRefNumber,
                                       OfficerName = ve.Name,
                                       InstituteName = vv.iti_college_name,
                                       Districtname=dis.district_ename,
                                       Divisionname=div.division_name,
                                       Talukname=tlk.taluk_ename
                                       
                                   }).ToList();
                        return res;
                    }
                    else
                    {

                        var res = (from aa in _db.tbl_Grievance
                                   join bb in _db.tbl_Applicant_Detail on aa.ApplicationId equals bb.ApplicationId
                                   join cc in _db.tbl_GradationRank_Trans on aa.ApplicationId equals cc.ApplicantId
                                   join dd in _db.tbl_status_master on aa.Status equals dd.StatusId
                                   join vv in _db.tbl_iti_college_details on bb.DocVerificationCentre equals vv.iti_college_id
                                   join zz in _db.tbl_course_type_mast on vv.CourseCode equals zz.course_id
                                   join ve in _db.tbl_VerificationOfficer_Master on bb.AssignedVO equals ve.UserMasterId
                                   join dis in _db.tbl_district_master on vv.district_id equals dis.district_lgd_code
                                   join div in _db.tbl_division_master on vv.division_id equals div.division_id
                                   join tlk in _db.tbl_taluk_master on vv.taluk_id equals tlk.taluk_lgd_code
                                   where (division != 0 ? vv.division_id == division : true) && (AcademicYearReviewId!=0 ?  bb.ApplyYear == AcademicYearReviewId:true) 
                                   && (applicantType!=0 ?  bb.ApplicantType == applicantType:true) && (district !=0 ?vv.district_id == district:true) 
                                   && (taluk!=0? vv.taluk_id == taluk:true) && (institute!=0 ? bb.DocVerificationCentre==institute:true) && aa.Status==13
                                   select new VerificationOfficer
                                   {
                                       ApplicationId = bb.ApplicationId,
                                       GrievanceId = aa.GrievanceId,
                                       ApplicantNumber = bb.ApplicantNumber,
                                       ApplicantName = bb.ApplicantName,
                                       Rank = cc.Rank,
                                       //StatusName = aa.Status == 11 ? "Grievance Review Pending" : dd.StatusName,
                                       StatusName = dd.StatusName,
                                       Remarks = aa.Remarks,
                                       RoleId = roleId,
                                       CourseType = zz.course_type_name,
                                       Year = bb.ApplyYear,
                                       GrievanceRefNumber = aa.GrievanceRefNumber,
                                       division_id=vv.division_id,
                                       taluk_lgd_code= bb.TalukaId,
                                       district_lgd_code= bb.DistrictId,
                                       InstituteId=vv.iti_college_id,
                                       StatusId=aa.Status,
                                       ApplicantType=bb.ApplicantType,
                                       InstituteName=vv.iti_college_name,
                                       Districtname=dis.district_ename,
                                       Divisionname=div.division_name,
                                       OfficerName=ve.Name,
                                       Talukname=tlk.taluk_ename
                                   }).ToList();
                        return res;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public VerificationOfficer EditApplicantGrievance(int grivanceId, int roleId)
        {
            try
            {
                var rej = _db.tbl_Grievance.Where(x => x.GrievanceId == grivanceId && x.Status == 3).FirstOrDefault();
                if (rej != null)
                {
                    var res = (from aa in _db.tbl_Grievance
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicationId equals bb.ApplicationId
                               join cc in _db.tbl_GradationRank_Trans on aa.ApplicationId equals cc.ApplicantId
                               join dd in _db.tbl_status_master on aa.Status equals dd.StatusId
                               join ee in _db.tbl_GrievanceDoc on aa.GrievanceId equals ee.GrievanceId
                               join vv in _db.tbl_iti_college_details on bb.DocVerificationCentre equals vv.iti_college_id
                               join zz in _db.tbl_course_type_mast on vv.CourseCode equals zz.course_id
                               join ff in _db.tbl_Gender on bb.Gender equals ff.Gender_Id
                               join gg in _db.tbl_Category on bb.Category equals gg.CategoryId
                               join hh in _db.tbl_Result on bb.ResultQual equals hh.ResultId
                               join ii in _db.tbl_location_type on bb.ApplicantBelongTo equals ii.location_id
                               join jj in _db.tbl_qualification on bb.Qualification equals jj.QualificationId
                               where aa.GrievanceId == grivanceId && ee.IsActive == false
                               select new VerificationOfficer
                               {
                                   ApplicationId = bb.ApplicationId,
                                   GrievanceRefNumber = aa.GrievanceRefNumber,
                                   GrievanceId = aa.GrievanceId,
                                   ApplicantNumber = bb.ApplicantNumber,
                                   ApplicantName = bb.ApplicantName,
                                   Rank = cc.Rank,
                                   StatusId = aa.Status,
                                   FatherName = bb.FathersName,
                                   Gender = ff.Gender,
                                   StatusName = dd.StatusName,
                                   Remarks = aa.Remarks,
                                   DOB = bb.DOB,
                                   Category = gg.Category,
                                   CourseType = zz.course_type_name,
                                   Year = bb.ApplyYear,
                                   Result = hh.Result,
                                   MaxMarks = bb.MaxMarks,
                                   MarksObtained = bb.MarksObtained,
                                   RuralUrban = ii.location_name,
                                   DiffAbled = bb.PhysicallyHanidcapInd,
                                   Percentage = bb.Percentage,
                                   Qualification = jj.Qualification,
                                   HydKar=bb.HyderabadKarnatakaRegion==true?"Yes":"No",
                                   KannadaMedium = bb.KanndaMedium==true?"Yes":"No"
                               }).FirstOrDefault();
                    if (res != null)
                    {
                        if (res.DiffAbled == true)
                            res.DiffrentAbled = "Yes";
                        else
                            res.DiffrentAbled = "No";

                        var resid = (from qq in _db.tbl_Applicant_Reservation
                                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                                     where qq.ApplicantId == res.ApplicationId && xx.ReservationId == 2
                                     select xx.ReservationId).FirstOrDefault();
                        if (resid != 0)
                            res.ExService = "Yes";
                        else
                            res.ExService = "No";

                        var ewsid = (from qq in _db.tbl_Applicant_Reservation
                                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                                     where qq.ApplicantId == res.ApplicationId && xx.ReservationId == 5
                                     select xx.ReservationId).FirstOrDefault();
                        if (ewsid != 0)
                            res.EconomicWeekerSec = "Yes";
                        else
                            res.EconomicWeekerSec = "No";

                        res.Weightage = res.MarksObtained / 10;
                        decimal? weight = res.MarksObtained + res.Weightage;
                        if (weight > res.MaxMarks)
                            res.TotalMarks = res.MaxMarks;
                        else
                            res.TotalMarks = weight;
                    }
                    res.RoleId = roleId;
                    res.FileNames = _db.tbl_GrievanceDoc.Where(x => x.GrievanceId == res.GrievanceId).Select(y => y.DocFileName).ToList();
                    res.Files = _db.tbl_GrievanceDoc.Where(x => x.GrievanceId == res.GrievanceId).Select(y => y.DocPath).ToList();
                    res.FileStatus = _db.tbl_GrievanceDoc.Where(x => x.GrievanceId == res.GrievanceId).Select(y => y.Status).ToList();
                    res.FileTypes = _db.tbl_GrievanceDoc.Where(x => x.GrievanceId == res.GrievanceId).Select(y => y.CatTypeId).ToList();
                    //res.DocNames = _db.tbl_GrievanceDoc.Join(_db.tbl_GrievanceCategory, x => x.CatTypeId, y => y.CategoryId, (x, y) => new { y.CategoryName }).Select(a => a.CategoryName).ToList();
                    res.DocNames = (from ab in _db.tbl_GrievanceDoc
                                    join bc in _db.tbl_GrievanceCategory on ab.CatTypeId equals bc.CategoryId
                                    where ab.GrievanceId == grivanceId
                                    select bc.CategoryName).ToList();
                    res.Doctypes = (from aa in _db.tbl_GrievanceCategory where aa.IsActive==true
                                    select new Filetypes
                                    {
                                        DocTypeId = aa.CategoryId,
                                        DoctypeName = aa.CategoryName
                                    }
                        ).ToList();
                    return res;
                }
                else
                {
                    var res = (from aa in _db.tbl_Grievance
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicationId equals bb.ApplicationId
                               join cc in _db.tbl_GradationRank_Trans on aa.ApplicationId equals cc.ApplicantId
                               join dd in _db.tbl_status_master on aa.Status equals dd.StatusId
                               join ee in _db.tbl_GrievanceDoc on aa.GrievanceId equals ee.GrievanceId
                               join vv in _db.tbl_iti_college_details on bb.DocVerificationCentre equals vv.iti_college_id
                               join zz in _db.tbl_course_type_mast on vv.CourseCode equals zz.course_id
                               join ff in _db.tbl_Gender on bb.Gender equals ff.Gender_Id
                               join gg in _db.tbl_Category on bb.Category equals gg.CategoryId
                               join hh in _db.tbl_Result on bb.ResultQual equals hh.ResultId
                               join ii in _db.tbl_location_type on bb.ApplicantBelongTo equals ii.location_id
                               join jj in _db.tbl_qualification on bb.Qualification equals jj.QualificationId
                               where aa.GrievanceId == grivanceId && ee.IsActive == true
                               select new VerificationOfficer
                               {
                                   ApplicationId = bb.ApplicationId,
                                   GrievanceRefNumber = aa.GrievanceRefNumber,
                                   GrievanceId = aa.GrievanceId,
                                   ApplicantNumber = bb.ApplicantNumber,
                                   ApplicantName = bb.ApplicantName,
                                   Rank = cc.Rank,
                                   StatusId = aa.Status,
                                   FatherName = bb.FathersName,
                                   Gender = ff.Gender,
                                   StatusName = dd.StatusName,
                                   Remarks = aa.Remarks,
                                   DOB = bb.DOB,
                                   Category = gg.Category,
                                   CourseType = zz.course_type_name,
                                   Year = bb.ApplyYear,
                                   Result = hh.Result,
                                   MaxMarks = bb.MaxMarks,
                                   MarksObtained = bb.MarksObtained,
                                   RuralUrban = ii.location_name,
                                   DiffAbled = bb.PhysicallyHanidcapInd,
                                   Percentage = bb.Percentage,
                                   Qualification = jj.Qualification,
                                   HydKar = bb.HyderabadKarnatakaRegion == true ? "Yes" : "No",
                                   KannadaMedium = bb.KanndaMedium == true ? "Yes" : "No"
                               }
                    ).FirstOrDefault();
                    if (res != null)
                    {
                        if (res.DiffAbled == true)
                            res.DiffrentAbled = "Yes";
                        else
                            res.DiffrentAbled = "No";

                        var resid = (from qq in _db.tbl_Applicant_Reservation
                                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                                     where qq.ApplicantId == res.ApplicationId && xx.ReservationId == 2
                                     select xx.ReservationId).FirstOrDefault();
                        if (resid != 0)
                            res.ExService = "Yes";
                        else
                            res.ExService = "No";

                        var ewsid = (from qq in _db.tbl_Applicant_Reservation
                                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                                     where qq.ApplicantId == res.ApplicationId && xx.ReservationId == 5
                                     select xx.ReservationId).FirstOrDefault();
                        if (ewsid != 0)
                            res.EconomicWeekerSec = "Yes";
                        else
                            res.EconomicWeekerSec = "No";

                        res.Weightage = res.MarksObtained / 10;
                        decimal? weight = res.MarksObtained + res.Weightage;
                        if (weight > res.MaxMarks)
                            res.TotalMarks = res.MaxMarks;
                        else
                            res.TotalMarks = weight;
                    }

                    res.RoleId = roleId;
                    res.FileNames = _db.tbl_GrievanceDoc.Where(x => x.GrievanceId == res.GrievanceId).Select(y => y.DocFileName).ToList();
                    res.Files = _db.tbl_GrievanceDoc.Where(x => x.GrievanceId == res.GrievanceId).Select(y => y.DocPath).ToList();
                    res.FileStatus = _db.tbl_GrievanceDoc.Where(x => x.GrievanceId == res.GrievanceId).Select(y => y.Status).ToList();
                    res.FileTypes = _db.tbl_GrievanceDoc.Where(x => x.GrievanceId == res.GrievanceId).Select(y => y.CatTypeId).ToList();
                    //res.DocNames = _db.tbl_GrievanceDoc.Join(_db.tbl_GrievanceCategory, x => x.CatTypeId, y => y.CategoryId, (x, y) => new { y.CategoryName }).Select(a => a.CategoryName).ToList();
                    res.DocNames = (from ab in _db.tbl_GrievanceDoc
                                    join bc in _db.tbl_GrievanceCategory on ab.CatTypeId equals bc.CategoryId
                                    where ab.GrievanceId == grivanceId
                                    select bc.CategoryName).ToList();
                    res.Doctypes = (from aa in _db.tbl_GrievanceCategory where aa.IsActive==true
                                    select new Filetypes
                                    {
                                        DocTypeId = aa.CategoryId,
                                        DoctypeName = aa.CategoryName
                                    }
                        ).ToList();
                    return res;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool VerifyGrievance(List<int> fileType, List<string> status, int grievanceId, string remarks, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var res = _db.tbl_Grievance.Where(x => x.GrievanceId == grievanceId).FirstOrDefault();
                    var apl=_db.tbl_Applicant_Detail.Where(x => x.ApplicationId == res.ApplicationId).FirstOrDefault();
                    if (res != null)
                    {
                        foreach (var ss in fileType.Zip(status, (a, b) => new { filetype = a, Status = b }))
                        {
                            var rs = _db.tbl_GrievanceDoc.Where(x => x.GrievanceId == grievanceId && x.CatTypeId == ss.filetype).FirstOrDefault();
                            if (rs != null)
                            {
                                rs.Status = ss.Status;
                            }
                        }
                        res.Status = 13;
                        res.Remarks = remarks;
                        tbl_GrievanceHistory gh = new tbl_GrievanceHistory();
                        gh.GrievanceId = res.GrievanceId;
                        gh.ApplicationId = res.ApplicationId;
                        gh.Status = 13;
                        gh.Remarks = remarks;
                        gh.IsActive = true;
                        gh.CreatedBy = roleId;
                        gh.CreatedOn = DateTime.Now;
                        _db.tbl_GrievanceHistory.Add(gh);
                        _db.SaveChanges();

                        #region .. Updating in tbl_Applicant_Detail table .. 

                        var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == res.ApplicationId).FirstOrDefault();
                        update_query.ApplStatus = 13;       //Grievance Verified and Updated
                        update_query.ApplDescStatus = 10;
                        _db.SaveChanges();

                        var FlowIDExisting = (from tad in _db.tbl_ApplicantTrans
                                              where tad.ApplicantId == res.ApplicationId && tad.IsActive == 1
                                              orderby tad.ApplicantTransId descending
                                              select new ApplicantTransDetails
                                              {
                                                  VerfOfficer = tad.VerfOfficer,
                                                  CreatedBy = tad.CreatedBy,
                                                  ReVerficationStatus = tad.ReVerficationStatus,
                                                  FinalSubmitInd = tad.FinalSubmitInd,
                                                  FlowId = tad.FlowId,
                                                  AssignedVO = tad.AssignedVO
                                              }).ToList();

                        var CreatedByExisting = (from tad in _db.tbl_Applicant_Detail
                                                 where tad.ApplicationId == res.ApplicationId && tad.IsActive == true
                                                 select new ApplicantTransDetails
                                                 {
                                                     CreatedBy = tad.CredatedBy
                                                 }).ToList();

                        tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                        objtbl_ApplicantTrans.ApplicantId = res.ApplicationId;
                        objtbl_ApplicantTrans.Remark = gh.Remarks;
                        objtbl_ApplicantTrans.ApplDescStatus = 10;
                        objtbl_ApplicantTrans.Status = 13;
                        if (CreatedByExisting.Count > 0)
                        {
                            foreach (var CreatedByExistingVOval in CreatedByExisting)
                            {
                                objtbl_ApplicantTrans.FlowId = CreatedByExistingVOval.CreatedBy;
                            }
                        }
                        objtbl_ApplicantTrans.CreatedBy = loginId;
                        if (FlowIDExisting.Count > 0)
                        {
                            foreach (var ExistingAssignedVOval in FlowIDExisting)
                            {
                                objtbl_ApplicantTrans.VerfOfficer = ExistingAssignedVOval.VerfOfficer;
                                objtbl_ApplicantTrans.ReVerficationStatus = ExistingAssignedVOval.ReVerficationStatus;
                                objtbl_ApplicantTrans.FinalSubmitInd = ExistingAssignedVOval.FinalSubmitInd;
                                objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                                objtbl_ApplicantTrans.FinalSubmitInd = 1;
                                break;
                            }
                        }
                        objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                        objtbl_ApplicantTrans.TransDate = DateTime.Now;
                        objtbl_ApplicantTrans.IsActive = 1;

                        _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                        _db.SaveChanges();

                        #endregion

                        transaction.Commit();

                        //string templateid = WebConfigurationManager.AppSettings["NewApplicantOTPRegistration"];
                        //var OTPSuccuessFailure = SMSHttpPostClient.SendOTPMSG(Convert.ToString(apl.PhoneNumber), string.Format(CmnClass.EmailAndMobileMsgs.EmailGRVSubjectMOB, apl.ApplicantName), templateid);
                        ////smsresponse = _CommonBLL.SendSMSBLL(TOMobileNumber, string.Format(Message, objApplicant.Name, Session["generatedMobileOTP"].ToString()));
                        ////smsresponse = sendSMS(TOMobileNumber, string.Format(Message, objApplicant.Name, Session["generatedMobileOTP"].ToString()));
                        //var emailresponse = true;
                        //CommonDLL cd = new CommonDLL();
                        //emailresponse = cd.SendEmailDLL(apl.EmailId, "Grievance Confirmation", string.Format(CmnClass.EmailAndMobileMsgs.EmailGRVSubjectMOB, apl.ApplicantName));
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
        public bool SendForCorrection(List<int> fileType, List<string> status, int grievanceId, string remarks, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var res = _db.tbl_Grievance.Where(x => x.GrievanceId == grievanceId).FirstOrDefault();
                    if (res != null)
                    {
                        foreach (var ss in fileType.Zip(status, (a, b) => new { filetype = a, Status = b }))
                        {
                            var rs = _db.tbl_GrievanceDoc.Where(x => x.GrievanceId == grievanceId && x.CatTypeId == ss.filetype).FirstOrDefault();
                            if (rs != null)
                            {
                                rs.Status = ss.Status;
                            }
                        }
                        res.Status = 12;
                        res.Remarks = remarks;
                        tbl_GrievanceHistory gh = new tbl_GrievanceHistory();
                        gh.GrievanceId = res.GrievanceId;
                        gh.ApplicationId = res.ApplicationId;
                        gh.Status = 12;
                        gh.Remarks = remarks;
                        gh.IsActive = true;
                        gh.CreatedBy = roleId;
                        gh.CreatedOn = DateTime.Now;
                        _db.tbl_GrievanceHistory.Add(gh);
                        _db.SaveChanges();


                        #region .. Updating in tbl_Applicant_Detail table .. 

                        var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == gh.ApplicationId).FirstOrDefault();
                        update_query.ApplDescStatus = 17;
                        update_query.ApplStatus = 12;        //Grievance Sent for Correction
                        update_query.ApplRemarks = gh.Remarks;
                        _db.SaveChanges();

                        var FlowIDExisting = (from tad in _db.tbl_ApplicantTrans
                                              where tad.ApplicantId == gh.ApplicationId && tad.IsActive == 1
                                              orderby tad.ApplicantTransId descending
                                              select new ApplicantTransDetails
                                              {
                                                  VerfOfficer = tad.VerfOfficer,
                                                  CreatedBy = tad.CreatedBy,
                                                  ReVerficationStatus = tad.ReVerficationStatus,
                                                  FinalSubmitInd = tad.FinalSubmitInd,
                                                  FlowId = tad.FlowId,
                                                  AssignedVO = tad.AssignedVO
                                              }).ToList();

                        var CreatedByExisting = (from tad in _db.tbl_Applicant_Detail
                                                 where tad.ApplicationId == gh.ApplicationId && tad.IsActive == true
                                                 select new ApplicantTransDetails
                                                 {
                                                     CreatedBy = tad.CredatedBy
                                                 }).ToList();

                        tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                        objtbl_ApplicantTrans.ApplicantId = res.ApplicationId;
                        objtbl_ApplicantTrans.Remark = gh.Remarks;
                        objtbl_ApplicantTrans.FinalSubmitInd = 1;
                        objtbl_ApplicantTrans.ApplDescStatus = 17;
                        objtbl_ApplicantTrans.Status = 12;
                        if (CreatedByExisting.Count > 0)
                        {
                            foreach (var CreatedByval in CreatedByExisting)
                            {
                                objtbl_ApplicantTrans.FlowId = CreatedByval.CreatedBy;
                            }
                        }

                        objtbl_ApplicantTrans.CreatedBy = loginId;
                        if (FlowIDExisting.Count > 0)
                        {
                            foreach (var ExistingAssignedVOval in FlowIDExisting)
                            {
                                objtbl_ApplicantTrans.VerfOfficer = ExistingAssignedVOval.VerfOfficer;
                                objtbl_ApplicantTrans.ReVerficationStatus = ExistingAssignedVOval.ReVerficationStatus;
                                objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                                break;
                            }
                        }
                        objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                        objtbl_ApplicantTrans.TransDate = DateTime.Now;
                        objtbl_ApplicantTrans.IsActive = 1;

                        _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                        _db.SaveChanges();

                        #endregion


                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
        public bool UpdateGrievanceTentative(List<HttpPostedFileBase> list, List<int> fileType, List<string> status, int loginId, string remarks, int grievanceId, int roleId)
        {
            using (var transanction = _db.Database.BeginTransaction())
            {
                try
                {

                    var res = _db.tbl_Grievance.Where(x => x.GrievanceId == grievanceId).FirstOrDefault();

                    if (res != null && fileType != null)
                    {
                        //var gri = _db.tbl_Grievance.Where(x => x.CreatedBy == loginId).OrderByDescending(x => x.GrievanceId).Select(y => y.GrievanceId).Take(1).FirstOrDefault();
                        foreach (var ss in fileType.Zip(list, (a, b) => new { First = a, Second = b }).Zip(status, (a, b) => new { filetype = a.First, file = a.Second, Status = b }))
                        {
                            if (ss.file != null)
                            {
                                if (res.GrievanceId != 0)
                                {
                                    var rr = _db.tbl_GrievanceDoc.Where(x => x.CatTypeId == ss.filetype && x.GrievanceId == res.GrievanceId).FirstOrDefault();
                                    if (rr != null)
                                    {
                                        string documentfolder = System.Web.HttpContext.Current.Server.MapPath(" ");
                                        string pathstr = string.Concat(documentfolder.Reverse().Skip(10).Reverse()) + "//Content//Uploads//ApplicantGrievance//";
                                        var uploadRootFolderInput = pathstr;
                                        string UniqueFileName = null;
                                        var extension = System.IO.Path.GetExtension(ss.file.FileName).Substring(1);
                                        UniqueFileName = System.IO.Path.GetFileNameWithoutExtension(ss.file.FileName) + "_" + Guid.NewGuid().ToString() + "." + extension;
                                        if (!Directory.Exists(uploadRootFolderInput))

                                        {
                                            Directory.CreateDirectory(uploadRootFolderInput);
                                        }
                                        string path = Path.Combine(uploadRootFolderInput, Path.GetFileName(UniqueFileName));
                                        ss.file.SaveAs(path);
                                        string loc = "/Content/Uploads/ApplicantGrievance/" + UniqueFileName;
                                        rr.Status = ss.Status;
                                        rr.DocFileName = ss.file.FileName;
                                        rr.DocPath = loc;
                                        _db.SaveChanges();
                                    }
                                    else
                                    {
                                        string documentfolder = System.Web.HttpContext.Current.Server.MapPath(" ");
                                        string pathstr = string.Concat(documentfolder.Reverse().Skip(10).Reverse()) + "//Content//Uploads//ApplicantGrievance//";
                                        var uploadRootFolderInput = pathstr;
                                        string UniqueFileName = null;
                                        var extension = System.IO.Path.GetExtension(ss.file.FileName).Substring(1);
                                        UniqueFileName = System.IO.Path.GetFileNameWithoutExtension(ss.file.FileName) + "_" + Guid.NewGuid().ToString() + "." + extension;
                                        if (!Directory.Exists(uploadRootFolderInput))

                                        {
                                            Directory.CreateDirectory(uploadRootFolderInput);
                                        }
                                        string path = Path.Combine(uploadRootFolderInput, Path.GetFileName(UniqueFileName));
                                        ss.file.SaveAs(path);
                                        string loc = "/Content/Uploads/ApplicantGrievance/" + UniqueFileName;

                                        tbl_GrievanceDoc gd = new tbl_GrievanceDoc();
                                        gd.GrievanceId = Convert.ToInt32(res.GrievanceId);
                                        gd.DocFileName = ss.file.FileName;
                                        gd.DocPath = loc;
                                        gd.CatTypeId = ss.filetype;
                                        //gd.CreatedBy = loginId;
                                        gd.CreatedOn = DateTime.Now;
                                        _db.tbl_GrievanceDoc.Add(gd);
                                        //_db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        res.Remarks = remarks;
                        res.Status = 11;
                        tbl_GrievanceHistory gh = new tbl_GrievanceHistory();
                        gh.GrievanceId = res.GrievanceId;
                        gh.ApplicationId = res.ApplicationId;
                        gh.Status = 11;
                        gh.Remarks = remarks;
                        gh.IsActive = true;
                        gh.CreatedBy = roleId;
                        gh.CreatedOn = DateTime.Now;
                        _db.tbl_GrievanceHistory.Add(gh);
                        _db.SaveChanges();

                        #region .. Updating in tbl_Applicant_Detail table .. 

                        var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == res.ApplicationId).FirstOrDefault();
                        update_query.ApplDescStatus = 9;        //Grievance Raised
                        update_query.ApplStatus = 11;        //Grievance Raised
                        update_query.ApplRemarks = gh.Remarks;        //Grievance Raised
                        _db.SaveChanges();

                        var FlowIDExisting = (from tad in _db.tbl_ApplicantTrans
                                              where tad.ApplicantId == res.ApplicationId && tad.IsActive == 1
                                              orderby tad.ApplicantTransId descending
                                              select new ApplicantTransDetails
                                              {
                                                  VerfOfficer = tad.VerfOfficer,
                                                  CreatedBy = tad.CreatedBy,
                                                  ReVerficationStatus = tad.ReVerficationStatus,
                                                  FinalSubmitInd = tad.FinalSubmitInd,
                                                  FlowId = tad.FlowId,
                                                  AssignedVO = tad.AssignedVO
                                              }).ToList();

                        var CreatedByExisting = (from tad in _db.tbl_Applicant_Detail
                                                 where tad.ApplicationId == res.ApplicationId && tad.IsActive == true
                                                 select new ApplicantTransDetails
                                                 {
                                                     CreatedBy = tad.CredatedBy
                                                 }).ToList();

                        tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                        objtbl_ApplicantTrans.ApplicantId = res.ApplicationId;
                        objtbl_ApplicantTrans.Remark = gh.Remarks;
                        objtbl_ApplicantTrans.FinalSubmitInd = 1;
                        objtbl_ApplicantTrans.ApplDescStatus = 9;
                        objtbl_ApplicantTrans.Status = 11;
                        if (CreatedByExisting.Count > 0)
                        {
                            foreach (var CreatedByExistingVOval in CreatedByExisting)
                            {
                                objtbl_ApplicantTrans.CreatedBy = CreatedByExistingVOval.CreatedBy;
                            }
                        }
                        if (FlowIDExisting.Count > 0)
                        {
                            foreach (var ExistingAssignedVOval in FlowIDExisting)
                            {
                                objtbl_ApplicantTrans.VerfOfficer = ExistingAssignedVOval.VerfOfficer;
                                objtbl_ApplicantTrans.ReVerficationStatus = ExistingAssignedVOval.ReVerficationStatus;
                                objtbl_ApplicantTrans.FinalSubmitInd = ExistingAssignedVOval.FinalSubmitInd;
                                objtbl_ApplicantTrans.FlowId = ExistingAssignedVOval.AssignedVO;
                                objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                                break;
                            }
                        }
                        objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                        objtbl_ApplicantTrans.TransDate = DateTime.Now;
                        objtbl_ApplicantTrans.IsActive = 1;

                        _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                        _db.SaveChanges();

                        #endregion

                        transanction.Commit();
                        return true;
                    }
                    else if (res != null && fileType == null)
                    {
                        res.Remarks = remarks;
                        res.Status = 11;
                        tbl_GrievanceHistory gh = new tbl_GrievanceHistory();
                        gh.GrievanceId = res.GrievanceId;
                        gh.ApplicationId = res.ApplicationId;
                        gh.Status = 11;
                        gh.Remarks = remarks;
                        gh.IsActive = true;
                        gh.CreatedBy = roleId;
                        gh.CreatedOn = DateTime.Now;
                        _db.tbl_GrievanceHistory.Add(gh);
                        _db.SaveChanges();

                        #region .. Updating in tbl_Applicant_Detail table .. 

                        var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == res.ApplicationId).FirstOrDefault();
                        update_query.ApplDescStatus = 9;        //Grievance Raised
                        update_query.ApplStatus = 11;        //Grievance Raised
                        update_query.ApplRemarks = gh.Remarks;        //Grievance Raised
                        _db.SaveChanges();

                        var FlowIDExisting = (from tad in _db.tbl_ApplicantTrans
                                              where tad.ApplicantId == res.ApplicationId && tad.IsActive == 1
                                              orderby tad.ApplicantTransId descending
                                              select new ApplicantTransDetails
                                              {
                                                  VerfOfficer = tad.VerfOfficer,
                                                  CreatedBy = tad.CreatedBy,
                                                  ReVerficationStatus = tad.ReVerficationStatus,
                                                  FinalSubmitInd = tad.FinalSubmitInd,
                                                  FlowId = tad.FlowId,
                                                  AssignedVO = tad.AssignedVO
                                              }).ToList();

                        var CreatedByExisting = (from tad in _db.tbl_Applicant_Detail
                                                 where tad.ApplicationId == res.ApplicationId && tad.IsActive == true
                                                 select new ApplicantTransDetails
                                                 {
                                                     CreatedBy = tad.CredatedBy
                                                 }).ToList();

                        tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                        objtbl_ApplicantTrans.ApplicantId = res.ApplicationId;
                        objtbl_ApplicantTrans.Remark = gh.Remarks;
                        objtbl_ApplicantTrans.FinalSubmitInd = 1;
                        objtbl_ApplicantTrans.ApplDescStatus = 9;
                        objtbl_ApplicantTrans.Status = 11;
                        if (CreatedByExisting.Count > 0)
                        {
                            foreach (var CreatedByExistingVOval in CreatedByExisting)
                            {
                                objtbl_ApplicantTrans.CreatedBy = CreatedByExistingVOval.CreatedBy;
                            }
                        }
                        if (FlowIDExisting.Count > 0)
                        {
                            foreach (var ExistingAssignedVOval in FlowIDExisting)
                            {
                                objtbl_ApplicantTrans.VerfOfficer = ExistingAssignedVOval.VerfOfficer;
                                objtbl_ApplicantTrans.ReVerficationStatus = ExistingAssignedVOval.ReVerficationStatus;
                                objtbl_ApplicantTrans.FinalSubmitInd = ExistingAssignedVOval.FinalSubmitInd;
                                objtbl_ApplicantTrans.FlowId = ExistingAssignedVOval.AssignedVO;
                                objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                                break;
                            }
                        }
                        objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                        objtbl_ApplicantTrans.TransDate = DateTime.Now;
                        objtbl_ApplicantTrans.IsActive = 1;

                        _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                        _db.SaveChanges();

                        #endregion

                        transanction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception e)
                {
                    transanction.Rollback();
                    throw e;
                }
            }
        }
        public VerificationOfficer GetGrievanceGrid(int loginId)
        {
            try
            {
                var appId = _db.tbl_Applicant_Detail.Where(y => y.CredatedBy == loginId).Select(x => x.ApplicationId).FirstOrDefault();
                var ss = _db.tbl_GradationRank_Trans.Where(y => y.ApplicantId == appId && y.Tentative == true).FirstOrDefault();
                var rej = _db.tbl_Grievance.Where(x => x.CreatedBy == loginId && x.ApplicationId == appId).OrderByDescending(y => y.GrievanceId).FirstOrDefault();
                var ver = _db.tbl_VerOfficer_Applicant_Mapping.Where(x => x.ApplicantId == appId).Select(v => v.ApplicantId).FirstOrDefault();
                var se = new VerificationOfficer();
                //var from_date = _db.tbl_Tentative_admsn_eventDetails.OrderByDescending(y => y.Calendar_EventId).Select(x => x.FromDt_DocVerificationPeriod).FirstOrDefault();
                //var to_date = _db.tbl_Tentative_admsn_eventDetails.OrderByDescending(y => y.Calendar_EventId).Select(x => x.ToDt_DocVerificationPeriod).FirstOrDefault();
                var collegeId= _db.tbl_Applicant_Detail.Where(y => y.CredatedBy == loginId).Select(x => x.DocVerificationCentre).FirstOrDefault();
                var courseType=_db.tbl_iti_college_details.Where(x => x.iti_college_id == collegeId).Select(y => y.CourseCode).FirstOrDefault();
                var from_date = _db.tbl_Tentative_admsn_eventDetails.Join(_db.tbl_tentative_calendar_of_events,
                                ed => ed.Tentative_admsn_evnt_clndr_Id,
                                evn => evn.Tentative_admsn_evnt_clndr_Id,
                                (ed, evn) => new { event_detail = ed, events = evn })
                                .Where(ssa => ssa.events.StatusId == 106 && ssa.events.Course_Id== courseType).OrderByDescending(ssa => ssa.event_detail.Calendar_EventId)
                                .Select(ssa => ssa.event_detail.FromDt_DocVerificationPeriod).FirstOrDefault();

                var to_date = _db.tbl_Tentative_admsn_eventDetails.Join(_db.tbl_tentative_calendar_of_events,
                                ed => ed.Tentative_admsn_evnt_clndr_Id,
                                evn => evn.Tentative_admsn_evnt_clndr_Id,
                                (ed, evn) => new { event_detail = ed, events = evn })
                                .Where(ssa => ssa.events.StatusId == 106 && ssa.events.Course_Id == courseType ).OrderByDescending(ssa => ssa.event_detail.Calendar_EventId)
                                .Select(ssa => ssa.event_detail.ToDt_DocVerificationPeriod).FirstOrDefault();
               
                if (from_date != null)
                {
                    DateTime from_date1 = (DateTime)from_date;
                    DateTime to_date1 = (DateTime)to_date;
                    se.From = from_date1.ToString("yyyy,MM,d");
                    se.To = to_date1.ToString("yyyy,MM,d");
                }

                if (ver != 0)
                {
                    if (ss != null)
                    {
                        if (rej != null)
                        {
                            if (rej.IsActive == true)
                            {
                                se.Status = true;
                                return se;
                            }
                            else
                            {
                                se.Status = false;
                                return se;
                            }
                        }
                        else
                        {
                            se.Status = false;
                            return se;
                        }

                    }
                    else
                    {
                        se.Status = true;
                        return se;
                    }
                }
                else
                {
                    se.Status = true;
                    return se;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool RejectGrivance(int grivanceId, string remarks, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var res = _db.tbl_Grievance.Where(x => x.GrievanceId == grivanceId).FirstOrDefault();
                    var apl = _db.tbl_Applicant_Detail.Where(x => x.ApplicationId == res.ApplicationId).FirstOrDefault();
                    if (res != null)
                    {
                        res.Status = 3;
                        res.Remarks = remarks;
                        res.IsActive = false;

                        var ss = _db.tbl_GrievanceDoc.Where(x => x.GrievanceId == grivanceId).ToList();
                        foreach (var itm in ss)
                        {
                            itm.IsActive = false;
                        }
                        tbl_GrievanceHistory gh = new tbl_GrievanceHistory();
                        gh.GrievanceId = res.GrievanceId;
                        gh.ApplicationId = res.ApplicationId;
                        gh.Status = 3;
                        gh.Remarks = remarks;
                        gh.IsActive = true;
                        gh.CreatedBy = roleId;
                        gh.CreatedOn = DateTime.Now;
                        _db.tbl_GrievanceHistory.Add(gh);
                        _db.SaveChanges();

                        #region .. Updating in tbl_Applicant_Detail table .. 

                        var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == res.ApplicationId).FirstOrDefault();
                        update_query.ApplStatus = 3;
                        update_query.ApplDescStatus = 11;        //Grievance Rejected
                        _db.SaveChanges();

                        var FlowIDExisting = (from tad in _db.tbl_ApplicantTrans
                                              where tad.ApplicantId == res.ApplicationId && tad.IsActive == 1
                                              orderby tad.ApplicantTransId descending
                                              select new ApplicantTransDetails
                                              {
                                                  VerfOfficer = tad.VerfOfficer,
                                                  CreatedBy = tad.CreatedBy,
                                                  ReVerficationStatus = tad.ReVerficationStatus,
                                                  FinalSubmitInd = tad.FinalSubmitInd,
                                                  FlowId = tad.FlowId,
                                                  AssignedVO = tad.AssignedVO
                                              }).ToList();

                        var CreatedByExisting = (from tad in _db.tbl_Applicant_Detail
                                                 where tad.ApplicationId == res.ApplicationId && tad.IsActive == true
                                                 select new ApplicantTransDetails
                                                 {
                                                     CreatedBy = tad.CredatedBy
                                                 }).ToList();

                        tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                        objtbl_ApplicantTrans.ApplicantId = res.ApplicationId;
                        objtbl_ApplicantTrans.Remark = gh.Remarks;
                        objtbl_ApplicantTrans.FinalSubmitInd = 1;
                        objtbl_ApplicantTrans.ApplDescStatus = 11;
                        objtbl_ApplicantTrans.Status = 3;
                        if (CreatedByExisting.Count > 0)
                        {
                            foreach (var CreatedByExistingVOval in CreatedByExisting)
                            {
                                objtbl_ApplicantTrans.FlowId = CreatedByExistingVOval.CreatedBy;
                            }
                        }

                        objtbl_ApplicantTrans.CreatedBy = loginId;
                        if (FlowIDExisting.Count > 0)
                        {
                            foreach (var ExistingAssignedVOval in FlowIDExisting)
                            {
                                objtbl_ApplicantTrans.VerfOfficer = ExistingAssignedVOval.VerfOfficer;
                                objtbl_ApplicantTrans.ReVerficationStatus = ExistingAssignedVOval.ReVerficationStatus;
                                objtbl_ApplicantTrans.FinalSubmitInd = ExistingAssignedVOval.FinalSubmitInd;
                                objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                                break;
                            }
                        }
                        objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                        objtbl_ApplicantTrans.TransDate = DateTime.Now;
                        objtbl_ApplicantTrans.IsActive = 1;

                        _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                        _db.SaveChanges();

                        #endregion

                        transaction.Commit();

                        //string templateid = WebConfigurationManager.AppSettings["NewApplicantOTPRegistration"];
                        //var OTPSuccuessFailure = SMSHttpPostClient.SendOTPMSG(Convert.ToString(apl.PhoneNumber), string.Format(CmnClass.EmailAndMobileMsgs.EmailGRVSubjectMOB, apl.ApplicantName), templateid);
                        ////smsresponse = _CommonBLL.SendSMSBLL(TOMobileNumber, string.Format(Message, objApplicant.Name, Session["generatedMobileOTP"].ToString()));
                        ////smsresponse = sendSMS(TOMobileNumber, string.Format(Message, objApplicant.Name, Session["generatedMobileOTP"].ToString()));
                        //var emailresponse = true;
                        //CommonDLL cd = new CommonDLL();
                        //emailresponse = cd.SendEmailDLL(apl.EmailId, "Grievance Confirmation", string.Format(CmnClass.EmailAndMobileMsgs.EmailGRVSubjectMOB, apl.ApplicantName));
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
        public List<VerificationOfficer> GetGrievanceRemarks(int id)
        {
            try
            {
                var res = (from n in _db.tbl_GrievanceHistory
                           join bc in _db.tbl_Grievance on n.GrievanceId equals bc.GrievanceId
                           join u in _db.tbl_role_master on n.CreatedBy equals u.role_id
                           join bb in _db.tbl_status_master on n.Status equals bb.StatusId
                           where n.GrievanceId == id
                           select new VerificationOfficer
                           {
                               GrievanceRefNumber = bc.GrievanceRefNumber,
                               Remarks = n.Remarks,
                               To = n.CreatedBy == 10 ? "Verification officer" : "Applicant",
                               From = u.role_description,
                               StatusName = bb.StatusName,
                               Datestring = n.CreatedOn.ToString()
                           }).ToList();


                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
