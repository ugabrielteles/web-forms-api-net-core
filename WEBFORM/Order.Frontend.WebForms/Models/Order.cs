using System;

namespace Order.Frontend.WebForms.Models
{
    public class Order
    {
        public int orderId { get; set; }

        public string description { get; set; }

        public decimal? value { get; set; }

        public DateTime? createAt { get; set; }

        public string street { get; set; }

        public string zipCode { get; set; }

        public string number { get; set; }

        public string neighborhood { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public int orderStatusId { get; set; }

        public OrderStatus orderStatus { get; set; }

        public DeliveryOrder deliveryOrder { get; set; }
    }
}