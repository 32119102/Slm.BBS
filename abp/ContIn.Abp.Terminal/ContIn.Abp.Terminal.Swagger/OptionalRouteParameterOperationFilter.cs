using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.RegularExpressions;

namespace ContIn.Abp.Terminal.Swagger
{
    /// <summary>
    /// 可选参数
    /// </summary>
    public class OptionalRouteParameterOperationFilter : IOperationFilter
    {
        private const string captureName = "routeParameter";
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var httpMethodAttributes = context.MethodInfo.GetCustomAttributes(true).OfType<HttpMethodAttribute>();

            var httpMethodWithOptional = httpMethodAttributes?.FirstOrDefault(x => x.Template?.Contains("?") ?? false);
            if (httpMethodWithOptional == null)
                return;

            var regex = $"{{(?<{captureName}>\\w+)\\?}}";
            var matches = Regex.Matches(httpMethodWithOptional.Template!, regex);
            foreach (Match match in matches)
            {
                var name = match.Groups[captureName].Value;

                var parameter = operation.Parameters.FirstOrDefault(p => p.In == ParameterLocation.Path && p.Name == name);
                if (parameter != null)
                {
                    parameter.AllowEmptyValue = true;
                    parameter.Required = false;
                    parameter.Schema.Nullable = true;
                    parameter.Description = "";
                    //parameter.Schema.Default = new OpenApiString(string.Empty);
                }
            }
        }
    }
}
