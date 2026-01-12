using System.Net;
using System.Text.Json;
using Net9Odev.DTOs;

namespace Net9Odev.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    // DÜZELTME 3: Logger eklendi
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // İsteği logla
            _logger.LogInformation($"İstek geldi: {httpContext.Request.Method} {httpContext.Request.Path}");
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            // Hatayı logla (Kırmızı renkle konsola yazar)
            _logger.LogError(ex, $"Bir hata oluştu: {ex.Message}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = ApiResponse<object>.Fail("Sunucu hatası: " + exception.Message);
        
        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}