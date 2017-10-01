using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Image.Models.Encryption
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            //Controller controller = filterContext.Controller as Controller;

                if (session.GetString("ImageLoggedInUser") == null || session.GetString("userId") == null || session.GetString("Role") == null)
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
