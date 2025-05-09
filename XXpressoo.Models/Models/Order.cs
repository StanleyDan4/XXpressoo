using System;
using System.Collections.Generic;
using System.Text;

namespace XXpressoo.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public User User { get; set; }
    }
}
