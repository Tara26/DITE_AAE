using System;
using System.Collections.Generic;
using DLL.DBConnection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Models.Admission.MasterDataEntry;
using Models.Master;

namespace DLL.Admission.MasterDataEntry
{
    public class MasterDataEntryDll: IMasterDataEntryDll
    {
        private readonly DbConnection _db = new DbConnection();

        public List<CourseMaster> GetCourseTypes()
        {
            try
            {
                var res=(from a in _db.tbl_course_type_mast
                         orderby a.course_id descending
                         where a.is_active==true
                        select new CourseMaster
                        {
                            CourseId=a.course_id,
                            CourseTypeName=a.course_type_name
                        }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int UpdateDetails(DataTable dt, int CourseId,int userId)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    tbl_ITI_Trade trade = new tbl_ITI_Trade();

                    trade.TradeCode= Convert.ToInt32(row.ItemArray[8].ToString());
                    trade.StatusId = 1;
                    string miscode = row.ItemArray[0].ToString().Trim();
                    var ITICode = _db.tbl_iti_college_details.Where(x => x.MISCode == miscode).Select(y => y.iti_college_id).SingleOrDefault();
                    trade.ITICode = ITICode;
                    trade.CreatedOn = DateTime.Now;
                    trade.CreatedBy = userId;
                    _db.tbl_ITI_Trade.Add(trade);
                    _db.SaveChanges();
                    
                    tbl_ITI_trade_seat_master ItemDetails = new tbl_ITI_trade_seat_master();                                       

                    //ITICode                    
                    var Trade_ITI_Id = (from post in _db.tbl_ITI_Trade
                                        where post.ITICode == ITICode
                                        orderby post.Trade_ITI_id descending
                                     select post.Trade_ITI_id).Take(1).FirstOrDefault();

                    ItemDetails.Trade_ITI_Id = Convert.ToInt32(Trade_ITI_Id);

                    //UnitId
                    int itiColegeid = Convert.ToInt32(row.ItemArray[6].ToString());
                    var unit = _db.tbl_iti_college_details.Where(x => x.iti_college_id == itiColegeid).Select(y => y.Units).SingleOrDefault();
                    var UnitId = _db.tbl_units.Where(x => x.units == unit.ToString()).Select(y => y.u_id).FirstOrDefault();
                    ItemDetails.UnitId = Convert.ToInt32(UnitId);

                    //shift_id
                    string shifts = row.ItemArray[16].ToString();
                    var shiftid = _db.tbl_shifts.Where(x => x.shifts == shifts).Select(y=>y.s_id).FirstOrDefault();                    
                    ItemDetails.ShiftId = shiftid;

                    ItemDetails.SeatsPerUnit = Convert.ToInt32(row.ItemArray[8].ToString());

                    //seat_type_id
                    string seatType = row.ItemArray[5].ToString();
                    var seatTypeid = _db.tbl_seat_type.Where(x => x.SeatType == seatType).Select(y => y.Seat_type_id).FirstOrDefault();
                    ItemDetails.SeatsTypeId = seatTypeid;

                    //IsPPP
                    string tes = row.ItemArray[15].ToString();bool isPppSatatus = false;
                    if (tes == "Yes")
                        isPppSatatus = true;
                    ItemDetails.IsPPP = isPppSatatus;

                    //DualSystemTraining
                    string tes1 = row.ItemArray[17].ToString(); bool DualSystemStatus = false;
                    if (tes1 == "Yes")
                        DualSystemStatus = true;
                    ItemDetails.DualSystemTraining = DualSystemStatus;

                    //CreatedBy
                    ItemDetails.CreatedBy = userId;                    

                    //createdOn
                    ItemDetails.CreatedOn = DateTime.Now;
                    ItemDetails.Govt_Gia_seats =Convert.ToInt32(row.ItemArray[12].ToString());
                    ItemDetails.PPP_seats = Convert.ToInt32(row.ItemArray[13].ToString());
                    ItemDetails.Management_seats = Convert.ToInt32(row.ItemArray[14].ToString());
                    _db.tbl_ITI_trade_seat_master.Add(ItemDetails);
                    _db.SaveChanges();

                    tbl_ITI_trade_seat_trans tradeseat = new tbl_ITI_trade_seat_trans();
                    
                    var Trade_ITI_seat_Id = (from post in _db.tbl_ITI_trade_seat_master                                               
                                                orderby post.Trade_ITI_seat_Id descending
                                                select post.Trade_ITI_seat_Id).Take(1).FirstOrDefault();
                    tradeseat.Trade_ITI_seat_Id = Convert.ToInt32(Trade_ITI_seat_Id);
                    tradeseat.StatusId = 1;

                    tradeseat.Trans_Date = DateTime.Now;
                    tradeseat.CreatedBy = userId;
                    tradeseat.CreatedOn = DateTime.Now;
                    tradeseat.FlowId = 3;
                                   
                    _db.tbl_ITI_trade_seat_trans.Add(tradeseat);
                    _db.SaveChanges();
                }
                
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
               

        public List<MasterData> GetSeatAvailability()
        {
            try
            {
                var res = (from a in _db.tbl_iti_college_details
                           join b in _db.tbl_district_mast on a.district_id equals b.dist_id  
                           join c in _db.tbl_division_mast on b.division_id equals c.division_id
                           join d in _db.tbl_Institute_type on a.Insitute_TypeId equals d.Institute_type_id
                           select new MasterData
                           {
                               MISCode=a.MISCode,
                               DivisionId=b.division_id,
                               DivisionName=c.division_name,
                               DistrictCode=b.dist_id,
                               DistrictName=b.dist_name,
                               InstituteType=d.Institute_type,
                               //InstituteId=a.Insitute_TypeId,                               
                               InstituteName =a.iti_college_name
                               //,
                               //TradeId=,
                               //TradeName=,
                               //SeatsPerUnit=,
                               //Unit=,
                               //GovtGIASeats=,
                               //PPPSeats=,
                               //PrivateSeats=,
                               //ISPPP=,
                               //Shifts =,
                               //DualSystemTraining=,
                               //Status=,
                               //Remarks=,

                           }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CityDetails> GetDivisionTypes()
        {
            try
            {
                var res = (from a in _db.tbl_division_mast
                           where a.is_active == true
                           select new CityDetails
                           {
                               DivisionId = a.division_id,
                               DivisionName = a.division_name
                           }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CityDetails> GetDistricts(int divisionId)
        {
            try
            {
                var res = (from a in _db.tbl_district_mast
                           where a.is_active == true && a.division_id== divisionId
                           select new CityDetails
                           {
                               DistrictId = a.dist_id,
                               DistrictName = a.dist_name
                           }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CityDetails> GetTaluks(int districtId)
        {
            try
            {
                var res = (from a in _db.tbl_taluk_mast
                           where a.is_active == true && a.district_id== districtId
                           select new CityDetails
                           {
                               TalukId = a.taluk_id,
                               TalukName = a.taluk_name
                           }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MasterData> GetAailableSeats()
        {
            try
            {
                var res = (from a in _db.tbl_iti_college_details
                           join b in _db.tbl_district_mast on a.district_id equals b.dist_id
                           join c in _db.tbl_division_mast on b.division_id equals c.division_id
                           join d in _db.tbl_Institute_type on a.Insitute_TypeId equals d.Institute_type_id
                           join e in _db.tbl_ITI_trade_seat_master on a.iti_college_id equals e.Trade_ITI_Id
                           join f in _db.tbl_ITI_Trade on e.Trade_ITI_Id equals f.Trade_ITI_id
                           select new MasterData
                           {
                               MISCode = a.MISCode,
                               DivisionId = b.division_id,
                               DivisionName = c.division_name,
                               DistrictCode = b.dist_id,
                               DistrictName = b.dist_name,
                               InstituteType = d.Institute_type,
                               InstituteId=d.Institute_type_id,                               
                               InstituteName = a.iti_college_name,
                               TradeId=e.Trade_ITI_Id,
                               //TradeName =f.,
                               SeatsPerUnit =e.SeatsPerUnit,
                               Unit =a.Units,
                               GovtGIASeats =e.Govt_Gia_seats,
                               PPPSeats =e.PPP_seats,
                               //PrivateSeats =e.,
                               //ISPPP =e.IsPPP,
                               //Shifts =,
                               //DualSystemTraining =,
                               //Status =,
                               //Remarks =,

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
