using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Extensions;
using Max.Platform.Account.Bookmarks;
using Max.Platform.Authorization.Users;
using Max.Platform.Bookmarks.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Max.Platform.Bookmarks
{
    public class BookmarkChannelAppService : AsyncCrudAppService<BookmarkChannel, BookmarkChannelDto, int, BookmarkChannelGetAllInput, CreateBookmarkChannelDto, BookmarkChannelDto>, IBookmarkChannelAppService
    {
        private readonly IRepository<BookmarkClass> _classRepository;
        public BookmarkChannelAppService(IRepository<BookmarkChannel> repository, IRepository<BookmarkClass> classRepository) : base(repository)
        {
            _classRepository = classRepository;
        }

        #region 创建书签频道
        /// <summary>
        /// 创建书签频道
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<BookmarkChannelDto> Create(CreateBookmarkChannelDto input)
        {
            CheckCreatePermission();

            var bookmarkChannel = MapToEntity(input);
            bookmarkChannel.BookmarkClass =
                await _classRepository.FirstOrDefaultAsync(c => c.Id == input.BookmarkClassId);
            await Repository.InsertAsync(bookmarkChannel);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(bookmarkChannel);
        }
        #endregion

        #region 更新书签频道
        /// <summary>
        /// 更新书签频道
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<BookmarkChannelDto> Update(BookmarkChannelDto input)
        {
            CheckUpdatePermission();

            var bookmarkChannel = await GetEntityByIdAsync(input.Id);
            MapToEntity(input, bookmarkChannel);
            bookmarkChannel.BookmarkClass =
                await _classRepository.FirstOrDefaultAsync(c => c.Id == input.BookmarkClassId);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(bookmarkChannel);
        }
        #endregion

        protected override IQueryable<BookmarkChannel> CreateFilteredQuery(BookmarkChannelGetAllInput input)
        {
            return base.CreateFilteredQuery(input)
                .WhereIf(input.ClassId.HasValue && input.ClassId > 0, t => t.BookmarkClass.Id == input.ClassId.Value)
                .WhereIf(AbpSession.UserId.HasValue, t => t.CreatorUserId == AbpSession.UserId);
        }
    }
}
