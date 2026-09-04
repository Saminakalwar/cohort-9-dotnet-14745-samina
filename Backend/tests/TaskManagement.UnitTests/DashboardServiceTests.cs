using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Services;
using TaskManagement.Persistence.Context;
using Xunit;

namespace TaskManagement.UnitTests;

public class DashboardServiceTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly ApplicationDbContext _context;
    private readonly DashboardService _service;

    private readonly Guid _userId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    private readonly Guid _otherUserId =
        Guid.Parse("22222222-2222-2222-2222-222222222222");

    private readonly Guid _categoryId =
        Guid.Parse("33333333-3333-3333-3333-333333333333");

    public DashboardServiceTests()
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

        _service = new DashboardService(
            _context,
            _currentUserServiceMock.Object,
            NullLogger<DashboardService>.Instance);
    }

    [Fact]
    public async Task GetDashboardAsync_ShouldReturnCurrentUserTaskCounts()
    {
        // Arrange
        _context.Tasks.AddRange(
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Pending Task",
                Priority = TaskPriority.High,
                Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
                CategoryId = _categoryId,
                AssignedUserId = _userId.ToString()
            },
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "In Progress Task",
                Priority = TaskPriority.Medium,
                Status = TaskManagement.Domain.Enums.TaskStatus.InProgress,
                CategoryId = _categoryId,
                AssignedUserId = _userId.ToString()
            },
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Completed Task",
                Priority = TaskPriority.Low,
                Status = TaskManagement.Domain.Enums.TaskStatus.Completed,
                CategoryId = _categoryId,
                AssignedUserId = _userId.ToString()
            },
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Other User Task",
                Priority = TaskPriority.High,
                Status = TaskManagement.Domain.Enums.TaskStatus.Completed,
                CategoryId = _categoryId,
                AssignedUserId = _otherUserId.ToString()
            });

        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetDashboardAsync();

        // Assert
        Assert.Equal(1, result.Pending);
        Assert.Equal(1, result.InProgress);
        Assert.Equal(1, result.Completed);
    }

    [Fact]
    public async Task GetDashboardAsync_ShouldReturnAllTaskCounts_ForAdmin()
    {
        // Arrange
        _currentUserServiceMock
            .Setup(x => x.IsInRole(AppRoles.Admin))
            .Returns(true);

        _context.Tasks.AddRange(
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "User Pending",
                Priority = TaskPriority.High,
                Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
                CategoryId = _categoryId,
                AssignedUserId = _userId.ToString()
            },
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Other Pending",
                Priority = TaskPriority.Medium,
                Status = TaskManagement.Domain.Enums.TaskStatus.Pending,
                CategoryId = _categoryId,
                AssignedUserId = _otherUserId.ToString()
            },
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "User Completed",
                Priority = TaskPriority.Low,
                Status = TaskManagement.Domain.Enums.TaskStatus.Completed,
                CategoryId = _categoryId,
                AssignedUserId = _userId.ToString()
            });

        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetDashboardAsync();

        // Assert
        Assert.Equal(2, result.Pending);
        Assert.Equal(0, result.InProgress);
        Assert.Equal(1, result.Completed);
    }

    [Fact]
    public async Task GetDashboardAsync_ShouldReturnZero_WhenUserHasNoTasks()
    {
        // Act
        var result = await _service.GetDashboardAsync();

        // Assert
        Assert.Equal(0, result.Pending);
        Assert.Equal(0, result.InProgress);
        Assert.Equal(0, result.Completed);
    }
}