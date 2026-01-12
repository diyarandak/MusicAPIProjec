using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net9Odev.DTOs;
using Net9Odev.Services;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _userService.GetAllAsync();
        return Ok(ApiResponse<List<UserResponseDto>>.Ok(data, "Kullanıcılar listelendi"));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto request)
    {
        try 
        { 
            var id = await _userService.RegisterAsync(request);
            // 201 Created standartlara uygun
            return Created("", ApiResponse<object>.Ok(new { id }, "Kayıt başarılı")); 
        }
        catch (Exception ex) 
        { 
            // 409 Conflict (Zaten kayıtlı hatası için)
            return Conflict(ApiResponse<object>.Fail(ex.Message)); 
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto request)
    {
        var token = await _userService.LoginAsync(request);
        if (token == null) 
            return Unauthorized(ApiResponse<object>.Fail("E-posta veya şifre hatalı"));
            
        return Ok(ApiResponse<object>.Ok(new { Token = token }, "Giriş başarılı"));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto request)
    {
        return await _userService.UpdateAsync(id, request) 
            ? Ok(ApiResponse<object>.Ok(null, "Kullanıcı güncellendi")) 
            : NotFound(ApiResponse<object>.Fail("Kullanıcı bulunamadı"));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _userService.DeleteAsync(id) 
            ? Ok(ApiResponse<object>.Ok(null, "Kullanıcı silindi")) 
            : NotFound(ApiResponse<object>.Fail("Kullanıcı bulunamadı"));
    }
}