using EasyBuy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EasyBuy.Services.AUTH
{
    public class AuthService : IAuthService
    {
        private readonly EasyBuyContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(EasyBuyContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                // Tìm user theo email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user != null && user.AccountStatus == "Active")
                {
                    // Kiểm tra password bằng BCrypt (tương thích với logic hiện tại)
                    if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                    {
                        // Lưu thông tin user vào session (tương thích với logic hiện tại)
                        var session = _httpContextAccessor.HttpContext?.Session;
                        if (session != null)
                        {
                            session.SetInt32("UserID", user.UserId);
                            session.SetString("Phone", user.Phone ?? "");
                            session.SetString("Role", user.Role ?? "");
                            session.SetString("UserEmail", user.Email ?? "");
                            session.SetString("UserName", user.FullName ?? "");
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đăng nhập: {ex.Message}");
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.Clear();
            }
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return false;

            var userId = session.GetInt32("UserID");
            return userId.HasValue;
        }

        public async Task<string?> GetCurrentUserRoleAsync()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return null;

            return session.GetString("Role");
        }

        public async Task<int?> GetCurrentUserIdAsync()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return null;

            return session.GetInt32("UserID");
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId.HasValue)
            {
                return await _context.Users.FindAsync(userId.Value);
            }
            return null;
        }

        public async Task<bool> HasRoleAsync(string role)
        {
            var currentRole = await GetCurrentUserRoleAsync();
            return currentRole == role;
        }

        public async Task<bool> IsInAnyRoleAsync(params string[] roles)
        {
            var currentRole = await GetCurrentUserRoleAsync();
            return roles.Contains(currentRole);
        }
    }
} 