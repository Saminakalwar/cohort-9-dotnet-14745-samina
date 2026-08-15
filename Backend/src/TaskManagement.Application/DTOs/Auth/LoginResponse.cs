namespace TaskManagement.Application.DTOs.Auth;
public class LoginResponse
{
    public string Email {get; set;} = string.Empty;
    public string Token { get; set;} = string.Empty;
}