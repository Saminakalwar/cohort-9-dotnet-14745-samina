
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;


namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  //every endpoint in this controller requires authentication
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(CreateTaskRequest request)
    {
        try
        {
            var taskId = await _taskService.CreateTaskAsync(request);
            return Ok(new 
            { 
                Message = "Task created successfully",
                TaskId = taskId
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
    }
}