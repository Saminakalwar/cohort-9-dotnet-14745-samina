
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


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _taskService.GetTaskAsync();
        return Ok(tasks);
    }

    [Authorize]
[HttpGet("{id:guid}")]
public async Task<IActionResult> GetTaskById(Guid id)
{
    var task = await _taskService.GetTaskByIdAsync(id);

    if (task == null)
    {
        return NotFound(new
        {
            message = "Task not found."
        });
    }

    return Ok(task);
}

[Authorize]
[HttpPut("{id:guid}")]
public async Task<IActionResult> UpdateTask(
    Guid id,
    [FromBody] UpdateTaskRequest request)
{
    await _taskService.UpdateTaskAsync(id, request);

    return Ok(new
    {
        message = "Task updated successfully."
    });
}

[Authorize]
[HttpDelete("{id:guid}")]
public async Task<IActionResult> DeleteTask(Guid id)
{
    await _taskService.DeleteTaskAsync(id);

    return Ok(new
    {
        message = "Task deleted successfully."
    });
}

}