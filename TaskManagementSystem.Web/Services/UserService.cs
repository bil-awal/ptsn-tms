using System.Text;
using System.Text.Json;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TaskManagementAPI");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/Users");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<UserViewModel>>(content, _jsonOptions) ?? new List<UserViewModel>();
        }

        public async Task<UserViewModel?> GetUserByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Users/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserViewModel>(content, _jsonOptions);
        }

        public async Task<UserViewModel> CreateUserAsync(CreateUserViewModel user)
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/Users", content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserViewModel>(responseContent, _jsonOptions)!;
        }
    }
}
