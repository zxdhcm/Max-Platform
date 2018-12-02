using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Max.Platform.Account.Bookmarks;
using Max.Platform.Bookmarks.Dto;

namespace Max.Platform.Bookmarks
{
    public class BookmarkAppService : AsyncCrudAppService<Bookmark, BookmarkDto, int, BookmarkGetAllInput, CreateBookmarkDto, BookmarkDto>, IBookmarkAppService
    {
        private readonly IRepository<BookmarkChannel> _channelRepository;
        public BookmarkAppService(IRepository<Bookmark> repository, IRepository<BookmarkChannel> channelRepository) : base(repository)
        {
            _channelRepository = channelRepository;
        }

        //public int ReName()
        //{
        //   var directoryinfo = new System.IO.DirectoryInfo(@"D:\DotNetProjects\Asp.Net\Max.Platform\4.2.0\ant-design-pro\public\avator2");
        //    var i = 1;
        //    //读取当前目录文件信息
        //    foreach (var item in directoryinfo.GetFiles())
        //    {
        //        var destPath =System.IO.Path.Combine(@"D:\DotNetProjects\Asp.Net\Max.Platform\4.2.0\ant-design-pro\public\avator", "HeadPortrait" + i + ".jpg");
        //        item.MoveTo(destPath);
        //        i++;
        //    }

        //    return i;
        //}

        #region 创建书签
        /// <summary>
        /// 创建书签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<BookmarkDto> Create(CreateBookmarkDto input)
        {
            CheckCreatePermission();

            var bookmark = MapToEntity(input);
            bookmark.BookmarkChannel =
                await _channelRepository.FirstOrDefaultAsync(c => c.Id == input.BookmarkChannelId);
            await Repository.InsertAsync(bookmark);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(bookmark);
        }
        #endregion

        #region 更新书签
        /// <summary>
        /// 更新书签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<BookmarkDto> Update(BookmarkDto input)
        {
            CheckUpdatePermission();

            var bookmark = await GetEntityByIdAsync(input.Id);
            MapToEntity(input, bookmark);
            bookmark.BookmarkChannel =
                await _channelRepository.FirstOrDefaultAsync(c => c.Id == input.BookmarkChannelId);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(bookmark);
        }
        #endregion

        #region 获取书签网站相关信息
        /// <summary>
        /// 获取书签网站相关信息 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<WebSiteInfoDto> PostWebSiteInfo(BookmarkGetFaviconInput input)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    var results = await httpClient.GetStringAsync(input.Url);
                    return new WebSiteInfoDto()
                    {
                        FaviconUrl = GetFaviconUrl(input.Url, results),
                        Title = GetTitle(results),
                        Description = GetDescription(results)
                    };
                }
                catch (Exception e)
                {
                  return new WebSiteInfoDto() { FaviconUrl = "", Title = "", Description = "" };
                }
            }
        }
        #endregion

        protected override IQueryable<Bookmark> CreateFilteredQuery(BookmarkGetAllInput input)
        {
            return base.CreateFilteredQuery(input)
                .WhereIf(input.ChannelId.HasValue && input.ChannelId > 0, t => t.BookmarkChannel.Id == input.ChannelId.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(input.KeyWord), t => t.Url.Contains(input.KeyWord) || t.Remarks.Contains(input.KeyWord) || t.Title.Contains(input.KeyWord))
                .WhereIf(AbpSession.UserId.HasValue,t=>t.CreatorUserId==AbpSession.UserId);
        }

        #region 获取网站图标地址
        /// <summary>
        /// 获取网站图标地址
        /// </summary>
        /// <param name="url"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        private string GetFaviconUrl(string url, string html)
        {
            var faviconUrl = string.Empty;
            var match = Regex.Matches(html, "<link.*?>").FirstOrDefault(m => m.Groups[0].Value.Contains("ico") || m.Groups[0].Value.Contains("Ico"));
            if (match != null)
            {
                var iconMatch = Regex.Matches(match.Groups[0].Value, "href=\"(.*?)\"").FirstOrDefault();
                if (iconMatch != null)
                {
                    faviconUrl = iconMatch.Groups[1].Value;
                }
            }

            var host = new Uri(url).Host;
            host = host.StartsWith("https:") ? $"https://{host}" : $"http://{host}";
            if (string.IsNullOrWhiteSpace(faviconUrl))
            {
                faviconUrl = $"{host}/favicon.ico";
            }
            else if (!faviconUrl.Trim().Replace("http:", "").Replace("https:", "").StartsWith("//"))
            {
                faviconUrl = $"{host}/{faviconUrl}";
            }
            return faviconUrl;
        }
        #endregion

        #region 获取网站标题
        /// <summary>
        /// 获取网站标题
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string GetTitle(string html)
        {
            var title = "我是一个书签名";
            var match = Regex.Matches(html, "<title>([\\S\\s]*?)</title>").FirstOrDefault();
            if (match != null)
            {
                title = match.Groups[1].Value;
            }
            return title;
        }
        #endregion

        #region 获取网站相关描述
        /// <summary>
        /// 获取网站相关描述
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string GetDescription(string html)
        {
            var description = string.Empty;
            var match = Regex.Matches(html, "<meta.*?content=\"(.*?)\".*?>")
                .FirstOrDefault(m => m.Groups[0].Value.Contains("name=\"description\"") || m.Groups[0].Value.Contains("name=\"Description\"") || m.Groups[0].Value.Contains("property=\"og: description\"") || m.Groups[0].Value.Contains("property=\"og: Description\""));
            if (match != null)
            {
                description = match.Groups[1].Value;
            }
            return description;
        } 
        #endregion
    }
}
