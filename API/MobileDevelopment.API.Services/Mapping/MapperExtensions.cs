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
using MobileDevelopment.API.Models.DTO.Tags;
using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.DTO.WorkoutSets;

namespace MobileDevelopment.API.Services.Mapping
{
    public static class MapperExtensions
    {
        public static UserDto ToDto(this User entity, bool includeProfile = false, bool includeSessions = false)
        {
            if (entity is null)
            {
                return null!;
            }

            return new UserDto(
                entity.Id,
                entity.Login,
                entity.FirstName,
                entity.LastName,
                entity.FullName,
                entity.Email,
                entity.MobilePhone,
                entity.CreatedAt,
                entity.DateOfBirth,
                entity.Age,
                entity.Role,
                entity.ProfileId,
                includeProfile && entity.Profile != null ? entity.Profile.ToDto() : null,
                includeSessions && entity.Sessions != null ? entity.Sessions.Select(s => s.ToDto()).ToList() : null
            );
        }

        public static ProfileDto ToDto(this Profile entity, bool includeUser = false, bool includeInterests = false)
        {
            if (entity is null)
            {
                return null!;
            }

            return new ProfileDto(
                entity.Id,
                entity.UserId,
                entity.Avatar,
                entity.Age,
                entity.Weight,
                entity.Height,
                entity.PreferredWeightUnit,
                entity.CurrentGoal,
                includeUser && entity.User != null ? entity.User.ToDto() : null,
                includeInterests && entity.Interests != null ? entity.Interests.Select(i => i.ToDto()).ToList() : null
            );
        }

        public static PostDto ToDto(this Post entity, bool includeUser = false, bool includeComments = false, bool includeLikes = false, bool includeTags = false)
        {
            if (entity is null)
            {
                return null!;
            }
              
            return new PostDto(
                entity.Id,
                entity.UserId,
                entity.Title,
                entity.Content,
                entity.CreatedAt,
                entity.TargetGoal,
                includeUser && entity.User != null ? entity.User.ToDto() : null,
                includeComments && entity.Comments != null ? entity.Comments.Select(c => c.ToDto()).ToList() : null,
                includeLikes && entity.Likes != null ? entity.Likes.Select(l => l.ToDto()).ToList() : null,
                includeTags && entity.Tags != null ? entity.Tags.Select(t => t.ToDto()).ToList() : null
            );
        }

        public static CommentDto ToDto(this Comment entity, bool includePost = false, bool includeUser = false)
        {
            if (entity is null)
            {
                return null!;
            }

            return new CommentDto(
                entity.Id,
                entity.PostId,
                entity.UserId,
                entity.Content,
                entity.CreatedAt,
                includePost && entity.Post != null ? entity.Post.ToDto() : null,
                includeUser && entity.User != null ? entity.User.ToDto() : null
            );
        }

        public static PostLikeDto ToDto(this PostLike entity, bool includePost = false, bool includeUser = false)
        {
            if (entity is null)
            {
                return null!;
            }
                

            return new PostLikeDto(
                entity.Id,
                entity.PostId,
                entity.UserId,
                entity.CreatedAt,
                includePost && entity.Post != null ? entity.Post.ToDto() : null,
                includeUser && entity.User != null ? entity.User.ToDto() : null
            );
        }

        public static TagDto ToDto(this Tag entity, bool includePosts = false, bool includeInterestedProfiles = false)
        {
            if (entity is null)
            {
                return null!;
            }
                
            return new TagDto(
                entity.Id,
                entity.Name,
                includePosts && entity.Posts != null ? entity.Posts.Select(p => p.ToDto()).ToList() : null,
                includeInterestedProfiles && entity.InterestedProfiles != null ? entity.InterestedProfiles.Select(p => p.ToDto()).ToList() : null
            );
        }

        public static WorkoutSessionDto ToDto(this WorkoutSession entity, bool includeUser = false, bool includeSets = false)
        {
            if (entity is null)
            {
                return null!;
            }

            return new WorkoutSessionDto(
                entity.Id,
                entity.UserId,
                entity.Name,
                entity.Description,
                entity.StartTime,
                entity.EndTime,
                entity.GlobalSessionRpe,
                includeUser && entity.User != null ? entity.User.ToDto() : null,
                includeSets && entity.Sets != null ? entity.Sets.Select(s => s.ToDto()).ToList() : null
            );
        }

        public static WorkoutSetDto ToDto(this WorkoutSet entity, bool includeWorkoutSession = false, bool includeExercise = false)
        {
            if (entity is null)
            {
                return null!;
            }

            return new WorkoutSetDto(
                entity.Id,
                entity.WorkoutSessionId,
                entity.ExerciseId,
                entity.SetNumber,
                entity.Weight,
                entity.Reps,
                entity.Rpe,
                includeWorkoutSession && entity.WorkoutSession != null ? entity.WorkoutSession.ToDto() : null,
                includeExercise && entity.Exercise != null ? entity.Exercise.ToDto() : null
            );
        }

        public static ExerciseDto ToDto(this Exercise entity, bool includeTargetedMuscles = false, bool includeSets = false)
        {
            if (entity is null)
            {
                return null!;
            }

            return new ExerciseDto(
                entity.Id,
                entity.Name,
                entity.Description,
                entity.IsCompound,
                includeTargetedMuscles && entity.TargetedMuscles != null ? entity.TargetedMuscles.Select(m => m.ToDto()).ToList() : null,
                includeSets && entity.Sets != null ? entity.Sets.Select(s => s.ToDto()).ToList() : null
            );
        }

        public static MuscleGroupDto ToDto(this MuscleGroup entity, bool includeExercises = false)
        {
            if (entity is null)
            {
                return null!;
            }

            return new MuscleGroupDto(
                entity.Id,
                entity.Name,
                includeExercises && entity.Exercises != null ? entity.Exercises.Select(e => e.ToDto()).ToList() : null
            );
        }

        public static DietDto ToDto(this Diet entity, bool includeUser = false, bool includeDietDays = false)
        {
            if (entity is null)
            {
                return null!;
            }

            return new DietDto(
                entity.Id,
                entity.UserId,
                entity.Name,
                entity.Description,
                entity.StartDate,
                entity.EndDate,
                entity.IsActive,
                includeUser && entity.User != null ? entity.User.ToDto() : null,
                includeDietDays && entity.DietDays != null ? entity.DietDays.Select(d => d.ToDto()).ToList() : null
            );
        }

        public static DietDayDto ToDto(this DietDay entity, bool includeDiet = false, bool includeMeals = false)
        {
            if (entity is null)
            {
                return null!;
            }

            return new DietDayDto(
                entity.Id,
                entity.DietId,
                entity.Date,
                entity.Notes,
                includeDiet && entity.Diet != null ? entity.Diet.ToDto() : null,
                includeMeals && entity.Meals != null ? entity.Meals.Select(m => m.ToDto()).ToList() : null
            );
        }

        public static MealDto ToDto(this Meal entity, bool includeDietDay = false)
        {
            if (entity is null)
            {
                return null!;
            }

            return new MealDto(
                entity.Id,
                entity.DietDayId,
                entity.Name,
                entity.Time,
                entity.TotalCalories,
                entity.Protein,
                entity.Carbs,
                entity.Fats,
                includeDietDay && entity.DietDay != null ? entity.DietDay.ToDto() : null
            );
        }
    }
}
