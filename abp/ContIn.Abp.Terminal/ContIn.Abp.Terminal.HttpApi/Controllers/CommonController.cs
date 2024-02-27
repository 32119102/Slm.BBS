using ContIn.Abp.Terminal.Application.Contracts;
using ContIn.Abp.Terminal.Core;
using ContIn.Abp.Terminal.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Json;

namespace ContIn.Abp.Terminal.HttpApi.Controllers
{
    /// <summary>
    /// 公共控制器
    /// </summary>
    [Route("api/common")]
    [ApiExplorerSettings(GroupName = SwaggerGroupName.Common)]
    public class CommonController : AbpController
    {
        private readonly IConfigurationRoot _configRoot;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger _logger;
        private readonly IAuditingHelper _auditingHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="jsonSerializer"></param>
        /// <param name="logger"></param>
        /// <param name="auditingHelper"></param>
        public CommonController(IConfiguration configuration
            , IJsonSerializer jsonSerializer
            , ILogger<CommonController> logger, IAuditingHelper auditingHelper)
        {
            _configRoot = (IConfigurationRoot)configuration;
            _jsonSerializer = jsonSerializer;
            _logger = logger;
            _auditingHelper = auditingHelper;
        }

        /// <summary>
        /// 审计信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("auditing")]
        public string GetAuditingAsync()
        {
            var auditInfo = _auditingHelper.CreateAuditLogInfo();
            return _jsonSerializer.Serialize(auditInfo);
        }

        /// <summary>
        /// 系统信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("systeminfo")]
        public async Task<SystemInfoDto> GetSystemInfoAsync()
        {
            var appEnv = PlatformServices.Default.Application;
            SystemInfoDto systemInfoDto = new SystemInfoDto()
            { 
                MachineName = Environment.MachineName,
                Host = Request.Host.Host,
                Port = Request.Host.Port?.ToString() ?? string.Empty,
                RuntimeFramework = appEnv.RuntimeFramework.FullName,
                ApplicationName = appEnv.ApplicationName,
                ApplicationVersion = appEnv.ApplicationVersion,
                OsVersion = Environment.OSVersion.ToString(),
                Arch = RuntimeInformation.OSArchitecture.ToString(),
                ApplicationBasePath = appEnv.ApplicationBasePath,
                ProcessorCount = Environment.ProcessorCount.ToString()
            };
            return await Task.FromResult(systemInfoDto);
        }


        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("ntime")]
        public string GetClockTime()
        {
            var now = Clock.Now;
            return now.ToString();
        }

        /// <summary>
        /// 获取配置提供程序
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("crps")]
        public ContentResult GetConfigRootProviders()
        {
            string str = "";
            foreach (var provider in _configRoot.Providers.ToList())
            {
                str += provider.ToString() + "\n";
            }
            str += "\n";

            var sections = _configRoot.AsEnumerable();
            str += _jsonSerializer.Serialize(sections);

            str += "\n";

            var t = Base64UrlEncoder.Decode(@"eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhQGV4YW1wbGUuY29tIiwiZXhwIjoxNjUxMDY4MzM0LCJuYmYiOiItODU4NTUwNTM4NjExNDMzNzk3NiIsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjUxMjgiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MTI4In0");
            str += t + "\n";
            
            return Content(str);
        }

        /// <summary>
        /// 触发异常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("texp")]
        public string TriggerException(string? t = null)
        {
            if (t == null)
            {
                throw new CustomException(TerminalErrorCodes.CanNotVoteYourOwnAnswer, "msg", "detail",
                    new List<ValidationResult>()
                    {
                        new ValidationResult("name is null",new string[]{ "name" })
                    }).WithData("a", "a");
            }
            var a = 1;
            var b = 0;
            var c = a / b;
            return c.ToString();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("log")]
        public string LoggerTest()
        {
            _logger.LogDebug("测试记录Debug日志");
            _logger.LogInformation("测试记录Information日志");
            _logger.LogWarning("测试记录Warning日志");
            _logger.LogError("测试记录Error日志");
            return "0";
        }
    }
}