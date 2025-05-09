using Microsoft.EntityFrameworkCore;
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

app.Run();