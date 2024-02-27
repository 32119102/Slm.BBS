using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Logging;
using Volo.Abp.Validation;

namespace ContIn.Abp.Terminal.Domain.Shared
{
    /// <summary>
    /// 自定义异常信息
    /// IHasErrorCode 接口来填充错误代码
    /// IHasErrorDetails 接口来填充错误详细信息
    /// IHasLogLevel 接口来指定日志的级别
    /// IHasValidationErrors 接口来填充验证错误信息
    /// IExceptionWithSelfLogging 接口来记录指定日志
    /// IUserFriendlyException 接口显示原 Message 和 Details 信息
    /// </summary>
    public class CustomException : Exception, IHasErrorCode, IHasErrorDetails, IHasLogLevel, IHasValidationErrors, IExceptionWithSelfLogging, IUserFriendlyException
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 错误详细信息
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 日志记录级别
        /// </summary>
        public LogLevel LogLevel { get; set; }
        /// <summary>
        /// 验证错误
        /// </summary>
        public IList<ValidationResult> ValidationErrors { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        /// <param name="validationResults"></param>
        /// <param name="innerException"></param>
        /// <param name="logLevel"></param>
        public CustomException(string errorCode, string message, string? details = null, IList<ValidationResult>? validationResults = null
            , Exception? innerException = null, LogLevel logLevel = LogLevel.Error) 
            : base(message, innerException)
        {
            Code = errorCode;
            Details = details ?? string.Empty;
            ValidationErrors = validationResults ?? new List<ValidationResult>();
            LogLevel = logLevel;
        }
        /// <summary>
        /// 设置异常参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public CustomException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
        /// <summary>
        /// 自定义日志
        /// </summary>
        /// <param name="logger"></param>
        public void Log(ILogger logger)
        {
            //...log additional info
            
        }
    }
}
