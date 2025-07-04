using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskViewModel>> GetAllTasksAsync();
        Task<TaskViewModel?> GetTaskByIdAsync(Guid id);
        Task<IEnumerable<TaskViewModel>> GetTasksByUserIdAsync(Guid userId);
        Task<TaskViewModel> CreateTaskAsync(CreateTaskViewModel task);
        Task<TaskViewModel> UpdateTaskAsync(Guid id, UpdateTaskViewModel task);
        Task DeleteTaskAsync(Guid id);
    }
}
