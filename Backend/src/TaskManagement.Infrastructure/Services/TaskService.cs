using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Persistence.Context;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public TaskService(ApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> CreateTaskAsync(CreateTaskRequest request)
    {
        var categoryExist = await _context.Categories.AnyAsync(category => category.Id == request.CategoryId);
        
        if (!categoryExist)
        {
            throw new Exception("Category not found");
        }

        if(string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Priority = request.Priority,
            Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow,
            AssignedUserId = _currentUserService.UserId,
            CreatedBy = _currentUserService.UserId,
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return task.Id;
    }
}