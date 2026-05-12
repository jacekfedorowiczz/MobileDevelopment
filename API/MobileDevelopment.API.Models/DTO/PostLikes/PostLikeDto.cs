namespace MobileDevelopment.API.Models.DTO.PostLikes
{
    public sealed record PostLikeDto(
        int Id,
        int PostId,
        int UserId,
        DateTime CreatedAt,
        MobileDevelopment.API.Models.DTO.Posts.PostDto? Post = null,
        MobileDevelopment.API.Models.DTO.Users.UserDto? User = null
    );

    public sealed record CreateEditPostLikeDto(
        int PostId,
        int UserId
    );
}
