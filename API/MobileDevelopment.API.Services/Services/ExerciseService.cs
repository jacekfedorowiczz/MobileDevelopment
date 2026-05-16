using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Domain.Interfaces.Auth;
using MobileDevelopment.API.Models.DTO.Exercises;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepo;
        private readonly IMuscleGroupRepository _muscleGroupRepo;
        private readonly IUserContext _userContext;
        private readonly ICacheService _cacheService;
        private readonly ILogger<ExerciseService> _logger;

        public ExerciseService(
            IExerciseRepository exerciseRepo,
            IMuscleGroupRepository muscleGroupRepo,
            IUserContext userContext,
            ICacheService cacheService,
            ILogger<ExerciseService> logger)
        {
            _exerciseRepo = exerciseRepo;
            _muscleGroupRepo = muscleGroupRepo;
            _userContext = userContext;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Result<ExerciseDto>> GetExerciseByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var exercise = await _exerciseRepo
                    .GetQueryable()
                    .Include(e => e.TargetedMuscles)
                    .FirstOrDefaultAsync(e => e.Id == id, ct);

                if (exercise is null)
                {
                    return Result<ExerciseDto>.Failure($"Exercise with id {id} not found.");
                }

                return Result<ExerciseDto>.Success(exercise.ToDto(includeTargetedMuscles: true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exercise {Id}", id);
                return Result<ExerciseDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagedResult<ExerciseDto>>> GetPagedExercisesAsync(
            int pageNumber,
            int pageSize,
            string? searchPhrase = null,
            IEnumerable<int>? muscleGroupIds = null,
            CancellationToken ct = default)
        {
            try
            {
                var cacheKey = BuildPagedExercisesCacheKey(pageNumber, pageSize, searchPhrase, muscleGroupIds);
                var result = await _cacheService.GetOrSetVersionedAsync(
                    "exercises",
                    cacheKey,
                    BuildPagedResultAsync,
                    TimeSpan.FromMinutes(10),
                    ct);
                return Result<PagedResult<ExerciseDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged exercises");
                return Result<PagedResult<ExerciseDto>>.Failure(ex.Message);
            }

            async Task<PagedResult<ExerciseDto>> BuildPagedResultAsync(CancellationToken token)
            {
                var query = _exerciseRepo
                    .GetQueryable()
                    .Include(e => e.TargetedMuscles)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchPhrase))
                {
                    query = query.Where(e => e.Name.Contains(searchPhrase));
                }

                var mgIds = muscleGroupIds?.ToList();
                if (mgIds is { Count: > 0 })
                {
                    query = query.Where(e => e.TargetedMuscles.Any(mg => mgIds.Contains(mg.Id)));
                }

                query = query.OrderBy(e => e.Name);

                var totalCount = await query.CountAsync(token);
                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(token);

                var dtos = items.Select(e => e.ToDto(includeTargetedMuscles: true)).ToList();
                var result = new PagedResult<ExerciseDto>(dtos, totalCount, pageNumber, pageSize);
                return result;
            }
        }

        public async Task<Result<IEnumerable<MuscleGroupDto>>> GetAllMuscleGroupsAsync(CancellationToken ct = default)
        {
            try
            {
                var groups = await _cacheService.GetOrSetVersionedAsync(
                    "muscle-groups",
                    "all",
                    async token =>
                    {
                        var muscleGroups = await _muscleGroupRepo.GetAllAsync(token);
                        return muscleGroups.Select(mg => mg.ToDto()).ToList();
                    },
                    TimeSpan.FromMinutes(30),
                    ct);
                return Result<IEnumerable<MuscleGroupDto>>.Success(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting muscle groups");
                return Result<IEnumerable<MuscleGroupDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<ExerciseDto>> CreateExerciseAsync(CreateEditExerciseDto dto, CancellationToken ct = default)
        {
            try
            {
                var muscleGroups = await _muscleGroupRepo
                    .GetQueryable()
                    .Where(mg => dto.MuscleGroupIds.Contains(mg.Id))
                    .ToListAsync(ct);

                var userId = _userContext.UserId;
                var exercise = new Exercise
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ImageUrl = NormalizeOptionalUrl(dto.ImageUrl),
                    Difficulty = dto.Difficulty,
                    IsCompound = dto.IsCompound,
                    TargetedMuscles = muscleGroups,
                    CreatedByUserId = userId,
                    IsSystem = false
                };

                _muscleGroupRepo.AttachRange(muscleGroups);
                var created = await _exerciseRepo.CreateAsync(exercise, ct);

                var withMuscles = await _exerciseRepo
                    .GetQueryable()
                    .Include(e => e.TargetedMuscles)
                    .FirstOrDefaultAsync(e => e.Id == created.Id, ct);

                await _cacheService.InvalidateAreaAsync("exercises", ct);
                return Result<ExerciseDto>.Success(withMuscles!.ToDto(includeTargetedMuscles: true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exercise");
                return Result<ExerciseDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<ExerciseDto>> UpdateExerciseAsync(int id, CreateEditExerciseDto dto, CancellationToken ct = default)
        {
            try
            {
                var exercise = await _exerciseRepo
                    .GetQueryable()
                    .Include(e => e.TargetedMuscles)
                    .FirstOrDefaultAsync(e => e.Id == id, ct);

                if (exercise is null)
                {
                    return Result<ExerciseDto>.Failure($"Exercise with id {id} not found.");
                }

                var newMuscleGroups = await _muscleGroupRepo
                    .GetQueryable()
                    .Where(mg => dto.MuscleGroupIds.Contains(mg.Id))
                    .ToListAsync(ct);

                exercise.Name = dto.Name;
                exercise.Description = dto.Description;
                exercise.ImageUrl = NormalizeOptionalUrl(dto.ImageUrl);
                exercise.Difficulty = dto.Difficulty;
                exercise.IsCompound = dto.IsCompound;
                exercise.TargetedMuscles = newMuscleGroups;

                _muscleGroupRepo.AttachRange(newMuscleGroups);
                await _exerciseRepo.UpdateAsync(exercise, ct);
                await _cacheService.InvalidateAreaAsync("exercises", ct);
                return Result<ExerciseDto>.Success(exercise.ToDto(includeTargetedMuscles: true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating exercise {Id}", id);
                return Result<ExerciseDto>.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteExerciseAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var exercise = await _exerciseRepo.GetQueryable()
                    .FirstOrDefaultAsync(e => e.Id == id, ct);

                if (exercise is null)
                {
                    return Result.Failure($"Exercise with id {id} not found.");
                }

                var currentUserId = _userContext.UserId;
                var isAdmin = string.Equals(_userContext.UserRole, Role.Administrator.ToString(), StringComparison.OrdinalIgnoreCase);

                if (!isAdmin && exercise.CreatedByUserId != currentUserId)
                {
                    return Result.Failure("Unauthorized.");
                }

                await _exerciseRepo.DeleteAsync(id, ct);
                await _cacheService.InvalidateAreaAsync("exercises", ct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exercise {Id}", id);
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteExercisesRangeAsync(IEnumerable<int> ids, CancellationToken ct = default)
        {
            try
            {
                var idList = ids.ToList();
                await _exerciseRepo.DeleteRangeAsync(idList, ct);
                await _cacheService.InvalidateAreaAsync("exercises", ct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exercises range");
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<ExerciseDto>>> GetAllExercisesAsync(CancellationToken ct = default)
        {
            try
            {
                var exercises = await _exerciseRepo.GetQueryable()
                    .Include(e => e.TargetedMuscles)
                    .ToListAsync(ct);

                return Result<IEnumerable<ExerciseDto>>.Success(
                    exercises.Select(e => e.ToDto(includeTargetedMuscles: true)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all exercises");
                return Result<IEnumerable<ExerciseDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<MuscleGroupDto>> GetMuscleGroupByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var mg = await _muscleGroupRepo.GetByIdAsync(id, ct);
                if (mg is null)
                {
                    return Result<MuscleGroupDto>.Failure($"Muscle group {id} not found.");
                }

                return Result<MuscleGroupDto>.Success(mg.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting muscle group {Id}", id);
                return Result<MuscleGroupDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagedResult<MuscleGroupDto>>> GetPagedMuscleGroupsAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            try
            {
                var query = _muscleGroupRepo.GetQueryable();
                var totalCount = await query.CountAsync(ct);
                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                var dtos = items.Select(mg => mg.ToDto()).ToList();
                var result = new PagedResult<MuscleGroupDto>(dtos, totalCount, pageNumber, pageSize);
                return Result<PagedResult<MuscleGroupDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged muscle groups");
                return Result<PagedResult<MuscleGroupDto>>.Failure(ex.Message);
            }
        }

        private static string BuildPagedExercisesCacheKey(
            int pageNumber,
            int pageSize,
            string? searchPhrase,
            IEnumerable<int>? muscleGroupIds)
        {
            var groups = muscleGroupIds is null
                ? "none"
                : string.Join(",", muscleGroupIds.OrderBy(id => id));
            var search = Uri.EscapeDataString((searchPhrase ?? string.Empty).Trim().ToLowerInvariant());
            return $"paged:p:{pageNumber}:s:{pageSize}:q:{search}:mg:{groups}";
        }

        private static string? NormalizeOptionalUrl(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
