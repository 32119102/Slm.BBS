namespace ContIn.Abp.Terminal.Domain.Shared
{
    /// <summary>
    /// 定义异常代码
    /// 建议使用下面的错误代码格式:<code-namespace>:<error-code>
    /// </summary>
    public class TerminalErrorCodes
    {
        /* You can add your business exception error codes here, as constants */

        public const string CanNotVoteYourOwnAnswer = "Terminal:01001";

        // 实体没有找到
        public const string EntityNotFound = "Entity.NotFound:10001";
        // 实体已存在
        public const string EntityAlreadyExists = "Entity.HasExists:10002";
        // 参数为空
        public const string ArgumentIsEmpty = "Argument.Empty:10100";
        // 通用错误
        public const string CommonErrorCode = "Terminal.Common:10200";
        // 未登录
        public const string UserNotLogin = "Terminal.NotLogin:10300";

        // GitHub授权code为空
        public const string AuthorizeCodeIsEmpty = "Auth:20001";
        // GitHub授权获取AccessToken时错误
        public const string AuthorizeAccessTokenException = "Auth:20002";
        // GitHub授权 AccessToken为空
        public const string AuthorizeAccessTokenIsEmpty = "Auth:20003";
        // Github授权获取用户信息时错误
        public const string AuthorizeGetUserInfoException = "Auth:20004";
        // 用户不存在或被禁用
        public const string AuthorizeUserNotExistsOrForbidden = "Auth.NotExistsOrForbidden:20005";
        // 密码错误
        public const string AuthorizeUserPwdFail = "Auth.PasswordWrong:20006";

        // token失效，请重新登录
        public const string AuthorizeJwtTokenInvalid = "Auth.JwtToken.Invalid:20100";

        // 验证码过期
        public const string CaptchaOutTime = "Captcha:20101";
        // 验证码错误
        public const string CaptchaNotEqual = "Captcha:20102";

        // 文件大小超过限制
        public const string RequestSizeLimit = "Request.Size.Limit:30001";
    }
}
