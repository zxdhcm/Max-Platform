using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Application.Services.Dto;

namespace Max.Platform.Bookmarks.Dto
{
   public class BookmarkGetAllInput: PagedResultRequestDto
    {
        public  int? ChannelId { get; set; }

        public string KeyWord { get; set; }
    }

    public class BookmarkGetFaviconInput
    {
        [Url, Required]
        public string Url { get; set; }
    }
}
