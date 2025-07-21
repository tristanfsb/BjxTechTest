using System.Security.Claims;
using System.Text;
using SharedLib.DTOs;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using static BCrypt.Net.BCrypt;

namespace BusinessLogic.Services;

public class AuthService
{
    private readonly string _secret;
    private readonly TimeSpan _tokenLifeSpan = TimeSpan.FromMinutes(30);
    private readonly BjxDbContext _context;
    private readonly ILogger _logger;

    public AuthService(IConfiguration config, ILogger<AuthService> logger, BjxDbContext context)
    {
        _secret = config["Jwt:Key"]!;
        _logger = logger;
        _context = context;
    }

    public string GenerateToken(int userId, string email, string role)
    {
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.Add(_tokenLifeSpan),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<User?> Login(LoginDTO loginDto)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            
            if (user == null || !Verify(loginDto.Password!, user.Password))
            {
                return null;
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError($"[ERROR]: {ex.Message} - {ex.StackTrace}");
            return null;
        }
    }
}
