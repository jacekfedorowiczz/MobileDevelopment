namespace MobileDevelopment.API.Models.DTO.Comments
{
    public sealed record CommentDto(
        int Id,
        int PostId,
        int UserId,
        string Content,
        DateTime CreatedAt
    );

    public sealed record CreateEditCommentDto(
        int PostId,
        int UserId,
        string Content
    );
}
