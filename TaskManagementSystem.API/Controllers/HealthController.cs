using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Infrastructure.Data;

namespace TaskManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        private readonly ILogger<HealthController> _logger;

        public HealthController(TaskManagementContext context, ILogger<HealthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Check database connectivity
                var canConnect = await _context.Database.CanConnectAsync();
                
                var healthStatus = new
                {
                    status = "Healthy",
                    timestamp = DateTime.UtcNow,
                    services = new
                    {
                        database = canConnect ? "Connected" : "Disconnected",
                        api = "Running"
                    },
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                    version = "1.0.0"
                };

                _logger.LogInformation("Health check performed successfully");
                return Ok(healthStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(503, new
                {
                    status = "Unhealthy",
                    timestamp = DateTime.UtcNow,
                    error = ex.Message
                });
            }
        }

        [HttpGet("ready")]
        public IActionResult Ready()
        {
            // Simple readiness check
            return Ok(new { ready = true, timestamp = DateTime.UtcNow });
        }

        [HttpGet("live")]
        public IActionResult Live()
        {
            // Simple liveness check
            return Ok(new { alive = true, timestamp = DateTime.UtcNow });
        }
    }
}