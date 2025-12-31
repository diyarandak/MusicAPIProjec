using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Services;

public class AlbumService : IAlbumService
{
    private readonly AppDbContext _context;

    public AlbumService(AppDbContext context)
    {
        _context = context;
    }

    // Listeleme
    public async Task<List<AlbumResponseDto>> GetAllAsync()
    {
        var albums = await _context.Albums.ToListAsync();
        return albums.Select(a => new AlbumResponseDto(
            a.Id, a.Name, a.Price, a.ReleaseDate, a.ArtistId, a.CreatedAt
        )).ToList();
    }

    // Tekli Getirme
    public async Task<AlbumResponseDto?> GetByIdAsync(int id)
    {
        var a = await _context.Albums.FindAsync(id);
        if (a == null) return null;

        return new AlbumResponseDto(a.Id, a.Name, a.Price, a.ReleaseDate, a.ArtistId, a.CreatedAt);
    }

    // Ekleme
    public async Task<int> CreateAsync(CreateAlbumDto request)
    {
        var artistExists = await _context.Artists.AnyAsync(a => a.Id == request.ArtistId);
        if (!artistExists)
            throw new Exception("Böyle bir sanatçı bulunamadı.");

        var newAlbum = new Album
        {
            Name = request.Name,
            Price = request.Price,
            ReleaseDate = request.ReleaseDate,
            ArtistId = request.ArtistId
        };

        _context.Albums.Add(newAlbum);
        await _context.SaveChangesAsync();
        return newAlbum.Id;
    }

    // Güncelleme
    public async Task<bool> UpdateAsync(int id, UpdateAlbumDto request)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album == null) return false;

        // Sanatçı değişiyorsa kontrol et
        if (request.ArtistId != album.ArtistId)
        {
            var artistExists = await _context.Artists.AnyAsync(a => a.Id == request.ArtistId);
            if (!artistExists) throw new Exception("Yeni belirtilen sanatçı bulunamadı.");
        }

        album.Name = request.Name;
        album.Price = request.Price;
        album.ReleaseDate = request.ReleaseDate;
        album.ArtistId = request.ArtistId;

        await _context.SaveChangesAsync();
        return true;
    }

    // Silme
    public async Task<bool> DeleteAsync(int id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album == null) return false;

        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();
        return true;
    }
}