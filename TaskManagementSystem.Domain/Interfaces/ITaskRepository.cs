using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Domain.Interfaces
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(Guid userId);
        Task<IEnumerable<TaskItem>> GetOverdueTasksAsync();
    }
}
