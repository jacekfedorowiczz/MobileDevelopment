using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Persistence.Context;
using MuscleGroupEntity = MobileDevelopment.API.Domain.Entities.MuscleGroup;

namespace MobileDevelopment.API.Persistence.Seeding
{
    internal static class DatabaseSeeder
    {
        public static void Seed(DbContext context, IPasswordHasher<User> hasher)
        {
            var systemContext = (SystemContext)context;

            SeedMuscleGroups(systemContext);
            SeedExercises(systemContext);
            SeedTags(systemContext);
            SeedUsersAndProfiles(systemContext, hasher);
            SeedWorkoutSessions(systemContext);
            SeedWorkoutSets(systemContext);
            SeedPosts(systemContext);
            SeedComments(systemContext);
            SeedPostLikes(systemContext);
            SeedDiets(systemContext);
            SeedDietDays(systemContext);
            SeedMeals(systemContext);
        }

        public static async Task SeedAsync(DbContext context, IPasswordHasher<User> hasher, CancellationToken ct)
        {
            var systemContext = (SystemContext)context;

            await SeedMuscleGroupsAsync(systemContext, ct);
            await SeedExercisesAsync(systemContext, ct);
            await SeedTagsAsync(systemContext, ct);
            await SeedUsersAndProfilesAsync(systemContext, hasher, ct);
            await SeedWorkoutSessionsAsync(systemContext, ct);
            await SeedWorkoutSetsAsync(systemContext, ct);
            await SeedPostsAsync(systemContext, ct);
            await SeedCommentsAsync(systemContext, ct);
            await SeedPostLikesAsync(systemContext, ct);
            await SeedDietsAsync(systemContext, ct);
            await SeedDietDaysAsync(systemContext, ct);
            await SeedMealsAsync(systemContext, ct);
        }

        // ──────────────────────────────────────────────
        // MuscleGroups
        // ──────────────────────────────────────────────

        private static readonly string[] MuscleGroupNames =
        [
            "Chest", "Back", "Lats", "Traps", "Lower Back",
            "Shoulders", "Biceps", "Triceps", "Forearms",
            "Core", "Glutes", "Quads", "Hamstrings", "Calves"
        ];

        private static void SeedMuscleGroups(SystemContext ctx)
        {
            if (ctx.MuscleGroups.Any()) return;
            ctx.MuscleGroups.AddRange(MuscleGroupNames.Select(n => new MuscleGroupEntity { Name = n }));
            ctx.SaveChanges();
        }

