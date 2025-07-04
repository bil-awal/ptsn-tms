using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.Models;
using TaskManagementSystem.Web.Services;

namespace TaskManagementSystem.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ITaskService taskService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _taskService = taskService;
            _logger = logger;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading users");
                TempData["Error"] = "Error loading users. Please try again.";
                return View(new List<UserViewModel>());
            }
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                
                var userTasks = await _taskService.GetTasksByUserIdAsync(id);
                ViewBag.UserTasks = userTasks;
                
                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user details");
                TempData["Error"] = "Error loading user details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.CreateUserAsync(model);
                    TempData["Success"] = "User created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating user");
                    ModelState.AddModelError("", "Error creating user. Please try again.");
                }
            }
            
            return View(model);
        }
    }
}
