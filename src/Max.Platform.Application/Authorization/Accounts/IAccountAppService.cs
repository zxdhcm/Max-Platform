using System.Threading.Tasks;
using Abp.Application.Services;
using Max.Platform.Authorization.Accounts.Dto;

namespace Max.Platform.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
