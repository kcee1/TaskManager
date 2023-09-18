using TaskManager.DomainLayer.Models;

namespace TaskManagerApi.Auth
{
    public interface ITokenAuth 
    {
        string GenerateToken(User user, string role);
    }
}
