using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace ContIn.Abp.Terminal.Application.Contracts
{
    /// <summary>
    /// 手动验证对象逻辑
    /// </summary>
    public class CustomValidationContributor : IObjectValidationContributor, ITransientDependency
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task AddErrorsAsync(ObjectValidationContext context)
        {
            //Get the validating object
            //var obj = context.ValidatingObject;

            //Add the validation errors if available
            //context.Errors.Add();

            return Task.CompletedTask;
        }
    }
}
