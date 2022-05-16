using DLL.DBConnection;
using Models.Admission;
using Models.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Models;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Reflection;

namespace DLL.Admission
{
    public class AdmissionSeatMatrixDLL : IAdmissionSeatMatrixDLL
    {
        private readonly DbConnection _db = new DbConnection();

        string SQLConnection = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        //private readonly Random _random = new Random();
        private static Random random = new Random();

        #region ..Seat Matrix..
        //public List<seatmatrixmodel> GetInstituteTypeDll()
        //{
        //    var res = (from a in _db.tbl_Institute_type
        //               select new seatmatrixmodel
        //               {
        //                   Institute_typeId = a.Institute_type_id,
        //                   InstituteType = a.Institute_type
        //               }).ToList();
        //    return res;
        //}
        //public List<SelectListItem> GetTalukDLL(int DistId)
        //{
        //    var res = (from a in _db.tbl_taluk_master.Where(a => a.district_lgd_code == DistId && a.taluk_is_active == true)

        //               select new SelectListItem
        //               {
        //                   Text = a.taluk_ename,
        //                   Value = a.taluk_lgd_code.ToString()

        //               }).ToList();
        //    return res;
        //}

        //public List<seatmatrixmodel> GetviewSeatmatrixDLL(int ApplicantTypeId, int AcademicYear, int Institute, int id)
        //{
        //    try
        //    {
        //        List<seatmatrixmodel> res = null;
        //        res = (from t1 in _db.tbl_ITI_trade_seat_master
        //               join t2 in _db.tbl_ITI_Trade on t1.Trade_ITI_Id equals t2.Trade_ITI_id
        //               join t3 in _db.tbl_seat_type on t1.SeatsTypeId equals t3.Seat_type_id
        //               join t4 in _db.tbl_shifts on t1.ShiftId equals t4.s_id
        //               join t5 in _db.tbl_units on t1.UnitId equals t5.u_id
        //               join t6 in _db.tbl_iti_college_details on t2.ITICode equals t6.iti_college_id
        //               join t7 in _db.tbl_district_mast on t6.district_id equals t7.dist_id
        //               join t9 in _db.tbl_trade_mast on t2.Trade_ITI_id equals t9.trade_id
        //               join t8 in _db.tbl_division_mast on t7.division_id equals t8.division_id
        //               //join App in _db.tbl_ApplicantType on
        //               select new seatmatrixmodel
        //               {
        //                   MISCode = t6.MISCode,
        //                   district_id = t7.dist_id,
        //                   iti_college_id = t6.iti_college_id,
        //                   iti_college_name = t6.iti_college_name,

        //               }).Distinct().ToList();
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<seatmatrixmodel> GetSummarySeatMatrixDLL(int id)
        //{
        //    try
        //    {
        //        List<seatmatrixmodel> res = null;
        //        res = (from sa in _db.Tbl_rules_allocation_master
        //               join sa1 in _db.tbl_Vertical_rule_value on sa.Rules_allocation_master_id equals sa1.Rules_allocation_master_id
        //               join sa2 in _db.tbl_Vertical_rules on sa1.Vertical_rules_id equals sa2.Vertical_rules_id
        //               join hori1 in _db.tbl_horizontal_rules_values on sa.Rules_allocation_master_id equals hori1.Rules_allocation_master_id
        //               join hori2 in _db.Tbl_horizontal_rules on hori1.Horizontal_rules_id equals hori2.Horizontal_rules_id

        //               select new seatmatrixmodel
        //               {
        //                   Vertical_rulesid = sa1.Vertical_rules_id,
        //                   //RuleValue = sa1.RuleValue,
        //                   verticalRule = sa1.RuleValue,
        //                   HorizonRule = hori1.RuleValue,
        //                   Vertical_Rules = sa2.Vertical_Rules,
        //                   Rules_allocation_masterid = sa.Rules_allocation_master_id
        //               }).ToList();


        //        foreach (var p in res)
        //        {
        //            p.RuleValue = p.verticalRule + p.HorizonRule;
        //        }
        //        return res.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<seatmatrixmodel> GetGenerateSeatMatrixDLL(int ApplicantTypeGen, int AcademicYearGen, int RoundGen, int id)
        {
            try
            {

                List<seatmatrixmodel> res = null;
                res = (from div in _db.tbl_division_master
                       join dis in _db.tbl_district_master on div.division_id equals dis.division_id
                       join tal in _db.tbl_taluk_master on dis.district_lgd_code equals tal.district_lgd_code
                       //join pa in _db.tbl_Panchayat on tal.taluk_id equals pa.TalukCode
                       join icd in _db.tbl_iti_college_details on tal.taluk_id equals icd.taluk_id

                       //where (icd.iti_college_name =="Shanti ITI College")
                       //    _db.tbl_ITI_trade_seat_master
                       //join b in _db.tbl_ITI_Trade on a.Trade_ITI_Id equals b.Trade_ITI_id
                       //join c in _db.tbl_trade_mast on a.Trade_ITI_Id equals c.trade_id
                       //join d in _db.tbl_iti_college_details on b.Trade_ITI_id equals d.iti_college_id
                       //join e in _db.tbl_district_master on d.district_id equals e.district_lgd_code
                       //join f in _db.tbl_division_master on d.division_id equals f.division_id
                       //join g in _db.tbl_Institute_type on d.Insitute_TypeId equals g.Institute_type_id
                       //join h in _db.tbl_seat_type on a.SeatsTypeId equals h.Seat_type_id
                       //join i in _db.tbl_shifts on a.ShiftId equals i.s_id
                       //join j in _db.tbl_units on a.UnitId equals j.u_id

                       //join sa in _db.Tbl_rules_allocation_master on d.CourseCode equals sa.CourseId
                       //join sa1 in _db.tbl_Vertical_rule_value on sa.Rules_allocation_master_id equals sa1.Rules_allocation_master_id
                       //join sa2 in _db.tbl_Vertical_rules on sa1.Vertical_rules_id equals sa2.Vertical_rules_id
                       //join hori1 in _db.tbl_horizontal_rules_values on sa.Rules_allocation_master_id equals hori1.Rules_allocation_master_id
                       //join hori2 in _db.Tbl_horizontal_rules on hori1.Horizontal_rules_id equals hori2.Horizontal_rules_id

                       //join thrv in _db.tbl_HYD_kar_rules_value on sa.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
                       //join thr in _db.Tbl_Hyd_kar_rules on thrv.Hyd_NonHyd_rules_id equals thr.Hyd_NonHyd_rules_id
                       //join thnc in _db.tbl_HYD_NonHYD_candidates on thr.Hyd_NonHyd_candidates_id equals thnc.Hyd_NonHyd_candidates_id
                       //join thnr in _db.tbl_HYD_NonHYD_regions on thr.Hyd_NonHyd_region_id equals thnr.Hyd_NonHyd_region_id

                       //join App in _db.tbl_ApplicantType on
                       select new seatmatrixmodel
                       {
                           division_name = div.division_name,
                           district_ename = dis.district_ename,
                           taluk_ename = tal.taluk_ename,
                           iti_college_name = icd.iti_college_name,
                           MISCode = icd.MISCode,
                           iti_college_id = icd.iti_college_id,
                           ////Trade_ITIId = a.Trade_ITI_Id,
                           ////division_id = f.division_id,

                           //district_id = e.district_lgd_code,

                           //MISCode = d.MISCode,
                           //InstituteType = g.Institute_type,
                           //InstituteName = d.iti_college_name,
                           ////TradeCode = c.trade_id,
                           //TradeName = c.trade_name,
                           //Govtseat = a.Govt_Gia_seats,
                           //GovtPPP = a.PPP_seats,
                           //Private = a.Management_seats,
                           //CategoryId = sa2.Vertical_rules_id,
                           //CategoryName = sa2.Vertical_Rules,
                           //ResGroupId = hori2.Horizontal_rules_id,
                           //GroupName = hori2.Horizontal_rules,
                           //verticalRule = sa1.RuleValue,
                           //HorizonRule = hori1.RuleValue,
                           //TotalSeats = a.Govt_Gia_seats,
                           //GMWomenH = thnr.Hyd_NonHyd_region_id,
                           //GMWomenNH = thnr.Hyd_NonHyd_region_id,
                       }).Distinct().ToList();

                int actualCal = 0;
                foreach (var p in res)
                {
                    p.GovtAidedseats = p.verticalRule + p.HorizonRule;
                    actualCal = Convert.ToInt32(p.TotalSeats * p.GovtAidedseats);
                }
                return res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Generate seat matrix by dhanraj          
        int GetAvailableSeats(int instituteId, int tradeId, int seats, int year, int courseType, int round)
        {

            //int year1 = Convert.ToInt32(_db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault());
            var s = _db.tbl_SeatAllocation_SeatMatrix.Where(x => x.IsActive == true && x.AcademicYear == year && x.Round == (round - 1) && x.CourseTypeId == courseType).FirstOrDefault();
            int totalseats = seats;
            if (s != null)
            {
                int allotted_seats = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == s.AllocationId && x.TradeId == tradeId)
                    .Join(_db.tbl_Applicant_ITI_Institute_Detail, x=> x.ApplicantId, y=> y.ApplicationId, (x, y) => new { y.AdmittedStatus }).Where(y=> y.AdmittedStatus == 6).Count();
                if (allotted_seats != 0)
                    totalseats = seats - allotted_seats;
            }
            return totalseats;
        }
        GenerateTradewiseSeat GetAdmittedSeats(int instituteId, int tradeId, int year, int courseType, int round)
        {

            //int year1 = Convert.ToInt32(_db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault());
            //seat allocation

            //seat matrix 
            var seatmtrix = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == instituteId && x.IsActive == true && x.AcademicYear == year && x.Round == (round - 1) && x.CourseTypeId == courseType).FirstOrDefault();

            for (int i = 1; i <= round - 1; i++)
            {
                if (seatmtrix == null)
                    seatmtrix = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == instituteId && x.IsActive == true && x.AcademicYear == year && x.Round == (round - i) && x.CourseTypeId == courseType).FirstOrDefault();
                else
                    break;
            }

            var seats = new tbl_SeatMatrix_TradeWise();
            if (seatmtrix != null)
            {
                seats = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == seatmtrix.SeatMaxId && x.TradeId == tradeId).FirstOrDefault();
            }


            var s = _db.tbl_SeatAllocation_SeatMatrix.Where(x => x.IsActive == true && x.AcademicYear == year && x.Round == (round - 1) && x.CourseTypeId == courseType).FirstOrDefault();
            var applicantList = new List<tbl_SeatAllocationDetail_Seatmatrix>();
            if (s != null)
            {
                applicantList = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.IsActive == true && x.InstituteId == instituteId && x.AllocationId == s.AllocationId && x.TradeId == tradeId && x.Status == (int)CmnClass.Status.SeatAlloted).ToList();

