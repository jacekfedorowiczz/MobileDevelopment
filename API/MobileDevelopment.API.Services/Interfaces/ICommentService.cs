using MobileDevelopment.API.Models.DTO.Comments;
using MobileDevelopment.API.Services.Interfaces.Base;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface ICommentService : IBaseService<CommentDto, CreateEditCommentDto>
    {
    }
}
