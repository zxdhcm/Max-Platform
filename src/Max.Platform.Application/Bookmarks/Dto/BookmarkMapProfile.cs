using AutoMapper;
using Max.Platform.Account.Bookmarks;

namespace Max.Platform.Bookmarks.Dto
{
    public class BookmarkMapProfile : Profile
    {
        public BookmarkMapProfile()
        {
            CreateMap<BookmarkChannelDto, BookmarkChannel>()
                .ForMember(x => x.BookmarkClass, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.IsDeleted, opt => opt.Ignore())
                .ForMember(x => x.DeletionTime, opt => opt.Ignore())
                .ForMember(x => x.DeleterUserId, opt => opt.Ignore())
                .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
                .ForMember(x => x.LastModifierUserId, opt => opt.Ignore())
                .ForMember(x => x.CreatorUserId, opt => opt.Ignore());

            CreateMap<BookmarkDto, Bookmark>()
                .ForMember(x => x.BookmarkChannel, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.IsDeleted, opt => opt.Ignore())
                .ForMember(x => x.DeletionTime, opt => opt.Ignore())
                .ForMember(x => x.DeleterUserId, opt => opt.Ignore())
                .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
                .ForMember(x => x.LastModifierUserId, opt => opt.Ignore())
                .ForMember(x => x.CreatorUserId, opt => opt.Ignore());

            CreateMap<BookmarkClassDto, BookmarkClass>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.IsDeleted, opt => opt.Ignore())
                .ForMember(x => x.DeletionTime, opt => opt.Ignore())
                .ForMember(x => x.DeleterUserId, opt => opt.Ignore())
                .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
                .ForMember(x => x.LastModifierUserId, opt => opt.Ignore())
                .ForMember(x => x.CreatorUserId, opt => opt.Ignore());
        }
    }
}
