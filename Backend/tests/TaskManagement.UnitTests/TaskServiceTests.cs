using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Services;
using TaskManagement.Persistence.Context;
using Xunit;

namespace TaskManagement.UnitTests;

public class TaskServiceTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly ApplicationDbContext _context;
    private readonly TaskService _service;

    private readonly Guid _userId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    private readonly Guid _otherUserId =
        Guid.Parse("22222222-2222-2222-2222-222222222222");

    private readonly Guid _categoryId =
        Guid.Parse("33333333-3333-3333-3333-333333333333");

    public TaskServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        _currentUserServiceMock = new Mock<ICurrentUserService>();

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(_userId.ToString());

        _currentUserServiceMock
            .Setup(x => x.IsInRole(AppRoles.Admin))
            .Returns(false);

        _context.Categories.Add(new Category
        {
            Id = _categoryId,
            Name = "Development"
        });

        _context.SaveChanges();

        _service = new TaskService(
            _context,
            _currentUserServiceMock.Object,
            NullLogger<TaskService>.Instance);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldCreateTaskWithPendingStatus()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Test Task",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            Priority = TaskPriority.High,
            CategoryId = _categoryId
        };

        // Act
        var taskId = await _service.CreateTaskAsync(request);

        // Assert
        var task = await _context.Tasks.FindAsync(taskId);

        Assert.NotNull(task);
        Assert.Equal("Test Task", task.Title);
        Assert.Equal(TaskManagement.Domain.Enums.TaskPriority.High, task.Priority);
        Assert.Equal(TaskManagement.Domain.Enums.TaskStatus.Pending, task.Status);
        Assert.Equal(_userId.ToString(), task.AssignedUserId);
        Assert.Equal(_userId.ToString(), task.CreatedBy);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldThrow_WhenPriorityIsInvalid()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Invalid Priority Task",
            Description = "Test",
            Priority = (TaskPriority)99,
            CategoryId = _categoryId
        };

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateTaskAsync(request));

        // Assert
        Assert.Equal("Invalid task priority.", exception.Message);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldThrow_WhenCategoryDoesNotExist()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "Invalid Category Task",
            Description = "Test",
            Priority = TaskPriority.Low,
            CategoryId = Guid.NewGuid()
        };

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.CreateTaskAsync(request));

        // Assert
        Assert.Equal("Category not found.", exception.Message);
    }

    [Fact]
    public async Task GetTaskAsync_ShouldReturnOnlyCurrentUsersTasks()
    {
        // Arrange
        var ownTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "My Task",
            Priority = TaskPriority.High,
            Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
            CategoryId = _categoryId,
            AssignedUserId = _userId.ToString()
        };

        var otherTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Other User Task",
            Priority = TaskPriority.Low,
            Status = TaskManagement.Domain.Enums.TaskStatus.Completed,
            CategoryId = _categoryId,
            AssignedUserId = _otherUserId.ToString()
        };

        _context.Tasks.AddRange(ownTask, otherTask);
        await _context.SaveChangesAsync();

        // Act
        var result = (await _service.GetTaskAsync()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("My Task", result[0].Title);
    }

    [Fact]
    public async Task GetTaskAsync_ShouldReturnAllTasks_ForAdmin()
    {
        // Arrange
        _currentUserServiceMock
            .Setup(x => x.IsInRole(AppRoles.Admin))
            .Returns(true);

        _context.Tasks.AddRange(
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "User One Task",
                Priority = TaskPriority.High,
                Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
                CategoryId = _categoryId,
                AssignedUserId = _userId.ToString()
            },
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "User Two Task",
                Priority = TaskPriority.Low,
                Status = TaskManagement.Domain.Enums.TaskStatus.Completed,
                CategoryId = _categoryId,
                AssignedUserId = _otherUserId.ToString()
            });

        await _context.SaveChangesAsync();

        // Act
        var result = (await _service.GetTaskAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnOwnTask()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _context.Tasks.Add(new TaskItem
        {
            Id = taskId,
            Title = "My Task",
            Priority = TaskPriority.Medium,
            Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
            CategoryId = _categoryId,
            AssignedUserId = _userId.ToString()
        });

        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetTaskByIdAsync(taskId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(taskId, result.Id);
        Assert.Equal("My Task", result.Title);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskBelongsToAnotherUser()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _context.Tasks.Add(new TaskItem
        {
            Id = taskId,
            Title = "Other User Task",
            Priority = TaskPriority.Medium,
            Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
            CategoryId = _categoryId,
            AssignedUserId = _otherUserId.ToString()
        });

        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetTaskByIdAsync(taskId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateOwnTask()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _context.Tasks.Add(new TaskItem
        {
            Id = taskId,
            Title = "Old Title",
            Description = "Old Description",
            Priority = TaskPriority.Low,
            Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
            CategoryId = _categoryId,
            AssignedUserId = _userId.ToString()
        });

        await _context.SaveChangesAsync();

        var request = new UpdateTaskRequest
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Priority = 4,
            Status = 2,
            CategoryId = _categoryId
        };

        // Act
        await _service.UpdateTaskAsync(taskId, request);

        // Assert
        var task = await _context.Tasks.FindAsync(taskId);

        Assert.NotNull(task);
        Assert.Equal("Updated Title", task.Title);
        Assert.Equal("Updated Description", task.Description);
        Assert.Equal(TaskPriority.Critical, task.Priority);
        Assert.Equal(TaskManagement.Domain.Enums.TaskStatus.InProgress, task.Status);
        Assert.Equal(_userId.ToString(), task.UpdatedBy);
        Assert.NotNull(task.UpdatedAt);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldThrow_WhenUserDoesNotOwnTask()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _context.Tasks.Add(new TaskItem
        {
            Id = taskId,
            Title = "Other User Task",
            Priority = TaskPriority.Low,
            Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
            CategoryId = _categoryId,
            AssignedUserId = _otherUserId.ToString()
        });

        await _context.SaveChangesAsync();

        var request = new UpdateTaskRequest
        {
            Title = "Trying To Update",
            Description = "Unauthorized",
            Priority = 1,
            Status = 1,
            CategoryId = _categoryId
        };

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.UpdateTaskAsync(taskId, request));

        // Assert
        Assert.Equal(
            "You are not authorized to update this task.",
            exception.Message);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldThrow_WhenStatusIsInvalid()
    {
        // Arrange
        var request = new UpdateTaskRequest
        {
            Title = "Invalid Status",
            Description = "Test",
            Priority = 1,
            Status = 99,
            CategoryId = _categoryId
        };

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateTaskAsync(Guid.NewGuid(), request));

        // Assert
        Assert.Equal("Invalid task status.", exception.Message);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldDeleteOwnTask()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _context.Tasks.Add(new TaskItem
        {
            Id = taskId,
            Title = "Task To Delete",
            Priority = TaskPriority.Low,
            Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
            CategoryId = _categoryId,
            AssignedUserId = _userId.ToString()
        });

        await _context.SaveChangesAsync();

        // Act
        await _service.DeleteTaskAsync(taskId);

        // Assert
        var task = await _context.Tasks.FindAsync(taskId);

        Assert.Null(task);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldThrow_WhenUserDoesNotOwnTask()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _context.Tasks.Add(new TaskItem
        {
            Id = taskId,
            Title = "Other User Task",
            Priority = TaskPriority.Low,
            Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
            CategoryId = _categoryId,
            AssignedUserId = _otherUserId.ToString()
        });

        await _context.SaveChangesAsync();

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.DeleteTaskAsync(taskId));

        // Assert
        Assert.Equal(
            "You are not authorized to delete this task.",
            exception.Message);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldThrow_WhenTaskDoesNotExist()
    {
        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteTaskAsync(Guid.NewGuid()));

        // Assert
        Assert.Equal("Task not found.", exception.Message);
    }
}