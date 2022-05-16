using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Tentative_admsn_eventDetails
    {
        [Key]
        public int Calendar_EventId { get; set; }
        public int Tentative_admsn_evnt_clndr_Id { get; set; }
        public DateTime? FromDt_ApplyingOnlineApplicationForm { get; set; }
        public DateTime? ToDt_ApplyingOnlineApplicationForm { get; set; }
        public DateTime? FromDt_DocVerificationPeriod { get; set; }
        public DateTime? ToDt_DocVerificationPeriod { get; set; }
        public DateTime? Dt_DisplayEigibleVerifiedlist { get; set; }
        public DateTime? Dt_DBBackupSeatMatrixFInalByDept { get; set; }
        public DateTime? Dt_DisplayTentativeGradation { get; set; }
        public DateTime? Dt_DisplayFinalGradationList { get; set; }
        public DateTime? Dt_1stListSeatAllotment { get; set; }
        public DateTime? FromDt_1stRoundAdmissionProcess { get; set; }
        public DateTime? ToDt_1stRoundAdmissionProcess { get; set; }
        public DateTime? Dt_1stListAdmittedCand { get; set; }
        public DateTime? FromDt_2ndRoundEntryChoiceTrade { get; set; }
        public DateTime? ToDt_2ndRoundEntryChoiceTrade { get; set; }
        public DateTime? FromDt_DbBkp2ndRoundOnlineSeat { get; set; }
        public DateTime? ToDt_DbBkp2ndRoundOnlineSeat { get; set; }
        public DateTime? FromDt_2ndRoundAdmissionProcess { get; set; }
        public DateTime? ToFDt_2ndRoundAdmissionProcess { get; set; }
        public DateTime? Dt_2ndAdmittedCand { get; set; }
        public DateTime? FromDt_3rdRoundEntryChoiceTrade { get; set; }
        public DateTime? ToDt_3rdRoundEntryChoiceTrade { get; set; }
        public DateTime? Dt_DbBkp3rdRoundOnlineSeat { get; set; }
        public DateTime? FromDt_3rdRoundAdmissionProcess { get; set; }
        public DateTime? ToDt_3rdRoundAdmissionProcess { get; set; }
        public DateTime? Dt_3rdAdmittedCand { get; set; }
        public DateTime? FromDt_FinalRoundEntryChoiceTrade { get; set; }
        public DateTime? ToDt_FinalRoundEntryChoiceTrade { get; set; }
        public DateTime? Dt_FinalRoundSeatAllotment { get; set; }
        public DateTime? FromDt_AdmissionFinalRound { get; set; }
        public DateTime? ToDt_AdmissionFinalRound { get; set; }
        public DateTime? Dt_CommencementOfTraining { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? FromDt_GrievanceTentativeGradationList { get; set; }
        public DateTime? FromDt_TntGrlistbyAppl { get; set; }
        public DateTime? ToDt_TntGrlistbyAppl { get; set; }
        public DateTime? FromDt_ApplDocVerif { get; set; }
        public DateTime? ToDt_ApplDocVerif { get; set; }
        public DateTime? FromDt_AplGrVRoffcr { get; set; }
        public DateTime? ToDt_AplGrVRoffcr { get; set; }
        public DateTime? Dt_GradationList2round { get; set; }
        public DateTime? Publishgrdlist3Rnd { get; set; }
        public DateTime? FromDt_Tentatviveseat { get; set; }
        public DateTime? Dt_Notification { get; set; }





    }
}
