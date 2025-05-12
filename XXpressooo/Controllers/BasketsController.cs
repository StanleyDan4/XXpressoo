using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XXpressooo.Data;
using Xpressoo.Models;
using XPressoo.Models.Dtos;

namespace XXpressooo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BasketsController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/baskets?userId=...
        [HttpGet]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cartItems = await _context.Baskets
                .Where(b => b.UserId == userId)
                .Join(
                    _context.Products,
                    basket => basket.ProductId,
                    product => product.ProductId,
                    (basket, product) => new BasketItem
                    {
                        ProductId = product.ProductId,
                        Name = product.ProductName,
                        Price = product.Price,
                        Quantity = basket.Quantity,
                        ImageUrl = product.Image
                    })
                .ToListAsync();

            return Ok(cartItems);
        }

        // POST /api/baskets
        [HttpPost]
        public async Task<IActionResult> AddToBasket([FromBody] BasketDto dto)
        {
            var basketItem = new Basket
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await _context.Baskets.AddAsync(basketItem);
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        // PUT /api/baskets
        [HttpPut]
        public async Task<IActionResult> UpdateCartItem([FromBody] BasketDto dto)
        {
            var item = await _context.Baskets
                .FirstOrDefaultAsync(b => b.UserId == dto.UserId && b.ProductId == dto.ProductId);

            if (item == null)
                return NotFound(new { success = false, message = "Товар не найден в корзине" });

            item.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        // POST /api/baskets/checkout
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(int userId)
        {
            var cart = await _context.Baskets.Where(b => b.UserId == userId).ToListAsync();

            if (cart.Count == 0)
                return BadRequest(new { success = false, message = "Корзина пустая" });

            var order = new Order
            {
                UserId = userId,
                OrderDate = "2025 - 05 - 08 12:06:53",
                TotalAmount = cart.Sum(c => c.Quantity * _context.Products.Find(c.ProductId)?.Price ?? 0m)
            };

            await _context.Orders.AddAsync(order);
            _context.Baskets.RemoveRange(cart);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, orderId = order.OrderId });
        }
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromCart([FromBody] BasketDto dto)
        {
            var basketItem = await _context.Baskets
                .FirstOrDefaultAsync(b => b.UserId == dto.UserId && b.ProductId == dto.ProductId);

            if (basketItem == null)
                return NotFound(new { success = false, message = "Товар не найден в корзине" });

            _context.Baskets.Remove(basketItem);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }
}