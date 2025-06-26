using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Infrastructure.Data;

namespace TaskManagementSystem.Infrastructure.Repositories
{
    public class TaskRepository : Repository<TaskItem>, ITaskRepository
    {
        public TaskRepository(TaskManagementContext context) : base(context)
        {
        }

        public override async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.AssignedToUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Include(t => t.AssignedToUser)
                .Where(t => t.AssignedToUserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetOverdueTasksAsync()
        {
            return await _dbSet
                .Include(t => t.AssignedToUser)
                .Where(t => t.DueDate < DateTime.UtcNow && t.Status != TaskManagementSystem.Domain.Enums.TaskStatus.Completed)
                .ToListAsync();
        }
    }
}
