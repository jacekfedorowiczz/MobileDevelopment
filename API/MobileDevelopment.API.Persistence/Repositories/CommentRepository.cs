using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Persistence.Interfaces;

namespace MobileDevelopment.API.Persistence.Repositories
{
    public class CommentRepository(SystemContext context)
        : Repository<Comment>(context), ICommentRepository
    {
    }
}
