using System.Web.Mvc;
using System.Web.Routing;

namespace Nagnoi.SiC.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();
            routes.LowercaseUrls = true;

            routes.MapRoute(
               "SpecificItem",                                              // Route name
               "{controller}/{action}/{id}/{itemId}",                           // URL with parameters
               new { controller = "Home", action = "Index", id = "", itemId = "" }  // Parameter defaults
           );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}