using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string? Description { get; set; }

        public decimal? Value { get; set; }

        public DateTime? CreateAt { get; set; }

        public string? Street { get; set; }

        public string? ZipCode { get; set; }

        public string? Number { get; set; }

        public string? Neighborhood { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public int OrderStatusId { get; set; }

        public OrderStatus? OrderStatus { get; set; }
        public DeliveryOrder? DeliveryOrder { get; set; }
    }
}