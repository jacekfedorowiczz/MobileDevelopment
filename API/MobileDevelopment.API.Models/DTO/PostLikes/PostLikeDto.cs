namespace MobileDevelopment.API.Models.DTO.PostLikes
{
    public sealed record PostLikeDto(
        int Id,
        int PostId,
        int UserId,
        DateTime CreatedAt
    );

    public sealed record CreateEditPostLikeDto(
        int PostId,
        int UserId
    );
}
