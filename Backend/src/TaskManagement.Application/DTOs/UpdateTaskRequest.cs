using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.DTOs.Tasks;

public class UpdateTaskRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    [Range(1,4)]
    public int Priority { get; set; }
    [Range(1,3)]
    public int Status { get; set; }
    [Required]
    public Guid CategoryId { get; set; }
}