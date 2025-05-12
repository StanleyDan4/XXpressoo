using System;
using System.Collections.Generic;
using System.Text;

namespace XPressoo.Models.Dtos
{
    public class BasketDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
