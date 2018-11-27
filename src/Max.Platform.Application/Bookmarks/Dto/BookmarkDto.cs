using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Max.Platform.Account.Bookmarks;

namespace Max.Platform.Bookmarks.Dto
{
    [AutoMapFrom(typeof(Bookmark))]
    public class BookmarkDto : EntityDto
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

    public class WebSiteInfoDto
    {
        /// <summary>
        /// ��վ����
        /// </summary>
        public  string Title { get; set; }

        /// <summary>
        /// ��վͼ���ַ
        /// </summary>
        public string FaviconUrl { get; set; }

        /// <summary>
        /// ��վ����
        /// </summary>
        public string Description { get; set; }
    }
}
