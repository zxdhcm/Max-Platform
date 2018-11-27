using System.Threading.Tasks;
using Max.Platform.Configuration.Dto;

namespace Max.Platform.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
