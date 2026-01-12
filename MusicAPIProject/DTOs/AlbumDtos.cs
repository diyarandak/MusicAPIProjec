namespace Net9Odev.DTOs;

// 1. Listelerken göstereceğimiz veriler
public record AlbumResponseDto(
    int Id,
    string Name,
    decimal Price,
    DateTime ReleaseDate,
    int ArtistId, // Hangi sanatçıya ait olduğu
    DateTime CreatedAt
);

// 2. Yeni Albüm eklerken isteyeceğimiz veriler
public record CreateAlbumDto(
    string Name,
    decimal Price,
    DateTime ReleaseDate,
    int ArtistId // Bunu göndermezsek albüm sahipsiz kalır
);

// 3. Güncelleme paketi
public record UpdateAlbumDto(
    string Name,
    decimal Price,
    DateTime ReleaseDate,
    int ArtistId
);