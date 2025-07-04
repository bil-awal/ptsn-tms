using System.Text;
using System.Text.Json;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Services
{
    public class TaskService : ITaskService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public TaskService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TaskManagementAPI");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IEnumerable<TaskViewModel>> GetAllTasksAsync()
        {
            var response = await _httpClient.GetAsync("api/Tasks");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<TaskViewModel>>(content, _jsonOptions) ?? new List<TaskViewModel>();
        }

        public async Task<TaskViewModel?> GetTaskByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Tasks/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TaskViewModel>(content, _jsonOptions);
        }

        public async Task<IEnumerable<TaskViewModel>> GetTasksByUserIdAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"api/Tasks/user/{userId}");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<TaskViewModel>>(content, _jsonOptions) ?? new List<TaskViewModel>();
        }

        public async Task<TaskViewModel> CreateTaskAsync(CreateTaskViewModel task)
        {
            var json = JsonSerializer.Serialize(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/Tasks", content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TaskViewModel>(responseContent, _jsonOptions)!;
        }

        public async Task<TaskViewModel> UpdateTaskAsync(Guid id, UpdateTaskViewModel task)
        {
            var json = JsonSerializer.Serialize(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"api/Tasks/{id}", content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TaskViewModel>(responseContent, _jsonOptions)!;
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Tasks/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
