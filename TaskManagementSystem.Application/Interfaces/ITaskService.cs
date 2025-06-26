using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<TaskDto?> GetTaskByIdAsync(Guid id);
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(Guid userId);
        Task<TaskDto> UpdateTaskAsync(Guid id, UpdateTaskDto updateTaskDto);
        Task DeleteTaskAsync(Guid id);
    }
}
