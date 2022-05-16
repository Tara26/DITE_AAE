using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Affiliation
{
   public class CsystemType
    {
        public enum getCommon
        {
            //Status
            Upload = 1,
            Approved = 2,
            Rejected = 3,
            Sent_back = 4,
            Submitted = 5,
            Published=6,
            Review_and_Recommend = 7,
            Sent_for_Review=8,
            Sent_for_Correction=9,
            pub=19,

            //Roles
            OFFICE_SUPERINTENDENT = 7,
            Assistant_Director=3,
            Deputy_Director=5,
            Joint_Director=4,
            Additional_Director=6,
            Director=2,
            Commissioner=9,
            Tabulator=10,
            //Scrutinizer=11,
            //Chief_Tabulator=12,
            CaseWorker=8,
            ITIAdmin=9,
            // College_Login=15
            CaseWorkerDiv = 17,
            Office_Supeintendent_Div=13,
            Assistant_Director_Div = 14,
            Deputy_Director_Div = 15,
            //Flag for Operation Performed
            New_Trade = 1,
            New_Units = 2,
            Deactivate = 3,
            Activate = 4,
            Deaffiliate = 5,
            Affiliate = 6,

            //Status For operation Performed
          
        }
    }
}
