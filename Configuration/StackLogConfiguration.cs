using StackLogger.Logger.Mvc.ActorService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
//using System.Web.Configuration;
using System.Runtime.CompilerServices;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace StackLog.Configuration
{
    public static class StackLogExtensionPoints
    {
        public static IApplicationBuilder UseStackLog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<StackLogConfiguration.StackLogMiddleware>();
        }

        public static IServiceCollection AddStackLog(this IServiceCollection service,
            Action<StackLogOptions> options = default)
        {
            options = options ?? (opts => { });

            service.Configure(options);
            service.AddTransient<IStackLog, StackLog>();
            return service;
        }
    }
    public class StackLogConfiguration
    {
        
        // mvc and web api filter here
        // .net core filter here

        
        public class StackLogMiddleware
        {
            private RequestDelegate _next;
            private StackLogOptions options;
            private IStackLog _logger;
        
            public StackLogMiddleware(RequestDelegate context, IOptions<StackLogOptions> options)
            {
                this._next = context;
                this.options = options.Value;
                this._logger = new StackLog(this.options);
                //StackLoggerMvc.getInstance(this.options.secretKey, this.options.bucketKey, this.options.enableCloudWatch);


            }

            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                
                    await _next(context);
                }
                catch(Exception es)
                {
                    await HandleExceptionAsync(context, es);
                }
            }

            private async Task HandleExceptionAsync(HttpContext context,Exception es)
            {
                if(options.secretKey != null || options.secretKey != "") 
                {
                    if(options.bucketKey != null || options.bucketKey != "")
                    {
                        await _logger.LogFatal(es);
                        return;
                    }
                   
                    throw new StackLogException("bucket key is missing.....UNAUTHORIZED");
                    
                }
                
                    throw new StackLogException("secret key is missing.....UNAUTHORIZED");
                
            
            }
        }
        public class Initialize
        {
        }
        public static void RegisterStackLog(GlobalFilterCollection filter)
        {
            filter.Add(new StackLogConfiguration.Initialize());
        }
        
    }

    public class AllowedFramework
    {
        public const string NETCORE = "__NETCORE__";
        public const string ASPNET_WEBAPI = "__ASPNET_WEBAPI__";
        public const string ASPNET_MVC = "__ASPNET_MVC__";
        public const string XAMARIN_FORMS = "__XAMARIN_FORM__";
    }
}