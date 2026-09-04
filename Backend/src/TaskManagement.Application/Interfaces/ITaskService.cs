using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Interfaces;

public interface ITaskService
{
    Task<Guid> CreateTaskAsync(CreateTaskRequest request);
    Task<IEnumerable<TaskResponse>> GetTaskAsync();
    Task<TaskResponse?> GetTaskByIdAsync(Guid id);
    Task UpdateTaskAsync(Guid id, UpdateTaskRequest request);
    Task DeleteTaskAsync(Guid id);
}