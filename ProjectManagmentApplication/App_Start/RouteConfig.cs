using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectManagementApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Board",
                "boards/{id}",
                new { controller = "Boards", action = "Details" },
                new { id = @"\d+" });

            

            //routes.MapRoute(
            //    "404-PageNotFound",
            //    "{*url}",
            //    new { controller = "Home", action = "PageNotFound" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
