using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Persistence.Context;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Common;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ApplicationDbContext context, ICurrentUserService currentUserService, ILogger<TaskService> logger)
    {
        _context = context;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<Guid> CreateTaskAsync(CreateTaskRequest request)
    {
        if (!Enum.IsDefined(typeof(TaskPriority), request.Priority))
        {
            throw new ArgumentException("Invalid task priority.");
        }

        var categoryExist = await _context.Categories.AnyAsync(category => category.Id == request.CategoryId);

        if (!categoryExist)
        {
            throw new KeyNotFoundException("Category not found.");
        }

        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Priority = (TaskPriority)request.Priority,
            Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow,
            AssignedUserId = _currentUserService.UserId,
            CreatedBy = _currentUserService.UserId,
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Task {TaskId} created by user {UserId}", task.Id,_currentUserService.UserId);

        return task.Id;
    }

    public async Task<IEnumerable<TaskResponse>> GetTaskAsync()
    {
        var query = _context.Tasks.Include(t => t.Category).AsQueryable(); //unexecutable query

        if (!_currentUserService.IsInRole(AppRoles.Admin))
        {
            query = query.Where(t =>
            t.AssignedUserId == _currentUserService.UserId);
        }
        var tasks = await query.Select(t => new TaskResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            Priority = (int)t.Priority,
            Status = (int)t.Status,
            CategoryId = t.CategoryId,
            CategoryName = t.Category.Name,
            AssignedUserId = t.AssignedUserId,

            AssignedUserName = _context.Users
                .Where(u => u.Id == t.AssignedUserId)
                .Select(u => $"{u.FirstName} {u.LastName}")
                .FirstOrDefault(),

            AssignedUserEmail = _context.Users
                .Where(u => u.Id == t.AssignedUserId)
                .Select(u => u.Email)
                .FirstOrDefault()

        }).ToListAsync();

        _logger.LogInformation("User {UserId} retrieved {TaskCount} tasks", _currentUserService.UserId, tasks.Count);

        return tasks;
    }

    public async Task<TaskResponse?> GetTaskByIdAsync(Guid id)
    {
        var query = _context.Tasks
            .Include(t => t.Category)
            .AsQueryable();

        if (!_currentUserService.IsInRole(AppRoles.Admin))
        {
            query = query.Where(t => t.AssignedUserId == _currentUserService.UserId);
        }

        var task = await query
            .Where(t => t.Id == id)
            .Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = (int)t.Priority,
                Status = (int)t.Status,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                AssignedUserId = t.AssignedUserId,
                AssignedUserName = _context.Users
                .Where(u => u.Id == t.AssignedUserId)
                .Select(u => $"{u.FirstName} {u.LastName}")
                .FirstOrDefault(),

            AssignedUserEmail = _context.Users
                .Where(u => u.Id == t.AssignedUserId)
                .Select(u => u.Email)
                .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        _logger.LogInformation( "User {UserId} retrieved task {TaskId}", _currentUserService.UserId, id);
        return task;

    }

    public async Task UpdateTaskAsync(Guid id, UpdateTaskRequest request)
    {
            if (!Enum.IsDefined(typeof(TaskPriority), request.Priority))
        {
            throw new ArgumentException("Invalid task priority.");
        }

        if (!Enum.IsDefined(typeof(TaskManagement.Domain.Enums.TaskStatus), request.Status))
        {
            throw new ArgumentException("Invalid task status.");
        }

        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            throw new KeyNotFoundException("Task not found.");
        }

        if (!_currentUserService.IsInRole(AppRoles.Admin) &&
            task.AssignedUserId != _currentUserService.UserId)
        {
            throw new UnauthorizedAccessException(
                "You are not authorized to update this task.");
        }

        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == request.CategoryId);

        if (!categoryExists)
        {
            throw new KeyNotFoundException("Category not found.");
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.DueDate = request.DueDate;
        task.Priority = (TaskPriority)request.Priority;
        task.Status = (TaskManagement.Domain.Enums.TaskStatus)request.Status;
        task.CategoryId = request.CategoryId;
        task.UpdatedAt = DateTime.UtcNow;
        task.UpdatedBy = _currentUserService.UserId;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Task {TaskId} updated by user {UserId}", task.Id, _currentUserService.UserId);
    }

    public async Task DeleteTaskAsync(Guid id)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            throw new KeyNotFoundException("Task not found.");
        }

        if (!_currentUserService.IsInRole(AppRoles.Admin) &&
            task.AssignedUserId != _currentUserService.UserId)
        {
            throw new UnauthorizedAccessException(
                "You are not authorized to delete this task.");
        }

        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync();
        _logger.LogInformation("Task {TaskId} deleted by user {UserId}", task.Id, _currentUserService.UserId);
    }

}