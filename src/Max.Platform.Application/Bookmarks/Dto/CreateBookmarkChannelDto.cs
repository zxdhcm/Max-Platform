using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Max.Platform.Account.Bookmarks;

namespace Max.Platform.Bookmarks.Dto
{
    [AutoMapTo(typeof(BookmarkChannel))]
    public class CreateBookmarkChannelDto
    {
        /// <summary>
        /// 频道名称
        /// </summary>
        [Required]
        [StringLength(128)]
        public string ChannelName { get; set; }

        /// <summary>
        /// 所属分类
        /// </summary>
        public int BookmarkClassId { get; set; }

    }
}
