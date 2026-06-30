using Hotelo.Core.DTOs.Identity;

namespace Hotelo.Core.Interfaces.Services;

public interface IUserService
{
    Task<UsersDashboardDto> GetDashboardAsync();
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserDto> CreateUserAsync(CreateUserDto dto);
    Task<UserDto> UpdateUserAsync(string id, UpdateUserDto dto);
    Task DeleteUserAsync(string id);
    Task ChangePasswordAsync(ChangePasswordDto dto);
    Task AssignRoleAsync(string userId, string role);
}
