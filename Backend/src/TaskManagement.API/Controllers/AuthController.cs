using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Entities;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(IAuthService authService, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
    {
        _authService = authService;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

// Register Controller
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        if (!result)
        {
            return BadRequest("User Registration Failed");

        }
        return Ok("User Registered Successfully");
    }


// Login Controller
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        if (result is null)
        {
            return Unauthorized("Invalid email or Password");
        }
        return Ok(result);
    }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            return Ok(new
            {
                IsAuthenticated = _currentUserService.IsAuthenticated,
                UserId = _currentUserService.UserId,
                Email = _currentUserService.Email,
                IsUser = _currentUserService.IsInRole(AppRoles.User),
                IsAdmin = _currentUserService.IsInRole(AppRoles.Admin)
            });
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpGet("admin")]
        public IActionResult AdminOnly()
        {
            return Ok("Welcome Admin!");
        }


        [HttpPost("make-admin/{email}")]
        public async Task<IActionResult> MakeAdmin(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.AddToRoleAsync(
                user,
                AppRoles.Admin);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Admin role assigned successfully.");
        }


}




    // [Authorize]
    // [HttpGet("protected")]
    // public IActionResult Protected()
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     var email = User.FindFirst(ClaimTypes.Email)?.Value;
    //     var claims = User.Claims.Select(c=> new{c.Type, c.Value});

    //     return Ok(new
    //     {
    //         IsAuthenticated = User.Identity?.IsAuthenticated,
    //         UserId = userId,
    //         Email = email,
    //         Claims = claims
    //     });
    //     // return Ok("You are authenticated!");
    // }