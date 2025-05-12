using System;
using System.Collections.Generic;
using System.Text;

namespace XPressoo.Models.Dtos
{
    public class ReviewDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; } = 1;
        public int Rating { get; set; }
        public string ReviewText { get; set; }
    }
}
