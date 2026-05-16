using MobileDevelopment.API.Models.DTO.WorkoutSets;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IWorkoutSetService
    {
        /// <summary>Gets a single set by ID, including exercise details.</summary>
        Task<Result<WorkoutSetDto>> GetSetByIdAsync(int id, CancellationToken ct = default);

        /// <summary>Gets all sets for a given session, ordered by SetNumber.</summary>
        Task<Result<IEnumerable<WorkoutSetDto>>> GetSetsBySessionAsync(int sessionId, CancellationToken ct = default);

        /// <summary>
        /// Adds a new set. If SetNumber is 0, auto-assigns MAX+1 for the (sessionId, exerciseId) pair.
        /// Validates that the session belongs to the current user.
        /// </summary>
        Task<Result<WorkoutSetDto>> CreateSetAsync(CreateEditWorkoutSetDto dto, CancellationToken ct = default);

        /// <summary>Updates a set. Validates ownership through parent session.</summary>
        Task<Result<WorkoutSetDto>> UpdateSetAsync(int id, CreateEditWorkoutSetDto dto, CancellationToken ct = default);

        /// <summary>Deletes a set. Validates ownership through parent session.</summary>
        Task<Result> DeleteSetAsync(int id, CancellationToken ct = default);

        /// <summary>Deletes multiple sets at once.</summary>
        Task<Result> DeleteSetsRangeAsync(IEnumerable<int> ids, CancellationToken ct = default);
    }
}
