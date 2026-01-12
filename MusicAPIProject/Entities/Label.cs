namespace Net9Odev.Entities;

public class Label : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Şirket Adı (Örn: DMC, Sony Music)
    public string Country { get; set; } = string.Empty; // Ülke

    // İlişki: Bu şirkete bağlı Sanatçılar
    public ICollection<Artist> Artists { get; set; } = new List<Artist>();
}