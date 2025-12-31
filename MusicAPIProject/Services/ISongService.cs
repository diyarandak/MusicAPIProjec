using Net9Odev.DTOs;
namespace Net9Odev.Services;

public interface ISongService
{
    Task<List<SongResponseDto>> GetAllAsync();
    Task<SongResponseDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateSongDto request);
    Task<bool> UpdateAsync(int id, UpdateSongDto request);
    Task<bool> DeleteAsync(int id);
}