using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Max.Platform.Controllers
{
    public abstract class PlatformControllerBase: AbpController
    {
        protected PlatformControllerBase()
        {
            LocalizationSourceName = PlatformConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
