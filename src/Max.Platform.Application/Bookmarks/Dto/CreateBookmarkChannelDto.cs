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
