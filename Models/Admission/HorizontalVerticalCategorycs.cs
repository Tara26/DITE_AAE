using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class HorizontalVerticalCategorycs
    {
        public int Horizontal_rules_id { get; set; }
        public int Vertical_rules_id { get; set; }
        public string Horizontal_rules { get; set; }
        public string Vertical_Rules { get; set; }
        public bool IsActive { get; set; }
        public int TradeName { get; set; }
        public int s_id { get; set; }
        public string shifts { get; set; }
        public int u_id { get; set; }
        public string units { get; set; }
        public string TradeNameDet { get; set; }
    }
}
