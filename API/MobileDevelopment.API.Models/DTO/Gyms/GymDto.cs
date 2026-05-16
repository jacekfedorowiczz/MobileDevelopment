namespace MobileDevelopment.API.Models.DTO.Gyms
{
    public sealed record GymDto(
        int Id,
        string Name,
        string Street,
        string City,
        string ZipCode,
        double Latitude,
        double Longitude,
        double Rating,
        string? Description,
        bool IsActive,
        int? CreatedByUserId);
}
