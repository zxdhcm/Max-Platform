using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Max.Platform.Roles.Dto;
using Max.Platform.Users.Dto;

namespace Max.Platform.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
