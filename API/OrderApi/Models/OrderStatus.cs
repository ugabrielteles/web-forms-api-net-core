using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class OrderStatus
    {
        public int OrderStatusId { get; set; }

        public string? Name { get; set; }

        public bool? Active { get; set; }

        [NotMapped]
        public int? OrderId { get; set; }

        public DateTime? CreateAt { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}