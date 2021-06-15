using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using StackLog.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackLog
{
    public class StackLogCloudWatch : Attribute, IActionFilter, IResourceFilter
    {
        private IStackLog instance;
        private StackLogResponse information;
        public StackLogCloudWatch(IStackLog instance)
        {
            //instance =  HttpContext.Items["StackLoggerInstance"] as StackLoggerMvc;
            information = new StackLogResponse();
            information.startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            this.instance = instance;
        }



        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var endDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            information.endTime = endDate;
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
                        if (newViewResult.Value != null)
                        {
                            information.httpResponse = JsonConvert.SerializeObject(newViewResult.Value);
                        }
                    }
                }

            }
            else
            {
                dynamic newViewResult = filterContext.Result;

                if (newViewResult != null)
                {
                    if (newViewResult.Value != null)
                    {
                        information.httpResponse = JsonConvert.SerializeObject(newViewResult.Value);
                    }
                }
            }

            

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {


            //instance = filterContext.HttpContext.Items["StackLoggerInstance"] as StackLoggerMvc;
            //if (instance == null)
            //{
            //    throw new StackLoggerException("StackLogger instance failed to register");
            //}

            var ctx = filterContext.HttpContext;
           // var uri = new Uri(filterContext.HttpContext.Request.Path.Value);
            information.httpProtocol = ctx.Request.Protocol;
            information.header = filterContext.HttpContext.Request.Headers.ToString();
            information.userAgent = filterContext.HttpContext.Request.Headers["User-Agent"].ToString();
            information.controllerName = filterContext.Controller.ToString();
            information.isRequest = true;
            information.controllerMethod = filterContext.ActionDescriptor.DisplayName;
            information.httpVerb = filterContext.HttpContext.Request.Method;
            information.url = ctx.Request.Path.Value;
            information.queryString = filterContext.HttpContext.Request.QueryString.ToString();
            information.remoteIpAddress = filterContext.HttpContext.Request.Scheme;
            information.executedSqlQueries = "a";

            StringBuilder paramInfo = new StringBuilder("{");


            var controllerDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;

            if (controllerDescriptor != null)
            {
                var parameters = controllerDescriptor.MethodInfo.GetParameters();

                foreach (var k in parameters)
                {
                    filterContext.ActionArguments.TryGetValue(k.Name, out var result);
                    paramInfo.Append($"ParamName:{k.Name}, ParamValue:{result}");
                }
            }


            paramInfo.Append("}");
            information.requestParam = paramInfo.ToString();

            //base.OnActionExecuting(filterContext);
            //filterContext.
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if (instance != null)
            {
                //if(instance.)
                instance.LogCloudWatch(information);
            }
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var ctx = context;


            var viewResult = ctx.Result as ViewResult;

            if (viewResult != null)
            {
                if (viewResult.Model != null)
                {
                    information.httpResponse = JsonConvert.SerializeObject(viewResult.Model);
                }
                else
                {
                    var newViewResult = ctx.Result as JsonResult;

                    if (newViewResult != null)
                    {
                        if (newViewResult.Value != null)
                        {
                            information.httpResponse = JsonConvert.SerializeObject(newViewResult.Value);
                        }
                    }
                }

            }
            else
            {
                var newViewResult = ctx.Result as JsonResult;

                if (newViewResult != null)
                {
                    if (newViewResult.Value != null)
                    {
                        information.httpResponse = JsonConvert.SerializeObject(newViewResult.Value);
                    }
                }
            }

        }
    }
}
