namespace Net9Odev.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // Şifre
    public string Role { get; set; } = "User"; // Admin veya User olabilir
}