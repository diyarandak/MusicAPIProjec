using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;
// Şifreleme kütüphanesi
using BCrypt.Net; 

namespace Net9Odev.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<List<UserResponseDto>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();
        return users.Select(u => new UserResponseDto(u.Id, u.FullName, u.Email, u.Role, u.CreatedAt)).ToList();
    }

    public async Task<int> RegisterAsync(UserRegisterDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new Exception("Bu e-posta zaten kayıtlı.");

        // DÜZELTME 1: Şifreyi Hash'leyerek kaydet
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var newUser = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Password = passwordHash, // Hashlenmiş şifre
            Role = request.Role
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser.Id;
    }

    public async Task<string?> LoginAsync(UserLoginDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        
        // DÜZELTME 2: Şifre kontrolünü Hash üzerinden yap
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserDto request)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.Role = request.Role;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user); // Soft delete otomatik çalışacak
        await _context.SaveChangesAsync();
        return true;
    }
}