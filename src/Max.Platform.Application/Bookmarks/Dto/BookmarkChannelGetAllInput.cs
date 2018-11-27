using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Max.Platform.Bookmarks.Dto
{
   public class BookmarkChannelGetAllInput : PagedResultRequestDto
    {
        public  int? ClassId { get; set; }
    }
}
