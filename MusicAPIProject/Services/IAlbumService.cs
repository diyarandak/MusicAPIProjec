using Net9Odev.DTOs;

namespace Net9Odev.Services;

public interface IAlbumService
{
    // 1. Hepsini Listele
    Task<List<AlbumResponseDto>> GetAllAsync();
    
    // 2. Tek Albüm Getir (Yeni)
    Task<AlbumResponseDto?> GetByIdAsync(int id);
    
    // 3. Ekle
    Task<int> CreateAsync(CreateAlbumDto request);
    
    // 4. Güncelle (Yeni)
    Task<bool> UpdateAsync(int id, UpdateAlbumDto request);
    
    // 5. Sil (Yeni)
    Task<bool> DeleteAsync(int id);
}