using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Services;

public class ArtistService : IArtistService
{
    private readonly AppDbContext _context;

    public ArtistService(AppDbContext context)
    {
        _context = context;
    }

    // Listeleme
    public async Task<List<ArtistResponseDto>> GetAllArtistsAsync()
    {
        // İlişkili Label verisini de (Include) çekiyoruz
        var artists = await _context.Artists.Include(a => a.Label).ToListAsync();
        
        return artists.Select(a => new ArtistResponseDto(
            a.Id, 
            a.Name, 
            a.Bio,
            a.Label != null ? a.Label.Name : "Şirket Yok" // Null kontrolü
        )).ToList();
    }

    // Detay
    public async Task<ArtistResponseDto?> GetArtistByIdAsync(int id)
    {
        var a = await _context.Artists.Include(x => x.Label).FirstOrDefaultAsync(x => x.Id == id);
        if (a == null) return null;

        return new ArtistResponseDto(
            a.Id, 
            a.Name, 
            a.Bio,
            a.Label != null ? a.Label.Name : "Şirket Yok"
        );
    }

    // Ekleme
    public async Task<int> AddArtistAsync(CreateArtistDto request)
    {
        var newArtist = new Artist
        {
            Name = request.Name,
            Bio = request.Bio,
            LabelId = request.LabelId // DTO'da artık int zorunlu olduğu için direkt atıyoruz
        };

        _context.Artists.Add(newArtist);
        await _context.SaveChangesAsync();
        return newArtist.Id;
    }

    // Güncelleme
    public async Task<bool> UpdateArtistAsync(int id, UpdateArtistDto request)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return false;

        artist.Name = request.Name;
        artist.Bio = request.Bio;
        artist.LabelId = request.LabelId;

        await _context.SaveChangesAsync();
        return true;
    }

    // Silme
    public async Task<bool> DeleteArtistAsync(int id)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return false;

        _context.Artists.Remove(artist); // Soft Delete otomatik devreye girer (AppDbContext ayarından dolayı)
        await _context.SaveChangesAsync();
        return true;
    }
}