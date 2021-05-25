//using StackLogger.Logger.Mvc.ActorService;
using System;
using System.Threading.Tasks;

using System.Web.Mvc;
using System.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.DependencyInjection;

using StackLog.HttpModule.Configuration;
using Microsoft.Extensions.Options;

//using StackLog.HttpModule.Configuration;
//using Microsoft.Web.Administration;

namespace StackLog.Configuration
{
   
    public class StackLogExceptionErrors
    {
        public const string BUCKET_KEY_MISSING = "bucket key is missing....";
        public const string SECRET_KEY_MISSING = "secret key is missing";
        public const string INSTANCE_REGISTRATION_FAILED = "StackLogger instance failed to register";
        public const string NULL_OPTIONS = "configuration options is null, please check your settings"; 

        public const string KEY_MISSING = BUCKET_KEY_MISSING + " and/or " + SECRET_KEY_MISSING;

        public const string UNABLE_TO_CREATE_LOG =
            "unable to create log, please pass appropriate secret key and bucket key";

        public static string MakeError(string errorMessage)
        {
            return errorMessage;
        }
    }
    public static class StackLogExtensionPoints
    {
        public static IApplicationBuilder UseStackLog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<StackLogConfiguration.StackLogMiddleware>();
        }
        private static IStackLogOptions optionsForInjection { get; set; }
        static void SetStackLogOptions(IStackLogOptions options)
        {
            optionsForInjection = options;
        }

        public static IServiceCollection AddStackLog(this IServiceCollection service,
            Action<IStackLogOptions> options)
        {
            //options.
            IStackLogOptions opts = new StackLogOptions();
            // service.Configure(options);
            options(opts);
            // options?.Invoke(opts);
            // opts(SetStackLogOptions)
            // x=> {
            // IStackLogOptions stackLogOptions = new StackLogOptions();
            //return new StackLog(optionsForInjection);
        
            service.AddTransient<IStackLog, StackLog>(x=> {
                return new StackLog(opts);
            });
            //service.AddTransient<IStackLogOptions, StackLogOptions>(x=> { 
            //    return 
            //});
            
           // service.AddScoped<IStackLog, StackLog>();
            return service;
        }
    }
    
    //[DllImport("StackLog.HttpModule")]
    public class StackLogConfiguration
    {
        public static object ReadConfigSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static IStackLogOptions StaticSetter(IStackLogOptions options )
        {
            return options;
        }

        public static IStackLogOptions StaticSetter(StackLogOptions opts)
        {
            return new StackLogOptions()
            {
                bucketKey = opts.bucketKey,
                secretKey = opts.secretKey,
                enableCloudWatch = opts.enableCloudWatch,
                 enableConsoleLogging = opts.enableConsoleLogging,
                  enableFileLogging = opts.enableFileLogging,
                   errorViewName = opts.errorViewName,
                    filePath = opts.filePath
            };
        }
       
        
        public class StackLogMiddleware
        {
            private RequestDelegate _next;
            private IStackLogOptions options;
            private IStackLog _logger;

            private IStackLogOptions _options { get; set; }
            private void SetStckLogOptions(IStackLogOptions options)
            {
                
            }
            public StackLogMiddleware(RequestDelegate context)
            {
                this._next = context;
              //  this.options = options.Value;

               // this._logger = new StackLog(this.options);
                //StackLoggerMvc.getInstance(this.options.secretKey, this.options.bucketKey, this.options.enableCloudWatch);
            }

            public async Task InvokeAsync(HttpContext context)
            {
               IStackLog StackLogService = context.RequestServices.GetRequiredService<IStackLog>();
                this._logger = StackLogService;
                try
                {
                    //context.Items.
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
                
             //       throw new StackLogException("secret key is missing.....UNAUTHORIZED");
                
            
            }
        }
       

       
        
        public static void RegisterStackLog(GlobalFilterCollection filter, StackLogOptions options, dynamic context)
        {
            var instance = new StackLog(null);
            context.Items.Add("StackLogInstance", instance);
            filter.Add(new StackLogGlobalExceptionHandler());
            filter.Add(new StackLogWebMvcRequestInsight(instance));
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