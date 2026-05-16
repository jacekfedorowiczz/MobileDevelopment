namespace MobileDevelopment.API.Services.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan ttl,
            CancellationToken cancellationToken = default);

        Task<T> GetOrSetVersionedAsync<T>(
            string area,
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan ttl,
            CancellationToken cancellationToken = default);

        Task InvalidateAreaAsync(string area, CancellationToken cancellationToken = default);
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}
