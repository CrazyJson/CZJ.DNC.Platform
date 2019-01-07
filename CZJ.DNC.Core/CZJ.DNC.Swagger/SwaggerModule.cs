using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.Extensions.Configuration;
using CZJ.Common.Module;
using CZJ.Common.Core;
using CZJ.DNC.SwaggerExtend;
using System;

namespace CZJ.DNC.Swagger.Module
{
    /// <summary>
    /// Swagger API描述服务添加
    /// </summary>
    public class SwaggerModule : IServiceModule
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order => 70;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{SysConfig.MicroServiceOption.Title}接口描述文档V1");
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //swagger接口描述
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = $"{SysConfig.MicroServiceOption.Title}接口描述文档"
                });
                options.OperationFilter<SwaggerFileUploadFilter>();
                options.OperationFilter<AddAuthTokenHeaderParameter>();
                options.OperationFilter<CustomHeaderFilter>();

                string[] files = Directory.GetFiles(AppContext.BaseDirectory, "CZJ.*.xml");
                foreach (var path in files)
                {
                    options.IncludeXmlComments(path);
                }
                options.DocumentFilter<DocumentFilter>();
            });
        }
    }
}