        private static async Task SeedMuscleGroupsAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.MuscleGroups.AnyAsync(ct)) return;
            ctx.MuscleGroups.AddRange(MuscleGroupNames.Select(n => new MuscleGroupEntity { Name = n }));
            await ctx.SaveChangesAsync(ct);
        }

        // ──────────────────────────────────────────────
        // Exercises
        // ──────────────────────────────────────────────

        private static void SeedExercises(SystemContext ctx)
        {
            if (ctx.Exercises.Any()) return;
            ctx.Exercises.AddRange(BuildExercises());
            ctx.SaveChanges();
        }

        private static async Task SeedExercisesAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.Exercises.AnyAsync(ct)) return;
            ctx.Exercises.AddRange(BuildExercises());
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<Exercise> BuildExercises() =>
        [
            new Exercise { Name = "Bench Press",        Description = "Barbell flat bench press.",    IsCompound = true  },
            new Exercise { Name = "Squat",              Description = "Barbell back squat.",           IsCompound = true  },
            new Exercise { Name = "Deadlift",           Description = "Conventional barbell deadlift.", IsCompound = true },
            new Exercise { Name = "Overhead Press",     Description = "Standing barbell press.",       IsCompound = true  },
            new Exercise { Name = "Barbell Row",        Description = "Bent-over barbell row.",        IsCompound = true  },
            new Exercise { Name = "Pull-up",            Description = "Bodyweight pull-up.",           IsCompound = true  },
            new Exercise { Name = "Dumbbell Curl",      Description = "Standing dumbbell bicep curl.", IsCompound = false },
            new Exercise { Name = "Tricep Pushdown",    Description = "Cable tricep pushdown.",        IsCompound = false },
            new Exercise { Name = "Leg Press",          Description = "Machine leg press.",            IsCompound = true  },
            new Exercise { Name = "Lateral Raise",      Description = "Dumbbell lateral raise.",       IsCompound = false },
            new Exercise { Name = "Romanian Deadlift",  Description = "Barbell RDL for hamstrings.",   IsCompound = true  },
            new Exercise { Name = "Incline Bench Press",Description = "Incline barbell bench press.",  IsCompound = true  },
            new Exercise { Name = "Cable Fly",          Description = "Cable chest fly.",              IsCompound = false },
            new Exercise { Name = "Face Pull",          Description = "Rear delt face pull.",          IsCompound = false },
            new Exercise { Name = "Plank",              Description = "Isometric core hold.",          IsCompound = false },
        ];

        // ──────────────────────────────────────────────
        // Tags
        // ──────────────────────────────────────────────

        private static readonly string[] TagNames =
        [
            "Strength", "Hypertrophy", "Cardio", "Nutrition", "Mobility",
            "Beginner", "Intermediate", "Advanced", "Powerlifting", "Bodybuilding",
            "Weight Loss", "Muscle Gain", "Recovery", "Mindset", "Supplements"
        ];

        private static void SeedTags(SystemContext ctx)
        {
            if (ctx.Tags.Any()) return;
            ctx.Tags.AddRange(TagNames.Select(n => new Tag { Name = n }));
            ctx.SaveChanges();
        }

        private static async Task SeedTagsAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.Tags.AnyAsync(ct)) return;
            ctx.Tags.AddRange(TagNames.Select(n => new Tag { Name = n }));
            await ctx.SaveChangesAsync(ct);
        }

        // ──────────────────────────────────────────────
        // Users & Profiles
        // ──────────────────────────────────────────────

        private static void SeedUsersAndProfiles(SystemContext ctx, IPasswordHasher<User> hasher)
        {
            if (ctx.Users.Any() || ctx.Profiles.Any()) return;
            ctx.Users.AddRange(BuildUsersAndProfiles(hasher));
            ctx.SaveChanges();
        }

        private static async Task SeedUsersAndProfilesAsync(SystemContext ctx, IPasswordHasher<User> hasher, CancellationToken ct)
        {
            if (await ctx.Users.AnyAsync(ct) || await ctx.Profiles.AnyAsync(ct)) return;
            ctx.Users.AddRange(BuildUsersAndProfiles(hasher));
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<User> BuildUsersAndProfiles(IPasswordHasher<User> hasher)
        {
            var rawUsers = new[]
            {
                new { Login = "admin",       First = "Admin",    Last = "System",    Email = "admin@fittrack.app",       Phone = "+48100000001", Role = Role.Administrator, Password = "Admin@1234" },
                new { Login = "jkowalski",   First = "Jan",      Last = "Kowalski",  Email = "j.kowalski@mail.com",      Phone = "+48100000002", Role = Role.User,          Password = "Test@1234" },
                new { Login = "anowak",      First = "Anna",     Last = "Nowak",     Email = "a.nowak@mail.com",         Phone = "+48100000003", Role = Role.User,          Password = "Test@1234" },
                new { Login = "pzielinski",  First = "Piotr",    Last = "Zieliński", Email = "p.zielinski@mail.com",     Phone = "+48100000004", Role = Role.User,          Password = "Test@1234" },
                new { Login = "mwisniewska", First = "Marta",    Last = "Wiśniewska",Email = "m.wisniewska@mail.com",    Phone = "+48100000005", Role = Role.User,          Password = "Test@1234" },
                new { Login = "kwojcik",     First = "Kamil",    Last = "Wójcik",    Email = "k.wojcik@mail.com",        Phone = "+48100000006", Role = Role.User,          Password = "Test@1234" },
                new { Login = "elewandowska",First = "Ewa",      Last = "Lewandowska",Email = "e.lewandowska@mail.com",  Phone = "+48100000007", Role = Role.User,          Password = "Test@1234" },
                new { Login = "tdabrowski",  First = "Tomasz",   Last = "Dąbrowski", Email = "t.dabrowski@mail.com",     Phone = "+48100000008", Role = Role.User,          Password = "Test@1234" },
                new { Login = "kszymanska",  First = "Katarzyna",Last = "Szymańska", Email = "k.szymanska@mail.com",     Phone = "+48100000009", Role = Role.User,          Password = "Test@1234" },
                new { Login = "mwojcik",     First = "Michał",   Last = "Wójcik",    Email = "m.wojcik2@mail.com",       Phone = "+48100000010", Role = Role.User,          Password = "Test@1234" },
                new { Login = "bkaczmarek",  First = "Barbara",  Last = "Kaczmarek", Email = "b.kaczmarek@mail.com",     Phone = "+48100000011", Role = Role.User,          Password = "Test@1234" },
                new { Login = "rpawlak",     First = "Robert",   Last = "Pawlak",    Email = "r.pawlak@mail.com",        Phone = "+48100000012", Role = Role.User,          Password = "Test@1234" },
            };

            var goals = Enum.GetValues<FitnessGoal>();
            var units = Enum.GetValues<WeightUnits>();

            return rawUsers.Select((u, i) =>
            {
                var profile = new Profile
                {
                    Age = 20 + (i * 3 % 30),
                    Weight = 60m + i * 4,
                    Height = 160m + i * 2,
                    PreferredWeightUnit = units[i % units.Length],
                    IsDarkModeEnabled = i % 2 == 0,
                    CurrentGoal = goals[i % goals.Length],
                };

                var user = new User
                {
                    Login = u.Login,
                    FirstName = u.First,
                    LastName = u.Last,
                    Email = u.Email,
                    MobilePhone = u.Phone,
                    Role = u.Role,
                    CreatedAt = DateTime.UtcNow,
                    PasswordHash = string.Empty,
                    DateOfBirth = new DateOnly(1996, 02, 29),
                    Profile = profile
                };

                user.PasswordHash = hasher.HashPassword(user, u.Password);
                return user;
            });
        }

        // ──────────────────────────────────────────────
        // WorkoutSessions
        // ──────────────────────────────────────────────

        private static void SeedWorkoutSessions(SystemContext ctx)
        {
            if (ctx.WorkoutSessions.Any()) return;
            ctx.WorkoutSessions.AddRange(BuildWorkoutSessions(ctx));
            ctx.SaveChanges();
        }

        private static async Task SeedWorkoutSessionsAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.WorkoutSessions.AnyAsync(ct)) return;
            ctx.WorkoutSessions.AddRange(BuildWorkoutSessions(ctx));
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<WorkoutSession> BuildWorkoutSessions(SystemContext ctx)
        {
            var users = ctx.Users.ToList();
            var sessionNames = new[] { "Push Day", "Pull Day", "Leg Day", "Upper Body", "Full Body", "Cardio Session" };
            var list = new List<WorkoutSession>();
            for (int i = 0; i < users.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var start = DateTime.UtcNow.AddDays(-(i * 3 + j));
                    list.Add(new WorkoutSession
                    {
                        UserId = users[i].Id,
                        Name = sessionNames[(i + j) % sessionNames.Length],
                        Description = $"Training session #{j + 1}",
                        StartTime = start,
                        EndTime = start.AddHours(1.5),
                        GlobalSessionRpe = 6 + (j % 4),
                    });
                }
            }
            return list;
        }

        // ──────────────────────────────────────────────
        // WorkoutSets
        // ──────────────────────────────────────────────

        private static void SeedWorkoutSets(SystemContext ctx)
        {
            if (ctx.WorkoutSets.Any()) return;
            ctx.WorkoutSets.AddRange(BuildWorkoutSets(ctx));
            ctx.SaveChanges();
        }

        private static async Task SeedWorkoutSetsAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.WorkoutSets.AnyAsync(ct)) return;
            ctx.WorkoutSets.AddRange(BuildWorkoutSets(ctx));
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<WorkoutSet> BuildWorkoutSets(SystemContext ctx)
        {
            var sessions = ctx.WorkoutSessions.ToList();
            var exercises = ctx.Exercises.ToList();
            var list = new List<WorkoutSet>();
            foreach (var (session, si) in sessions.Select((s, i) => (s, i)))
            {
                for (int setNum = 1; setNum <= 4; setNum++)
                {
                    list.Add(new WorkoutSet
                    {
                        WorkoutSessionId = session.Id,
                        ExerciseId = exercises[(si + setNum) % exercises.Count].Id,
                        SetNumber = setNum,
                        Weight = 60m + si * 5 + setNum * 2.5m,
                        Reps = 8 + setNum,
                        Rpe = 7 + (setNum % 3),
                    });
                }
            }
            return list;
        }

        // ──────────────────────────────────────────────
        // Posts
        // ──────────────────────────────────────────────

        private static void SeedPosts(SystemContext ctx)
        {
            if (ctx.Posts.Any()) return;
            ctx.Posts.AddRange(BuildPosts(ctx));
            ctx.SaveChanges();
        }

        private static async Task SeedPostsAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.Posts.AnyAsync(ct)) return;
            ctx.Posts.AddRange(BuildPosts(ctx));
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<Post> BuildPosts(SystemContext ctx)
        {
            var users = ctx.Users.ToList();
            var goals = Enum.GetValues<FitnessGoal>();
            var titles = new[]
            {
                "My first week on the program", "Hit a new PR today!", "Nutrition tips that changed everything",
                "Why recovery matters more than you think", "Beginner's guide to progressive overload",
                "Meal prep Sunday – what I cooked", "How I lost 10 kg in 3 months", "Strength vs hypertrophy training",
                "My go-to pre-workout meal", "Lessons from my first powerlifting meet",
                "Top 5 exercises for bigger lats", "Why I switched to RPE-based training",
                "Sleep and performance – the missing link", "Tracking macros: is it worth it?",
                "6-month transformation update"
            };

            return users.SelectMany((u, ui) =>
                Enumerable.Range(0, 2).Select(pi => new Post
                {
                    UserId = u.Id,
                    Title = titles[(ui * 2 + pi) % titles.Length],
                    Content = $"This is a detailed post about {titles[(ui * 2 + pi) % titles.Length].ToLower()}. Sharing my experience and insights with the community.",
                    CreatedAt = DateTime.UtcNow.AddDays(-(ui + pi * 2)),
                    TargetGoal = goals[(ui + pi) % goals.Length],
                }));
        }

        // ──────────────────────────────────────────────
        // Comments
        // ──────────────────────────────────────────────

        private static void SeedComments(SystemContext ctx)
        {
            if (ctx.Comments.Any()) return;
            ctx.Comments.AddRange(BuildComments(ctx));
            ctx.SaveChanges();
        }

        private static async Task SeedCommentsAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.Comments.AnyAsync(ct)) return;
            ctx.Comments.AddRange(BuildComments(ctx));
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<Comment> BuildComments(SystemContext ctx)
        {
            var posts = ctx.Posts.ToList();
            var users = ctx.Users.ToList();
            var contents = new[]
            {
                "Great post, very motivating!", "Thanks for sharing your experience.",
                "I had the same results, keep it up!", "Could you share more details about your diet?",
                "This is exactly what I needed to read today.", "Incredible progress, well done!",
                "Have you tried adding more volume?", "Solid advice, following this approach as well.",
                "What program are you running?", "Love the consistency, inspiring!",
                "How long did it take to see results?", "I appreciate the detailed breakdown.",
            };

            var list = new List<Comment>();
            for (int pi = 0; pi < posts.Count; pi++)
            {
                for (int ci = 0; ci < 2; ci++)
                {
                    list.Add(new Comment
                    {
                        PostId = posts[pi].Id,
                        UserId = users[(pi + ci + 1) % users.Count].Id,
                        Content = contents[(pi * 2 + ci) % contents.Length],
                        CreatedAt = DateTime.UtcNow.AddHours(-(pi + ci)),
                    });
                }
            }
            return list;
        }

        // ──────────────────────────────────────────────
        // PostLikes
        // ──────────────────────────────────────────────

        private static void SeedPostLikes(SystemContext ctx)
        {
            if (ctx.PostLikes.Any()) return;
            ctx.PostLikes.AddRange(BuildPostLikes(ctx));
            ctx.SaveChanges();
        }

        private static async Task SeedPostLikesAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.PostLikes.AnyAsync(ct)) return;
            ctx.PostLikes.AddRange(BuildPostLikes(ctx));
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<PostLike> BuildPostLikes(SystemContext ctx)
        {
            var posts = ctx.Posts.ToList();
            var users = ctx.Users.ToList();
            var seen = new HashSet<(int, int)>();
            var list = new List<PostLike>();

            for (int pi = 0; pi < posts.Count; pi++)
            {
                for (int ui = 1; ui <= 3; ui++)
                {
                    var userId = users[(pi + ui) % users.Count].Id;
                    var postId = posts[pi].Id;
                    if (!seen.Add((postId, userId))) continue;

                    list.Add(new PostLike
                    {
                        PostId = postId,
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow.AddMinutes(-(pi * 10 + ui)),
                    });
                }
            }
            return list;
        }

        // ──────────────────────────────────────────────
        // Diets
        // ──────────────────────────────────────────────

        private static void SeedDiets(SystemContext ctx)
        {
            if (ctx.Diets.Any()) return;
            ctx.Diets.AddRange(BuildDiets(ctx));
            ctx.SaveChanges();
        }

        private static async Task SeedDietsAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.Diets.AnyAsync(ct)) return;
            ctx.Diets.AddRange(BuildDiets(ctx));
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<Diet> BuildDiets(SystemContext ctx)
        {
            var users = ctx.Users.ToList();
            var dietNames = new[] { "Bulking Plan", "Cutting Phase", "Maintenance Diet", "High Protein Plan", "Mediterranean Diet" };

            return users.SelectMany((u, ui) =>
                Enumerable.Range(0, 2).Select(di => new Diet
                {
                    UserId = u.Id,
                    Name = dietNames[(ui * 2 + di) % dietNames.Length],
                    Description = $"A structured eating plan focused on {dietNames[(ui * 2 + di) % dietNames.Length].ToLower()}.",
                    StartDate = DateTime.UtcNow.AddMonths(-(ui + di + 1)),
                    EndDate = di == 0 ? null : DateTime.UtcNow.AddMonths(3),
                }));
        }

        // ──────────────────────────────────────────────
        // DietDays
        // ──────────────────────────────────────────────

        private static void SeedDietDays(SystemContext ctx)
        {
            if (ctx.DietDays.Any()) return;
            ctx.DietDays.AddRange(BuildDietDays(ctx));
            ctx.SaveChanges();
        }

        private static async Task SeedDietDaysAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.DietDays.AnyAsync(ct)) return;
            ctx.DietDays.AddRange(BuildDietDays(ctx));
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<DietDay> BuildDietDays(SystemContext ctx)
        {
            var diets = ctx.Diets.ToList();
            var list = new List<DietDay>();
            foreach (var (diet, di) in diets.Select((d, i) => (d, i)))
            {
                for (int dayOff = 0; dayOff < 7; dayOff++)
                {
                    list.Add(new DietDay
                    {
                        DietId = diet.Id,
                        Date = diet.StartDate.AddDays(dayOff),
                        Notes = dayOff == 0 ? "First day – stay consistent!" : null,
                    });
                }
            }
            return list;
        }

        // ──────────────────────────────────────────────
        // Meals
        // ──────────────────────────────────────────────

        private static void SeedMeals(SystemContext ctx)
        {
            if (ctx.Meals.Any()) return;
            ctx.Meals.AddRange(BuildMeals(ctx));
            ctx.SaveChanges();
        }

        private static async Task SeedMealsAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.Meals.AnyAsync(ct)) return;
            ctx.Meals.AddRange(BuildMeals(ctx));
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<Meal> BuildMeals(SystemContext ctx)
        {
            var dietDays = ctx.DietDays.ToList();
            var mealNames = new[] { "Breakfast", "Second Breakfast", "Lunch", "Pre-Workout", "Dinner" };
            var list = new List<Meal>();

            foreach (var (day, di) in dietDays.Select((d, i) => (d, i)))
            {
                for (int mi = 0; mi < 3; mi++)
                {
                    list.Add(new Meal
                    {
                        DietDayId = day.Id,
                        Name = mealNames[(di + mi) % mealNames.Length],
                        Time = TimeSpan.FromHours(7 + mi * 4),
                        TotalCalories = 400m + mi * 150 + di * 10,
                        Protein = 30m + mi * 5,
                        Carbs = 50m + mi * 10,
                        Fats = 15m + mi * 3,
                    });
                }
            }
            return list;
        }
    }
}
