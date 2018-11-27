using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Max.Platform.Account.Bookmarks;
using Max.Platform.Bookmarks.Dto;
using Microsoft.AspNetCore.Http;

namespace Max.Platform.Bookmarks
{
   public class BookmarkClassAppService : AsyncCrudAppService<BookmarkClass, BookmarkClassDto, int, PagedResultRequestDto, CreateBookmarkClassDto, BookmarkClassDto>, IBookmarkClassAppService
    {
        
        public BookmarkClassAppService(IRepository<BookmarkClass> repository) : base(repository)
        {
            
        }

        protected override IQueryable<BookmarkClass> CreateFilteredQuery(PagedResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                .WhereIf(AbpSession.UserId.HasValue, t => t.CreatorUserId == AbpSession.UserId);
        }
    }
}
