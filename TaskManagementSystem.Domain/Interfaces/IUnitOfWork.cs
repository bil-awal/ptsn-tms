using System;
using System.Threading.Tasks;

namespace TaskManagementSystem.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository Tasks { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
