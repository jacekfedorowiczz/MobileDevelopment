using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Domain.Interfaces.Auth;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;
using MobileDevelopment.API.Services.Model;
using MobileDevelopment.API.Services.Services.Base;
using ZetaLongPaths;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class ProfileService : BaseService<Profile, ProfileDto, CreateEditProfileDto>, IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IWorkoutSessionRepository _workoutSessionRepository;
        private readonly IUserContext _userContext;
        private readonly ICacheService _cacheService;
        private readonly IWebHostEnvironment _environment;

        public ProfileService(
            IProfileRepository profileRepository,
            IWorkoutSessionRepository workoutSessionRepository,
            IUserContext userContext,
            ICacheService cacheService,
            IWebHostEnvironment environment) : base(profileRepository)
        {
            _profileRepository = profileRepository;
            _workoutSessionRepository = workoutSessionRepository;
            _userContext = userContext;
            _cacheService = cacheService;
            _environment = environment;
        }

        public override async Task<Result<int>> CreateAsync(CreateEditProfileDto dto, CancellationToken token = default)
        {
            var userId = dto.UserId ?? _userContext.UserId;
            if (userId is null)
            {
                return Result<int>.Failure("User id is required.");
            }

            var existingProfile = await _profileRepository.GetByUserIdAsync(userId.Value, token);
            if (existingProfile is not null)
            {
                return Result<int>.Failure("Profile already exists.");
            }

            var profile = new Profile
            {
                UserId = userId.Value,
                Avatar = NormalizeOptionalText(dto.Avatar),
                Weight = dto.Weight ?? 0m,
                Height = dto.Height ?? 0m,
                PreferredWeightUnit = dto.PreferredWeightUnit ?? Domain.Enums.WeightUnits.Kilos,
                CurrentGoal = dto.CurrentGoal ?? Domain.Enums.FitnessGoal.Maintain,
                DietType = NormalizeOptionalText(dto.DietType),
                DailyCaloriesGoal = dto.DailyCaloriesGoal,
                ProteinPercentage = dto.ProteinPercentage,
                CarbsPercentage = dto.CarbsPercentage,
                FatPercentage = dto.FatPercentage
            };

            var validation = ValidateDietAssumptions(dto, requireComplete: false);
            if (!validation.IsSuccess)
            {
                return Result<int>.Failure(validation.ErrorMessage!);
            }

            var created = await _profileRepository.CreateAsync(profile, token);
            await InvalidateDashboardAsync(userId.Value, token);

            return Result<int>.Success(created.Id);
        }

        public override async Task<Result<IEnumerable<ProfileDto>>> GetAllAsync(CancellationToken token = default)
        {
            var profiles = await _profileRepository.GetAllAsync(token);
            return Result<IEnumerable<ProfileDto>>.Success(profiles.Select(profile => profile.ToDto()));
        }

        public override async Task<Result<ProfileDto>> GetByIdAsync(int id, CancellationToken token = default)
        {
            var profile = await _profileRepository.GetByIdWithUserAsync(id, token);
            if (profile is null)
            {
                return Result<ProfileDto>.Failure("Profile not found");
            }

            if (_userContext.UserId is { } userId && profile.UserId != userId)
            {
                return Result<ProfileDto>.Failure("Unauthorized");
            }

            return Result<ProfileDto>.Success(profile.ToDto(includeUser: true));
        }

        public override Task<Result<PagedResult<ProfileDto>>> GetPaginatedResultAsync(GetPagedQuery<ProfileDto> query, CancellationToken ct)
        {
            throw new NotSupportedException();
        }

        public override async Task<Result> UpdateAsync(int id, CreateEditProfileDto dto, CancellationToken token = default)
        {
            var profile = await _profileRepository.GetByIdWithUserAsync(id, token);
            if (profile is null)
            {
                return Result.Failure("Profile not found");
            }

            if (_userContext.UserId is { } userId && profile.UserId != userId)
            {
                return Result.Failure("Unauthorized");
            }

            var validation = ValidateDietAssumptions(dto, requireComplete: false);
            if (!validation.IsSuccess)
            {
                return validation;
            }

            ApplyProfileUpdates(profile, dto);

            await _profileRepository.UpdateAsync(profile, token);
            await InvalidateDashboardAsync(profile.UserId, token);

            return Result.Success();
        }

        public async Task<Result<MyProfileDto>> GetMyProfileAsync(CancellationToken ct = default)
        {
            var userId = _userContext.UserId;
            if (userId is null)
            {
                return Result<MyProfileDto>.Failure("Unauthorized");
            }

            var profile = await _profileRepository.GetByUserIdWithUserAndAchievementsAsync(userId.Value, ct);
            if (profile is null || profile.User is null)
            {
                return Result<MyProfileDto>.Failure("Profile not found");
            }

            var monthStart = DateTime.UtcNow.AddMonths(-1);
            var workoutsThisMonth = await _workoutSessionRepository.GetQueryable()
                .Where(session => session.UserId == userId.Value && session.StartTime >= monthStart)
                .CountAsync(ct);

            var workoutDurations = await _workoutSessionRepository.GetQueryable()
                .Where(session => session.UserId == userId.Value && session.EndTime != null)
                .Select(session => (session.EndTime!.Value - session.StartTime).TotalMinutes)
                .ToListAsync(ct);

            var averageWorkoutTime = workoutDurations.Count > 0
                ? (decimal)Math.Round(workoutDurations.Average(), 0)
                : 0m;

            var dto = new MyProfileDto(
                FirstName: profile.User.FirstName,
                LastName: profile.User.LastName,
                Email: profile.User.Email,
                ProfileImageUrl: profile.Avatar,
                WorkoutsThisMonth: workoutsThisMonth,
                AverageWorkoutTime: averageWorkoutTime,
                AchievementsCount: profile.ProfileAchievements.Count,
                Weight: profile.Weight,
                Height: profile.Height,
                CurrentGoal: (int)profile.CurrentGoal,
                PreferredWeightUnit: (int)profile.PreferredWeightUnit,
                DietType: profile.DietType,
                DailyCaloriesGoal: profile.DailyCaloriesGoal,
                ProteinPercentage: profile.ProteinPercentage,
                CarbsPercentage: profile.CarbsPercentage,
                FatPercentage: profile.FatPercentage);

            return Result<MyProfileDto>.Success(dto);
        }

        public async Task<Result> UpdateMyProfileAsync(CreateEditProfileDto dto, CancellationToken ct = default)
        {
            var userId = _userContext.UserId;
            if (userId is null)
            {
                return Result.Failure("Unauthorized");
            }

            var profile = await _profileRepository.GetByUserIdAsync(userId.Value, ct);
            if (profile is null)
            {
                return Result.Failure("Profile not found");
            }

            return await UpdateAsync(profile.Id, dto, ct);
        }

        public async Task<Result> UpdateDietAssumptionsAsync(CreateEditProfileDto dto, CancellationToken ct = default)
        {
            var userId = _userContext.UserId;
            if (userId is null)
            {
                return Result.Failure("Unauthorized");
            }

            var validation = ValidateDietAssumptions(dto, requireComplete: true);
            if (!validation.IsSuccess)
            {
                return validation;
            }

            var profile = await _profileRepository.GetByUserIdAsync(userId.Value, ct);
            if (profile is null)
            {
                return Result.Failure("Profile not found");
            }

            return await UpdateAsync(profile.Id, dto, ct);
        }

        public async Task<Result<string>> UpdateAvatarAsync(IFormFile file, CancellationToken ct = default)
        {
            var userId = _userContext.UserId;
            if (userId is null)
            {
                return Result<string>.Failure("Unauthorized");
            }

            if (file is null || file.Length == 0)
            {
                return Result<string>.Failure("No file was uploaded.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = ZlpPathHelper.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return Result<string>.Failure("Invalid file extension. Allowed: jpg, jpeg, png, webp.");
            }

            var profile = await _profileRepository.GetByUserIdAsync(userId.Value, ct);
            if (profile is null)
            {
                return Result<string>.Failure("Profile not found");
            }

            var webRootPath = _environment.WebRootPath ?? ZlpPathHelper.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = ZlpPathHelper.Combine(webRootPath, "uploads", "avatars");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            if (!string.IsNullOrEmpty(profile.Avatar) && !profile.Avatar.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                var oldFilePath = ZlpPathHelper.Combine(webRootPath, profile.Avatar.TrimStart('/'));
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = ZlpPathHelper.Combine(uploadsFolder, uniqueFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream, ct);
            }

            var relativeUrl = $"/uploads/avatars/{uniqueFileName}";
            var updateResult = await UpdateAsync(profile.Id, new CreateEditProfileDto(Avatar: relativeUrl), ct);
            if (!updateResult.IsSuccess)
            {
                return Result<string>.Failure(updateResult.ErrorMessage!);
            }

            return Result<string>.Success(relativeUrl);
        }

        private static void ApplyProfileUpdates(Profile profile, CreateEditProfileDto dto)
        {
            if (profile.User is not null)
            {
                if (!string.IsNullOrWhiteSpace(dto.FirstName)) profile.User.FirstName = dto.FirstName.Trim();
                if (!string.IsNullOrWhiteSpace(dto.LastName)) profile.User.LastName = dto.LastName.Trim();
                if (!string.IsNullOrWhiteSpace(dto.Email)) profile.User.Email = dto.Email.Trim();
            }

            if (dto.Avatar is not null) profile.Avatar = NormalizeOptionalText(dto.Avatar);
            if (dto.Weight.HasValue) profile.Weight = dto.Weight.Value;
            if (dto.Height.HasValue) profile.Height = dto.Height.Value;
            if (dto.PreferredWeightUnit.HasValue) profile.PreferredWeightUnit = dto.PreferredWeightUnit.Value;
            if (dto.CurrentGoal.HasValue) profile.CurrentGoal = dto.CurrentGoal.Value;
            if (dto.DietType is not null) profile.DietType = NormalizeOptionalText(dto.DietType);
            if (dto.DailyCaloriesGoal.HasValue) profile.DailyCaloriesGoal = dto.DailyCaloriesGoal;
            if (dto.ProteinPercentage.HasValue) profile.ProteinPercentage = dto.ProteinPercentage;
            if (dto.CarbsPercentage.HasValue) profile.CarbsPercentage = dto.CarbsPercentage;
            if (dto.FatPercentage.HasValue) profile.FatPercentage = dto.FatPercentage;
        }

        private static Result ValidateDietAssumptions(CreateEditProfileDto dto, bool requireComplete)
        {
            var hasAnyMacroField = dto.DailyCaloriesGoal.HasValue
                || dto.ProteinPercentage.HasValue
                || dto.CarbsPercentage.HasValue
                || dto.FatPercentage.HasValue;

            if (!hasAnyMacroField && !requireComplete)
            {
                return Result.Success();
            }

            if (requireComplete && (!dto.DailyCaloriesGoal.HasValue
                || !dto.ProteinPercentage.HasValue
                || !dto.CarbsPercentage.HasValue
                || !dto.FatPercentage.HasValue))
            {
                return Result.Failure("Daily calories goal and macro percentages are required.");
            }

            if (dto.DailyCaloriesGoal is < 1000 or > 5000)
            {
                return Result.Failure("Daily calories goal must be between 1000 and 5000.");
            }

            if (dto.ProteinPercentage is < 0 or > 100
                || dto.CarbsPercentage is < 0 or > 100
                || dto.FatPercentage is < 0 or > 100)
            {
                return Result.Failure("Macro percentages must be between 0 and 100.");
            }

            if (dto.ProteinPercentage.HasValue && dto.CarbsPercentage.HasValue && dto.FatPercentage.HasValue)
            {
                var macroSum = dto.ProteinPercentage.Value + dto.CarbsPercentage.Value + dto.FatPercentage.Value;
                if (macroSum != 100)
                {
                    return Result.Failure("Macro percentages must sum to 100.");
                }
            }

            return Result.Success();
        }

        private static string? NormalizeOptionalText(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private Task InvalidateDashboardAsync(int userId, CancellationToken ct)
        {
            return _cacheService.InvalidateAreaAsync($"dashboard:user:{userId}", ct);
        }
    }
}
