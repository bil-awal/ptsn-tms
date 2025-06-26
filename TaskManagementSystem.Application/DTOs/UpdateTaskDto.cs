using System;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.DTOs
{
    public class UpdateTaskDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority? Priority { get; set; }
        public TaskManagementSystem.Domain.Enums.TaskStatus? Status { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }
}
