using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Max.Platform.EntityFrameworkCore
{
    public static class PlatformDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<PlatformDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<PlatformDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
