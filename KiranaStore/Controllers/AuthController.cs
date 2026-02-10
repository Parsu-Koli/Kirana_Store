using BLL.DTOs;
using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KiranaStore.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid login data.");

            try
            {
                var user = _authService.Login(dto.Username, dto.Password);

                var token = GenerateJwtToken(user);

                return Ok(new
                {
                    user.UserId,
                    user.Username,
                    user.Role,
                    Token = token
                });
            }
            catch (Exception ex)
            {

                return Unauthorized(ex.Message);
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid registration data.");

            var user = new User
            {
                FullName = dto.FullName,
                Username = dto.Username,
                Password = dto.Password,
                Phone = dto.Phone,
                Role = dto.Role,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            try
            {
                var result = _authService.Register(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ================= JWT METHOD =================
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("UserId", user.UserId.ToString())
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
