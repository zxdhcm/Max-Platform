using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Max.Platform.Account.Bookmarks;
using Max.Platform.Authorization.Roles;
using Max.Platform.Authorization.Users;
using Max.Platform.MultiTenancy;

namespace Max.Platform.EntityFrameworkCore
{
    public class PlatformDbContext : AbpZeroDbContext<Tenant, Role, User, PlatformDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookmarkClass> BookmarkClasss { get; set; }

        public virtual DbSet<BookmarkChannel> BookmarkChannels { get; set; }

        public virtual DbSet<Bookmark> Bookmarks { get; set; }
    }
}
