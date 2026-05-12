using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Services.Interfaces.Base;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IDietService : IBaseService<DietDto, CreateEditDietDto>
    {
    }
}
