using System.Threading.Tasks;
using Abp.Application.Services;
using Max.Platform.Sessions.Dto;

namespace Max.Platform.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
