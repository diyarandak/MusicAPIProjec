using Microsoft.EntityFrameworkCore;
using Net9Odev.Entities;

namespace Net9Odev.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Concert> Concerts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // GLOBAL FİLTRE (Soft Delete Olanları Gizle)
        // Her sorguda "WHERE IsDeleted = false" otomatik eklenecek.
        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Artist>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Album>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Song>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Label>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Concert>().HasQueryFilter(x => !x.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            
            // BONUS: Silme işlemi gelirse, onu Güncelleme'ye çevir ve IsDeleted=true yap
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}