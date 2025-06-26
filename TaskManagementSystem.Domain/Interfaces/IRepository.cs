using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Guid id);
    }
}
