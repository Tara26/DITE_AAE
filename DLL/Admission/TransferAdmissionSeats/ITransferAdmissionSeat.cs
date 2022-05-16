using Models.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Admission.TransferAdmissionSeats
{
    public interface ITransferAdmissionSeat
    {
        List<ApplicantTransferModel> GetAdmittedData(int loginId, int session, int course, int? trade, int round);
        List<ApplicantTransferModel> GetRequestedDetails(int loginId, int session, int course, int trade);
        List<Trades> GetTrades(int loginId);
        string GetMisCode(int instiId);
        List<Trades> GetAvailableseatsTrades(int instiId);
        List<Trades> GetUnits(int instiId, int trade);
        List<Trades> GetShifts(int instiId, int trade,int unit);
        string GetDualSystem(int insti, int trade);
        List<Trades> GetTranseferInstitutes(int type, int taluklgd);
        string SubmitAdmittedData(List<ApplicantTransferModel> seat, int loginId, int roleId);
        List<Trades> GetInstituteTypes();
        List<Trades> GetInstituteNames(int type);
        bool ForwardAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId, int flowId);
        List<ApplicantTransferModel> GetTransferRemarks(int seatId);
        bool ApproveAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId);
        bool SendBackAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId);
        List<ApplicantTransferModel> GetAdmittedDataStatus(int loginId,int roleId);
        List<ApplicantTransferModel> GetApplicantTransferbyList(int loginId,int roleId);
        List<ApplicantTransferModel> GetApprovedTransferbyList(int loginId, int roleId);
        List<ApplicantTransferModel> GetApplicantInstituteDetails(int id);
        bool UpdateApplicantInstituteDetails(ApplicantTransferModel tran, int roleId);
        List<SeatDetails> SeatTransferStatus(int roleId);
    }
}
