using DLL.DBConnection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SqlBulkTools;
using System.Transactions;
using System.Configuration;
using System.Data;
using System.Web.Mvc;

namespace DLL
{
    public class ExaminationDLL : IExaminationDLL
    {
        private readonly DbConnection _db = new DbConnection();
        public class av
        {
            public int Id { get; set; }
            public string UIN { get; set; }
        }

        //Get Course List
        public SelectList GetExamCenterListDLL()
        {
            List<SelectListItem> ExamCenterList = _db.tbl_exam_centers.Where(x=>x.ec_is_active == true).Select(x => new SelectListItem
            {
                Text = x.ec_name.ToString(),
                Value = x.ec_id.ToString()
            }).ToList();
            var CenterList = new SelectListItem()
            {

                Value = null,
                Text = "Select Exam Center",

            };
            ExamCenterList.Insert(0, CenterList);
            return new SelectList(ExamCenterList, "Value", "Text");
        }


        public string GenerateUniqueCodeForTraineeDLL(Examination model)
        {
            List<av> res = (from g in _db.tbl_trainee_fee_paid.AsEnumerable()
                            join f in _db.tbl_unique_trainee_mast on g.tfp_id equals f.ut_tfp_id
                            where g.fee_paid_status == true
                            select new av
                            {
                                Id = g.tfp_id,
                                UIN = "",
                            }).ToList();

            for (int i = 0; i < res.Count; i++)
            {
                StringBuilder builder = new StringBuilder();
                Enumerable
                    .Range(65, 26)
                    .Select(e => ((char)e).ToString())
                    .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                    .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                    .OrderBy(e => Guid.NewGuid())
                    .Take(8)
                    .ToList().ForEach(e => builder.Append(e));

                res[i].UIN = builder.ToString();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(Int32));
            dt.Columns.Add("UIN", typeof(String));
            foreach (var r in res)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = r.Id;
                dr["UIN"] = r.UIN;

                dt.Rows.Add(dr);
            }

            var bulk = new BulkOperations();
            using (TransactionScope trans = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager
                .ConnectionStrings["DbConnection"].ConnectionString))
                {
                    using (var command = new SqlCommand("", conn))
                    {
                        conn.Open();

                        // Creating temp table on database
                        command.CommandText = "CREATE TABLE #TmpTable([Id] [INT] NOT NULL, [UIN] [VARCHAR](15) NULL)";
                        command.ExecuteNonQuery();

                        using (var bulkcopy = new SqlBulkCopy(conn))
                        {
                            bulkcopy.BulkCopyTimeout = 660;
                            bulkcopy.DestinationTableName = "#TmpTable";
                            bulkcopy.WriteToServer(dt);
                            bulkcopy.Close();
                        }

                        // Updating destination table, and dropping temp table
                        command.CommandTimeout = 300;
                        command.CommandText = "UPDATE utm SET utm.unique_identification_code = Temp.UIN " +
                                              "FROM tbl_unique_trainee_mast utm " +
                                              "INNER JOIN #TmpTable Temp ON (Temp.Id = utm.ut_tfp_id) " +
                                              "DROP TABLE #TmpTable";
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                trans.Complete();
            }
            return "Success";
        }

        public string GetSubjectAndTradeDLL(string ExamDate)
        {
            DateTime dateTime = Convert.ToDateTime(ExamDate);
            tbl_exam_subject_trans tbl_exam_subject_trans = _db.tbl_exam_subject_trans.Where(x => x.est_exam_date == dateTime).FirstOrDefault();
            if (tbl_exam_subject_trans != null)
            {
                tbl_exam_subject_type_mast tbl_exam_subject_type_mast = _db.tbl_exam_subject_type_mast.Where(x=>x.exam_subject_type_id == tbl_exam_subject_trans.est_exam_subject_type_id).FirstOrDefault();
                if(tbl_exam_subject_type_mast != null)
                {
                    return tbl_exam_subject_type_mast.exam_subject_type_desc.Trim()+"-"+ tbl_exam_subject_type_mast.exam_subject_type_id;
                }
            }
            return "Success";
        }

