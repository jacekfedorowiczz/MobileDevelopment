using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IWorkoutSessionService
    {
        /// <summary>Gets a workout session by ID, including its sets and exercises.</summary>
        Task<Result<WorkoutSessionDto>> GetSessionByIdAsync(int id, CancellationToken ct = default);

        /// <summary>Gets paginated workout sessions for the currently authenticated user.</summary>
        Task<Result<PagedResult<WorkoutSessionSummaryDto>>> GetPagedSessionsForCurrentUserAsync(int pageNumber, int pageSize, CancellationToken ct = default);

        /// <summary>Gets all workout sessions for the current user (unpaged).</summary>
        Task<Result<IEnumerable<WorkoutSessionDto>>> GetAllSessionsForCurrentUserAsync(CancellationToken ct = default);

        /// <summary>Creates a new workout session. UserId is taken from IUserContext (JWT).</summary>
        Task<Result<WorkoutSessionDto>> CreateSessionAsync(CreateEditWorkoutSessionDto dto, CancellationToken ct = default);

        /// <summary>Creates a session together with its initial sets in one transaction.</summary>
        Task<Result<WorkoutSessionDto>> CreateSessionWithSetsAsync(CreateWorkoutSessionWithSetsDto dto, CancellationToken ct = default);

        /// <summary>Updates an existing session. Validates that it belongs to current user.</summary>
        Task<Result<WorkoutSessionDto>> UpdateSessionAsync(int id, CreateEditWorkoutSessionDto dto, CancellationToken ct = default);

        /// <summary>Deletes a session (and cascades to its sets). Validates ownership.</summary>
        Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default);

        /// <summary>Deletes multiple sessions. Validates all belong to current user.</summary>
        Task<Result> DeleteSessionsRangeAsync(IEnumerable<int> ids, CancellationToken ct = default);
    }
}
