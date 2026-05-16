using Microsoft.EntityFrameworkCore;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Models.DTO.Dashboard;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class DashboardService : IDashboardService
    {
        private static readonly Regex CaloriesRegex = new(@"\d+", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

        private readonly IUserRepository _userRepository;
        private readonly IWorkoutSessionRepository _workoutSessionRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IDietRepository _dietRepository;
        private readonly ICacheService _cacheService;

        public DashboardService(
            IUserRepository userRepository,
            IWorkoutSessionRepository workoutSessionRepository,
            IMealRepository mealRepository,
            IDietRepository dietRepository,
            ICacheService cacheService)
        {
            _userRepository = userRepository;
            _workoutSessionRepository = workoutSessionRepository;
            _mealRepository = mealRepository;
            _dietRepository = dietRepository;
            _cacheService = cacheService;
        }

        public async Task<Result<DashboardSummaryDto>> GetSummaryAsync(int userId, CancellationToken ct = default)
        {
            var user = await _userRepository.GetQueryable()
                .Include(user => user.Profile)
                .FirstOrDefaultAsync(user => user.Id == userId, ct);

            if (user is null)
            {
                return Result<DashboardSummaryDto>.Failure("User was not found.");
            }

            var dto = await _cacheService.GetOrSetVersionedAsync(
                $"dashboard:user:{userId}",
                "summary",
                cancellationToken => BuildDashboardSummaryAsync(user, cancellationToken),
                TimeSpan.FromSeconds(45),
                ct);

            return Result<DashboardSummaryDto>.Success(dto);
        }

        private async Task<DashboardSummaryDto> BuildDashboardSummaryAsync(User user, CancellationToken ct)
        {
            var today = DateTime.UtcNow.Date;
            var weekStart = today.AddDays(-6);

            var workouts = await _workoutSessionRepository.GetQueryable()
                .Include(session => session.Sets)
                .Where(session => session.UserId == user.Id && session.StartTime.Date >= weekStart && session.StartTime.Date <= today)
                .ToListAsync(ct);

            var recentWorkouts = await _workoutSessionRepository.GetQueryable()
                .Include(session => session.Sets)
                .Where(session => session.UserId == user.Id)
                .OrderByDescending(session => session.StartTime)
                .Take(3)
                .ToListAsync(ct);

            var todaysMeals = await _mealRepository.GetQueryable()
                .Where(meal => meal.DietDay.Diet.UserId == user.Id && meal.DietDay.Date.Date == today)
                .ToListAsync(ct);

            var activeDiet = await _dietRepository.GetQueryable()
                .Where(diet => diet.UserId == user.Id && (!diet.EndDate.HasValue || diet.EndDate.Value >= today))
                .OrderByDescending(diet => diet.StartDate)
                .FirstOrDefaultAsync(ct);

            var caloriesGoal = user.Profile?.DailyCaloriesGoal
                ?? TryParseCaloriesGoal(activeDiet?.Description)
                ?? 2500;

            var weeklyActivity = Enumerable.Range(0, 7)
                .Select(offset =>
                {
                    var day = weekStart.AddDays(offset);
                    return new ActivityDayDto
                    {
                        Day = CultureInfo.GetCultureInfo("pl-PL").DateTimeFormat.GetAbbreviatedDayName(day.DayOfWeek),
                        Minutes = workouts
                            .Where(workout => workout.StartTime.Date == day)
                            .Sum(GetWorkoutDurationMinutes)
                    };
                })
                .ToList();

            var totalWorkoutMinutes = weeklyActivity.Sum(day => day.Minutes);

            return new DashboardSummaryDto
            {
                FirstName = user.FirstName,
                CaloriesBurned = totalWorkoutMinutes * 6,
                CaloriesConsumedToday = (int)Math.Round(todaysMeals.Sum(meal => meal.TotalCalories)),
                CaloriesGoal = caloriesGoal,
                ProteinToday = (int)Math.Round(todaysMeals.Sum(meal => meal.Protein)),
                CarbsToday = (int)Math.Round(todaysMeals.Sum(meal => meal.Carbs)),
                FatToday = (int)Math.Round(todaysMeals.Sum(meal => meal.Fats)),
                TotalSets = workouts.Sum(workout => workout.Sets.Count),
                TotalWorkouts = workouts.Count,
                WorkoutMinutesThisWeek = totalWorkoutMinutes,
                WeeklyActivity = weeklyActivity,
                RecentWorkouts = [.. recentWorkouts.Select(workout => new RecentWorkoutDto
                {
                    Id = workout.Id,
                    Name = workout.Name,
                    ExercisesCount = workout.Sets.Select(set => set.ExerciseId).Distinct().Count(),
                    Duration = $"{GetWorkoutDurationMinutes(workout)} min",
                    Date = workout.StartTime.ToString("d MMM", CultureInfo.GetCultureInfo("pl-PL"))
                })]
            };
        }

        private static int GetWorkoutDurationMinutes(WorkoutSession session)
        {
            var endTime = session.EndTime ?? DateTime.UtcNow;
            var minutes = (endTime - session.StartTime).TotalMinutes;
            return Math.Max(0, (int)Math.Round(minutes));
        }

        private static int? TryParseCaloriesGoal(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return null;
            }

            var match = CaloriesRegex.Match(description);
            return match.Success && int.TryParse(match.Value, out var calories) ? calories : null;
        }
    }
}
