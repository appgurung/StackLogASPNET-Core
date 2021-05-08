using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Newtonsoft.Json;


using StackLog.WebMVC.HttpModule.Configuration;

namespace StackLog.WebMVC.HttpModule
{
    public static class StackLogWebMvcExtensionPoints
    {
        public static string BodyToString(this HttpRequestBase request)
        {
            using (var body = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(body))
                {
                    var readToEnd = reader.ReadToEnd();

                    return readToEnd ?? "";
                }
            }
            //using (var reader = new System.IO.StreamReader(request.Body))
            //{
            //    return reader.ReadToEnd();
            //}
        }
    }

    public class StackLogWebRequestConfiguration
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
        public static object ReadWebConfigSettings(string key)
        {
            
            //WebConfigurationManage
            return WebConfigurationManager.AppSettings[key];
        }
    }
    public class StackLogWebMvcRequestInsight : ActionFilterAttribute
    {
        
            dynamic instance;
            private StackLogResponse information; 
            public StackLogWebMvcRequestInsight(dynamic _instance)
            {
                
                //instance =  HttpContext.Items["StackLoggerInstance"] as StackLoggerMvc;
                information = new StackLogResponse();
                instance = _instance;
                information.startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            }

            public override void OnResultExecuted(ResultExecutedContext filterContext)
            {
               
                //if(filterContext.HttpContext.Response.Mo)
                //information.httpResponse = filterContext.HttpContext.Response.
                base.OnResultExecuted(filterContext);
            }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var endDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            information.endTime = endDate;
            //filterContext.HttpContext;
            information.statusCode = filterContext.HttpContext.Response.StatusCode;
            var requestIdPool = new Random();
            string id = "";
            for (var i = 0; i < 6; i++)
            {
                var next = Math.Floor(Convert.ToDecimal(requestIdPool.Next(1, 100)));
                id += next;

            }

            information.requestId = id;

            var viewResult = filterContext.Result as ViewResult;

            if (viewResult != null)
            {
                if (viewResult.Model != null)
                {
                    information.httpResponse = JsonConvert.SerializeObject(viewResult.Model);
                }
                else
                {
                    var newViewResult = filterContext.Result as JsonResult;

                    if (newViewResult != null)
                    {
                        if (newViewResult.Data != null)
                        {
                            information.httpResponse = JsonConvert.SerializeObject(newViewResult.Data);
                        }
                    }
                }

            }
            else
            {
                var newViewResult = filterContext.Result as JsonResult;

                if (newViewResult != null)
                {
                    if (newViewResult.Data != null)
                    {
                        information.httpResponse = JsonConvert.SerializeObject(newViewResult.Data);
                    }
                }
            }

            if (instance != null)
            {
                instance.LogCloudWatch(information);
            }
           
            base.OnResultExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var responseStream = filterContext.Controller.ControllerContext.HttpContext.Response.OutputStream;
            base.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
           
            instance = filterContext.HttpContext.Items["StackLogInstance"];
            if (instance == null)
            {
                throw new StackLogException(StackLogWebRequestConfiguration.StackLogExceptionErrors.INSTANCE_REGISTRATION_FAILED);
            }


            var uri = new Uri(filterContext.HttpContext.Request.Url.AbsoluteUri);
            information.httpProtocol = uri.Scheme;
            information.header = filterContext.HttpContext.Request.Headers.ToString();
            information.userAgent = filterContext.HttpContext.Request.Headers["User-Agent"].ToString();
            information.controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            information.isRequest = true;
            information.controllerMethod = filterContext.ActionDescriptor.ActionName;
            information.httpVerb = filterContext.HttpContext.Request.HttpMethod;
            information.url = filterContext.HttpContext.Request.RawUrl;
           // information.queryString = filterContext.HttpContext.Request.QueryString.ToString();
            information.remoteIpAddress = filterContext.HttpContext.Request.UserHostAddress;
            information.executedSqlQueries = "";

            var serverVaraibles = filterContext.HttpContext.Request.ServerVariables;
            var attributes = filterContext.ActionDescriptor.GetCustomAttributes(true);

          //  var protocol = new Uri(filterContext.HttpContext.Request.RawUrl);
            //information.httpProtocol = protocol.ToString();
            StringBuilder methodAttributeInfo = new StringBuilder("{");

            var currentActionMethodFilterAttributes = filterContext.ActionDescriptor.GetFilterAttributes(true);

            foreach(var current in currentActionMethodFilterAttributes)
            {
                methodAttributeInfo.Append(current.GetType().Name + ", "); 
            }

            foreach (var currentAttribute in attributes)
            {
                methodAttributeInfo.Append(currentAttribute.GetType().FullName + ", ");
            }

            methodAttributeInfo.Append("}");
            information.methodAttributes = methodAttributeInfo.ToString();
            StringBuilder paramInfo = new StringBuilder("{");

            foreach(var p in filterContext.ActionParameters)
            {
                
                paramInfo.Append($"Name:{p.Key}, Value:{p.Value}");
            }
            paramInfo.Append("}");
            information.queryString = paramInfo.ToString();
            information.requestParam = filterContext.HttpContext?.Request.BodyToString();
            //var param = filterContext.HttpContext.Request.R
            
            base.OnActionExecuting(filterContext);
            //filterContext.
        }
    }
    
    
}