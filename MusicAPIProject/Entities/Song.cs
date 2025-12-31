namespace Net9Odev.Entities;

public class Song : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Şarkı Adı
    public int DurationSeconds { get; set; } // Süre (Saniye cinsinden)
    public int TrackNumber { get; set; } // Şarkı sırası

    // İlişki: Hangi Albüme ait?
    public int AlbumId { get; set; }
    public Album Album { get; set; } = null!;
}