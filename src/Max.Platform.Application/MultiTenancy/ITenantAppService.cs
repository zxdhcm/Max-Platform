using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Max.Platform.MultiTenancy.Dto;

namespace Max.Platform.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
