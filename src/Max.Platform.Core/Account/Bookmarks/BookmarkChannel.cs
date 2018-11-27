using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Domain.Entities.Auditing;

namespace Max.Platform.Account.Bookmarks
{
   public class BookmarkChannel : FullAuditedEntity
    {
        /// <summary>
        /// 频道名称
        /// </summary>
        [Required]
        [StringLength(128)]
        public virtual string ChannelName { get; set; }

        /// <summary>
        /// 所属分类
        /// </summary>
        public virtual BookmarkClass BookmarkClass { get; set; }
    } 
}
