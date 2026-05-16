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
            SeedExerciseMuscleGroups(systemContext);
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
            SeedGyms(systemContext);
            SeedAchievements(systemContext);
        }

        public static async Task SeedAsync(DbContext context, IPasswordHasher<User> hasher, CancellationToken ct)
        {
            var systemContext = (SystemContext)context;

            await SeedMuscleGroupsAsync(systemContext, ct);
            await SeedExercisesAsync(systemContext, ct);
            await SeedExerciseMuscleGroupsAsync(systemContext, ct);
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
            await SeedGymsAsync(systemContext, ct);
            await SeedAchievementsAsync(systemContext, ct);
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
        // Achievements
        // ──────────────────────────────────────────────

        private static void SeedAchievements(SystemContext ctx)
        {
            if (ctx.Achievements.Any()) return;
            ctx.Achievements.AddRange(BuildAchievements());
            ctx.SaveChanges();
        }

        private static async Task SeedAchievementsAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.Achievements.AnyAsync(ct)) return;
            ctx.Achievements.AddRange(BuildAchievements());
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<Achievement> BuildAchievements() =>
        [
            new Achievement { Name = "10 Treningów", Description = "Ukończono 10 sesji treningowych", IconCode = "award", AchievementType = AchievementType.WorkoutCount, TargetValue = 10 },
            new Achievement { Name = "50 Treningów", Description = "Ukończono 50 sesji treningowych", IconCode = "award", AchievementType = AchievementType.WorkoutCount, TargetValue = 50 },
            new Achievement { Name = "100 Treningów", Description = "Ukończono 100 sesji treningowych", IconCode = "award", AchievementType = AchievementType.WorkoutCount, TargetValue = 100 },
            new Achievement { Name = "Pierwszy post", Description = "Opublikuj swój pierwszy post", IconCode = "message-circle", AchievementType = AchievementType.PostCount, TargetValue = 1 },
            new Achievement { Name = "Siła drzemie we mnie", Description = "Wycisnąłeś 100 kg na klatkę (symulowane)", IconCode = "zap", AchievementType = AchievementType.PostCount, TargetValue = 100 }
        ];

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
            new Exercise { Name = "Bench Press",         Description = "Barbell flat bench press.",     IsCompound = true,  Difficulty = ExerciseDifficulty.Intermediate, ImageUrl = "https://images.unsplash.com/photo-1571019613914-85f342c6a11e?auto=format&fit=crop&w=1200&q=80" },
            new Exercise { Name = "Squat",               Description = "Barbell back squat.",           IsCompound = true,  Difficulty = ExerciseDifficulty.Intermediate, ImageUrl = "https://images.unsplash.com/photo-1534367610401-9f5ed68180aa?auto=format&fit=crop&w=1200&q=80" },
            new Exercise { Name = "Deadlift",            Description = "Conventional barbell deadlift.", IsCompound = true,  Difficulty = ExerciseDifficulty.Advanced,     ImageUrl = "https://images.unsplash.com/photo-1599058917212-d750089bc07e?auto=format&fit=crop&w=1200&q=80" },
            new Exercise { Name = "Overhead Press",      Description = "Standing barbell press.",       IsCompound = true,  Difficulty = ExerciseDifficulty.Intermediate },
            new Exercise { Name = "Barbell Row",         Description = "Bent-over barbell row.",        IsCompound = true,  Difficulty = ExerciseDifficulty.Intermediate },
            new Exercise { Name = "Pull-up",             Description = "Bodyweight pull-up.",           IsCompound = true,  Difficulty = ExerciseDifficulty.Advanced },
            new Exercise { Name = "Dumbbell Curl",       Description = "Standing dumbbell bicep curl.", IsCompound = false, Difficulty = ExerciseDifficulty.Beginner },
            new Exercise { Name = "Tricep Pushdown",     Description = "Cable tricep pushdown.",        IsCompound = false, Difficulty = ExerciseDifficulty.Beginner },
            new Exercise { Name = "Leg Press",           Description = "Machine leg press.",            IsCompound = true,  Difficulty = ExerciseDifficulty.Beginner },
            new Exercise { Name = "Lateral Raise",       Description = "Dumbbell lateral raise.",       IsCompound = false, Difficulty = ExerciseDifficulty.Beginner },
            new Exercise { Name = "Romanian Deadlift",   Description = "Barbell RDL for hamstrings.",   IsCompound = true,  Difficulty = ExerciseDifficulty.Intermediate },
            new Exercise { Name = "Incline Bench Press", Description = "Incline barbell bench press.",  IsCompound = true,  Difficulty = ExerciseDifficulty.Intermediate },
            new Exercise { Name = "Cable Fly",           Description = "Cable chest fly.",              IsCompound = false, Difficulty = ExerciseDifficulty.Beginner },
            new Exercise { Name = "Face Pull",           Description = "Rear delt face pull.",          IsCompound = false, Difficulty = ExerciseDifficulty.Beginner },
            new Exercise { Name = "Plank",               Description = "Isometric core hold.",          IsCompound = false, Difficulty = ExerciseDifficulty.Beginner },
        ];

        private static void SeedExerciseMuscleGroups(SystemContext ctx)
        {
            var exercises = ctx.Exercises
                .Include(e => e.TargetedMuscles)
                .ToList();

            AssignExerciseMuscleGroups(exercises, ctx.MuscleGroups.ToList());
            ctx.SaveChanges();
        }

        private static async Task SeedExerciseMuscleGroupsAsync(SystemContext ctx, CancellationToken ct)
        {
            var exercises = await ctx.Exercises
                .Include(e => e.TargetedMuscles)
                .ToListAsync(ct);

            var muscleGroups = await ctx.MuscleGroups.ToListAsync(ct);
            AssignExerciseMuscleGroups(exercises, muscleGroups);
            await ctx.SaveChangesAsync(ct);
        }

        private static void AssignExerciseMuscleGroups(IEnumerable<Exercise> exercises, IEnumerable<MuscleGroupEntity> muscleGroups)
        {
            var muscleGroupsByName = muscleGroups.ToDictionary(mg => mg.Name, StringComparer.OrdinalIgnoreCase);
            var mappings = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                ["Bench Press"] = ["Chest", "Triceps", "Shoulders"],
                ["Squat"] = ["Quads", "Glutes", "Hamstrings", "Core"],
                ["Deadlift"] = ["Lower Back", "Glutes", "Hamstrings", "Traps"],
                ["Overhead Press"] = ["Shoulders", "Triceps", "Core"],
                ["Barbell Row"] = ["Back", "Lats", "Biceps"],
                ["Pull-up"] = ["Lats", "Back", "Biceps"],
                ["Dumbbell Curl"] = ["Biceps", "Forearms"],
                ["Tricep Pushdown"] = ["Triceps"],
                ["Leg Press"] = ["Quads", "Glutes", "Hamstrings"],
                ["Lateral Raise"] = ["Shoulders"],
                ["Romanian Deadlift"] = ["Hamstrings", "Glutes", "Lower Back"],
                ["Incline Bench Press"] = ["Chest", "Shoulders", "Triceps"],
                ["Cable Fly"] = ["Chest"],
                ["Face Pull"] = ["Shoulders", "Traps"],
                ["Plank"] = ["Core"]
            };

            foreach (var exercise in exercises)
            {
                if (exercise.TargetedMuscles.Count > 0 || !mappings.TryGetValue(exercise.Name, out var groupNames))
                    continue;

                foreach (var groupName in groupNames)
                {
                    if (muscleGroupsByName.TryGetValue(groupName, out var muscleGroup))
                    {
                        exercise.TargetedMuscles.Add(muscleGroup);
                    }
                }
            }
        }

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
                new { Login = "admin",       First = "Admin",    Last = "System",    Email = "admin@fittrack.app",       Phone = "+48100000001", Role = Role.Administrator, Password = "Start123" },
                new { Login = "jkowalski",   First = "Jan",      Last = "Kowalski",  Email = "j.kowalski@mail.com",      Phone = "+48100000002", Role = Role.User,          Password = "Start123" },
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
            var sessionNames = new[] { "Push A", "Pull A", "Leg Day", "Upper Body", "Full Body", "Push B" };
            var descriptions = new[]
            {
                "Main compound lift plus accessory work.",
                "Controlled tempo and clean reps.",
                "Moderate volume with progressive overload.",
                "Technique-focused session.",
                "Hypertrophy block workout.",
                "Lower intensity deload-style session."
            };
            var list = new List<WorkoutSession>();
            for (int i = 0; i < users.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var start = DateTime.UtcNow.Date
                        .AddDays(-(i * 3 + j))
                        .AddHours(16 + ((i + j) % 4));
                    var durationMinutes = 50 + ((i + j) % 4) * 10;

                    list.Add(new WorkoutSession
                    {
                        UserId = users[i].Id,
                        Name = sessionNames[(i + j) % sessionNames.Length],
                        Description = descriptions[(i + j) % descriptions.Length],
                        StartTime = start,
                        EndTime = start.AddMinutes(durationMinutes),
                        GlobalSessionRpe = null,
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
            UpdateSeededWorkoutSessionRpe(ctx);
            ctx.SaveChanges();
        }

        private static async Task SeedWorkoutSetsAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.WorkoutSets.AnyAsync(ct)) return;
            ctx.WorkoutSets.AddRange(BuildWorkoutSets(ctx));
            await ctx.SaveChangesAsync(ct);
            await UpdateSeededWorkoutSessionRpeAsync(ctx, ct);
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<WorkoutSet> BuildWorkoutSets(SystemContext ctx)
        {
            var sessions = ctx.WorkoutSessions.ToList();
            var exercisesByName = ctx.Exercises.ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);
            var list = new List<WorkoutSet>();

            foreach (var (session, si) in sessions.Select((s, i) => (s, i)))
            {
                var plan = GetWorkoutPlan(session.Name);
                foreach (var (exerciseName, exerciseIndex) in plan.Select((name, i) => (name, i)))
                {
                    if (!exercisesByName.TryGetValue(exerciseName, out var exercise))
                    {
                        continue;
                    }

                    var isCompound = exercise.IsCompound;
                    var baseWeight = GetSeedBaseWeight(exerciseName) + (si % 5) * 2.5m;
                    var setCount = isCompound ? 3 : 2;

                    for (int setNumber = 1; setNumber <= setCount; setNumber++)
                    {
                        list.Add(new WorkoutSet
                        {
                            WorkoutSessionId = session.Id,
                            ExerciseId = exercise.Id,
                            SetNumber = setNumber,
                            Weight = Math.Max(0, baseWeight - (setCount - setNumber) * 2.5m),
                            Reps = isCompound ? Math.Max(5, 9 - setNumber) : 10 + setNumber,
                            Rpe = Math.Min(9, 6 + setNumber + ((si + exerciseIndex) % 2)),
                            DurationSeconds = isCompound ? 120 + setNumber * 15 : 60 + setNumber * 10,
                        });
                    }
                }
            }
            return list;
        }

        private static string[] GetWorkoutPlan(string sessionName)
        {
            return sessionName switch
            {
                "Push A" => ["Bench Press", "Overhead Press", "Tricep Pushdown", "Lateral Raise"],
                "Push B" => ["Incline Bench Press", "Cable Fly", "Overhead Press", "Tricep Pushdown"],
                "Pull A" => ["Deadlift", "Pull-up", "Barbell Row", "Face Pull", "Dumbbell Curl"],
                "Leg Day" => ["Squat", "Romanian Deadlift", "Leg Press", "Plank"],
                "Upper Body" => ["Bench Press", "Barbell Row", "Pull-up", "Lateral Raise"],
                "Full Body" => ["Squat", "Bench Press", "Barbell Row", "Plank"],
                _ => ["Bench Press", "Squat", "Barbell Row"]
            };
        }

        private static decimal GetSeedBaseWeight(string exerciseName)
        {
            return exerciseName switch
            {
                "Bench Press" => 70m,
                "Squat" => 95m,
                "Deadlift" => 120m,
                "Overhead Press" => 42.5m,
                "Barbell Row" => 65m,
                "Pull-up" => 0m,
                "Dumbbell Curl" => 14m,
                "Tricep Pushdown" => 35m,
                "Leg Press" => 160m,
                "Lateral Raise" => 8m,
                "Romanian Deadlift" => 85m,
                "Incline Bench Press" => 60m,
                "Cable Fly" => 25m,
                "Face Pull" => 30m,
                "Plank" => 0m,
                _ => 40m
            };
        }

        private static void UpdateSeededWorkoutSessionRpe(SystemContext ctx)
        {
            var sessions = ctx.WorkoutSessions
                .Include(s => s.Sets)
                .ToList();

            foreach (var session in sessions)
            {
                session.GlobalSessionRpe = CalculateGlobalSessionRpe(session.Sets);
            }
        }

        private static async Task UpdateSeededWorkoutSessionRpeAsync(SystemContext ctx, CancellationToken ct)
        {
            var sessions = await ctx.WorkoutSessions
                .Include(s => s.Sets)
                .ToListAsync(ct);

            foreach (var session in sessions)
            {
                session.GlobalSessionRpe = CalculateGlobalSessionRpe(session.Sets);
            }
        }

        private static int? CalculateGlobalSessionRpe(IEnumerable<WorkoutSet> sets)
        {
            var exerciseAverages = sets
                .Where(set => set.Rpe.HasValue)
                .GroupBy(set => set.ExerciseId)
                .Select(group => group.Average(set => set.Rpe!.Value))
                .ToList();

            return exerciseAverages.Count > 0
                ? (int)Math.Round(exerciseAverages.Average(), MidpointRounding.AwayFromZero)
                : null;
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

        // ──────────────────────────────────────────────
        // Gyms
        // ──────────────────────────────────────────────

        private static void SeedGyms(SystemContext ctx)
        {
            if (ctx.Gyms.Any())
            {
                return;
            }
            ctx.Gyms.AddRange(BuildGyms());
            ctx.SaveChanges();
        }

        private static async Task SeedGymsAsync(SystemContext ctx, CancellationToken ct)
        {
            if (await ctx.Gyms.AnyAsync(ct))
            {
                return;
            }
            ctx.Gyms.AddRange(BuildGyms());
            await ctx.SaveChangesAsync(ct);
        }

        private static IEnumerable<Gym> BuildGyms()
        {
            return new List<Gym>
            {
                new Gym { Name = "CityFit Rzeszów Plaza", Street = "Rejtana 65", City = "Rzeszów", ZipCode = "35-326", Latitude = 50.0245, Longitude = 22.0156 },
                new Gym { Name = "Just Gym Rzeszów", Street = "Lubelska 50", City = "Rzeszów", ZipCode = "35-233", Latitude = 50.0521, Longitude = 22.0089 },
                new Gym { Name = "McFIT Warszawa", Street = "Świętokrzyska 3", City = "Warszawa", ZipCode = "00-049", Latitude = 52.2356, Longitude = 21.0125 },
                new Gym { Name = "Zdrofit Warszawa Wola", Street = "Towarowa 28", City = "Warszawa", ZipCode = "00-839", Latitude = 52.2301, Longitude = 20.9856 },
                new Gym { Name = "CityFit Katowice", Street = "Rynek 1", City = "Katowice", ZipCode = "40-003", Latitude = 50.2598, Longitude = 19.0215 },
                new Gym { Name = "Smart Gym Katowice", Street = "Roździeńskiego 1A", City = "Katowice", ZipCode = "40-202", Latitude = 50.2645, Longitude = 19.0305 },
                new Gym { Name = "Fitness Platinum Kraków", Street = "Pawia 5", City = "Kraków", ZipCode = "31-154", Latitude = 50.0682, Longitude = 19.9475 },
                new Gym { Name = "MyFitnessPlace Kraków", Street = "Szlak 77", City = "Kraków", ZipCode = "31-153", Latitude = 50.0715, Longitude = 19.9412 },
                new Gym { Name = "Fitness Academy Wrocław", Street = "Legnicka 58", City = "Wrocław", ZipCode = "54-204", Latitude = 51.1235, Longitude = 16.9856 },
                new Gym { Name = "McFIT Wrocław", Street = "Kazimierza Wielkiego 1", City = "Wrocław", ZipCode = "50-077", Latitude = 51.1095, Longitude = 17.0325 },
                new Gym { Name = "Zdrofit Gdańsk", Street = "Targ Sienny 7", City = "Gdańsk", ZipCode = "80-806", Latitude = 54.3485, Longitude = 18.6456 },
                new Gym { Name = "CityFit Gdańsk", Street = "Aleja Grunwaldzka 472", City = "Gdańsk", ZipCode = "80-309", Latitude = 54.4056, Longitude = 18.5756 },
                new Gym { Name = "Just Gym Poznań", Street = "Szwajcarska 14", City = "Poznań", ZipCode = "61-285", Latitude = 52.3925, Longitude = 16.9985 },
                new Gym { Name = "Fitness Academy Poznań", Street = "Półwiejska 42", City = "Poznań", ZipCode = "61-888", Latitude = 52.4005, Longitude = 16.9325 },
                new Gym { Name = "Smart Gym Gliwice", Street = "Lipowa 1", City = "Gliwice", ZipCode = "44-100", Latitude = 50.3015, Longitude = 18.6756 }
            };
        }
    }
}
