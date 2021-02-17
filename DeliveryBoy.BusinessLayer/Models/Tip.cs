using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryBoy.BusinessLayer.Models
{
    public class Tip
    {
        public int Id { get; set; }
        public double GeoLat { get; set; }
        public double GeoLong { get; set; }
        public int NoTip { get; set; }
        public int LowTip { get; set; }
        public int GoodTip { get; set; }
        public int GreatTip { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
