using MobileDevelopment.API.Models.DTO.Exercises;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IExerciseService
    {
        /// <summary>Gets an exercise by ID with its targeted muscle groups.</summary>
        Task<Result<ExerciseDto>> GetExerciseByIdAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// Gets paginated exercises with optional full-text search and muscle group filtering.
        /// </summary>
        Task<Result<PagedResult<ExerciseDto>>> GetPagedExercisesAsync(
            int pageNumber,
            int pageSize,
            string? searchPhrase = null,
            IEnumerable<int>? muscleGroupIds = null,
            CancellationToken ct = default);

        /// <summary>Gets all available muscle groups (for dropdowns/filters).</summary>
        Task<Result<IEnumerable<MuscleGroupDto>>> GetAllMuscleGroupsAsync(CancellationToken ct = default);

        /// <summary>Creates a new exercise and assigns muscle groups by IDs.</summary>
        Task<Result<ExerciseDto>> CreateExerciseAsync(CreateEditExerciseDto dto, CancellationToken ct = default);

        /// <summary>Updates an exercise, re-syncing its muscle group associations.</summary>
        Task<Result<ExerciseDto>> UpdateExerciseAsync(int id, CreateEditExerciseDto dto, CancellationToken ct = default);

        /// <summary>Deletes an exercise.</summary>
        Task<Result> DeleteExerciseAsync(int id, CancellationToken ct = default);

        /// <summary>Deletes multiple exercises.</summary>
        Task<Result> DeleteExercisesRangeAsync(IEnumerable<int> ids, CancellationToken ct = default);

        /// <summary>Gets all exercises (unpaged).</summary>
        Task<Result<IEnumerable<ExerciseDto>>> GetAllExercisesAsync(CancellationToken ct = default);

        /// <summary>Gets a muscle group by ID.</summary>
        Task<Result<MuscleGroupDto>> GetMuscleGroupByIdAsync(int id, CancellationToken ct = default);

        /// <summary>Gets paged muscle groups.</summary>
        Task<Result<PagedResult<MuscleGroupDto>>> GetPagedMuscleGroupsAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
