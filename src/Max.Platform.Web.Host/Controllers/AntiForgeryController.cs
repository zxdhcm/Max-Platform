using Microsoft.AspNetCore.Antiforgery;
using Max.Platform.Controllers;

namespace Max.Platform.Web.Host.Controllers
{
    public class AntiForgeryController : PlatformControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
