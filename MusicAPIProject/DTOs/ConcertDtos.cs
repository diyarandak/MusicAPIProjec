namespace Net9Odev.DTOs;

public record ConcertResponseDto(
    int Id,
    string Venue,
    string City,
    DateTime Date,
    int ArtistId, // Hangi sanatçı?
    DateTime CreatedAt
);

public record CreateConcertDto(
    string Venue, // Mekan Adı
    string City,
    DateTime Date,
    int ArtistId
);

public record UpdateConcertDto(
    string Venue,
    string City,
    DateTime Date,
    int ArtistId
);