                for (int i = applicantList.Count - 1; i >= 0; i--)
                {
                    int applId = applicantList[i].ApplicantId;
                    var applAdmStatus = _db.tbl_Applicant_ITI_Institute_Detail.Where(a => a.ApplicationId == applId).FirstOrDefault();
                    bool ParticipateNextRnd = _db.tbl_Applicant_Detail.Where(a => a.ApplicationId == applId).Select(a => a.ParticipateNextRound).FirstOrDefault();
                    if (applAdmStatus.AdmittedStatus != 6)
                    {
                        Type t = seats.GetType();
                        PropertyInfo[] pClass = t.GetProperties();

                        int value = 0; // or whatever value you want to set
                        foreach (var property in pClass)
                        {
                            string propName = property.Name;
                            if (propName == applicantList[i].AllocByCategory.Replace("-", ""))
                            {
                                value = Convert.ToInt32(t.GetProperty(propName).GetValue(seats));
                                value += 1;
                                property.SetValue(seats, value, null);
                                break;
                            }
                        }
                        applicantList.RemoveAt(i);
                    }
                }

            }

            GenerateTradewiseSeat aw = new GenerateTradewiseSeat();
            if (round == 1)
            {
                foreach (var applicant in applicantList)
                {
                    //GM
                    if (applicant.VerticalId == 1 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 1)
                        aw.GMWH += 1;
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 2)
                        aw.GMWNH += 1;
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 1)
                        aw.GMPDH += 1;
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 2)
                        aw.GMPDNH += 1;
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 1)
                        aw.GMEXSH += 1;
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 2)
                        aw.GMEXSNH += 1;
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 1)
                        aw.GMKMH += 1;
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 2)
                        aw.GMKMNH += 1;
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 1)
                        aw.GMEWSH += 1;
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 2)
                        aw.GMEWSNH += 1;
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 1)
                        aw.GMGPH += 1;
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 2)
                        aw.GMGPNH += 1;
                    //SC
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 1)
                        aw.SCWH += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 2)
                        aw.SCWNH += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 1)
                        aw.SCPDH += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 2)
                        aw.SCPDNH += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 1)
                        aw.SCEXSH += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 2)
                        aw.SCEXSNH += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 1)
                        aw.SCKMH += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 2)
                        aw.SCKMNH += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 1)
                        aw.SCEWSH += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 2)
                        aw.SCEWSNH += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 1)
                        aw.SCGPH += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 2)
                        aw.SCGPNH += 1;
                    //ST
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 1)
                        aw.STWH += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 2)
                        aw.STWNH += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 1)
                        aw.STPDH += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 2)
                        aw.STPDNH += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 1)
                        aw.STEXSH += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 2)
                        aw.STEXSNH += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 1)
                        aw.STKMH += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 2)
                        aw.STKMNH += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 1)
                        aw.STEWSH += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 2)
                        aw.STEWSNH += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 1)
                        aw.STGPH += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 2)
                        aw.STGPNH += 1;
                    //C1
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 1)
                        aw.C1WH += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 2)
                        aw.C1WNH += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 1)
                        aw.C1PDH += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 2)
                        aw.C1PDNH += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 1)
                        aw.C1EXSH += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 2)
                        aw.C1EXSNH += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 1)
                        aw.C1KMH += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 2)
                        aw.C1KMNH += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 1)
                        aw.C1EWSH += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 2)
                        aw.C1EWSNH += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 1)
                        aw.C1GPH += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 2)
                        aw.C1GPNH += 1;
                    //IIA
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 1)
                        aw.TWOAWH += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 2)
                        aw.TWOAWNH += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 1)
                        aw.TWOAPDH += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 2)
                        aw.TWOAPDNH += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 1)
                        aw.TWOAEXSH += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 2)
                        aw.TWOAEXSNH += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 1)
                        aw.TWOAKMH += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 2)
                        aw.TWOAKMNH += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 1)
                        aw.TWOAEWSH += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 2)
                        aw.TWOAEWSNH += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 1)
                        aw.TWOAGPH += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 2)
                        aw.TWOAGPNH += 1;
                    //IIB
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 1)
                        aw.TWOBWH += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 2)
                        aw.TWOBWNH += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 1)
                        aw.TWOBPDH += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 2)
                        aw.TWOBPDNH += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 1)
                        aw.TWOBEXSH += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 2)
                        aw.TWOBEXSNH += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 1)
                        aw.TWOBKMH += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 2)
                        aw.TWOBKMNH += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 1)
                        aw.TWOBEWSH += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 2)
                        aw.TWOBEWSNH += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 1)
                        aw.TWOBGPH += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 2)
                        aw.TWOBGPNH += 1;
                    //IIIA
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 1)
                        aw.THREEAWH += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 2)
                        aw.THREEAWNH += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 1)
                        aw.THREEAPDH += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 2)
                        aw.THREEAPDNH += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 1)
                        aw.THREEAEXSH += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 2)
                        aw.THREEAEXSNH += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 1)
                        aw.THREEAKMH += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 2)
                        aw.THREEAKMNH += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 1)
                        aw.THREEAEWSH += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 2)
                        aw.THREEAEWSNH += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 1)
                        aw.THREEAGPH += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 2)
                        aw.THREEAGPNH += 1;
                    //IIIB
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 1)
                        aw.THREEBWH += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 1 && applicant.HyrNonHydrId == 2)
                        aw.THREEBWNH += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 1)
                        aw.THREEBPDH += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 2 && applicant.HyrNonHydrId == 2)
                        aw.THREEBPDNH += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 1)
                        aw.THREEBEXSH += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 3 && applicant.HyrNonHydrId == 2)
                        aw.THREEBEXSNH += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 1)
                        aw.THREEBKMH += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 4 && applicant.HyrNonHydrId == 2)
                        aw.THREEBKMNH += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 1)
                        aw.THREEBEWSH += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 7 && applicant.HyrNonHydrId == 2)
                        aw.THREEBEWSNH += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 1)
                        aw.THREEBGPH += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 8 && applicant.HyrNonHydrId == 2)
                        aw.THREEBGPNH += 1;
                    //GP
                    else if (applicant.VerticalId == 1 && applicant.HorizontalId == 8)
                        aw.GMGP += 1;
                    else if (applicant.VerticalId == 2 && applicant.HorizontalId == 8)
                        aw.SCGP += 1;
                    else if (applicant.VerticalId == 3 && applicant.HorizontalId == 8)
                        aw.STGP += 1;
                    else if (applicant.VerticalId == 4 && applicant.HorizontalId == 8)
                        aw.C1GP += 1;
                    else if (applicant.VerticalId == 5 && applicant.HorizontalId == 8)
                        aw.TWOAGP += 1;
                    else if (applicant.VerticalId == 6 && applicant.HorizontalId == 8)
                        aw.TWOBGP += 1;
                    else if (applicant.VerticalId == 7 && applicant.HorizontalId == 8)
                        aw.THREEAGP += 1;
                    else if (applicant.VerticalId == 8 && applicant.HorizontalId == 8)
                        aw.THREEBGP += 1;
                }
            }
            else if (round == 2)
            {
                var aw1 = Round2SeatData(aw, seats);
                Round2SeatData(aw, aw1);
            }
            else if (round == 3)
            {
                seats.GMGP = seats.GMWH + seats.GMWNH + seats.GMPDH + seats.GMPDNH + seats.GMEXSH +
                    seats.GMEXSNH + seats.GMKMH + seats.GMKMNH + seats.GMEWSH + seats.GMEWSNH + seats.GMGPH + seats.GMGPNH;
                seats.SCGP = seats.SCWH + seats.SCWNH + seats.SCPDH + seats.SCPDNH + seats.SCEXSH +
                    seats.SCEXSNH + seats.SCKMH + seats.SCKMNH + seats.SCEWSH + seats.SCEWSNH + seats.SCGPH + seats.SCGPNH;
                seats.STGP = seats.STWH + seats.STWNH + seats.STPDH + seats.STPDNH + seats.STEXSH +
                    seats.STEXSNH + seats.STKMH + seats.STKMNH + seats.STEWSH + seats.STEWSNH + seats.STGPH + seats.STGPNH;
                seats.C1GP = seats.C1WH + seats.C1WNH + seats.C1PDH + seats.C1PDNH + seats.C1EXSH +
                    seats.C1EXSNH + seats.C1KMH + seats.C1KMNH + seats.C1EWSH + seats.C1EWSNH + seats.C1GPH + seats.C1GPNH;
                seats.TWOAGP = seats.TWOAWH + seats.TWOAWNH + seats.TWOAPDH + seats.TWOAPDNH + seats.TWOAEXSH +
                    seats.TWOAEXSNH + seats.TWOAKMH + seats.TWOAKMNH + seats.TWOAEWSH + seats.TWOAEWSNH + seats.TWOAGPH + seats.TWOAGPNH;
                seats.TWOBGP = seats.TWOBWH + seats.TWOBWNH + seats.TWOBPDH + seats.TWOBPDNH + seats.TWOBEXSH +
                    seats.TWOBEXSNH + seats.TWOBKMH + seats.TWOBKMNH + seats.TWOBEWSH + seats.TWOBEWSNH + seats.TWOBGPH + seats.TWOBGPNH;
                seats.THREEAGP = seats.THREEAWH + seats.THREEAWNH + seats.THREEAPDH + seats.THREEAPDNH + seats.THREEAEXSH +
                    seats.THREEAEXSNH + seats.THREEAKMH + seats.THREEAKMNH + seats.THREEAEWSH + seats.THREEAEWSNH + seats.THREEAGPH + seats.THREEAGPNH;
                seats.THREEBGP = seats.THREEBWH + seats.THREEBWNH + seats.THREEBPDH + seats.THREEBPDNH + seats.THREEBEXSH +
                    seats.THREEBEXSNH + seats.THREEBKMH + seats.THREEBKMNH + seats.THREEBEWSH + seats.THREEBEWSNH + seats.THREEBGPH + seats.THREEBGPNH;

                aw.GMGP = Convert.ToDecimal(seats.GMGP) - (aw.GMWH + aw.GMWNH + aw.GMPDH + aw.GMPDNH + aw.GMEXSH +
                    aw.GMEXSNH + aw.GMKMH + aw.GMKMNH + aw.GMEWSH + aw.GMEWSNH + aw.GMGPH + aw.GMGPNH);
                aw.SCGP = Convert.ToDecimal(seats.SCGP) - (aw.SCWH + aw.SCWNH + aw.SCPDH + aw.SCPDNH + aw.SCEXSH +
                    aw.SCEXSNH + aw.SCKMH + aw.SCKMNH + aw.SCEWSH + aw.SCEWSNH + aw.SCGPH + aw.SCGPNH);
                aw.STGP = Convert.ToDecimal(seats.STGP) - (aw.STWH + aw.STWNH + aw.STPDH + aw.STPDNH + aw.STEXSH +
                    aw.STEXSNH + aw.STKMH + aw.STKMNH + aw.STEWSH + aw.STEWSNH + aw.STGPH + aw.STGPNH);
                aw.C1GP = Convert.ToDecimal(seats.C1GP) - (aw.C1WH + aw.C1WNH + aw.C1PDH + aw.C1PDNH + aw.C1EXSH +
                    aw.C1EXSNH + aw.C1KMH + aw.C1KMNH + aw.C1EWSH + aw.C1EWSNH + aw.C1GPH + aw.C1GPNH);
                aw.TWOAGP = Convert.ToDecimal(seats.TWOAGP) - (aw.TWOAWH + aw.TWOAWNH + aw.TWOAPDH + aw.TWOAPDNH + aw.TWOAEXSH +
                    aw.TWOAEXSNH + aw.TWOAKMH + aw.TWOAKMNH + aw.TWOAEWSH + aw.TWOAEWSNH + aw.TWOAGPH + aw.TWOAGPNH);
                aw.TWOBGP = Convert.ToDecimal(seats.TWOBGP) - (aw.TWOBWH + aw.TWOBWNH + aw.TWOBPDH + aw.TWOBPDNH + aw.TWOBEXSH +
                    aw.TWOBEXSNH + aw.TWOBKMH + aw.TWOBKMNH + aw.TWOBEWSH + aw.TWOBEWSNH + aw.TWOBGPH + aw.TWOBGPNH);
                aw.THREEAGP = Convert.ToDecimal(seats.THREEAGP) - (aw.THREEAWH + aw.THREEAWNH + aw.THREEAPDH + aw.THREEAPDNH + aw.THREEAEXSH +
                    aw.THREEAEXSNH + aw.THREEAKMH + aw.THREEAKMNH + aw.THREEAEWSH + aw.THREEAEWSNH + aw.THREEAGPH + aw.THREEAGPNH);
                aw.THREEBGP = Convert.ToDecimal(seats.THREEBGP) - (aw.THREEBWH + aw.THREEBWNH + aw.THREEBPDH + aw.THREEBPDNH + aw.THREEBEXSH +
                    aw.THREEBEXSNH + aw.THREEBKMH + aw.THREEBKMNH + aw.THREEBEWSH + aw.THREEBEWSNH + aw.THREEBGPH + aw.THREEBGPNH);
            }
            else if (round == 4 || round == 5)
            {
                seats.GMGP = seats.GMWH + seats.GMWNH + seats.GMPDH + seats.GMPDNH + seats.GMEXSH +
                   seats.GMEXSNH + seats.GMKMH + seats.GMKMNH + seats.GMEWSH + seats.GMEWSNH + seats.GMGPH + seats.GMGPNH;
                seats.SCGP = seats.SCWH + seats.SCWNH + seats.SCPDH + seats.SCPDNH + seats.SCEXSH +
                    seats.SCEXSNH + seats.SCKMH + seats.SCKMNH + seats.SCEWSH + seats.SCEWSNH + seats.SCGPH + seats.SCGPNH;
                seats.STGP = seats.STWH + seats.STWNH + seats.STPDH + seats.STPDNH + seats.STEXSH +
                    seats.STEXSNH + seats.STKMH + seats.STKMNH + seats.STEWSH + seats.STEWSNH + seats.STGPH + seats.STGPNH;
                seats.C1GP = seats.C1WH + seats.C1WNH + seats.C1PDH + seats.C1PDNH + seats.C1EXSH +
                    seats.C1EXSNH + seats.C1KMH + seats.C1KMNH + seats.C1EWSH + seats.C1EWSNH + seats.C1GPH + seats.C1GPNH;
                seats.TWOAGP = seats.TWOAWH + seats.TWOAWNH + seats.TWOAPDH + seats.TWOAPDNH + seats.TWOAEXSH +
                    seats.TWOAEXSNH + seats.TWOAKMH + seats.TWOAKMNH + seats.TWOAEWSH + seats.TWOAEWSNH + seats.TWOAGPH + seats.TWOAGPNH;
                seats.TWOBGP = seats.TWOBWH + seats.TWOBWNH + seats.TWOBPDH + seats.TWOBPDNH + seats.TWOBEXSH +
                    seats.TWOBEXSNH + seats.TWOBKMH + seats.TWOBKMNH + seats.TWOBEWSH + seats.TWOBEWSNH + seats.TWOBGPH + seats.TWOBGPNH;
                seats.THREEAGP = seats.THREEAWH + seats.THREEAWNH + seats.THREEAPDH + seats.THREEAPDNH + seats.THREEAEXSH +
                    seats.THREEAEXSNH + seats.THREEAKMH + seats.THREEAKMNH + seats.THREEAEWSH + seats.THREEAEWSNH + seats.THREEAGPH + seats.THREEAGPNH;
                seats.THREEBGP = seats.THREEBWH + seats.THREEBWNH + seats.THREEBPDH + seats.THREEBPDNH + seats.THREEBEXSH +
                    seats.THREEBEXSNH + seats.THREEBKMH + seats.THREEBKMNH + seats.THREEBEWSH + seats.THREEBEWSNH + seats.THREEBGPH + seats.THREEBGPNH;
                aw.GMGP = Convert.ToDecimal(seats.GMGP) - aw.GMGP;
                aw.SCGP = Convert.ToDecimal(seats.SCGP) - aw.SCGP;
                aw.STGP = Convert.ToDecimal(seats.STGP) - aw.STGP;
                aw.C1GP = Convert.ToDecimal(seats.C1GP) - aw.C1GP;
                aw.TWOAGP = Convert.ToDecimal(seats.TWOAGP) - aw.TWOAGP;
                aw.TWOBGP = Convert.ToDecimal(seats.TWOBGP) - aw.TWOBGP;
                aw.THREEAGP = Convert.ToDecimal(seats.THREEAGP) - aw.THREEAGP;
                aw.THREEBGP = Convert.ToDecimal(seats.THREEBGP) - aw.THREEBGP;
            }
            else
            {
                seats.GMGP = seats.GMGP + seats.SCGP + seats.STGP + seats.C1GP + seats.TWOAGP + seats.TWOBGP + seats.THREEAGP + seats.THREEBGP;
                aw.GMGP = Convert.ToDecimal(seats.GMGP) - aw.GMGP;
            }

            return aw;
        }

        public bool SubmitCollegeWiseSeatData(GenerateTradewiseSeat aw, int instituteId, int tradeId, int year, int courseType, int round, int loginId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == instituteId && x.AcademicYear == year && x.Round == round && x.CourseTypeId == courseType).FirstOrDefault();
                    int appType = 0;
                    if (round > 1 && round < 5)
                        appType = 1;
                    else
                        appType = 2;

                    if (rs == null)
                    {
                        tbl_SeatMatrix_Main sm = new tbl_SeatMatrix_Main();
                        sm.AcademicYear = year;
                        sm.InstituteId = instituteId;
                        sm.ApplicantType = appType;
                        sm.Round = round;
                        sm.Status = 5;
                        sm.IsActive = true;
                        sm.CreatedOn = DateTime.Now;
                        sm.CourseTypeId = courseType;
                        sm.Remarks = "aw";
                        _db.tbl_SeatMatrix_Main.Add(sm);
                        _db.SaveChanges();

                        var setmxId = _db.tbl_SeatMatrix_Main.OrderByDescending(x => x.SeatMaxId).Select(y => y.SeatMaxId).FirstOrDefault();
                        tbl_SeatMatrix_Trans st = new tbl_SeatMatrix_Trans();
                        st.SeatMaxId = setmxId;
                        st.AcademicYear = year;
                        st.InstituteId = instituteId;
                        st.ApplicantType = appType;
                        st.Round = round;
                        st.Status = 5;
                        st.Remarks = "aw";
                        st.IsActive = true;
                        st.CreatedOn = DateTime.Now;
                        st.CourseTypeId = courseType;
                        _db.tbl_SeatMatrix_Trans.Add(st);

                        tbl_SeatMatrix_TradeWise tw = new tbl_SeatMatrix_TradeWise();
                        if (round == 2)
                        {
                            tw.SeatMaxId = setmxId; tw.IsActive = true; tw.CreatedBy = loginId; tw.CreatedOn = DateTime.Now;tw.TradeId = tradeId;
                            tw.GMW = Convert.ToInt32(aw.GMW); tw.GMWH = Convert.ToInt32(aw.GMWH); tw.GMWNH = Convert.ToInt32(aw.GMWNH); tw.GMPD = Convert.ToInt32(aw.GMPD); tw.GMPDH = Convert.ToInt32(aw.GMPDH); tw.GMPDNH = Convert.ToInt32(aw.GMPDNH); tw.GMEXS = Convert.ToInt32(aw.GMEXS); tw.GMEXSH = Convert.ToInt32(aw.GMEXSH); tw.GMEXSNH = Convert.ToInt32(aw.GMEXSNH); tw.GMKM = Convert.ToInt32(aw.GMKM); tw.GMKMH = Convert.ToInt32(aw.GMKMH); tw.GMKMNH = Convert.ToInt32(aw.GMKMNH); tw.GMEWS = Convert.ToInt32(aw.GMEWS); tw.GMEWSH = Convert.ToInt32(aw.GMEWSH); tw.GMEWSNH = Convert.ToInt32(aw.GMEWSNH); tw.GMGP = Convert.ToInt32(aw.GMGP); tw.GMGPH = Convert.ToInt32(aw.GMGPH); tw.GMGPNH = Convert.ToInt32(aw.GMGPNH);
                            tw.SCW = Convert.ToInt32(aw.SCW); tw.SCWH = Convert.ToInt32(aw.SCWH); tw.SCWNH = Convert.ToInt32(aw.SCWNH); tw.SCPD = Convert.ToInt32(aw.SCPD); tw.SCPDH = Convert.ToInt32(aw.SCPDH); tw.SCPDNH = Convert.ToInt32(aw.SCPDNH); tw.SCEXS = Convert.ToInt32(aw.SCEXS); tw.SCEXSH = Convert.ToInt32(aw.SCEXSH); tw.SCEXSNH = Convert.ToInt32(aw.SCEXSNH); tw.SCKM = Convert.ToInt32(aw.SCKM); tw.SCKMH = Convert.ToInt32(aw.SCKMH); tw.SCKMNH = Convert.ToInt32(aw.SCKMNH); tw.SCEWS = Convert.ToInt32(aw.SCEWS); tw.SCEWSH = Convert.ToInt32(aw.SCEWSH); tw.SCEWSNH = Convert.ToInt32(aw.SCEWSNH); tw.SCGP = Convert.ToInt32(aw.SCGP); tw.SCGPH = Convert.ToInt32(aw.SCGPH); tw.SCGPNH = Convert.ToInt32(aw.SCGPNH);
                            tw.STW = Convert.ToInt32(aw.STW); tw.STWH = Convert.ToInt32(aw.STWH); tw.STWNH = Convert.ToInt32(aw.STWNH); tw.STPD = Convert.ToInt32(aw.STPD); tw.STPDH = Convert.ToInt32(aw.STPDH); tw.STPDNH = Convert.ToInt32(aw.STPDNH); tw.STEXS = Convert.ToInt32(aw.STEXS); tw.STEXSH = Convert.ToInt32(aw.STEXSH); tw.STEXSNH = Convert.ToInt32(aw.STEXSNH); tw.STKM = Convert.ToInt32(aw.STKM); tw.STKMH = Convert.ToInt32(aw.STKMH); tw.STKMNH = Convert.ToInt32(aw.STKMNH); tw.STEWS = Convert.ToInt32(aw.STEWS); tw.STEWSH = Convert.ToInt32(aw.STEWSH); tw.STEWSNH = Convert.ToInt32(aw.STEWSNH); tw.STGP = Convert.ToInt32(aw.STGP); tw.STGPH = Convert.ToInt32(aw.STGPH); tw.STGPNH = Convert.ToInt32(aw.STGPNH);
                            tw.C1W = Convert.ToInt32(aw.C1W); tw.C1WH = Convert.ToInt32(aw.C1WH); tw.C1WNH = Convert.ToInt32(aw.C1WNH); tw.C1PD = Convert.ToInt32(aw.C1PD); tw.C1PDH = Convert.ToInt32(aw.C1PDH); tw.C1PDNH = Convert.ToInt32(aw.C1PDNH); tw.C1EXS = Convert.ToInt32(aw.C1EXS); tw.C1EXSH = Convert.ToInt32(aw.C1EXSH); tw.C1EXSNH = Convert.ToInt32(aw.C1EXSNH); tw.C1KM = Convert.ToInt32(aw.C1KM); tw.C1KMH = Convert.ToInt32(aw.C1KMH); tw.C1KMNH = Convert.ToInt32(aw.C1KMNH); tw.C1EWS = Convert.ToInt32(aw.C1EWS); tw.C1EWSH = Convert.ToInt32(aw.C1EWSH); tw.C1EWSNH = Convert.ToInt32(aw.C1EWSNH); tw.C1GP = Convert.ToInt32(aw.C1GP); tw.C1GPH = Convert.ToInt32(aw.C1GPH); tw.C1GPNH = Convert.ToInt32(aw.C1GPNH);
                            tw.TWOAW = Convert.ToInt32(aw.TWOAW); tw.TWOAWH = Convert.ToInt32(aw.TWOAWH); tw.TWOAWNH = Convert.ToInt32(aw.TWOAWNH); tw.TWOAPD = Convert.ToInt32(aw.TWOAPD); tw.TWOAPDH = Convert.ToInt32(aw.TWOAPDH); tw.TWOAPDNH = Convert.ToInt32(aw.TWOAPDNH); tw.TWOAEXS = Convert.ToInt32(aw.TWOAEXS); tw.TWOAEXSH = Convert.ToInt32(aw.TWOAEXSH); tw.TWOAEXSNH = Convert.ToInt32(aw.TWOAEXSNH); tw.TWOAKM = Convert.ToInt32(aw.TWOAKM); tw.TWOAKMH = Convert.ToInt32(aw.TWOAKMH); tw.TWOAKMNH = Convert.ToInt32(aw.TWOAKMNH); tw.TWOAEWS = Convert.ToInt32(aw.TWOAEWS); tw.TWOAEWSH = Convert.ToInt32(aw.TWOAEWSH); tw.TWOAEWSNH = Convert.ToInt32(aw.TWOAEWSNH); tw.TWOAGP = Convert.ToInt32(aw.TWOAGP); tw.TWOAGPH = Convert.ToInt32(aw.TWOAGPH); tw.TWOAGPNH = Convert.ToInt32(aw.TWOAGPNH);
                            tw.TWOBW = Convert.ToInt32(aw.TWOBW); tw.TWOBWH = Convert.ToInt32(aw.TWOBWH); tw.TWOBWNH = Convert.ToInt32(aw.TWOBWNH); tw.TWOBPD = Convert.ToInt32(aw.TWOBPD); tw.TWOBPDH = Convert.ToInt32(aw.TWOBPDH); tw.TWOBPDNH = Convert.ToInt32(aw.TWOBPDNH); tw.TWOBEXS = Convert.ToInt32(aw.TWOBEXS); tw.TWOBEXSH = Convert.ToInt32(aw.TWOBEXSH); tw.TWOBEXSNH = Convert.ToInt32(aw.TWOBEXSNH); tw.TWOBKM = Convert.ToInt32(aw.TWOBKM); tw.TWOBKMH = Convert.ToInt32(aw.TWOBKMH); tw.TWOBKMNH = Convert.ToInt32(aw.TWOBKMNH); tw.TWOBEWS = Convert.ToInt32(aw.TWOBEWS); tw.TWOBEWSH = Convert.ToInt32(aw.TWOBEWSH); tw.TWOBEWSNH = Convert.ToInt32(aw.TWOBEWSNH); tw.TWOBGP = Convert.ToInt32(aw.TWOBGP); tw.TWOBGPH = Convert.ToInt32(aw.TWOBGPH); tw.TWOBGPNH = Convert.ToInt32(aw.TWOBGPNH);
                            tw.THREEAW = Convert.ToInt32(aw.THREEAW); tw.THREEAWH = Convert.ToInt32(aw.THREEAWH); tw.THREEAWNH = Convert.ToInt32(aw.THREEAWNH); tw.THREEAPD = Convert.ToInt32(aw.THREEAPD); tw.THREEAPDH = Convert.ToInt32(aw.THREEAPDH); tw.THREEAPDNH = Convert.ToInt32(aw.THREEAPDNH); tw.THREEAEXS = Convert.ToInt32(aw.THREEAEXS); tw.THREEAEXSH = Convert.ToInt32(aw.THREEAEXSH); tw.THREEAEXSNH = Convert.ToInt32(aw.THREEAEXSNH); tw.THREEAKM = Convert.ToInt32(aw.THREEAKM); tw.THREEAKMH = Convert.ToInt32(aw.THREEAKMH); tw.THREEAKMNH = Convert.ToInt32(aw.THREEAKMNH); tw.THREEAEWS = Convert.ToInt32(aw.THREEAEWS); tw.THREEAEWSH = Convert.ToInt32(aw.THREEAEWSH); tw.THREEAEWSNH = Convert.ToInt32(aw.THREEAEWSNH); tw.THREEAGP = Convert.ToInt32(aw.THREEAGP); tw.THREEAGPH = Convert.ToInt32(aw.THREEAGPH); tw.THREEAGPNH = Convert.ToInt32(aw.THREEAGPNH);
                            tw.THREEBW = Convert.ToInt32(aw.THREEBW); tw.THREEBWH = Convert.ToInt32(aw.THREEBWH); tw.THREEBWNH = Convert.ToInt32(aw.THREEBWNH); tw.THREEBPD = Convert.ToInt32(aw.THREEBPD); tw.THREEBPDH = Convert.ToInt32(aw.THREEBPDH); tw.THREEBPDNH = Convert.ToInt32(aw.THREEBPDNH); tw.THREEBEXS = Convert.ToInt32(aw.THREEBEXS); tw.THREEBEXSH = Convert.ToInt32(aw.THREEBEXSH); tw.THREEBEXSNH = Convert.ToInt32(aw.THREEBEXSNH); tw.THREEBKM = Convert.ToInt32(aw.THREEBKM); tw.THREEBKMH = Convert.ToInt32(aw.THREEBKMH); tw.THREEBKMNH = Convert.ToInt32(aw.THREEBKMNH); tw.THREEBEWS = Convert.ToInt32(aw.THREEBEWS); tw.THREEBEWSH = Convert.ToInt32(aw.THREEBEWSH); tw.THREEBEWSNH = Convert.ToInt32(aw.THREEBEWSNH); tw.THREEBGP = Convert.ToInt32(aw.THREEBGP); tw.THREEBGPH = Convert.ToInt32(aw.THREEBGPH); tw.THREEBGPNH = Convert.ToInt32(aw.THREEBGPNH);
                            _db.tbl_SeatMatrix_TradeWise.Add(tw);
                        }
                        else if (round == 6)
                        {
                            tw.SeatMaxId = setmxId;
                            tw.IsActive = true;
                            tw.CreatedBy = loginId;
                            tw.CreatedOn = DateTime.Now;
                            tw.GMGP = Convert.ToInt32(aw.GMGP);
                            _db.tbl_SeatMatrix_TradeWise.Add(tw);
                        }
                        else
                        {
                            tw.SeatMaxId = setmxId;
                            tw.IsActive = true;
                            tw.CreatedBy = loginId;
                            tw.CreatedOn = DateTime.Now;
                            tw.GMGP = Convert.ToInt32(aw.GMGP);
                            tw.SCGP = Convert.ToInt32(aw.SCGP);
                            tw.STGP = Convert.ToInt32(aw.STGP);
                            tw.C1GP = Convert.ToInt32(aw.C1GP);
                            tw.TWOAGP = Convert.ToInt32(aw.TWOAGP);
                            tw.TWOBGP = Convert.ToInt32(aw.TWOBGP);
                            tw.THREEAGP = Convert.ToInt32(aw.THREEAGP);
                            tw.THREEBGP = Convert.ToInt32(aw.THREEBGP);
                            _db.tbl_SeatMatrix_TradeWise.Add(tw);
                        }

                        _db.SaveChanges();
                        transaction.Commit();
                        return true;

                    }
                    else
                    {
                        var wert = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == rs.SeatMaxId && x.TradeId == tradeId).FirstOrDefault();
                        if (wert == null)
                        {
                            tbl_SeatMatrix_TradeWise tw = new tbl_SeatMatrix_TradeWise();
                            if (round == 2)
                            {
                                tw.SeatMaxId = rs.SeatMaxId; tw.IsActive = true; tw.CreatedBy = loginId; tw.CreatedOn = DateTime.Now;  tw.TradeId = tradeId;
                                tw.GMW = Convert.ToInt32(aw.GMW); tw.GMWH = Convert.ToInt32(aw.GMWH); tw.GMWNH = Convert.ToInt32(aw.GMWNH); tw.GMPD = Convert.ToInt32(aw.GMPD); tw.GMPDH = Convert.ToInt32(aw.GMPDH); tw.GMPDNH = Convert.ToInt32(aw.GMPDNH); tw.GMEXS = Convert.ToInt32(aw.GMEXS); tw.GMEXSH = Convert.ToInt32(aw.GMEXSH); tw.GMEXSNH = Convert.ToInt32(aw.GMEXSNH); tw.GMKM = Convert.ToInt32(aw.GMKM); tw.GMKMH = Convert.ToInt32(aw.GMKMH); tw.GMKMNH = Convert.ToInt32(aw.GMKMNH); tw.GMEWS = Convert.ToInt32(aw.GMEWS); tw.GMEWSH = Convert.ToInt32(aw.GMEWSH); tw.GMEWSNH = Convert.ToInt32(aw.GMEWSNH); tw.GMGP = Convert.ToInt32(aw.GMGP); tw.GMGPH = Convert.ToInt32(aw.GMGPH); tw.GMGPNH = Convert.ToInt32(aw.GMGPNH);
                                tw.SCW = Convert.ToInt32(aw.SCW); tw.SCWH = Convert.ToInt32(aw.SCWH); tw.SCWNH = Convert.ToInt32(aw.SCWNH); tw.SCPD = Convert.ToInt32(aw.SCPD); tw.SCPDH = Convert.ToInt32(aw.SCPDH); tw.SCPDNH = Convert.ToInt32(aw.SCPDNH); tw.SCEXS = Convert.ToInt32(aw.SCEXS); tw.SCEXSH = Convert.ToInt32(aw.SCEXSH); tw.SCEXSNH = Convert.ToInt32(aw.SCEXSNH); tw.SCKM = Convert.ToInt32(aw.SCKM); tw.SCKMH = Convert.ToInt32(aw.SCKMH); tw.SCKMNH = Convert.ToInt32(aw.SCKMNH); tw.SCEWS = Convert.ToInt32(aw.SCEWS); tw.SCEWSH = Convert.ToInt32(aw.SCEWSH); tw.SCEWSNH = Convert.ToInt32(aw.SCEWSNH); tw.SCGP = Convert.ToInt32(aw.SCGP); tw.SCGPH = Convert.ToInt32(aw.SCGPH); tw.SCGPNH = Convert.ToInt32(aw.SCGPNH);
                                tw.STW = Convert.ToInt32(aw.STW); tw.STWH = Convert.ToInt32(aw.STWH); tw.STWNH = Convert.ToInt32(aw.STWNH); tw.STPD = Convert.ToInt32(aw.STPD); tw.STPDH = Convert.ToInt32(aw.STPDH); tw.STPDNH = Convert.ToInt32(aw.STPDNH); tw.STEXS = Convert.ToInt32(aw.STEXS); tw.STEXSH = Convert.ToInt32(aw.STEXSH); tw.STEXSNH = Convert.ToInt32(aw.STEXSNH); tw.STKM = Convert.ToInt32(aw.STKM); tw.STKMH = Convert.ToInt32(aw.STKMH); tw.STKMNH = Convert.ToInt32(aw.STKMNH); tw.STEWS = Convert.ToInt32(aw.STEWS); tw.STEWSH = Convert.ToInt32(aw.STEWSH); tw.STEWSNH = Convert.ToInt32(aw.STEWSNH); tw.STGP = Convert.ToInt32(aw.STGP); tw.STGPH = Convert.ToInt32(aw.STGPH); tw.STGPNH = Convert.ToInt32(aw.STGPNH);
                                tw.C1W = Convert.ToInt32(aw.C1W); tw.C1WH = Convert.ToInt32(aw.C1WH); tw.C1WNH = Convert.ToInt32(aw.C1WNH); tw.C1PD = Convert.ToInt32(aw.C1PD); tw.C1PDH = Convert.ToInt32(aw.C1PDH); tw.C1PDNH = Convert.ToInt32(aw.C1PDNH); tw.C1EXS = Convert.ToInt32(aw.C1EXS); tw.C1EXSH = Convert.ToInt32(aw.C1EXSH); tw.C1EXSNH = Convert.ToInt32(aw.C1EXSNH); tw.C1KM = Convert.ToInt32(aw.C1KM); tw.C1KMH = Convert.ToInt32(aw.C1KMH); tw.C1KMNH = Convert.ToInt32(aw.C1KMNH); tw.C1EWS = Convert.ToInt32(aw.C1EWS); tw.C1EWSH = Convert.ToInt32(aw.C1EWSH); tw.C1EWSNH = Convert.ToInt32(aw.C1EWSNH); tw.C1GP = Convert.ToInt32(aw.C1GP); tw.C1GPH = Convert.ToInt32(aw.C1GPH); tw.C1GPNH = Convert.ToInt32(aw.C1GPNH);
                                tw.TWOAW = Convert.ToInt32(aw.TWOAW); tw.TWOAWH = Convert.ToInt32(aw.TWOAWH); tw.TWOAWNH = Convert.ToInt32(aw.TWOAWNH); tw.TWOAPD = Convert.ToInt32(aw.TWOAPD); tw.TWOAPDH = Convert.ToInt32(aw.TWOAPDH); tw.TWOAPDNH = Convert.ToInt32(aw.TWOAPDNH); tw.TWOAEXS = Convert.ToInt32(aw.TWOAEXS); tw.TWOAEXSH = Convert.ToInt32(aw.TWOAEXSH); tw.TWOAEXSNH = Convert.ToInt32(aw.TWOAEXSNH); tw.TWOAKM = Convert.ToInt32(aw.TWOAKM); tw.TWOAKMH = Convert.ToInt32(aw.TWOAKMH); tw.TWOAKMNH = Convert.ToInt32(aw.TWOAKMNH); tw.TWOAEWS = Convert.ToInt32(aw.TWOAEWS); tw.TWOAEWSH = Convert.ToInt32(aw.TWOAEWSH); tw.TWOAEWSNH = Convert.ToInt32(aw.TWOAEWSNH); tw.TWOAGP = Convert.ToInt32(aw.TWOAGP); tw.TWOAGPH = Convert.ToInt32(aw.TWOAGPH); tw.TWOAGPNH = Convert.ToInt32(aw.TWOAGPNH);
                                tw.TWOBW = Convert.ToInt32(aw.TWOBW); tw.TWOBWH = Convert.ToInt32(aw.TWOBWH); tw.TWOBWNH = Convert.ToInt32(aw.TWOBWNH); tw.TWOBPD = Convert.ToInt32(aw.TWOBPD); tw.TWOBPDH = Convert.ToInt32(aw.TWOBPDH); tw.TWOBPDNH = Convert.ToInt32(aw.TWOBPDNH); tw.TWOBEXS = Convert.ToInt32(aw.TWOBEXS); tw.TWOBEXSH = Convert.ToInt32(aw.TWOBEXSH); tw.TWOBEXSNH = Convert.ToInt32(aw.TWOBEXSNH); tw.TWOBKM = Convert.ToInt32(aw.TWOBKM); tw.TWOBKMH = Convert.ToInt32(aw.TWOBKMH); tw.TWOBKMNH = Convert.ToInt32(aw.TWOBKMNH); tw.TWOBEWS = Convert.ToInt32(aw.TWOBEWS); tw.TWOBEWSH = Convert.ToInt32(aw.TWOBEWSH); tw.TWOBEWSNH = Convert.ToInt32(aw.TWOBEWSNH); tw.TWOBGP = Convert.ToInt32(aw.TWOBGP); tw.TWOBGPH = Convert.ToInt32(aw.TWOBGPH); tw.TWOBGPNH = Convert.ToInt32(aw.TWOBGPNH);
                                tw.THREEAW = Convert.ToInt32(aw.THREEAW); tw.THREEAWH = Convert.ToInt32(aw.THREEAWH); tw.THREEAWNH = Convert.ToInt32(aw.THREEAWNH); tw.THREEAPD = Convert.ToInt32(aw.THREEAPD); tw.THREEAPDH = Convert.ToInt32(aw.THREEAPDH); tw.THREEAPDNH = Convert.ToInt32(aw.THREEAPDNH); tw.THREEAEXS = Convert.ToInt32(aw.THREEAEXS); tw.THREEAEXSH = Convert.ToInt32(aw.THREEAEXSH); tw.THREEAEXSNH = Convert.ToInt32(aw.THREEAEXSNH); tw.THREEAKM = Convert.ToInt32(aw.THREEAKM); tw.THREEAKMH = Convert.ToInt32(aw.THREEAKMH); tw.THREEAKMNH = Convert.ToInt32(aw.THREEAKMNH); tw.THREEAEWS = Convert.ToInt32(aw.THREEAEWS); tw.THREEAEWSH = Convert.ToInt32(aw.THREEAEWSH); tw.THREEAEWSNH = Convert.ToInt32(aw.THREEAEWSNH); tw.THREEAGP = Convert.ToInt32(aw.THREEAGP); tw.THREEAGPH = Convert.ToInt32(aw.THREEAGPH); tw.THREEAGPNH = Convert.ToInt32(aw.THREEAGPNH);
                                tw.THREEBW = Convert.ToInt32(aw.THREEBW); tw.THREEBWH = Convert.ToInt32(aw.THREEBWH); tw.THREEBWNH = Convert.ToInt32(aw.THREEBWNH); tw.THREEBPD = Convert.ToInt32(aw.THREEBPD); tw.THREEBPDH = Convert.ToInt32(aw.THREEBPDH); tw.THREEBPDNH = Convert.ToInt32(aw.THREEBPDNH); tw.THREEBEXS = Convert.ToInt32(aw.THREEBEXS); tw.THREEBEXSH = Convert.ToInt32(aw.THREEBEXSH); tw.THREEBEXSNH = Convert.ToInt32(aw.THREEBEXSNH); tw.THREEBKM = Convert.ToInt32(aw.THREEBKM); tw.THREEBKMH = Convert.ToInt32(aw.THREEBKMH); tw.THREEBKMNH = Convert.ToInt32(aw.THREEBKMNH); tw.THREEBEWS = Convert.ToInt32(aw.THREEBEWS); tw.THREEBEWSH = Convert.ToInt32(aw.THREEBEWSH); tw.THREEBEWSNH = Convert.ToInt32(aw.THREEBEWSNH); tw.THREEBGP = Convert.ToInt32(aw.THREEBGP); tw.THREEBGPH = Convert.ToInt32(aw.THREEBGPH); tw.THREEBGPNH = Convert.ToInt32(aw.THREEBGPNH);
                                _db.tbl_SeatMatrix_TradeWise.Add(tw);
                            }
                            else if (round == 6)
                            {
                                tw.SeatMaxId = rs.SeatMaxId;
                                tw.IsActive = true;
                                tw.CreatedBy = loginId;
                                tw.CreatedOn = DateTime.Now;
                                tw.GMGP = Convert.ToInt32(aw.GMGP);
                                _db.tbl_SeatMatrix_TradeWise.Add(tw);
                            }
                            else
                            {
                                tw.SeatMaxId = rs.SeatMaxId;
                                tw.IsActive = true;
                                tw.CreatedBy = loginId;
                                tw.CreatedOn = DateTime.Now;
                                tw.GMGP = Convert.ToInt32(aw.GMGP);
                                tw.SCGP = Convert.ToInt32(aw.SCGP);
                                tw.STGP = Convert.ToInt32(aw.STGP);
                                tw.C1GP = Convert.ToInt32(aw.C1GP);
                                tw.TWOAGP = Convert.ToInt32(aw.TWOAGP);
                                tw.TWOBGP = Convert.ToInt32(aw.TWOBGP);
                                tw.THREEAGP = Convert.ToInt32(aw.THREEAGP);
                                tw.THREEBGP = Convert.ToInt32(aw.THREEBGP);
                                _db.tbl_SeatMatrix_TradeWise.Add(tw);
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
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public List<DivisionListModel> GenerateSeaMatrix(int year, int courseType, int round, int loginId)
        {
            try
            {
                //List<GenerateTradewiseSeat> lstTrade = new List<GenerateTradewiseSeat>
                //{
                //     new GenerateTradewiseSeat
                //     {
                //          TradeId = 0, TradeName = "zero", Seats = 0,
                //          GMW = 0, GMWH = 0, GMWNH = 0, GMPD = 0, GMPDH = 0, GMPDNH = 0, GMEXS = 0, GMEXSH = 0, GMEXSNH = 0, GMKM = 0, GMKMH = 0, GMKMNH = 0, GMEWS = 0, GMEWSH = 0, GMEWSNH = 0, GMGP = 0, GMGPH = 0, GMGPNH = 0,
                //          SCW = 0, SCWH = 0, SCWNH = 0, SCPD = 0, SCPDH = 0, SCPDNH = 0, SCEXS = 0, SCEXSH = 0, SCEXSNH = 0, SCKM = 0, SCKMH = 0, SCKMNH = 0, SCEWS = 0, SCEWSH = 0, SCEWSNH = 0, SCGP = 0, SCGPH = 0, SCGPNH = 0,
                //          STW = 0, STWH = 0, STWNH = 0, STPD = 0, STPDH = 0, STPDNH = 0, STEXS = 0, STEXSH = 0, STEXSNH = 0, STKM = 0, STKMH = 0, STKMNH = 0, STEWS = 0, STEWSH = 0, STEWSNH = 0, STGP = 0, STGPH = 0, STGPNH = 0,
                //          C1W = 0, C1WH = 0, C1WNH = 0, C1PD = 0, C1PDH = 0, C1PDNH = 0, C1EXS = 0, C1EXSH = 0, C1EXSNH = 0, C1KM = 0, C1KMH = 0, C1KMNH = 0, C1EWS = 0, C1EWSH = 0, C1EWSNH = 0, C1GP = 0, C1GPH = 0, C1GPNH = 0,
                //          TWOAW = 0, TWOAWH = 0, TWOAWNH = 0, TWOAPD = 0, TWOAPDH = 0, TWOAPDNH = 0, TWOAEXS = 0, TWOAEXSH = 0, TWOAEXSNH = 0, TWOAKM = 0, TWOAKMH = 0, TWOAKMNH = 0, TWOAEWS = 0, TWOAEWSH = 0, TWOAEWSNH = 0, TWOAGP = 0, TWOAGPH = 0, TWOAGPNH = 0,
                //          TWOBW = 0, TWOBWH = 0, TWOBWNH = 0, TWOBPD = 0, TWOBPDH = 0, TWOBPDNH = 0, TWOBEXS = 0, TWOBEXSH = 0, TWOBEXSNH = 0, TWOBKM = 0, TWOBKMH = 0, TWOBKMNH = 0, TWOBEWS = 0, TWOBEWSH = 0, TWOBEWSNH = 0, TWOBGP = 0, TWOBGPH = 0, TWOBGPNH = 0,
                //          THREEAW = 0, THREEAWH = 0, THREEAWNH = 0, THREEAPD = 0, THREEAPDH = 0, THREEAPDNH = 0, THREEAEXS = 0, THREEAEXSH = 0, THREEAEXSNH = 0, THREEAKM = 0, THREEAKMH = 0, THREEAKMNH = 0, THREEAEWS = 0, THREEAEWSH = 0, THREEAEWSNH = 0, THREEAGP = 0, THREEAGPH = 0, THREEAGPNH = 0,
                //          THREEBW = 0, THREEBWH = 0, THREEBWNH = 0, THREEBPD = 0, THREEBPDH = 0, THREEBPDNH = 0, THREEBEXS = 0, THREEBEXSH = 0, THREEBEXSNH = 0, THREEBKM = 0, THREEBKMH = 0, THREEBKMNH = 0, THREEBEWS = 0, THREEBEWSH = 0, THREEBEWSNH = 0, THREEBGP = 0, THREEBGPH = 0, THREEBGPNH = 0,
                //     }
                //};

                var div = (from aa in _db.tbl_division_master
                           select new DivisionListModel
                           {
                               DivisionId = aa.division_id,
                               DivisionName = aa.division_name,
                               Institutes = new List<InstituteListModel> {
                                   new InstituteListModel {
                                       InstituteId = 0, InstituteName = "zero", TotalSeats=0,Round=0,MisCode="",DistrictName="",TalukName="",
                                         Trades = new List<GenerateTradewiseSeat>
                {
                     new GenerateTradewiseSeat
                     {
                          TradeId = 0, TradeName = "zero", Seats = 0,
                          GMW = 0, GMWH = 0, GMWNH = 0, GMPD = 0, GMPDH = 0, GMPDNH = 0, GMEXS = 0, GMEXSH = 0, GMEXSNH = 0, GMKM = 0, GMKMH = 0, GMKMNH = 0, GMEWS = 0, GMEWSH = 0, GMEWSNH = 0, GMGP = 0, GMGPH = 0, GMGPNH = 0,
                          SCW = 0, SCWH = 0, SCWNH = 0, SCPD = 0, SCPDH = 0, SCPDNH = 0, SCEXS = 0, SCEXSH = 0, SCEXSNH = 0, SCKM = 0, SCKMH = 0, SCKMNH = 0, SCEWS = 0, SCEWSH = 0, SCEWSNH = 0, SCGP = 0, SCGPH = 0, SCGPNH = 0,
                          STW = 0, STWH = 0, STWNH = 0, STPD = 0, STPDH = 0, STPDNH = 0, STEXS = 0, STEXSH = 0, STEXSNH = 0, STKM = 0, STKMH = 0, STKMNH = 0, STEWS = 0, STEWSH = 0, STEWSNH = 0, STGP = 0, STGPH = 0, STGPNH = 0,
                          C1W = 0, C1WH = 0, C1WNH = 0, C1PD = 0, C1PDH = 0, C1PDNH = 0, C1EXS = 0, C1EXSH = 0, C1EXSNH = 0, C1KM = 0, C1KMH = 0, C1KMNH = 0, C1EWS = 0, C1EWSH = 0, C1EWSNH = 0, C1GP = 0, C1GPH = 0, C1GPNH = 0,
                          TWOAW = 0, TWOAWH = 0, TWOAWNH = 0, TWOAPD = 0, TWOAPDH = 0, TWOAPDNH = 0, TWOAEXS = 0, TWOAEXSH = 0, TWOAEXSNH = 0, TWOAKM = 0, TWOAKMH = 0, TWOAKMNH = 0, TWOAEWS = 0, TWOAEWSH = 0, TWOAEWSNH = 0, TWOAGP = 0, TWOAGPH = 0, TWOAGPNH = 0,
                          TWOBW = 0, TWOBWH = 0, TWOBWNH = 0, TWOBPD = 0, TWOBPDH = 0, TWOBPDNH = 0, TWOBEXS = 0, TWOBEXSH = 0, TWOBEXSNH = 0, TWOBKM = 0, TWOBKMH = 0, TWOBKMNH = 0, TWOBEWS = 0, TWOBEWSH = 0, TWOBEWSNH = 0, TWOBGP = 0, TWOBGPH = 0, TWOBGPNH = 0,
                          THREEAW = 0, THREEAWH = 0, THREEAWNH = 0, THREEAPD = 0, THREEAPDH = 0, THREEAPDNH = 0, THREEAEXS = 0, THREEAEXSH = 0, THREEAEXSNH = 0, THREEAKM = 0, THREEAKMH = 0, THREEAKMNH = 0, THREEAEWS = 0, THREEAEWSH = 0, THREEAEWSNH = 0, THREEAGP = 0, THREEAGPH = 0, THREEAGPNH = 0,
                          THREEBW = 0, THREEBWH = 0, THREEBWNH = 0, THREEBPD = 0, THREEBPDH = 0, THREEBPDNH = 0, THREEBEXS = 0, THREEBEXSH = 0, THREEBEXSNH = 0, THREEBKM = 0, THREEBKMH = 0, THREEBKMNH = 0, THREEBEWS = 0, THREEBEWSH = 0, THREEBEWSNH = 0, THREEBGP = 0, THREEBGPH = 0, THREEBGPNH = 0,
                     }
                }
                                   }
                               }
                           }).ToList();

                foreach (var s in div)
                {
                    //is_active to check college deaffiliate -- ravi sirigiri
                    var insti = (from aa in _db.tbl_iti_college_details
                                 join bb in _db.tbl_district_master on aa.district_id equals bb.district_lgd_code
                                 join cc in _db.tbl_taluk_master on aa.taluk_id equals cc.taluk_lgd_code
                                 where aa.division_id == s.DivisionId && aa.is_active == true
                                 select new InstituteListModel
                                 {
                                     MisCode = aa.MISCode,
                                     DistrictName = bb.district_ename,
                                     TalukName = cc.taluk_ename,
                                     InstituteId = aa.iti_college_id,
                                     InstituteName = aa.iti_college_name,
                                     Round = round,
                                     Trades = new List<GenerateTradewiseSeat>
                {
                     new GenerateTradewiseSeat
                     {
                          TradeId = 0, TradeName = "zero", Seats = 0,
                          GMW = 0, GMWH = 0, GMWNH = 0, GMPD = 0, GMPDH = 0, GMPDNH = 0, GMEXS = 0, GMEXSH = 0, GMEXSNH = 0, GMKM = 0, GMKMH = 0, GMKMNH = 0, GMEWS = 0, GMEWSH = 0, GMEWSNH = 0, GMGP = 0, GMGPH = 0, GMGPNH = 0,
                          SCW = 0, SCWH = 0, SCWNH = 0, SCPD = 0, SCPDH = 0, SCPDNH = 0, SCEXS = 0, SCEXSH = 0, SCEXSNH = 0, SCKM = 0, SCKMH = 0, SCKMNH = 0, SCEWS = 0, SCEWSH = 0, SCEWSNH = 0, SCGP = 0, SCGPH = 0, SCGPNH = 0,
                          STW = 0, STWH = 0, STWNH = 0, STPD = 0, STPDH = 0, STPDNH = 0, STEXS = 0, STEXSH = 0, STEXSNH = 0, STKM = 0, STKMH = 0, STKMNH = 0, STEWS = 0, STEWSH = 0, STEWSNH = 0, STGP = 0, STGPH = 0, STGPNH = 0,
                          C1W = 0, C1WH = 0, C1WNH = 0, C1PD = 0, C1PDH = 0, C1PDNH = 0, C1EXS = 0, C1EXSH = 0, C1EXSNH = 0, C1KM = 0, C1KMH = 0, C1KMNH = 0, C1EWS = 0, C1EWSH = 0, C1EWSNH = 0, C1GP = 0, C1GPH = 0, C1GPNH = 0,
                          TWOAW = 0, TWOAWH = 0, TWOAWNH = 0, TWOAPD = 0, TWOAPDH = 0, TWOAPDNH = 0, TWOAEXS = 0, TWOAEXSH = 0, TWOAEXSNH = 0, TWOAKM = 0, TWOAKMH = 0, TWOAKMNH = 0, TWOAEWS = 0, TWOAEWSH = 0, TWOAEWSNH = 0, TWOAGP = 0, TWOAGPH = 0, TWOAGPNH = 0,
                          TWOBW = 0, TWOBWH = 0, TWOBWNH = 0, TWOBPD = 0, TWOBPDH = 0, TWOBPDNH = 0, TWOBEXS = 0, TWOBEXSH = 0, TWOBEXSNH = 0, TWOBKM = 0, TWOBKMH = 0, TWOBKMNH = 0, TWOBEWS = 0, TWOBEWSH = 0, TWOBEWSNH = 0, TWOBGP = 0, TWOBGPH = 0, TWOBGPNH = 0,
                          THREEAW = 0, THREEAWH = 0, THREEAWNH = 0, THREEAPD = 0, THREEAPDH = 0, THREEAPDNH = 0, THREEAEXS = 0, THREEAEXSH = 0, THREEAEXSNH = 0, THREEAKM = 0, THREEAKMH = 0, THREEAKMNH = 0, THREEAEWS = 0, THREEAEWSH = 0, THREEAEWSNH = 0, THREEAGP = 0, THREEAGPH = 0, THREEAGPNH = 0,
                          THREEBW = 0, THREEBWH = 0, THREEBWNH = 0, THREEBPD = 0, THREEBPDH = 0, THREEBPDNH = 0, THREEBEXS = 0, THREEBEXSH = 0, THREEBEXSNH = 0, THREEBKM = 0, THREEBKMH = 0, THREEBKMNH = 0, THREEBEWS = 0, THREEBEWSH = 0, THREEBEWSNH = 0, THREEBGP = 0, THREEBGPH = 0, THREEBGPNH = 0,
                     }
                }
                                 }).ToList().OrderBy(x => x.MisCode);

                    foreach (var itm in insti)
                    {
                        s.Institutes.Add(itm);
                    }
                }
                int yer = Convert.ToInt32(_db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault().Split('-')[1]);

                #region commented code for optimization
                //var tradeIds = _db.tbl_ITI_trade_seat_master.Where(x => x.Status == 4
                //&& x.AcademicYear.Year == yer && x.CourseType == courseType).
                //Select(y => y.Trade_ITI_Id).Distinct().ToList();
                //List<CollegeTradeSeat> sit = new List<CollegeTradeSeat>();
                //foreach (var tradeId in tradeIds)
                //{
                //    CollegeTradeSeat nt = new CollegeTradeSeat();
                //    //get only governament seats only. dont consider management seats --ravi sirigiri
                //    nt.Seats = _db.tbl_ITI_trade_seat_master.Where(x => x.Status == 4 && x.AcademicYear.Year == yer 
                //    && x.Trade_ITI_Id == tradeId && x.CourseType == courseType).Sum(a => a.Govt_Gia_seats.Value);
                //    // && x.Trade_ITI_Id == tradeId && x.CourseType == courseType).Sum(a => a.SeatsPerUnit);
                //    nt.TradeId = (int)_db.tbl_ITI_Trade.Where(x => x.Trade_ITI_id == tradeId).
                //        Select(y => y.TradeCode).FirstOrDefault();
                //    nt.CollegeId = (int)_db.tbl_ITI_Trade.Where(x => x.Trade_ITI_id == tradeId).
                //        Select(y => y.ITICode).FirstOrDefault();
                //    nt.Units = _db.tbl_ITI_trade_seat_master.Where(x => x.Status == 4 && x.AcademicYear.Year == yer 
                //    && x.Trade_ITI_Id == tradeId && x.CourseType == courseType).Select(a => a.UnitId).Count();
                //    nt.TradeYear = _db.tbl_trade_mast.Where(x => x.trade_id == nt.TradeId).
                //        Select(y => y.trade_duration).FirstOrDefault();
                //    sit.Add(nt);
                //}
                #endregion
                //optimize code for above commented logic 
                //verified trade, unit and shift is active
                var seats = from a in _db.tbl_ITI_Trade
                          join b in _db.tbl_trade_mast on a.TradeCode equals b.trade_id
                          join c in _db.tbl_ITI_trade_seat_master on a.Trade_ITI_id equals c.Trade_ITI_Id
                          join d in _db.tbl_ITI_Trade_Shifts on new { x1 = c.UnitId, x2 = c.ShiftId, x3 = c.Trade_ITI_Id }
                          equals new { x1 = d.Units, x2 = d.Shift, x3 = d.ITI_Trade_Id }
                          where c.Status == 4 && c.CourseType == courseType && c.AcademicYear.Year == yer && a.IsActive == true
                          && d.IsActive == true && c.Govt_Gia_seats > 0
                          select new { c.Govt_Gia_seats, a.TradeCode, a.ITICode, c.UnitId, b.trade_name, b.trade_duration };
                List<CollegeTradeSeat> sit = seats?.GroupBy(x => new { x.TradeCode, x.ITICode, x.trade_name,
                    x.trade_duration }).Select(
                    s => new CollegeTradeSeat()
                    {
                        CollegeId = s.Key.ITICode.Value,
                        TradeId = s.Key.TradeCode.Value,
                        TradeYear = s.Key.trade_duration,
                        TradeName=s.Key.trade_name,
                        Seats = s.Sum(i => i.Govt_Gia_seats).Value,
                        Units = s.Select(u => u.UnitId).Count()
                    }).ToList();
                seats = null;
                if (sit != null && sit.Count > 0)
                {
                    foreach (var ad in div)
                    {
                        foreach (var clg in ad.Institutes.Where(x => x.InstituteId != 0))
                        {
                            foreach (var clgid in sit)
                            {
                                if (clg.InstituteId == clgid.CollegeId)
                                {
                                    //var tradName = _db.tbl_trade_mast.Where(x => x.trade_id == clgid.TradeId).Select(y => y.trade_name).FirstOrDefault();

                                    GenerateTradewiseSeat sn = new GenerateTradewiseSeat();
                                    sn.TradeId = clgid.TradeId;
                                    sn.TradeName = clgid.TradeName;
                                    sn.Units = clgid.Units;
                                    sn.TradeYear = clgid.TradeYear;
                                    if (round == 1)
                                        sn.Seats = clgid.Seats;
                                    else
                                    {
                                        //collect admitted details count for this college fr this trade and minus * seats =sn.seats -admitted count
                                        sn.Seats = GetAvailableSeats(clgid.CollegeId, clgid.TradeId, clgid.Seats, year, courseType, round);
                                    }
                                    clg.Trades.Add(sn);
                                }
                            }
                        }
                    }

                    //here statusid 2 is approved 
                    var rules = _db.Tbl_rules_allocation_master.Where(x => x.Status_Id == 2 && x.Exam_Year == year
                    && x.IsActive == true && x.CourseId == courseType).OrderByDescending(d => d.Trans_Date).
                    Select(y => y.Rules_allocation_master_id).FirstOrDefault();

                    var vrules = _db.tbl_Vertical_rules.Where(x => x.IsActive == true).ToList();

                    var hrules = _db.Tbl_horizontal_rules.Where(x => x.IsActive == true).ToList();

                    var vvalue = _db.tbl_Vertical_rule_value.Where(x => x.Rules_allocation_master_id == rules).ToList();
                    var hvalue = _db.tbl_horizontal_rules_values.Where(x => x.Rules_allocation_master_id == rules).ToList();

                    foreach (var ad in div)
                    {
                        foreach (var clg in ad.Institutes.Where(x => x.InstituteId != 0).ToList())
                        {
                            int ttl_seat = 0;
                            foreach (var trade in clg.Trades.Where(x => x.TradeId != 0).ToList())
                            {
                                List<VerticleCat> vcat = new List<VerticleCat>();
                                List<HorizontalCat> hcat = new List<HorizontalCat>();
                                //round is 6th no verticle calculation only GM

                                if (round == 6) // GM GP only
                                {

                                    VerticleCat vc = new VerticleCat();
                                    vc.SeatValue = trade.Seats; //all seats will assign to GM
                                    vc.Vcat = 1;//GM rule id
                                    vcat.Add(vc);

                                    HorizontalCat hc = new HorizontalCat();
                                    hc.SeatValue = vc.SeatValue;
                                    hc.Hcat = 8;//GP id
                                    hcat.Add(hc);
                                }

                                else
                                {
                                    foreach (var vv in vvalue)
                                    {
                                        VerticleCat vc = new VerticleCat();
                                        vc.SeatValue = (vv.RuleValue / 100) * trade.Seats;
                                        vc.Vcat = vv.Vertical_rules_id;//GM,SC,ST
                                        vcat.Add(vc);
                                    }

                                    foreach (var vcatVal in vcat)
                                    {
                                        if (round == 3 || round == 4 || round == 5) //veritcial rule with GP only
                                        {
                                            HorizontalCat hc = new HorizontalCat();
                                            hc.SeatValue = vcatVal.SeatValue;
                                            hc.Hcat = 8;//GP id
                                            hcat.Add(hc);
                                        }
                                        else
                                        {
                                            foreach (var hh in hvalue) // round 1,2
                                            {
                                                HorizontalCat hc = new HorizontalCat();
                                                hc.SeatValue = (hh.RuleValue / 100) * vcatVal.SeatValue;
                                                hc.Hcat = hh.Horizontal_rules_id;//Women,ex ,
                                                hcat.Add(hc);
                                            }
                                        }
                                    }
                                }

                                //hor foreach
                                //hor foreach take division id if 4 HYd else non HYD 
                                List<HyderbadKarnataka> vf = new List<HyderbadKarnataka>();
                                if (round == 1)
                                {
                                    foreach (var hcatVal in hcat)
                                    {
                                        HyderbadKarnataka bu = new HyderbadKarnataka();

                                        var hyd_kar_ruleValues = _db.tbl_HYD_kar_rules_value.Where(x => x.Rules_allocation_master_id == rules).ToList();
                                        if (ad.DivisionId == 4) //kalaburgi division
                                        {
                                            //hyd region
                                            //here 2, 3 values should come from enum
                                            //hyd canidate hyd_kar_rule is 2 as per current table structure
                                            //non hyd canidate hyd_kar_rule is 3 as per current table structure
                                            //default 70%
                                            bu.Hydrabad = kar_hyd_per(hyd_kar_ruleValues, 2, 70, hcatVal.SeatValue);
                                            //default 92%
                                            bu.NonHydrabad = kar_hyd_per(hyd_kar_ruleValues, 3, 30, hcatVal.SeatValue);

                                            //var hyd = _db.tbl_HYD_kar_rules_value.Where(x => x.Rules_allocation_master_id == rules)
                                            //.Take(2).ToList();
                                            //bu.Hydrabad = (hyd[0].RuleValue / 100) * hcatVal.SeatValue;
                                            //bu.NonHydrabad = (hyd[1].RuleValue / 100) * hcatVal.SeatValue;
                                        }
                                        else
                                        {
                                            //non hyd region
                                            //here 4, 5 values should come from enum
                                            //hyd canidate hyd_kar_rule is 4 as per current table structure
                                            //non hyd canidate hyd_kar_rule is 5 as per current table structure
                                            //default 8%
                                            bu.Hydrabad = kar_hyd_per(hyd_kar_ruleValues, 4, 8, hcatVal.SeatValue);
                                            //default 92%
                                            bu.NonHydrabad = kar_hyd_per(hyd_kar_ruleValues, 5, 92, hcatVal.SeatValue);

                                            //var nonHyd = _db.tbl_HYD_kar_rules_value.Where(x => x.Rules_allocation_master_id == rules).
                                            //    OrderByDescending(y => y.Hyd_NonHyd_rules_val_id).Take(2).ToList();
                                            //bu.Hydrabad = (nonHyd[0].RuleValue / 100) * hcatVal.SeatValue;
                                            //bu.NonHydrabad = (nonHyd[1].RuleValue / 100) * hcatVal.SeatValue;

                                        }
                                        vf.Add(bu);
                                    }
                                    var seatMxId = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == clg.InstituteId &&
                                    x.AcademicYear == year && x.CourseTypeId == courseType && x.Round == round).
                                    Select(y => y.SeatMaxId).FirstOrDefault();
                                    if (seatMxId != 0)
                                    {
                                        var aw = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == seatMxId &&
                                        x.TradeId == trade.TradeId).FirstOrDefault();
                                        if (aw != null)
                                        {
                                            trade.GMWH = Convert.ToDecimal(aw.GMWH); trade.GMWNH = Convert.ToDecimal(aw.GMWNH);
                                            trade.GMPDH = Convert.ToDecimal(aw.GMPDH); trade.GMPDNH = Convert.ToDecimal(aw.GMPDNH);
                                            trade.GMEXSH = Convert.ToDecimal(aw.GMEXSH); trade.GMEXSNH = Convert.ToDecimal(aw.GMEXSNH);
                                            trade.GMKMH = Convert.ToDecimal(aw.GMKMH); trade.GMKMNH = Convert.ToDecimal(aw.GMKMNH);
                                            trade.GMEWSH = Convert.ToDecimal(aw.GMEWSH); trade.GMEWSNH = Convert.ToDecimal(aw.GMEWSNH);
                                            trade.GMGPH = Convert.ToDecimal(aw.GMGPH); trade.GMGPNH = Convert.ToDecimal(aw.GMGPNH);
                                            trade.SCWH = Convert.ToDecimal(aw.SCWH); trade.SCWNH = Convert.ToDecimal(aw.SCWNH);
                                            trade.SCPDH = Convert.ToDecimal(aw.SCPDH); trade.SCPDNH = Convert.ToDecimal(aw.SCPDNH);
                                            trade.SCEXSH = Convert.ToDecimal(aw.SCEXSH); trade.SCEXSNH = Convert.ToDecimal(aw.SCEXSNH);
                                            trade.SCKMH = Convert.ToDecimal(aw.SCKMH); trade.SCKMNH = Convert.ToDecimal(aw.SCKMNH);
                                            //trade.SCEWSH = Convert.ToDecimal(aw.SCEWSH); trade.SCEWSNH = Convert.ToDecimal(aw.SCEWSNH);
                                            trade.SCGPH = Convert.ToDecimal(aw.SCGPH); trade.SCGPNH = Convert.ToDecimal(aw.SCGPNH);
                                            trade.STWH = Convert.ToDecimal(aw.STWH); trade.STWNH = Convert.ToDecimal(aw.STWNH);
                                            trade.STPDH = Convert.ToDecimal(aw.STPDH); trade.STPDNH = Convert.ToDecimal(aw.STPDNH);
                                            trade.STEXSH = Convert.ToDecimal(aw.STEXSH); trade.STEXSNH = Convert.ToDecimal(aw.STEXSNH);
                                            trade.STKMH = Convert.ToDecimal(aw.STKMH); trade.STKMNH = Convert.ToDecimal(aw.STKMNH);
                                            //trade.STEWSH = Convert.ToDecimal(aw.STEWSH); trade.STEWSNH = Convert.ToDecimal(aw.STEWSNH);
                                            trade.STGPH = Convert.ToDecimal(aw.STGPH); trade.STGPNH = Convert.ToDecimal(aw.STGPNH);
                                            trade.C1WH = Convert.ToDecimal(aw.C1WH); trade.C1WNH = Convert.ToDecimal(aw.C1WNH);
                                            trade.C1PDH = Convert.ToDecimal(aw.C1PDH); trade.C1PDNH = Convert.ToDecimal(aw.C1PDNH);
                                            trade.C1EXSH = Convert.ToDecimal(aw.C1EXSH); trade.C1EXSNH = Convert.ToDecimal(aw.C1EXSNH);
                                            trade.C1KMH = Convert.ToDecimal(aw.C1KMH); trade.C1KMNH = Convert.ToDecimal(aw.C1KMNH);
                                            //trade.C1EWSH = Convert.ToDecimal(aw.C1EWSH); trade.C1EWSNH = Convert.ToDecimal(aw.C1EWSNH);
                                            trade.C1GPH = Convert.ToDecimal(aw.C1GPH); trade.C1GPNH = Convert.ToDecimal(aw.C1GPNH);
                                            trade.TWOAWH = Convert.ToDecimal(aw.TWOAWH); trade.TWOAWNH = Convert.ToDecimal(aw.TWOAWNH);
                                            trade.TWOAPDH = Convert.ToDecimal(aw.TWOAPDH); trade.TWOAPDNH = Convert.ToDecimal(aw.TWOAPDNH);
                                            trade.TWOAEXSH = Convert.ToDecimal(aw.TWOAEXSH); trade.TWOAEXSNH = Convert.ToDecimal(aw.TWOAEXSNH);
                                            trade.TWOAKMH = Convert.ToDecimal(aw.TWOAKMH); trade.TWOAKMNH = Convert.ToDecimal(aw.TWOAKMNH);
                                            //trade.TWOAEWSH = Convert.ToDecimal(aw.TWOAEWSH); trade.TWOAEWSNH = Convert.ToDecimal(aw.TWOAEWSNH);
                                            trade.TWOAGPH = Convert.ToDecimal(aw.TWOAGPH); trade.TWOAGPNH = Convert.ToDecimal(aw.TWOAGPNH);
                                            trade.TWOBWH = Convert.ToDecimal(aw.TWOBWH); trade.TWOBWNH = Convert.ToDecimal(aw.TWOBWNH);
                                            trade.TWOBPDH = Convert.ToDecimal(aw.TWOBPDH); trade.TWOBPDNH = Convert.ToDecimal(aw.TWOBPDNH);
                                            trade.TWOBEXSH = Convert.ToDecimal(aw.TWOBEXSH); trade.TWOBEXSNH = Convert.ToDecimal(aw.TWOBEXSNH);
                                            trade.TWOBKMH = Convert.ToDecimal(aw.TWOBKMH); trade.TWOBKMNH = Convert.ToDecimal(aw.TWOBKMNH);
                                            //trade.TWOBEWSH = Convert.ToDecimal(aw.TWOBEWSH); trade.TWOBEWSNH = Convert.ToDecimal(aw.TWOBEWSNH);
                                            trade.TWOBGPH = Convert.ToDecimal(aw.TWOBGPH); trade.TWOBGPNH = Convert.ToDecimal(aw.TWOBGPNH);
                                            trade.THREEAWH = Convert.ToDecimal(aw.THREEAWH); trade.THREEAWNH = Convert.ToDecimal(aw.THREEAWNH);
                                            trade.THREEAPDH = Convert.ToDecimal(aw.THREEAPDH); trade.THREEAPDNH = Convert.ToDecimal(aw.THREEAPDNH);
                                            trade.THREEAEXSH = Convert.ToDecimal(aw.THREEAEXSH); trade.THREEAEXSNH = Convert.ToDecimal(aw.THREEAEXSNH);
                                            trade.THREEAKMH = Convert.ToDecimal(aw.THREEAKMH); trade.THREEAKMNH = Convert.ToDecimal(aw.THREEAKMNH);
                                            //trade.THREEAEWSH = Convert.ToDecimal(aw.THREEAEWSH); trade.THREEAEWSNH = Convert.ToDecimal(aw.THREEAEWSNH);
                                            trade.THREEAGPH = Convert.ToDecimal(aw.THREEAGPH); trade.THREEAGPNH = Convert.ToDecimal(aw.THREEAGPNH);
                                            trade.THREEBWH = Convert.ToDecimal(aw.THREEBWH); trade.THREEBWNH = Convert.ToDecimal(aw.THREEBWNH);
                                            trade.THREEBPDH = Convert.ToDecimal(aw.THREEBPDH); trade.THREEBPDNH = Convert.ToDecimal(aw.THREEBPDNH);
                                            trade.THREEBEXSH = Convert.ToDecimal(aw.THREEBEXSH); trade.THREEBEXSNH = Convert.ToDecimal(aw.THREEBEXSNH);
                                            trade.THREEBKMH = Convert.ToDecimal(aw.THREEBKMH); trade.THREEBKMNH = Convert.ToDecimal(aw.THREEBKMNH);
                                            //trade.THREEBEWSH = Convert.ToDecimal(aw.THREEBEWSH); trade.THREEBEWSNH = Convert.ToDecimal(aw.THREEBEWSNH);
                                            trade.THREEBGPH = Convert.ToDecimal(aw.THREEBGPH); trade.THREEBGPNH = Convert.ToDecimal(aw.THREEBGPNH);
                                        }
                                    }
                                    else
                                    {
                                        trade.GMWH = vf[0].Hydrabad; trade.GMWNH = vf[0].NonHydrabad;
                                        trade.GMPDH = vf[1].Hydrabad; trade.GMPDNH = vf[1].NonHydrabad;
                                        trade.GMEXSH = vf[2].Hydrabad; trade.GMEXSNH = vf[2].NonHydrabad;
                                        trade.GMKMH = vf[3].Hydrabad; trade.GMKMNH = vf[3].NonHydrabad;
                                        trade.GMEWSH = vf[4].Hydrabad; trade.GMEWSNH = vf[4].NonHydrabad;
                                        trade.GMGPH = vf[5].Hydrabad; trade.GMGPNH = vf[5].NonHydrabad;
                                        trade.SCWH = vf[6].Hydrabad; trade.SCWNH = vf[6].NonHydrabad;
                                        trade.SCPDH = vf[7].Hydrabad; trade.SCPDNH = vf[7].NonHydrabad;
                                        trade.SCEXSH = vf[8].Hydrabad; trade.SCEXSNH = vf[8].NonHydrabad;
                                        trade.SCKMH = vf[9].Hydrabad; trade.SCKMNH = vf[9].NonHydrabad;
                                        //trade.SCEWSH = vf[10].Hydrabad; trade.SCEWSNH = vf[10].NonHydrabad;
                                        trade.SCGPH = vf[11].Hydrabad; trade.SCGPNH = vf[11].NonHydrabad;
                                        trade.STWH = vf[12].Hydrabad; trade.STWNH = vf[12].NonHydrabad;
                                        trade.STPDH = vf[13].Hydrabad; trade.STPDNH = vf[13].NonHydrabad;
                                        trade.STEXSH = vf[14].Hydrabad; trade.STEXSNH = vf[14].NonHydrabad;
                                        trade.STKMH = vf[15].Hydrabad; trade.STKMNH = vf[15].NonHydrabad;
                                        //trade.STEWSH = vf[16].Hydrabad; trade.STEWSNH = vf[16].NonHydrabad;
                                        trade.STGPH = vf[17].Hydrabad; trade.STGPNH = vf[17].NonHydrabad;
                                        trade.C1WH = vf[18].Hydrabad; trade.C1WNH = vf[18].NonHydrabad;
                                        trade.C1PDH = vf[19].Hydrabad; trade.C1PDNH = vf[19].NonHydrabad;
                                        trade.C1EXSH = vf[20].Hydrabad; trade.C1EXSNH = vf[20].NonHydrabad;
                                        trade.C1KMH = vf[21].Hydrabad; trade.C1KMNH = vf[21].NonHydrabad;
                                        //trade.C1EWSH = vf[22].Hydrabad; trade.C1EWSNH = vf[22].NonHydrabad;
                                        trade.C1GPH = vf[23].Hydrabad; trade.C1GPNH = vf[23].NonHydrabad;
                                        trade.TWOAWH = vf[24].Hydrabad; trade.TWOAWNH = vf[24].NonHydrabad;
                                        trade.TWOAPDH = vf[25].Hydrabad; trade.TWOAPDNH = vf[25].NonHydrabad;
                                        trade.TWOAEXSH = vf[26].Hydrabad; trade.TWOAEXSNH = vf[26].NonHydrabad;
                                        trade.TWOAKMH = vf[27].Hydrabad; trade.TWOAKMNH = vf[27].NonHydrabad;
                                        //trade.TWOAEWSH = vf[28].Hydrabad; trade.TWOAEWSNH = vf[28].NonHydrabad;
                                        trade.TWOAGPH = vf[29].Hydrabad; trade.TWOAGPNH = vf[29].NonHydrabad;
                                        trade.TWOBWH = vf[30].Hydrabad; trade.TWOBWNH = vf[30].NonHydrabad;
                                        trade.TWOBPDH = vf[31].Hydrabad; trade.TWOBPDNH = vf[31].NonHydrabad;
                                        trade.TWOBEXSH = vf[32].Hydrabad; trade.TWOBEXSNH = vf[32].NonHydrabad;
                                        trade.TWOBKMH = vf[33].Hydrabad; trade.TWOBKMNH = vf[33].NonHydrabad;
                                        //trade.TWOBEWSH = vf[34].Hydrabad; trade.TWOBEWSNH = vf[34].NonHydrabad;
                                        trade.TWOBGPH = vf[35].Hydrabad; trade.TWOBGPNH = vf[35].NonHydrabad;
                                        trade.THREEAWH = vf[36].Hydrabad; trade.THREEAWNH = vf[36].NonHydrabad;
                                        trade.THREEAPDH = vf[37].Hydrabad; trade.THREEAPDNH = vf[37].NonHydrabad;
                                        trade.THREEAEXSH = vf[38].Hydrabad; trade.THREEAEXSNH = vf[38].NonHydrabad;
                                        trade.THREEAKMH = vf[39].Hydrabad; trade.THREEAKMNH = vf[39].NonHydrabad;
                                        //trade.THREEAEWSH = vf[40].Hydrabad; trade.THREEAEWSNH = vf[40].NonHydrabad;
                                        trade.THREEAGPH = vf[41].Hydrabad; trade.THREEAGPNH = vf[41].NonHydrabad;
                                        trade.THREEBWH = vf[42].Hydrabad; trade.THREEBWNH = vf[42].NonHydrabad;
                                        trade.THREEBPDH = vf[43].Hydrabad; trade.THREEBPDNH = vf[43].NonHydrabad;
                                        trade.THREEBEXSH = vf[44].Hydrabad; trade.THREEBEXSNH = vf[44].NonHydrabad;
                                        trade.THREEBKMH = vf[45].Hydrabad; trade.THREEBKMNH = vf[45].NonHydrabad;
                                        //trade.THREEBEWSH = vf[46].Hydrabad; trade.THREEBEWSNH = vf[46].NonHydrabad;
                                        trade.THREEBGPH = vf[47].Hydrabad; trade.THREEBGPNH = vf[47].NonHydrabad;
                                    }
                                }
                                else if (round == 2)
                                {
                                    var seatMxId = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == clg.InstituteId && x.AcademicYear == year && x.CourseTypeId == courseType && x.Round == round).Select(y => y.SeatMaxId).FirstOrDefault();
                                    if (seatMxId != 0)
                                    {
                                        var aw = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == seatMxId && x.TradeId == trade.TradeId).FirstOrDefault();
                                        if (aw != null)
                                        {
                                            var aw1 = Round2SeatData(trade, aw);
                                            Round2SeatData(trade, aw1);
                                        }
                                        else
                                        {
                                            GenerateTradewiseSeat aw1 = GetAdmittedSeats(clg.InstituteId, trade.TradeId, year, courseType, round);
                                            bool sttus = SubmitCollegeWiseSeatData(aw1, clg.InstituteId, trade.TradeId, year, courseType, round, loginId);
                                            if (sttus == true)
                                            {
                                                Round2SeatData(trade, aw1);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        GenerateTradewiseSeat aw = GetAdmittedSeats(clg.InstituteId, trade.TradeId, year, courseType, round);
                                        bool sttus = SubmitCollegeWiseSeatData(aw, clg.InstituteId, trade.TradeId, year, courseType, round, loginId);
                                        if (sttus == true)
                                        {
                                            Round2SeatData(trade, aw) ;
                                        }
                                    }
                                }
                                else if (round == 3)
                                {
                                    var seatMxId = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == clg.InstituteId && x.AcademicYear == year && x.CourseTypeId == courseType && x.Round == round).Select(y => y.SeatMaxId).FirstOrDefault();
                                    if (seatMxId != 0)
                                    {
                                        var aw = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == seatMxId && x.TradeId == trade.TradeId).FirstOrDefault();
                                        if (aw != null)
                                        {
                                            trade.GMGP = Convert.ToDecimal(aw.GMGP);
                                            trade.SCGP = Convert.ToDecimal(aw.SCGP);
                                            trade.STGP = Convert.ToDecimal(aw.STGP);
                                            trade.C1GP = Convert.ToDecimal(aw.C1GP);
                                            trade.TWOAGP = Convert.ToDecimal(aw.TWOAGP);
                                            trade.TWOBGP = Convert.ToDecimal(aw.TWOBGP);
                                            trade.THREEAGP = Convert.ToDecimal(aw.THREEAGP);
                                            trade.THREEBGP = Convert.ToDecimal(aw.THREEBGP);
                                        }
                                        else
                                        {
                                            GenerateTradewiseSeat aw1 = GetAdmittedSeats(clg.InstituteId, trade.TradeId, year, courseType, round);
                                            bool sttus = SubmitCollegeWiseSeatData(aw1, clg.InstituteId, trade.TradeId, year, courseType, round, loginId);
                                            if (sttus == true)
                                            {
                                                trade.GMGP = aw1.GMGP;
                                                trade.SCGP = aw1.SCGP;
                                                trade.STGP = aw1.STGP;
                                                trade.C1GP = aw1.C1GP;
                                                trade.TWOAGP = aw1.TWOAGP;
                                                trade.TWOBGP = aw1.TWOBGP;
                                                trade.THREEAGP = aw1.THREEAGP;
                                                trade.THREEBGP = aw1.THREEBGP;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        GenerateTradewiseSeat aw = GetAdmittedSeats(clg.InstituteId, trade.TradeId, year, courseType, round);
                                        bool sttus = SubmitCollegeWiseSeatData(aw, clg.InstituteId, trade.TradeId, year, courseType, round, loginId);
                                        if (sttus == true)
                                        {
                                            trade.GMGP = aw.GMGP;
                                            trade.SCGP = aw.SCGP;
                                            trade.STGP = aw.STGP;
                                            trade.C1GP = aw.C1GP;
                                            trade.TWOAGP = aw.TWOAGP;
                                            trade.TWOBGP = aw.TWOBGP;
                                            trade.THREEAGP = aw.THREEAGP;
                                            trade.THREEBGP = aw.THREEBGP;
                                        }
                                    }
                                }
                                else if (round == 4)
                                {
                                    var seatMxId = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == clg.InstituteId && x.AcademicYear == year && x.CourseTypeId == courseType && x.Round == round).Select(y => y.SeatMaxId).FirstOrDefault();
                                    if (seatMxId != 0)
                                    {
                                        var aw = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == seatMxId && x.TradeId == trade.TradeId).FirstOrDefault();
                                        if (aw != null)
                                        {
                                            trade.GMGP = Convert.ToDecimal(aw.GMGP);
                                            trade.SCGP = Convert.ToDecimal(aw.SCGP);
                                            trade.STGP = Convert.ToDecimal(aw.STGP);
                                            trade.C1GP = Convert.ToDecimal(aw.C1GP);
                                            trade.TWOAGP = Convert.ToDecimal(aw.TWOAGP);
                                            trade.TWOBGP = Convert.ToDecimal(aw.TWOBGP);
                                            trade.THREEAGP = Convert.ToDecimal(aw.THREEAGP);
                                            trade.THREEBGP = Convert.ToDecimal(aw.THREEBGP);
                                        }
                                        else
                                        {
                                            GenerateTradewiseSeat aw1 = GetAdmittedSeats(clg.InstituteId, trade.TradeId, year, courseType, round);
                                            bool sttus = SubmitCollegeWiseSeatData(aw1, clg.InstituteId, trade.TradeId, year, courseType, round, loginId);
                                            if (sttus == true)
                                            {
                                                trade.GMGP = aw1.GMGP;
                                                trade.SCGP = aw1.SCGP;
                                                trade.STGP = aw1.STGP;
                                                trade.C1GP = aw1.C1GP;
                                                trade.TWOAGP = aw1.TWOAGP;
                                                trade.TWOBGP = aw1.TWOBGP;
                                                trade.THREEAGP = aw1.THREEAGP;
                                                trade.THREEBGP = aw1.THREEBGP;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        GenerateTradewiseSeat aw = GetAdmittedSeats(clg.InstituteId, trade.TradeId, year, courseType, round);
                                        bool sttus = SubmitCollegeWiseSeatData(aw, clg.InstituteId, trade.TradeId, year, courseType, round, loginId);
                                        if (sttus == true)
                                        {
                                            trade.GMGP = aw.GMGP;
                                            trade.SCGP = aw.SCGP;
                                            trade.STGP = aw.STGP;
                                            trade.C1GP = aw.C1GP;
                                            trade.TWOAGP = aw.TWOAGP;
                                            trade.TWOBGP = aw.TWOBGP;
                                            trade.THREEAGP = aw.THREEAGP;
                                            trade.THREEBGP = aw.THREEBGP;
                                        }
                                    }
                                }
                                else
                                {
                                    var seatMxId = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == clg.InstituteId && x.AcademicYear == year && x.CourseTypeId == courseType && x.Round == round).Select(y => y.SeatMaxId).FirstOrDefault();
                                    if (seatMxId != 0)
                                    {
                                        var aw = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == seatMxId && x.TradeId == trade.TradeId).FirstOrDefault();
                                        if (aw != null)
                                        {
                                            trade.GMGP = Convert.ToDecimal(aw.GMGP);
                                        }
                                        else
                                        {
                                            GenerateTradewiseSeat aw1 = GetAdmittedSeats(clg.InstituteId, trade.TradeId, year, courseType, round);
                                            bool sttus = SubmitCollegeWiseSeatData(aw1, clg.InstituteId, trade.TradeId, year, courseType, round, loginId);
                                            if (sttus == true)
                                            {
                                                trade.GMGP = aw1.GMGP;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        GenerateTradewiseSeat aw = GetAdmittedSeats(clg.InstituteId, trade.TradeId, year, courseType, round);
                                        bool sttus = SubmitCollegeWiseSeatData(aw, clg.InstituteId, trade.TradeId, year, courseType, round, loginId);
                                        if (sttus == true)
                                        {
                                            trade.GMGP = aw.GMGP;
                                        }
                                    }
                                }

                                ttl_seat += trade.Seats;
                            }
                            clg.TotalSeats = ttl_seat;
                        }
                    }

                    return div;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private decimal kar_hyd_per(List<tbl_HYD_kar_rules_value> hyd_kar_ruleValues, int rule_id, int defaultVal, decimal totalSeats)
        {
            decimal returnVal = 0;
            var percentVal = hyd_kar_ruleValues.Where(x => x.Hyd_NonHyd_rules_id == rule_id)
                                             .Select(v => v.RuleValue).FirstOrDefault();
            if (percentVal > 0)
                returnVal = (percentVal / 100) * totalSeats;
            else
                returnVal = (defaultVal / 100) * totalSeats;
            return returnVal;
        }

        public bool SubmitSeatMatrixCollegeWise(List<Tradewideseat> listItem, int collegeId, int round, int year, int appType, int courseType, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var setmxId = 0;
                    var rs = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == collegeId && x.AcademicYear == year && x.Round == round && x.CourseTypeId == courseType).FirstOrDefault();
                    if (rs == null)
                    {
                        tbl_SeatMatrix_Main sm = new tbl_SeatMatrix_Main
                        {
                            AcademicYear = year,
                            InstituteId = collegeId,
                            ApplicantType = appType,
                            Round = round,
                            Status = 5,
                            IsActive = true,
                            CreatedOn = DateTime.Now,
                            CourseTypeId = courseType,
                            Remarks = "aw"
                        };
                        _db.tbl_SeatMatrix_Main.Add(sm);
                        _db.SaveChanges();
                        setmxId = _db.tbl_SeatMatrix_Main.OrderByDescending(x => x.SeatMaxId).Select(y => y.SeatMaxId).FirstOrDefault();
                        tbl_SeatMatrix_Trans st = new tbl_SeatMatrix_Trans
                        {
                            SeatMaxId = setmxId,
                            AcademicYear = year,
                            InstituteId = collegeId,
                            ApplicantType = appType,
                            Round = round,
                            Status = 5,
                            Remarks = "aw",
                            IsActive = true,
                            CreatedOn = DateTime.Now,
                            CourseTypeId = courseType
                        };
                        _db.tbl_SeatMatrix_Trans.Add(st);
                    }
                    else
                        setmxId = rs.SeatMaxId;
                    #region commented code
                    //if (round == 1 || round == 2)
                    //{
                    //    foreach (var tm in listItem)
                    //    {
                    //        tbl_SeatMatrix_TradeWise tw = new tbl_SeatMatrix_TradeWise();
                    //        tw.SeatMaxId = setmxId; tw.IsActive = true; tw.CreatedBy = loginId; tw.CreatedOn = DateTime.Now;
                    //        tw.TradeId = tm.TradeId; tw.GMWH = tm.GMWH; tw.GMWNH = tm.GMWNH; tw.GMPDH = tm.GMPDH; tw.GMPDNH = tm.GMPDNH; tw.GMEXSH = tm.GMEXSH; tw.GMEXSNH = tm.GMEXSNH;
                    //        tw.GMKMH = tm.GMKMH; tw.GMKMNH = tm.GMKMNH; tw.GMEWSH = tm.GMEWSH; tw.GMEWSNH = tm.GMEWSNH; tw.GMGPH = tm.GMGPH; tw.GMGPNH = tm.GMGPNH; tw.SCWH = tm.SCWH; tw.SCWNH = tm.SCWNH;
                    //        tw.SCPDH = tm.SCPDH; tw.SCPDNH = tm.SCPDNH; tw.SCEXSH = tm.SCEXSH; tw.SCEXSNH = tm.SCEXSNH; tw.SCKMH = tm.SCKMH; tw.SCKMNH = tm.SCKMNH; tw.SCEWSH = tm.SCEWSH;
                    //        tw.SCEWSNH = tm.SCEWSNH; tw.SCGPH = tm.SCGPH; tw.SCGPNH = tm.SCGPNH; tw.STWH = tm.STWH; tw.STWNH = tm.STWNH; tw.STPDH = tm.STPDH; tw.STPDNH = tm.STPDNH; tw.STEXSH = tm.STEXSH;
                    //        tw.STEXSNH = tm.STEXSNH; tw.STKMH = tm.STKMH; tw.STKMNH = tm.STKMNH; tw.STEWSH = tm.STEWSH; tw.STEWSNH = tm.STEWSNH; tw.STGPH = tm.STGPH; tw.STGPNH = tm.STGPNH; tw.C1WH = tm.C1WH;
                    //        tw.C1WNH = tm.C1WNH; tw.C1PDH = tm.C1PDH; tw.C1PDNH = tm.C1PDNH; tw.C1EXSH = tm.C1EXSH; tw.C1EXSNH = tm.C1EXSNH; tw.C1KMH = tm.C1KMH; tw.C1KMNH = tm.C1KMNH; tw.C1EWSH = tm.C1EWSH;
                    //        tw.C1EWSNH = tm.C1EWSNH; tw.C1GPH = tm.C1GPH; tw.C1GPNH = tm.C1GPNH; tw.TWOAWH = tm.TWOAWH; tw.TWOAWNH = tm.TWOAWNH; tw.TWOAPDH = tm.TWOAPDH; tw.TWOAPDNH = tm.TWOAPDNH;
                    //        tw.TWOAEXSH = tm.TWOAEXSH; tw.TWOAEXSNH = tm.TWOAEXSNH; tw.TWOAKMH = tm.TWOAKMH; tw.TWOAKMNH = tm.TWOAKMNH; tw.TWOAEWSH = tm.TWOAEWSH; tw.TWOAEWSNH = tm.TWOAEWSNH;
                    //        tw.TWOAGPH = tm.TWOAGPH; tw.TWOAGPNH = tm.TWOAGPNH; tw.TWOBWH = tm.TWOBWH; tw.TWOBWNH = tm.TWOBWNH; tw.TWOBPDH = tm.TWOBPDH; tw.TWOBPDNH = tm.TWOBPDNH;
                    //        tw.TWOBEXSH = tm.TWOBEXSH; tw.TWOBEXSNH = tm.TWOBEXSNH; tw.TWOBKMH = tm.TWOBKMH; tw.TWOBKMNH = tm.TWOBKMNH; tw.TWOBEWSH = tm.TWOBEWSH; tw.TWOBEWSNH = tm.TWOBEWSNH;
                    //        tw.TWOBGPH = tm.TWOBGPH; tw.TWOBGPNH = tm.TWOBGPNH; tw.THREEAWH = tm.THREEAWH; tw.THREEAWNH = tm.THREEAWNH; tw.THREEAPDH = tm.THREEAPDH; tw.THREEAPDNH = tm.THREEAPDNH;
                    //        tw.THREEAEXSH = tm.THREEAEXSH; tw.THREEAEXSNH = tm.THREEAEXSNH; tw.THREEAKMH = tm.THREEAKMH; tw.THREEAKMNH = tm.THREEAKMNH; tw.THREEAEWSH = tm.THREEAEWSH;
                    //        tw.THREEAEWSNH = tm.THREEAEWSNH; tw.THREEAGPH = tm.THREEAGPH; tw.THREEAGPNH = tm.THREEAGPNH; tw.THREEBWH = tm.THREEBWH; tw.THREEBWNH = tm.THREEBWNH;
                    //        tw.THREEBPDH = tm.THREEBPDH; tw.THREEBPDNH = tm.THREEBPDNH; tw.THREEBEXSH = tm.THREEBEXSH; tw.THREEEXSNH = tm.THREEEXSNH; tw.THREEBKMH = tm.THREEBKMH;
                    //        tw.THREEBKMNH = tm.THREEBKMNH; tw.THREEBEWSH = tm.THREEBEWSH; tw.THREEBEWSNH = tm.THREEBEWSNH; tw.THREEBGPH = tm.THREEBGPH; tw.THREEBGPNH = tm.THREEBGPNH;
                    //        _db.tbl_SeatMatrix_TradeWise.Add(tw);
                    //    }
                    //}
                    //else if (round == 6)
                    //{
                    //    foreach (var tm in listItem)
                    //    {
                    //        tbl_SeatMatrix_TradeWise tw = new tbl_SeatMatrix_TradeWise();
                    //        tw.SeatMaxId = setmxId;
                    //        tw.IsActive = true;
                    //        tw.CreatedBy = loginId;
                    //        tw.CreatedOn = DateTime.Now;
                    //        tw.GMGP = tm.GMGP;
                    //        _db.tbl_SeatMatrix_TradeWise.Add(tw);
                    //    }

                    //}
                    //else
                    //{
                    //    foreach (var tm in listItem)
                    //    {
                    //        tbl_SeatMatrix_TradeWise tw = new tbl_SeatMatrix_TradeWise();
                    //        tw.SeatMaxId = setmxId;
                    //        tw.IsActive = true;
                    //        tw.CreatedBy = loginId;
                    //        tw.CreatedOn = DateTime.Now;
                    //        tw.GMGP = tm.GMGP;
                    //        tw.SCGP = tm.SCGP;
                    //        tw.STGP = tm.STGP;
                    //        tw.C1GP = tm.C1GP;
                    //        tw.TWOAGP = tm.TWOAGP;
                    //        tw.TWOBGP = tm.TWOBGP;
                    //        tw.THREEAGP = tm.THREEAGP;
                    //        tw.THREEBGP = tm.THREEBGP;
                    //        _db.tbl_SeatMatrix_TradeWise.Add(tw);
                    //    }
                    //}
                    #endregion
                    bool val = InsertSeatMetrixData(listItem, setmxId, round, loginId);
                    if (val)
                    {
                        _db.SaveChanges();
                        transaction.Commit();
                    }
                    return val;

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        private bool InsertSeatMetrixData(List<Tradewideseat> listItem, int setmxId, int round, int loginId)
        {
            bool isInsertorUpdate = false;
            foreach (var tm in listItem)
            {
                var checkTrade = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == setmxId && x.TradeId == tm.TradeId).FirstOrDefault();
                if (checkTrade == null)
                {
                    tbl_SeatMatrix_TradeWise tw;
                    if (round == 1 || round == 2)
                        tw = new tbl_SeatMatrix_TradeWise
                        {
                            SeatMaxId = setmxId,
                            IsActive = true,
                            CreatedBy = loginId,
                            CreatedOn = DateTime.Now,
                            TradeId = tm.TradeId,
                            GMWH = tm.GMWH,
                            GMWNH = tm.GMWNH,
                            GMPDH = tm.GMPDH,
                            GMPDNH = tm.GMPDNH,
                            GMEXSH = tm.GMEXSH,
                            GMEXSNH = tm.GMEXSNH,
                            GMKMH = tm.GMKMH,
                            GMKMNH = tm.GMKMNH,
                            GMEWSH = tm.GMEWSH,
                            GMEWSNH = tm.GMEWSNH,
                            GMGPH = tm.GMGPH,
                            GMGPNH = tm.GMGPNH,
                            SCWH = tm.SCWH,
                            SCWNH = tm.SCWNH,
                            SCPDH = tm.SCPDH,
                            SCPDNH = tm.SCPDNH,
                            SCEXSH = tm.SCEXSH,
                            SCEXSNH = tm.SCEXSNH,
                            SCKMH = tm.SCKMH,
                            SCKMNH = tm.SCKMNH,
                            SCEWSH = tm.SCEWSH,
                            SCEWSNH = tm.SCEWSNH,
                            SCGPH = tm.SCGPH,
                            SCGPNH = tm.SCGPNH,
                            STWH = tm.STWH,
                            STWNH = tm.STWNH,
                            STPDH = tm.STPDH,
                            STPDNH = tm.STPDNH,
                            STEXSH = tm.STEXSH,
                            STEXSNH = tm.STEXSNH,
                            STKMH = tm.STKMH,
                            STKMNH = tm.STKMNH,
                            STEWSH = tm.STEWSH,
                            STEWSNH = tm.STEWSNH,
                            STGPH = tm.STGPH,
                            STGPNH = tm.STGPNH,
                            C1WH = tm.C1WH,
                            C1WNH = tm.C1WNH,
                            C1PDH = tm.C1PDH,
                            C1PDNH = tm.C1PDNH,
                            C1EXSH = tm.C1EXSH,
                            C1EXSNH = tm.C1EXSNH,
                            C1KMH = tm.C1KMH,
                            C1KMNH = tm.C1KMNH,
                            C1EWSH = tm.C1EWSH,
                            C1EWSNH = tm.C1EWSNH,
                            C1GPH = tm.C1GPH,
                            C1GPNH = tm.C1GPNH,
                            TWOAWH = tm.TWOAWH,
                            TWOAWNH = tm.TWOAWNH,
                            TWOAPDH = tm.TWOAPDH,
                            TWOAPDNH = tm.TWOAPDNH,
                            TWOAEXSH = tm.TWOAEXSH,
                            TWOAEXSNH = tm.TWOAEXSNH,
                            TWOAKMH = tm.TWOAKMH,
                            TWOAKMNH = tm.TWOAKMNH,
                            TWOAEWSH = tm.TWOAEWSH,
                            TWOAEWSNH = tm.TWOAEWSNH,
                            TWOAGPH = tm.TWOAGPH,
                            TWOAGPNH = tm.TWOAGPNH,
                            TWOBWH = tm.TWOBWH,
                            TWOBWNH = tm.TWOBWNH,
                            TWOBPDH = tm.TWOBPDH,
                            TWOBPDNH = tm.TWOBPDNH,
                            TWOBEXSH = tm.TWOBEXSH,
                            TWOBEXSNH = tm.TWOBEXSNH,
                            TWOBKMH = tm.TWOBKMH,
                            TWOBKMNH = tm.TWOBKMNH,
                            TWOBEWSH = tm.TWOBEWSH,
                            TWOBEWSNH = tm.TWOBEWSNH,
                            TWOBGPH = tm.TWOBGPH,
                            TWOBGPNH = tm.TWOBGPNH,
                            THREEAWH = tm.THREEAWH,
                            THREEAWNH = tm.THREEAWNH,
                            THREEAPDH = tm.THREEAPDH,
                            THREEAPDNH = tm.THREEAPDNH,
                            THREEAEXSH = tm.THREEAEXSH,
                            THREEAEXSNH = tm.THREEAEXSNH,
                            THREEAKMH = tm.THREEAKMH,
                            THREEAKMNH = tm.THREEAKMNH,
                            THREEAEWSH = tm.THREEAEWSH,
                            THREEAEWSNH = tm.THREEAEWSNH,
                            THREEAGPH = tm.THREEAGPH,
                            THREEAGPNH = tm.THREEAGPNH,
                            THREEBWH = tm.THREEBWH,
                            THREEBWNH = tm.THREEBWNH,
                            THREEBPDH = tm.THREEBPDH,
                            THREEBPDNH = tm.THREEBPDNH,
                            THREEBEXSH = tm.THREEBEXSH,
                            THREEBEXSNH = tm.THREEBEXSNH,
                            THREEBKMH = tm.THREEBKMH,
                            THREEBKMNH = tm.THREEBKMNH,
                            THREEBEWSH = tm.THREEBEWSH,
                            THREEBEWSNH = tm.THREEBEWSNH,
                            THREEBGPH = tm.THREEBGPH,
                            THREEBGPNH = tm.THREEBGPNH
                        };
                    else if (round == 6)
                        tw = new tbl_SeatMatrix_TradeWise
                        {
                            SeatMaxId = setmxId,
                            IsActive = true,
                            CreatedBy = loginId,
                            CreatedOn = DateTime.Now,
                            GMGP = tm.GMGP
                        };
                    else
                        tw = new tbl_SeatMatrix_TradeWise
                        {
                            SeatMaxId = setmxId,
                            IsActive = true,
                            CreatedBy = loginId,
                            CreatedOn = DateTime.Now,
                            GMGP = tm.GMGP,
                            SCGP = tm.SCGP,
                            STGP = tm.STGP,
                            C1GP = tm.C1GP,
                            TWOAGP = tm.TWOAGP,
                            TWOBGP = tm.TWOBGP,
                            THREEAGP = tm.THREEAGP,
                            THREEBGP = tm.THREEBGP
                        };
                    if (tw != null)
                    {                        
                        _db.tbl_SeatMatrix_TradeWise.Add(tw);
                        isInsertorUpdate = true;
                    }
                }
            }
            return isInsertorUpdate;
        }

        public List<DivisionModel> GetDivisions()
        {
            try
            {
                var res = (from aa in _db.tbl_division_master
                           select new DivisionModel
                           {
                               DivisionId = aa.division_id,
                               DivisionName = aa.division_name,
                               Institutes = new List<InstituteModel> { new InstituteModel { InstituteId = 0, InstituteName = "zero" } }
                           }).ToList();
                foreach (var s in res)
                {
                    var insti = (from aa in _db.tbl_iti_college_details
                                 where aa.division_id == s.DivisionId
                                 select new InstituteModel
                                 {
                                     InstituteId = aa.iti_college_id,
                                     InstituteName = aa.iti_college_name,
                                     Trades = new List<TradesModel> { new TradesModel { TradeId = 0, TradeName = "zero" } }
                                 }).ToList();

                    foreach (var itm in insti)
                    {

                        var trade = (from ee in _db.tbl_ITI_Trade
                                     join ff in _db.tbl_trade_mast on ee.TradeCode equals ff.trade_id
                                     where ee.ITICode == itm.InstituteId
                                     select new TradesModel
                                     {
                                         TradeId = ff.trade_id,
                                         TradeName = ff.trade_name
                                     }).ToList();

                        if (trade.Count != 0)
                        {
                            foreach (var i in trade)
                            {
                                itm.Trades.Add(i);
                            }
                            s.Institutes.Add(itm);
                        }
                        else
                        {
                            s.Institutes.Add(itm);
                        }
                    }

                }

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool SubmitSeatMatrix(int year, int appType, int courseType, int round, int role, int loginId, int roleId, string remark)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var institute = _db.tbl_SeatMatrix_Main.Where(x => x.AcademicYear == year && x.CourseTypeId == courseType && x.Round == round).ToList();
                    foreach (var ins in institute)
                    {
                        ins.Status = 5;
                        ins.Remarks = remark;
                        ins.FlowId = role;
                        ins.CreatedBy = roleId;
                    }
                    var institute1 = _db.tbl_SeatMatrix_Trans.Where(x => x.AcademicYear == year && x.CourseTypeId == courseType && x.Round == round).ToList();
                    foreach (var tran in institute1)
                    {
                        tran.Status = 5;
                        tran.FlowId = role;
                        tran.Remarks = remark;
                        tran.CreatedBy = roleId;
                    }

                    _db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }

            }
        }

        #endregion

        #region Update seat matrix by dhanraj   
        public List<DivisionUpdateModel> GetDivisionsInstitutesTrades(int year, int round, int applicantType, int courseId)
        {
            try
            {
                var res = (from aa in _db.tbl_division_master
                           select new DivisionUpdateModel
                           {
                               DivisionId = aa.division_id,
                               DivisionName = aa.division_name,
                               Institutes = new List<InstituteUpdateModel> { new InstituteUpdateModel { InstituteId = 0, InstituteName = "zero", Round = 0, TotalSeats = 0 } }
                           }).ToList();


                var instituteList = _db.tbl_SeatMatrix_Main.Where(x => x.AcademicYear == year
                && x.Round == round && x.ApplicantType == applicantType && x.CourseTypeId == courseId)
                    .Select(y => y.InstituteId).ToList();

                List<InstituteUpdateModel> somer = new List<InstituteUpdateModel>();

                foreach (var institute in instituteList)
                {
                    var instituteName = (from abc in _db.tbl_iti_college_details
                                         where abc.iti_college_id == institute
                                         select new InstituteUpdateModel
                                         {
                                             InstituteId = abc.iti_college_id,
                                             InstituteName = abc.iti_college_name,
                                             DivisionId = abc.division_id,
                                             Round = round,
                                             Trades = new List<Tradewideseat> {
                                                new Tradewideseat {
                                                    TradeId = 0, TradeName = "zero",Seats = 0, GMWH = 0 ,GMWNH = 0 , GMPDH = 0, GMPDNH = 0,  GMEXSH = 0, GMEXSNH = 0, GMKMH =0, GMKMNH =0,
                                                    GMEWSH =0,  GMEWSNH =0, GMGPH =0, GMGPNH =0, SCWH =0, SCWNH =0, SCPDH =0, SCPDNH =0, SCEXSH =0, SCEXSNH =0, SCKMH =0, SCKMNH =0, SCEWSH =0,
                                                    SCEWSNH =0, SCGPH =0, SCGPNH=0, STWH =0, STWNH =0, STPDH =0, STPDNH =0, STEXSH =0, STEXSNH =0, STKMH =0, STKMNH =0, STEWSH =0, STEWSNH=0, STGPH =0,
                                                    STGPNH =0, C1WH =0, C1WNH =0, C1PDH =0, C1PDNH=0, C1EXSH =0,  C1EXSNH =0, C1KMH =0, C1KMNH =0, C1EWSH =0, C1EWSNH =0, C1GPH = 0, C1GPNH = 0, TWOAWH = 0,
                                                    TWOAWNH = 0, TWOAPDH = 0, TWOAPDNH = 0, TWOAEXSH = 0, TWOAEXSNH = 0, TWOAKMH = 0, TWOAKMNH = 0, TWOAEWSH = 0, TWOAEWSNH = 0, TWOAGPH = 0, TWOAGPNH = 0,
                                                    TWOBWH = 0, TWOBWNH = 0, TWOBPDH = 0, TWOBPDNH = 0, TWOBEXSH = 0, TWOBEXSNH = 0, TWOBKMH = 0, TWOBKMNH = 0, TWOBEWSH = 0, TWOBEWSNH = 0,  TWOBGPH = 0,
                                                    TWOBGPNH = 0, THREEAWH = 0, THREEAWNH = 0, THREEAPDH = 0, THREEAPDNH = 0,  THREEAEXSH = 0,  THREEAEXSNH = 0, THREEAKMH = 0, THREEAKMNH = 0, THREEAEWSH = 0,
                                                    THREEAEWSNH = 0,  THREEAGPH = 0, THREEAGPNH = 0, THREEBWH = 0, THREEBWNH = 0, THREEBPDH = 0, THREEBPDNH = 0,  THREEBEXSH = 0, THREEBEXSNH = 0, THREEBKMH = 0,
                                                    THREEBKMNH = 0, THREEBEWSH = 0, THREEBEWSNH = 0,  THREEBGPH = 0, THREEBGPNH = 0
                                                }
                                             }

                                         }).FirstOrDefault();

                    var seatList = _db.tbl_SeatMatrix_Main.Where(x => x.AcademicYear == year && x.Round == round
                    && x.InstituteId == institute && x.ApplicantType == applicantType && x.CourseTypeId == courseId).
                    Select(y => y.SeatMaxId).ToList();

                    int ttl_seats = 0;
                    foreach (var itm in seatList)
                    {
                        var tradeRes = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == itm).ToList();
                        if (round == 1 || round == 2)
                        {
                            foreach (var tm in tradeRes)
                            {
                                Tradewideseat tw = new Tradewideseat();
                                var tdname = _db.tbl_trade_mast.Where(x => x.trade_id == tm.TradeId).Select(y => y.trade_name).FirstOrDefault();
                                tw.TradeName = tdname;
                                int yer = Convert.ToInt32(_db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault().Split('-')[1]);
                                var cours = _db.tbl_SeatMatrix_Main.Where(x => x.AcademicYear == year && x.Round == round && x.InstituteId == institute).Select(y => y.CourseTypeId).FirstOrDefault();
                                tw.Units = _db.tbl_ITI_trade_seat_master.Where(x => x.Status == 4 && x.AcademicYear.Year == year && x.Trade_ITI_Id == institute && x.CourseType == cours).Select(a => a.UnitId).Count();
                                tw.TradeYear = _db.tbl_trade_mast.Where(x => x.trade_id == tm.TradeId).Select(y => y.trade_duration).FirstOrDefault();
                                tw.Seats = Convert.ToInt32(tm.GMWH) + Convert.ToInt32(tm.GMWNH) + Convert.ToInt32(tm.GMPDH) + Convert.ToInt32(tm.GMPDNH) + Convert.ToInt32(tm.GMEXSH) + Convert.ToInt32(tm.GMEXSNH) + Convert.ToInt32(tm.GMKMH) + Convert.ToInt32(tm.GMKMNH) + Convert.ToInt32(tm.GMEWSH) + Convert.ToInt32(tm.GMEWSNH) + Convert.ToInt32(tm.GMGPH) + Convert.ToInt32(tm.GMGPNH)
                                + Convert.ToInt32(tm.SCWH) + Convert.ToInt32(tm.SCWNH) + Convert.ToInt32(tm.SCPDH) + Convert.ToInt32(tm.SCPDNH) + Convert.ToInt32(tm.SCEXSH) + Convert.ToInt32(tm.SCEXSNH) + Convert.ToInt32(tm.SCKMH) + Convert.ToInt32(tm.SCKMNH) + Convert.ToInt32(tm.SCEWSH) + Convert.ToInt32(tm.SCEWSNH) + Convert.ToInt32(tm.SCGPH) + Convert.ToInt32(tm.SCGPNH)
                                + Convert.ToInt32(tm.STWH) + Convert.ToInt32(tm.STWNH) + Convert.ToInt32(tm.STPDH) + Convert.ToInt32(tm.STPDNH) + Convert.ToInt32(tm.STEXSH) + Convert.ToInt32(tm.STEXSNH) + Convert.ToInt32(tm.STKMH) + Convert.ToInt32(tm.STKMNH) + Convert.ToInt32(tm.STEWSH) + Convert.ToInt32(tm.STEWSNH) + Convert.ToInt32(tm.STGPH) + Convert.ToInt32(tm.STGPNH)
                                + Convert.ToInt32(tm.C1WH) + Convert.ToInt32(tm.C1WNH) + Convert.ToInt32(tm.C1PDH) + Convert.ToInt32(tm.C1PDNH) + Convert.ToInt32(tm.C1EXSH) + Convert.ToInt32(tm.C1EXSNH) + Convert.ToInt32(tm.C1KMH) + Convert.ToInt32(tm.C1KMNH) + Convert.ToInt32(tm.C1EWSH) + Convert.ToInt32(
                                tm.C1EWSNH) + Convert.ToInt32(tm.C1GPH) + Convert.ToInt32(tm.C1GPNH) + Convert.ToInt32(tm.TWOAWH) + Convert.ToInt32(tm.TWOAWNH) + Convert.ToInt32(tm.TWOAPDH) + Convert.ToInt32(tm.TWOAPDNH) + Convert.ToInt32(
                                tm.TWOAEXSH) + Convert.ToInt32(tm.TWOAEXSNH) + Convert.ToInt32(tm.TWOAKMH) + Convert.ToInt32(tm.TWOAKMNH) + Convert.ToInt32(tm.TWOAEWSH) + Convert.ToInt32(tm.TWOAEWSNH) + Convert.ToInt32(
                                tm.TWOAGPH) + Convert.ToInt32(tm.TWOAGPNH) + Convert.ToInt32(tm.TWOBWH) + Convert.ToInt32(tm.TWOBWNH) + Convert.ToInt32(tm.TWOBPDH) + Convert.ToInt32(tm.TWOBPDNH) + Convert.ToInt32(
                                tm.TWOBEXSH) + Convert.ToInt32(tm.TWOBEXSNH) + Convert.ToInt32(tm.TWOBKMH) + Convert.ToInt32(tm.TWOBKMNH) + Convert.ToInt32(tm.TWOBEWSH) + Convert.ToInt32(tm.TWOBEWSNH) + Convert.ToInt32(
                                tm.TWOBGPH) + Convert.ToInt32(tm.TWOBGPNH) + Convert.ToInt32(tm.THREEAWH) + Convert.ToInt32(tm.THREEAWNH) + Convert.ToInt32(tm.THREEAPDH) + Convert.ToInt32(tm.THREEAPDNH) + Convert.ToInt32(
                                tm.THREEAEXSH) + Convert.ToInt32(tm.THREEAEXSNH) + Convert.ToInt32(tm.THREEAKMH) + Convert.ToInt32(tm.THREEAKMNH) + Convert.ToInt32(tm.THREEAEWSH) + Convert.ToInt32(
                                tm.THREEAEWSNH) + Convert.ToInt32(tm.THREEAGPH) + Convert.ToInt32(tm.THREEAGPNH) + Convert.ToInt32(tm.THREEBWH) + Convert.ToInt32(tm.THREEBWNH) + Convert.ToInt32(
                                tm.THREEBPDH) + Convert.ToInt32(tm.THREEBPDNH) + Convert.ToInt32(tm.THREEBEXSH) + Convert.ToInt32(tm.THREEBEXSNH) + Convert.ToInt32(tm.THREEBKMH) + Convert.ToInt32(
                                tm.THREEBKMNH) + Convert.ToInt32(tm.THREEBEWSH) + Convert.ToInt32(tm.THREEBEWSNH) + Convert.ToInt32(tm.THREEBGPH) + Convert.ToInt32(tm.THREEBGPNH);

                                tw.TradeId = tm.TradeId; tw.GMWH = tm.GMWH; tw.GMWNH = tm.GMWNH; tw.GMPDH = tm.GMPDH; tw.GMPDNH = tm.GMPDNH; tw.GMEXSH = tm.GMEXSH; tw.GMEXSNH = tm.GMEXSNH;
                                tw.GMKMH = tm.GMKMH; tw.GMKMNH = tm.GMKMNH; tw.GMEWSH = tm.GMEWSH; tw.GMEWSNH = tm.GMEWSNH; tw.GMGPH = tm.GMGPH; tw.GMGPNH = tm.GMGPNH; tw.SCWH = tm.SCWH; tw.SCWNH = tm.SCWNH;
                                tw.SCPDH = tm.SCPDH; tw.SCPDNH = tm.SCPDNH; tw.SCEXSH = tm.SCEXSH; tw.SCEXSNH = tm.SCEXSNH; tw.SCKMH = tm.SCKMH; tw.SCKMNH = tm.SCKMNH; tw.SCEWSH = tm.SCEWSH;
                                tw.SCEWSNH = tm.SCEWSNH; tw.SCGPH = tm.SCGPH; tw.SCGPNH = tm.SCGPNH; tw.STWH = tm.STWH; tw.STWNH = tm.STWNH; tw.STPDH = tm.STPDH; tw.STPDNH = tm.STPDNH; tw.STEXSH = tm.STEXSH;
                                tw.STEXSNH = tm.STEXSNH; tw.STKMH = tm.STKMH; tw.STKMNH = tm.STKMNH; tw.STEWSH = tm.STEWSH; tw.STEWSNH = tm.STEWSNH; tw.STGPH = tm.STGPH; tw.STGPNH = tm.STGPNH; tw.C1WH = tm.C1WH;
                                tw.C1WNH = tm.C1WNH; tw.C1PDH = tm.C1PDH; tw.C1PDNH = tm.C1PDNH; tw.C1EXSH = tm.C1EXSH; tw.C1EXSNH = tm.C1EXSNH; tw.C1KMH = tm.C1KMH; tw.C1KMNH = tm.C1KMNH; tw.C1EWSH = tm.C1EWSH;
                                tw.C1EWSNH = tm.C1EWSNH; tw.C1GPH = tm.C1GPH; tw.C1GPNH = tm.C1GPNH; tw.TWOAWH = tm.TWOAWH; tw.TWOAWNH = tm.TWOAWNH; tw.TWOAPDH = tm.TWOAPDH; tw.TWOAPDNH = tm.TWOAPDNH;
                                tw.TWOAEXSH = tm.TWOAEXSH; tw.TWOAEXSNH = tm.TWOAEXSNH; tw.TWOAKMH = tm.TWOAKMH; tw.TWOAKMNH = tm.TWOAKMNH; tw.TWOAEWSH = tm.TWOAEWSH; tw.TWOAEWSNH = tm.TWOAEWSNH;
                                tw.TWOAGPH = tm.TWOAGPH; tw.TWOAGPNH = tm.TWOAGPNH; tw.TWOBWH = tm.TWOBWH; tw.TWOBWNH = tm.TWOBWNH; tw.TWOBPDH = tm.TWOBPDH; tw.TWOBPDNH = tm.TWOBPDNH;
                                tw.TWOBEXSH = tm.TWOBEXSH; tw.TWOBEXSNH = tm.TWOBEXSNH; tw.TWOBKMH = tm.TWOBKMH; tw.TWOBKMNH = tm.TWOBKMNH; tw.TWOBEWSH = tm.TWOBEWSH; tw.TWOBEWSNH = tm.TWOBEWSNH;
                                tw.TWOBGPH = tm.TWOBGPH; tw.TWOBGPNH = tm.TWOBGPNH; tw.THREEAWH = tm.THREEAWH; tw.THREEAWNH = tm.THREEAWNH; tw.THREEAPDH = tm.THREEAPDH; tw.THREEAPDNH = tm.THREEAPDNH;
                                tw.THREEAEXSH = tm.THREEAEXSH; tw.THREEAEXSNH = tm.THREEAEXSNH; tw.THREEAKMH = tm.THREEAKMH; tw.THREEAKMNH = tm.THREEAKMNH; tw.THREEAEWSH = tm.THREEAEWSH;
                                tw.THREEAEWSNH = tm.THREEAEWSNH; tw.THREEAGPH = tm.THREEAGPH; tw.THREEAGPNH = tm.THREEAGPNH; tw.THREEBWH = tm.THREEBWH; tw.THREEBWNH = tm.THREEBWNH;
                                tw.THREEBPDH = tm.THREEBPDH; tw.THREEBPDNH = tm.THREEBPDNH; tw.THREEBEXSH = tm.THREEBEXSH; tw.THREEBEXSNH = tm.THREEBEXSNH; tw.THREEBKMH = tm.THREEBKMH;
                                tw.THREEBKMNH = tm.THREEBKMNH; tw.THREEBEWSH = tm.THREEBEWSH; tw.THREEBEWSNH = tm.THREEBEWSNH; tw.THREEBGPH = tm.THREEBGPH; tw.THREEBGPNH = tm.THREEBGPNH;

                                instituteName.Trades.Add(tw);
                                ttl_seats += (int)tw.Seats;
                            }
                        }
                        else if (round == 6)
                        {
                            foreach (var tm in tradeRes)
                            {
                                Tradewideseat tw = new Tradewideseat();
                                var tdname = _db.tbl_trade_mast.Where(x => x.trade_id == tm.TradeId).Select(y => y.trade_name).FirstOrDefault();
                                tw.TradeName = tdname;
                                tw.Seats = (int)tm.GMGP;
                                tw.GMGP = tm.GMGP;
                                instituteName.Trades.Add(tw);
                                ttl_seats += (int)tw.Seats;
                            }
                        }
                        else
                        {
                            foreach (var tm in tradeRes)
                            {
                                Tradewideseat tw = new Tradewideseat();
                                var tdname = _db.tbl_trade_mast.Where(x => x.trade_id == tm.TradeId).Select(y => y.trade_name).FirstOrDefault();
                                tw.TradeName = tdname;
                                tw.Seats = (int)(tm.GMGP + tm.SCGP + tm.STGP + tm.C1GP + tm.TWOAGP + tm.TWOBGP + tm.THREEAGP + tm.THREEBGP);
                                tw.GMGP = tm.GMGP;
                                tw.SCGP = tm.SCGP;
                                tw.STGP = tm.STGP;
                                tw.C1GP = tm.C1GP;
                                tw.TWOAGP = tm.TWOAGP;
                                tw.TWOBGP = tm.TWOBGP;
                                tw.THREEAGP = tm.THREEAGP;
                                tw.THREEBGP = tm.THREEBGP;
                                instituteName.Trades.Add(tw);
                                ttl_seats += (int)tw.Seats;
                            }
                        }

                    }
                    instituteName.TotalSeats = ttl_seats;
                    somer.Add(instituteName);
                }

                foreach (var mtn in somer)
                {
                    foreach (var ds in res)
                    {
                        if (mtn.DivisionId == ds.DivisionId)
                        {
                            ds.Institutes.Add(mtn);
                        }
                    }

                }
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateSeatMatrixCollegeWise(List<Tradewideseat> listItem, int collegeId, int round, int year, int appType, int courseType, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var sm = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == collegeId &&
                    x.Round == round && x.AcademicYear == year && x.CourseTypeId == courseType).FirstOrDefault();

                    sm.Status = 5;
                    sm.Remarks = "resubmitted";

                    //var setmxId = _db.tbl_SeatMatrix_Main.OrderByDescending(x => x.SeatMaxId).
                    //    Select(y => y.SeatMaxId).FirstOrDefault();
                    //maintain history
                    tbl_SeatMatrix_Trans st = new tbl_SeatMatrix_Trans();
                    st.SeatMaxId = sm.SeatMaxId;
                    st.AcademicYear = year;
                    st.InstituteId = collegeId;
                    st.ApplicantType = appType;
                    st.Round = round;
                    st.Status = 5;
                    st.Remarks = "submitted for update";
                    st.IsActive = true;
                    st.CreatedOn = DateTime.Now;
                    st.CourseTypeId = courseType;
                    _db.tbl_SeatMatrix_Trans.Add(st);

                    //if (round == 1 || round == 2)
                    //{
                    foreach (var tm in listItem)
                    {
                        //tbl_SeatMatrix_TradeWise tw = new tbl_SeatMatrix_TradeWise();
                        tbl_SeatMatrix_TradeWise tw = _db.tbl_SeatMatrix_TradeWise.
                        Where(x => x.SeatMaxId == sm.SeatMaxId && x.TradeId == tm.TradeId).FirstOrDefault();
                        //tw.SeatMaxId = setmxId; 
                        //tw.TradeId = tm.TradeId;
                        if (tw != null)
                        {
                            if (round == 1 || round == 2)
                            {
                                tw.IsActive = true; tw.CreatedBy = loginId; tw.CreatedOn = DateTime.Now;
                                tw.GMWH = tm.GMWH; tw.GMWNH = tm.GMWNH; tw.GMPDH = tm.GMPDH; tw.GMPDNH = tm.GMPDNH; tw.GMEXSH = tm.GMEXSH; tw.GMEXSNH = tm.GMEXSNH;
                                tw.GMKMH = tm.GMKMH; tw.GMKMNH = tm.GMKMNH; tw.GMEWSH = tm.GMEWSH; tw.GMEWSNH = tm.GMEWSNH; tw.GMGPH = tm.GMGPH; tw.GMGPNH = tm.GMGPNH; tw.SCWH = tm.SCWH; tw.SCWNH = tm.SCWNH;
                                tw.SCPDH = tm.SCPDH; tw.SCPDNH = tm.SCPDNH; tw.SCEXSH = tm.SCEXSH; tw.SCEXSNH = tm.SCEXSNH; tw.SCKMH = tm.SCKMH; tw.SCKMNH = tm.SCKMNH; tw.SCEWSH = tm.SCEWSH;
                                tw.SCEWSNH = tm.SCEWSNH; tw.SCGPH = tm.SCGPH; tw.SCGPNH = tm.SCGPNH; tw.STWH = tm.STWH; tw.STWNH = tm.STWNH; tw.STPDH = tw.STPDH; tw.STPDNH = tm.STPDNH; tw.STEXSH = tm.STEXSH;
                                tw.STEXSNH = tm.STEXSNH; tw.STKMH = tm.STKMH; tw.STKMNH = tm.STKMNH; tw.STEWSH = tm.STEWSH; tw.STEWSNH = tm.STEWSNH; tw.STGPH = tm.STGPH; tw.STGPNH = tm.STGPNH; tw.C1WH = tm.C1WH;
                                tw.C1WNH = tm.C1WNH; tw.C1PDH = tm.C1PDH; tw.C1PDNH = tm.C1PDNH; tw.C1EXSH = tm.C1EXSH; tw.C1EXSNH = tm.C1EXSNH; tw.C1KMH = tm.C1KMH; tw.C1KMNH = tm.C1KMNH; tw.C1EWSH = tm.C1EWSH;
                                tw.C1EWSNH = tm.C1EWSNH; tw.C1GPH = tm.C1GPH; tw.C1GPNH = tm.C1GPNH; tw.TWOAWH = tm.TWOAWH; tw.TWOAWNH = tm.TWOAWNH; tw.TWOAPDH = tm.TWOAPDH; tw.TWOAPDNH = tm.TWOAPDNH;
                                tw.TWOAEXSH = tm.TWOAEXSH; tw.TWOAEXSNH = tm.TWOAEXSNH; tw.TWOAKMH = tm.TWOAKMH; tw.TWOAKMNH = tm.TWOAKMNH; tw.TWOAEWSH = tm.TWOAEWSH; tw.TWOAEWSNH = tm.TWOAEWSNH;
                                tw.TWOAGPH = tm.TWOAGPH; tw.TWOAGPNH = tm.TWOAGPNH; tw.TWOBWH = tm.TWOBWH; tw.TWOBWNH = tm.TWOBWNH; tw.TWOBPDH = tm.TWOBPDH; tw.TWOBPDNH = tm.TWOBPDNH;
                                tw.TWOBEXSH = tm.TWOBEXSH; tw.TWOBEXSNH = tm.TWOBEXSNH; tw.TWOBKMH = tm.TWOBKMH; tw.TWOBKMNH = tm.TWOBKMNH; tw.TWOBEWSH = tm.TWOBEWSH; tw.TWOBEWSNH = tm.TWOBEWSNH;
                                tw.TWOBGPH = tm.TWOBGPH; tw.TWOBGPNH = tm.TWOBGPNH; tw.THREEAWH = tm.THREEAWH; tw.THREEAWNH = tm.THREEAWNH; tw.THREEAPDH = tm.THREEAPDH; tw.THREEAPDNH = tm.THREEAPDNH;
                                tw.THREEAEXSH = tm.THREEAEXSH; tw.THREEAEXSNH = tm.THREEAEXSNH; tw.THREEAKMH = tm.THREEAKMH; tw.THREEAKMNH = tm.THREEAKMNH; tw.THREEAEWSH = tm.THREEAEWSH;
                                tw.THREEAEWSNH = tm.THREEAEWSNH; tw.THREEAGPH = tm.THREEAGPH; tw.THREEAGPNH = tm.THREEAGPNH; tw.THREEBWH = tm.THREEBWH; tw.THREEBWNH = tw.THREEBWNH;
                                tw.THREEBPDH = tm.THREEBPDH; tw.THREEBPDNH = tm.THREEBPDNH; tw.THREEBEXSH = tm.THREEBEXSH; tw.THREEBEXSNH = tm.THREEBEXSNH; tw.THREEBKMH = tm.THREEBKMH;
                                tw.THREEBKMNH = tm.THREEBKMNH; tw.THREEBEWSH = tm.THREEBEWSH; tw.THREEBEWSNH = tm.THREEBEWSNH; tw.THREEBGPH = tm.THREEBGPH; tw.THREEBGPNH = tm.THREEBGPNH;

                            }
                            else if (round == 6)
                            {
                                tw.IsActive = true;
                                tw.CreatedBy = loginId;
                                tw.CreatedOn = DateTime.Now;
                                tw.GMGP = tm.GMGP;
                            }
                            else
                            {
                                tw.IsActive = true;
                                tw.CreatedBy = loginId;
                                tw.CreatedOn = DateTime.Now;
                                tw.GMGP = tm.GMGP;
                                tw.SCGP = tm.SCGP;
                                tw.STGP = tm.STGP;
                                tw.C1GP = tm.C1GP;
                                tw.TWOAGP = tm.TWOAGP;
                                tw.TWOBGP = tm.TWOBGP;
                                tw.THREEAGP = tm.THREEAGP;
                                tw.THREEBGP = tm.THREEBGP;
                            }
                        }
                    }
                    //}
                    //else if (round == 6)
                    //{
                    //    foreach (var tm in listItem)
                    //    {
                    //        tbl_SeatMatrix_TradeWise tw = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == sm.SeatMaxId).FirstOrDefault();
                    //        //tw.SeatMaxId = setmxId;
                    //        if (tw != null)
                    //        {
                    //            tw.IsActive = true;
                    //            tw.CreatedBy = loginId;
                    //            tw.CreatedOn = DateTime.Now;
                    //            tw.GMGP = tm.GMGP;
                    //        }
                    //    }

                    //}
                    //else
                    //{
                    //    foreach (var tm in listItem)
                    //    {
                    //        tbl_SeatMatrix_TradeWise tw = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == sm.SeatMaxId).FirstOrDefault();
                    //        if (tw != null)
                    //        {
                    //            tw.IsActive = true;
                    //            tw.CreatedBy = loginId;
                    //            tw.CreatedOn = DateTime.Now;
                    //            tw.GMGP = tm.GMGP;
                    //            tw.SCGP = tm.SCGP;
                    //            tw.STGP = tm.STGP;
                    //            tw.C1GP = tm.C1GP;
                    //            tw.TWOAGP = tm.TWOAGP;
                    //            tw.TWOBGP = tm.TWOBGP;
                    //            tw.THREEAGP = tm.THREEAGP;
                    //            tw.THREEBGP = tm.THREEBGP;
                    //        }
                    //    }
                    //}

                    _db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }

        }
        public bool UpdateSeatMatrix(int year, int appType, int courseType, int round, int role, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var institute = _db.tbl_SeatMatrix_Main.Where(x => x.AcademicYear == year && x.CourseTypeId == courseType && x.Round == round).ToList();
                    foreach (var ins in institute)
                    {
                        ins.Status = 5;
                        ins.Remarks = "Submitted";
                        ins.FlowId = role;
                    }
                    foreach (var tran in institute)
                    {
                        tran.Status = 5;
                        tran.FlowId = role;
                        tran.Remarks = "Submitted";

                    }

                    _db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }

            }
        }
        #endregion

        #region Review seat matrix by dhanraj
        public bool ForwardSendBackApproveSeatMatrix(int round, int year, string remarks, int courseType, int Status, int loginId, int roleId, int loginRole)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var institute = _db.tbl_SeatMatrix_Main.Where(x => x.AcademicYear == year && x.CourseTypeId == courseType && x.Round == round).ToList();
                    foreach (var ins in institute)
                    {
                        ins.Status = Status;
                        ins.Remarks = remarks;
                        if (Status == 2)//if approve
                            ins.FlowId = 5;//deputy director
                        else
                            ins.FlowId = roleId;

                        tbl_SeatMatrix_Trans st = new tbl_SeatMatrix_Trans();
                        st.SeatMaxId = ins.SeatMaxId;
                        st.AcademicYear = year;
                        st.InstituteId = ins.InstituteId;
                        if (round == 5 || round == 6)
                            st.ApplicantType = 2;
                        else
                            st.ApplicantType = 1;
                        st.Round = round;
                        st.Status = Status;
                        st.Remarks = remarks;
                        st.IsActive = true;
                        if (Status == 2)//if approve
                            st.FlowId = 5;//deputy director
                        else
                            st.FlowId = roleId;
                        st.CreatedOn = DateTime.Now;
                        st.CourseTypeId = courseType;
                        st.CreatedBy = loginRole;
                        _db.tbl_SeatMatrix_Trans.Add(st);
                    }

                    _db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }

            }
        }
        public List<RemarkSeat> GetSeatmatrixRemarks(int year, int round, int applId, int courseType)
        {
            try
            {
                var som =(from u in _db.tbl_SeatMatrix_Trans
                           where u.AcademicYear == year && u.Round == round && u.ApplicantType == applId && u.CourseTypeId == courseType                          
                           select new SeatMatrix
                           {
                               Status= u.Status,
                               CreatedBy=u.CreatedBy,
                               FlowId=u.FlowId,
                               AcademicYear= u.AcademicYear,
                               Round=u.Round,
                               ApplicantType=u.ApplicantType,
                               CourseTypeId=u.CourseTypeId,
                               Remarks=u.Remarks                               
                           }).Distinct().ToList();

                foreach (var itm in som)
                {
                    itm.CreatedOn = _db.tbl_SeatMatrix_Trans.Where(u => u.AcademicYear == year && u.Round == round && u.ApplicantType == applId && u.CourseTypeId == courseType)
                           .Select(y => y.CreatedOn).FirstOrDefault();                   
                }
                
                           
                var res = (from aa in som
                           join bb in _db.tbl_status_master on aa.Status equals bb.StatusId
                           join cc in _db.tbl_role_master on aa.CreatedBy equals cc.role_id
                           join dd in _db.tbl_role_master on aa.FlowId equals dd.role_id
                           where aa.AcademicYear == year && aa.Round == round && aa.ApplicantType == applId && aa.CourseTypeId == courseType
                           select new RemarkSeat
                           {
                               //Slno = aa.SeatMaxTransId,
                               StatusName = bb.StatusName,
                               Remarks = aa.Remarks,
                               From = cc.role_description,
                               Date = aa.CreatedOn.ToString(),
                               FlowId = aa.FlowId                               
                           }).Distinct().ToList();

                var rss = (from a in res
                           join b in _db.tbl_role_master on a.FlowId equals b.role_id
                           select new RemarkSeat
                           {
                               StatusName = a.StatusName,
                               Remarks = a.Remarks,
                               From = a.From,
                               Date = a.Date,
                               To = b.role_description
                           }).ToList();

                return rss;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetSeatmatrixNotification()
        {
            try
            {
                DateTime dt = DateTime.Now;
                string year = dt.Year.ToString();
                var yr = _db.tbl_Year.Where(x => x.Year == year).Select(y => y.YearID).FirstOrDefault();
                var res = _db.tbl_SeatMatrix_Main.Where(x => x.AcademicYear == yr && x.Status == 2).OrderByDescending(y => y.SeatMaxId).Select(z => z.Round).FirstOrDefault();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<seatmatrixmodel> GetStatusDLL(int id)
        {
            try
            {
                List<seatmatrixmodel> res = null;
                if (id == 5)
                {
                    res = (from a in _db.tbl_status_master
                           where a.StatusId == 8
                           select new seatmatrixmodel
                           {
                               StatusId = a.StatusId,
                               StatusName = a.StatusName
                           }).ToList();
                }
                else if (id == 6 || id == 4)
                {
                    res = (from a in _db.tbl_status_master
                           where (a.StatusId == 7 || a.StatusId == 9)
                           select new seatmatrixmodel
                           {
                               StatusId = a.StatusId,
                               StatusName = a.StatusName
                           }).ToList();
                }
                else if (id == 2)
                {
                    res = (from a in _db.tbl_status_master
                           where (a.StatusId == 2 || a.StatusId == 7 || a.StatusId == 9)
                           select new seatmatrixmodel
                           {
                               StatusId = a.StatusId,
                               StatusName = a.StatusName
                           }).ToList();
                }
                else if (id == 1)
                {
                    res = (from a in _db.tbl_status_master
                           where (a.StatusId == 2 || a.StatusId == 9)
                           select new seatmatrixmodel
                           {
                               StatusId = a.StatusId,
                               StatusName = a.StatusName
                           }).ToList();
                }
                else
                {
                    res = (from a in _db.tbl_status_master

                           select new seatmatrixmodel
                           {
                               StatusId = a.StatusId,
                               StatusName = a.StatusName
                           }).ToList();
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public List<seatmatrixmodel> GetGenSeatMatrixDLL(int ApplicantTypeGen, int AcademicYearGen, int RoundGen, int roleid)
        {
            try
            {
                var res = (from aa in _db.tbl_ITI_trade_seat_master
                           join bb in _db.tbl_ITI_Trade_Shifts on aa.Trade_ITI_Id equals bb.ITI_Trade_Id
                           join cc in _db.tbl_ITI_Trade on bb.ITI_Trade_Id equals cc.Trade_ITI_id
                           join dd in _db.tbl_trade_mast on cc.TradeCode equals dd.trade_id
                           join ee in _db.tbl_iti_college_details on cc.ITICode equals ee.iti_college_id
                           join gg in _db.tbl_location_type on ee.location_id equals gg.location_id
                           join hh in _db.tbl_district_master on ee.district_id equals hh.district_lgd_code
                           join ii in _db.tbl_taluk_master on ee.district_id equals ii.district_lgd_code
                           join ff in _db.tbl_Institute_type on ee.Insitute_TypeId equals ff.Institute_type_id
                           join jj in _db.tbl_course_type_mast on aa.CourseType equals jj.course_id
                           join kk in _db.tbl_SeatAvail_status_master on aa.Status equals kk.StatusId
                           join cd in _db.tbl_seat_type on aa.SeatsTypeId equals cd.Seat_type_id
                           //where ee.taluk_id == ii.taluk_lgd_code && ee.iti_college_id == collegeID && aa.CourseType == courseType
                           select new seatmatrixmodel
                           {
                               Id = aa.Trade_ITI_seat_Id,
                               MISCode = ee.MISCode,
                               division_id = ee.location_id,
                               division_name = gg.location_name,
                               district_id = ee.district_id,
                               district_ename = hh.district_ename,
                               taluk_id = ee.taluk_id,
                               taluk_ename = ii.taluk_ename,
                               Insitute_TypeId = ee.Insitute_TypeId,
                               iti_college_id = ee.iti_college_id,
                               iti_college_name = ee.iti_college_name,
                               TradeId = dd.trade_id,
                               TradeName = dd.trade_name,
                               ShiftId = aa.ShiftId,
                               Unit = aa.UnitId,
                               DualSystemTraining = aa.DualSystemTraining,
                               Govt_Gia_seats = aa.Govt_Gia_seats,
                               Management_seats = aa.Management_seats,
                               StatusName = kk.StatusName,
                               StatusId = aa.Status,
                               Remarks = aa.Remarks,
                               RoleId = roleid,
                               FlowId = aa.FlowId,
                               AcademicYeardate = aa.AcademicYear,
                               CourseTypeName = jj.course_type_name,
                               SeatName = cd.SeatType,
                               Duration = dd.trade_duration
                           }).Distinct().ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Tradewideseat> GetGenerateSeatMatrixDLLNested(int round, int instiId, int year, int tradeId)
        {
            try
            {
                List<Tradewideseat> res = new List<Tradewideseat>();
                Tradewideseat tw = new Tradewideseat();
                var setmxId = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == instiId).Select(x => x.SeatMaxId).FirstOrDefault();
                var tm = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == setmxId && x.TradeId == tradeId).FirstOrDefault();
                var itiTradecode = _db.tbl_ITI_Trade.Where(x => x.ITICode == instiId && x.TradeCode == tradeId).Select(y => y.Trade_ITI_id).FirstOrDefault();
                int yer = Convert.ToInt32(_db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault().Split('-')[1]);
                tw.Units = _db.tbl_ITI_trade_seat_master.Where(x => x.Status == 4 && x.AcademicYear.Year == yer && x.Trade_ITI_Id == itiTradecode).Select(a => a.UnitId).Count();
                if (round == 1 || round == 2)
                {
                    var tname = _db.tbl_trade_mast.Where(x => x.trade_id == tm.TradeId).Select(y => y.trade_name).FirstOrDefault();
                    tw.TradeName = tname;
                    tw.Seats = (int)(tm.GMWH + tm.GMWNH + tm.GMPDH + tm.GMPDNH + tm.GMEXSH + tm.GMEXSNH + tm.GMKMH + tm.GMKMNH + tm.GMEWSH + tm.GMEWSNH + tm.GMGPH + tm.GMGPNH + tm.SCWH + tm.SCWNH +
                    tm.SCPDH + tm.SCPDNH + tm.SCEXSH + tm.SCEXSNH + tm.SCKMH + tm.SCKMNH + tm.SCEWSH + tm.SCEWSNH + tm.SCGPH + tm.SCGPNH + tm.STWH + tm.STWNH + tm.STPDH + tm.STPDNH + tm.STEXSH +
                    tm.STEXSNH + tm.STKMH + tm.STKMNH + tm.STEWSH + tm.STEWSNH + tm.STGPH + tm.STGPNH + tm.C1WH + tm.C1WNH + tm.C1PDH + tm.C1PDNH + tm.C1EXSH + tm.C1EXSNH + tm.C1KMH +
                    tm.C1KMNH + tm.C1EWSH + tm.C1EWSNH + tm.C1GPH + tm.C1GPNH + tm.TWOAWH + tm.TWOAWNH + tm.TWOAPDH + tm.TWOAPDNH + tm.TWOAEXSH + tm.TWOAEXSNH + tm.TWOAKMH + tm.TWOAKMNH +
                    tm.TWOAEWSH + tm.TWOAEWSNH + tm.TWOAGPH + tm.TWOAGPNH + tm.TWOBWH + tm.TWOBWNH + tm.TWOBPDH + tm.TWOBPDNH + tm.TWOBEXSH + tm.TWOBEXSNH + tm.TWOBKMH + tm.TWOBKMNH + tm.TWOBEWSH +
                    tm.TWOBEWSNH + tm.TWOBGPH + tm.TWOBGPNH + tm.THREEAWH + tm.THREEAWNH + tm.THREEAPDH + tm.THREEAPDNH + tm.THREEAEXSH + tm.THREEAEXSNH + tm.THREEAKMH + tm.THREEAKMNH + tm.THREEAEWSH +
                    tm.THREEAEWSNH + tm.THREEAGPH + tm.THREEAGPNH + tm.THREEBWH + tm.THREEBWNH + tm.THREEBPDH + tm.THREEBPDNH + tm.THREEBEXSH + tm.THREEBEXSNH + tm.THREEBKMH +
                    tm.THREEBKMNH + tm.THREEBEWSH + tm.THREEBEWSNH + tm.THREEBGPH + tm.THREEBGPNH);

                    tw.TradeId = tm.TradeId; tw.GMWH = tm.GMWH; tw.GMWNH = tm.GMWNH; tw.GMPDH = tm.GMPDH; tw.GMPDNH = tm.GMPDNH; tw.GMEXSH = tm.GMEXSH; tw.GMEXSNH = tm.GMEXSNH;
                    tw.GMKMH = tm.GMKMH; tw.GMKMNH = tm.GMKMNH; tw.GMEWSH = tm.GMEWSH; tw.GMEWSNH = tm.GMEWSNH; tw.GMGPH = tm.GMGPH; tw.GMGPNH = tm.GMGPNH; tw.SCWH = tm.SCWH; tw.SCWNH = tm.SCWNH;
                    tw.SCPDH = tm.SCPDH; tw.SCPDNH = tm.SCPDNH; tw.SCEXSH = tm.SCEXSH; tw.SCEXSNH = tm.SCEXSNH; tw.SCKMH = tm.SCKMH; tw.SCKMNH = tm.SCKMNH; tw.SCEWSH = tm.SCEWSH;
                    tw.SCEWSNH = tm.SCEWSNH; tw.SCGPH = tm.SCGPH; tw.SCGPNH = tm.SCGPNH; tw.STWH = tm.STWH; tw.STWNH = tm.STWNH; tw.STPDH = tm.STPDH; tw.STPDNH = tm.STPDNH; tw.STEXSH = tm.STEXSH;
                    tw.STEXSNH = tm.STEXSNH; tw.STKMH = tm.STKMH; tw.STKMNH = tm.STKMNH; tw.STEWSH = tm.STEWSH; tw.STEWSNH = tm.STEWSNH; tw.STGPH = tm.STGPH; tw.STGPNH = tm.STGPNH; tw.C1WH = tm.C1WH;
                    tw.C1WNH = tm.C1WNH; tw.C1PDH = tm.C1PDH; tw.C1PDNH = tm.C1PDNH; tw.C1EXSH = tm.C1EXSH; tw.C1EXSNH = tm.C1EXSNH; tw.C1KMH = tm.C1KMH; tw.C1KMNH = tm.C1KMNH; tw.C1EWSH = tm.C1EWSH;
                    tw.C1EWSNH = tm.C1EWSNH; tw.C1GPH = tm.C1GPH; tw.C1GPNH = tm.C1GPNH; tw.TWOAWH = tm.TWOAWH; tw.TWOAWNH = tm.TWOAWNH; tw.TWOAPDH = tm.TWOAPDH; tw.TWOAPDNH = tm.TWOAPDNH;
                    tw.TWOAEXSH = tm.TWOAEXSH; tw.TWOAEXSNH = tm.TWOAEXSNH; tw.TWOAKMH = tm.TWOAKMH; tw.TWOAKMNH = tm.TWOAKMNH; tw.TWOAEWSH = tm.TWOAEWSH; tw.TWOAEWSNH = tm.TWOAEWSNH;
                    tw.TWOAGPH = tm.TWOAGPH; tw.TWOAGPNH = tm.TWOAGPNH; tw.TWOBWH = tm.TWOBWH; tw.TWOBWNH = tm.TWOBWNH; tw.TWOBPDH = tm.TWOBPDH; tw.TWOBPDNH = tm.TWOBPDNH;
                    tw.TWOBEXSH = tm.TWOBEXSH; tw.TWOBEXSNH = tm.TWOBEXSNH; tw.TWOBKMH = tm.TWOBKMH; tw.TWOBKMNH = tm.TWOBKMNH; tw.TWOBEWSH = tm.TWOBEWSH; tw.TWOBEWSNH = tm.TWOBEWSNH;
                    tw.TWOBGPH = tm.TWOBGPH; tw.TWOBGPNH = tm.TWOBGPNH; tw.THREEAWH = tm.THREEAWH; tw.THREEAWNH = tm.THREEAWNH; tw.THREEAPDH = tm.THREEAPDH; tw.THREEAPDNH = tm.THREEAPDNH;
                    tw.THREEAEXSH = tm.THREEAEXSH; tw.THREEAEXSNH = tm.THREEAEXSNH; tw.THREEAKMH = tm.THREEAKMH; tw.THREEAKMNH = tm.THREEAKMNH; tw.THREEAEWSH = tm.THREEAEWSH;
                    tw.THREEAEWSNH = tm.THREEAEWSNH; tw.THREEAGPH = tm.THREEAGPH; tw.THREEAGPNH = tm.THREEAGPNH; tw.THREEBWH = tm.THREEBWH; tw.THREEBWNH = tm.THREEBWNH;
                    tw.THREEBPDH = tm.THREEBPDH; tw.THREEBPDNH = tm.THREEBPDNH; tw.THREEBEXSH = tm.THREEBEXSH; tw.THREEBEXSNH = tm.THREEBEXSNH; tw.THREEBKMH = tm.THREEBKMH;
                    tw.THREEBKMNH = tm.THREEBKMNH; tw.THREEBEWSH = tm.THREEBEWSH; tw.THREEBEWSNH = tm.THREEBEWSNH; tw.THREEBGPH = tm.THREEBGPH; tw.THREEBGPNH = tm.THREEBGPNH;

                }
                else if (round == 6)
                {
                    tw.GMGP = tm.GMGP;
                    tw.Seats = (int)tm.GMGP;
                }
                else
                {

                    tw.GMGP = tm.GMGP;
                    tw.SCGP = tm.SCGP;
                    tw.STGP = tm.STGP;
                    tw.C1GP = tm.C1GP;
                    tw.TWOAGP = tm.TWOAGP;
                    tw.TWOBGP = tm.TWOBGP;
                    tw.THREEAGP = tm.THREEAGP;
                    tw.THREEBGP = tm.THREEBGP;
                    tw.Seats = (int)(tm.GMGP + tm.SCGP + tm.STGP + tm.C1GP + tm.TWOAGP + tm.TWOBGP + tm.THREEAGP + tm.THREEBGP);
                }
                res.Add(tw);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<seatmatrixmodelNested> GetGenerateSeatMatrixDLLNested(int CollegeID)
        {
            try
            {
                List<seatmatrixmodelNested> res = null;
                //                select icd.iti_college_name,tm.trade_name From tbl_iti_college_details icd join tbl_ITI_Trade it on icd.iti_college_id = it.ITICode
                //join tbl_trade_mast tm on tm.trade_id = it.TradeCode
                res = (from icd in _db.tbl_iti_college_details
                       join it in _db.tbl_ITI_Trade on icd.iti_college_id equals it.ITICode
                       join tm in _db.tbl_trade_mast on it.TradeCode equals tm.trade_id
                       where (icd.iti_college_id == CollegeID)
                       select new seatmatrixmodelNested
                       {
                           iti_college_name = icd.iti_college_name,
                           TradeName = tm.trade_name,
                           iti_college_id = icd.iti_college_id,
                       }).Distinct().ToList();

                return res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string InsertSeatMatrixDLL(seatmatrixmodelNested s)
        {
            tbl_SeatMatrix_TradeWise smt = new tbl_SeatMatrix_TradeWise();
            smt.SeatMaxTradeId = 1;
            smt.SeatMaxId = 1;
            smt.GMWH = s.GMWomenH;
            smt.GMWNH = s.GMWomenNH;
            smt.GMPDH = s.GMPwdH;
            smt.GMPDNH = s.GMPwdNH;
            smt.GMEXSH = s.GMExSH;
            smt.GMEXSNH = s.GMExSNH;
            smt.GMKMH = s.GMkmH;
            smt.GMKMNH = s.GMkmNH;
            smt.GMEWSH = s.GMEwsH;
            smt.GMEWSNH = s.GMEwsNH;
            smt.GMGPH = s.GMgpH;
            smt.GMGPNH = s.GMgpNH;
            smt.SCWH = s.SCWomenH;
            smt.SCWNH = s.SCWomenNH;
            smt.SCPDH = s.SCPwdH;
            smt.SCPDNH = s.SCPwdNH;
            smt.SCEXSH = s.SCExSH;
            smt.SCEXSNH = s.SCExSNH;
            smt.SCKMH = s.SCkmH;
            smt.SCKMNH = s.SCkmNH;
            smt.SCEWSH = s.SCEwsH;
            smt.SCEWSNH = s.SCEwsNH;
            smt.SCGPH = s.SCgpH;
            smt.SCGPNH = s.SCgpNH;
            smt.STWH = s.STWomenH;
            smt.STWNH = s.STWomenNH;
            smt.STPDH = s.STPwdH;
            smt.STPDNH = s.STPwdNH;
            smt.STEXSH = s.STExSH;
            smt.STEXSNH = s.STExSNH;
            smt.STKMH = s.STkmH;
            smt.STKMNH = s.STkmNH;
            smt.STEWSH = s.STEwsH;
            smt.STEWSNH = s.STEwsNH;
            smt.STGPH = s.STgpH;
            smt.STGPNH = s.STgpNH;
            smt.C1WH = s.C1WomenH;
            smt.C1WNH = s.C1WomenNH;
            smt.C1PDH = s.C1PwdH;
            smt.C1PDNH = s.C1PwdNH;
            smt.C1EXSH = s.C1ExSH;
            smt.C1EXSNH = s.C1ExSNH;
            smt.C1KMH = s.C1kmH;
            smt.C1KMNH = s.C1KasMNH;
            smt.C1EWSH = s.C1EwsH;
            smt.C1EWSNH = s.C1EwsNH;
            smt.C1GPH = s.C1gpH;
            smt.C1GPNH = s.C1gpNH;
            smt.TWOAWH = s.IIAWomenH;
            smt.TWOAWNH = s.IIAWomenNH;
            smt.TWOAPDH = s.IIAPwdH;
            smt.TWOAPDNH = s.IIAPwdNH;
            smt.TWOAEXSH = s.IIAExSH;
            smt.TWOAEXSNH = s.IIAExSNH;
            smt.TWOAKMH = s.IIAkmH;
            smt.TWOAKMNH = s.IIAkmNH;
            smt.TWOAEWSH = s.IIAEwsH;
            smt.TWOAEWSNH = s.IIAEwsNH;
            smt.TWOAGPH = s.IIAgpH;
            smt.TWOAGPNH = s.IIAgpNH;
            smt.TWOBWH = s.IIBWomenH;
            smt.TWOBWNH = s.IIBWomenNH;
            smt.TWOBPDH = s.IIBPwdH;
            smt.TWOBPDNH = s.IIBPwdNH;
            smt.TWOBEXSH = s.IIBExSH;
            smt.TWOBEXSNH = s.IIBExSNH;
            smt.TWOBKMH = s.IIBkmH;
            smt.TWOBKMNH = s.IIBkmNH;
            smt.TWOBEWSH = s.IIBEwsH;
            smt.TWOBEWSNH = s.IIBEwsNH;
            smt.TWOBGPH = s.IIBgpH;
            smt.TWOBGPNH = s.IIBgpNH;

            smt.THREEAWH = s.IIIAWomenH;
            smt.THREEAWNH = s.IIIAWomenNH;
            smt.THREEAPDH = s.IIIAPwdH;
            smt.THREEAPDNH = s.IIIAPwdNH;
            smt.THREEAEXSH = s.IIIAExSH;
            smt.THREEAEXSNH = s.IIIAExSNH;
            smt.THREEAKMH = s.IIIAkmH;
            smt.THREEAKMNH = s.IIIAkmNH;
            smt.THREEAEWSH = s.IIIAEwsH;
            smt.THREEAEWSNH = s.IIIAEwsNH;
            smt.THREEAGPH = s.IIIAgpH;
            smt.THREEAGPNH = s.IIIAgpNH;
            smt.THREEBWH = s.IIIBWomenH;
            smt.THREEBWNH = s.IIIBWomenNH;
            smt.THREEBPDH = s.IIIBPwdH;
            smt.THREEBPDNH = s.IIIBPwdNH;
            smt.THREEBEXSH = s.IIIBExSH;
            smt.THREEBEXSNH = s.IIIBExSNH;
            smt.THREEBKMH = s.IIIBkmH;
            smt.THREEBKMNH = s.IIIBkmNH;
            smt.THREEBEWSH = s.IIIBEwsH;
            smt.THREEBEWSNH = s.IIIBEwsNH;
            smt.THREEBGPH = s.IIIBgpH;
            smt.THREEBGPNH = s.IIBgpNH;

            _db.tbl_SeatMatrix_TradeWise.Add(smt);
            _db.SaveChanges();
            return "Success";
        }
        #endregion

        #region ..Seat Matrix..Added by Sujit
        public List<seatmatrixmodel> GetInstituteTypeDll()
        {
            var res = (from a in _db.tbl_Institute_type
                       select new seatmatrixmodel
                       {
                           Institute_typeId = a.Institute_type_id,
                           InstituteType = a.Institute_type
                       }).ToList();
            return res;
        }
        public List<SelectListItem> GetTalukDLL(int DistId)
        {
            var res = (from a in _db.tbl_taluk_master.Where(a => a.district_lgd_code == DistId && a.taluk_is_active == true)

                       select new SelectListItem
                       {
                           Text = a.taluk_ename,
                           Value = a.taluk_lgd_code.ToString()

                       }).ToList();
            return res;
        }

        public List<seatmatrixmodel> GetviewSeatmatrixDLL(int ApplicantTypeId, int AcademicYear, int Institute, int id)
        {
            try
            {
                List<seatmatrixmodel> res = null;
                res = (from a in _db.tbl_ITI_trade_seat_master
                       join b in _db.tbl_ITI_Trade on a.Trade_ITI_Id equals b.Trade_ITI_id
                       join d in _db.tbl_iti_college_details on b.Trade_ITI_id equals d.iti_college_id
                       join e in _db.tbl_district_master on d.district_id equals e.district_lgd_code
                       join f in _db.tbl_division_master on d.division_id equals f.division_id
                       join g in _db.tbl_Institute_type on d.Insitute_TypeId equals g.Institute_type_id
                       join sa in _db.Tbl_rules_allocation_master on d.CourseCode equals sa.CourseId
                       join sa1 in _db.tbl_Vertical_rule_value on sa.Rules_allocation_master_id equals sa1.Rules_allocation_master_id
                       join sa2 in _db.tbl_Vertical_rules on sa1.Vertical_rules_id equals sa2.Vertical_rules_id
                       join hori1 in _db.tbl_horizontal_rules_values on sa.Rules_allocation_master_id equals hori1.Rules_allocation_master_id
                       join hori2 in _db.Tbl_horizontal_rules on hori1.Horizontal_rules_id equals hori2.Horizontal_rules_id
                       select new seatmatrixmodel
                       {
                           //MISCode = t6.MISCode,
                           //district_id = t7.dist_id,
                           //iti_college_id = t6.iti_college_id,
                           //iti_college_name = t6.iti_college_name,

                       }).Distinct().ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<seatmatrixmodel> GetSummarySeatMatrixDLL(int id)
        {
            try
            {
                List<seatmatrixmodel> res = null;
                res = (from sa in _db.Tbl_rules_allocation_master
                       join sa1 in _db.tbl_Vertical_rule_value on sa.Rules_allocation_master_id equals sa1.Rules_allocation_master_id
                       join sa2 in _db.tbl_Vertical_rules on sa1.Vertical_rules_id equals sa2.Vertical_rules_id
                       join hori1 in _db.tbl_horizontal_rules_values on sa.Rules_allocation_master_id equals hori1.Rules_allocation_master_id
                       join hori2 in _db.Tbl_horizontal_rules on hori1.Horizontal_rules_id equals hori2.Horizontal_rules_id

                       select new seatmatrixmodel
                       {
                           Vertical_rulesid = sa1.Vertical_rules_id,
                           //RuleValue = sa1.RuleValue,
                           verticalRule = sa1.RuleValue,
                           HorizonRule = hori1.RuleValue,
                           Vertical_Rules = sa2.Vertical_Rules,
                           Rules_allocation_masterid = sa.Rules_allocation_master_id
                       }).ToList();


                foreach (var p in res)
                {
                    decimal x = 200;
                    p.RuleValue = p.verticalRule + p.HorizonRule;
                    p.TotalSeatMatrix = Math.Round((x * p.RuleValue) / 100, 2);

                }
                return res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<summarySeat> GetCheckSummaryDLL(int id, int AcademicYearId, int ApplicantTypeId, int InstituteId, int Round, int DistrictId, int DivisionId, int TalukId)
        {
            try
            {
                List<summarySeat> res = new List<summarySeat>{ new summarySeat
                 {  //GM
                    GMGPH = 0,
                    GMWH = 0,
                    GMPDH = 0,
                    GMEXSH = 0,
                    GMKMH = 0,
                    GMEWSH = 0,
                    //SC
                    SCGPH = 0,
                    SCWH = 0,
                    SCPDH = 0,
                    SCEXSH = 0,
                    SCKMH = 0,
                    SCEWSH = 0,
                    //ST
                    STGPH = 0,
                    STWH = 0,
                    STPDH = 0,
                    STEXSH = 0,
                    STKMH = 0,
                    STEWSH = 0,
                    //C1
                    C1GPH = 0,
                    C1WH = 0,
                    C1PDH = 0,
                    C1EXSH = 0,
                    C1KMH = 0,
                    C1EWSH = 0,
                    //2A
                    TWOAGPH = 0,
                    TWOAWH = 0,
                    TWOAPDH = 0,
                    TWOAEXSH = 0,
                    TWOAKMH = 0,
                    TWOAEWSH = 0,
                    //2B
                    TWOBGPH = 0,
                    TWOBWH = 0,
                    TWOBPDH = 0,
                    TWOBEXSH = 0,
                    TWOBKMH = 0,
                    TWOBEWSH = 0,
                    //3A
                    THREEAGPH = 0,
                    THREEAWH = 0,
                    THREEAPDH = 0,
                    THREEAEXSH = 0,
                    THREEAKMH = 0,
                    THREEAEWSH = 0,
                    //3B
                    THREEBGPH = 0,
                    THREEBWH = 0,
                    THREEBPDH = 0,
                    THREEBEXSH = 0,
                    THREEBKMH = 0,
                    THREEBEWSH = 0
                } };
                var rest = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == InstituteId && x.AcademicYear == AcademicYearId && x.Round == Round).Select(y => y.SeatMaxId).FirstOrDefault();
                if (rest != 0)
                {
                    var trad = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == rest).Select(y => y.SeatMaxTradeId).ToList();
                    summarySeat set = new summarySeat();
                    foreach (var itm in trad)
                    {
                        var es = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxTradeId == itm).FirstOrDefault();
                        //GM
                        set.GMGPH += (int)(es.GMGPH + es.GMGPNH);
                        set.GMWH += (int)(es.GMWH + es.GMWNH);
                        set.GMPDH += (int)(es.GMPDH + es.GMPDNH);
                        set.GMEXSH += (int)(es.GMEXSH + es.GMEXSNH);
                        set.GMKMH += (int)(es.GMKMH + es.GMKMNH);
                        set.GMEWSH += (int)(es.GMEWSH + es.GMEWSNH);
                        //SC
                        set.SCGPH += (int)(es.SCGPH + es.SCGPNH);
                        set.SCWH += (int)(es.SCWH + es.SCWNH);
                        set.SCPDH += (int)(es.SCPDH + es.SCPDNH);
                        set.SCEXSH += (int)(es.SCEXSH + es.SCEXSNH);
                        set.SCKMH += (int)(es.SCKMH + es.SCKMNH);
                        set.SCEWSH += (int)(es.SCEWSH + es.SCEWSNH);
                        //ST
                        set.STGPH += (int)(es.STGPH + es.STGPNH);
                        set.STWH += (int)(es.STWH + es.STWNH);
                        set.STPDH += (int)(es.STPDH + es.STPDNH);
                        set.STEXSH += (int)(es.STEXSH + es.STEXSNH);
                        set.STKMH += (int)(es.STKMH + es.STKMNH);
                        set.STEWSH += (int)(es.STEWSH + es.STEWSNH);
                        //C1
                        set.C1GPH += (int)(es.C1GPH + es.C1GPNH);
                        set.C1WH += (int)(es.C1WH + es.C1WNH);
                        set.C1PDH += (int)(es.C1PDH + es.C1PDNH);
                        set.C1EXSH += (int)(es.C1EXSH + es.C1EXSNH);
                        set.C1KMH += (int)(es.C1KMH + es.C1KMNH);
                        set.C1EWSH += (int)(es.C1EWSH + es.C1EWSNH);
                        //2A
                        set.TWOAGPH += (int)(es.TWOAGPH + es.TWOAGPNH);
                        set.TWOAWH += (int)(es.TWOAWH + es.TWOAWNH);
                        set.TWOAPDH += (int)(es.TWOAPDH + es.TWOAPDNH);
                        set.TWOAEXSH += (int)(es.TWOAEXSH + es.TWOAEXSNH);
                        set.TWOAKMH += (int)(es.TWOAKMH + es.TWOAKMNH);
                        set.TWOAEWSH += (int)(es.TWOAEWSH + es.TWOAEWSNH);
                        //2B
                        set.TWOBGPH += (int)(es.TWOBGPH + es.TWOBGPNH);
                        set.TWOBWH += (int)(es.TWOBWH + es.TWOBWNH);
                        set.TWOBPDH += (int)(es.TWOBPDH + es.TWOBPDNH);
                        set.TWOBEXSH += (int)(es.TWOBEXSH + es.TWOBEXSNH);
                        set.TWOBKMH += (int)(es.TWOBKMH + es.TWOBKMNH);
                        set.TWOBEWSH += (int)(es.TWOBEWSH + es.TWOBEWSNH);
                        //3A
                        set.THREEAGPH += (int)(es.THREEAGPH + es.THREEAGPNH);
                        set.THREEAWH += (int)(es.THREEAWH + es.THREEAWNH);
                        set.THREEAPDH += (int)(es.THREEAPDH + es.THREEAPDNH);
                        set.THREEAEXSH += (int)(es.THREEAEXSH + es.THREEAEXSNH);
                        set.THREEAKMH += (int)(es.THREEAKMH + es.THREEAKMNH);
                        set.THREEAEWSH += (int)(es.THREEAEWSH + es.THREEAEWSNH);
                        //3B
                        set.THREEBGPH += (int)(es.THREEBGPH + es.THREEBGPNH);
                        set.THREEBWH += (int)(es.THREEBWH + es.THREEBWNH);
                        set.THREEBPDH += (int)(es.THREEBPDH + es.THREEBPDNH);
                        set.THREEBEXSH += (int)(es.THREEBEXSH + es.THREEBEXSNH);
                        set.THREEBKMH += (int)(es.THREEBKMH + es.THREEBKMNH);
                        set.THREEBEWSH += (int)(es.THREEBEWSH + es.THREEBEWSNH);
                    }
                    res.Add(set);

                    return res;
                }
                else
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //UseCase31-Review Seat Matrix(AD) 
        public List<seatmatrixmodel> GetReviewSeatMatrixDLL(int id, int AcademicYearId)
        {
            try
            {
                List<seatmatrixmodel> res = null;
                if (id == 6)
                {
                    res = (from sm in _db.tbl_SeatMatrix_Main.Where(x => x.AcademicYear == AcademicYearId)
                           join clg in _db.tbl_iti_college_details on sm.InstituteId equals clg.iti_college_id
                           join sts in _db.tbl_status_master on sm.Status equals sts.StatusId
                           join ctMast in _db.tbl_course_type_mast on sm.CourseTypeId equals ctMast.course_id
                           join aptype in _db.tbl_ApplicantType on sm.ApplicantType equals aptype.ApplicantTypeId
                           where sm.Status == 8
                           select new seatmatrixmodel
                           {
                               SeatMaxId = sm.SeatMaxId,
                               ApplicantType = aptype.ApplicantTypeId,
                               ApplicantTypeDdl = aptype.ApplicantType,
                               StatusId = sts.StatusId,
                               StatusName = sts.StatusName,
                               courseid = ctMast.course_id,
                               coursetypename = ctMast.course_type_name,
                               iti_college_id = clg.iti_college_id,
                               iti_college_name = clg.iti_college_name,
                               Round = sm.Round
                           }).Distinct().ToList();

                }
                return res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<seatmatrixmodel> GetReviewTradeSeatMatrixIdDLL(int id, int SeatMaxId)
        {
            try
            {
                List<seatmatrixmodel> res = null;
                res = (from div in _db.tbl_division_master
                       join dis in _db.tbl_district_master on div.division_id equals dis.division_id
                       join tal in _db.tbl_taluk_master on dis.district_lgd_code equals tal.district_lgd_code
                       join icd in _db.tbl_iti_college_details on tal.taluk_id equals icd.taluk_id
                       join ad in _db.tbl_Applicant_Detail on dis.district_lgd_code equals ad.PDistrict
                       join ap in _db.tbl_ApplicantType on ad.ApplicantType equals ap.ApplicantTypeId
                       join smm in _db.tbl_SeatMatrix_Main on icd.iti_college_id equals smm.InstituteId
                       where smm.SeatMaxId == SeatMaxId
                       //where smm.ApplicantType == ApplicantTypeId && smm.AcademicYear == AcademicYearId && smm.Round == Round
                       //join App in _db.tbl_ApplicantType on
                       select new seatmatrixmodel
                       {
                           division_name = div.division_name,
                           district_ename = dis.district_ename,
                           taluk_ename = tal.taluk_ename,
                           iti_college_name = icd.iti_college_name,
                           MISCode = icd.MISCode,
                           iti_college_id = icd.iti_college_id,
                       }).ToList();

                return res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //UseCase30-Update Seat Matrix(DD) 
        public List<seatmatrixmodel> GetUpdateSeatMatrixDLL(int id, int AcademicYearId)
        {
            try
            {
                var res = (from sm in _db.tbl_SeatMatrix_Main
                           join clg in _db.tbl_iti_college_details on sm.InstituteId equals clg.iti_college_id
                           join sts in _db.tbl_status_master on sm.Status equals sts.StatusId
                           join ctMast in _db.tbl_course_type_mast on sm.CourseTypeId equals ctMast.course_id
                           join aptype in _db.tbl_ApplicantType on sm.ApplicantType equals aptype.ApplicantTypeId
                           join ab in _db.tbl_role_master on sm.FlowId equals ab.role_id
                           where (sm.Status == 5 || sm.Status == 7 || sm.Status == 9 || sm.Status == 2) && sm.AcademicYear == AcademicYearId
                           select new seatmatrixmodel
                           {
                               //SeatMaxId = sm.SeatMaxId,
                               ApplicantType = aptype.ApplicantTypeId,
                               ApplicantTypeDdl = aptype.ApplicantType,
                               StatusId = sts.StatusId,
                               StatusName = sts.StatusId == 2 ? sts.StatusName : sts.StatusName + " - " + ab.role_DescShortForm,
                               courseid = ctMast.course_id,
                               coursetypename = ctMast.course_type_name,
                               //iti_college_id = clg.iti_college_id,
                               //iti_college_name = clg.iti_college_name,
                               Round = sm.Round,
                               FlowId = sm.FlowId
                           }).Distinct().ToList();

                return res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<seatmatrixmodel> GetUpdateTradeSeatMatrixDLL(int id, int SeatMaxId)
        {
            try
            {
                var res = (from smm in _db.tbl_SeatMatrix_Main
                           join icd in _db.tbl_iti_college_details on smm.InstituteId equals icd.iti_college_id
                           join div in _db.tbl_division_master on icd.division_id equals div.division_id
                           join dis in _db.tbl_district_master on icd.district_id equals dis.district_lgd_code
                           join tal in _db.tbl_taluk_master on icd.taluk_id equals tal.taluk_lgd_code
                           where smm.SeatMaxId == SeatMaxId
                           select new seatmatrixmodel
                           {
                               division_name = div.division_name,
                               district_ename = dis.district_ename,
                               taluk_ename = tal.taluk_ename,
                               iti_college_name = icd.iti_college_name,
                               MISCode = icd.MISCode,
                               iti_college_id = icd.iti_college_id,
                           }).ToList();

                return res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Tradewideseat> getSeatistByTradeWise(int seatmaxId)
        {
            try
            {
                var res = (from sm in _db.tbl_SeatMatrix_TradeWise
                           where sm.SeatMaxId == seatmaxId
                           select new Tradewideseat
                           {
                               GMWH = sm.GMWH,
                               GMWNH = sm.GMWNH,
                               GMPDH = sm.GMPDH,
                               GMPDNH = sm.GMPDNH,
                               GMEXSH = sm.GMEXSH,
                               GMEXSNH = sm.GMEXSNH,
                               GMKMH = sm.GMKMH,
                               GMKMNH = sm.GMKMNH,
                               GMEWSH = sm.GMEWSH,
                               GMEWSNH = sm.GMEWSNH,
                               GMGPH = sm.GMGPH,
                               GMGPNH = sm.GMGPNH,
                               SCWH = sm.SCWH,
                               SCWNH = sm.SCWNH,
                               SCPDH = sm.SCPDH,
                               SCPDNH = sm.SCPDNH,
                               SCEXSH = sm.SCEXSH,
                               SCEXSNH = sm.SCEXSNH,
                               SCKMH = sm.SCKMH,
                               SCKMNH = sm.SCKMNH,
                               SCEWSH = sm.SCEWSH,
                               SCEWSNH = sm.SCEWSNH,
                               SCGPH = sm.SCGPH,
                               SCGPNH = sm.SCGPNH,
                               STWH = sm.STWH,
                               STWNH = sm.STWNH,
                               STPDH = sm.STPDH,
                               STPDNH = sm.STPDNH,
                               STEXSH = sm.STEXSH,
                               STEXSNH = sm.STEXSNH,
                               STKMH = sm.STKMH,
                               STKMNH = sm.STKMNH,
                               STEWSH = sm.STEWSH,
                               STEWSNH = sm.STEWSNH,
                               STGPH = sm.STGPH,
                               STGPNH = sm.STGPNH,
                               C1WH = sm.C1WH,
                               C1WNH = sm.C1WNH,
                               C1PDH = sm.C1PDH,
                               C1PDNH = sm.C1PDNH,
                               C1EXSH = sm.C1EXSH,
                               C1EXSNH = sm.C1EXSNH,
                               C1KMH = sm.C1KMH,
                               C1KMNH = sm.C1KMNH,
                               C1EWSH = sm.C1EWSH,
                               C1EWSNH = sm.C1EWSNH,
                               C1GPH = sm.C1GPH,
                               C1GPNH = sm.C1GPNH,
                               TWOAWH = sm.TWOAWH,
                               TWOAWNH = sm.TWOAWNH,
                               TWOAPDH = sm.TWOAPDH,
                               TWOAPDNH = sm.TWOAPDNH,
                               TWOAEXSH = sm.TWOAEXSH,
                               TWOAEXSNH = sm.TWOAEXSNH,
                               TWOAKMH = sm.TWOAKMH,
                               TWOAKMNH = sm.TWOAKMNH,
                               TWOAEWSH = sm.TWOAEWSH,
                               TWOAEWSNH = sm.TWOAEWSNH,
                               TWOAGPH = sm.TWOAGPH,
                               TWOAGPNH = sm.TWOAGPNH,
                               TWOBWH = sm.TWOBWH,
                               TWOBWNH = sm.TWOBWNH,
                               TWOBPDH = sm.TWOBPDH,
                               TWOBPDNH = sm.TWOBPDNH,
                               TWOBEXSH = sm.TWOBEXSH,
                               TWOBEXSNH = sm.TWOBEXSNH,
                               TWOBKMH = sm.TWOBKMH,
                               TWOBKMNH = sm.TWOBKMNH,
                               TWOBEWSH = sm.TWOBEWSH,
                               TWOBEWSNH = sm.TWOBEWSNH,
                               TWOBGPH = sm.TWOBGPH,
                               TWOBGPNH = sm.TWOBGPNH,
                               THREEAWH = sm.THREEAWH,
                               THREEAWNH = sm.THREEAWNH,
                               THREEAPDH = sm.THREEAPDH,
                               THREEAPDNH = sm.THREEAPDNH,
                               THREEAEXSH = sm.THREEAEXSH,
                               THREEAEXSNH = sm.THREEAEXSNH,
                               THREEAKMH = sm.THREEAKMH,
                               THREEAKMNH = sm.THREEAKMNH,
                               THREEAEWSH = sm.THREEAEWSH,
                               THREEAEWSNH = sm.THREEAEWSNH,
                               THREEAGPH = sm.THREEAGPH,
                               THREEAGPNH = sm.THREEAGPNH,
                               THREEBWH = sm.THREEBWH,
                               THREEBWNH = sm.THREEBWNH,
                               THREEBPDH = sm.THREEBPDH,
                               THREEBPDNH = sm.THREEBPDNH,
                               THREEBEXSH = sm.THREEBEXSH,
                               THREEBEXSNH = sm.THREEBEXSNH,
                               THREEBKMH = sm.THREEBKMH,
                               THREEBKMNH = sm.THREEBKMNH,
                               THREEBEWSH = sm.THREEBEWSH,
                               THREEBEWSNH = sm.THREEBEWSNH,
                               THREEBGPH = sm.THREEBGPH,
                               THREEBGPNH = sm.THREEBGPNH
                           }).ToList();

                return res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //UseCase32-Approve & Publish Seat Matrix(Director/Commissioner) 
        public List<seatmatrixmodel> GetApproveSeatMatrixDLL(int id, int AcademicYearId)
        {
            try
            {
                List<seatmatrixmodel> res = null;
                if (id == 1 || id == 2)
                {
                    res = (from sm in _db.tbl_SeatMatrix_Main.Where(x => x.AcademicYear == AcademicYearId)
                           join clg in _db.tbl_iti_college_details on sm.InstituteId equals clg.iti_college_id
                           join sts in _db.tbl_status_master on sm.Status equals sts.StatusId
                           join ctMast in _db.tbl_course_type_mast on sm.CourseTypeId equals ctMast.course_id
                           join aptype in _db.tbl_ApplicantType on sm.ApplicantType equals aptype.ApplicantTypeId
                           where sm.Status == 7
                           select new seatmatrixmodel
                           {
                               SeatMaxId = sm.SeatMaxId,
                               ApplicantType = aptype.ApplicantTypeId,
                               ApplicantTypeDdl = aptype.ApplicantType,
                               StatusId = sts.StatusId,
                               StatusName = sts.StatusName,
                               courseid = ctMast.course_id,
                               coursetypename = ctMast.course_type_name,
                               iti_college_id = clg.iti_college_id,
                               iti_college_name = clg.iti_college_name,
                               Round = sm.Round
                           }).Distinct().ToList();
                }
                return res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<seatmatrixmodel> GetAproveTradeSeatMatrixIdDLL(int id, int SeatMaxId)
        {
            try
            {
                List<seatmatrixmodel> res = null;
                res = (from div in _db.tbl_division_master
                       join dis in _db.tbl_district_master on div.division_id equals dis.division_id
                       join tal in _db.tbl_taluk_master on dis.district_lgd_code equals tal.district_lgd_code
                       join icd in _db.tbl_iti_college_details on tal.taluk_id equals icd.taluk_id
                       join ad in _db.tbl_Applicant_Detail on dis.district_lgd_code equals ad.PDistrict
                       join ap in _db.tbl_ApplicantType on ad.ApplicantType equals ap.ApplicantTypeId
                       join smm in _db.tbl_SeatMatrix_Main on icd.iti_college_id equals smm.InstituteId
                       where smm.SeatMaxId == SeatMaxId
                       //where smm.ApplicantType == ApplicantTypeId && smm.AcademicYear == AcademicYearId && smm.Round == Round
                       //join App in _db.tbl_ApplicantType on
                       select new seatmatrixmodel
                       {
                           division_name = div.division_name,
                           district_ename = dis.district_ename,
                           taluk_ename = tal.taluk_ename,
                           iti_college_name = icd.iti_college_name,
                           MISCode = icd.MISCode,
                           iti_college_id = icd.iti_college_id,
                       }).ToList();

                return res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<seatmatrixmodel> GetViewSeatMatrixGridDLL(int id, int ApplicantTypeId, int AcademicYearId, int Round, int DistrictId, int DivisionId, int TalukId, int InstituteId)
        {
            try
            {
                List<seatmatrixmodel> res = null;
                if (InstituteId == 0)
                {
                    //var se = _db.tbl_SeatMatrix_Main.Where(x => x.Round == Round && x.AcademicYear == AcademicYearId)
                    //    .Select(y => y.SeatMaxId).ToList();

                    // var srr = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == se).ToList();

                    res = (from zz in _db.tbl_SeatMatrix_TradeWise
                           join yy in _db.tbl_SeatMatrix_Main on zz.SeatMaxId equals yy.SeatMaxId
                           join aa in _db.tbl_iti_college_details on yy.InstituteId equals aa.iti_college_id
                           join bb in _db.tbl_division_master on aa.division_id equals bb.division_id
                           join cc in _db.tbl_district_master on aa.district_id equals cc.district_lgd_code
                           join pp in _db.tbl_taluk_master on aa.taluk_id equals pp.taluk_lgd_code
                           join dd in _db.tbl_Institute_type on aa.Insitute_TypeId equals dd.Institute_type_id
                           join ee in _db.tbl_ITI_Trade on aa.iti_college_id equals ee.ITICode
                           join ff in _db.tbl_trade_mast on ee.TradeCode equals ff.trade_id
                           where yy.Round == Round && yy.AcademicYear == AcademicYearId && yy.Status == 2
                           //where zz.SeatMaxId == se && yy.Status==2
                           select new seatmatrixmodel
                           {
                               division_name = bb.division_name,
                               district_ename = cc.district_ename,
                               taluk_ename = pp.taluk_ename,
                               MISCode = aa.MISCode,
                               InstituteType = dd.Institute_type,
                               InstituteName = aa.iti_college_name,
                               TradeName = ff.trade_name,
                               InstituteId = aa.iti_college_id,
                               Round = Round,
                               TradeId = ff.trade_id,
                               AcademicYear = AcademicYearId,
                           }).Distinct().OrderBy(d => d.division_name).ThenBy(di=>di.district_ename)
                           .ThenBy(t=>t.taluk_ename).ThenBy(m=>m.MISCode).ToList();

                    return res;

                }
                else
                {
                    return res;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<seatmatrixmodel> GetViewSeatMatrix(int ApplicantTypeId, int AcademicYearId, int Round, int loginId)
        {
            try
            {
                List<seatmatrixmodel> res = null;
                var InstituteId = GetCollegeId(loginId);
                if (InstituteId != 0)
                {
                    var se = _db.tbl_SeatMatrix_Main.Where(x => x.InstituteId == InstituteId && x.Round == Round && x.AcademicYear == AcademicYearId && x.Status == 2).Select(y => y.SeatMaxId).FirstOrDefault();

                    var srr = _db.tbl_SeatMatrix_TradeWise.Where(x => x.SeatMaxId == se).ToList();

                    res = (from zz in _db.tbl_SeatMatrix_TradeWise
                           join yy in _db.tbl_SeatMatrix_Main on zz.SeatMaxId equals yy.SeatMaxId
                           join aa in _db.tbl_iti_college_details on yy.InstituteId equals aa.iti_college_id
                           join bb in _db.tbl_division_master on aa.division_id equals bb.division_id
                           join cc in _db.tbl_district_master on aa.district_id equals cc.district_lgd_code
                           join dd in _db.tbl_Institute_type on aa.Insitute_TypeId equals dd.Institute_type_id
                           join ee in _db.tbl_ITI_Trade on aa.iti_college_id equals ee.ITICode
                           join ff in _db.tbl_trade_mast on ee.TradeCode equals ff.trade_id
                           where zz.SeatMaxId == se
                           select new seatmatrixmodel
                           {
                               division_name = bb.division_name,
                               district_ename = cc.district_ename,
                               MISCode = aa.MISCode,
                               InstituteType = dd.Institute_type,
                               InstituteName = aa.iti_college_name,
                               TradeName = ff.trade_name,
                               InstituteId = aa.iti_college_id,
                               Round = Round,
                               TradeId = ff.trade_id,
                               AcademicYear = AcademicYearId,
                           }).Distinct().ToList();

                    return res;

                }
                else
                {
                    return res;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        int GetCollegeId(int loginId)
        {
            try
            {
                int id = _db.Staff_Institute_Detail.Where(x => x.UserId == loginId).Select(y => y.InstituteId).FirstOrDefault();
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        GenerateTradewiseSeat Round2SeatData(GenerateTradewiseSeat gts, GenerateTradewiseSeat gts1)
        {
            gts.GMW = Convert.ToDecimal(gts.GMWH) + Convert.ToDecimal(gts.GMWNH); gts.GMWH = 0; gts.GMWNH = 0;
            gts.GMPD = Convert.ToDecimal(gts.GMPDH) + Convert.ToDecimal(gts.GMPDNH); gts.GMPDH = 0; gts.GMPDNH = 0;
            gts.GMEXS = Convert.ToDecimal(gts.GMEXSH) + Convert.ToDecimal(gts.GMEXSNH); gts.GMEXSH = 0; gts.GMEXSNH = 0;
            gts.GMKM = Convert.ToDecimal(gts.GMKMH) + Convert.ToDecimal(gts.GMKMNH); gts.GMKMH = 0; gts.GMKMNH = 0;
            gts.GMEWS = Convert.ToDecimal(gts.GMEWSH) + Convert.ToDecimal(gts.GMEWSNH); gts.GMEWSH = 0; gts.GMEWSNH = 0;
            gts.GMGP = Convert.ToDecimal(gts.GMGPH) + Convert.ToDecimal(gts.GMGPNH); gts.GMGPH = 0; gts.GMGPNH = 0;
            gts.SCW = Convert.ToDecimal(gts.SCWH) + Convert.ToDecimal(gts.SCWNH); gts.SCWH = 0; gts.SCWNH = 0;
            gts.SCPD = Convert.ToDecimal(gts.SCPDH) + Convert.ToDecimal(gts.SCPDNH); gts.SCPDH = 0; gts.SCPDNH = 0;
            gts.SCEXS = Convert.ToDecimal(gts.SCEXSH) + Convert.ToDecimal(gts.SCEXSNH); gts.SCEXSH = 0; gts.SCEXSNH = 0;
            gts.SCKM = Convert.ToDecimal(gts.SCKMH) + Convert.ToDecimal(gts.SCKMNH); gts.SCKMH = 0; gts.SCKMNH = 0;
            gts.SCEWS = Convert.ToDecimal(gts.SCEWSH) + Convert.ToDecimal(gts.SCEWSNH); gts.SCEWSH = 0; gts.SCEWSNH = 0;
            gts.SCGP = Convert.ToDecimal(gts.SCGPH) + Convert.ToDecimal(gts.SCGPNH); gts.SCGPH = 0; gts.SCGPNH = 0;
            gts.STWH = Convert.ToDecimal(gts.STWH) + Convert.ToDecimal(gts.STWNH); gts.STWH = 0; gts.STWNH = 0;
            gts.STPD = Convert.ToDecimal(gts.STPDH) + Convert.ToDecimal(gts.STPDNH); gts.STPDH = 0; gts.STPDNH = 0;
            gts.STEXS = Convert.ToDecimal(gts.STEXSH) + Convert.ToDecimal(gts.STEXSNH); gts.STEXSH = 0; gts.STEXSNH = 0;
            gts.STKM = Convert.ToDecimal(gts.STKMH) + Convert.ToDecimal(gts.STKMNH); gts.STKMH = 0; gts.STKMNH = 0;
            gts.STEWS = Convert.ToDecimal(gts.STEWSH) + Convert.ToDecimal(gts.STEWSNH); gts.STEWSH = 0; gts.STEWSNH = 0;
            gts.STGP = Convert.ToDecimal(gts.STGPH) + Convert.ToDecimal(gts.STGPNH); gts.STGPH = 0; gts.STGPNH = 0;
            gts.C1W = Convert.ToDecimal(gts.C1WH) + Convert.ToDecimal(gts.C1WNH); gts.C1WH = 0; gts.C1WNH = 0;
            gts.C1PD = Convert.ToDecimal(gts.C1PDH) + Convert.ToDecimal(gts.C1PDNH); gts.C1PDH = 0; gts.C1PDNH = 0;
            gts.C1EXS = Convert.ToDecimal(gts.C1EXSH) + Convert.ToDecimal(gts.C1EXSNH); gts.C1EXSH = 0; gts.C1EXSNH = 0;
            gts.C1KM = Convert.ToDecimal(gts.C1KMH) + Convert.ToDecimal(gts.C1KMNH); gts.C1KMH = 0; gts.C1KMNH = 0;
            gts.C1EWS = Convert.ToDecimal(gts.C1EWSH) + Convert.ToDecimal(gts.C1EWSNH); gts.C1EWSH = 0; gts.C1EWSNH = 0;
            gts.C1GP = Convert.ToDecimal(gts.C1GPH) + Convert.ToDecimal(gts.C1GPNH); gts.C1GPH = 0; gts.C1GPNH = 0;
            gts.TWOAW = Convert.ToDecimal(gts.TWOAWH) + Convert.ToDecimal(gts.TWOAWNH); gts.TWOAWH = 0; gts.TWOAWNH = 0;
            gts.TWOAPD = Convert.ToDecimal(gts.TWOAPDH) + Convert.ToDecimal(gts.TWOAPDNH); gts.TWOAPDH = 0; gts.TWOAPDNH = 0;
            gts.TWOAEXS = Convert.ToDecimal(gts.TWOAEXSH) + Convert.ToDecimal(gts.TWOAEXSNH); gts.TWOAEXSH = 0; gts.TWOAEXSNH = 0;
            gts.TWOAKM = Convert.ToDecimal(gts.TWOAKMH) + Convert.ToDecimal(gts.TWOAKMNH); gts.TWOAKMH = 0; gts.TWOAKMNH = 0;
            gts.TWOAEWS = Convert.ToDecimal(gts.TWOAEWSH) + Convert.ToDecimal(gts.TWOAEWSNH); gts.TWOAEWSH = 0; gts.TWOAEWSNH = 0;
            gts.TWOAGP = Convert.ToDecimal(gts.TWOAGPH) + Convert.ToDecimal(gts.TWOAGPNH); gts.TWOAGPH = 0; gts.TWOAGPNH = 0;
            gts.TWOBW = Convert.ToDecimal(gts.TWOBWH) + Convert.ToDecimal(gts.TWOBWNH); gts.TWOBWH = 0; gts.TWOBWNH = 0;
            gts.TWOBPD = Convert.ToDecimal(gts.TWOBPDH) + Convert.ToDecimal(gts.TWOBPDNH); gts.TWOBPDH = 0; gts.TWOBPDNH = 0;
            gts.TWOBEXS = Convert.ToDecimal(gts.TWOBEXSH) + Convert.ToDecimal(gts.TWOBEXSNH); gts.TWOBEXSH = 0; gts.TWOBEXSNH = 0;
            gts.TWOBKM = Convert.ToDecimal(gts.TWOBKMH) + Convert.ToDecimal(gts.TWOBKMNH); gts.TWOBKMH = 0; gts.TWOBKMNH = 0;
            gts.TWOBEWS = Convert.ToDecimal(gts.TWOBEWSH) + Convert.ToDecimal(gts.TWOBEWSNH); gts.TWOBEWSH = 0; gts.TWOBEWSNH = 0;
            gts.TWOBGP = Convert.ToDecimal(gts.TWOBGPH) + Convert.ToDecimal(gts.TWOBGPNH); gts.TWOBGPH = 0; gts.TWOBGPNH = 0;
            gts.THREEAW = Convert.ToDecimal(gts.THREEAWH) + Convert.ToDecimal(gts.THREEAWNH); gts.THREEAWH = 0; gts.THREEAWNH = 0;
            gts.THREEAPD = Convert.ToDecimal(gts.THREEAPDH) + Convert.ToDecimal(gts.THREEAPDNH); gts.THREEAPDH = 0; gts.THREEAPDNH = 0;
            gts.THREEAEXS = Convert.ToDecimal(gts.THREEAEXSH) + Convert.ToDecimal(gts.THREEAEXSNH); gts.THREEAEXSH = 0; gts.THREEAEXSNH = 0;
            gts.THREEAKM = Convert.ToDecimal(gts.THREEAKMH) + Convert.ToDecimal(gts.THREEAKMNH); gts.THREEAKMH = 0; gts.THREEAKMNH = 0;
            gts.THREEAEWS = Convert.ToDecimal(gts.THREEAEWSH) + Convert.ToDecimal(gts.THREEAEWSNH); gts.THREEAEWSH = 0; gts.THREEAEWSNH = 0;
            gts.THREEAGP = Convert.ToDecimal(gts.THREEAGPH) + Convert.ToDecimal(gts.THREEAGPNH); gts.THREEAGPH = 0; gts.THREEAGPNH = 0;
            gts.THREEBW = Convert.ToDecimal(gts.THREEBWH) + Convert.ToDecimal(gts.THREEBWNH); gts.THREEBWH = 0; gts.THREEBWNH = 0;
            gts.THREEBPD = Convert.ToDecimal(gts.THREEBPDH) + Convert.ToDecimal(gts.THREEBPDNH); gts.THREEBPDH = 0; gts.THREEBPDNH = 0;
            gts.THREEBEXS = Convert.ToDecimal(gts.THREEBEXSH) + Convert.ToDecimal(gts.THREEBEXSNH); gts.THREEBEXSH = 0; gts.THREEBEXSNH = 0;
            gts.THREEBKM = Convert.ToDecimal(gts.THREEBKMH) + Convert.ToDecimal(gts.THREEBKMNH); gts.THREEBKMH = 0; gts.THREEBKMNH = 0;
            gts.THREEBEWS = Convert.ToDecimal(gts.THREEBEWSH) + Convert.ToDecimal(gts.THREEBEWSNH); gts.THREEBEWSH = 0; gts.THREEBEWSNH = 0;
            gts.THREEBGP = Convert.ToDecimal(gts.THREEBGPH) + Convert.ToDecimal(gts.THREEBGPNH); gts.THREEBGPH = 0; gts.THREEBGPNH = 0;
            return gts;
        }

        //Convert tblSeatMatrixTradeWiseClsToGenTradeSeatModel
        GenerateTradewiseSeat Round2SeatData  (GenerateTradewiseSeat gts, tbl_SeatMatrix_TradeWise tsmtw) 
        {
            gts.GMWH = Convert.ToDecimal(tsmtw.GMWH); gts.GMWNH = Convert.ToDecimal(tsmtw.GMWNH);
            gts.GMPDH = Convert.ToDecimal(tsmtw.GMPDH); gts.GMPDNH = Convert.ToDecimal(tsmtw.GMPDNH);
            gts.GMEXSH = Convert.ToDecimal(tsmtw.GMEXSH); gts.GMEXSNH = Convert.ToDecimal(tsmtw.GMEXSNH);
            gts.GMKMH = Convert.ToDecimal(tsmtw.GMKMH); gts.GMKMNH = Convert.ToDecimal(tsmtw.GMKMNH);
            gts.GMEWSH = Convert.ToDecimal(tsmtw.GMEWSH); gts.GMEWSNH = Convert.ToDecimal(tsmtw.GMEWSNH);
            gts.GMGPH = Convert.ToDecimal(tsmtw.GMGPH); gts.GMGPNH = Convert.ToDecimal(tsmtw.GMGPNH);
            gts.SCWH = Convert.ToDecimal(tsmtw.SCWH); gts.SCWNH = Convert.ToDecimal(tsmtw.SCWNH);
            gts.SCPDH = Convert.ToDecimal(tsmtw.SCPDH); gts.SCPDNH = Convert.ToDecimal(tsmtw.SCPDNH);
            gts.SCEXSH = Convert.ToDecimal(tsmtw.SCEXSH); gts.SCEXSNH = Convert.ToDecimal(tsmtw.SCEXSNH);
            gts.SCKMH = Convert.ToDecimal(tsmtw.SCKMH); gts.SCKMNH = Convert.ToDecimal(tsmtw.SCKMNH);
            gts.SCEWSH = Convert.ToDecimal(tsmtw.SCEWSH); gts.SCEWSNH = Convert.ToDecimal(tsmtw.SCEWSNH);
            gts.SCGPH = Convert.ToDecimal(tsmtw.SCGPH); gts.SCGPNH = Convert.ToDecimal(tsmtw.SCGPNH);
            gts.STWH = Convert.ToDecimal(tsmtw.STWH); gts.STWNH = Convert.ToDecimal(tsmtw.STWNH);
            gts.STPDH = Convert.ToDecimal(tsmtw.STPDH); gts.STPDNH = Convert.ToDecimal(tsmtw.STPDNH);
            gts.STEXSH = Convert.ToDecimal(tsmtw.STEXSH); gts.STEXSNH = Convert.ToDecimal(tsmtw.STEXSNH);
            gts.STKMH = Convert.ToDecimal(tsmtw.STKMH); gts.STKMNH = Convert.ToDecimal(tsmtw.STKMNH);
            gts.STEWSH = Convert.ToDecimal(tsmtw.STEWSH); gts.STEWSNH = Convert.ToDecimal(tsmtw.STEWSNH);
            gts.STGPH = Convert.ToDecimal(tsmtw.STGPH); gts.STGPNH = Convert.ToDecimal(tsmtw.STGPNH);
            gts.C1WH = Convert.ToDecimal(tsmtw.C1WH); gts.C1WNH = Convert.ToDecimal(tsmtw.C1WNH);
            gts.C1PDH = Convert.ToDecimal(tsmtw.C1PDH); gts.C1PDNH = Convert.ToDecimal(tsmtw.C1PDNH);
            gts.C1EXSH = Convert.ToDecimal(tsmtw.C1EXSH); gts.C1EXSNH = Convert.ToDecimal(tsmtw.C1EXSNH);
            gts.C1KMH = Convert.ToDecimal(tsmtw.C1KMH); gts.C1KMNH = Convert.ToDecimal(tsmtw.C1KMNH);
            gts.C1EWSH = Convert.ToDecimal(tsmtw.C1EWSH); gts.C1EWSNH = Convert.ToDecimal(tsmtw.C1EWSNH);
            gts.C1GPH = Convert.ToDecimal(tsmtw.C1GPH); gts.C1GPNH = Convert.ToDecimal(tsmtw.C1GPNH);
            gts.TWOAWH = Convert.ToDecimal(tsmtw.TWOAWH); gts.TWOAWNH = Convert.ToDecimal(tsmtw.TWOAWNH);
            gts.TWOAPDH = Convert.ToDecimal(tsmtw.TWOAPDH); gts.TWOAPDNH = Convert.ToDecimal(tsmtw.TWOAPDNH);
            gts.TWOAEXSH = Convert.ToDecimal(tsmtw.TWOAEXSH); gts.TWOAEXSNH = Convert.ToDecimal(tsmtw.TWOAEXSNH);
            gts.TWOAKMH = Convert.ToDecimal(tsmtw.TWOAKMH); gts.TWOAKMNH = Convert.ToDecimal(tsmtw.TWOAKMNH);
            gts.TWOAEWSH = Convert.ToDecimal(tsmtw.TWOAEWSH); gts.TWOAEWSNH = Convert.ToDecimal(tsmtw.TWOAEWSNH);
            gts.TWOAGPH = Convert.ToDecimal(tsmtw.TWOAGPH); gts.TWOAGPNH = Convert.ToDecimal(tsmtw.TWOAGPNH);
            gts.TWOBWH = Convert.ToDecimal(tsmtw.TWOBWH); gts.TWOBWNH = Convert.ToDecimal(tsmtw.TWOBWNH);
            gts.TWOBPDH = Convert.ToDecimal(tsmtw.TWOBPDH); gts.TWOBPDNH = Convert.ToDecimal(tsmtw.TWOBPDNH);
            gts.TWOBEXSH = Convert.ToDecimal(tsmtw.TWOBEXSH); gts.TWOBEXSNH = Convert.ToDecimal(tsmtw.TWOBEXSNH);
            gts.TWOBKMH = Convert.ToDecimal(tsmtw.TWOBKMH); gts.TWOBKMNH = Convert.ToDecimal(tsmtw.TWOBKMNH);
            gts.TWOBEWSH = Convert.ToDecimal(tsmtw.TWOBEWSH); gts.TWOBEWSNH = Convert.ToDecimal(tsmtw.TWOBEWSNH);
            gts.TWOBGPH = Convert.ToDecimal(tsmtw.TWOBGPH); gts.TWOBGPNH = Convert.ToDecimal(tsmtw.TWOBGPNH);
            gts.THREEAWH = Convert.ToDecimal(tsmtw.THREEAWH); gts.THREEAWNH = Convert.ToDecimal(tsmtw.THREEAWNH);
            gts.THREEAPDH = Convert.ToDecimal(tsmtw.THREEAPDH); gts.THREEAPDNH = Convert.ToDecimal(tsmtw.THREEAPDNH);
            gts.THREEAEXSH = Convert.ToDecimal(tsmtw.THREEAEXSH); gts.THREEAEXSNH = Convert.ToDecimal(tsmtw.THREEAEXSNH);
            gts.THREEAKMH = Convert.ToDecimal(tsmtw.THREEAKMH); gts.THREEAKMNH = Convert.ToDecimal(tsmtw.THREEAKMNH);
            gts.THREEAEWSH = Convert.ToDecimal(tsmtw.THREEAEWSH); gts.THREEAEWSNH = Convert.ToDecimal(tsmtw.THREEAEWSNH);
            gts.THREEAGPH = Convert.ToDecimal(tsmtw.THREEAGPH); gts.THREEAGPNH = Convert.ToDecimal(tsmtw.THREEAGPNH);
            gts.THREEBWH = Convert.ToDecimal(tsmtw.THREEBWH); gts.THREEBWNH = Convert.ToDecimal(tsmtw.THREEBWNH);
            gts.THREEBPDH = Convert.ToDecimal(tsmtw.THREEBPDH); gts.THREEBPDNH = Convert.ToDecimal(tsmtw.THREEBPDNH);
            gts.THREEBEXSH = Convert.ToDecimal(tsmtw.THREEBEXSH); gts.THREEBEXSNH = Convert.ToDecimal(tsmtw.THREEBEXSNH);
            gts.THREEBKMH = Convert.ToDecimal(tsmtw.THREEBKMH); gts.THREEBKMNH = Convert.ToDecimal(tsmtw.THREEBKMNH);
            gts.THREEBEWSH = Convert.ToDecimal(tsmtw.THREEBEWSH); gts.THREEBEWSNH = Convert.ToDecimal(tsmtw.THREEBEWSNH);
            gts.THREEBGPH = Convert.ToDecimal(tsmtw.THREEBGPH); gts.THREEBGPNH = Convert.ToDecimal(tsmtw.THREEBGPNH);

            return gts;
        }

        List<GenerateTradewiseSeat> InitTradeList()
        {
            return new List<GenerateTradewiseSeat>
            {
                 new GenerateTradewiseSeat
                 {
                      TradeId = 0, TradeName = "zero", Seats = 0,
                      GMW = 0, GMWH = 0, GMWNH = 0, GMPD = 0, GMPDH = 0, GMPDNH = 0, GMEXS = 0, GMEXSH = 0, GMEXSNH = 0, GMKM = 0, GMKMH = 0, GMKMNH = 0, GMEWS = 0, GMEWSH = 0, GMEWSNH = 0, GMGP = 0, GMGPH = 0, GMGPNH = 0,
                      SCW = 0, SCWH = 0, SCWNH = 0, SCPD = 0, SCPDH = 0, SCPDNH = 0, SCEXS = 0, SCEXSH = 0, SCEXSNH = 0, SCKM = 0, SCKMH = 0, SCKMNH = 0, SCEWS = 0, SCEWSH = 0, SCEWSNH = 0, SCGP = 0, SCGPH = 0, SCGPNH = 0,
                      STW = 0, STWH = 0, STWNH = 0, STPD = 0, STPDH = 0, STPDNH = 0, STEXS = 0, STEXSH = 0, STEXSNH = 0, STKM = 0, STKMH = 0, STKMNH = 0, STEWS = 0, STEWSH = 0, STEWSNH = 0, STGP = 0, STGPH = 0, STGPNH = 0,
                      C1W = 0, C1WH = 0, C1WNH = 0, C1PD = 0, C1PDH = 0, C1PDNH = 0, C1EXS = 0, C1EXSH = 0, C1EXSNH = 0, C1KM = 0, C1KMH = 0, C1KMNH = 0, C1EWS = 0, C1EWSH = 0, C1EWSNH = 0, C1GP = 0, C1GPH = 0, C1GPNH = 0,
                      TWOAW = 0, TWOAWH = 0, TWOAWNH = 0, TWOAPD = 0, TWOAPDH = 0, TWOAPDNH = 0, TWOAEXS = 0, TWOAEXSH = 0, TWOAEXSNH = 0, TWOAKM = 0, TWOAKMH = 0, TWOAKMNH = 0, TWOAEWS = 0, TWOAEWSH = 0, TWOAEWSNH = 0, TWOAGP = 0, TWOAGPH = 0, TWOAGPNH = 0,
                      TWOBW = 0, TWOBWH = 0, TWOBWNH = 0, TWOBPD = 0, TWOBPDH = 0, TWOBPDNH = 0, TWOBEXS = 0, TWOBEXSH = 0, TWOBEXSNH = 0, TWOBKM = 0, TWOBKMH = 0, TWOBKMNH = 0, TWOBEWS = 0, TWOBEWSH = 0, TWOBEWSNH = 0, TWOBGP = 0, TWOBGPH = 0, TWOBGPNH = 0,
                      THREEAW = 0, THREEAWH = 0, THREEAWNH = 0, THREEAPD = 0, THREEAPDH = 0, THREEAPDNH = 0, THREEAEXS = 0, THREEAEXSH = 0, THREEAEXSNH = 0, THREEAKM = 0, THREEAKMH = 0, THREEAKMNH = 0, THREEAEWS = 0, THREEAEWSH = 0, THREEAEWSNH = 0, THREEAGP = 0, THREEAGPH = 0, THREEAGPNH = 0,
                      THREEBW = 0, THREEBWH = 0, THREEBWNH = 0, THREEBPD = 0, THREEBPDH = 0, THREEBPDNH = 0, THREEBEXS = 0, THREEBEXSH = 0, THREEBEXSNH = 0, THREEBKM = 0, THREEBKMH = 0, THREEBKMNH = 0, THREEBEWS = 0, THREEBEWSH = 0, THREEBEWSNH = 0, THREEBGP = 0, THREEBGPH = 0, THREEBGPNH = 0,
                 }
            };
        }
    }
}
