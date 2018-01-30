using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace CamerackStudio.Models.Encryption
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("StudioLoggedInUserId") == null
                && filterContext.HttpContext.Session.GetString("StudioLoggedInUser") == null)
            {
                filterContext.Result =
                    new RedirectToRouteResult(
                       
                        new RouteValueDictionary{{ "controller", "Home" },
                            { "action", "Index" },{"returnUrl","sessionExpired"}

                        });
            }


            base.OnActionExecuting(filterContext);
        }
    }
}
