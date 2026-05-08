namespace MobileDevelopment.API.Models.DTO.MuscleGroups
{
    public sealed record MuscleGroupDto(
        int Id,
        string Name
    );

    public sealed record CreateEditMuscleGroupDto(
        string Name
    );
}
