using Net9Odev.DTOs;

namespace Net9Odev.Services;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllAsync();
    Task<int> RegisterAsync(UserRegisterDto request);
    Task<string?> LoginAsync(UserLoginDto request); // Token döner
    
    // Yeni eklediklerimiz:
    Task<bool> UpdateAsync(int id, UpdateUserDto request);
    Task<bool> DeleteAsync(int id);
}