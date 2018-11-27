using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Max.Platform.Account.Bookmarks;

namespace Max.Platform.Bookmarks.Dto
{
    [AutoMapTo(typeof(BookmarkClass))]
    public class CreateBookmarkClassDto
    {
        /// <summary>
        /// ∑÷¿‡√˚≥∆
        /// </summary>
        [Required]
        [StringLength(128)]
        public string ClassName { get; set; }
    }
}
