using System.Collections.Generic;

namespace TaskManagementSystem.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    }
}
