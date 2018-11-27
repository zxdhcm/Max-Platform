using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Max.Platform.Account.Bookmarks;

namespace Max.Platform.Bookmarks.Dto
{
    [AutoMapFrom(typeof(BookmarkClass))]
    public class BookmarkClassDto : EntityDto
    {
        /// <summary>
        /// ∑÷¿‡√˚≥∆
        /// </summary>
        [Required]
        [StringLength(128)]
        public string ClassName { get; set; }
    }
}
