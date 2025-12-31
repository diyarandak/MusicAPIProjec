namespace Net9Odev.DTOs;

// 1. Listeleme ve Detay için (Controller'ın beklediği isim BU)
public record ArtistResponseDto(
    int Id, 
    string Name, 
    string Bio, 
    string LabelName
);

// 2. Yeni Ekleme
public record CreateArtistDto(
    string Name, 
    string Bio, 
    int LabelId
);

// 3. Güncelleme
public record UpdateArtistDto(
    string Name, 
    string Bio, 
    int LabelId
);