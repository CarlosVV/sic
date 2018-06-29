namespace Nagnoi.SiC.Infrastructure.Web.Utilities
{
    #region Imports

    using System;
    using System.Linq;
    using System.Web.Mvc;

    #endregion

    public static class HtmlExtensions
    {
        /// <summary>
        /// Returns a site relative HTTP path from a partial path starting out with a ~
        /// </summary>
        /// <param name="html">HtmlUtil instance</param>
        /// <param name="url">Any Url including those starting with ~</param>
        /// <returns>Returns the relative URL</returns>
        public static MvcHtmlString ResolveUrl(this HtmlHelper html, string url)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            return MvcHtmlString.Create(urlHelper.Content(url.ToLowerInvariant()));
        }

        /// <summary>
        /// Determines whether an tree menu must be is active
        /// </summary>
        /// <param name="html">HtmlHelper instance</param>
        /// <param name="controllerName">Controller name</param>
        /// <returns>Returns active class or not</returns>
        public static string MakeTreeActive(this HtmlHelper html, params string[] controllerNames)
        {
            var routeData = html.ViewContext.RouteData;

            string routeController = routeData.Values["controller"].ToString();

            bool returnIsActive = controllerNames.Any(item => item.ToLowerInvariant().Equals(routeController, StringComparison.OrdinalIgnoreCase));

            return returnIsActive ? "active" : string.Empty;
        }

        /// <summary>
        /// Determines whether an action link must be active
        /// </summary>
        /// <param name="html">HtmlHelper instance</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <returns>Returns active class or not</returns>
        public static string MakeActive(this HtmlHelper html, string actionName, string controllerName)
        {
            var routeData = html.ViewContext.RouteData;

            string routeAction = routeData.Values["action"].ToString();
            string routeController = routeData.Values["controller"].ToString();

            // both must match
            bool returnIsActive = controllerName.Equals(routeController, StringComparison.OrdinalIgnoreCase) &&
                                  actionName.Equals(routeAction, StringComparison.OrdinalIgnoreCase);

            return returnIsActive ? "active" : string.Empty;
        }

        /// <summary>
        /// Determines whether a tree view must be active
        /// </summary>
        /// <param name="html">HtmlHelper instance</param>
        /// <param name="controllerName">Controller name</param>
        /// <returns>Returns angle down class or not</returns>
        public static string MakeCollapse(this HtmlHelper html, params string[] controllerNames)
        {
            var routeData = html.ViewContext.RouteData;

            string routeController = routeData.Values["controller"].ToString();

            bool returnIsActive = controllerNames.Any(item => item.ToLowerInvariant().Equals(routeController, StringComparison.OrdinalIgnoreCase));

            return returnIsActive ? "fa-angle-down" : "fa-angle-left";
        }
    }
}