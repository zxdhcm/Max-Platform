using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.UI;
using AutoMapper.QueryableExtensions;
using Max.Platform.Authorization.Roles;
using Max.Platform.MultiTenancy;

namespace Max.Platform.Authorization.Users
{
    public class UserRegistrationManager : DomainService
    {
        public IAbpSession AbpSession { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Role> _roles;

        public UserRegistrationManager(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher, ICacheManager cacheManager, IRepository<Role> roles)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _cacheManager = cacheManager;
            _roles = roles;

            AbpSession = NullAbpSession.Instance;
        }

        public async Task<User> RegisterAsync(string name, string surname, string emailAddress, string userName, string plainPassword, string phoneNumber, string code, bool isEmailConfirmed)
        {
            CheckForTenant();
            await CheckForPhoneNumber(phoneNumber, code);
            var tenant = await GetActiveTenantAsync();

            var user = new User
            {
                TenantId = tenant.Id,
                Name = name,
                Surname = surname,
                EmailAddress = emailAddress,
                IsActive = true,
                UserName = userName,
                IsEmailConfirmed = isEmailConfirmed,
                Roles = new List<UserRole>(),
                PhoneNumber = phoneNumber,
                IsPhoneNumberConfirmed = true,
                Avator = $"HeadPortrait{new Random().Next(1, PlatformConsts.AvatorNumber)}.jpg"
            };

            user.SetNormalizedNames();

            var defaultRole = await _roles.FirstOrDefaultAsync(r => r.IsDefault && r.Name == "Users");
            if (defaultRole != null)
            {
                user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
            }

            await _userManager.InitializeOptionsAsync(tenant.Id);

            CheckErrors(await _userManager.CreateAsync(user, plainPassword));
            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        public async Task<User> RegisterAsync(string name, string surname, string emailAddress, string userName, string plainPassword, bool isEmailConfirmed)
        {
            CheckForTenant();
            var tenant = await GetActiveTenantAsync();

            var user = new User
            {
                TenantId = tenant.Id,
                Name = name,
                Surname = surname,
                EmailAddress = emailAddress,
                IsActive = true,
                UserName = userName,
                IsEmailConfirmed = isEmailConfirmed,
                Roles = new List<UserRole>(),
                IsPhoneNumberConfirmed = true
            };

            user.SetNormalizedNames();

            foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
            {
                user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
            }

            await _userManager.InitializeOptionsAsync(tenant.Id);

            CheckErrors(await _userManager.CreateAsync(user, plainPassword));
            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        #region 验证手机验证码
        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> ValidateCodeAsync(string phoneNumber, string code)
        {
            var cache = _cacheManager.GetCache(PlatformConsts.VerificationCodeCacheName);
            var cacheCode = await cache.GetOrDefaultAsync<string, string>(phoneNumber);
            return !string.IsNullOrWhiteSpace(cacheCode) && cacheCode == code;
        }
        #endregion

        #region 生成手机验证码

        /// <summary>
        /// 生成手机验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="type">生成的验证码类型(1:注册验证码,2:修改账户密码验证码)</param>
        /// <returns></returns>
        public async Task<string> GenerateVerificationCodeAsync(string phoneNumber, int type = 1)
        {
            var code = string.Empty;
            if (type == 1)
            {
                var cache = _cacheManager.GetCache(PlatformConsts.VerificationCodeCacheName);
                var cacheCode = await cache.GetOrDefaultAsync<string, string>(phoneNumber);
                if (!string.IsNullOrWhiteSpace(cacheCode))
                {
                    return cacheCode;
                }

                code = new Random().Next(1000, 9999).ToString();
                var absoluteExpireTime = new DateTime().AddHours(1).Subtract(new DateTime());
                await cache.SetAsync(phoneNumber, code, absoluteExpireTime: absoluteExpireTime);
            }
            else if (type == 2 && AbpSession.UserId.HasValue)
            {
                var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
                code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            }
            return code;
        }
        #endregion

        private void CheckForTenant()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new InvalidOperationException("Can not register host users!");
            }
        }

        private async Task CheckForPhoneNumber(string phoneNumber, string code)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(code))
            {
                return;
            }
            var user = await _userManager.GetUserByPhoneNumberAsync(phoneNumber);
            if (user != null)
            {
                throw new UserFriendlyException("该手机号码已被使用！");
            }

            var falg = await ValidateCodeAsync(phoneNumber, code);
            if (!falg)
            {
                throw new UserFriendlyException("验证码错误！");
            }
        }

        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await GetActiveTenantAsync(AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
