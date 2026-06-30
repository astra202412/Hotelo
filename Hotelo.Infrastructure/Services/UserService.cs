using Hotelo.Core.DTOs.Identity;
using Hotelo.Core.Entities.Identity;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserService(UserManager<ApplicationUser> userManager,
                       RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    private async Task<UserDto> ToDto(ApplicationUser u)
    {
        var roles = await _userManager.GetRolesAsync(u);
        return new UserDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email ?? "",
            UserName = u.UserName ?? "",
            IsActive = u.IsActive,
            EmailConfirmed = u.EmailConfirmed,
            Roles = roles.ToList(),
            CreatedAt = u.CreatedAt
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.OrderBy(u => u.FullName).ToListAsync();
        var result = new List<UserDto>();
        foreach (var u in users) result.Add(await ToDto(u));
        return result;
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var u = await _userManager.FindByIdAsync(id);
        return u == null ? null : await ToDto(u);
    }

    public async Task<UsersDashboardDto> GetDashboardAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var roles = await _roleManager.Roles.Select(r => r.Name ?? "").ToListAsync();
        var dtos = new List<UserDto>();
        foreach (var u in users) dtos.Add(await ToDto(u));

        return new UsersDashboardDto
        {
            TotalUsers = users.Count,
            ActiveUsers = users.Count(u => u.IsActive),
            TotalRoles = roles.Count,
            Users = dtos,
            AvailableRoles = roles
        };
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        var user = new ApplicationUser
        {
            FullName = dto.FullName,
            Email = dto.Email,
            UserName = dto.Email,
            IsActive = true,
            EmailConfirmed = true,
            DepartmentId = dto.DepartmentId,
            CreatedAt = DateTime.UtcNow
        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        if (!string.IsNullOrEmpty(dto.Role))
            await _userManager.AddToRoleAsync(user, dto.Role);

        return await ToDto(user);
    }

    public async Task<UserDto> UpdateUserAsync(string id, UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"Utilisateur {id} introuvable");

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.UserName = dto.Email;
        user.IsActive = dto.IsActive;
        await _userManager.UpdateAsync(user);

        // Mettre a jour le role
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!string.IsNullOrEmpty(dto.Role))
            await _userManager.AddToRoleAsync(user, dto.Role);

        return await ToDto(user);
    }

    public async Task DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"Utilisateur {id} introuvable");
        user.IsActive = false;
        await _userManager.UpdateAsync(user);
    }

    public async Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId)
            ?? throw new KeyNotFoundException("Utilisateur introuvable");
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task AssignRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException("Utilisateur introuvable");
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);
    }
}
