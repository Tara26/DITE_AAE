using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class AutoAllocation
    {
        public int ApplicantId { get; set; }
        public int Rank { get; set; }
        public int? Gender { get; set; }
        public int? VCategory { get; set; }
        public int Caste { get; set; }
        public int? Religion { get; set; }
        public int? Minority { get; set; }
        public bool PhysicalHandicap { get; set; }
        public int HorizontalCat { get; set; }
        public int VerticalCat { get; set; }
        public int GeneralPool { get; set; }
        public int HCategory { get; set; }
        public int SeatMaxId { get; set; }
        public int AllocationId { get; set; }
        public bool? ExServiceMan { get; set; }
        public bool? KanndaMedium { get; set; }
        public bool? EconomyWeakerSection { get; set; }
        public bool? HyderabadKarnataka { get; set; }
    }
}
