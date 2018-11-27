using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Max.Platform.Account.Bookmarks;

namespace Max.Platform.Bookmarks.Dto
{
    [AutoMapFrom(typeof(BookmarkChannel))]
    public class BookmarkChannelDto : EntityDto
    {
        /// <summary>
        /// Ƶ������
        /// </summary>
        [Required]
        [StringLength(128)]
        public string ChannelName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public int BookmarkClassId { get; set; }
    }
}
