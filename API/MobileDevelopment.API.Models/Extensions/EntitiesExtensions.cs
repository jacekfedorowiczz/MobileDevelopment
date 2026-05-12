using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Models.DTO.Comments;
using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.DTO.Exercises;
using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.DTO.PostLikes;
using MobileDevelopment.API.Models.DTO.Posts;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.DTO.RefreshTokens;
using MobileDevelopment.API.Models.DTO.Tags;
using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.DTO.WorkoutSets;

namespace MobileDevelopment.API.Models.Extensions
{
    public static class EntitiesExtensions
    {
        public static UserDto ToDto(this User user) =>
            new(
                Id: user.Id,
                Login: user.Login,
                FirstName: user.FirstName,
                LastName: user.LastName,
                FullName: user.FullName,
                Email: user.Email,
                MobilePhone: user.MobilePhone,
                CreatedAt: user.CreatedAt,
                DateOfBirth: user.DateOfBirth,
                Age: user.Age,
                Role: user.Role,
                ProfileId: user.ProfileId
            );

        public static ProfileDto ToDto(this Profile profile) =>
            new(
                Id: profile.Id,
                UserId: profile.UserId,
                Avatar: profile.Avatar,
                Age: profile.Age,
                Weight: profile.Weight,
                Height: profile.Height,
                PreferredWeightUnit: profile.PreferredWeightUnit,
                CurrentGoal: profile.CurrentGoal
            );

        public static ExerciseDto ToDto(this Exercise exercise) =>
            new(
                Id: exercise.Id,
                Name: exercise.Name,
                Description: exercise.Description,
                IsCompound: exercise.IsCompound
            );

        public static MuscleGroupDto ToDto(this MuscleGroup muscleGroup) =>
            new(
                Id: muscleGroup.Id,
                Name: muscleGroup.Name
            );

        public static WorkoutSessionDto ToDto(this WorkoutSession session) =>
            new(
                Id: session.Id,
                UserId: session.UserId,
                Name: session.Name,
                Description: session.Description,
                StartTime: session.StartTime,
                EndTime: session.EndTime,
                GlobalSessionRpe: session.GlobalSessionRpe
            );

        public static WorkoutSetDto ToDto(this WorkoutSet set) =>
            new(
                Id: set.Id,
                WorkoutSessionId: set.WorkoutSessionId,
                ExerciseId: set.ExerciseId,
                SetNumber: set.SetNumber,
                Weight: set.Weight,
                Reps: set.Reps,
                Rpe: set.Rpe
            );

        public static DietDto ToDto(this Diet diet) =>
            new(
                Id: diet.Id,
                UserId: diet.UserId,
                Name: diet.Name,
                Description: diet.Description,
                StartDate: diet.StartDate,
                EndDate: diet.EndDate,
                IsActive: diet.IsActive
            );

        public static DietDayDto ToDto(this DietDay dietDay) =>
            new(
                Id: dietDay.Id,
                DietId: dietDay.DietId,
                Date: dietDay.Date,
                Notes: dietDay.Notes
            );

        public static MealDto ToDto(this Meal meal) =>
            new(
                Id: meal.Id,
                DietDayId: meal.DietDayId,
                Name: meal.Name,
                Time: meal.Time,
                TotalCalories: meal.TotalCalories,
                Protein: meal.Protein,
                Carbs: meal.Carbs,
                Fats: meal.Fats
            );

        public static TagDto ToDto(this Tag tag) =>
            new(
                Id: tag.Id,
                Name: tag.Name
            );

        public static PostDto ToDto(this Post post) =>
            new(
                Id: post.Id,
                UserId: post.UserId,
                Title: post.Title,
                Content: post.Content,
                CreatedAt: post.CreatedAt,
                TargetGoal: post.TargetGoal
            );

        public static CommentDto ToDto(this Comment comment) =>
            new(
                Id: comment.Id,
                PostId: comment.PostId,
                UserId: comment.UserId,
                Content: comment.Content,
                CreatedAt: comment.CreatedAt
            );

        public static PostLikeDto ToDto(this PostLike postLike) =>
            new(
                Id: postLike.Id,
                PostId: postLike.PostId,
                UserId: postLike.UserId,
                CreatedAt: postLike.CreatedAt
            );

        public static RefreshTokenDto ToDto(this RefreshToken refreshToken) =>
            new(
                Id: refreshToken.Id,
                UserId: refreshToken.UserId,
                Token: refreshToken.Token,
                ExpiresAt: refreshToken.ExpiresAt,
                CreatedAt: refreshToken.CreatedAt,
                RevokedAt: refreshToken.RevokedAt,
                IsRevoked: refreshToken.IsRevoked,
                IsExpired: refreshToken.IsExpired,
                IsActive: refreshToken.IsActive
            );
    }
}
