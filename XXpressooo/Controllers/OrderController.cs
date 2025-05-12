using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xpressoo.Models;
using XXpressooo.Data;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto dto)
    {
        if (dto.Items == null || !dto.Items.Any())
        {
            return BadRequest(new { success = false, message = "Корзина пустая" });
        }

        // Создаем заказ
        var order = new Order
        {
            UserId = dto.UserId,
            OrderDate = Convert.ToString(DateTime.UtcNow),
            TotalAmount = dto.Items.Sum(i => i.Quantity * GetProductPrice(i.ProductId))
        };

        await _context.Orders.AddAsync(order);

        // Удаляем товары из корзины
        var userBaskets = await _context.Baskets
            .Where(b => b.UserId == dto.UserId)
            .ToListAsync();

        _context.Baskets.RemoveRange(userBaskets);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, orderId = order.OrderId });
    }

    private decimal GetProductPrice(int productId)
    {
        return _context.Products
            .Where(p => p.ProductId == productId)
            .Select(p => p.Price)
            .FirstOrDefault();
    }
}

public class OrderDto
{
    public int UserId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}