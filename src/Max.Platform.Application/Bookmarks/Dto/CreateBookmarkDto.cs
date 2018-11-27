using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Max.Platform.Account.Bookmarks;

namespace Max.Platform.Bookmarks.Dto
{
    [AutoMapTo(typeof(Bookmark))]
    public class CreateBookmarkDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        /// <summary>
        /// 网站链接
        /// </summary>
        [Required]
        [Url]
        public string Url { get; set; }

        /// <summary>
        /// 网站的Favicon链接
        /// </summary>
        public string FaviconUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 频道Id
        /// </summary>
        public int BookmarkChannelId { get; set; }


    }
}
