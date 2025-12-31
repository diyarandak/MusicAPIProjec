namespace Net9Odev.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    
    // Uyarıyı önlemek için = string.Empty atadık
    public string Message { get; set; } = string.Empty; 
    
    public T? Data { get; set; }

    // Başarılı Cevap Helper
    public static ApiResponse<T> Ok(T data, string message = "İşlem başarılı")
    {
        return new ApiResponse<T> { Success = true, Message = message, Data = data };
    }

    // Hata Cevabı Helper
    public static ApiResponse<T> Fail(string message)
    {
        return new ApiResponse<T> { Success = false, Message = message, Data = default };
    }
}