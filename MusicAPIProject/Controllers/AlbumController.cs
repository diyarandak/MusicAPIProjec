using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net9Odev.DTOs;
using Net9Odev.Services;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumController(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _albumService.GetAllAsync();
        return Ok(ApiResponse<List<AlbumResponseDto>>.Ok(result, "Albümler listelendi"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _albumService.GetByIdAsync(id);
        return result is null 
            ? NotFound(ApiResponse<object>.Fail("Albüm bulunamadı")) 
            : Ok(ApiResponse<AlbumResponseDto>.Ok(result));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateAlbumDto request)
    {
        try
        {
            var id = await _albumService.CreateAsync(request);
            // 201 Created
            return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<object>.Ok(new { id }, "Albüm eklendi"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAlbumDto request)
    {
        try
        {
            return await _albumService.UpdateAsync(id, request) 
                ? Ok(ApiResponse<object>.Ok(null, "Albüm güncellendi")) 
                : NotFound(ApiResponse<object>.Fail("Albüm bulunamadı"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _albumService.DeleteAsync(id) 
            ? Ok(ApiResponse<object>.Ok(null, "Albüm silindi")) 
            : NotFound(ApiResponse<object>.Fail("Albüm bulunamadı"));
    }
}