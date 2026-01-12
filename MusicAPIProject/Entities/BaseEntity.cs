namespace Net9Odev.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // BONUS: Soft Delete Alanı
    public bool IsDeleted { get; set; } = false;
}