        public string GeneratePackingSlipDLL(PackingSlip model)
        {
            var traineePaid = (from tr in _db.tbl_iti_trainees_details
                               join pd in _db.tbl_trainee_fee_paid on tr.trainee_id equals pd.trainee_id
                               where pd.fee_paid_status == true
                               select new
                               {
                                   TraineeId = tr.trainee_id,
                               }).ToList();

            string PackingSlipnum = string.Empty;
            int ActualtraineeInSheet = 21;
            if (traineePaid.Count() > 0)
            {
                if (traineePaid.Count() < ActualtraineeInSheet)
                {
                    PackingSlipnum = GeneratePackingSlipNo(model);

                    tbl_packaging_slip_mast tbl_packaging_slip_mast = _db.tbl_packaging_slip_mast.Where(x => x.packing_slip_code == PackingSlipnum).FirstOrDefault();
                    if (tbl_packaging_slip_mast != null)
                    {
                        int inc = 0;
                        foreach (var r in traineePaid)
                        {
                            inc++;
                            tbl_packaging_slip_trans tbl_packaging_slip_trans = new tbl_packaging_slip_trans();
                            tbl_packaging_slip_trans.ps_id = tbl_packaging_slip_mast.ps_id;
                            tbl_packaging_slip_trans.trainee_id = r.TraineeId;
                            tbl_packaging_slip_trans.slno = inc;
                            tbl_packaging_slip_trans.ps_is_active = true;
                            tbl_packaging_slip_trans.ps_created_by = model.user_id;
                            tbl_packaging_slip_trans.ps_created_on = DateTime.Now;
                            _db.tbl_packaging_slip_trans.Add(tbl_packaging_slip_trans);
                            _db.SaveChanges();
                        }
                    }
                }

                if (traineePaid.Count() > ActualtraineeInSheet)
                {
                    int RemainingCount = 0;
                    int LastRow = 0;
                    int inc = 0;
                    for (int i = 0; i < traineePaid.Count(); i++)
                    {
                        if (i > 0 && i % ActualtraineeInSheet == 0)
                        {
                            PackingSlipnum = GeneratePackingSlipNo(model);
                            RemainingCount = traineePaid.Count() - i;
                            
                            tbl_packaging_slip_mast tbl_packaging_slip_mast = _db.tbl_packaging_slip_mast.Where(x => x.packing_slip_code == PackingSlipnum).FirstOrDefault();
                            if (tbl_packaging_slip_mast != null)
                            {
                                
                                for (int j = LastRow; j < i; j++)
                                {
                                    inc++;
                                    tbl_packaging_slip_trans tbl_packaging_slip_trans = new tbl_packaging_slip_trans();
                                    tbl_packaging_slip_trans.ps_id = tbl_packaging_slip_mast.ps_id;
                                    tbl_packaging_slip_trans.trainee_id = traineePaid[j].TraineeId;
                                    tbl_packaging_slip_trans.slno = inc;
                                    tbl_packaging_slip_trans.ps_is_active = true;
                                    tbl_packaging_slip_trans.ps_created_by = model.user_id;
                                    tbl_packaging_slip_trans.ps_created_on = DateTime.Now;
                                    _db.tbl_packaging_slip_trans.Add(tbl_packaging_slip_trans);
                                    _db.SaveChanges();
                                    LastRow = i;
                                }
                            }
                        }

                        if (RemainingCount < ActualtraineeInSheet && RemainingCount > 0)
                        {
                            PackingSlipnum = GeneratePackingSlipNo(model);

                            tbl_packaging_slip_mast tbl_packaging_slip_mast = _db.tbl_packaging_slip_mast.Where(x => x.packing_slip_code == PackingSlipnum).FirstOrDefault();
                            if (tbl_packaging_slip_mast != null)
                            {
                                LastRow++;
                                for (int k = LastRow - 1; k < traineePaid.Count(); k++)
                                {
                                    inc++;
                                    tbl_packaging_slip_trans tbl_packaging_slip_trans = new tbl_packaging_slip_trans();
                                    tbl_packaging_slip_trans.ps_id = tbl_packaging_slip_mast.ps_id;
                                    tbl_packaging_slip_trans.trainee_id = traineePaid[k].TraineeId;
                                    tbl_packaging_slip_trans.slno = inc;
                                    tbl_packaging_slip_trans.ps_is_active = true;
                                    tbl_packaging_slip_trans.ps_created_by = model.user_id;
                                    tbl_packaging_slip_trans.ps_created_on = DateTime.Now;
                                    _db.tbl_packaging_slip_trans.Add(tbl_packaging_slip_trans);
                                    _db.SaveChanges();
                                }
                            }
                            break;
                        }
                    }
                }
            }

            return "Success";
        }

