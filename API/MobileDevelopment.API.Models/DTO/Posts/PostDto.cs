using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Models.DTO.Posts
{
    public sealed record PostDto(
        int Id,
        int UserId,
        string Title,
        string Content,
        DateTime CreatedAt,
        FitnessGoal? TargetGoal
    );

    public sealed record CreateEditPostDto(
        int UserId,
        string Title,
        string Content,
        FitnessGoal? TargetGoal,
        IEnumerable<int> TagIds
    );
}
