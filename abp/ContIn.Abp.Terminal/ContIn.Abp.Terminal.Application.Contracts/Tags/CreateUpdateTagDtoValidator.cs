using FluentValidation;

namespace ContIn.Abp.Terminal.Application.Contracts.Tags
{
    /// <summary>
    /// 标签验证器
    /// FluentValidation
    /// </summary>
    public class CreateUpdateTagDtoValidator : AbstractValidator<CreateUpdateTagDto>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CreateUpdateTagDtoValidator()
        {
            // RuleFor(x => x.Name).NotEmpty().Length(2, 20);
        }
    }
}
