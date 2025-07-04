using TaskManagementSystem.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure for Heroku deployment
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://+:{port}");

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Configure HttpClient for API calls
builder.Services.AddHttpClient("TaskManagementAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://localhost:5001/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Register services
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Comment out HTTPS redirection for Heroku
// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
