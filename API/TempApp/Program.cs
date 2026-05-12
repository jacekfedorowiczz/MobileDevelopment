using System.Text.Json;

namespace MobileDevelopment.API.Models.Wrappers
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }
        
        public Result(bool isSuccess, T? value, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
        }
    }
}

namespace MobileDevelopment.API.Models.DTO.Auth
{
    public sealed record AuthResponse()
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
        public System.DateTime AccessTokenExpiration { get; init; }
    }
}

class Program {
    static void Main() {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = new MobileDevelopment.API.Models.Wrappers.Result<MobileDevelopment.API.Models.DTO.Auth.AuthResponse>(true, new MobileDevelopment.API.Models.DTO.Auth.AuthResponse { AccessToken = "abc", RefreshToken = "def", AccessTokenExpiration = System.DateTime.UtcNow }, null); 
        System.Console.WriteLine("CamelCase: " + JsonSerializer.Serialize(result, options));
        System.Console.WriteLine("Default: " + JsonSerializer.Serialize(result));
    }
}
