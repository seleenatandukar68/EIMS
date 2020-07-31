using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EIMS
{
    public class Rule : ActionFilterAttribute
    {
        // Any public properties here can be set within the declaration of the filter
        public int YourProperty { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Define some condition to check here
            if (Convert.ToInt32(HttpContext.Current.Session["RoleId"]) != 1)
            {
                // Redirect the user accordingly
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "LogOff" } });
            }
        }
    }


}