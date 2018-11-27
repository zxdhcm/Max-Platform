using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Max.Platform.MultiTenancy;

namespace Max.Platform.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
