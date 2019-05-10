using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BankID.WebDemo
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // Set the program to use the recommended TLS version, TLS1.2.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Bumps up the connection limit as the default value is quite low (10).
            ServicePointManager.DefaultConnectionLimit = 9999;

            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "Default Api",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
