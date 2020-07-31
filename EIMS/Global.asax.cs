using EIMS.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EIMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string con = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();           
            //RouteTable.Routes.MapHubs();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
          //  SqlDependency.Start(con);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            NotificationComponent NC = new NotificationComponent();
            var currentTime = DateTime.Now;
            HttpContext.Current.Session["UserName"] = "";
          //  string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["UserName"].ToString()
           // NC.RegisterNotification(currentTime);
        }


        protected void Application_End()
        {
            //here we will stop Sql Dependency
         //   SqlDependency.Stop(con);
        }
    }
}
