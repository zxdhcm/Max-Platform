using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Max.Platform.Configuration;
using Max.Platform.Web;

namespace Max.Platform.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class PlatformDbContextFactory : IDesignTimeDbContextFactory<PlatformDbContext>
    {
        public PlatformDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PlatformDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            PlatformDbContextConfigurer.Configure(builder, configuration.GetConnectionString(PlatformConsts.ConnectionStringName));

            return new PlatformDbContext(builder.Options);
        }
    }
}
