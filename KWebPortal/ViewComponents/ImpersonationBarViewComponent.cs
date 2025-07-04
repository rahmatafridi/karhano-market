using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KWebPortal.ViewComponents
{
    public class ImpersonationBarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var isImpersonating = User.HasClaim(c => c.Type == "IsImpersonating" && c.Value == "True");
            
            if (!isImpersonating)
            {
                return Content(string.Empty);
            }

            var model = new ImpersonationBarViewModel
            {
                ImpersonatedUserName = User.FindFirstValue("Name"),
                ImpersonatedUserEmail = User.FindFirstValue(ClaimTypes.Name),
                OriginalUserName = User.FindFirstValue("OriginalUserName"),
                StoreName = User.FindFirstValue("StoreName")
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
