using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xpressoo.Models;

using XXpressooo.Data;
using XXpressooo.Dtos;

namespace XXpressoo.Api.Controllers
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

        [HttpPost]
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
                    message = "Email уже используется"
                });
            }

            try
            {
                var user = new User
                {
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Password = dto.Password,
                    
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    userId = user.UserId,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Внутренняя ошибка сервера",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("exists")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            bool exists = await _context.Users.AnyAsync(u => u.Email == email);
            return Ok(exists);
        }

        private string HashPassword(string password)
        {
            // В будущем замените на BCrypt / IdentityHasher
            return password;
        }
    }
}