namespace TaskManagement.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(string userId, string email, IEnumerable<string> roles);
}