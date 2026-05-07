namespace MobileDevelopment.API.Models.DTO.Users
{
    public sealed record UserDto(int Id, string FirstName, string LastName, string Email, string MobilePhone, DateTime CreatedAt, int ProfileId);
    public sealed record CreateUserDto(string FirstName, string LastName, string Email, string MobilePhone, string Password);
}
