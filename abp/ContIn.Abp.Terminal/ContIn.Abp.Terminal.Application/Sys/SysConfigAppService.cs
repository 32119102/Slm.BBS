using ContIn.Abp.Terminal.Application.Contracts.Sys;
using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Sys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Json;

namespace ContIn.Abp.Terminal.Application.Sys
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.Common)]
    public class SysConfigAppService : ApplicationService, ISysConfigAppService
    {
        private readonly ISysConfigRepository _repository;
        private readonly IDistributedCache<SysConfigDto> _cache;
        private readonly IJsonSerializer _jsonSerializer;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="cache"></param>
        /// <param name="jsonSerializer"></param>
        public SysConfigAppService(ISysConfigRepository repository, IDistributedCache<SysConfigDto> cache, IJsonSerializer jsonSerializer)
        { 
            _repository = repository;
            _cache = cache;
            _jsonSerializer = jsonSerializer;
        }
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<SysConfigDto> GetAllAsync()
        {
            return await _cache.GetOrAddAsync("all",
                async () =>
                {
                    var configs = await _repository.GetListAsync();
                    SysConfigDto config = new SysConfigDto();

                    config.SiteTitle = configs.Find(x => x.Key == nameof(config.SiteTitle).FirstCharToLower())?.Value;
                    config.SiteDescription = configs.Find(x => x.Key == nameof(config.SiteDescription).FirstCharToLower())?.Value;
                    config.SiteKeywords = _jsonSerializer.Deserialize<List<string>>(configs.Find(x => x.Key == nameof(config.SiteKeywords).FirstCharToLower())?.Value);
                    config.SiteNotification = configs.Find(x => x.Key == nameof(config.SiteNotification).FirstCharToLower())?.Value;
                    config.RecommendTags = _jsonSerializer.Deserialize<List<string>>(configs.Find(x => x.Key == nameof(config.RecommendTags).FirstCharToLower())?.Value);
                    config.DefaultNodeId = configs.Find(x => x.Key == nameof(config.DefaultNodeId).FirstCharToLower())?.Value?.To<long>() ?? 0;
                    config.LoginMethod = _jsonSerializer.Deserialize<SysConfigLoginMethodDto>(configs.Find(x => x.Key == nameof(config.LoginMethod).FirstCharToLower())?.Value);
                    config.UrlRedirect = configs.Find(x => x.Key == nameof(config.UrlRedirect).FirstCharToLower())?.Value?.To<bool>() ?? false;
                    config.SiteNavs = _jsonSerializer.Deserialize<List<SysConfigSiteNavsDto>>(configs.Find(x => x.Key == nameof(config.SiteNavs).FirstCharToLower())?.Value);
                    config.ScoreConfig = _jsonSerializer.Deserialize<SysConfigScoreConfigDto>(configs.Find(x => x.Key == nameof(config.ScoreConfig).FirstCharToLower())?.Value);
                    config.TopicCaptcha = configs.Find(x => x.Key == nameof(config.TopicCaptcha).FirstCharToLower())?.Value?.To<bool>() ?? false;
                    config.CreateTopicEmailVerified = configs.Find(x => x.Key == nameof(config.CreateTopicEmailVerified).FirstCharToLower())?.Value?.To<bool>() ?? false;
                    config.CreateArticleEmailVerified = configs.Find(x => x.Key == nameof(config.CreateArticleEmailVerified).FirstCharToLower())?.Value?.To<bool>() ?? false;
                    config.CreateCommentEmailVerified = configs.Find(x => x.Key == nameof(config.CreateCommentEmailVerified).FirstCharToLower())?.Value?.To<bool>() ?? false;
                    config.ArticlePending = configs.Find(x => x.Key == nameof(config.ArticlePending).FirstCharToLower())?.Value?.To<bool>() ?? false;
                    config.UserObserveSeconds = configs.Find(x => x.Key == nameof(config.UserObserveSeconds).FirstCharToLower())?.Value?.To<int>() ?? 0;
                    config.TokenExpireDays = configs.Find(x => x.Key == nameof(config.TokenExpireDays).FirstCharToLower())?.Value?.To<int>() ?? 7;

                    return config;
                });
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task PostSaveAsync(SysConfigDto input)
        {
            var properties = input.GetType().GetProperties();
            foreach (var property in properties)
            {
                var pt = property.PropertyType;
                string? v;
                if (pt == typeof(string) || pt == typeof(int) || pt == typeof(long) || pt == typeof(bool))
                {
                    v = property.GetValue(input, null)?.ToString();
                }
                else
                {
                    v = _jsonSerializer.Serialize(property.GetValue(input, null));
                }

                var config = await _repository.GetByKeyAsync(property.Name.FirstCharToLower());
                if (config != null)
                {
                    config.Value = v;
                    config.UpdateTime = Clock.Now.ToMillisecondsTimestamp();
                    await _repository.UpdateAsync(config);
                    // 移除缓存
                    await _cache.RemoveAsync("all");
                }
            }
        }
    }
}
