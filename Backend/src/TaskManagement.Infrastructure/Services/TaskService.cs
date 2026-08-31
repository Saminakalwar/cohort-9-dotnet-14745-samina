

using TaskManagement.Application.Interfaces;
using TaskManagement.Persistence.Context;

namespace TaskManagement.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
}