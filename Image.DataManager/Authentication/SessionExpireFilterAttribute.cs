using System.Web.Mvc;

namespace DataManager.Authentication
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext != null)
            {
                var session = filterContext.HttpContext.Session;
                //Controller controller = filterContext.Controller as Controller;

                //if (session.GetString("vmsloggedinuser") == null || session.GetString("userId") == null)
                //{
                //    filterContext.Result =
                //        new RedirectToRouteResult(
                //            new RouteValueDictionary{{ "controller", "Account" },
                //                { "action", "Login" },{"returnUrl","sessionExpired"}

                //            });
                //}
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
