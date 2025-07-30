using EasyBuy.Services.AUTH;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EasyBuy.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRoleAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _allowedRoles;

        public AuthorizeRoleAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();

            // Kiểm tra xem user đã đăng nhập chưa
            if (!await authService.IsAuthenticatedAsync())
            {
                context.Result = new RedirectToActionResult("Error404", "Error", new { area = "" });
                return;
            }

            // Kiểm tra role
            if (_allowedRoles.Length > 0)
            {
                var currentRole = await authService.GetCurrentUserRoleAsync();
                
                // Nếu user đã đăng nhập nhưng không có role phù hợp
                if (!await authService.IsInAnyRoleAsync(_allowedRoles))
                {
                    // User đã đăng nhập nhưng không có quyền -> Access Denied
                    context.Result = new RedirectToActionResult("Error404", "Error", new { area = "" });
                    return;
                }
            }
        }
    }
} 