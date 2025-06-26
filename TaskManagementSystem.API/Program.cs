using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Infrastructure.Data;
using TaskManagementSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/taskmanagement.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework with In-Memory Database
builder.Services.AddDbContext<TaskManagementContext>(options =>
    options.UseInMemoryDatabase("TaskManagementDb"));

// Register repositories
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register application services
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TaskManagementContext>();
    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

    // Create sample users
    var user1 = new TaskManagementSystem.Domain.Entities.User
    {
        Id = Guid.NewGuid(),
        Name = "John Doe",
        Email = "john.doe@example.com",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    var user2 = new TaskManagementSystem.Domain.Entities.User
    {
        Id = Guid.NewGuid(),
        Name = "Jane Smith",
        Email = "jane.smith@example.com",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    await unitOfWork.Users.AddAsync(user1);
    await unitOfWork.Users.AddAsync(user2);
    await unitOfWork.SaveChangesAsync();

    Log.Information("Initial data seeded successfully");
}

Log.Information("Starting Task Management System API");

app.Run();
