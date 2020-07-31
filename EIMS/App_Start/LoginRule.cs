using EIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EIMS.App_Start
{
    public class LoginRule : ActionFilterAttribute
    {
        // Any public properties here can be set within the declaration of the filter
        public int YourProperty { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if(HttpContext.Current.Session["LoginCredentials"] != null)
            {
               
                if (Convert.ToInt32(HttpContext.Current.Session["RoleId"]) == 1)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" } });
                }
                if (Convert.ToInt32(HttpContext.Current.Session["RoleId"]) == 2)
                {
                    

                    // Redirect the user accordingly
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "InternDashboard" } });
                }
                if (Convert.ToInt32(HttpContext.Current.Session["RoleId"]) ==3)
                {
                    // Redirect the user accordingly
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "MentorDashboard" } });
                }
            }

            // Define some condition to check here
         
        }
    }
}