﻿using System;
using System.Collections.Generic;
using System.Text;

namespace XXpressoo.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }

        public Order Order { get; set; }
    }
}
