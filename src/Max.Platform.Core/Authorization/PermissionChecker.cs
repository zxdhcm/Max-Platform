using Abp.Authorization;
using Max.Platform.Authorization.Roles;
using Max.Platform.Authorization.Users;

namespace Max.Platform.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
