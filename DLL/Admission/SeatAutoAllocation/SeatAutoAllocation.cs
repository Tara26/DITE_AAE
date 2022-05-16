using DLL.DBConnection;
using Models;
using Models.Admission;
using Models.Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Admission.SeatAutoAllocation
{
    public class SeatAutoAllocation : ISeatAutoAllocation
    {
        private readonly DbConnection _db = new DbConnection();

        #region Generate Seat Auto allocation

        #region .. Round 1,2 ..
        public List<SeatAutoAllocationModel> Round1GenerateSeatAutoAllotment(int courseType, int applicantType, 
            int academicYear, int round, int RoleId,int LoginID)
        {
            using (var trasaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var allocIdTransDet = 0;
                    int CreatedByExisting = _db.tbl_SeatAllocation_SeatMatrix.Where(x => x.IsActive == true 
                    && x.AcademicYear == academicYear && x.CourseTypeId == courseType && x.ApplicantType == applicantType
                    && x.Round == round && x.CourseTypeId == courseType).Select(y => y.AllocationId).FirstOrDefault();
                    List<SeatAutoAllocationModel> res = new List<SeatAutoAllocationModel>();
                    if (CreatedByExisting == 0)
                    {
                        var autoseat = new tbl_SeatAllocation_SeatMatrix();
                        autoseat.AcademicYear = academicYear;
                        autoseat.ApplicantType = applicantType;
                        autoseat.Round = round;
                        autoseat.Status = (int)CmnClass.Status.SeatAlloted;
                        autoseat.Remarks = _db.tbl_status_master.Where(a=> a.StatusId == (int)CmnClass.Status.SeatAlloted).Select(x => x.StatusName).FirstOrDefault();
                        autoseat.IsActive = true;
                        autoseat.CreatedBy = RoleId;
                        autoseat.CreatedOn = DateTime.Now;
                        autoseat.CourseTypeId = courseType;
                        autoseat.FlowId = RoleId;
                        _db.tbl_SeatAllocation_SeatMatrix.Add(autoseat);
                        _db.SaveChanges();

                        allocIdTransDet = autoseat.AllocationId;
                        //_db.tbl_SeatAllocation_SeatMatrix.OrderByDescending(x => x.AllocationId).Select(y => y.AllocationId).FirstOrDefault();
                        var seatTrans = new tbl_SeatAllocation_SeatMatrix_Trans();
                        seatTrans.AllocationId = allocIdTransDet;
                        seatTrans.Status = (int)CmnClass.Status.SeatAlloted;
                        seatTrans.IsActive = true;
                        seatTrans.CreatedBy = RoleId;
                        seatTrans.CreatedOn = DateTime.Now;
                        seatTrans.FlowId = RoleId;
                        seatTrans.Remarks = _db.tbl_status_master.Where(a => a.StatusId == (int)CmnClass.Status.SeatAlloted).Select(x => x.StatusName).FirstOrDefault();
                        _db.tbl_SeatAllocation_SeatMatrix_Trans.Add(seatTrans);
                        _db.SaveChanges();

                        List<AutoAllocation> result = new List<AutoAllocation>();
                        result = (from bb in _db.tbl_Applicant_Detail
                                  join cc in _db.tbl_GradationRank_Trans on bb.ApplicationId equals cc.ApplicantId
                                  where cc.Final == true && cc.Status == 2 && bb.IsActive == true && bb.ApplicantType == applicantType
                                  && cc.RoundId == round
                                  orderby cc.Rank ascending
                                  select new AutoAllocation
                                  {
                                      ApplicantId = bb.ApplicationId,
                                      Rank = cc.Rank,
                                      VCategory = bb.Category,
                                      Minority = bb.MinorityCategory,
                                      //Horizontal Category
                                      Gender = bb.Gender,
                                      PhysicalHandicap = bb.PhysicallyHanidcapInd,
                                      ExServiceMan = bb.ExServiceMan,
                                      KanndaMedium = bb.KanndaMedium,
                                      EconomyWeakerSection = bb.EconomyWeakerSection,
                                      HyderabadKarnataka = bb.HyderabadKarnatakaRegion

                                  }).OrderBy(x => x.Rank).ToList();

                        //int ApplicationIdToChk = 0;
                        List<tbl_SeatAllocationDetail_Seatmatrix> lstSeatAllocSeatMatrix = new List<tbl_SeatAllocationDetail_Seatmatrix>();
                        int RoundCycle = 1;
                        List<tbl_SeatMatrix_TradeWise> lstSeatMatrixTradeWise = _db.tbl_SeatMatrix_TradeWise.Where(a => _db.tbl_SeatMatrix_Main.Where(x=> x.ApplicantType ==applicantType && x.Round == round).Any(b => b.SeatMaxId == a.SeatMaxId)).ToList();
                        while (true)
                        {
                            int i = 0;
                            //Loop for Total Number of Applicants
                            foreach (var item in result)
                            {
                                if (item.Rank == 5)
                                {

                                }

                                i++;
                                // If seat Allotted for the current applicant in any of the previous halves, then skip allotting the applicant.
                                if (lstSeatAllocSeatMatrix.Count > 0 && lstSeatAllocSeatMatrix.Any(x=> x.ApplicantId == item.ApplicantId && x.RankNumber == item.Rank))
                                {
                                    continue;
                                }

                                int seatAllotedFlag = 0;
                                var preference_colleges = _db.tbl_Applicant_InstitutePreference.OrderBy(y =>
                                y.PreferenceId).Where(x => x.IsActive == true && x.ApplicantId ==
                                item.ApplicantId).OrderBy(a=> a.PreferenceId).ToList();
                                foreach (var itm in preference_colleges) // Loop for Each Applicant's Preferences
                                {
                                    //To Get the total Seats from Matrix 
                                    var seamax_id = (from aa in _db.tbl_SeatMatrix_Main
                                                     join bb in _db.tbl_Year on aa.AcademicYear equals bb.YearID
                                                     join cc in _db.tbl_course_type_mast on aa.CourseTypeId equals cc.course_id
                                                     join dd in _db.tbl_ApplicantType on aa.ApplicantType equals dd.ApplicantTypeId
                                                     where aa.IsActive == true && aa.Status == 2 && aa.AcademicYear == academicYear && aa.CourseTypeId == courseType
                                                     && dd.ApplicantTypeId == applicantType && aa.InstituteId == itm.InstituteId && aa.Round == round
                                                     select new AutoAllocation { SeatMaxId = aa.SeatMaxId }).FirstOrDefault();

                                    if (seamax_id != null && allocIdTransDet != 0)
                                    {
                                        //To Get Available seats based on category wise for the applicant 
                                        Hashtable HCategory = new Hashtable();
                                        HCategory.Add(1, item.Gender);
                                        HCategory.Add(2, item.PhysicalHandicap);
                                        HCategory.Add(3, item.ExServiceMan);
                                        HCategory.Add(4, item.KanndaMedium);
                                        HCategory.Add(5, item.EconomyWeakerSection);
                                        //HCategory.Add(6, item.resultRegion);
                                        HCategory.Add(6, item.HyderabadKarnataka);

                                        //Available Seats from Applicant Inst Preference 
                                        var TupleSeats = GetSeatsAvailable(Convert.ToInt32(item.VCategory), HCategory,
                                            itm.InstituteId, itm.TradeId, seamax_id.SeatMaxId, allocIdTransDet, round, RoundCycle, lstSeatAllocSeatMatrix, lstSeatMatrixTradeWise);
                                        var allocId = _db.tbl_SeatAllocation_SeatMatrix.OrderByDescending(x =>
                                            x.AllocationId).Select(y => y.AllocationId).FirstOrDefault();

                                        int AVCategory = 0;
                                        int AHCategory = 0;
                                        if (TupleSeats.Item2 != 0)
                                        {
                                            //Allocating seats to Applicant based on Preference
                                            seatAllotedFlag = 1;
                                            string[] CategoryList = TupleSeats.Item1.Split('-');
                                            string AVCategoryVal = CategoryList[0];
                                            string AHCategoryVal = CategoryList[1];

                                            AVCategory = _db.tbl_Category.Where(x => x.Category_desc == AVCategoryVal).
                                                Select(y => y.CategoryId).FirstOrDefault();
                                            AHCategory = _db.Tbl_horizontal_rules.Where(x => x.Horizontal_rules_desc ==
                                            AHCategoryVal).Select(y => y.Horizontal_rules_id).FirstOrDefault();
                                            var seat = new tbl_SeatAllocationDetail_Seatmatrix();
                                            seat.AllocationId = allocIdTransDet;
                                            seat.RankNumber = item.Rank;
                                            seat.ApplicantId = item.ApplicantId;
                                            seat.InstituteId = itm.InstituteId;
                                            seat.TradeId = itm.TradeId;
                                            seat.HorizontalId = AHCategory;
                                            seat.VerticalId = AVCategory;
                                            //seat.HyrNonHydrId = resultRegion;
                                            //1 for hyd, 2 for non hyd, these values should get from hyd-kar table
                                            seat.HyrNonHydrId = item.HyderabadKarnataka.Value ? 1 : 2;
                                            seat.PreferenceNum = itm.PreferenceId;
                                            seat.Status = (int)CmnClass.Status.SeatAlloted;
                                            seat.IsActive = true;
                                            seat.CreatedBy = LoginID;
                                            seat.CreatedOn = DateTime.Now;
                                            seat.FlowId = RoleId;
                                            seat.UnitId = TupleSeats.Item3;
                                            seat.ShiftId = TupleSeats.Item4;
                                            seat.AllocByCategory = TupleSeats.Item1;
                                            seat.InstitutePreferenceId = itm.InstitutePreferenceId;
                                            lstSeatAllocSeatMatrix.Add(seat);

                                            UpdtApplDetailsStatus(item.ApplicantId, (int)CmnClass.Status.SeatAlloted);
                                            UpdtApplITIInstDetailsDLL(item.ApplicantId, LoginID, RoleId, allocId);
                                            break;
                                        }
                                        // Don't check for next preference for 2nd round 1st half and 3rd half and 4th round 1st half
                                        if ((round == 2 && (RoundCycle == 1 || RoundCycle == 3)) || (round == 4 && RoundCycle == 1))
                                        {
                                            break;
                                        }
                                    }
                                }
                                if (round == 2 && RoundCycle < 3)
                                {
                                    continue;
                                }
                                if (seatAllotedFlag == 0)    // else when no seat Alloted in any Institutes in particular round
                                {
                                    var seat = new tbl_SeatAllocationDetail_Seatmatrix();
                                    seat.AllocationId = allocIdTransDet;
                                    seat.RankNumber = item.Rank;
                                    seat.ApplicantId = item.ApplicantId;
                                    seat.Status = (int)CmnClass.Status.SeatNOTAlloted;
                                    seat.IsActive = true;
                                    seat.CreatedBy = LoginID;
                                    seat.CreatedOn = DateTime.Now;
                                    seat.FlowId = RoleId;
                                    lstSeatAllocSeatMatrix.Add(seat);
                                    UpdtApplDetailsStatus(item.ApplicantId, (int)CmnClass.Status.SeatNOTAlloted);
                                }
                            }
                            // If round 2 and Round cycle less than 4 (For Round 2 there are 4 halves)or Round 4 and Round Cycle less than 2(For round 4 there are 2 halves), then loop through all applicants again
                            if ((round == 2 && RoundCycle < 3) || (round == 4 && RoundCycle < 2))
                            {
                                RoundCycle += 1;
                                continue;
                            }
                            else
                                // Else break the loop and continue
                                break;
                        }
                        _db.tbl_SeatAllocationDetail_Seatmatrix.AddRange(lstSeatAllocSeatMatrix);
                        _db.SaveChanges();
                        trasaction.Commit();

                        // Fetch the allocated data.
                        res = (from aa in _db.tbl_SeatAllocationDetail_Seatmatrix
                               join bb in _db.tbl_GradationRank_Trans on aa.ApplicantId equals bb.ApplicantId
                               join cc in _db.tbl_Applicant_Detail on aa.ApplicantId equals cc.ApplicationId
                               join dd in _db.tbl_location_type on cc.ApplicantBelongTo equals dd.location_id
                               join ee in _db.tbl_iti_college_details on aa.InstituteId equals ee.iti_college_id
                               join ff in _db.tbl_district_master on ee.district_id equals ff.district_lgd_code
                               join gg in _db.tbl_Institute_type on ee.Insitute_TypeId equals gg.Institute_type_id
                               join hh in _db.tbl_trade_mast on aa.TradeId equals hh.trade_id
                               join ii in _db.tbl_Category on aa.VerticalId equals ii.CategoryId
                               join jj in _db.Tbl_horizontal_rules on aa.HorizontalId equals jj.Horizontal_rules_id
                               join kk in _db.tbl_Gender on cc.Gender equals kk.Gender_Id
                               join ll in _db.tbl_SeatAllocation_SeatMatrix on aa.AllocationId equals ll.AllocationId
                               join mm in _db.tbl_HYD_NonHYD_regions on aa.HyrNonHydrId equals mm.Hyd_NonHyd_region_id
                               //join nn in _db.tbl_SeatAllocation_SeatMatrix on aa.AllocationId equals nn.AllocationId
                               join oo in _db.tbl_taluk_master on ee.taluk_id equals oo.taluk_lgd_code
                               join pp in _db.tbl_division_master on ee.division_id equals pp.division_id
                               join qq in _db.tbl_shifts on aa.ShiftId equals qq.s_id
                               join rr in _db.tbl_units on aa.UnitId equals rr.u_id
                               join ss in _db.tbl_status_master on aa.Status equals ss.StatusId

                               where bb.Final == true && bb.Status == 2 && bb.RoundId == round
                               && aa.AllocationId == allocIdTransDet && aa.Status == (int)CmnClass.Status.SeatAlloted
                               select new SeatAutoAllocationModel
                               {
                                   Rank = bb.Rank,
                                   Status = ss.StatusName + " - " + round,
                                   seatAllocDetailId = ll.AllocationId,
                                   division_id = pp.division_id,
                                   DivisionName = pp.division_name,
                                   district_id = ff.district_lgd_code,
                                   DistrictName = ff.district_ename,
                                   TalukId = oo.taluk_lgd_code,
                                   TalukName = oo.taluk_ename,
                                   MISCode = ee.MISCode,
                                   ITIType = gg.Institute_type,
                                   ITIName = ee.iti_college_name,
                                   TradeCode = hh.trade_id,
                                   TradeName = hh.trade_name,
                                   ShiftsDet = qq.shifts,
                                   UnitsDet = rr.units,
                                   AllottedCategory = ii.Category_desc,
                                   AllottedGroup = jj.Horizontal_rules,
                                   LocalStatus = mm.Region_type,
                                   PreferenceNumber = aa.PreferenceNum,
                                   FirstName = cc.ApplicantName,
                                   FatherName = cc.FathersName,
                                   Gender = kk.Gender,
                                   MobileNumber = cc.PhoneNumber,
                                   DOB = cc.DOB,
                                   DateOfBirth = cc.DOB.ToString(),
                                   Remarks = ll.Remarks
                               }).ToList();
                        // Fetch the Non-allocated data.
                        List<SeatAutoAllocationModel> res1 = (from aaa in _db.tbl_SeatAllocationDetail_Seatmatrix
                                                              join bbb in _db.tbl_GradationRank_Trans on aaa.ApplicantId equals bbb.ApplicantId
                                                              join ccc in _db.tbl_Applicant_Detail on aaa.ApplicantId equals ccc.ApplicationId
                                                              join ddd in _db.tbl_location_type on ccc.ApplicantBelongTo equals ddd.location_id
                                                              join eee in _db.tbl_Gender on ccc.Gender equals eee.Gender_Id
                                                              join fff in _db.tbl_SeatAllocation_SeatMatrix on aaa.AllocationId equals fff.AllocationId
                                                              join ggg in _db.tbl_status_master on aaa.Status equals ggg.StatusId

                                                              where bbb.Final == true && bbb.Status == 2 && bbb.RoundId == round
                                                                  && aaa.AllocationId == allocIdTransDet && aaa.Status == (int)CmnClass.Status.SeatNOTAlloted

                                                              select new SeatAutoAllocationModel
                                                              {
                                                                  Rank = bbb.Rank,
                                                                  Status = ggg.StatusName + " - " + round,
                                                                  seatAllocDetailId = fff.AllocationId,
                                                                  PreferenceNumber = aaa.PreferenceNum,
                                                                  FirstName = ccc.ApplicantName,
                                                                  FatherName = ccc.FathersName,
                                                                  Gender = eee.Gender,
                                                                  MobileNumber = ccc.PhoneNumber,
                                                                  DOB = ccc.DOB,
                                                                  DateOfBirth = ccc.DOB.ToString(),
                                                                  Remarks = fff.Remarks
                                                              }).ToList();

                        res.AddRange(res1);
                    }
                    return res.OrderBy(a=> a.Rank).ToList();
                }
                catch (Exception e)
                {
                    trasaction.Rollback();
                    throw e;
                }
            }
        }
        public Tuple<string, int,int,int> GetSeatsAvailable(int VCategory, Hashtable HCategory, int instituteId, 
            int tradeId, int seamax_id, int allocationId, int RoundDetails, int RoundCycle, List<tbl_SeatAllocationDetail_Seatmatrix> lstSeatAllocSeatMatrix, List<tbl_SeatMatrix_TradeWise> lstSeatMatrixTradeWise)
        {
            int HGender = 0; bool isHyd = false; 
            bool HPhysicalHandicap = false; bool HExServiceMan = false; bool KanndaMedium = false;
            bool EconomyWeakerSection = false;
            int AssignedUnitsId = 1; int AssignedshiftsId = 1;
            foreach (DictionaryEntry DE in HCategory)
            {
                if (Convert.ToInt32(DE.Key) == 1)
                    HGender = Convert.ToInt32(DE.Value);
                else if (Convert.ToInt32(DE.Key) == 2)
                    HPhysicalHandicap = Convert.ToBoolean(DE.Value);
                else if (Convert.ToInt32(DE.Key) == 3)
                    HExServiceMan = Convert.ToBoolean(DE.Value);
                else if (Convert.ToInt32(DE.Key) == 4)
                    KanndaMedium = Convert.ToBoolean(DE.Value);
                else if (Convert.ToInt32(DE.Key) == 5)
                    EconomyWeakerSection = Convert.ToBoolean(DE.Value);
                else
                    isHyd = Convert.ToBoolean(DE.Value);
            }
           
            int VerifiedCategory = 0; string AllocByCategory = "";

            //To get the all seats availability
            var gmgp_seats = lstSeatMatrixTradeWise.Where(x => x.SeatMaxId == seamax_id &&
            x.TradeId == tradeId).FirstOrDefault();
            int? total = 0;
            int allottedSeatsCount = 0;
            if (RoundDetails == 1)
            {
                //First round , Second Round in First Half
                if (gmgp_seats != null)
                {
                    #region GMGP verification is first priority
                    //GMGP -- no reservation any body can get seat                    
                    AllocByCategory = (total = isHyd ? gmgp_seats.GMGPH : gmgp_seats.GMGPNH).Value > 0 ?
                        isHyd ? "GM-GP-H" : "GM-GP-NH" : "";
                    allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                        allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    //no vacant seat? allow only womans under GM
                    if (allottedSeatsCount == 0 && EconomyWeakerSection && VCategory == 1)
                    {
                        AllocByCategory = (total = isHyd ? gmgp_seats.GMEWSH : gmgp_seats.GMEWSNH).Value > 0 ?
                            isHyd ? "GM-EWS-H" : "GM-EWS-NH" : "";
                        allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    }
                    if (allottedSeatsCount == 0 && HGender == 2)
                    {
                        AllocByCategory = (total = isHyd ? gmgp_seats.GMWH : gmgp_seats.GMWNH).Value > 0 ?
                            isHyd ? "GM-W-H" : "GM-W-NH" : "";
                        allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                          allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    }
                    //no vacant seat? allow only PH under GM
                    if (allottedSeatsCount == 0 && HPhysicalHandicap)
                    {
                        AllocByCategory = (total = isHyd ? gmgp_seats.GMPDH : gmgp_seats.GMPDNH).Value > 0 ?
                            isHyd ? "GM-PD-H" : "GM-PD-NH" : "";
                        allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                          allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    }
                    //no vacant seat? allow ex-service 
                    if (allottedSeatsCount == 0 && HExServiceMan)
                    {
                        AllocByCategory = (total = isHyd ? gmgp_seats.GMEXSH : gmgp_seats.GMEXSNH).Value > 0 ?
                            isHyd ? "GM-EXS-H" : "GM-EXS-NH" : "";
                        allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                          allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    }
                    //no vacant seat? allow Kannada medium
                    if (allottedSeatsCount == 0 && KanndaMedium)
                    {
                        AllocByCategory = (total = isHyd ? gmgp_seats.GMKMH : gmgp_seats.GMKMNH).Value > 0 ?
                            isHyd ? "GM-KM-H" : "GM-KM-NH" : "";
                        allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                          allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    }
                    //no vacant seat? allow EWS, other caste by default false
                    //allow only for UR canidates, because EWS will not applicable for any other caste

                    #endregion
                    #region fallowed by vertical and horizantal combination if no vacant seat
                    //No vacant seat for GM, then go with Vertical and Horizantal combination
                    if (allottedSeatsCount == 0)
                    {
                        if (VCategory == 3)//SC
                        {
                            AllocByCategory = (total = isHyd ? gmgp_seats.SCGPH : gmgp_seats.SCGPNH).Value > 0 ?
                                isHyd ? "SC-GP-H" : "SC-GP-NH" : "";
                            allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                                allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.SCWH : gmgp_seats.SCWNH).Value > 0 ?
                                    isHyd ? "SC-W-H" : "SC-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.SCEXSH : gmgp_seats.SCEXSNH).Value > 0 ?
                                    isHyd ? "SC-EXS-H" : "SC-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.SCPDH : gmgp_seats.SCPDNH).Value > 0 ?
                                    isHyd ? "SC-PD-H" : "SC-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            //No EWS check                          
                            if (allottedSeatsCount == 0 && KanndaMedium)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.SCKMH : gmgp_seats.SCKMNH).Value > 0 ?
                                    isHyd ? "SC-KM-H" : "SC-KM-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }

                        }
                        else if (VCategory == 4) //ST
                        {
                            AllocByCategory = (total = isHyd ? gmgp_seats.STGPH : gmgp_seats.STGPNH).Value > 0 ?
                                   isHyd ? "ST-GP-H" : "ST-GP-NH" : "";
                            allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);

                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.STWH : gmgp_seats.STWNH).Value > 0 ?
                                    isHyd ? "ST-W-H" : "ST-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.STPDH : gmgp_seats.STPDNH).Value > 0 ?
                                    isHyd ? "ST-PD-H" : "ST-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            //No EWS check
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.STEXSH : gmgp_seats.STEXSNH).Value > 0 ?
                                    isHyd ? "ST-EXS-H" : "ST-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && KanndaMedium)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.STKMH : gmgp_seats.STKMNH).Value > 0 ?
                                    isHyd ? "ST-KM-H" : "ST-KM-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }

                        }
                        else if (VCategory == 5) //C1
                        {
                            AllocByCategory = (total = isHyd ? gmgp_seats.C1GPH : gmgp_seats.C1GPNH).Value > 0 ?
                                      isHyd ? "C1-GP-H" : "C1-GP-NH" : "";
                            allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);

                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.C1WH : gmgp_seats.C1WNH).Value > 0 ?
                                    isHyd ? "C1-W-H" : "C1-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.C1PDH : gmgp_seats.C1PDNH).Value > 0 ?
                                    isHyd ? "C1-PD-H" : "C1-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            //No EWS check
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.C1EXSH : gmgp_seats.C1EXSNH).Value > 0 ?
                                    isHyd ? "C1-EXS-H" : "C1-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                                  allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && KanndaMedium)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.C1KMH : gmgp_seats.C1KMNH).Value > 0 ?
                                    isHyd ? "C1-KM-H" : "C1-KM-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                                  allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }

                        }
                        else if (VCategory == 6) //IIA
                        {
                            AllocByCategory = (total = isHyd ? gmgp_seats.TWOAGPH : gmgp_seats.TWOAGPNH).Value > 0 ?
                                         isHyd ? "TWOA-GP-H" : "TWOA-GP-NH" : "";
                            allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);

                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOAWH : gmgp_seats.TWOAWNH).Value > 0 ?
                                    isHyd ? "TWOA-W-H" : "TWOA-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOAPDH : gmgp_seats.TWOAPDNH).Value > 0 ?
                                    isHyd ? "TWOA-PD-H" : "TWOA-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            //No EWS check
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOAEXSH : gmgp_seats.TWOAEXSNH).Value > 0 ?
                                    isHyd ? "TWOA-EXS-H" : "TWOA-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && KanndaMedium)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOAKMH : gmgp_seats.TWOAKMNH).Value > 0 ?
                                    isHyd ? "TWOA-KM-H" : "TWOA-KM-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                    CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }


                        }
                        else if (VCategory == 7) //IIB
                        {
                            AllocByCategory = (total = isHyd ? gmgp_seats.TWOBGPH : gmgp_seats.TWOBGPNH).Value > 0 ?
                                         isHyd ? "TWOB-GP-H" : "TWOB-GP-NH" : "";
                            allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);

                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOBWH : gmgp_seats.TWOBWNH).Value > 0 ?
                                    isHyd ? "TWOB-W-H" : "TWOB-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOBPDH : gmgp_seats.TWOBPDNH).Value > 0 ?
                                    isHyd ? "TWOB-PD-H" : "TWOB-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            //No EWS check
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOBEXSH : gmgp_seats.TWOBEXSNH).Value > 0 ?
                                    isHyd ? "TWOB-EXS-H" : "TWOB-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && KanndaMedium)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOBKMH : gmgp_seats.TWOBKMNH).Value > 0 ?
                                    isHyd ? "TWOB-KM-H" : "TWOB-KM-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                       CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }

                        }
                        else if (VCategory == 8) //IIIA
                        {
                            AllocByCategory = (total = isHyd ? gmgp_seats.THREEAGPH : gmgp_seats.THREEAGPNH).Value > 0 ?
                                         isHyd ? "THREEA-GP-H" : "THREEA-GP-NH" : "";
                            allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);

                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEAWH : gmgp_seats.THREEAWNH).Value > 0 ?
                                    isHyd ? "THREEA-W-H" : "THREEA-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEAPDH : gmgp_seats.THREEAPDNH).Value > 0 ?
                                    isHyd ? "THREEA-PD-H" : "THREEA-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            //No EWS check
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEAEXSH : gmgp_seats.THREEAEXSNH).Value > 0 ?
                                    isHyd ? "THREEA-EXS-H" : "THREEA-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && KanndaMedium)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEAKMH : gmgp_seats.THREEAKMNH).Value > 0 ?
                                    isHyd ? "THREEA-KM-H" : "THREEA-KM-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }

                        }
                        else if (VCategory == 9) //IIIB
                        {
                            AllocByCategory = (total = isHyd ? gmgp_seats.THREEBGPH : gmgp_seats.THREEBGPNH).Value > 0 ?
                                           isHyd ? "THREEB-GP-H" : "THREEB-GP-NH" : "";
                            allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);

                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEBWH : gmgp_seats.THREEBWNH).Value > 0 ?
                                    isHyd ? "THREEB-W-H" : "THREEB-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEBPDH : gmgp_seats.THREEBPDNH).Value > 0 ?
                                    isHyd ? "THREEB-PD-H" : "THREEB-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            //No EWS check
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEBEXSH : gmgp_seats.THREEBEXSNH).Value > 0 ?
                                    isHyd ? "THREEB-EXS-H" : "THREE-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && KanndaMedium)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEBKMH : gmgp_seats.THREEBKMNH).Value > 0 ?
                                    isHyd ? "THREEB-KM-H" : "THREEB-KM-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }

                        }
                    }
                    #endregion
                }
            }
            //Second Round
            else if (RoundDetails == 2)
            {
                if (RoundCycle == 1)
                {
                    gmgp_seats.GMGP = gmgp_seats.GMGPH + gmgp_seats.GMGPNH;
                    gmgp_seats.GMGPH = 0; gmgp_seats.GMGPNH = 0;
                     AllocByCategory = (total = gmgp_seats.GMGP).Value > 0 ? "GM-GP" : "";
                    allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                        allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                }
                if (RoundCycle == 2)
                {
                    if (allottedSeatsCount == 0 && HGender == 2)
                    {
                        AllocByCategory = (total = isHyd ? gmgp_seats.GMWH : gmgp_seats.GMWNH).Value > 0 ?
                            isHyd ? "GM-W-H" : "GM-W-NH" : "";
                        allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                          allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    }
                    if (allottedSeatsCount == 0 && HPhysicalHandicap)
                    {
                        AllocByCategory = (total = isHyd ? gmgp_seats.GMPDH : gmgp_seats.GMPDNH).Value > 0 ?
                            isHyd ? "GM-PD-H" : "GM-PD-NH" : "";
                        allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                          allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    }
                    if (allottedSeatsCount == 0 && HExServiceMan)
                    {
                        AllocByCategory = (total = isHyd ? gmgp_seats.GMEXSH : gmgp_seats.GMEXSNH).Value > 0 ?
                            isHyd ? "GM-EXS-H" : "GM-EXS-NH" : "";
                        allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                          allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    }
                    if (allottedSeatsCount == 0)
                    {
                        if (VCategory == 3)//SC
                        {
                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.SCWH : gmgp_seats.SCWNH).Value > 0 ?
                                    isHyd ? "SC-W-H" : "SC-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.SCPDH : gmgp_seats.SCPDNH).Value > 0 ?
                                    isHyd ? "SC-PD-H" : "SC-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.SCEXSH : gmgp_seats.SCEXSNH).Value > 0 ?
                                    isHyd ? "SC-EXS-H" : "SC-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                        }
                        else if (VCategory == 4) //ST
                        {
                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.STWH : gmgp_seats.STWNH).Value > 0 ?
                                    isHyd ? "ST-W-H" : "ST-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.STPDH : gmgp_seats.STPDNH).Value > 0 ?
                                    isHyd ? "ST-PD-H" : "ST-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.STEXSH : gmgp_seats.STEXSNH).Value > 0 ?
                                    isHyd ? "ST-EXS-H" : "ST-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                        }
                        else if (VCategory == 5) //C1
                        {
                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.C1WH : gmgp_seats.C1WNH).Value > 0 ?
                                    isHyd ? "C1-W-H" : "C1-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.C1PDH : gmgp_seats.C1PDNH).Value > 0 ?
                                    isHyd ? "C1-PD-H" : "C1-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.C1EXSH : gmgp_seats.C1EXSNH).Value > 0 ?
                                    isHyd ? "C1-EXS-H" : "C1-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                                  allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                        }
                        else if (VCategory == 6) //IIA
                        {
                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOAWH : gmgp_seats.TWOAWNH).Value > 0 ?
                                    isHyd ? "TWOA-W-H" : "TWOA-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOAPDH : gmgp_seats.TWOAPDNH).Value > 0 ?
                                    isHyd ? "TWOA-PD-H" : "TWOA-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOAEXSH : gmgp_seats.TWOAEXSNH).Value > 0 ?
                                    isHyd ? "TWOA-EXS-H" : "TWOA-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                        }
                        else if (VCategory == 7) //IIB
                        {
                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOBWH : gmgp_seats.TWOBWNH).Value > 0 ?
                                    isHyd ? "TWOB-W-H" : "TWOB-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOBPDH : gmgp_seats.TWOBPDNH).Value > 0 ?
                                    isHyd ? "TWOB-PD-H" : "TWOB-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.TWOBEXSH : gmgp_seats.TWOBEXSNH).Value > 0 ?
                                    isHyd ? "TWOB-EXS-H" : "TWOB-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                        }
                        else if (VCategory == 8) //IIIA
                        {
                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEAWH : gmgp_seats.THREEAWNH).Value > 0 ?
                                    isHyd ? "THREEA-W-H" : "THREEA-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEAPDH : gmgp_seats.THREEAPDNH).Value > 0 ?
                                    isHyd ? "THREEA-PD-H" : "THREEA-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEAEXSH : gmgp_seats.THREEAEXSNH).Value > 0 ?
                                    isHyd ? "THREEA-EXS-H" : "THREEA-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                        }
                        else if (VCategory == 9) //IIIB
                        {
                            if (allottedSeatsCount == 0 && HGender == 2)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEBWH : gmgp_seats.THREEBWNH).Value > 0 ?
                                    isHyd ? "THREEB-W-H" : "THREEB-W-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HPhysicalHandicap)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEBPDH : gmgp_seats.THREEBPDNH).Value > 0 ?
                                    isHyd ? "THREEB-PD-H" : "THREEB-PD-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                            if (allottedSeatsCount == 0 && HExServiceMan)
                            {
                                AllocByCategory = (total = isHyd ? gmgp_seats.THREEBEXSH : gmgp_seats.THREEBEXSNH).Value > 0 ?
                                    isHyd ? "THREEB-EXS-H" : "THREE-EXS-NH" : "";
                                allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                            }
                        }
                    }
                }
                if (RoundCycle == 3)
                {
                    gmgp_seats.GMGP = gmgp_seats.GMWH + gmgp_seats.GMWNH + gmgp_seats.GMPDH + gmgp_seats.GMPDNH + gmgp_seats.GMEXSH + gmgp_seats.GMEXSNH;
                    total = gmgp_seats.GMGP;
                    gmgp_seats.GMWH = 0; gmgp_seats.GMWNH = 0; gmgp_seats.GMPDH = 0; gmgp_seats.GMPDNH = 0; gmgp_seats.GMEXSH = 0; gmgp_seats.GMEXSNH = 0;
                    AllocByCategory = total.Value > 0 ? "GM-GP" : "";
                    allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    if (allottedSeatsCount == 0 && EconomyWeakerSection)
                    {
                        AllocByCategory = (total = isHyd ? gmgp_seats.GMEWSH : gmgp_seats.GMEWSNH).Value > 0 ?
                            isHyd ? "GM-EWS-H" : "GM-EWS-NH" : "";
                        allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                              allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    }
                    if (allottedSeatsCount == 0 && KanndaMedium)
                    {
                        AllocByCategory = (total = isHyd ? gmgp_seats.GMKMH : gmgp_seats.GMKMNH).Value > 0 ?
                            isHyd ? "GM-KM-H" : "GM-KM-NH" : "";
                        allottedSeatsCount = AllocByCategory == "" ? 0 : CheckSeatAllottedCount(instituteId, tradeId,
                          allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                    }

                }
                if (RoundCycle == 4)
                {
                    gmgp_seats.GMGP = gmgp_seats.GMKMH + gmgp_seats.GMKMNH + gmgp_seats.GMEWSH + gmgp_seats.GMEWSNH;
                    total = gmgp_seats.GMGP;
                    AllocByCategory = total.Value > 0 ? "GM-GP" : "";
                    allottedSeatsCount = AllocByCategory == "" ? 0 :
                                   CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);

                    gmgp_seats.SCGP = gmgp_seats.SCKMH + gmgp_seats.SCKMNH;
                    gmgp_seats.STGP = gmgp_seats.STKMH + gmgp_seats.STKMNH;
                    gmgp_seats.C1GP = gmgp_seats.C1KMH + gmgp_seats.C1KMNH;
                    gmgp_seats.TWOAGP = gmgp_seats.TWOAKMH + gmgp_seats.TWOAKMNH;
                    gmgp_seats.TWOBGP = gmgp_seats.TWOBKMH + gmgp_seats.TWOBKMNH;
                    gmgp_seats.THREEAGP = gmgp_seats.THREEAKMH + gmgp_seats.THREEAKMNH;
                    gmgp_seats.THREEBGP = gmgp_seats.THREEBKMH + gmgp_seats.THREEBKMNH;


                    if (allottedSeatsCount == 0)
                    {
                        if (VCategory == 3)
                        {
                            total = gmgp_seats.SCGP;
                            AllocByCategory = total.Value > 0 ? "SC-GP" : "";
                        }
                        else if (VCategory == 4)
                        {
                            total = gmgp_seats.STGP;
                            AllocByCategory = total.Value > 0 ? "ST-GP" : "";
                            //AllocByCategory = "ST-GP";
                        }
                        else if (VCategory == 5)
                        {
                            total = gmgp_seats.C1GP;
                            AllocByCategory = total.Value > 0 ? "C1-GP" : "";
                        }
                        else if (VCategory == 6)
                        {
                            total = gmgp_seats.TWOAGP;
                            AllocByCategory = total.Value > 0 ? "TWOA-GP" : "";
                        }
                        else if (VCategory == 7)
                        {
                            total = gmgp_seats.TWOBGP;
                            AllocByCategory = total.Value > 0 ? "TWOB-GP" : "";
                        }
                        else if (VCategory == 8)
                        {
                            total = gmgp_seats.THREEAGP;
                            AllocByCategory = total.Value > 0 ? "THREEA-GP" : "";
                        }
                        else if (VCategory == 9) //IIIB
                        {
                            total = gmgp_seats.THREEBGP;
                            AllocByCategory = total.Value > 0 ? "THREEB-GP" : "";
                        }
                        //final logic is at GOTO:
                    }
                }
            }
            else if (RoundDetails == 3 || (RoundDetails == 4 && RoundCycle == 1))
            {
                //Third Round, Fourth Round, Fifth Round
                total = gmgp_seats.GMGP;
                AllocByCategory = total.Value > 0 ? "GM-GP" : "";

                allottedSeatsCount = AllocByCategory == "" ? 0 :
                               CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
                if (allottedSeatsCount == 0)
                {
                    if (VCategory == 3)
                    {
                        total = gmgp_seats.SCGP;
                        AllocByCategory = total.Value > 0 ? "SC-GP" : "";
                    }
                    else if (VCategory == 4)
                    {
                        total = gmgp_seats.STGP;
                        AllocByCategory = total.Value > 0 ? "ST-GP" : "";
                        //AllocByCategory = "ST-GP";
                    }
                    else if (VCategory == 5)
                    {
                        total = gmgp_seats.C1GP;
                        AllocByCategory = total.Value > 0 ? "C1-GP" : "";
                    }
                    else if (VCategory == 6)
                    {
                        total = gmgp_seats.TWOAGP;
                        AllocByCategory = total.Value > 0 ? "TWOA-GP" : "";
                    }
                    else if (VCategory == 7)
                    {
                        total = gmgp_seats.TWOBGP;
                        AllocByCategory = total.Value > 0 ? "TWOB-GP" : "";
                    }
                    else if (VCategory == 8)
                    {
                        total = gmgp_seats.THREEAGP;
                        AllocByCategory = total.Value > 0 ? "THREEA-GP" : "";
                    }
                    else if (VCategory == 9) //IIIB
                    {
                        total = gmgp_seats.THREEBGP;
                        AllocByCategory = total.Value > 0 ? "THREEB-GP" : "";
                    }
                    //final logic is at GOTO:
                }
            }
            else if (RoundDetails == 4)
            {
                gmgp_seats.GMGP = gmgp_seats.SCGP + gmgp_seats.STGP + gmgp_seats.C1GP + gmgp_seats.TWOAGP + gmgp_seats.TWOBGP + gmgp_seats.THREEAGP + gmgp_seats.THREEBGP;
                //Fourth Round
                total = gmgp_seats.GMGP;
                AllocByCategory = total.Value > 0 ? "GM-GP" : "";
                if (allottedSeatsCount == 0)
                    allottedSeatsCount = AllocByCategory == "" ? 0 :
                                         CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, gmgp_seats, lstSeatAllocSeatMatrix);
            }
            //GOTO:
            //final check for all rounds except 1 and 2-1
            //1 and 2-1 each category we already called this method, if there allocated seats count zero 
            //we can send directly, no issues
            //if ((RoundDetails == 2 && RoundCycle == 2) || RoundDetails == 3 || RoundDetails == 4 ||
            //    RoundDetails == 5 || RoundDetails == 6)
            //{
            //    //this logic is required for only above mentioned rounds only
            //    if (allottedSeatsCount == 0)
            //        allottedSeatsCount = AllocByCategory == "" ? 0 :
            //                             CheckSeatAllottedCount(instituteId, tradeId, allocationId, total, AllocByCategory, lstSeatAllocSeatMatrix);
            //}

            //if (total.Value > 0)
            //{
            //    Allotted_seats = _db.tbl_SeatAllocationDetail_Seatmatrix?.Where(x => x.IsActive == true &&
            //    x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId &&
            //    x.AllocByCategory == AllocByCategory)?.Count();
            //    TotalSeats = Convert.ToInt32(total) - Allotted_seats.Value;
            //}
            if (allottedSeatsCount > 0)
            {
                int index = lstSeatMatrixTradeWise.IndexOf(gmgp_seats);
                lstSeatMatrixTradeWise[index] = gmgp_seats;

                //if seats are there then only check units and shifts
                var EligibleSeatsUnitsShifts = (from ticd in _db.tbl_iti_college_details
                                                join tit in _db.tbl_ITI_Trade on ticd.iti_college_id equals tit.ITICode
                                                join ttm in _db.tbl_trade_mast on tit.TradeCode equals ttm.trade_id
                                                join tits in _db.tbl_ITI_Trade_Shifts on tit.Trade_ITI_id equals tits.ITI_Trade_Id
                                                where ticd.iti_college_id == instituteId && ticd.is_active == true && tit.IsActive == true
                                                && tits.IsActive == true
                                                orderby tits.Units, tits.Shift ascending
                                                select new SeatAutoAllocationModel
                                                {
                                                    UnitId = tits.Units,
                                                    ShiftId = tits.Shift
                                                }).ToList();

                var AllottedSeatsUnitsShifts = (from bbb in _db.tbl_SeatAllocationDetail_Seatmatrix
                                                where bbb.IsActive == true && bbb.InstituteId == instituteId && bbb.AllocationId == allocationId && bbb.TradeId == tradeId && bbb.AllocByCategory == AllocByCategory
                                                orderby bbb.UnitId, bbb.ShiftId ascending
                                                select new SeatAutoAllocationModel
                                                {
                                                    UnitId = bbb.UnitId,
                                                    ShiftId = bbb.ShiftId
                                                }).ToList();


                foreach (var ESUS in EligibleSeatsUnitsShifts)
                {
                    foreach (var ASUS in AllottedSeatsUnitsShifts)
                    {
                        if (ASUS.UnitId == ESUS.UnitId && ASUS.ShiftId == ESUS.ShiftId)
                        {
                            AssignedUnitsId = 1;
                            AssignedshiftsId = 1;
                        }
                        else
                        {
                            AssignedUnitsId = Convert.ToInt32(ESUS.UnitId);
                            AssignedshiftsId = Convert.ToInt32(ESUS.ShiftId);
                        }
                    }
                }

            }
            return new Tuple<string, int,int,int>(AllocByCategory, allottedSeatsCount, AssignedUnitsId, AssignedshiftsId);

        }
        private int CheckSeatAllottedCount(int instituteId,
            int tradeId,  int allocationId,int? total,string AllocByCategory, tbl_SeatMatrix_TradeWise tsmtw, List<tbl_SeatAllocationDetail_Seatmatrix> lstSeatAllocSeatMatrix)
        {
            int? Allotted_seats = 0; int TotalSeats = 0;
            if (total.Value > 0)
            {
                Allotted_seats = lstSeatAllocSeatMatrix.Where(x => x.IsActive == true &&
                x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId &&
                x.AllocByCategory == AllocByCategory)?.Count();

                Type t = tsmtw.GetType();
                PropertyInfo[] pClass = t.GetProperties();

                int value = 0; // or whatever value you want to set
                foreach (var property in pClass)
                {
                    string propName = property.Name;
                    if (propName == AllocByCategory.Replace("-", ""))
                    {
                        value = Convert.ToInt32(t.GetProperty(propName).GetValue(tsmtw));
                        value = value - 1;
                        property.SetValue(tsmtw, value, null);
                        break;
                    }
                }

                //Allotted_seats = lstSeatAllocSeatMatrix.
                //Allotted_seats = _db.tbl_SeatAllocationDetail_Seatmatrix?.Where(x => x.IsActive == true &&
                //x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId &&
                //x.AllocByCategory == AllocByCategory)?.Count();
                TotalSeats = Convert.ToInt32(total) - Allotted_seats.Value;
            }
            //avoid minus values
            return TotalSeats > 0 ? TotalSeats : 0;
        }

        #endregion

        #region .. UnUsed Methods ..

        #region .. Round 3, 4, 5 ..

        public List<SeatAutoAllocationModel> Round3_4GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round, int RoleId)
        {
            using (var trasaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var allocIdTransDet = 0;
                    int CreatedByExisting = _db.tbl_SeatAllocation_SeatMatrix.Where(x => x.IsActive == true && x.AcademicYear == academicYear
                    && x.CourseTypeId == courseType && x.ApplicantType == applicantType && x.Round == round && x.CourseTypeId == courseType).
                    Select(y => y.AllocationId).FirstOrDefault();

                    var autoseat = new tbl_SeatAllocation_SeatMatrix();
                    autoseat.AcademicYear = academicYear;
                    autoseat.ApplicantType = applicantType;
                    autoseat.Round = round;
                    autoseat.Status = 1;
                    autoseat.Remarks = _db.tbl_status_master.Where(a => a.StatusId == (int)CmnClass.Status.SeatAlloted).Select(x => x.StatusName).FirstOrDefault();
                    autoseat.IsActive = true;
                    autoseat.CreatedBy = RoleId;
                    autoseat.CreatedOn = DateTime.Now;
                    autoseat.CourseTypeId = courseType;
                    autoseat.FlowId = RoleId;
                    _db.tbl_SeatAllocation_SeatMatrix.Add(autoseat);
                    _db.SaveChanges();

                    allocIdTransDet = _db.tbl_SeatAllocation_SeatMatrix.OrderByDescending(x => x.AllocationId).Select(y => y.AllocationId).FirstOrDefault();
                    var seatTrans = new tbl_SeatAllocation_SeatMatrix_Trans();
                    seatTrans.AllocationId = allocIdTransDet;
                    seatTrans.Status = 1;
                    seatTrans.IsActive = true;
                    seatTrans.CreatedBy = RoleId;
                    seatTrans.CreatedOn = DateTime.Now;
                    seatTrans.FlowId = RoleId;
                    seatTrans.Remarks = _db.tbl_status_master.Where(a => a.StatusId == (int)CmnClass.Status.SeatAlloted).Select(x => x.StatusName).FirstOrDefault();
                    _db.tbl_SeatAllocation_SeatMatrix_Trans.Add(seatTrans);
                    _db.SaveChanges();

                    var result = (from bb in _db.tbl_Applicant_Detail
                                  join cc in _db.tbl_GradationRank_Trans on bb.ApplicationId equals cc.ApplicantId
                                  where cc.Final == true && cc.Status == 2 && bb.IsActive == true && bb.ApplicantType == 1
                                  orderby cc.Rank ascending
                                  select new AutoAllocation
                                  {
                                      ApplicantId = bb.ApplicationId,
                                      Rank = cc.Rank,
                                      VCategory = bb.Category,
                                      Minority = bb.MinorityCategory,
                                  }).OrderBy(x => x.Rank).ToList();

                    foreach (var item in result)
                    {
                        var preference_colleges = _db.tbl_Applicant_InstitutePreference.Where(x => x.IsActive == true && x.ApplicantId == item.ApplicantId).ToList();
                        int AllocationPreferenceFlag = 1;
                        foreach (var itm in preference_colleges)
                        {
                            //To Get the total Seats from Matrix 
                            var seamax_id = (from aa in _db.tbl_SeatMatrix_Main
                                             join bb in _db.tbl_Year on aa.AcademicYear equals bb.YearID
                                             join cc in _db.tbl_course_type_mast on aa.CourseTypeId equals cc.course_id
                                             join dd in _db.tbl_ApplicantType on aa.ApplicantType equals dd.ApplicantTypeId
                                             where aa.IsActive == true && aa.Status == 2 && aa.AcademicYear == academicYear && aa.CourseTypeId == courseType
                                             && dd.ApplicantTypeId == applicantType && aa.InstituteId == itm.InstituteId && aa.Round == round
                                             select new AutoAllocation { SeatMaxId = aa.SeatMaxId }).FirstOrDefault();

                            //Get to know the Allocated Seats thorugh Auto Allocation
                            var allocationId = (from aa in _db.tbl_SeatAllocation_SeatMatrix
                                                join bb in _db.tbl_Year on aa.AcademicYear equals bb.YearID
                                                join cc in _db.tbl_course_type_mast on aa.CourseTypeId equals cc.course_id
                                                join dd in _db.tbl_ApplicantType on aa.ApplicantType equals dd.ApplicantTypeId
                                                where aa.IsActive == true && aa.AcademicYear == academicYear && aa.CourseTypeId == courseType &&
                                                dd.ApplicantTypeId == applicantType && aa.Round == round
                                                select new AutoAllocation { AllocationId = aa.AllocationId }).FirstOrDefault();
                            if (seamax_id != null && allocationId != null)
                            {
                                //Available Seats from Applicant Inst Preference 
                                var TupleSeats = GetSeatsAvailableFor3_4(Convert.ToInt32(item.VCategory), itm.InstituteId, itm.TradeId, seamax_id.SeatMaxId, allocationId.AllocationId);
                                if (TupleSeats.Item2 != 0)
                                {
                                    //Allocating seats to Applicant based on Preference
                                    var allocId = _db.tbl_SeatAllocation_SeatMatrix.OrderByDescending(x => x.AllocationId).Select(y => y.AllocationId).FirstOrDefault();

                                    string[] CategoryList = TupleSeats.Item1.Split('-');
                                    string AVCategoryVal = CategoryList[0];
                                    string AHCategoryVal = CategoryList[1];

                                    int AVCategory = _db.tbl_Vertical_rules.Where(x => x.Vertical_Rules == AVCategoryVal).Select(y => y.Vertical_rules_id).FirstOrDefault();
                                    int AHCategory = _db.Tbl_horizontal_rules.Where(x => x.Horizontal_rules_desc == AHCategoryVal).Select(y => y.Horizontal_rules_id).FirstOrDefault();

                                    var seat = new tbl_SeatAllocationDetail_Seatmatrix();
                                    seat.AllocationId = allocId;
                                    seat.RankNumber = item.Rank;
                                    seat.ApplicantId = item.ApplicantId;
                                    seat.InstituteId = itm.InstituteId;
                                    seat.TradeId = itm.TradeId;
                                    seat.HorizontalId = AHCategory;
                                    seat.VerticalId = AVCategory;
                                    seat.PreferenceNum = AllocationPreferenceFlag;
                                    seat.Status = 1;
                                    seat.IsActive = true;
                                    seat.CreatedBy = RoleId;
                                    seat.CreatedOn = DateTime.Now;
                                    seat.FlowId = RoleId;
                                    seat.UnitId = 1;
                                    seat.ShiftId = 1;
                                    seat.AllocByCategory = TupleSeats.Item1;
                                    _db.tbl_SeatAllocationDetail_Seatmatrix.Add(seat);
                                    _db.SaveChanges();

                                    AllocationPreferenceFlag++;
                                    break;
                                }
                            }
                        }
                    }
                    _db.SaveChanges();
                    trasaction.Commit();

                    var res = (from aa in _db.tbl_SeatAllocationDetail_Seatmatrix
                               join bb in _db.tbl_GradationRank_Trans on aa.ApplicantId equals bb.ApplicantId
                               join cc in _db.tbl_Applicant_Detail on aa.ApplicantId equals cc.ApplicationId
                               join dd in _db.tbl_location_type on cc.ApplicantBelongTo equals dd.location_id
                               join ee in _db.tbl_iti_college_details on aa.InstituteId equals ee.iti_college_id
                               join ff in _db.tbl_district_master on ee.district_id equals ff.district_lgd_code
                               join gg in _db.tbl_Institute_type on ee.Insitute_TypeId equals gg.Institute_type_id
                               join hh in _db.tbl_trade_mast on aa.TradeId equals hh.trade_id
                               join jj in _db.Tbl_horizontal_rules on aa.HorizontalId equals jj.Horizontal_rules_id
                               join kk in _db.tbl_Gender on cc.Gender equals kk.Gender_Id
                               join ll in _db.tbl_SeatAllocation_SeatMatrix on aa.AllocationId equals ll.AllocationId
                               where bb.Final == true && bb.Status == 2 && bb.RoundId == round
                               select new SeatAutoAllocationModel
                               {
                                   Rank = bb.Rank,
                                   seatAllocDetailId = aa.AllocationDetailId,
                                   DivisionName = dd.location_name,
                                   DistrictName = ff.district_ename,
                                   MISCode = ee.MISCode,
                                   ITIType = gg.Institute_type,
                                   ITIName = ee.iti_college_name,
                                   TradeName = hh.trade_name,
                                   AllottedGroup = jj.Horizontal_rules,
                                   PreferenceNumber = aa.PreferenceNum,
                                   FirstName = cc.ApplicantName,
                                   FatherName = cc.FathersName,
                                   Gender = kk.Gender,
                                   DOB = cc.DOB,
                                   Remarks = ll.Remarks
                               }).ToList();
                    return res;
                }
                catch (Exception e)
                {
                    trasaction.Rollback();
                    throw e;
                }
            }
        }
        public Tuple<string, int> GetSeatsAvailableFor3_4(int VCategory, int instituteId, int tradeId, int seamax_id, int allocationId)
        {
            int TotalSeats = 0;
            int? total = 0; string AllocByCategory = "";
            var gmgp_seats = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == seamax_id && x.TradeId == tradeId).FirstOrDefault();
            if (VCategory == 1)
            {
                total = gmgp_seats.GMGP;
                AllocByCategory = "GM-GP";
            }
            else if (VCategory == 3)
            {
                total = gmgp_seats.SCGP;
                AllocByCategory = "SC-GP";
            }
            else if (VCategory == 4)
            {
                total = gmgp_seats.STGP;
                AllocByCategory = "ST-GP";
            }
            else if (VCategory == 5)
            {
                total = gmgp_seats.TWOAGP;
                AllocByCategory = "TWOA-GP";
            }
            else if (VCategory == 6)
            {
                total = gmgp_seats.TWOBGP;
                AllocByCategory = "TWOB-GP";
            }
            else if (VCategory == 7)
            {
                total = gmgp_seats.THREEAGP;
                AllocByCategory = "THREEA-GP";
            }
            else if (VCategory == 8)
            {
                total = gmgp_seats.THREEBGP;
                AllocByCategory = "THREEB-GP";
            }

            //Institute , Trade , Round Wise Getting the allocated seats
            int Allotted_seats = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId && x.AllocByCategory == AllocByCategory).Count();
            TotalSeats = Convert.ToInt32(total) - Allotted_seats;
            return new Tuple<string, int>(AllocByCategory, TotalSeats);
        }

        #endregion

        #region .. Round 6 ..

        public List<SeatAutoAllocationModel> Round6GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round, int RoleId)
        {
            using (var trasaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var allocIdTransDet = 0;
                    int CreatedByExisting = _db.tbl_SeatAllocation_SeatMatrix.Where(x => x.IsActive == true && x.AcademicYear == academicYear
                    && x.CourseTypeId == courseType && x.ApplicantType == applicantType && x.Round == round && x.CourseTypeId == courseType).
                    Select(y => y.AllocationId).FirstOrDefault();

                    var autoseat = new tbl_SeatAllocation_SeatMatrix();
                    autoseat.AcademicYear = academicYear;
                    autoseat.ApplicantType = applicantType;
                    autoseat.Round = round;
                    autoseat.Status = 1;
                    autoseat.Remarks = "Seat Allotted";
                    autoseat.IsActive = true;
                    autoseat.CreatedBy = RoleId;
                    autoseat.CreatedOn = DateTime.Now;
                    autoseat.CourseTypeId = courseType;
                    autoseat.FlowId = RoleId;
                    _db.tbl_SeatAllocation_SeatMatrix.Add(autoseat);
                    _db.SaveChanges();

                    allocIdTransDet = _db.tbl_SeatAllocation_SeatMatrix.OrderByDescending(x => x.AllocationId).Select(y => y.AllocationId).FirstOrDefault();
                    var seatTrans = new tbl_SeatAllocation_SeatMatrix_Trans();
                    seatTrans.AllocationId = allocIdTransDet;
                    seatTrans.Status = 1;
                    seatTrans.IsActive = true;
                    seatTrans.CreatedBy = RoleId;
                    seatTrans.CreatedOn = DateTime.Now;
                    seatTrans.FlowId = RoleId;
                    seatTrans.Remarks = "Seat Allocated";
                    _db.tbl_SeatAllocation_SeatMatrix_Trans.Add(seatTrans);
                    _db.SaveChanges();

                    var result = (from bb in _db.tbl_Applicant_Detail
                                  join cc in _db.tbl_GradationRank_Trans on bb.ApplicationId equals cc.ApplicantId
                                  where cc.Final == true && cc.Status == 2 && bb.IsActive == true && bb.ApplicantType == 1
                                  orderby cc.Rank ascending
                                  select new AutoAllocation
                                  {
                                      ApplicantId = bb.ApplicationId,
                                      Rank = cc.Rank,
                                      VCategory = bb.Category,
                                      Minority = bb.MinorityCategory,
                                  }).OrderBy(x => x.Rank).ToList();

                    foreach (var item in result)
                    {
                        var preference_colleges = _db.tbl_Applicant_InstitutePreference.Where(x => x.IsActive == true && x.ApplicantId == item.ApplicantId).ToList();
                        int AllocationPreferenceFlag = 1;
                        foreach (var itm in preference_colleges)
                        {
                            //To Get the total Seats from Matrix 
                            var seamax_id = (from aa in _db.tbl_SeatMatrix_Main
                                             join bb in _db.tbl_Year on aa.AcademicYear equals bb.YearID
                                             join cc in _db.tbl_course_type_mast on aa.CourseTypeId equals cc.course_id
                                             join dd in _db.tbl_ApplicantType on aa.ApplicantType equals dd.ApplicantTypeId
                                             where aa.IsActive == true && aa.Status == 2 && aa.AcademicYear == academicYear && aa.CourseTypeId == courseType
                                             && dd.ApplicantTypeId == applicantType && aa.InstituteId == itm.InstituteId && aa.Round == round
                                             select new AutoAllocation { SeatMaxId = aa.SeatMaxId }).FirstOrDefault();

                            //Get to know the Allocated Seats thorugh Auto Allocation
                            var allocationId = (from aa in _db.tbl_SeatAllocation_SeatMatrix
                                                join bb in _db.tbl_Year on aa.AcademicYear equals bb.YearID
                                                join cc in _db.tbl_course_type_mast on aa.CourseTypeId equals cc.course_id
                                                join dd in _db.tbl_ApplicantType on aa.ApplicantType equals dd.ApplicantTypeId
                                                where aa.IsActive == true && aa.AcademicYear == academicYear && aa.CourseTypeId == courseType &&
                                                dd.ApplicantTypeId == applicantType && aa.Round == round
                                                select new AutoAllocation { AllocationId = aa.AllocationId }).FirstOrDefault();
                            if (seamax_id != null && allocationId != null)
                            {
                                //Available Seats from Applicant Inst Preference 
                                var TupleSeats = GetSeatsAvailableFor6(itm.InstituteId, itm.TradeId, seamax_id.SeatMaxId, allocationId.AllocationId);
                                if (TupleSeats.Item2 != 0)
                                {
                                    //Allocating seats to Applicant based on Preference
                                    var allocId = _db.tbl_SeatAllocation_SeatMatrix.OrderByDescending(x => x.AllocationId).Select(y => y.AllocationId).FirstOrDefault();

                                    var seat = new tbl_SeatAllocationDetail_Seatmatrix();
                                    seat.AllocationId = allocId;
                                    seat.RankNumber = item.Rank;
                                    seat.ApplicantId = item.ApplicantId;
                                    seat.InstituteId = itm.InstituteId;
                                    seat.TradeId = itm.TradeId;
                                    seat.PreferenceNum = AllocationPreferenceFlag;
                                    seat.Status = 1;
                                    seat.IsActive = true;
                                    seat.CreatedBy = RoleId;
                                    seat.CreatedOn = DateTime.Now;
                                    seat.FlowId = RoleId;
                                    seat.UnitId = 1;
                                    seat.ShiftId = 1;
                                    seat.AllocByCategory = TupleSeats.Item1;
                                    _db.tbl_SeatAllocationDetail_Seatmatrix.Add(seat);
                                    _db.SaveChanges();

                                    AllocationPreferenceFlag++;
                                    break;
                                }
                            }
                        }
                    }
                    _db.SaveChanges();
                    trasaction.Commit();

                    var res = (from aa in _db.tbl_SeatAllocationDetail_Seatmatrix
                               join bb in _db.tbl_GradationRank_Trans on aa.ApplicantId equals bb.ApplicantId
                               join cc in _db.tbl_Applicant_Detail on aa.ApplicantId equals cc.ApplicationId
                               join dd in _db.tbl_location_type on cc.ApplicantBelongTo equals dd.location_id
                               join ee in _db.tbl_iti_college_details on aa.InstituteId equals ee.iti_college_id
                               join ff in _db.tbl_district_master on ee.district_id equals ff.district_lgd_code
                               join gg in _db.tbl_Institute_type on ee.Insitute_TypeId equals gg.Institute_type_id
                               join hh in _db.tbl_trade_mast on aa.TradeId equals hh.trade_id
                               join kk in _db.tbl_Gender on cc.Gender equals kk.Gender_Id
                               join ll in _db.tbl_SeatAllocation_SeatMatrix on aa.AllocationId equals ll.AllocationId
                               where bb.Final == true && bb.Status == 2 && ll.Round == round
                               select new SeatAutoAllocationModel
                               {
                                   Rank = bb.Rank,
                                   seatAllocDetailId = aa.AllocationDetailId,
                                   DivisionName = dd.location_name,
                                   DistrictName = ff.district_ename,
                                   MISCode = ee.MISCode,
                                   ITIType = gg.Institute_type,
                                   ITIName = ee.iti_college_name,
                                   TradeName = hh.trade_name,
                                   PreferenceNumber = aa.PreferenceNum,
                                   FirstName = cc.ApplicantName,
                                   FatherName = cc.FathersName,
                                   Gender = kk.Gender,
                                   DOB = cc.DOB,
                                   Remarks = ll.Remarks
                               }).ToList();
                    return res;
                }
                catch (Exception e)
                {
                    trasaction.Rollback();
                    throw e;
                }
            }
        }
        public Tuple<string, int> GetSeatsAvailableFor6(int instituteId, int tradeId, int seamax_id, int allocationId)
        {
            int TotalSeats = 0;
            int? total = 0; string AllocByCategory = "";
            var gmgp_seats = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == seamax_id && x.TradeId == tradeId).FirstOrDefault();
            total = gmgp_seats.GMGP;
            AllocByCategory = "GM-GP";

            return new Tuple<string, int>(AllocByCategory, TotalSeats);
        }

        #endregion


        public List<SeatAutoAllocationModel> Round2GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round, int RoleId)
        {
            using (var trasaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = (from bb in _db.tbl_Applicant_Detail
                                  join cc in _db.tbl_GradationRank_Trans on bb.ApplicationId equals cc.ApplicantId
                                  join dd in _db.tbl_Applicant_Reservation on bb.ApplicationId equals dd.ApplicantId
                                  where cc.Final == true && cc.Status == 2 && bb.IsActive == true && bb.ApplicantType == 1
                                  select new AutoAllocation
                                  {
                                      ApplicantId = bb.ApplicationId,
                                      Rank = cc.Rank,
                                      Gender = bb.Gender,
                                      VCategory = bb.Caste,
                                      Religion = bb.Religion,
                                      Minority = bb.MinorityCategory,
                                      PhysicalHandicap = bb.PhysicallyHanidcapInd,
                                      HCategory = dd.ReservationId
                                  }).OrderBy(x => x.Rank).ToList();

                    var autoseat = new tbl_SeatAllocation_SeatMatrix();
                    autoseat.AcademicYear = academicYear;
                    autoseat.ApplicantType = applicantType;
                    autoseat.Round = round;
                    autoseat.Status = 1;
                    autoseat.Remarks = "seat allotted";
                    autoseat.IsActive = true;
                    autoseat.CourseTypeId = courseType;
                    autoseat.CreatedBy = RoleId;
                    autoseat.CreatedOn = DateTime.Now;
                    _db.tbl_SeatAllocation_SeatMatrix.Add(autoseat);
                    _db.SaveChanges();

                    foreach (var item in result)
                    {
                        var preference_colleges = _db.tbl_Applicant_InstitutePreference.Where(x => x.IsActive == true && x.ApplicantId == item.ApplicantId).ToList();
                        int flag = 1;
                        foreach (var itm in preference_colleges)
                        {
                            int seamax_id = _db.tbl_SeatMatrix_Main.Where(x => x.IsActive == true && x.InstituteId == itm.InstituteId && x.ApplicantType == applicantType && x.Round == round && x.AcademicYear == academicYear && x.CourseTypeId == courseType).Select(y => y.SeatMaxId).FirstOrDefault();
                            int allocationId = _db.tbl_SeatAllocation_SeatMatrix.Where(x => x.IsActive == true && x.ApplicantType == applicantType && x.Round == round && x.AcademicYear == academicYear && x.CourseTypeId == courseType).Select(y => y.AllocationId).FirstOrDefault();
                            int seats = GetSeatsAvailableRound2(itm.InstituteId, itm.TradeId, seamax_id, allocationId);
                            if (seats != 0)
                            {
                                var allocId = _db.tbl_SeatAllocation_SeatMatrix.OrderByDescending(x => x.AllocationId).Select(y => y.AllocationId).FirstOrDefault();
                                var seat = new tbl_SeatAllocationDetail_Seatmatrix();
                                seat.AllocationId = allocId;
                                seat.RankNumber = item.Rank;
                                seat.ApplicantId = item.ApplicantId;
                                seat.InstituteId = itm.InstituteId;
                                seat.TradeId = itm.TradeId;
                                seat.HorizontalId = item.HCategory;
                                seat.VerticalId = item.VCategory;
                                seat.PreferenceNum = flag;
                                seat.Status = 1;
                                seat.IsActive = true;
                                seat.CreatedBy = RoleId;
                                seat.CreatedOn = DateTime.Now;
                                _db.tbl_SeatAllocationDetail_Seatmatrix.Add(seat);
                                break;
                            }
                            flag++;
                        }
                    }
                    foreach (var item in result)
                    {
                        var preference_colleges = _db.tbl_Applicant_InstitutePreference.Where(x => x.IsActive == true && x.ApplicantId == item.ApplicantId).ToList();
                        foreach (var itm in preference_colleges)
                        {
                            int seamax_id = _db.tbl_SeatMatrix_Main.Where(x => x.IsActive == true && x.InstituteId == itm.InstituteId && x.ApplicantType == applicantType && x.Round == round && x.AcademicYear == academicYear && x.CourseTypeId == courseType).Select(y => y.SeatMaxId).FirstOrDefault();
                            int allocationId = _db.tbl_SeatAllocation_SeatMatrix.Where(x => x.IsActive == true && x.ApplicantType == applicantType && x.Round == round && x.AcademicYear == academicYear && x.CourseTypeId == courseType).Select(y => y.AllocationId).FirstOrDefault();
                            HorizontalSeats seats = GpAllocation(itm.InstituteId, itm.TradeId, seamax_id, allocationId);
                            var tradewise = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == seamax_id && x.TradeId == itm.TradeId).FirstOrDefault();
                            tradewise.GMGPNH = seats.GM;
                            tradewise.SCGPNH = seats.SC;
                            tradewise.STGPNH = seats.ST;
                            tradewise.C1GPNH = seats.C1;
                            tradewise.TWOAGPNH = seats.IIA;
                            tradewise.TWOBGPNH = seats.IIB;
                            tradewise.THREEAGPNH = seats.IIIA;
                            tradewise.THREEBGPNH = seats.IIIB;
                            _db.SaveChanges();
                        }
                    }


                    _db.SaveChanges();
                    trasaction.Commit();

                    var res = (from aa in _db.tbl_SeatAllocationDetail_Seatmatrix
                               join bb in _db.tbl_GradationRank_Trans on aa.ApplicantId equals bb.ApplicantId
                               join cc in _db.tbl_Applicant_Detail on aa.ApplicantId equals cc.ApplicationId
                               join dd in _db.tbl_location_type on cc.ApplicantBelongTo equals dd.location_id
                               join ee in _db.tbl_iti_college_details on aa.InstituteId equals ee.iti_college_id
                               join ff in _db.tbl_district_master on ee.district_id equals ff.district_lgd_code
                               join gg in _db.tbl_Institute_type on ee.Insitute_TypeId equals gg.Institute_type_id
                               join hh in _db.tbl_trade_mast on aa.TradeId equals hh.trade_id
                               join ii in _db.tbl_Vertical_rules on aa.VerticalId equals ii.Vertical_rules_id
                               join jj in _db.Tbl_horizontal_rules on aa.HorizontalId equals jj.Horizontal_rules_id
                               join kk in _db.tbl_Gender on cc.Gender equals kk.Gender_Id
                               join ll in _db.tbl_SeatAllocation_SeatMatrix on aa.AllocationId equals ll.AllocationId
                               where bb.Final == true && bb.Status == 2 && ll.Round == 1
                               select new SeatAutoAllocationModel
                               {
                                   Rank = bb.Rank,
                                   DivisionName = dd.location_name,
                                   DistrictName = ff.district_ename,
                                   MISCode = ee.MISCode,
                                   ITIType = gg.Institute_type,
                                   ITIName = ee.iti_college_name,
                                   TradeName = hh.trade_name,
                                   AllottedCategory = ii.Vertical_Rules,
                                   AllottedGroup = jj.Horizontal_rules,
                                   LocalStatus = "Hyd/NonHyd",
                                   PreferenceNumber = aa.PreferenceNum,
                                   FirstName = cc.ApplicantName,
                                   FatherName = cc.FathersName,
                                   Gender = kk.Gender,
                                   DOB = cc.DOB,
                                   Remarks = ll.Remarks
                               }).ToList();
                    return res;
                }
                catch (Exception e)
                {
                    trasaction.Rollback();
                    throw e;
                }
            }
        }
        int GetSeatsAvailableRound2(int instituteId, int tradeId, int seamax_id, int allocationId)
        {
            var gm_seats = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == seamax_id && x.TradeId == tradeId).FirstOrDefault();
            int? total = gm_seats.GMGPH + gm_seats.GMGPNH + gm_seats.GMWH + gm_seats.GMWNH + gm_seats.GMPDH + gm_seats.GMPDNH + gm_seats.GMEXSH + gm_seats.GMEXSNH + gm_seats.GMKMH + gm_seats.GMKMNH + gm_seats.GMEWSH + gm_seats.GMEWSNH
                + gm_seats.SCGPH + gm_seats.SCGPNH + gm_seats.SCWH + gm_seats.SCWNH + gm_seats.SCPDH + gm_seats.SCPDNH + gm_seats.SCEXSH + gm_seats.SCEXSNH + gm_seats.SCKMH + gm_seats.SCKMNH + gm_seats.SCEWSH + gm_seats.SCEWSNH
                + gm_seats.STGPH + gm_seats.STGPNH + gm_seats.STWH + gm_seats.STWNH + gm_seats.STPDH + gm_seats.STPDNH + gm_seats.STEXSH + gm_seats.STEXSNH + gm_seats.STKMH + gm_seats.STKMNH + gm_seats.STEWSH + gm_seats.STEWSNH
                + gm_seats.C1GPH + gm_seats.C1GPNH + gm_seats.C1WH + gm_seats.C1WNH + gm_seats.C1PDH + gm_seats.C1PDNH + gm_seats.C1EXSH + gm_seats.C1EXSNH + gm_seats.C1KMH + gm_seats.C1KMNH + gm_seats.C1EWSH + gm_seats.C1EWSNH
                + gm_seats.TWOAGPH + gm_seats.TWOAGPNH + gm_seats.TWOAWH + gm_seats.TWOAWNH + gm_seats.TWOAPDH + gm_seats.TWOAPDNH + gm_seats.TWOAEXSH + gm_seats.TWOAEXSNH + gm_seats.TWOAKMH + gm_seats.TWOAKMNH + gm_seats.TWOAEWSH + gm_seats.TWOAEWSNH
                + gm_seats.TWOBGPH + gm_seats.TWOBGPNH + gm_seats.TWOBWH + gm_seats.TWOBWNH + gm_seats.TWOBPDH + gm_seats.TWOBPDNH + gm_seats.TWOBEXSH + gm_seats.TWOBEXSNH + gm_seats.TWOBKMH + gm_seats.TWOBKMNH + gm_seats.TWOBEWSH + gm_seats.TWOBEWSNH
                + gm_seats.THREEAGPH + gm_seats.THREEAGPNH + gm_seats.THREEAWH + gm_seats.THREEAWNH + gm_seats.THREEAPDH + gm_seats.THREEAPDNH + gm_seats.THREEAEXSH + gm_seats.THREEAEXSNH + gm_seats.THREEAKMH + gm_seats.THREEAKMNH + gm_seats.THREEAEWSH + gm_seats.THREEAEWSNH
                + gm_seats.THREEBGPH + gm_seats.THREEBGPNH + gm_seats.THREEAWH + gm_seats.THREEAWNH + gm_seats.THREEAPDH + gm_seats.THREEAPDNH + gm_seats.THREEAEXSH + gm_seats.THREEAEXSNH + gm_seats.THREEAKMH + gm_seats.THREEAKMNH + gm_seats.THREEAEWSH + gm_seats.THREEAEWSNH;
            int allotted_seats = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId).Count();
            int totalseats = Convert.ToInt32(total) - allotted_seats;
            return totalseats;
        }
        HorizontalSeats GpAllocation(int instituteId, int tradeId, int seamax_id, int allocationId)
        {
            HorizontalSeats seat = new HorizontalSeats();
            var gm_seats = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == seamax_id && x.TradeId == tradeId && x.IsActive == true).FirstOrDefault();
            seat.GM = gm_seats.GMGPH + gm_seats.GMGPNH + gm_seats.GMWH + gm_seats.GMWNH + gm_seats.GMPDH + gm_seats.GMPDNH + gm_seats.GMEXSH + gm_seats.GMEXSNH + gm_seats.GMKMH + gm_seats.GMKMNH + gm_seats.GMEWSH + gm_seats.GMEWSNH;
            seat.SC = gm_seats.SCGPH + gm_seats.SCGPNH + gm_seats.SCWH + gm_seats.SCWNH + gm_seats.SCPDH + gm_seats.SCPDNH + gm_seats.SCEXSH + gm_seats.SCEXSNH + gm_seats.SCKMH + gm_seats.SCKMNH + gm_seats.SCEWSH + gm_seats.SCEWSNH;
            seat.ST = gm_seats.STGPH + gm_seats.STGPNH + gm_seats.STWH + gm_seats.STWNH + gm_seats.STPDH + gm_seats.STPDNH + gm_seats.STEXSH + gm_seats.STEXSNH + gm_seats.STKMH + gm_seats.STKMNH + gm_seats.STEWSH + gm_seats.STEWSNH;
            seat.C1 = gm_seats.C1GPH + gm_seats.C1GPNH + gm_seats.C1WH + gm_seats.C1WNH + gm_seats.C1PDH + gm_seats.C1PDNH + gm_seats.C1EXSH + gm_seats.C1EXSNH + gm_seats.C1KMH + gm_seats.C1KMNH + gm_seats.C1EWSH + gm_seats.C1EWSNH;
            seat.IIA = gm_seats.TWOAGPH + gm_seats.TWOAGPNH + gm_seats.TWOAWH + gm_seats.TWOAWNH + gm_seats.TWOAPDH + gm_seats.TWOAPDNH + gm_seats.TWOAEXSH + gm_seats.TWOAEXSNH + gm_seats.TWOAKMH + gm_seats.TWOAKMNH + gm_seats.TWOAEWSH + gm_seats.TWOAEWSNH;
            seat.IIB = gm_seats.TWOBGPH + gm_seats.TWOBGPNH + gm_seats.TWOBWH + gm_seats.TWOBWNH + gm_seats.TWOBPDH + gm_seats.TWOBPDNH + gm_seats.TWOBEXSH + gm_seats.TWOBEXSNH + gm_seats.TWOBKMH + gm_seats.TWOBKMNH + gm_seats.TWOBEWSH + gm_seats.TWOBEWSNH;
            seat.IIIA = gm_seats.THREEAGPH + gm_seats.THREEAGPNH + gm_seats.THREEAWH + gm_seats.THREEAWNH + gm_seats.THREEAPDH + gm_seats.THREEAPDNH + gm_seats.THREEAEXSH + gm_seats.THREEAEXSNH + gm_seats.THREEAKMH + gm_seats.THREEAKMNH + gm_seats.THREEAEWSH + gm_seats.THREEAEWSNH;
            seat.IIIB = gm_seats.THREEBGPH + gm_seats.THREEBGPNH + gm_seats.THREEAWH + gm_seats.THREEAWNH + gm_seats.THREEAPDH + gm_seats.THREEAPDNH + gm_seats.THREEAEXSH + gm_seats.THREEAEXSNH + gm_seats.THREEAKMH + gm_seats.THREEAKMNH + gm_seats.THREEAEWSH + gm_seats.THREEAEWSNH;

            int GM = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId && x.VerticalId == 1).Count();
            int SC = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId && x.VerticalId == 2).Count();
            int ST = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId && x.VerticalId == 3).Count();
            int C1 = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId && x.VerticalId == 4).Count();
            int IIA = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId && x.VerticalId == 5).Count();
            int IIB = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId && x.VerticalId == 6).Count();
            int IIIA = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId && x.VerticalId == 7).Count();
            int IIIB = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == allocationId && x.TradeId == tradeId && x.VerticalId == 8).Count();

            seat.GM = seat.GM - Convert.ToInt32(GM);
            seat.SC = seat.SC - Convert.ToInt32(SC);
            seat.ST = seat.ST - Convert.ToInt32(ST);
            seat.C1 = seat.C1 - Convert.ToInt32(C1);
            seat.IIA = seat.IIA - Convert.ToInt32(IIA);
            seat.IIB = seat.IIB - Convert.ToInt32(IIB);
            seat.IIIA = seat.IIIA - Convert.ToInt32(IIIA);
            seat.IIIB = seat.IIIB - Convert.ToInt32(IIIB);
            return seat;
        }
        public List<SeatAutoAllocationModel> Round5GenerateSeatAutoAllotment(int courseType, int applicantType, int academicYear, int round, int loginId)
        {
            using (var trasaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = (from bb in _db.tbl_Applicant_Detail
                                  join cc in _db.tbl_GradationRank_Trans on bb.ApplicationId equals cc.ApplicantId
                                  join dd in _db.tbl_Applicant_Reservation on bb.ApplicationId equals dd.ApplicantId
                                  where cc.Final == true && cc.Status == 2 && bb.IsActive == true && bb.ApplicantType == 2
                                  select new AutoAllocation
                                  {
                                      ApplicantId = bb.ApplicationId,
                                      Rank = cc.Rank,
                                      Gender = bb.Gender,
                                      VCategory = bb.Caste,
                                      Religion = bb.Religion,
                                      Minority = bb.MinorityCategory,
                                      PhysicalHandicap = bb.PhysicallyHanidcapInd,
                                      HCategory = dd.ReservationId
                                  }).OrderBy(x => x.Rank).ToList();

                    var autoseat = new tbl_SeatAllocation_SeatMatrix();
                    autoseat.AcademicYear = academicYear;
                    autoseat.ApplicantType = applicantType;
                    autoseat.Round = round;
                    autoseat.Status = 1;
                    autoseat.Remarks = "seat allotted";
                    autoseat.IsActive = true;
                    autoseat.CourseTypeId = courseType;
                    autoseat.CreatedBy = loginId;
                    autoseat.CreatedOn = DateTime.Now;
                    _db.tbl_SeatAllocation_SeatMatrix.Add(autoseat);
                    _db.SaveChanges();

                    foreach (var item in result)
                    {
                        var preference_colleges = _db.tbl_Applicant_InstitutePreference.Where(x => x.IsActive == true && x.ApplicantId == item.ApplicantId).ToList();
                        int flag = 1;
                        foreach (var itm in preference_colleges)
                        {
                            int seamax_id = _db.tbl_SeatMatrix_Main.Where(x => x.IsActive == true && x.InstituteId == itm.InstituteId && x.ApplicantType == applicantType && x.Round == round && x.AcademicYear == academicYear && x.CourseTypeId == courseType).Select(y => y.SeatMaxId).FirstOrDefault();
                            int allocationId = _db.tbl_SeatAllocation_SeatMatrix.Where(x => x.IsActive == true && x.ApplicantType == applicantType && x.Round == round && x.AcademicYear == academicYear && x.CourseTypeId == courseType).Select(y => y.AllocationId).FirstOrDefault();
                            //int seats = GetSeatsAvailableFor3_4(Convert.ToInt32(item.VCategory), item.HCategory, itm.InstituteId, itm.TradeId, seamax_id, allocationId);
                            int seats = 0;
                            if (seats != 0)
                            {
                                var allocId = _db.tbl_SeatAllocation_SeatMatrix.OrderByDescending(x => x.AllocationId).Select(y => y.AllocationId).FirstOrDefault();
                                var seat = new tbl_SeatAllocationDetail_Seatmatrix();
                                seat.AllocationId = allocId;
                                seat.RankNumber = item.Rank;
                                seat.ApplicantId = item.ApplicantId;
                                seat.InstituteId = itm.InstituteId;
                                seat.TradeId = itm.TradeId;
                                seat.HorizontalId = item.HCategory;
                                seat.VerticalId = item.VCategory;
                                seat.PreferenceNum = flag;
                                seat.Status = 1;
                                seat.IsActive = true;
                                seat.CreatedBy = loginId;
                                seat.CreatedOn = DateTime.Now;
                                _db.tbl_SeatAllocationDetail_Seatmatrix.Add(seat);
                                break;
                            }
                            flag++;
                        }
                    }
                    _db.SaveChanges();
                    trasaction.Commit();

                    var res = (from aa in _db.tbl_SeatAllocationDetail_Seatmatrix
                               join bb in _db.tbl_GradationRank_Trans on aa.ApplicantId equals bb.ApplicantId
                               join cc in _db.tbl_Applicant_Detail on aa.ApplicantId equals cc.ApplicationId
                               join dd in _db.tbl_location_type on cc.ApplicantBelongTo equals dd.location_id
                               join ee in _db.tbl_iti_college_details on aa.InstituteId equals ee.iti_college_id
                               join ff in _db.tbl_district_master on ee.district_id equals ff.district_lgd_code
                               join gg in _db.tbl_Institute_type on ee.Insitute_TypeId equals gg.Institute_type_id
                               join hh in _db.tbl_trade_mast on aa.TradeId equals hh.trade_id
                               join ii in _db.tbl_Vertical_rules on aa.VerticalId equals ii.Vertical_rules_id
                               join jj in _db.Tbl_horizontal_rules on aa.HorizontalId equals jj.Horizontal_rules_id
                               join kk in _db.tbl_Gender on cc.Gender equals kk.Gender_Id
                               join ll in _db.tbl_SeatAllocation_SeatMatrix on aa.AllocationId equals ll.AllocationId
                               where bb.Final == true && bb.Status == 2 && ll.Round == 1
                               select new SeatAutoAllocationModel
                               {
                                   Rank = bb.Rank,
                                   DivisionName = dd.location_name,
                                   DistrictName = ff.district_ename,
                                   MISCode = ee.MISCode,
                                   ITIType = gg.Institute_type,
                                   ITIName = ee.iti_college_name,
                                   TradeName = hh.trade_name,
                                   AllottedCategory = ii.Vertical_Rules,
                                   AllottedGroup = jj.Horizontal_rules,
                                   LocalStatus = "Hyd/NonHyd",
                                   PreferenceNumber = aa.PreferenceNum,
                                   FirstName = cc.ApplicantName,
                                   FatherName = cc.FathersName,
                                   Gender = kk.Gender,
                                   DOB = cc.DOB,
                                   Remarks = ll.Remarks
                               }).ToList();
                    return res;
                }
                catch (Exception e)
                {
                    trasaction.Rollback();
                    throw e;
                }
            }
        }


        #endregion

        public string DirectAdmissionSeatAllotmentDLL(InstituteWiseAdmission model)
        {
            string msg = "";
            int round = 0;    // Set round to 0 for Direct Admission
            //int allocIdTransDet = 0;
            int statId = 21;
            using (var trasaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var ApplDetails = _db.tbl_Applicant_Detail.Where(a => a.ApplicationId == model.ApplicationId).Select(x => new { x.DistrictId, x.ApplicantType }).FirstOrDefault();

                    int academicYear = _db.tbl_Year.Where(a => a.IsActive == true && model.ApplyYear.ToString() == a.Year).Select(a=> a.YearID).FirstOrDefault();

                    int CreatedByExisting = _db.tbl_SeatAllocation_SeatMatrix.Where(x => x.IsActive == true && x.AcademicYear == academicYear
                                            && x.CourseTypeId == 101
                                            && x.ApplicantType == ApplDetails.ApplicantType && x.Round == round).
                                            Select(y => y.AllocationId).FirstOrDefault();
                    List<SeatAutoAllocationModel> res = new List<SeatAutoAllocationModel>();
                    if (CreatedByExisting == 0)
                    {
                        var autoseat = new tbl_SeatAllocation_SeatMatrix();
                        autoseat.AcademicYear = academicYear;
                        autoseat.ApplicantType = (int)ApplDetails.ApplicantType;
                        autoseat.Round = round;
                        autoseat.Status = statId;
                        autoseat.Remarks = _db.tbl_status_master.Where(a => a.StatusId == statId).Select(x => x.StatusName).FirstOrDefault();
                        autoseat.IsActive = true;
                        autoseat.CreatedBy = Convert.ToInt32(model.userRole);
                        autoseat.CreatedOn = DateTime.Now;
                        autoseat.CourseTypeId = 101;
                        autoseat.FlowId = Convert.ToInt32(model.userRole);
                        _db.tbl_SeatAllocation_SeatMatrix.Add(autoseat);
                        _db.SaveChanges();

                        var seatTrans = new tbl_SeatAllocation_SeatMatrix_Trans();
                        seatTrans.AllocationId = autoseat.AllocationId;
                        seatTrans.Status = statId;
                        seatTrans.IsActive = true;
                        seatTrans.CreatedBy = Convert.ToInt32(model.userRole);
                        seatTrans.CreatedOn = DateTime.Now;
                        seatTrans.FlowId = Convert.ToInt32(model.userRole);
                        seatTrans.Remarks = _db.tbl_status_master.Where(a => a.StatusId == statId).Select(x => x.StatusName).FirstOrDefault();
                        _db.tbl_SeatAllocation_SeatMatrix_Trans.Add(seatTrans);
                        _db.SaveChanges();

                    }
                    int InstituteId = (from tad in _db.Staff_Institute_Detail
                                       where tad.UserId == model.CreatedBy && tad.IsActive == true
                                       select tad.InstituteId).FirstOrDefault();

                    //int seamax_id = _db.tbl_SeatMatrix_Main.Where(x => x.IsActive == true && x.InstituteId == itm.InstituteId && x.ApplicantType == applicantType && x.Round == round && x.AcademicYear == academicYear && x.CourseTypeId == courseType).Select(y => y.SeatMaxId).FirstOrDefault();
                    //int allocationId = _db.tbl_SeatAllocation_SeatMatrix.Where(x => x.IsActive == true && x.ApplicantType == applicantType && x.Round == round && x.AcademicYear == academicYear && x.CourseTypeId == courseType).Select(y => y.AllocationId).FirstOrDefault();
                    //int seats = GetSeatsAvailable(itm.InstituteId, itm.TradeId, seamax_id, allocationId);
                    //if (seats != 0)
                    //{


                    int? HNHId = (from aa in _db.tbl_Applicant_Detail
                                 join bb in _db.tbl_district_master on aa.DistrictId equals bb.district_lgd_code
                                 join cc in _db.tbl_division_master on bb.division_id equals cc.division_id
                                 where cc.division_is_active == true && bb.district_lgd_code == ApplDetails.DistrictId
                                 select cc.Hyd_NonHyd_region_id).FirstOrDefault();

                    var allocId = _db.tbl_SeatAllocation_SeatMatrix.OrderByDescending(x => x.AllocationId).Select(y => y.AllocationId).FirstOrDefault();
                    var seat = new tbl_SeatAllocationDetail_Seatmatrix();
                    seat.AllocationId = allocId;
                    seat.RankNumber = 0;
                    seat.ApplicantId = model.ApplicationId;
                    seat.InstituteId = InstituteId;
                    seat.TradeId = model.TradeId;
                    seat.HorizontalId = Convert.ToInt32(model.HorizontalCategory);
                    seat.VerticalId = Convert.ToInt32(model.VerticalCategory);
                    seat.HyrNonHydrId = HNHId;
                    seat.PreferenceNum = 0;
                    seat.Status = statId;
                    seat.IsActive = true;
                    seat.CreatedBy = model.CreatedBy;
                    seat.UnitId = model.Unitid;
                    seat.ShiftId = model.Shiftid;
                    seat.FlowId = model.CreatedBy;
                    seat.CreatedOn = DateTime.Now;
                    seat.AllocByCategory = "MGMT";
                    seat.InstitutePreferenceId = 0;
                    _db.tbl_SeatAllocationDetail_Seatmatrix.Add(seat);
                    _db.SaveChanges();
                    //}

                    trasaction.Commit();
                    msg = "Data Saved Successfully!!";

                }
                catch (Exception e)
                {
                    trasaction.Rollback();
                    throw e;
                }
                return msg;
            }
        }
        
        public bool ForwardSeatAutoAllotment(List<int> allocationId, int loginId, int roleId, string Remarks, int Status)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    int FlagToTrans = 0;
                    if (allocationId != null && roleId != 0)
                    {
                        foreach (var itm in allocationId)
                        {
                            var singleseat = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.AllocationId == itm).FirstOrDefault();
                            if (singleseat != null)
                            {
                                singleseat.FlowId = roleId;
                                _db.SaveChanges();
                                FlagToTrans++;
                            }
                            if (FlagToTrans == 1)
                            {
                                var allocIdTransId = _db.tbl_SeatAllocation_SeatMatrix.OrderByDescending(x => x.AllocationId == itm).FirstOrDefault();
                                var seatTrans = new tbl_SeatAllocation_SeatMatrix_Trans();
                                seatTrans.AllocationId = allocIdTransId.AllocationId;
                                seatTrans.FlowId = roleId;
                                seatTrans.Remarks = Remarks;
                                seatTrans.Status = Status;
                                seatTrans.CreatedBy = loginId;
                                seatTrans.IsActive = true;
                                seatTrans.CreatedOn = DateTime.Now;
                                _db.tbl_SeatAllocation_SeatMatrix_Trans.Add(seatTrans);
                                _db.SaveChanges();

                                var singleseatSeatMatrix = _db.tbl_SeatAllocation_SeatMatrix.Where(x => x.AllocationId == allocIdTransId.AllocationId).FirstOrDefault();
                                if (singleseatSeatMatrix != null)
                                {
                                    singleseatSeatMatrix.FlowId = roleId;
                                    singleseatSeatMatrix.Status = Status;
                                    _db.SaveChanges();
                                }
                            }
                        }

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
        #endregion

        #region Update Seat Auto Allocation
        public List<SeatAutoAllocationModel> GetGeneratedSeatAutoAllotmentList(int courseType, int applicantType, int academicYear, int round)
        {
            try
            {
                if (courseType != 0)
                {
                    var res = (from bb in _db.tbl_SeatAllocation_SeatMatrix
                               join cc in _db.tbl_course_type_mast on bb.CourseTypeId equals cc.course_id
                               join dd in _db.tbl_ApplicantType on bb.ApplicantType equals dd.ApplicantTypeId
                               join ee in _db.tbl_status_master on bb.Status equals ee.StatusId
                               join ff in _db.tbl_Year on bb.AcademicYear equals ff.YearID
                               join gg in _db.tbl_ApplicantAdmissionRounds on bb.Round equals gg.ApplicantAdmissionRoundsId
                               join hh in _db.tbl_role_master on bb.FlowId equals hh.role_id
                               where bb.CourseTypeId == courseType && bb.ApplicantType == applicantType
                               && bb.AcademicYear == academicYear && bb.Round == round && bb.IsActive == true && bb.Status != 2
                               select new SeatAutoAllocationModel
                               {
                                   FlowId = bb.FlowId,
                                   CourseType = cc.course_type_name,
                                   ApplicantType = dd.ApplicantType,
                                   AcademicYearSm = ff.Year,
                                   RoundSm = gg.RoundList,
                                   Status =
                               (
                                    ee.StatusName == "Approve" ? "Approved" : ee.StatusName
                               ),
                                   AllocationId = bb.AllocationId,
                                   userRole = hh.role_DescShortForm
                               }).ToList();
                    return res;
                }
                else
                {
                    var res = (from bb in _db.tbl_SeatAllocation_SeatMatrix
                               join cc in _db.tbl_course_type_mast on bb.CourseTypeId equals cc.course_id
                               join dd in _db.tbl_ApplicantType on bb.ApplicantType equals dd.ApplicantTypeId
                               join ee in _db.tbl_status_master on bb.Status equals ee.StatusId
                               join ff in _db.tbl_Year on bb.AcademicYear equals ff.YearID
                               join gg in _db.tbl_ApplicantAdmissionRounds on bb.Round equals gg.ApplicantAdmissionRoundsId
                               join hh in _db.tbl_role_master on bb.FlowId equals hh.role_id
                               where bb.IsActive == true && bb.Status != (int)CmnClass.Status.Approve
                               select new SeatAutoAllocationModel
                               {
                                   FlowId = bb.FlowId,
                                   CourseType = cc.course_type_name,
                                   ApplicantType = dd.ApplicantType,
                                   AcademicYearSm = ff.Year,
                                   RoundSm = gg.RoundList,
                                   Status =
                               (
                                    ee.StatusName == "Approve" ? "Approved" : ee.StatusName
                               ),
                                   AllocationId = bb.AllocationId,
                                   userRole = hh.role_DescShortForm
                               }).ToList();
                    return res;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SeatAutoAllocationModel> ViewSeatAutoAllotment(int allocationId)
        {
            try
            {

                var res = (from a in _db.tbl_SeatAllocationDetail_Seatmatrix
                           join b in _db.tbl_trade_mast on a.TradeId equals b.trade_id
                           join c in _db.tbl_iti_college_details on a.InstituteId equals c.iti_college_id
                           join d in _db.tbl_district_master on c.district_id equals d.district_lgd_code
                           join e in _db.tbl_division_master on c.division_id equals e.division_id
                           join f in _db.tbl_Institute_type on c.Insitute_TypeId equals f.Institute_type_id
                           join g in _db.tbl_Applicant_Detail on a.ApplicantId equals g.ApplicationId
                           join h in _db.tbl_Gender on g.Gender equals h.Gender_Id
                           join i in _db.tbl_Category on a.VerticalId equals i.CategoryId
                           //join i in _db.tbl_Vertical_rules on a.VerticalId equals i.Vertical_rules_id
                           join j in _db.Tbl_horizontal_rules on a.HorizontalId equals j.Horizontal_rules_id
                           join k in _db.tbl_units on a.UnitId equals k.u_id
                           join l in _db.tbl_shifts on a.ShiftId equals l.s_id
                           join aa in _db.tbl_SeatAllocation_SeatMatrix on a.AllocationId equals aa.AllocationId
                           join bb in _db.tbl_taluk_master on c.taluk_id equals bb.taluk_lgd_code
                           join cc in _db.tbl_HYD_NonHYD_regions on a.HyrNonHydrId equals cc.Hyd_NonHyd_region_id
                           join dd in _db.tbl_status_master on a.Status equals dd.StatusId

                           where a.AllocationId == allocationId
                           select new SeatAutoAllocationModel
                           {
                               AllocationId = a.AllocationId,
                               RankNumber = a.RankNumber,
                               Status = dd.StatusName + " - " + aa.Round,
                               division_id = e.division_id,
                               division_name = e.division_name,
                               district_id = d.district_lgd_code,
                               district_ename = d.district_ename,
                               TalukId = bb.taluk_lgd_code,
                               TalukName = bb.taluk_ename,
                               MISCode = c.MISCode,
                               InstituteType = f.Institute_type,
                               InstituteName = c.iti_college_name,
                               TradeCode = b.trade_id,
                               TradeName = b.trade_name,
                               AllottedCategory = i.Category,
                               AllottedGroup = j.Horizontal_rules,
                               PreferenceNumber = a.PreferenceNum,
                               UnitsDet = k.units,
                               ShiftsDet = l.shifts,
                               FirstName = g.ApplicantName,
                               FatherName = g.FathersName,
                               LocalStatus = cc.Region_type,
                               Gender = h.Gender,
                               DOB = g.DOB,
                               DateOfBirth = g.DOB.ToString(),
                               MobileNumber = g.PhoneNumber,
                               FlowId = aa.FlowId

                           }).ToList();
                List<SeatAutoAllocationModel> res1 = (from aaa in _db.tbl_SeatAllocationDetail_Seatmatrix
                                                      join ccc in _db.tbl_Applicant_Detail on aaa.ApplicantId equals ccc.ApplicationId
                                                      join ddd in _db.tbl_location_type on ccc.ApplicantBelongTo equals ddd.location_id
                                                      join eee in _db.tbl_Gender on ccc.Gender equals eee.Gender_Id
                                                      join fff in _db.tbl_SeatAllocation_SeatMatrix on aaa.AllocationId equals fff.AllocationId
                                                      join ggg in _db.tbl_status_master on aaa.Status equals ggg.StatusId

                                                      where aaa.Status == 23
                                                      select new SeatAutoAllocationModel
                                                      {
                                                          RankNumber = aaa.RankNumber,
                                                          Status = ggg.StatusName + " - " + fff.Round,
                                                          seatAllocDetailId = fff.AllocationId,
                                                          PreferenceNumber = aaa.PreferenceNum,
                                                          FirstName = ccc.ApplicantName,
                                                          FatherName = ccc.FathersName,
                                                          Gender = eee.Gender,
                                                          MobileNumber = ccc.PhoneNumber,
                                                          DOB = ccc.DOB,
                                                          DateOfBirth = ccc.DOB.ToString(),
                                                          Remarks = fff.Remarks
                                                      }).ToList();

                res.AddRange(res1);

                return res.OrderBy(a=> a.RankNumber).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Seat Allocation Review & Recommand Of Seat Matrix
        //ram use case-35 & Screen 55&56
        public List<ReviewSeatAllocated> GenerateSeatAllotmentReviewDLL(int ddlCourseTypesGen, int ddlApplicantTypeGen, int ddlAcademicYearGen, int ddlRoundGen, int id)
        {
            try
            {
                var res = (from tram in _db.tbl_SeatAllocation_SeatMatrix
                               //join ty in _db.tbl_Year on tram.AcademicYear equals ty.YearID
                           join tctm in _db.tbl_course_type_mast on tram.CourseTypeId equals tctm.course_id
                           join apt in _db.tbl_ApplicantType on tram.ApplicantType equals apt.ApplicantTypeId
                           join tsm in _db.tbl_status_master on tram.Status equals tsm.StatusId
                           where tram.IsActive == true

                           select new ReviewSeatAllocated
                           {
                               AllocationId = tram.AllocationId,
                               CourseName = tctm.course_type_name,
                               ApplicantTypeDdl = apt.ApplicantType,
                               ExamYear = tram.AcademicYear,
                               Round = tram.Round,
                               StatusName = tsm.StatusName,
                               Remarks = tram.Remarks,
                               //FormwardFrom = tur.um_name
                           }).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SeatMatrixAllocationDetail> GeneratedSeatAllotmentReviewListDLL(int id)
        {
            try
            {
                var res = (from a in _db.tbl_SeatAllocationDetail_Seatmatrix
                           join b in _db.tbl_trade_mast on a.TradeId equals b.trade_id
                           join c in _db.tbl_iti_college_details on a.InstituteId equals c.Insitute_TypeId
                           join d in _db.tbl_district_master on c.district_id equals d.district_lgd_code
                           join e in _db.tbl_division_master on c.division_id equals e.division_id
                           join f in _db.tbl_Institute_type on c.Insitute_TypeId equals f.Institute_type_id
                           join g in _db.tbl_Applicant_Detail on a.AllocationDetailId equals g.ApplicationId
                           join h in _db.tbl_Gender on g.Gender equals h.Gender_Id
                           join i in _db.tbl_Vertical_rules on a.VerticalId equals i.Vertical_rules_id
                           join j in _db.Tbl_horizontal_rules on a.HorizontalId equals j.Horizontal_rules_id
                           where a.AllocationId == id

                           select new SeatMatrixAllocationDetail
                           {
                               AllocationId = a.AllocationId,
                               RankNumber = a.RankNumber,
                               division_id = e.division_id,
                               division_name = e.division_name,
                               district_id = d.district_lgd_code,
                               district_ename = d.district_ename,
                               MISCode = c.MISCode,
                               InstituteType = f.Institute_type,
                               InstituteName = c.iti_college_name,
                               TradeCode = b.trade_id,
                               TradeName = b.trade_name,
                               AllottedCategory = i.Vertical_Rules,
                               AllottedGroup = j.Horizontal_rules,
                               PreferenceNumber = a.PreferenceNum,
                               LocalStatus = "Hyd/NonHyd",
                               FirstName = g.ApplicantName,
                               FatherName = g.FathersName,
                               Gender = h.Gender,
                               DOB = g.DOB,
                               FlowId = a.FlowId

                           }).OrderByDescending(x => x.AllocationId).Take(1).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SeatAutoAllocationModel> GetSeatMatrixViewListDLL(int courseType, int applicantType, int academicYear, int round)
        {
            try
            {

                var res = (from bb in _db.tbl_SeatAllocation_SeatMatrix
                           join cc in _db.tbl_course_type_mast on bb.CourseTypeId equals cc.course_id
                           join dd in _db.tbl_ApplicantType on bb.ApplicantType equals dd.ApplicantTypeId
                           join ee in _db.tbl_status_master on bb.Status equals ee.StatusId
                           join ff in _db.tbl_Year on bb.AcademicYear equals ff.YearID
                           join gg in _db.tbl_ApplicantAdmissionRounds on bb.Round equals gg.ApplicantAdmissionRoundsId
                           join hh in _db.tbl_role_master on bb.FlowId equals hh.role_id
                           where bb.CourseTypeId == courseType && bb.ApplicantType == applicantType
                           && bb.AcademicYear == academicYear && bb.Round == round && bb.IsActive == true && bb.Status == 2
                           select new SeatAutoAllocationModel
                           {
                               FlowId = bb.FlowId,
                               CourseType = cc.course_type_name,
                               ApplicantType = dd.ApplicantType,
                               AcademicYearSm = ff.Year,
                               RoundSm = gg.RoundList,
                               Status =
                               (
                                    ee.StatusName == "Approve" ? "Approved" : ee.StatusName
                               ),
                               AllocationId = bb.AllocationId,
                               userRole = hh.role_DescShortForm
                           }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ForwardSeatAutoAllotmentReviewDLL(List<int> allocationId, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (allocationId != null && roleId != 0)
                    {
                        foreach (var itm in allocationId)
                        {
                            var singleseat = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.AllocationDetailId == itm).FirstOrDefault();
                            singleseat.FlowId = roleId;
                        }
                        _db.SaveChanges();
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

        public List<SeatMatrixAllocationDetail> GetCommentsListSeatAllocationDLL(int SeatAllocationId)
        {
            var res = (from tram in _db.tbl_SeatAllocation_SeatMatrix_Trans
                       join tur in _db.tbl_role_master on tram.CreatedBy equals tur.role_id
                       join turs in _db.tbl_role_master on tram.FlowId equals turs.role_id
                       join tsm in _db.tbl_status_master on tram.Status equals tsm.StatusId
                       where tram.AllocationId == SeatAllocationId
                       orderby tram.CreatedOn descending
                       select new SeatMatrixAllocationDetail
                       {
                           AllocationDetailId = tram.AllocationId,
                           userRole = tur.role_description,
                           ForwardedTo = turs.role_description,
                           StatusName = tsm.StatusName,
                           Remarks = tram.Remarks,
                           CommentsCreatedOn = tram.CreatedOn.ToString(),
                           StatusId = tsm.StatusId
                       }).ToList();
            if (res.Count > 0)
            {
                res = res.Take(res.Count - 1).ToList();
            }
            return res;
        }
        #endregion

        public string UpdtApplITIInstDetailsDLL(int ApplicantId, int LoginID, int roleID, int? allocId)
        {
            var InstituteDetails = new tbl_Applicant_ITI_Institute_Detail();
            InstituteDetails.ApplicationId = ApplicantId;
            InstituteDetails.AdmittedStatus = 1;    //Seat allotment done , pending at admission institute level
            InstituteDetails.IsActive = true;
            InstituteDetails.CreatedBy = LoginID;
            InstituteDetails.CreatedOn = DateTime.Now;
            InstituteDetails.FlowId = roleID;
            InstituteDetails.AllocationId = allocId;
            _db.tbl_Applicant_ITI_Institute_Detail.Add(InstituteDetails);
            _db.SaveChanges();

            int MaxInsertRecords = InstituteDetails.ApplicantITIInstituteId;
                //= _db.tbl_Applicant_ITI_Institute_Detail.Where(a => a.IsActive == true).OrderByDescending(x => x.CreatedOn).
                //Select(y => y.ApplicantITIInstituteId).FirstOrDefault();

            //Insert on tbl_Applicant_ITI_Institute_Detail_Trans
            var InstituteDetailsTrans = new tbl_Applicant_ITI_Institute_Detail_Trans();
            InstituteDetailsTrans.ApplicantITIInstituteId = MaxInsertRecords;
            InstituteDetailsTrans.ApplicationId = ApplicantId;
            InstituteDetailsTrans.AdmittedStatus = 1;    //Seat allotment done , pending at admission institute level
            InstituteDetailsTrans.IsActive = true;
            InstituteDetailsTrans.CreatedBy = LoginID;
            InstituteDetailsTrans.CreatedOn = DateTime.Now;
            InstituteDetailsTrans.FlowId = roleID;
            InstituteDetailsTrans.Remarks = _db.tbl_status_master.Where(a => a.StatusId == (int)CmnClass.Status.SeatAlloted).Select(x => x.StatusName).FirstOrDefault(); ;
            InstituteDetailsTrans.ApplInstiStatus = 1;
            InstituteDetailsTrans.AllocationId = allocId;
            _db.tbl_Applicant_ITI_Institute_Detail_Trans.Add(InstituteDetailsTrans);
            _db.SaveChanges();
            return null;
        }

        private void UpdtApplDetailsStatus(int ApplicantId, int status)
        {
                try
                {
                    int applDescStatId = 0;
                    tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();
                    var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == ApplicantId && s.IsActive == true).FirstOrDefault();

                    update_query.ApplStatus = status;

                    if (status == (int)CmnClass.Status.SeatAlloted)
                        applDescStatId = (int)CmnClass.applFormDescStatus.SeatAllocated;
                    else if (status == (int)CmnClass.Status.SeatNOTAlloted)
                        applDescStatId = (int)CmnClass.applFormDescStatus.SeatNOTAllocated;

                    update_query.ApplDescStatus = applDescStatId;
                    update_query.ApplRemarks = _db.tbl_ApplicationFormDescStatus.Where(a=> a.ApplicationFormDescStatus_id == applDescStatId).Select(a=> a.ApplDescription).FirstOrDefault();
                    //update_query.FlowId = objApplicationStatusUpdate.CredatedBy;

                    _db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
        }

    }
}
