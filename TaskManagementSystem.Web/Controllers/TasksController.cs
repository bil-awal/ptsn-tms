using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Web.Models;
using TaskManagementSystem.Web.Services;
using TaskStatus = TaskManagementSystem.Web.Models.TaskStatus;

namespace TaskManagementSystem.Web.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, IUserService userService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _userService = userService;
            _logger = logger;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(string? searchTerm, TaskStatus? status, TaskPriority? priority)
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                
                // Apply filters
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    tasks = tasks.Where(t => 
                        t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
                }
                
                if (status.HasValue)
                {
                    tasks = tasks.Where(t => t.Status == status.Value);
                }
                
                if (priority.HasValue)
                {
                    tasks = tasks.Where(t => t.Priority == priority.Value);
                }
                
                ViewBag.SearchTerm = searchTerm;
                ViewBag.StatusFilter = status;
                ViewBag.PriorityFilter = priority;
                
                return View(tasks.OrderByDescending(t => t.CreatedAt).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading tasks");
                TempData["Error"] = "Error loading tasks. Please try again.";
                return View(new List<TaskViewModel>());
            }
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                return View(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading task details");
                TempData["Error"] = "Error loading task details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Tasks/Create
        public async Task<IActionResult> Create()
        {
            await LoadUsersViewBag();
            return View(new CreateTaskViewModel());
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _taskService.CreateTaskAsync(model);
                    TempData["Success"] = "Task created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating task");
                    ModelState.AddModelError("", "Error creating task. Please try again.");
                }
            }
            
            await LoadUsersViewBag();
            return View(model);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                
                var model = new UpdateTaskViewModel
                {
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    Priority = task.Priority,
                    Status = task.Status,
                    AssignedToUserId = task.AssignedToUserId
                };
                
                await LoadUsersViewBag();
                ViewBag.TaskId = id;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading task for edit");
                TempData["Error"] = "Error loading task.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _taskService.UpdateTaskAsync(id, model);
                    TempData["Success"] = "Task updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating task");
                    ModelState.AddModelError("", "Error updating task. Please try again.");
                }
            }
            
            await LoadUsersViewBag();
            ViewBag.TaskId = id;
            return View(model);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                return View(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading task for delete");
                TempData["Error"] = "Error loading task.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                TempData["Success"] = "Task deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task");
                TempData["Error"] = "Error deleting task. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task LoadUsersViewBag()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                ViewBag.Users = new SelectList(users, "Id", "Name");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading users");
                ViewBag.Users = new SelectList(new List<UserViewModel>(), "Id", "Name");
            }
        }
    }
}
