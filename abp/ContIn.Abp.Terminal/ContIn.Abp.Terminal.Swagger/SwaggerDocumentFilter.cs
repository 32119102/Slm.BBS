using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace ContIn.Abp.Terminal.Swagger
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var tags = new List<OpenApiTag>
            {
                new OpenApiTag {
                    Name = "Common",
                    Description = "公共分类接口",
                    ExternalDocs = new OpenApiExternalDocs { Description = "一些描述" }
                },
                new OpenApiTag {
                    Name = "Tag",
                    Description = "标签",
                    ExternalDocs = new OpenApiExternalDocs { Description = "标签相关接口" }
                },
                new OpenApiTag {
                    Name = "Authorize",
                    Description = "授权验证",
                    ExternalDocs = new OpenApiExternalDocs { Description = "授权验证相关接口" }
                },
                new OpenApiTag {
                    Name = "User",
                    Description = "用户信息",
                    ExternalDocs = new OpenApiExternalDocs { Description = "用户信息相关接口" }
                }
            };

            // 当前分组名称
            var groupName = context.ApiDescriptions.FirstOrDefault()?.GroupName;
            // 当前所有API对象
            var apis = context.ApiDescriptions.GetType().GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(context.ApiDescriptions) 
                as IEnumerable<ApiDescription>;
            // 属于当前分组的所有Controller
            var controllers = apis?.Where(x => x.GroupName == groupName).Select(x => ((ControllerActionDescriptor)x.ActionDescriptor).ControllerName).Distinct() 
                ?? new List<string>();

            swaggerDoc.Tags = tags.Where(x => controllers.Contains(x.Name)).OrderBy(x => x.Name).ToList();
        }
    }
}
