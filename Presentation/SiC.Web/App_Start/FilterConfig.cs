using System.Web.Http;
using System.Web.Mvc;

namespace Nagnoi.SiC.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            #if !DEBUG            
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());
            #endif

            GlobalConfiguration.Configuration.Filters.Add(new ElmahErrorAttribute());
        }
    }
}