using System.Collections.Generic;

namespace MobileDevelopment.API.Services.Interfaces.Commands
{
    public interface IRemoveRangeCommand
    {
        IEnumerable<int> Ids { get; }
    }
}