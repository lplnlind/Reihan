namespace Application.Interfaces
{
    public interface IUserContextService
    {
        int GetUserId();
        string GetUserRole();
        bool IsInRole(string role);
        bool IsAdmin(); 
    }
}
