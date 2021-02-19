using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryBoy.BusinessLayer.Models
{
    public class TipDetail
    {
        public int Id { get; set; }
        public double GeoLat { get; set; }
        public double GeoLong { get; set; }
        public bool NoTip { get; set; }
        public bool LowTip { get; set; }
        public bool GoodTip { get; set; }
        public bool GreatTip { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
