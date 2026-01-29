using GameZoneErp.Server.Data;
using GameZoneErp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameZoneErp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GameZoneDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(GameZoneDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto login)
        {
            // Simple password check for demo - in production, use hashed passwords!
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == login.Username);

            if (user == null || user.PasswordHash != login.Password) // Insecure comparison, demo only
            {
                return Unauthorized("Invalid credentials");
            }

            var token = GenerateJwtToken(user);
            return Ok(new LoginResponseDto { Token = token });
        }

        private string GenerateJwtToken(GameZoneErp.Shared.Entities.User user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "super_secret_key_12345_make_it_longer"; // Fallback for demo
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"] ?? "GameZoneErp",
                _configuration["Jwt:Audience"] ?? "GameZoneErp",
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
