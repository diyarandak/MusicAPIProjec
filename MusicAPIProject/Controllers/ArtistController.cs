using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net9Odev.DTOs;
using Net9Odev.Services;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistController : ControllerBase
{
    private readonly IArtistService _service;
    public ArtistController(IArtistService service) { _service = service; }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllArtistsAsync();
        return Ok(ApiResponse<List<ArtistResponseDto>>.Ok(data, "Sanatçılar listelendi"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetArtistByIdAsync(id);
        return result is null 
            ? NotFound(ApiResponse<object>.Fail("Sanatçı bulunamadı")) 
            : Ok(ApiResponse<ArtistResponseDto>.Ok(result));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateArtistDto request)
    {
        try
        {
            var id = await _service.AddArtistAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<object>.Ok(new { id }, "Sanatçı eklendi"));
        }
        catch (Exception ex) { return BadRequest(ApiResponse<object>.Fail(ex.Message)); }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateArtistDto request) => 
        await _service.UpdateArtistAsync(id, request) 
            ? Ok(ApiResponse<object>.Ok(null, "Güncellendi")) 
            : NotFound(ApiResponse<object>.Fail("Bulunamadı"));

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => 
        await _service.DeleteArtistAsync(id) 
            ? Ok(ApiResponse<object>.Ok(null, "Silindi")) 
            : NotFound(ApiResponse<object>.Fail("Bulunamadı"));
}