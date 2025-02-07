using System;
using System.Collections.Generic;

namespace Order.Frontend.WebForms.Models
{
    public class OrderStatus
    {
        public int orderStatusId { get; set; }

        public string name { get; set; }

        public bool? active { get; set; }
        
        public DateTime? createAt { get; set; }        

        public List<Models.Order> orders { get; set; }
    }
}