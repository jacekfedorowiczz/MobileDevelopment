namespace MobileDevelopment.API.Models.DTO.Tags
{
    public sealed record TagDto(
        int Id,
        string Name,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.Posts.PostDto>? Posts = null,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.Profiles.ProfileDto>? InterestedProfiles = null
    );

    public sealed record CreateEditTagDto(
        string Name
    );
}
