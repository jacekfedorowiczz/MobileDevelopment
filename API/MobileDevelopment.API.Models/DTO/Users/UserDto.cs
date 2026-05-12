using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Models.DTO.Users
{
    public sealed record UserDto(
        int Id,
        string Login,
        string FirstName,
        string LastName,
        string FullName,
        string Email,
        string MobilePhone,
        DateTime CreatedAt,
        DateOnly DateOfBirth,
        int Age,
        Role Role,
        int ProfileId,
        MobileDevelopment.API.Models.DTO.Profiles.ProfileDto? Profile = null,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.WorkoutSessions.WorkoutSessionDto>? Sessions = null
    );

    public sealed record CreateEditUserDto(
        string Login,
        string FirstName,
        string LastName,
        string Email,
        string MobilePhone,
        string Password,
        DateOnly DateOfBirth,
        Role Role = Role.User
    );
}
