using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(string name, string email);
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
