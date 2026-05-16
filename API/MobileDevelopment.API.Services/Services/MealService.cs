using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Domain.Interfaces.Auth;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class MealService : IMealService
    {
        private readonly IMealRepository _mealRepo;
        private readonly IDietDayRepository _dietDayRepo;
        private readonly IUserContext _userContext;
        private readonly ICacheService _cacheService;
        private readonly ILogger<MealService> _logger;

        public MealService(
            IMealRepository mealRepo, 
            IDietDayRepository dietDayRepo,
            IUserContext userContext,
            ICacheService cacheService,
            ILogger<MealService> logger)
        {
            _mealRepo = mealRepo;
            _dietDayRepo = dietDayRepo;
            _userContext = userContext;
            _cacheService = cacheService;
            _logger = logger;
        }

        private async Task<bool> IsDietDayOwnerAsync(int dietDayId, CancellationToken ct)
        {
            var userId = _userContext.UserId ?? throw new UnauthorizedAccessException();
            var day = await _dietDayRepo.GetQueryable().Include(d => d.Diet).FirstOrDefaultAsync(d => d.Id == dietDayId, ct);
            return day?.Diet?.UserId == userId;
        }

        private async Task<bool> IsMealOwnerAsync(int mealId, CancellationToken ct)
        {
            var meal = await _mealRepo.GetQueryable().Include(m => m.DietDay).ThenInclude(d => d.Diet).FirstOrDefaultAsync(m => m.Id == mealId, ct);
            var userId = _userContext.UserId ?? throw new UnauthorizedAccessException();
            return meal?.DietDay?.Diet?.UserId == userId;
        }

        public async Task<Result<MealDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                if (!await IsMealOwnerAsync(id, ct))
                {
                    return Result<MealDto>.Failure("Unauthorized.");
                }
                    
                var meal = await _mealRepo.GetByIdAsync(id, ct);
                if (meal is null)
                {
                    return Result<MealDto>.Failure($"Meal {id} not found.");
                }
                    
                return Result<MealDto>.Success(meal.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Meal");
                return Result<MealDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagedResult<MealDto>>> GetPagedByDietDayIdAsync(int dietDayId, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            try
            {
                if (!await IsDietDayOwnerAsync(dietDayId, ct))
                {
                    return Result<PagedResult<MealDto>>.Failure("Unauthorized.");
                }
                    
                var query = _mealRepo.GetQueryable()
                    .Where(m => m.DietDayId == dietDayId)
                    .OrderBy(m => m.Time);

                var totalCount = await query.CountAsync(ct);
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

                var dtos = items.Select(m => m.ToDto()).ToList();
                return Result<PagedResult<MealDto>>.Success(new PagedResult<MealDto>(dtos, totalCount, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Meals");
                return Result<PagedResult<MealDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<MealDto>>> GetAllByDietDayIdAsync(int dietDayId, CancellationToken ct = default)
        {
            try
            {
                if (!await IsDietDayOwnerAsync(dietDayId, ct))
                {
                    return Result<IEnumerable<MealDto>>.Failure("Unauthorized.");
                }

                var items = await _mealRepo.GetQueryable()
                    .Where(m => m.DietDayId == dietDayId)
                    .OrderBy(m => m.Time)
                    .ToListAsync(ct);

                return Result<IEnumerable<MealDto>>.Success(items.Select(m => m.ToDto()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all Meals");
                return Result<IEnumerable<MealDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<MealDto>> CreateAsync(CreateEditMealDto dto, CancellationToken ct = default)
        {
            try
            {
                if (!await IsDietDayOwnerAsync(dto.DietDayId, ct))
                {
                    return Result<MealDto>.Failure("Unauthorized.");
                }

                var meal = new Meal
                {
                    DietDayId = dto.DietDayId,
                    Name = dto.Name,
                    Time = dto.Time,
                    TotalCalories = dto.TotalCalories,
                    Protein = dto.Protein,
                    Carbs = dto.Carbs,
                    Fats = dto.Fats
                };

                var created = await _mealRepo.CreateAsync(meal, ct);
                await InvalidateCurrentUserDashboardAsync(ct);
                return Result<MealDto>.Success(created.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Meal");
                return Result<MealDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<MealDto>> UpdateAsync(int id, CreateEditMealDto dto, CancellationToken ct = default)
        {
            try
            {
                if (!await IsMealOwnerAsync(id, ct))
                {
                    return Result<MealDto>.Failure("Unauthorized.");
                }

                var meal = await _mealRepo.GetByIdAsync(id, ct);
                if (meal is null)
                {
                    return Result<MealDto>.Failure("Meal not found.");
                }
                   
                meal.Name = dto.Name;
                meal.Time = dto.Time;
                meal.TotalCalories = dto.TotalCalories;
                meal.Protein = dto.Protein;
                meal.Carbs = dto.Carbs;
                meal.Fats = dto.Fats;

                await _mealRepo.UpdateAsync(meal, ct);
                await InvalidateCurrentUserDashboardAsync(ct);
                return Result<MealDto>.Success(meal.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Meal");
                return Result<MealDto>.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
        {
            try
            {
                if (!await IsMealOwnerAsync(id, ct))
                {
                    return Result.Failure("Unauthorized.");
                }
                    
                await _mealRepo.DeleteAsync(id, ct);
                await InvalidateCurrentUserDashboardAsync(ct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Meal");
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken ct = default)
        {
            try
            {
                var idList = ids.ToList();
                foreach (var id in idList)
                {
                    if (!await IsMealOwnerAsync(id, ct))
                    {
                        return Result.Failure($"Unauthorized access to Meal {id}.");
                    }   
                }

                await _mealRepo.DeleteRangeAsync(idList, ct);
                await InvalidateCurrentUserDashboardAsync(ct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Meals range");
                return Result.Failure(ex.Message);
            }
        }

        private Task InvalidateCurrentUserDashboardAsync(CancellationToken ct)
        {
            var userId = _userContext.UserId;
            return userId.HasValue
                ? _cacheService.InvalidateAreaAsync($"dashboard:user:{userId.Value}", ct)
                : Task.CompletedTask;
        }
    }
}
