using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManger.API.Data;
using TaskManger.API.DTOs;
using TaskManger.API.Models;
using TaskManger.API.Services;

namespace TaskManger.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [EnableRateLimiting("fixed")]
    public class TasksController : Controller
    {
        private readonly AppDpContext _context;
        private readonly LogService _logService;
        public TasksController(AppDpContext context,LogService logService)
        {
            _context = context;
            _logService = logService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = Guid.Parse(userId)
            };

            var response = new TaskResponseDto
            {
                Id = task.Id,
                title = task.Title,
                description = task.Description,
                status = task.Status,
                createdAt = task.CreatedAt
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            await _logService.LogAsync(Guid.Parse(userId), $"Task created: {task.Id}",
                HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()??"Unkown");

            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var tasks = await _context.Tasks.
                Where(t => t.UserId == Guid.Parse(userId)).
                OrderByDescending(t => t.CreatedAt).
                Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    title = t.Title,
                    description = t.Description,
                    status = t.Status,
                    createdAt = t.CreatedAt
                })
                .ToListAsync();
            return Ok(tasks);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            
            var userGuid = Guid.Parse(userId);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userGuid);

            if (task == null)
                return NotFound(new {message = "No Task with this ID OR It's not your task"});

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.status;

            await _context.SaveChangesAsync();
            
            await _logService.LogAsync(Guid.Parse(userId), $"Task updated: {task.Id}",
                HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "Unkown");

            var response = new TaskResponseDto
            {
                Id = task.Id,
                title = task.Title,
                description = task.Description,
                status = task.Status,
                createdAt = task.CreatedAt
            };
            return Ok(response);
        }
        [HttpDelete("{id:guid}")]
        public  async Task<IActionResult>DeleteTask(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                return Unauthorized();

                var userGuid = Guid.Parse(userId);
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userGuid);

            if (task == null)
                return NotFound(new {message = "No task with this ID OR it's not your task" });

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            
            await _logService.LogAsync(Guid.Parse(userId), $"Task deleted: {task.Id}",
                HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "Unkown");

            return NoContent();
        }
    }
}