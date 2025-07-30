using EasyBuy.Models;

namespace EasyBuy.Services.AUTH
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
        Task<string?> GetCurrentUserRoleAsync();
        Task<int?> GetCurrentUserIdAsync();
        Task<User?> GetCurrentUserAsync();
        Task<bool> HasRoleAsync(string role);
        Task<bool> IsInAnyRoleAsync(params string[] roles);
    }
} 