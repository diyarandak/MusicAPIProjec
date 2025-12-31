namespace Net9Odev.DTOs;

// 1. Listeleme Paketi
public record SongResponseDto(
    int Id,
    string Name,
    int DurationSeconds,
    int TrackNumber,
    int AlbumId, // Hangi albüme ait?
    DateTime CreatedAt
);

// 2. Ekleme Paketi
public record CreateSongDto(
    string Name,
    int DurationSeconds, // Saniye cinsinden süre (Örn: 215)
    int TrackNumber,     // Albümdeki sırası (Örn: 1)
    int AlbumId          // Zorunlu bağlantı
);

// 3. Güncelleme Paketi
public record UpdateSongDto(
    string Name,
    int DurationSeconds,
    int TrackNumber,
    int AlbumId
);