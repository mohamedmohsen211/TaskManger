using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManger.API.Data;
using TaskManger.API.DTOs;
using TaskManger.API.Models;
using TaskManger.API.Services;
using System.Security.Cryptography;
using System.Text;

namespace TaskManger.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AppDpContext appDpContext;
        private readonly JwtService jwtService;
        public AuthController(AppDpContext appDpContext, JwtService jwtService)
        {
            this.appDpContext = appDpContext;
            this.jwtService = jwtService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await appDpContext.Users.AnyAsync(u => u.Email == dto.Email))

                return BadRequest("Email is already taken");

            var user = new User
            {
                Username = dto.UserName,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password)
            };
            appDpContext.Users.Add(user);
            await appDpContext.SaveChangesAsync();
            return Ok("Registared Succesfully");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await appDpContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");
            var token = jwtService.GenerateToken(user);
            return Ok(new { token });
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private static bool VerifyPassword(string password, string passwordHash)
        {
            return HashPassword(password) == passwordHash;

        }
    }
}
