using Microsoft.EntityFrameworkCore;
using MobileDevelopment.API.Domain.Base;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Persistence.Interfaces.Base;
using System.Linq.Expressions;
using System.Reflection;

namespace MobileDevelopment.API.Persistence.Repositories.Base
{
    public class Repository<T> : IBaseEntityRepository<T> where T : BaseEntity
    {
        protected readonly SystemContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(SystemContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync([id], cancellationToken);
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity is not null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }

            return false;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Attach(T entity)
        {
            _dbSet.Attach(entity);
        }

        public void AttachRange(IEnumerable<T> entities)
        {
            _dbSet.AttachRange(entities);
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            await _dbSet
                .Where(x => ids.Contains(x.Id))
                .ExecuteDeleteAsync(cancellationToken);

            return true;
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync([id], cancellationToken);
            return entity is not null;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public async Task<PagedResult<T>> GetPagedDynamicAsync(
            string? searchValue,
            string? sortColumn,
            string? sortDirection,
            int pageIndex,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (include is not null)
            {
                query = include(query);
            }

            var searchExpression = BuildDynamicSearchExpression(searchValue);
            if (searchExpression is not null)
            {
                query = query.Where(searchExpression);
            }

            var isAscending = string.IsNullOrWhiteSpace(sortDirection) || sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase);
            query = ApplyDynamicSort(query, sortColumn, isAscending);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<T>(items, totalCount, pageIndex, pageSize);
        }

        #region Private methods
        private static Expression<Func<T, bool>>? BuildDynamicSearchExpression(string? searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return null;
            }

            var parameter = Expression.Parameter(typeof(T), "x");

            var stringProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string) && p.CanRead)
                .ToList();

            if (stringProperties.Count == 0)
            {
                return null;
            }


            Expression? body = null;
            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)]);
            var searchConstant = Expression.Constant(searchValue);

            foreach (var prop in stringProperties)
            {
                var propAccess = Expression.Property(parameter, prop);

                // x.Property is not null
                var nullCheck = Expression.NotEqual(propAccess, Expression.Constant(null, typeof(string)));

                // x.Property.Contains("searchValue")
                var containsExpression = Expression.Call(propAccess, containsMethod!, searchConstant);

                // x.Property is not null AND x.Property.Contains(...)
                var safeContains = Expression.AndAlso(nullCheck, containsExpression);
                body = body == null ? safeContains : Expression.OrElse(body, safeContains);
            }

            if (body == null)
            {
                return null;
            }

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static IQueryable<T> ApplyDynamicSort(IQueryable<T> query, string? sortColumn, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(sortColumn))
            {
                return query;
            }

            var property = typeof(T).GetProperty(sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                return query; // kolumna nie istnieje, nie sortuj
            }


            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var methodName = ascending ? "OrderBy" : "OrderByDescending";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                [typeof(T), property.PropertyType],
                query.Expression,
                Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<T>(resultExpression);
        }
        #endregion
    }
}
