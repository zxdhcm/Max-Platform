using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Runtime.Caching;
using Abp.UI;
using Abp.Zero.Configuration;
using Max.Platform.Authorization.Accounts.Dto;
using Max.Platform.Authorization.Users;

namespace Max.Platform.Authorization.Accounts
{
    public class AccountAppService : PlatformAppServiceBase, IAccountAppService
    {
        private readonly UserRegistrationManager _userRegistrationManager;

        public AccountAppService(
            UserRegistrationManager userRegistrationManager)
        {
            _userRegistrationManager = userRegistrationManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                input.PhoneNumber,
                input.CaptchaResponse,
                true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }

        #region 获取手机验证码
        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<string> GetVerificationCode(PhoneNumberInput input)
        {
            return await _userRegistrationManager.GenerateVerificationCodeAsync(input.PhoneNumber, input.Type ?? 1);
        } 
        #endregion
    }
}
