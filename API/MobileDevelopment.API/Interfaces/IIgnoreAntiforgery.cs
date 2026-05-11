using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MobileDevelopment.API.Interfaces
{
    public interface IIgnoreAntiforgery : IAntiforgeryPolicy, IOrderedFilter
    {
    }
}
