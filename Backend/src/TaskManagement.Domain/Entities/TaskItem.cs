using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;

public class TaskItem : AuditableEntity
{
    public string Title { get; set; } = string.Empty; // bcs it is required so must give default value
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    public TaskManagement.Domain.Enums.TaskStatus Status { get; set; } // to get rid of compiler confusion with System.Threading.Tasks.TaskStatus add whole path
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;  //must add !
    public string AssignedUserId { get; set; } = string.Empty;


}