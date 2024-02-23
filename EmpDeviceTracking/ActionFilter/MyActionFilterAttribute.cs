using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace EmpDeviceTracking.ActionFilter
{
    public class MyActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            var result = context.Result as ViewResult;

            if(result != null)
            {
                result.ViewData["username"] = context.HttpContext.Session.GetString("UserName");
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
           // string ID = context.HttpContext.Session.GetString("HostName");

            base.OnActionExecuting(context);
        }

    }
}
