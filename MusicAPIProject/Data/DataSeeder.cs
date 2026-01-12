using Net9Odev.Entities;
using BCrypt.Net; // Şifreleme için gerekli

namespace Net9Odev.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Veritabanı boşsa veri ekle
        if (!context.Users.Any())
        {
            var adminUser = new User
            {
                FullName = "Admin User",
                Email = "admin@music.com",
                // KRİTİK DÜZELTME: Şifreyi hashleyerek kaydediyoruz
                Password = BCrypt.Net.BCrypt.HashPassword("123"), 
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };
            
            var normalUser = new User
            {
                FullName = "Normal User",
                Email = "user@music.com",
                // KRİTİK DÜZELTME: Şifreyi hashleyerek kaydediyoruz
                Password = BCrypt.Net.BCrypt.HashPassword("123"),
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(adminUser, normalUser);
            await context.SaveChangesAsync();
        }
    }
}