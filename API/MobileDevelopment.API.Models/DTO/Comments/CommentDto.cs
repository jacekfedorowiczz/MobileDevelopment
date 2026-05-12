namespace MobileDevelopment.API.Models.DTO.Comments
{
    public sealed record CommentDto(
        int Id,
        int PostId,
        int UserId,
        string Content,
        DateTime CreatedAt,
        MobileDevelopment.API.Models.DTO.Posts.PostDto? Post = null,
        MobileDevelopment.API.Models.DTO.Users.UserDto? User = null
    );

    public sealed record CreateEditCommentDto(
        int PostId,
        int UserId,
        string Content
    );
}
