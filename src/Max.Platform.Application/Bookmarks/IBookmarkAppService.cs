using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Max.Platform.Bookmarks.Dto;

namespace Max.Platform.Bookmarks
{
    public interface IBookmarkAppService: IAsyncCrudAppService<BookmarkDto, int, BookmarkGetAllInput, CreateBookmarkDto, BookmarkDto>
    {
        Task<WebSiteInfoDto> PostWebSiteInfo(BookmarkGetFaviconInput input);
    }
}
