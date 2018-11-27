using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Domain.Entities.Auditing;

namespace Max.Platform.Account.Bookmarks
{
   public class BookmarkClass:FullAuditedEntity
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        [Required]
        [StringLength(128)]
        public virtual string ClassName { get; set; }
    }
}
