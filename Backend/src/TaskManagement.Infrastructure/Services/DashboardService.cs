using TaskManagement.Persistence.Context;
using TaskManagement.Application.Interfaces;
namespace TaskManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.Dashboard;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Common;
public class DashboardService : IDashboardService
{
private readonly ApplicationDbContext _context;
private readonly ICurrentUserService _currentUserService;
public DashboardService(ApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

public async Task<DashboardResponse> GetDashboardAsync()
    {
       var query = _context.Tasks.AsQueryable();
      if(!_currentUserService.IsInRole(AppRoles.Admin))
      {
        query = query.Where(task => task.AssignedUserId == _currentUserService.UserId);
      }

      return new DashboardResponse
      {
          
        Pending = await query.CountAsync(task => task.Status == TaskStatus.Pending),
        InProgress = await query.CountAsync(task => task.Status == TaskStatus.InProgress),
        Completed = await query.CountAsync(task => task.Status == TaskStatus.Completed)
      };
       
    }

}