        public string GeneratePackingSlipNo(PackingSlip model)
        {
            var PackingslipCount = (from purchase in _db.tbl_packaging_slip_mast select purchase).Count();
            string packingSlipNo = string.Empty;
            string packSlipNo = string.Empty;
            if (PackingslipCount > 0)
            {
                packingSlipNo = ((from pk in _db.tbl_packaging_slip_mast
                                  orderby pk.ps_id descending
                                  select pk.ps_id).FirstOrDefault() + 1).ToString();
            }
            else
            {
                packingSlipNo = "1";
            }

            var yyyy = DateTime.Now.Year;
            tbl_exam_centers tbl_exam_centers = _db.tbl_exam_centers.Where(x => x.ec_id == model.ExamCenterId).FirstOrDefault();
            if (tbl_exam_centers != null)
            {
                packingSlipNo = yyyy + "-" + tbl_exam_centers.ec_code + "-" + packingSlipNo;
            }

            tbl_packaging_slip_mast tbl_packaging_slip_mast = new tbl_packaging_slip_mast();
            tbl_packaging_slip_mast.packing_slip_code = packingSlipNo;
            tbl_packaging_slip_mast.ps_center_id = model.ExamCenterId;
            tbl_packaging_slip_mast.ps_subject_id = model.SubjectType;
            tbl_packaging_slip_mast.ps_trade_id = model.TradeId;
            tbl_packaging_slip_mast.ps_created_by = model.user_id;
            tbl_packaging_slip_mast.ps_is_active = true;
            tbl_packaging_slip_mast.ps_creation_datetime = DateTime.Now;
            _db.tbl_packaging_slip_mast.Add(tbl_packaging_slip_mast);
            _db.SaveChanges();


            return packingSlipNo;
        }

        public SelectList GetDisticListDLL(int? DivId)
        {
            List<SelectListItem> _ListItems = new List<SelectListItem>();
            _ListItems = (from dist in _db.tbl_district_mast.AsEnumerable()
                          join d in _db.tbl_division_mast on dist.division_id equals d.division_id
                          where d.division_id == DivId
                          select new SelectListItem
                          {
                              Text = dist.dist_name,
                              Value = dist.dist_id.ToString()
                              //Value=PD.proj_dtls_id.ToString(),
                          }).ToList();




            return new SelectList(_ListItems, "Value", "Text");
        }

        public SelectList DisticBasedCentersITIClgDLL(int? distic_Id,int? DivId)
        {
            List<SelectListItem> _ListItems = new List<SelectListItem>();
            _ListItems = (from dist in _db.tbl_district_mast.AsEnumerable()
                          join d in _db.tbl_division_mast on dist.division_id equals d.division_id
                         // join EC in _db.tbl_exam_centers on 


                          where d.division_id == DivId && dist.dist_id == distic_Id
                          select new SelectListItem
                          {
                              Text = dist.dist_name,
                              Value = dist.dist_id.ToString()
                              //Value=PD.proj_dtls_id.ToString(),
                          }).ToList();




            return new SelectList(_ListItems, "Value", "Text");
        }
    }
}
