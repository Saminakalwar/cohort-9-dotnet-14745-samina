namespace TaskManagement.Application.Interfaces;
public interface ICurrentUserService
{
    string? UserId{get;}
    string? Email{get;}
    bool IsAuthenticated{get;}
    bool IsInRole(string role);
}
// to know who is the current logged in user