using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Domain.Interfaces;

namespace TaskManagementSystem.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TaskService> _logger;

        public TaskService(IUnitOfWork unitOfWork, ILogger<TaskService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            _logger.LogInformation("Creating new task: {Title}", createTaskDto.Title);

            var taskItem = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate,
                Priority = createTaskDto.Priority,
                Status = TaskManagementSystem.Domain.Enums.TaskStatus.Todo,
                AssignedToUserId = createTaskDto.AssignedToUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Tasks.AddAsync(taskItem);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Task created successfully with ID: {TaskId}", taskItem.Id);

            return await MapToDto(taskItem);
        }

        public async Task<TaskDto?> GetTaskByIdAsync(Guid id)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            return task != null ? await MapToDto(task) : null;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _unitOfWork.Tasks.GetAllAsync();
            var taskDtos = new List<TaskDto>();
            
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }
            
            return taskDtos;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(Guid userId)
        {
            var tasks = await _unitOfWork.Tasks.GetTasksByUserIdAsync(userId);
            var taskDtos = new List<TaskDto>();
            
            foreach (var task in tasks)
            {
                taskDtos.Add(await MapToDto(task));
            }
            
            return taskDtos;
        }

        public async Task<TaskDto> UpdateTaskAsync(Guid id, UpdateTaskDto updateTaskDto)
        {
            _logger.LogInformation("Updating task with ID: {TaskId}", id);

            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null)
            {
                throw new InvalidOperationException($"Task with ID {id} not found");
            }

            if (!string.IsNullOrEmpty(updateTaskDto.Title))
                task.Title = updateTaskDto.Title;

            if (updateTaskDto.Description != null)
                task.Description = updateTaskDto.Description;

            if (updateTaskDto.DueDate.HasValue)
                task.DueDate = updateTaskDto.DueDate.Value;

            if (updateTaskDto.Priority.HasValue)
                task.Priority = updateTaskDto.Priority.Value;

            if (updateTaskDto.Status.HasValue)
                task.Status = updateTaskDto.Status.Value;

            if (updateTaskDto.AssignedToUserId.HasValue)
                task.AssignedToUserId = updateTaskDto.AssignedToUserId.Value;

            task.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Tasks.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Task updated successfully with ID: {TaskId}", id);

            return await MapToDto(task);
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            _logger.LogInformation("Deleting task with ID: {TaskId}", id);

            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null)
            {
                throw new InvalidOperationException($"Task with ID {id} not found");
            }

            await _unitOfWork.Tasks.DeleteAsync(task);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Task deleted successfully with ID: {TaskId}", id);
        }

        private async Task<TaskDto> MapToDto(TaskItem task)
        {
            var dto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                AssignedToUserId = task.AssignedToUserId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };

            if (task.AssignedToUserId.HasValue)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(task.AssignedToUserId.Value);
                dto.AssignedToUserName = user?.Name;
            }

            return dto;
        }
    }
}
