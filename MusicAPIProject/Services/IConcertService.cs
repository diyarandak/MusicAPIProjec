using Net9Odev.DTOs;
namespace Net9Odev.Services;

public interface IConcertService
{
    Task<List<ConcertResponseDto>> GetAllAsync();
    Task<ConcertResponseDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateConcertDto request);
    Task<bool> UpdateAsync(int id, UpdateConcertDto request);
    Task<bool> DeleteAsync(int id);
}