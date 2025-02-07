using System;

namespace Order.Frontend.WebForms.Models
{
    public class DeliveryOrder
    {
        public int deliveryOrderId { get; set; }

        public int orderId { get; set; }
        

        public DateTime? deliveryDate { get; set; }
    }
}