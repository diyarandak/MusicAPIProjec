namespace Net9Odev.Entities;

public class Album : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Albüm Adı
    public DateTime ReleaseDate { get; set; } // Çıkış Tarihi
    public decimal Price { get; set; } // Fiyat (E-Ticaret mantığı da olsun diye)

    // İlişki 1: Hangi Sanatçıya ait? (Artist ile bağlantı)
    public int ArtistId { get; set; }
    public Artist Artist { get; set; } = null!;

    // İlişki 2: Albümün içindeki şarkılar (Song ile bağlantı)
    public ICollection<Song> Songs { get; set; } = new List<Song>();
}