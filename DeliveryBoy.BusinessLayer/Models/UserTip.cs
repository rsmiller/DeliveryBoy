using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryBoy.BusinessLayer.Models
{
    public class UserTip
    {
        public int Id { get; set; }
        public int TipId { get; set; }
        public string Ip { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
