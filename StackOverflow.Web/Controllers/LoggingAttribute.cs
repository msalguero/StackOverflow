using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using FilterAttribute = System.Web.Http.Filters.FilterAttribute;
using IActionFilter = System.Web.Mvc.IActionFilter;
using log4net;

namespace StackOverflow.Web.Controllers
{
    public class LoggingAttribute : FilterAttribute, IActionFilter, IResultFilter, IExceptionFilter
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LoggingAttribute));

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string message = "Controller: " + filterContext.Controller;
            logger.Info(message);
        }

        public void OnException(ExceptionContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string message = "Controller: " + filterContext.Controller;
            message += ". Action: " + filterContext.ActionDescriptor.ActionName;
            logger.Info(message);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string message = "Controller: " + filterContext.Controller;
            message += ". Action: " + filterContext.ActionDescriptor.ActionName;
            logger.Info(message);
        }
    }

}