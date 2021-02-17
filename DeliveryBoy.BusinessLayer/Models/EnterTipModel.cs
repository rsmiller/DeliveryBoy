using System;

namespace DeliveryBoy.Models
{
    public class EnterTipModel
    {
        public string Ip { get; set; }
        public double GeoLat { get; set; }
        public double GeoLong { get; set; }

        public bool Good { get; set; } = false;
        public bool Bad { get; set; } = false;
        public bool Nuetral { get; set; } = false;

        public bool NoTip { get; set; } = false;
        public bool LowTip { get; set; } = false;
        public bool GoodTip { get; set; } = false;
        public bool GreatTip { get; set; } = false;
    }
}
