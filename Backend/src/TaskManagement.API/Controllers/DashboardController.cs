using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Dashboard;
using TaskManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboardData = await _dashboardService.GetDashboardAsync();
        return Ok(dashboardData);
    }
}