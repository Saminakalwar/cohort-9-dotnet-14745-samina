namespace TaskManagement.Application.DTOs.Tasks;
public class TaskResponse
{
    public Guid Id {get; set;}
    public string Title {get; set;} = string.Empty;
    public string? Description {get; set;} 
    public DateTime? DueDate{get; set;}
    public int Priority {get; set;}
    public int Status {get; set;}
    public Guid CategoryId {get; set;}
    public string? CategoryName {get; set;}
    public string AssignedUserId { get; set;} = string.Empty;
}