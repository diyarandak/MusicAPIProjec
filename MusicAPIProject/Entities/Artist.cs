namespace Net9Odev.Entities;

public class Artist : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    public int? LabelId { get; set; }
    public Label? Label { get; set; }

    public ICollection<Album> Albums { get; set; } = new List<Album>();
    public ICollection<Concert> Concerts { get; set; } = new List<Concert>();
}