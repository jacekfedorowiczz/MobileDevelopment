using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Domain.Interfaces.Auth;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class DietService : IDietService
    {
        private readonly IDietRepository _dietRepo;
        private readonly IMealRepository _mealRepo;
        private readonly IProfileRepository _profileRepo;
        private readonly IUserContext _userContext;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DietService> _logger;

        public DietService(
            IDietRepository dietRepo,
            IMealRepository mealRepo,
            IProfileRepository profileRepo,
            IUserContext userContext,
            ICacheService cacheService,
            ILogger<DietService> logger)
        {
            _dietRepo = dietRepo;
            _mealRepo = mealRepo;
            _profileRepo = profileRepo;
            _userContext = userContext;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Result<DietDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var diet = await _dietRepo.GetQueryable()
                    .Include(d => d.DietDays)
                        .ThenInclude(dd => dd.Meals)
                    .FirstOrDefaultAsync(d => d.Id == id, ct);

                if (diet is null)
                {
                    return Result<DietDto>.Failure($"Diet with id {id} not found.");
                }

                if (diet.UserId != _userContext.UserId)
                {
                    return Result<DietDto>.Failure("You do not have access to this diet.");
                }

                return Result<DietDto>.Success(diet.ToDto(includeDietDays: true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting diet {Id}", id);
                return Result<DietDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<DietSummaryDto>> GetSummaryForUserAsync(int userId, CancellationToken ct = default)
        {
            try
            {
                var today = DateTime.UtcNow.Date;

                var meals = await _mealRepo.GetQueryable()
                    .Where(meal => meal.DietDay.Date.Date == today && meal.DietDay.Diet.UserId == userId)
                    .Select(meal => new
                    {
                        meal.TotalCalories,
                        meal.Protein,
                        meal.Carbs,
                        meal.Fats
                    })
                    .ToListAsync(ct);

                var profile = await _profileRepo.GetQueryable()
                    .Where(profile => profile.UserId == userId)
                    .Select(profile => new
                    {
                        profile.DailyCaloriesGoal
                    })
                    .FirstOrDefaultAsync(ct);

                var summary = new DietSummaryDto(
                    CaloriesConsumed: (int)Math.Round(meals.Sum(meal => meal.TotalCalories)),
                    CaloriesGoal: profile?.DailyCaloriesGoal ?? 0,
                    Protein: (int)Math.Round(meals.Sum(meal => meal.Protein)),
                    Carbs: (int)Math.Round(meals.Sum(meal => meal.Carbs)),
                    Fat: (int)Math.Round(meals.Sum(meal => meal.Fats)));

                return Result<DietSummaryDto>.Success(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting diet summary for user {UserId}", userId);
                return Result<DietSummaryDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagedResult<DietDto>>> GetPagedDietsForCurrentUserAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId ?? throw new UnauthorizedAccessException();
                var query = _dietRepo.GetQueryable()
                    .Where(d => d.UserId == userId)
                    .OrderByDescending(d => d.StartDate);

                var totalCount = await query.CountAsync(ct);
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

                var dtos = items.Select(d => d.ToDto()).ToList();
                var result = new PagedResult<DietDto>(dtos, totalCount, pageNumber, pageSize);

                return Result<PagedResult<DietDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged diets");
                return Result<PagedResult<DietDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<DietDto>>> GetAllForCurrentUserAsync(CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId ?? throw new UnauthorizedAccessException();
                var items = await _dietRepo.GetQueryable()
                    .Where(d => d.UserId == userId)
                    .OrderByDescending(d => d.StartDate)
                    .ToListAsync(ct);

                return Result<IEnumerable<DietDto>>.Success(items.Select(d => d.ToDto()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all diets");
                return Result<IEnumerable<DietDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<DietDto>> CreateAsync(CreateEditDietDto dto, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId ?? throw new UnauthorizedAccessException();
                var diet = new Diet
                {
                    UserId = userId,
                    Name = dto.Name,
                    Description = dto.Description,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate
                };

                var created = await _dietRepo.CreateAsync(diet, ct);
                await InvalidateDashboardAsync(userId, ct);
                return Result<DietDto>.Success(created.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating diet");
                return Result<DietDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<DietDto>> CreateWithDaysAsync(CreateDietWithDaysDto dto, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId ?? throw new UnauthorizedAccessException();
                var diet = new Diet
                {
                    UserId = userId,
                    Name = dto.Name,
                    Description = dto.Description,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate
                };

                if (dto.DietDays != null)
                {
                    foreach (var dayDto in dto.DietDays)
                    {
                        var day = new DietDay
                        {
                            Date = dayDto.Date,
                            Notes = dayDto.Notes
                        };

                        if (dayDto.Meals != null)
                        {
                            foreach (var mealDto in dayDto.Meals)
                            {
                                day.Meals.Add(new Meal
                                {
                                    Name = mealDto.Name,
                                    Time = mealDto.Time,
                                    TotalCalories = mealDto.TotalCalories,
                                    Protein = mealDto.Protein,
                                    Carbs = mealDto.Carbs,
                                    Fats = mealDto.Fats
                                });
                            }
                        }
                        diet.DietDays.Add(day);
                    }
                }

                var created = await _dietRepo.CreateAsync(diet, ct);
                await InvalidateDashboardAsync(userId, ct);
                var fullDiet = await _dietRepo.GetQueryable()
                    .Include(d => d.DietDays)
                        .ThenInclude(dd => dd.Meals)
                    .FirstOrDefaultAsync(d => d.Id == created.Id, ct);

                return Result<DietDto>.Success(fullDiet!.ToDto(includeDietDays: true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating diet with days");
                return Result<DietDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<DietDto>> UpdateAsync(int id, CreateEditDietDto dto, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId ?? throw new UnauthorizedAccessException();
                var diet = await _dietRepo.GetByIdAsync(id, ct);
                
                if (diet is null)
                {
                    return Result<DietDto>.Failure("Diet not found.");
                }
                    
                if (diet.UserId != userId)
                {
                    return Result<DietDto>.Failure("Unauthorized.");
                }              

                diet.Name = dto.Name;
                diet.Description = dto.Description;
                diet.StartDate = dto.StartDate;
                diet.EndDate = dto.EndDate;

                await _dietRepo.UpdateAsync(diet, ct);
                await InvalidateDashboardAsync(userId, ct);
                return Result<DietDto>.Success(diet.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating diet");
                return Result<DietDto>.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId ?? throw new UnauthorizedAccessException();
                var diet = await _dietRepo.GetByIdAsync(id, ct);
                
                if (diet is null)
                {
                    return Result.Failure("Diet not found.");
                }
                    
                if (diet.UserId != userId)
                {
                    return Result.Failure("Unauthorized.");
                }
                    
                await _dietRepo.DeleteAsync(id, ct);
                await InvalidateDashboardAsync(userId, ct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting diet");
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId ?? throw new UnauthorizedAccessException();
                var idList = ids.ToList();
                var diets = await _dietRepo.GetQueryable().Where(d => idList.Contains(d.Id)).ToListAsync(ct);
                
                if (diets.Any(d => d.UserId != userId))
                {
                    return Result.Failure("Unauthorized access to some diets.");
                }

                await _dietRepo.DeleteRangeAsync(idList, ct);
                await InvalidateDashboardAsync(userId, ct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting diets range");
                return Result.Failure(ex.Message);
            }
        }

        private Task InvalidateDashboardAsync(int userId, CancellationToken ct)
        {
            return _cacheService.InvalidateAreaAsync($"dashboard:user:{userId}", ct);
        }
    }
}
