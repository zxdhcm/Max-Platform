﻿using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Max.Platform.Authorization
{
    public class PlatformAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Accounts, L("Accounts"));
            context.CreatePermission(PermissionNames.Pages_Dashboards, L("Dashboards"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, PlatformConsts.LocalizationSourceName);
        }
    }
}
