using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using FilterAttribute = System.Web.Mvc.FilterAttribute;
using IActionFilter = System.Web.Mvc.IActionFilter;
using IExceptionFilter = System.Web.Mvc.IExceptionFilter;
using log4net;

namespace StackOverflow.Web.Controllers
{
    public class LoggingAttribute : FilterAttribute, IActionFilter, IResultFilter, IExceptionFilter
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LoggingAttribute));

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
           
        }

        public void OnException(ExceptionContext filterContext)
        {
            var message = filterContext.Exception.Message;
            Logger.Error(message);

            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string message = "Controller: " + filterContext.Controller;
            message += ". Action: " + filterContext.ActionDescriptor.ActionName;
            Logger.Info(message);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}