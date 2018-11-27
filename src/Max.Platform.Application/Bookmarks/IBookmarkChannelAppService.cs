using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Max.Platform.Bookmarks.Dto;

namespace Max.Platform.Bookmarks
{
    public interface IBookmarkChannelAppService : IAsyncCrudAppService<BookmarkChannelDto, int, BookmarkChannelGetAllInput, CreateBookmarkChannelDto, BookmarkChannelDto>
    {
    }
}
