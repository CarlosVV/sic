using System.Web.Http;

namespace Nagnoi.SiC.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                //routeTemplate: "api/{controller}/{action}/{date}",
                routeTemplate: "api/{controller}/{date}",
                defaults: new { date = RouteParameter.Optional }
            );
        }
    }
}