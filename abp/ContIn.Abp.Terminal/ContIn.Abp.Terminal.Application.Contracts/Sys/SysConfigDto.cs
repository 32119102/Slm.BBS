namespace ContIn.Abp.Terminal.Application.Contracts.Sys
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SysConfigDto
    {
        /// <summary>
        /// 站点标题
        /// </summary>
        public string? SiteTitle { get; set; }
        /// <summary>
        /// 站点描述
        /// </summary>
        public string? SiteDescription { get; set; }
        /// <summary>
        /// 站点关键字
        /// </summary>
        public List<string>? SiteKeywords { get; set; }
        /// <summary>
        /// 网站公告
        /// </summary>
        public string? SiteNotification { get; set; }
        /// <summary>
        /// 推荐标签
        /// </summary>
        public List<string>? RecommendTags { get; set; }
        /// <summary>
        /// 默认节点
        /// </summary>
        public long DefaultNodeId { get; set; }
        /// <summary>
        /// 登录方式
        /// </summary>
        public SysConfigLoginMethodDto? LoginMethod { get; set; }
        /// <summary>
        /// 站外链接跳转页面
        /// 在跳转前需手动确认是否前往该站外链接
        /// </summary>
        public bool UrlRedirect { get; set; }
        /// <summary>
        /// 导航配置
        /// </summary>
        public List<SysConfigSiteNavsDto>? SiteNavs { get; set; }
        /// <summary>
        /// 积分配置
        /// </summary>
        public SysConfigScoreConfigDto? ScoreConfig { get; set; }
        /// <summary>
        /// 发帖验证码
        /// 发帖时是否开启验证码校验
        /// </summary>
        public bool TopicCaptcha { get; set; }
        /// <summary>
        /// 邮箱验证后发帖
        /// 需要验证邮箱后才能发帖
        /// </summary>
        public bool CreateTopicEmailVerified { get; set; }
        /// <summary>
        /// 邮箱验证后发表文章
        /// 需要验证邮箱后才能发表文章
        /// </summary>
        public bool CreateArticleEmailVerified { get; set; }
        /// <summary>
        /// 邮箱验证后评论
        /// 需要验证邮箱后才能发表评论
        /// </summary>
        public bool CreateCommentEmailVerified { get; set; }
        /// <summary>
        /// 发表文章审核
        /// 发布文章后是否开启审核
        /// </summary>
        public bool ArticlePending { get; set; }
        /// <summary>
        /// 用户观察期(秒)
        /// 观察期内用户无法发表话题、动态等内容，设置为 0 表示无观察期。
        /// </summary>
        public int UserObserveSeconds { get; set; }
        /// <summary>
        /// 用户登录有效期（天）
        /// </summary>
        public int TokenExpireDays { get; set; }
    }
}
