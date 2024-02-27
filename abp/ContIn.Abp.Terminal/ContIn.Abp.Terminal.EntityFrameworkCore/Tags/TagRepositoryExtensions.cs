using ContIn.Abp.Terminal.Domain.Tags;
using Volo.Abp.Threading;

namespace ContIn.Abp.Terminal.EntityFrameworkCore.Tags
{
    public static class TagRepositoryExtensions
    {
        /// <summary>
        /// 根据名称获取标签信息
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Tag? FindByTagName(this ITagRepository repository, string name)
        {
            return AsyncHelper.RunSync(
                () => repository.FindByTagNameAsync(name)
            );
        }
    }
}
