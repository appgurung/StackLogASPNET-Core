//using StackLogger.Logger.Mvc.ActorService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using System.Runtime.CompilerServices;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackLog.HttpModule.Configuration;
using System.Runtime.InteropServices;
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

        public static IServiceCollection AddStackLog(this IServiceCollection service,
            Action<StackLogOptions> options = default)
        {
            options = options ?? (opts => { });

            service.Configure(options);
            service.AddTransient<IStackLog, StackLog>();
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
        // mvc and web api filter here
        // .net core filter here

        //public class StackLogRequestInsight : System.Web.Mvc.ActionFilterAttribute
        //{
        //    private IStackLog instance;
        //    private StackLogResponse information; 
        //    public StackLogRequestInsight()
        //    {
        //        //instance =  HttpContext.Items["StackLoggerInstance"] as StackLoggerMvc;
        //        information = new StackLogResponse();
        //        information.startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
        //    }

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
           
        //    //if(filterContext.HttpContext.Response.Mo)
        //    //information.httpResponse = filterContext.HttpContext.Response.
        //    base.OnResultExecuted(filterContext);
        //}

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    var endDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
        //    information.endTime = endDate;
        //    //filterContext.HttpContext;
        //    information.statusCode = filterContext.HttpContext.Response.StatusCode;
        //    var requestIdPool = new Random();
        //    string id = "";
        //    for (var i = 0; i < 6; i++)
        //    {
        //        var next = Math.Floor(Convert.ToDecimal(requestIdPool.Next(1, 100)));
        //        id += next;

        //    }

        //    information.requestId = id;

        //    var viewResult = filterContext.Result as ViewResult;

        //    if (viewResult != null)
        //    {
        //        if (viewResult.Model != null)
        //        {
        //            information.httpResponse = JsonConvert.SerializeObject(viewResult.Model);
        //        }
        //        else
        //        {
        //            var newViewResult = filterContext.Result as JsonResult;

        //            if (newViewResult != null)
        //            {
        //                if (newViewResult.Data != null)
        //                {
        //                    information.httpResponse = JsonConvert.SerializeObject(newViewResult.Data);
        //                }
        //            }
        //        }

        //    }
        //    else
        //    {
        //        var newViewResult = filterContext.Result as JsonResult;

        //        if (newViewResult != null)
        //        {
        //            if (newViewResult.Data != null)
        //            {
        //                information.httpResponse = JsonConvert.SerializeObject(newViewResult.Data);
        //            }
        //        }
        //    }

        //    if (instance != null)
        //    {
        //        instance.LogCloudWatch(information);
        //    }
        //    else
        //    {
        //        string secretKeyCode = ReadConfigSettings("StackLogSecretKey").ToString();
        //        string bucketKeyCode = ReadConfigSettings("StackLogBucketKey").ToString();
        //        string enableCloudWatch = ReadConfigSettings("enableCloudWatch").ToString();
        //        string enableFileLog = ReadConfigSettings("enableFileLog").ToString();
        //        string enableConsoleLogging = ReadConfigSettings("enableConsoleLogging").ToString();
        //        //IConfigurationBuilder build = IConfigurationBuilder.

        //        bool isCloudWatchEnabled = false;
        //        bool isFileLogEnabled = false;
        //        bool isConsoleLogEnabled = false;
        //        if (enableCloudWatch != null || enableCloudWatch != "")
        //        {
        //            isCloudWatchEnabled = enableCloudWatch == "true" || enableCloudWatch == "TRUE" || enableCloudWatch == "True" ? true : false;
        //        }
                
        //        if (enableFileLog != null || enableFileLog != "")
        //        {
        //            isFileLogEnabled = enableFileLog == "true" || enableFileLog == "TRUE" || enableFileLog == "True" ? true : false;
        //        }
                
        //        if (enableConsoleLogging != null || enableConsoleLogging != "")
        //        {
        //            isConsoleLogEnabled = enableFileLog == "true" || enableFileLog == "TRUE" || enableFileLog == "True" ? true : false;
        //        }
                
               
        //        instance = new StackLog(new StackLogOptions()
        //        {
        //            secretKey = secretKeyCode,
        //            bucketKey = bucketKeyCode,
        //            enableCloudWatch = isCloudWatchEnabled,
        //            enableFileLogging = isFileLogEnabled,
        //            enableConsoleLogging = isConsoleLogEnabled
        //        });
                
        //        instance.LogCloudWatch(information);
        //    }
        //    base.OnResultExecuting(filterContext);
        //}

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    var responseStream = filterContext.Controller.ControllerContext.HttpContext.Response.OutputStream;
        //    base.OnActionExecuted(filterContext);
        //}

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
            
           
        //    instance = filterContext.HttpContext.Items["StackLogInstance"] as StackLog;
        //    if (instance == null)
        //    {
        //        throw new StackLogException(StackLogExceptionErrors.INSTANCE_REGISTRATION_FAILED);
        //    }


        //    var uri = new Uri(filterContext.HttpContext.Request.Url.AbsoluteUri);
        //    information.httpProtocol = uri.Scheme;
        //    information.header = filterContext.HttpContext.Request.Headers.ToString();
        //    information.userAgent = filterContext.HttpContext.Request.Headers["User-Agent"].ToString();
        //    information.controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        //    information.isRequest = true;
        //    information.controllerMethod = filterContext.ActionDescriptor.ActionName;
        //    information.httpVerb = filterContext.HttpContext.Request.HttpMethod;
        //    information.url = filterContext.HttpContext.Request.RawUrl;
        //   // information.queryString = filterContext.HttpContext.Request.QueryString.ToString();
        //    information.remoteIpAddress = filterContext.HttpContext.Request.UserHostAddress;
        //    information.executedSqlQueries = "";

        //    var serverVaraibles = filterContext.HttpContext.Request.ServerVariables;
        //    var attributes = filterContext.ActionDescriptor.GetCustomAttributes(true);

        //  //  var protocol = new Uri(filterContext.HttpContext.Request.RawUrl);
        //    //information.httpProtocol = protocol.ToString();
        //    StringBuilder methodAttributeInfo = new StringBuilder("{");

        //    var currentActionMethodFilterAttributes = filterContext.ActionDescriptor.GetFilterAttributes(true);

        //    foreach(var current in currentActionMethodFilterAttributes)
        //    {
        //        methodAttributeInfo.Append(current.GetType().Name + ", "); 
        //    }

        //    foreach (var currentAttribute in attributes)
        //    {
        //        methodAttributeInfo.Append(currentAttribute.GetType().FullName + ", ");
        //    }

        //    methodAttributeInfo.Append("}");
        //    information.methodAttributes = methodAttributeInfo.ToString();
        //    StringBuilder paramInfo = new StringBuilder("{");

        //    foreach(var p in filterContext.ActionParameters)
        //    {
                
        //        paramInfo.Append($"Name:{p.Key}, Value:{p.Value}");
        //    }
        //    paramInfo.Append("}");
        //    information.queryString = paramInfo.ToString();
        //    information.requestParam = filterContext.HttpContext?.Request.BodyToString();
        //    //var param = filterContext.HttpContext.Request.R
            
        //    base.OnActionExecuting(filterContext);
        //    //filterContext.
        //    }
        //}
        
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
       

       
        
        public static void RegisterStackLog(GlobalFilterCollection filter, dynamic context)
        {
            var instance = new StackLog();
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