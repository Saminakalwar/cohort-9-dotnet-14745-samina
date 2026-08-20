using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
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

            var result = await _userManager.CreateAsync(user,request.Password);

            return result.Succeeded;
        
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

        return new LoginResponse
        {
            Email = userExist.Email ?? string.Empty,
            Token = token
        };
        
    }
}
