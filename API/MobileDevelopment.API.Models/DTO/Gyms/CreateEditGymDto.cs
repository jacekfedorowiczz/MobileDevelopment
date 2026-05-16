namespace MobileDevelopment.API.Models.DTO.Gyms
{
    public sealed record CreateEditGymDto(
        string Name,
        string Street,
        string City,
        string ZipCode,
        double Latitude,
        double Longitude,
        double Rating,
        string? Description);
}
