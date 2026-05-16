using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Domain.Interfaces.Auth;
using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.DTO.WorkoutSets;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;

namespace MobileDevelopment.API.Services.Services
{
    internal sealed class WorkoutSessionService : IWorkoutSessionService
    {
        private readonly IWorkoutSessionRepository _sessionRepo;
        private readonly IWorkoutSetRepository _setRepo;
        private readonly IUserContext _userContext;
        private readonly ICacheService _cacheService;
        private readonly ILogger<WorkoutSessionService> _logger;

        public WorkoutSessionService(
            IWorkoutSessionRepository sessionRepo,
            IWorkoutSetRepository setRepo,
            IUserContext userContext,
            ICacheService cacheService,
            ILogger<WorkoutSessionService> logger)
        {
            _sessionRepo = sessionRepo;
            _setRepo = setRepo;
            _userContext = userContext;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Result<WorkoutSessionDto>> GetSessionByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var session = await _sessionRepo
                    .GetQueryable()
                    .Include(s => s.Sets)
                        .ThenInclude(ws => ws.Exercise)
                            .ThenInclude(e => e.TargetedMuscles)
                    .FirstOrDefaultAsync(s => s.Id == id, ct);

                if (session is null)
                {
                    return Result<WorkoutSessionDto>.Failure($"Workout session with id {id} not found.");
                }

                var currentUserId = _userContext.UserId;
                if (session.UserId != currentUserId)
                {
                    return Result<WorkoutSessionDto>.Failure("You do not have access to this workout session.");
                }

                return Result<WorkoutSessionDto>.Success(session.ToDto(includeSets: true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workout session {Id}", id);
                return Result<WorkoutSessionDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagedResult<WorkoutSessionSummaryDto>>> GetPagedSessionsForCurrentUserAsync(
            int pageNumber, int pageSize, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId
                    ?? throw new UnauthorizedAccessException("User not authenticated.");

                var query = _sessionRepo
                    .GetQueryable()
                    .Include(s => s.Sets)
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.StartTime);

                var totalCount = await query.CountAsync(ct);
                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                var dtos = items.Select(s => s.ToSummaryDto()).ToList();
                var result = new PagedResult<WorkoutSessionSummaryDto>(dtos, totalCount, pageNumber, pageSize);
                return Result<PagedResult<WorkoutSessionSummaryDto>>.Success(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Result<PagedResult<WorkoutSessionSummaryDto>>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged workout sessions");
                return Result<PagedResult<WorkoutSessionSummaryDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<WorkoutSessionDto>>> GetAllSessionsForCurrentUserAsync(CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId
                    ?? throw new UnauthorizedAccessException("User not authenticated.");

                var sessions = await _sessionRepo
                    .GetQueryable()
                    .Include(s => s.Sets)
                        .ThenInclude(ws => ws.Exercise)
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.StartTime)
                    .ToListAsync(ct);

                return Result<IEnumerable<WorkoutSessionDto>>.Success(
                    sessions.Select(s => s.ToDto(includeSets: true)));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Result<IEnumerable<WorkoutSessionDto>>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all workout sessions");
                return Result<IEnumerable<WorkoutSessionDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<WorkoutSessionDto>> CreateSessionAsync(
            CreateEditWorkoutSessionDto dto, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId
                    ?? throw new UnauthorizedAccessException("User not authenticated.");

                var validationError = ValidateSessionDates(dto.StartTime, dto.EndTime);
                if (validationError is not null)
                {
                    return Result<WorkoutSessionDto>.Failure(validationError);
                }

                var session = new WorkoutSession
                {
                    UserId = userId,
                    Name = dto.Name,
                    Description = dto.Description,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    GlobalSessionRpe = null,
                };

                var created = await _sessionRepo.CreateAsync(session, ct);
                await InvalidateDashboardAsync(userId, ct);
                return Result<WorkoutSessionDto>.Success(created.ToDto());
            }
            catch (UnauthorizedAccessException ex)
            {
                return Result<WorkoutSessionDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating workout session");
                return Result<WorkoutSessionDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<WorkoutSessionDto>> CreateSessionWithSetsAsync(
            CreateWorkoutSessionWithSetsDto dto, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId
                    ?? throw new UnauthorizedAccessException("User not authenticated.");

                var validationError = ValidateSessionDates(dto.StartTime, dto.EndTime);
                if (validationError is not null)
                {
                    return Result<WorkoutSessionDto>.Failure(validationError);
                }

                var sets = dto.Sets?.ToList() ?? [];
                if (sets.Any(set => !IsValidRpe(set.Rpe)))
                {
                    return Result<WorkoutSessionDto>.Failure("Set RPE must be in the range of 1-10.");
                }

                var session = new WorkoutSession
                {
                    UserId = userId,
                    Name = dto.Name,
                    Description = dto.Description,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    GlobalSessionRpe = CalculateGlobalSessionRpe(sets),
                };

                if (sets.Count > 0)
                {
                    var nextSetNumbers = new Dictionary<int, int>();
                    foreach (var setDto in sets)
                    {
                        if (!nextSetNumbers.TryGetValue(setDto.ExerciseId, out var nextSetNumber))
                        {
                            nextSetNumber = 1;
                        }

                        session.Sets.Add(new WorkoutSet
                        {
                            ExerciseId = setDto.ExerciseId,
                            SetNumber = setDto.SetNumber > 0 ? setDto.SetNumber : nextSetNumber,
                            Weight = setDto.Weight,
                            Reps = setDto.Reps,
                            Rpe = setDto.Rpe,
                            DurationSeconds = setDto.DurationSeconds,
                        });

                        nextSetNumbers[setDto.ExerciseId] = (setDto.SetNumber > 0 ? setDto.SetNumber : nextSetNumber) + 1;
                    }
                }

                var created = await _sessionRepo.CreateAsync(session, ct);

                var withSets = await _sessionRepo
                    .GetQueryable()
                    .Include(s => s.Sets)
                        .ThenInclude(ws => ws.Exercise)
                            .ThenInclude(e => e.TargetedMuscles)
                    .FirstOrDefaultAsync(s => s.Id == created.Id, ct);

                await InvalidateDashboardAsync(userId, ct);
                return Result<WorkoutSessionDto>.Success(withSets!.ToDto(includeSets: true));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Result<WorkoutSessionDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating workout session with sets");
                return Result<WorkoutSessionDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<WorkoutSessionDto>> UpdateSessionAsync(
            int id, CreateEditWorkoutSessionDto dto, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId
                    ?? throw new UnauthorizedAccessException("User not authenticated.");

                var session = await _sessionRepo.GetByIdAsync(id, ct);
                if (session is null)
                {
                    return Result<WorkoutSessionDto>.Failure($"Workout session with id {id} not found.");
                }

                if (session.UserId != userId)
                {
                    return Result<WorkoutSessionDto>.Failure("You do not have access to this workout session.");
                }

                var validationError = ValidateSessionDates(dto.StartTime, dto.EndTime);
                if (validationError is not null)
                {
                    return Result<WorkoutSessionDto>.Failure(validationError);
                }

                session.Name = dto.Name;
                session.Description = dto.Description;
                session.StartTime = dto.StartTime;
                session.EndTime = dto.EndTime;

                await _sessionRepo.UpdateAsync(session, ct);
                await InvalidateDashboardAsync(userId, ct);
                return Result<WorkoutSessionDto>.Success(session.ToDto());
            }
            catch (UnauthorizedAccessException ex)
            {
                return Result<WorkoutSessionDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workout session {Id}", id);
                return Result<WorkoutSessionDto>.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId
                    ?? throw new UnauthorizedAccessException("User not authenticated.");

                var session = await _sessionRepo.GetByIdAsync(id, ct);
                if (session is null)
                {
                    return Result.Failure($"Workout session with id {id} not found.");
                }

                if (session.UserId != userId)
                {
                    return Result.Failure("You do not have access to this workout session.");
                }

                await _sessionRepo.DeleteAsync(id, ct);
                await InvalidateDashboardAsync(userId, ct);
                return Result.Success();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting workout session {Id}", id);
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteSessionsRangeAsync(IEnumerable<int> ids, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId
                    ?? throw new UnauthorizedAccessException("User not authenticated.");

                var idList = ids.ToList();
                var sessions = await _sessionRepo
                    .GetQueryable()
                    .Where(s => idList.Contains(s.Id))
                    .ToListAsync(ct);

                if (sessions.Any(s => s.UserId != userId))
                {
                    return Result.Failure("One or more sessions do not belong to the current user.");
                }

                await _sessionRepo.DeleteRangeAsync(idList, ct);
                await InvalidateDashboardAsync(userId, ct);
                return Result.Success();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting workout sessions range");
                return Result.Failure(ex.Message);
            }
        }

        private static string? ValidateSessionDates(DateTime startTime, DateTime? endTime)
        {
            var today = DateTime.UtcNow.Date;
            if (startTime.Date > today)
            {
                return "Workout date cannot be later than today.";
            }

            if (endTime.HasValue && endTime.Value < startTime)
            {
                return "Workout end date cannot be earlier than the start date.";
            }

            return null;
        }

        private static int? CalculateGlobalSessionRpe(IEnumerable<CreateEditWorkoutSetDto>? sets)
        {
            var exerciseAverages = sets?
                .Where(set => set.Rpe.HasValue)
                .GroupBy(set => set.ExerciseId)
                .Select(group => group.Average(set => set.Rpe!.Value))
                .ToList();

            if (exerciseAverages is null || exerciseAverages.Count == 0)
            {
                return null;
            }

            return (int)Math.Round(exerciseAverages.Average(), MidpointRounding.AwayFromZero);
        }

        private static bool IsValidRpe(int? rpe)
        {
            return !rpe.HasValue || rpe.Value is >= 1 and <= 10;
        }

        private Task InvalidateDashboardAsync(int userId, CancellationToken ct)
        {
            return _cacheService.InvalidateAreaAsync($"dashboard:user:{userId}", ct);
        }
    }
}
