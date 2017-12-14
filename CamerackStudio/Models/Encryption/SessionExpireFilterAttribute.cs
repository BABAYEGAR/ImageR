using CamerackStudio.Models.Redis;
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
            if (new RedisDataAgent().GetStringValue("CamerackLoggedInUser") == null || new RedisDataAgent().GetStringValue("CamerackLoggedInUserId") == null)
                {
                    filterContext.Result =
                        new RedirectToRouteResult(
                            new RouteValueDictionary{{ "controller", "Account" },
                                { "action", "Login" },{"returnUrl","sessionExpired"}

                            });
                }
            

            base.OnActionExecuting(filterContext);
        }
    }
}
