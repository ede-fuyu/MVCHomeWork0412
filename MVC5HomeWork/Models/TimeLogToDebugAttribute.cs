using System;
using System.Web.Mvc;

namespace MVC5HomeWork.Models
{
    internal class TimeLogToDebugAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.dtStart = DateTime.Now;

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.dtEnd = DateTime.Now;

            var dtTimeSpan = (DateTime)filterContext.Controller.ViewBag.dtEnd - (DateTime)filterContext.Controller.ViewBag.dtStart;

            filterContext.Controller.ViewBag.dtTimespan = dtTimeSpan;

            base.OnActionExecuted(filterContext);
            System.Diagnostics.Debug.WriteLine(dtTimeSpan);
        }
    }
}