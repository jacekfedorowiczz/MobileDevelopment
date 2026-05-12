using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Models.DTO.Posts
{
    public sealed record PostDto(
        int Id,
        int UserId,
        string Title,
        string Content,
        DateTime CreatedAt,
        FitnessGoal? TargetGoal,
        MobileDevelopment.API.Models.DTO.Users.UserDto? User = null,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.Comments.CommentDto>? Comments = null,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.PostLikes.PostLikeDto>? Likes = null,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.Tags.TagDto>? Tags = null
    );

    public sealed record CreateEditPostDto(
        int UserId,
        string Title,
        string Content,
        FitnessGoal? TargetGoal,
        IEnumerable<int> TagIds
    );
}
