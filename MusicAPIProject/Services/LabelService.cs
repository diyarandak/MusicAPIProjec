using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Services;

public class LabelService : ILabelService
{
    private readonly AppDbContext _context;
    public LabelService(AppDbContext context) { _context = context; }

    public async Task<List<LabelResponseDto>> GetAllAsync()
    {
        var labels = await _context.Labels.ToListAsync();
        return labels.Select(l => new LabelResponseDto(l.Id, l.Name, l.Country, l.CreatedAt)).ToList();
    }

    public async Task<LabelResponseDto?> GetByIdAsync(int id)
    {
        var l = await _context.Labels.FindAsync(id);
        return l == null ? null : new LabelResponseDto(l.Id, l.Name, l.Country, l.CreatedAt);
    }

    public async Task<int> CreateAsync(CreateLabelDto request)
    {
        var newLabel = new Label { Name = request.Name, Country = request.Country };
        _context.Labels.Add(newLabel);
        await _context.SaveChangesAsync();
        return newLabel.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateLabelDto request)
    {
        var label = await _context.Labels.FindAsync(id);
        if (label == null) return false;
        
        label.Name = request.Name;
        label.Country = request.Country;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var label = await _context.Labels.FindAsync(id);
        if (label == null) return false;
        _context.Labels.Remove(label);
        await _context.SaveChangesAsync();
        return true;
    }
}