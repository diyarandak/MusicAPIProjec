using Net9Odev.DTOs;

namespace Net9Odev.Services;

public interface IArtistService
{
    // 1. Listeleme
    Task<List<ArtistResponseDto>> GetAllArtistsAsync();
    
    // 2. Detay Getirme
    Task<ArtistResponseDto?> GetArtistByIdAsync(int id);

    // 3. Ekleme
    Task<int> AddArtistAsync(CreateArtistDto request);
    
    // 4. Güncelleme
    Task<bool> UpdateArtistAsync(int id, UpdateArtistDto request);
    
    // 5. Silme
    Task<bool> DeleteArtistAsync(int id);
}