using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XXpressooo.Data;
using Xpressoo.Models;
using XXpressoo.Models.Dtos;

namespace XXpressooo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return NotFound(new { success = false, message = "Пользователь не найден" });

            if (user.Password != dto.Password) // ⚠️ Только для тестирования! Используйте хэширование!
                return Unauthorized(new { success = false, message = "Неверный пароль" });

            return Ok(user);
        }

        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // POST /api/users
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Некорректные данные",
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return Conflict(new
                {
                    success = false,
                    message = "Пользователь с таким email уже существует"
                });
            }

            var user = new User
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Password = dto.Password
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                userId = user.UserId
            });
        }

        // GET /api/users/exists?email=...
        [HttpGet("exists")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Email не указан"
                });
            }

            bool exists = await _context.Users.AnyAsync(u => u.Email == email);
            return Ok(new
            {
                success = true,
                exists
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Некорректные данные"
                });
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Пользователь не найден"
                });
            }

            // Обновляем только разрешенные поля
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            // Пароль обновляем только если он предоставлен
            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.Password = dto.Password; // В реальном приложении здесь должно быть хеширование
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = true,
                    user = new
                    {
                        user.UserId,
                        user.Email,
                        user.FirstName,
                        user.LastName
                        // Не возвращаем пароль
                    }
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new
                {
                    success = false,
                    message = "Ошибка при обновлении данных"
                });
            }
        }
    }
}