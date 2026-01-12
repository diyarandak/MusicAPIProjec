using Net9Odev.DTOs;
namespace Net9Odev.Services;

public interface ILabelService
{
    Task<List<LabelResponseDto>> GetAllAsync();
    Task<LabelResponseDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateLabelDto request);
    Task<bool> UpdateAsync(int id, UpdateLabelDto request);
    Task<bool> DeleteAsync(int id);
}