using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Domain.Interfaces;
using Xunit;

namespace TaskManagementSystem.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<TaskService>> _loggerMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<TaskService>>();
            
            _unitOfWorkMock.Setup(u => u.Tasks).Returns(_taskRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
            
            _taskService = new TaskService(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldCreateTask_WhenValidDataProvided()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto
            {
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.UtcNow.AddDays(1),
                Priority = TaskPriority.High
            };

            _taskRepositoryMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
                .ReturnsAsync((TaskItem task) => task);

            // Act
            var result = await _taskService.CreateTaskAsync(createTaskDto);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(createTaskDto.Title);
            result.Description.Should().Be(createTaskDto.Description);
            result.Priority.Should().Be(createTaskDto.Priority);
            result.Status.Should().Be(TaskManagementSystem.Domain.Enums.TaskStatus.Todo);
            
            _taskRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskItem
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.UtcNow.AddDays(1),
                Priority = TaskPriority.Medium,
                Status = TaskManagementSystem.Domain.Enums.TaskStatus.InProgress
            };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync(task);

            // Act
            var result = await _taskService.GetTaskByIdAsync(taskId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(taskId);
            result.Title.Should().Be(task.Title);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync((TaskItem?)null);

            // Act
            var result = await _taskService.GetTaskByIdAsync(taskId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldUpdateTask_WhenTaskExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var existingTask = new TaskItem
            {
                Id = taskId,
                Title = "Old Title",
                Description = "Old Description",
                DueDate = DateTime.UtcNow.AddDays(1),
                Priority = TaskPriority.Low,
                Status = TaskManagementSystem.Domain.Enums.TaskStatus.Todo
            };

            var updateDto = new UpdateTaskDto
            {
                Title = "New Title",
                Status = TaskManagementSystem.Domain.Enums.TaskStatus.Completed
            };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync(existingTask);

            // Act
            var result = await _taskService.UpdateTaskAsync(taskId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(updateDto.Title);
            result.Status.Should().Be(updateDto.Status.Value);
            result.Description.Should().Be(existingTask.Description); // Should remain unchanged
            
            _taskRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldDeleteTask_WhenTaskExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskItem { Id = taskId };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync(task);

            // Act
            await _taskService.DeleteTaskAsync(taskId);

            // Assert
            _taskRepositoryMock.Verify(r => r.DeleteAsync(task), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldThrowException_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync((TaskItem?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _taskService.DeleteTaskAsync(taskId));
        }
    }
}
