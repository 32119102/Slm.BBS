using ContIn.Abp.Terminal.Domain.Tags;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Tags
{
    /// <summary>
    /// 标签仓储接口实现
    /// </summary>
    public class TagRepository : EfCoreRepository<TerminalDbContext, Tag, long>, ITagRepository
    {
        public TagRepository(IDbContextProvider<TerminalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 根据名称获取标签
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Tag?> FindByTagNameAsync(string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(x => x.Name == name);
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(string? name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(
                    !name.IsNullOrWhiteSpace(),
                    tag => tag.Name!.Contains(name!)
                )
                .CountAsync();
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<Tag>> GetListAsync(int skipCount, int maxResultCount, string? name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(
                    !name.IsNullOrWhiteSpace(),
                    tag => tag.Name!.Contains(name!)
                )
                .OrderByDescending(x => x.Id)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
        /// <summary>
        /// 保存标签
        /// </summary>
        /// <param name="tagNames"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public async Task<List<Tag>> GetOrCreateAsync(string[] tagNames, long time)
        {
            if (tagNames.Length == 0)
            {
                return new List<Tag>();
            }
            List<Tag> tags = new List<Tag>();
            foreach (var tagName in tagNames)
            {
                if (tagName.IsNullOrWhiteSpace())
                {
                    continue;
                }
                var tag = await FindByTagNameAsync(tagName);
                if (tag != null)
                {
                    tags.Add(tag);
                    continue;
                }
                tag = await InsertAsync(new Tag()
                {
                    Name = tagName,
                    Description = tagName,
                    Status = Domain.Shared.Enum.StatusEnum.Normal,
                    CreateTime = time,
                    UpdateTime = time,
                }, true);
                tags.Add(tag);
            }
            return tags;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<(int, List<Tag>)> GetPagingAsync(int page = 1, int limit = 100)
        {
            var dbSet = await GetDbSetAsync();
            var count = await dbSet.Where(x => x.Status == Domain.Shared.Enum.StatusEnum.Normal).CountAsync();
            var list = await dbSet.Where(x => x.Status == Domain.Shared.Enum.StatusEnum.Normal)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
            return (count, list);
        }
    }
}
