namespace MobileDevelopment.API.Models.DTO.Dashboard
{
    public class DashboardSummaryDto
    {
        public string FirstName { get; set; } = string.Empty;
        public int CaloriesBurned { get; set; }
        public int CaloriesConsumedToday { get; set; }
        public int CaloriesGoal { get; set; }
        public int ProteinToday { get; set; }
        public int CarbsToday { get; set; }
        public int FatToday { get; set; }
        public int TotalSets { get; set; }
        public int TotalWorkouts { get; set; }
        public int WorkoutMinutesThisWeek { get; set; }
        public List<ActivityDayDto> WeeklyActivity { get; set; } = new();
        public List<RecentWorkoutDto> RecentWorkouts { get; set; } = new();
    }

    public class ActivityDayDto
    {
        public string Day { get; set; } = string.Empty;
        public int Minutes { get; set; }
    }

    public class RecentWorkoutDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ExercisesCount { get; set; }
        public string Duration { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }
}
