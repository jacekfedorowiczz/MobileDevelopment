using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Domain.Interfaces.Auth;
using MobileDevelopment.API.Models.DTO.WorkoutSets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class WorkoutSetService : IWorkoutSetService
    {
        private readonly IWorkoutSetRepository _setRepo;
        private readonly IWorkoutSessionRepository _sessionRepo;
        private readonly IUserContext _userContext;
        private readonly ICacheService _cacheService;
        private readonly ILogger<WorkoutSetService> _logger;

        public WorkoutSetService(
            IWorkoutSetRepository setRepo,
            IWorkoutSessionRepository sessionRepo,
            IUserContext userContext,
            ICacheService cacheService,
            ILogger<WorkoutSetService> logger)
        {
            _setRepo = setRepo;
            _sessionRepo = sessionRepo;
            _userContext = userContext;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Result<WorkoutSetDto>> GetSetByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var set = await _setRepo
                    .GetQueryable()
                    .Include(ws => ws.Exercise)
                        .ThenInclude(e => e.TargetedMuscles)
                    .FirstOrDefaultAsync(ws => ws.Id == id, ct);

                if (set is null)
                {
                    return Result<WorkoutSetDto>.Failure($"Workout set with id {id} not found.");
                }

                return Result<WorkoutSetDto>.Success(set.ToDto(includeExercise: true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workout set {Id}", id);
                return Result<WorkoutSetDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<WorkoutSetDto>>> GetSetsBySessionAsync(int sessionId, CancellationToken ct = default)
        {
            try
            {
                var sets = await _setRepo
                    .GetQueryable()
                    .Include(ws => ws.Exercise)
                        .ThenInclude(e => e.TargetedMuscles)
                    .Where(ws => ws.WorkoutSessionId == sessionId)
                    .OrderBy(ws => ws.SetNumber)
                    .ToListAsync(ct);

                return Result<IEnumerable<WorkoutSetDto>>.Success(
                    sets.Select(ws => ws.ToDto(includeExercise: true)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting sets for session {SessionId}", sessionId);
                return Result<IEnumerable<WorkoutSetDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<WorkoutSetDto>> CreateSetAsync(CreateEditWorkoutSetDto dto, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId
                    ?? throw new UnauthorizedAccessException("User not authenticated.");

                var session = await _sessionRepo.GetByIdAsync(dto.WorkoutSessionId, ct);
                if (session is null)
                {
                    return Result<WorkoutSetDto>.Failure($"Workout session with id {dto.WorkoutSessionId} not found.");
                }

                if (session.UserId != userId)
                {
                    return Result<WorkoutSetDto>.Failure("You do not have access to this workout session.");
                }

                if (!IsValidRpe(dto.Rpe))
                {
                    return Result<WorkoutSetDto>.Failure("Set RPE must be in the range of 1-10.");
                }

                var setNumber = dto.SetNumber;
                if (setNumber <= 0)
                {
                    var maxSetNumber = await _setRepo
                        .GetQueryable()
                        .Where(ws => ws.WorkoutSessionId == dto.WorkoutSessionId
                                  && ws.ExerciseId == dto.ExerciseId)
                        .Select(ws => (int?)ws.SetNumber)
                        .MaxAsync(ct);

                    setNumber = (maxSetNumber ?? 0) + 1;
                }

                var set = new WorkoutSet
                {
                    WorkoutSessionId = dto.WorkoutSessionId,
                    ExerciseId = dto.ExerciseId,
                    SetNumber = setNumber,
                    Weight = dto.Weight,
                    Reps = dto.Reps,
                    Rpe = dto.Rpe,
                    DurationSeconds = dto.DurationSeconds,
                };

                var created = await _setRepo.CreateAsync(set, ct);
                await RecalculateSessionStatsAsync(dto.WorkoutSessionId, ct);
                await InvalidateDashboardAsync(userId, ct);

                var withExercise = await _setRepo
                    .GetQueryable()
                    .Include(ws => ws.Exercise)
                        .ThenInclude(e => e.TargetedMuscles)
                    .FirstOrDefaultAsync(ws => ws.Id == created.Id, ct);

                return Result<WorkoutSetDto>.Success(withExercise!.ToDto(includeExercise: true));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Result<WorkoutSetDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating workout set");
                return Result<WorkoutSetDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<WorkoutSetDto>> UpdateSetAsync(int id, CreateEditWorkoutSetDto dto, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId
                    ?? throw new UnauthorizedAccessException("User not authenticated.");

                var set = await _setRepo
                    .GetQueryable()
                    .Include(ws => ws.WorkoutSession)
                    .Include(ws => ws.Exercise)
                    .FirstOrDefaultAsync(ws => ws.Id == id, ct);

                if (set is null)
                {
                    return Result<WorkoutSetDto>.Failure($"Workout set with id {id} not found.");
                }

                if (set.WorkoutSession.UserId != userId)
                {
                    return Result<WorkoutSetDto>.Failure("You do not have access to this workout set.");
                }

                if (!IsValidRpe(dto.Rpe))
                {
                    return Result<WorkoutSetDto>.Failure("Set RPE must be in the range of 1-10.");
                }

                set.ExerciseId = dto.ExerciseId;
                set.SetNumber = dto.SetNumber > 0 ? dto.SetNumber : set.SetNumber;
                set.Weight = dto.Weight;
                set.Reps = dto.Reps;
                set.Rpe = dto.Rpe;
                set.DurationSeconds = dto.DurationSeconds;

                await _setRepo.UpdateAsync(set, ct);
                await RecalculateSessionStatsAsync(set.WorkoutSessionId, ct);
                await InvalidateDashboardAsync(userId, ct);
                return Result<WorkoutSetDto>.Success(set.ToDto(includeExercise: true));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Result<WorkoutSetDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workout set {Id}", id);
                return Result<WorkoutSetDto>.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteSetAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId
                    ?? throw new UnauthorizedAccessException("User not authenticated.");

                var set = await _setRepo
                    .GetQueryable()
                    .Include(ws => ws.WorkoutSession)
                    .FirstOrDefaultAsync(ws => ws.Id == id, ct);

                if (set is null)
                {
                    return Result.Failure($"Workout set with id {id} not found.");
                }

                if (set.WorkoutSession.UserId != userId)
                {
                    return Result.Failure("You do not have access to this workout set.");
                }

                await _setRepo.DeleteAsync(id, ct);
                await RecalculateSessionStatsAsync(set.WorkoutSessionId, ct);
                await InvalidateDashboardAsync(userId, ct);
                return Result.Success();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting workout set {Id}", id);
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteSetsRangeAsync(IEnumerable<int> ids, CancellationToken ct = default)
        {
            try
            {
                var userId = _userContext.UserId
                    ?? throw new UnauthorizedAccessException("User not authenticated.");

                var idList = ids.ToList();
                var sets = await _setRepo
                    .GetQueryable()
                    .Include(ws => ws.WorkoutSession)
                    .Where(ws => idList.Contains(ws.Id))
                    .ToListAsync(ct);

                if (sets.Any(ws => ws.WorkoutSession.UserId != userId))
                {
                    return Result.Failure("One or more sets do not belong to the current user.");
                }

                await _setRepo.DeleteRangeAsync(idList, ct);
                
                var sessionIds = sets.Select(s => s.WorkoutSessionId).Distinct().ToList();
                foreach(var sId in sessionIds)
                {
                    await RecalculateSessionStatsAsync(sId, ct);
                }
                await InvalidateDashboardAsync(userId, ct);
                return Result.Success();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting workout sets range");
                return Result.Failure(ex.Message);
            }
        }

        private async Task RecalculateSessionStatsAsync(int sessionId, CancellationToken ct)
        {
            var session = await _sessionRepo.GetByIdAsync(sessionId, ct);
            if (session == null) return;

            var sessionSets = await _setRepo.GetQueryable()
                .Where(s => s.WorkoutSessionId == sessionId)
                .Select(s => new { s.ExerciseId, s.Rpe, s.DurationSeconds })
                .ToListAsync(ct);

            var totalSeconds = sessionSets.Sum(s => s.DurationSeconds ?? 0);
            session.EndTime = totalSeconds > 0 ? session.StartTime.AddSeconds(totalSeconds) : null;

            var exerciseRpeAverages = sessionSets
                .Where(s => s.Rpe.HasValue)
                .GroupBy(s => s.ExerciseId)
                .Select(group => group.Average(s => s.Rpe!.Value))
                .ToList();

            session.GlobalSessionRpe = exerciseRpeAverages.Count > 0
                ? (int)Math.Round(exerciseRpeAverages.Average(), MidpointRounding.AwayFromZero)
                : null;

            await _sessionRepo.UpdateAsync(session, ct);
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
