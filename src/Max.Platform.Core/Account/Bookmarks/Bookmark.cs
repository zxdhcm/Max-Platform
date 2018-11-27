using System;
using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace Max.Platform.Account.Bookmarks
{
    public class Bookmark : FullAuditedEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(128)] public virtual string Title { get; set; }

        /// <summary>
        /// 网站链接
        /// </summary>
        [Required]
        [Url]
        public virtual string Url { get; set; }

        /// <summary>
        /// 网站的Favicon链接
        /// </summary>
        public virtual string FaviconUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remarks { get; set; }

        /// <summary>
        /// 所属频道
        /// </summary>
        public virtual BookmarkChannel BookmarkChannel { get; set; }
    }
}
