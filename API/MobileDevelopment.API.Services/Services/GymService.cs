using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Models.DTO.Gyms;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Domain.Interfaces.Auth;

namespace MobileDevelopment.API.Services.Services
{
    public sealed class GymService : IGymService
    {
        private readonly IGymRepository _gymRepository;
        private readonly IUserContext _userContext;
        private readonly ICacheService _cacheService;

        public GymService(IGymRepository gymRepository, IUserContext userContext, ICacheService cacheService)
        {
            _gymRepository = gymRepository;
            _userContext = userContext;
            _cacheService = cacheService;
        }

        public async Task<Result<IEnumerable<GymDto>>> GetAllGymsAsync(string? search = null)
        {
            var userId = _userContext.UserId;
            var isAdmin = string.Equals(_userContext.UserRole, Role.Administrator.ToString(), StringComparison.OrdinalIgnoreCase);
            var roleKey = isAdmin ? "admin" : $"user:{userId}";
            var searchKey = Uri.EscapeDataString((search ?? string.Empty).Trim().ToLowerInvariant());
            var cacheKey = $"all:{roleKey}:q:{searchKey}";

            var dtos = await _cacheService.GetOrSetVersionedAsync(
                "gyms",
                cacheKey,
                async ct =>
                {
                    var query = _gymRepository.GetQueryable();

                    if (!isAdmin)
                    {
                        query = query.Where(g => g.IsActive || g.CreatedByUserId == userId);
                    }

                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        var lowerSearch = search.ToLower();
                        query = query.Where(g => g.Name.ToLower().Contains(lowerSearch) ||
                                                 g.City.ToLower().Contains(lowerSearch) ||
                                                 g.Street.ToLower().Contains(lowerSearch));
                    }

                    var gyms = await query.OrderBy(g => g.Name).ToListAsync(ct);
                    return gyms.Select(g => g.ToDto()).ToList();
                },
                TimeSpan.FromMinutes(3));

            return Result<IEnumerable<GymDto>>.Success(dtos);
        }

        public async Task<Result<PagedResult<GymDto>>> GetPagedGymsAsync(int pageNumber, int pageSize)
        {
            var userId = _userContext.UserId;
            var isAdmin = string.Equals(_userContext.UserRole, Role.Administrator.ToString(), StringComparison.OrdinalIgnoreCase);
            var roleKey = isAdmin ? "admin" : $"user:{userId}";
            var cacheKey = $"paged:{roleKey}:p:{pageNumber}:s:{pageSize}";

            var pagedDtos = await _cacheService.GetOrSetVersionedAsync(
                "gyms",
                cacheKey,
                async ct =>
                {
                    var query = _gymRepository.GetQueryable();

                    if (!isAdmin)
                    {
                        query = query.Where(g => g.IsActive || g.CreatedByUserId == userId);
                    }

                    var totalCount = await query.CountAsync(ct);
                    var gyms = await query
                        .OrderBy(g => g.Name)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync(ct);

                    return new PagedResult<GymDto>(
                        gyms.Select(g => g.ToDto()).ToList(),
                        totalCount,
                        pageNumber,
                        pageSize
                    );
                },
                TimeSpan.FromMinutes(3));

            return Result<PagedResult<GymDto>>.Success(pagedDtos);
        }

        public async Task<Result<GymDto>> GetGymByIdAsync(int id)
        {
            var gym = await _gymRepository.GetByIdAsync(id);
            if (gym is null)
            {
                return Result<GymDto>.Failure($"Gym with ID {id} not found.");
            }

            return Result<GymDto>.Success(gym.ToDto());
        }

        public async Task<Result<GymDto>> CreateGymAsync(CreateEditGymDto dto)
        {
            var userId = _userContext.UserId;
            var isAdmin = string.Equals(_userContext.UserRole, Role.Administrator.ToString(), StringComparison.OrdinalIgnoreCase);

            var gym = new Gym
            {
                Name = dto.Name,
                Street = dto.Street,
                City = dto.City,
                ZipCode = dto.ZipCode,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Rating = dto.Rating,
                Description = dto.Description,
                IsActive = isAdmin,
                CreatedByUserId = userId
            };

            await _gymRepository.CreateAsync(gym);
            await _gymRepository.SaveChangesAsync();
            await _cacheService.InvalidateAreaAsync("gyms");

            return Result<GymDto>.Success(gym.ToDto());
        }

        public async Task<Result<GymDto>> EditGymAsync(int id, CreateEditGymDto dto)
        {
            var gym = await _gymRepository.GetByIdAsync(id);
            if (gym is null)
            {
                return Result<GymDto>.Failure($"Gym with ID {id} not found.");
            }

            gym.Name = dto.Name;
            gym.Street = dto.Street;
            gym.City = dto.City;
            gym.ZipCode = dto.ZipCode;
            gym.Latitude = dto.Latitude;
            gym.Longitude = dto.Longitude;
            gym.Rating = dto.Rating;
            gym.Description = dto.Description;

            await _gymRepository.UpdateAsync(gym);
            await _gymRepository.SaveChangesAsync();
            await _cacheService.InvalidateAreaAsync("gyms");

            return Result<GymDto>.Success(gym.ToDto());
        }

        public async Task<Result> RemoveGymAsync(int id)
        {
            var exists = await _gymRepository.ExistsAsync(id);
            if (!exists)
            {
                return Result.Failure($"Gym with ID {id} not found.");
            }

            await _gymRepository.DeleteAsync(id);
            await _gymRepository.SaveChangesAsync();
            await _cacheService.InvalidateAreaAsync("gyms");

            return Result.Success();
        }

        public async Task<Result> RemoveRangeGymsAsync(IEnumerable<int> ids)
        {
            await _gymRepository.DeleteRangeAsync(ids);
            await _gymRepository.SaveChangesAsync();
            await _cacheService.InvalidateAreaAsync("gyms");
            return Result.Success();
        }
    }
}
