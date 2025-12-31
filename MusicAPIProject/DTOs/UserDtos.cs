namespace Net9Odev.DTOs;

// 1. Listeleme (Şifresiz)
public record UserResponseDto(
    int Id,
    string FullName,
    string Email,
    string Role,
    DateTime CreatedAt
);

// 2. Kayıt Olma
public record UserRegisterDto(
    string FullName,
    string Email,
    string Password,
    string Role = "User"
);

// 3. Giriş Yapma
public record UserLoginDto(
    string Email,
    string Password
);

// 4. Güncelleme (BU EKSİKTİ - Hata bundan kaynaklanıyor)
public record UpdateUserDto(
    string FullName,
    string Email,
    string Role
);