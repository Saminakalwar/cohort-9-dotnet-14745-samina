using TaskManagement.Application.DTOs.Auth;
namespace TaskManagement.Application.Interfaces;
public interface IAuthService
{
    Task <bool> RegisterAsync(RegisterRequest request);
    Task <LoginResponse?> LoginAsync(LoginRequest request);

    Task<ProfileResponse?> GetProfileAsync();
}