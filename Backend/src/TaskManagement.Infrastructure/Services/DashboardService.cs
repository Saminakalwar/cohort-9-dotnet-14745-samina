using TaskManagement.Persistence.Context;
using TaskManagement.Application.Interfaces;
namespace TaskManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.Dashboard;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Common;
using Microsoft.Extensions.Logging;
public class DashboardService : IDashboardService
{
private readonly ApplicationDbContext _context;
private readonly ICurrentUserService _currentUserService;
private readonly ILogger<DashboardService> _logger;
public DashboardService(ApplicationDbContext context, ICurrentUserService currentUserService, ILogger<DashboardService> logger)
    {
        _context = context;
        _currentUserService = currentUserService;
        _logger = logger;
    }

public async Task<DashboardResponse> GetDashboardAsync()
    {
       var query = _context.Tasks.AsQueryable();
      if(!_currentUserService.IsInRole(AppRoles.Admin))
      {
        query = query.Where(task => task.AssignedUserId == _currentUserService.UserId);
      }
      
      var status = new DashboardResponse
      {
          
        Pending = await query.CountAsync(task => task.Status == TaskStatus.Pending),
        InProgress = await query.CountAsync(task => task.Status == TaskStatus.InProgress),
        Completed = await query.CountAsync(task => task.Status == TaskStatus.Completed)
      };

      _logger.LogInformation(
    "User {UserId} retrieved dashboard: Pending={Pending}, InProgress={InProgress}, Completed={Completed}",
    _currentUserService.UserId,
    status.Pending,
    status.InProgress,
    status.Completed);

      return status;
       
    }

}