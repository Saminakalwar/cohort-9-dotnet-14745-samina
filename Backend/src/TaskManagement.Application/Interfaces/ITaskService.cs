using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Interfaces;

public interface ITaskService
{
    Task<Guid> CreateTaskAsync(CreateTaskRequest request);
}