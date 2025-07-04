using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KWebPortal.ViewComponents
{
    public class ImpersonationBarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsPrincipal = HttpContext.User as ClaimsPrincipal;
            
            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "IsImpersonating" && c.Value == "True"))
            {
                return Content(string.Empty);
            }

            var model = new ImpersonationBarViewModel
            {
                ImpersonatedUserName = claimsPrincipal.FindFirst("Name")?.Value,
                ImpersonatedUserEmail = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value,
                OriginalUserName = claimsPrincipal.FindFirst("OriginalUserName")?.Value,
                StoreName = claimsPrincipal.FindFirst("StoreName")?.Value
            };

            return View(model);
        }
    }

    public class ImpersonationBarViewModel
    {
        public string ImpersonatedUserName { get; set; }
        public string ImpersonatedUserEmail { get; set; }
        public string OriginalUserName { get; set; }
        public string StoreName { get; set; }
    }
}
