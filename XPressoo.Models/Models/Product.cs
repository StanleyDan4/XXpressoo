using System;
using System.Collections.Generic;
using System.Text;

namespace Xpressoo.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public string Image { get; set; } // Можно хранить URL или путь

        public ProductCategory Category { get; set; }
    }
}
