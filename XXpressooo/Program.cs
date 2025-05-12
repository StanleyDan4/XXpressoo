using Microsoft.EntityFrameworkCore;
using Xpressoo.Models;
using XPressoo.Models.Dtos;
using XXpressoo.Models.Dtos;
using XXpressooo.Data;

var builder = WebApplication.CreateBuilder(args);

// Подключаем EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);

builder.Services.AddControllers();

var app = builder.Build();
builder.WebHost.UseUrls("http://0.0.0.0:5000", "https://0.0.0.0:5001");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});
app.MapGet("/api/users/{id}", async (int id, AppDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user == null)
    {
        return Results.NotFound();
    }
    return Results.Json(user);
});
app.MapPost("/api/reviews", async (ReviewDto dto, AppDbContext db) =>
{
    var review = new Review
    {
        UserId = dto.UserId,
        ProductId = dto.ProductId,
        Rating = dto.Rating,
        ReviewText = dto.ReviewText,
        ReviewDate = DateTime.Now
    };
    await db.Reviews.AddAsync(review);
    await db.SaveChangesAsync();
    return Results.Json(review);
});
app.MapPut("/api/users/update-profile", async (UserDto dto, AppDbContext db) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
    if (user == null)
    {
        return Results.NotFound();
    }

    user.FirstName = dto.FirstName;
    user.LastName = dto.LastName;

    await db.SaveChangesAsync();

    return Results.Json(user);
});
app.Run();