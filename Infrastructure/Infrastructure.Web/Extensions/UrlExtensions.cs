namespace Nagnoi.SiC.Infrastructure.Web.Utilities
{
    #region Imports

    using System;
    using System.Globalization;
    using System.IO;
    using System.Web.Hosting;
    using System.Web.Mvc;

    #endregion

    /// <summary>
    /// URL Helper extension methods
    /// </summary>
    public static class UrlExtensions
    {
        /// <summary>
        /// Generates a fully qualified URL to an action method by using
        /// the specified action name, controller name and route values.
        /// </summary>
        /// <param name="url">URL Helper instance</param>
        /// <param name="actionName">The name of the action method</param>
        /// <param name="controllerName">The controller name</param>
        /// <returns>Returns the absolute URL</returns>
        public static string AbsoluteAction(this UrlHelper url, string actionName, string controllerName)
        {
            return AbsoluteAction(url, actionName, controllerName, null);
        }

        /// <summary>
        /// Generates a fully qualified URL to an action method by using
        /// the specified action name, controller name and route values.
        /// </summary>
        /// <param name="url">URL Helper instance</param>
        /// <param name="actionName">The name of the action method</param>
        /// <param name="controllerName">The controller name</param>
        /// <param name="routeValues">The route values</param>
        /// <returns>Returns the absolute URL</returns>
        public static string AbsoluteAction(this UrlHelper url, string actionName, string controllerName, object routeValues)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;

            string absoluteAction = string.Format("{0}{1}", requestUrl.GetLeftPart(UriPartial.Authority), url.Action(actionName, controllerName, routeValues));

            return absoluteAction;
        }

        /// <summary>
        /// Returns the URL for a image resource
        /// </summary>
        /// <param name="url">URL Helper instance</param>
        /// <param name="fileName">File name</param>
        /// <returns>Returns the absolute path</returns>
        public static string Image(this UrlHelper url, string fileName)
        {
            return url.Content(string.Format("~/content/img/{0}", fileName));
        }

        /// <summary>
        /// Returns the URL for image not found
        /// </summary>
        /// <param name="url">URL Helper instance</param>
        /// <returns>Returns the absolute path</returns>
        public static string NoImage(this UrlHelper url)
        {
            return url.Image("image_not_found.png");
        }

        /// <summary>
        /// Returns the URL home page
        /// </summary>
        /// <param name="url">URL Helper instance</param>
        /// <returns>Returns the absolute path</returns>
        public static string Home(this UrlHelper url)
        {
            return url.Content("~/");
        }

        /// <summary>
        /// Returns the URL sign in page
        /// </summary>
        /// <param name="url">URL Helper instance</param>
        /// <returns>Returns the absolute path</returns>
        public static string SignIn(this UrlHelper url)
        {
            return url.RouteUrl("SignIn");
        }

        /// <summary>
        /// Returns the globaliza script core
        /// </summary>
        /// <param name="url">URL Helper instance</param>
        /// <returns>Returns the absolute path</returns>
        public static string GlobalizeCore(this UrlHelper url)
        {
            return url.Content("~/Scripts/globalize/globalize.js");
        }

        /// <summary>
        /// Returns the globalize script culture
        /// </summary>
        /// <param name="url">URL Helper instance</param>
        /// <returns>Returns the absolute path</returns>
        public static string GlobalizeCulture(this UrlHelper url)
        {
            // Determine culture - GUI culture for preference, user selected culture as fallback
            var currentCulture = CultureInfo.CurrentCulture;
            var filePattern = "~/scripts/globalize/cultures/globalize.culture.{0}.js";
            var regionalisedFiletoUse = string.Format(filePattern, "en-US");

            // Try to pick a more appropiate regionalization
            if (File.Exists(HostingEnvironment.MapPath(string.Format(filePattern, currentCulture.Name))))
            {
                regionalisedFiletoUse = string.Format(filePattern, currentCulture.Name);
            }
            else if (File.Exists(HostingEnvironment.MapPath(string.Format(filePattern, currentCulture.TwoLetterISOLanguageName))))
            {
                regionalisedFiletoUse = string.Format(filePattern, currentCulture.TwoLetterISOLanguageName);
            }

            return regionalisedFiletoUse.ToLowerInvariant();
        }

        /// <summary>
        /// Returns a value that indicated whether the URL valiation culture s defined
        /// </summary>
        /// <param name="url">URL Helper instance</param>
        /// <returns>Returns true or false</returns>
        public static bool ExistsValidationCulture(this UrlHelper url)
        {
            // Determine culture - GUI culture for preference, user selected culture as fallback
            var currentCulture = CultureInfo.CurrentCulture;
            var filePattern = "~/scripts/localization/messages_{0}.js";

            // Try to pick a more appropiate regionalization
            if (File.Exists(HostingEnvironment.MapPath(string.Format(filePattern, currentCulture.TwoLetterISOLanguageName))))
            {
                return true;
            }
            else if (File.Exists(HostingEnvironment.MapPath(string.Format(filePattern, currentCulture.Name))))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the URL validation culture
        /// </summary>
        /// <param name="url">URL Helper instance</param>
        /// <returns>Returns the absolute path</returns>
        public static string ValidationCulture(this UrlHelper url)
        {
            // Determine culture - GUI culture for preference, user selected culture as fallback
            var currentCulture = CultureInfo.CurrentCulture;
            var filePattern = "~/Scripts/localization/messages_{0}.js";
            var regionalisedFiletoUse = string.Format(filePattern, "en-US");

            // Try to pick a more appropiate regionalization
            if (File.Exists(HostingEnvironment.MapPath(string.Format(filePattern, currentCulture.TwoLetterISOLanguageName))))
            {
                regionalisedFiletoUse = string.Format(filePattern, currentCulture.TwoLetterISOLanguageName);
            }
            else if (File.Exists(HostingEnvironment.MapPath(string.Format(filePattern, currentCulture.Name))))
            {
                regionalisedFiletoUse = string.Format(filePattern, currentCulture.Name);
            }

            return regionalisedFiletoUse.ToLowerInvariant();
        }
    }
}