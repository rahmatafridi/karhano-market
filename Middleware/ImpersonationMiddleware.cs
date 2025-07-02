using KarhanoMarket.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KarhanoMarket.Middleware
{
    public class ImpersonationMiddleware
    {
        private readonly RequestDelegate _next;

        public ImpersonationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var isImpersonating = user.HasClaim(c => c.Type == "IsImpersonating" && c.Value == "true");
                if (isImpersonating)
                {
                    Guid impersonatorId = Guid.Parse(user.FindFirst("ImpersonatorId")?.Value);
                    var appUser = context.Items["ApplicationUser"] as ApplicationUser;
                    if (appUser != null)
                    {
                        appUser.IsImpersonated = true;
                        appUser.ImpersonatorId = impersonatorId;
                    }
                }
            }

            await _next(context);
        }
    }
}
