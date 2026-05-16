using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Domain.Interfaces.Auth;
using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class DietDayService : IDietDayService
    {
        private readonly IDietDayRepository _dietDayRepo;
        private readonly IDietRepository _dietRepo;
        private readonly IUserContext _userContext;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DietDayService> _logger;

        public DietDayService(
            IDietDayRepository dietDayRepo, 
            IDietRepository dietRepo,
            IUserContext userContext,
            ICacheService cacheService,
            ILogger<DietDayService> logger)
        {
            _dietDayRepo = dietDayRepo;
            _dietRepo = dietRepo;
            _userContext = userContext;
            _cacheService = cacheService;
            _logger = logger;
        }

        private async Task<bool> IsDietOwnerAsync(int dietId, CancellationToken ct)
        {
            var userId = _userContext.UserId ?? throw new UnauthorizedAccessException();
            var diet = await _dietRepo.GetByIdAsync(dietId, ct);
            return diet?.UserId == userId;
        }

        private async Task<bool> IsDietDayOwnerAsync(int dietDayId, CancellationToken ct)
        {
            var userId = _userContext.UserId ?? throw new UnauthorizedAccessException();
            var day = await _dietDayRepo.GetQueryable().Include(d => d.Diet).FirstOrDefaultAsync(d => d.Id == dietDayId, ct);
            return day?.Diet?.UserId == userId;
        }

        public async Task<Result<DietDayDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var day = await _dietDayRepo.GetQueryable()
                    .Include(d => d.Meals)
                    .Include(d => d.Diet)
                    .FirstOrDefaultAsync(d => d.Id == id, ct);

                if (day is null)
                {
                    return Result<DietDayDto>.Failure($"DietDay {id} not found.");
                }
                    
                if (day.Diet?.UserId != _userContext.UserId)
                {
                    return Result<DietDayDto>.Failure("Unauthorized.");
                }

                return Result<DietDayDto>.Success(day.ToDto(includeMeals: true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting DietDay");
                return Result<DietDayDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagedResult<DietDayDto>>> GetPagedByDietIdAsync(int dietId, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            try
            {
                if (!await IsDietOwnerAsync(dietId, ct))
                {
                    return Result<PagedResult<DietDayDto>>.Failure("Unauthorized.");
                }

                var query = _dietDayRepo.GetQueryable()
                    .Include(d => d.Meals)
                    .Where(d => d.DietId == dietId)
                    .OrderBy(d => d.Date);

                var totalCount = await query.CountAsync(ct);
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

                var dtos = items.Select(d => d.ToDto(includeMeals: true)).ToList();
                return Result<PagedResult<DietDayDto>>.Success(new PagedResult<DietDayDto>(dtos, totalCount, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged DietDays");
                return Result<PagedResult<DietDayDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<DietDayDto>>> GetAllByDietIdAsync(int dietId, CancellationToken ct = default)
        {
            try
            {
                if (!await IsDietOwnerAsync(dietId, ct))
                {
                    return Result<IEnumerable<DietDayDto>>.Failure("Unauthorized.");
                }

                var items = await _dietDayRepo.GetQueryable()
                    .Include(d => d.Meals)
                    .Where(d => d.DietId == dietId)
                    .OrderBy(d => d.Date)
                    .ToListAsync(ct);

                return Result<IEnumerable<DietDayDto>>.Success(items.Select(d => d.ToDto(includeMeals: true)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all DietDays");
                return Result<IEnumerable<DietDayDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<DietDayDto>> CreateAsync(CreateEditDietDayDto dto, CancellationToken ct = default)
        {
            try
            {
                if (!await IsDietOwnerAsync(dto.DietId, ct))
                {
                    return Result<DietDayDto>.Failure("Unauthorized.");
                }           

                var day = new DietDay
                {
                    DietId = dto.DietId,
                    Date = dto.Date,
                    Notes = dto.Notes
                };

                var created = await _dietDayRepo.CreateAsync(day, ct);
                await InvalidateCurrentUserDashboardAsync(ct);
                return Result<DietDayDto>.Success(created.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating DietDay");
                return Result<DietDayDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<DietDayDto>> CreateWithMealsAsync(CreateDietDayWithMealsDto dto, CancellationToken ct = default)
        {
            try
            {
                if (!await IsDietOwnerAsync(dto.DietId, ct))
                {
                    return Result<DietDayDto>.Failure("Unauthorized.");
                }

                var day = new DietDay
                {
                    DietId = dto.DietId,
                    Date = dto.Date,
                    Notes = dto.Notes
                };

                if (dto.Meals != null)
                {
                    foreach (var meal in dto.Meals)
                    {
                        day.Meals.Add(new Meal
                        {
                            Name = meal.Name,
                            Time = meal.Time,
                            TotalCalories = meal.TotalCalories,
                            Protein = meal.Protein,
                            Carbs = meal.Carbs,
                            Fats = meal.Fats
                        });
                    }
                }

                var created = await _dietDayRepo.CreateAsync(day, ct);
                await InvalidateCurrentUserDashboardAsync(ct);
                var fullDay = await _dietDayRepo.GetQueryable()
                    .Include(d => d.Meals)
                    .FirstOrDefaultAsync(d => d.Id == created.Id, ct);

                return Result<DietDayDto>.Success(fullDay!.ToDto(includeMeals: true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating DietDay with meals");
                return Result<DietDayDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<DietDayDto>> UpdateAsync(int id, CreateEditDietDayDto dto, CancellationToken ct = default)
        {
            try
            {
                if (!await IsDietDayOwnerAsync(id, ct))
                {
                    return Result<DietDayDto>.Failure("Unauthorized.");
                }
                
                var day = await _dietDayRepo.GetByIdAsync(id, ct);
                if (day == null)
                {
                    return Result<DietDayDto>.Failure("DietDay not found.");
                }

                day.Date = dto.Date;
                day.Notes = dto.Notes;

                await _dietDayRepo.UpdateAsync(day, ct);
                await InvalidateCurrentUserDashboardAsync(ct);
                return Result<DietDayDto>.Success(day.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating DietDay");
                return Result<DietDayDto>.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
        {
            try
            {
                if (!await IsDietDayOwnerAsync(id, ct))
                {
                    return Result.Failure("Unauthorized.");
                }
                
                await _dietDayRepo.DeleteAsync(id, ct);
                await InvalidateCurrentUserDashboardAsync(ct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting DietDay");
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
                    if (!await IsDietDayOwnerAsync(id, ct))
                    {
                        return Result.Failure($"Unauthorized access to DietDay {id}.");
                    }    
                }

                await _dietDayRepo.DeleteRangeAsync(idList, ct);
                await InvalidateCurrentUserDashboardAsync(ct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting DietDays range");
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
