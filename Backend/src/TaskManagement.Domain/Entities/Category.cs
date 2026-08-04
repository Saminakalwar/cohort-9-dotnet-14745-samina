using TaskManagement.Domain.Common;

public class Category : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<TaskItem> Tasks { get; set; } = [];

}