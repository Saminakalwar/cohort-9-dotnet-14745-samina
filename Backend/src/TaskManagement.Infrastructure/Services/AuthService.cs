using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Common;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService, ICurrentUserService currentUserService, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if(existingUser is not null)
        {
            return false;
        }

        var user = new ApplicationUser
          {  FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email
            };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return false;
        }

        var roleResult =
            await _userManager.AddToRoleAsync(
                user,
                AppRoles.User);

        _logger.LogInformation("User {Email} registered successfully", user.Email);
        return roleResult.Succeeded;
        
    }


    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var userExist = await _userManager.FindByEmailAsync(request.Email);
        if(userExist is null)
        {
            return null;
        }

        var validPassword = await _userManager.CheckPasswordAsync(userExist, request.Password);
        if (!validPassword)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(userExist);
        var token = _jwtService.GenerateToken(userExist.Id, userExist.Email ?? string.Empty, roles);

        _logger.LogInformation("User {Email} logged in successfully", userExist.Email);
        return new LoginResponse
        {
            Email = userExist.Email ?? string.Empty,
            Token = token
        };
        
    }

    public async Task<ProfileResponse?> GetProfileAsync()
    {
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return null;
        }

        var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
        if (user == null)
        {
            return null;
        }

        return new ProfileResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty
        };
    }
}
