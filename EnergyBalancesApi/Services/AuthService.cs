using EnergyBalancesApi.Data;
using EnergyBalancesApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using EnergyBalancesApi.Models.Dto;

public class AuthService : IAuthService
{
    private readonly EnergyDbContext _db;
    private readonly IConfiguration _cfg;

    public AuthService(EnergyDbContext db, IConfiguration cfg)
    {
        _db = db;
        _cfg = cfg;
    }

    public async Task<User> Register(UserRegisterDto dto)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var user = new User
        {
            Username = dto.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            PasswordSalt = hmac.Key
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<string?> Login(UserLoginDto dto)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
        if (user == null) return null;
        using var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt);
        var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
        if (!computed.SequenceEqual(user.PasswordHash)) return null;

        // Generowanie tokenu
        var jwtSettings = _cfg.GetSection("JwtSettings");
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"]!)),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
