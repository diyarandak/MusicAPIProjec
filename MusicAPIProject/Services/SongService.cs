using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Services;

public class SongService : ISongService
{
    private readonly AppDbContext _context;
    public SongService(AppDbContext context) { _context = context; }

    public async Task<List<SongResponseDto>> GetAllAsync()
    {
        var songs = await _context.Songs.ToListAsync();
        return songs.Select(s => new SongResponseDto(s.Id, s.Name, s.DurationSeconds, s.TrackNumber, s.AlbumId, s.CreatedAt)).ToList();
    }

    public async Task<SongResponseDto?> GetByIdAsync(int id)
    {
        var s = await _context.Songs.FindAsync(id);
        return s == null ? null : new SongResponseDto(s.Id, s.Name, s.DurationSeconds, s.TrackNumber, s.AlbumId, s.CreatedAt);
    }

    public async Task<int> CreateAsync(CreateSongDto request)
    {
        if (!await _context.Albums.AnyAsync(a => a.Id == request.AlbumId))
            throw new Exception("Albüm bulunamadı.");

        var newSong = new Song { Name = request.Name, DurationSeconds = request.DurationSeconds, TrackNumber = request.TrackNumber, AlbumId = request.AlbumId };
        _context.Songs.Add(newSong);
        await _context.SaveChangesAsync();
        return newSong.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateSongDto request)
    {
        var song = await _context.Songs.FindAsync(id);
        if (song == null) return false;

        song.Name = request.Name;
        song.DurationSeconds = request.DurationSeconds;
        song.TrackNumber = request.TrackNumber;
        song.AlbumId = request.AlbumId;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var song = await _context.Songs.FindAsync(id);
        if (song == null) return false;
        _context.Songs.Remove(song);
        await _context.SaveChangesAsync();
        return true;
    }
}