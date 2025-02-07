using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class DeliveryOrder
    {
        public int DeliveryOrderId { get; set; }

        public int OrderId { get; set; }

        public Order? Order { get; set; }

        public DateTime? DeliveryDate { get; set; }
    }
}