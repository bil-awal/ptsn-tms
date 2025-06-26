using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskManagementSystem.Domain.Enums.TaskStatus Status { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }
    }
}
