using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
        Task<UserViewModel?> GetUserByIdAsync(Guid id);
        Task<UserViewModel> CreateUserAsync(CreateUserViewModel user);
    }
}
