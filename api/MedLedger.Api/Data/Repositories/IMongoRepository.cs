using System.Linq.Expressions;
using MedLedger.Api.Data.Entities;

namespace MedLedger.Api.Data.Repositories;

public interface IMongoRepository<T>
        where T : EntityBase
{
    Task<T?> GetByIdAsync(string id);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    Task<List<T>> GetAllAsync();
    Task<List<T>> FilterAsync(Expression<Func<T, bool>> filter);
    Task AddAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task DeleteAsync(string id);
}