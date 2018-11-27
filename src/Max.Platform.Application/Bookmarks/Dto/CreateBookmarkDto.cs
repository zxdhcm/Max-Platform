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
        /// ����
        /// </summary>
        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        /// <summary>
        /// ��վ����
        /// </summary>
        [Required]
        [Url]
        public string Url { get; set; }

        /// <summary>
        /// ��վ��Favicon����
        /// </summary>
        public string FaviconUrl { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Ƶ��Id
        /// </summary>
        public int BookmarkChannelId { get; set; }


    }
}
