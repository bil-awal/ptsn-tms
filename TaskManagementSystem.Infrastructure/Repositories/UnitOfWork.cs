using System;
using System.Threading.Tasks;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Infrastructure.Data;

namespace TaskManagementSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskManagementContext _context;
        private ITaskRepository? _tasks;
        private IUserRepository? _users;

        public UnitOfWork(TaskManagementContext context)
        {
            _context = context;
        }

        public ITaskRepository Tasks => _tasks ??= new TaskRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
