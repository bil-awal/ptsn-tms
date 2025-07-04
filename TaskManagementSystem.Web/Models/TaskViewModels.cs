namespace TaskManagementSystem.Web.Models
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateTaskViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7);
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public Guid? AssignedToUserId { get; set; }
    }

    public class UpdateTaskViewModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }

    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Urgent = 4
    }

    public enum TaskStatus
    {
        Todo = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4
    }
}
