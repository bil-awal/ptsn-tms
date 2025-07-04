using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.Models;
using TaskManagementSystem.Web.Services;
using System.Diagnostics;

namespace TaskManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, ITaskService taskService, IUserService userService)
        {
            _logger = logger;
            _taskService = taskService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                var users = await _userService.GetAllUsersAsync();
                
                ViewBag.TotalTasks = tasks.Count();
                ViewBag.TodoTasks = tasks.Count(t => t.Status == TaskManagementSystem.Web.Models.TaskStatus.Todo);
                ViewBag.InProgressTasks = tasks.Count(t => t.Status == TaskManagementSystem.Web.Models.TaskStatus.InProgress);
                ViewBag.CompletedTasks = tasks.Count(t => t.Status == TaskManagementSystem.Web.Models.TaskStatus.Completed);
                ViewBag.TotalUsers = users.Count();
                ViewBag.OverdueTasks = tasks.Count(t => t.DueDate < DateTime.Now && t.Status != TaskManagementSystem.Web.Models.TaskStatus.Completed && t.Status != TaskManagementSystem.Web.Models.TaskStatus.Cancelled);
                
                ViewBag.RecentTasks = tasks.OrderByDescending(t => t.CreatedAt).Take(5).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                ViewBag.TotalTasks = 0;
                ViewBag.TodoTasks = 0;
                ViewBag.InProgressTasks = 0;
                ViewBag.CompletedTasks = 0;
                ViewBag.TotalUsers = 0;
                ViewBag.OverdueTasks = 0;
                ViewBag.RecentTasks = new List<TaskViewModel>();
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
