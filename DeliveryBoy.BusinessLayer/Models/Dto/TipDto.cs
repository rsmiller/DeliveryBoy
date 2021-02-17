using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryBoy.BusinessLayer.Models.Dto
{
    public class TipDto
    {
        public decimal NoTipPercentage { get; set; }
        public decimal LowTipPercentage { get; set; }
        public decimal GoodTipPercentage { get; set; }
        public decimal GreatTipPercentage { get; set; }
    }
}
