using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Logs
{
    public class LogFile
    {
        public string _date { get; set; }
        public string _request { get; set; }
        public string _sourcefile { get; set; }
        public string _message { get; set; }
        public string _exception { get; set; }
        public string _stacktrace { get; set; }
    }
}
