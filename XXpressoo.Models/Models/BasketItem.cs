﻿namespace XXpressoo.Models
{
    public class BasketItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

        public BasketItem()
        {
            Name = string.Empty;
            ImageUrl = string.Empty;
        }
    }
}