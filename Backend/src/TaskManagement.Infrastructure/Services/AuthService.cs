using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    // private readonly Signin

    public AuthService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
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


    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var userExist = await _userManager.FindByEmailAsync(request.Email);
        if(userExist is null)
        {
            return null;
        }

        var validPass = await _userManager.CheckPasswordAsync(userExist, request.Password);
        if (!validPass)
        {
            return null;
        }

        return new LoginResponse
        {
            Email = userExist.Email ?? string.Empty
        };
        
    }
}
