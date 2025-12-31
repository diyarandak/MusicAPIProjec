namespace Net9Odev.Entities;

public class Concert : BaseEntity
{
    public string Venue { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public int ArtistId { get; set; }
    public Artist Artist { get; set; } = null!;
}