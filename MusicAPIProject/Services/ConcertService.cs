using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Services;

public class ConcertService : IConcertService
{
    private readonly AppDbContext _context;
    public ConcertService(AppDbContext context) { _context = context; }

    public async Task<List<ConcertResponseDto>> GetAllAsync()
    {
        var concerts = await _context.Concerts.ToListAsync();
        return concerts.Select(c => new ConcertResponseDto(c.Id, c.Venue, c.City, c.Date, c.ArtistId, c.CreatedAt)).ToList();
    }

    public async Task<ConcertResponseDto?> GetByIdAsync(int id)
    {
        var c = await _context.Concerts.FindAsync(id);
        return c == null ? null : new ConcertResponseDto(c.Id, c.Venue, c.City, c.Date, c.ArtistId, c.CreatedAt);
    }

    public async Task<int> CreateAsync(CreateConcertDto request)
    {
        if (!await _context.Artists.AnyAsync(a => a.Id == request.ArtistId))
            throw new Exception("Sanatçı bulunamadı.");

        var newConcert = new Concert { Venue = request.Venue, City = request.City, Date = request.Date, ArtistId = request.ArtistId };
        _context.Concerts.Add(newConcert);
        await _context.SaveChangesAsync();
        return newConcert.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateConcertDto request)
    {
        var concert = await _context.Concerts.FindAsync(id);
        if (concert == null) return false;

        concert.Venue = request.Venue;
        concert.City = request.City;
        concert.Date = request.Date;
        concert.ArtistId = request.ArtistId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var concert = await _context.Concerts.FindAsync(id);
        if (concert == null) return false;
        _context.Concerts.Remove(concert);
        await _context.SaveChangesAsync();
        return true;
    }
}