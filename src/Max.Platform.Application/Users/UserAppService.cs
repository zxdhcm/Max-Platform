using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using Max.Platform.Authorization;
using Max.Platform.Authorization.Roles;
using Max.Platform.Authorization.Users;
using Max.Platform.Roles.Dto;
using Max.Platform.Users.Dto;

namespace Max.Platform.Users
{
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task<UserDto> Create(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;
            user.Avator = $"HeadPortrait{new Random().Next(1, PlatformConsts.AvatorNumber)}.jpg";
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        [AbpAuthorize(new[] { PermissionNames.Pages_Users, PermissionNames.Pages_Accounts })]
        public override async Task<UserDto> Update(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);
           
            MapToEntity(input, user);
           
            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            return await Get(input);
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task Delete(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        #region 修改账户密码
        /// <summary>
        /// 修改账户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(new[] { PermissionNames.Pages_Users, PermissionNames.Pages_Accounts })]
        public async Task PostChangePassWord(ChangePassWordInput input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("参数错误！");
            }
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            CheckErrors(await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword));
        }
        #endregion

        #region 修改手机号码
        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [AbpAuthorize(new[] { PermissionNames.Pages_Users, PermissionNames.Pages_Accounts })]
        public async Task PostChangePhoneNumber(ChangePhoneNumberInput input)
        {
            await CheckForPhoneNumber(input.PhoneNumber);
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("参数错误！");
            }
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            CheckErrors(await _userManager.ChangePhoneNumberAsync(user, input.PhoneNumber, input.CaptchaResponse));
        }
        #endregion

        #region 获取手机号码
        /// <summary>
        /// 获取手机号码
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPhoneNumber()
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("参数错误！");
            }
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            return await _userManager.GetPhoneNumberAsync(user);
        } 
        #endregion

        private async Task CheckForPhoneNumber(string phoneNumber)
        {
            var user = await _userManager.GetUserByPhoneNumberAsync(phoneNumber);
            if (user != null)
            {
                throw new UserFriendlyException("该手机号码已被使用！");
            }
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roles = _roleManager.Roles.Where(r => user.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.NormalizedName);
            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
