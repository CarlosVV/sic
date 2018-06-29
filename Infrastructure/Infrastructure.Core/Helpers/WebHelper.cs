namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Security.Principal;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Hosting;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Infrastructure.Core.Configuration;
    using Infrastructure.Core.Exceptions;

    #endregion

    /// <summary>
    /// Provides a set of methods
    /// </summary>
    public static class WebHelper
    {
        #region Public Methods

        /// <summary>
        /// Gets URL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        public static string GetUrlReferrer()
        {
            string referrerUrl = string.Empty;

            if (HttpContext.Current != null &&
                HttpContext.Current.Request != null &&
                HttpContext.Current.Request.UrlReferrer != null)
            {
                referrerUrl = HttpContext.Current.Request.UrlReferrer.ToString();
            }

            return referrerUrl;
        }

        /// <summary>
        /// Returns a site relative HTTP path from a partial path starting out with a ~.
        /// Same syntax that ASP.Net internally supports but this method can be used
        /// outside of the Page framework.
        /// Works like Control.ResolveUrl including support for ~ syntax
        /// but returns an absolute URL.
        /// </summary>
        /// <param name="originalUrl">Any Url including those starting with ~</param>
        /// <returns>relative url</returns>
        public static string ResolveUrl(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                return originalUrl;
            }

            // *** Absolute path - just return
            if (IsAbsolutePath(originalUrl))
            {
                return originalUrl;
            }

            // *** We don't start with the '~' -> we don't process the Url
            if (!originalUrl.StartsWith("~"))
            {
                return originalUrl;
            }

            // *** Fix up path for ~ root app dir directory
            // VirtualPathUtility blows up if there is a 
            // query string, so we have to account for this.
            int queryStringStartIndex = originalUrl.IndexOf('?');
            if (queryStringStartIndex != -1)
            {
                string queryString = originalUrl.Substring(queryStringStartIndex);
                string baseUrl = originalUrl.Substring(0, queryStringStartIndex);

                return string.Concat(
                    VirtualPathUtility.ToAbsolute(baseUrl),
                    queryString);
            }
            else
            {
                return VirtualPathUtility.ToAbsolute(originalUrl);
            }
        }

        /// <summary>
        /// This method returns a fully qualified absolute server Url which includes
        /// the protocol, server, port in addition to the server relative Url.
        /// Works like Control.ResolveUrl including support for ~ syntax
        /// but returns an absolute URL.
        /// </summary>
        /// <param name="serverUrl">Any Url, either App relative or fully qualified</param>
        /// <param name="forceHttps">if true forces the url to use https</param>
        /// <returns>Returns a fully qualified absolute server Url</returns>
        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (string.IsNullOrEmpty(serverUrl))
            {
                return serverUrl;
            }

            // Is it already an absolute Url
            if (IsAbsolutePath(serverUrl))
            {
                return serverUrl;
            }

            string newServerUrl = ResolveUrl(serverUrl);
            Uri result = new Uri(HttpContext.Current.Request.Url, newServerUrl);

            if (!forceHttps)
            {
                return result.ToString();
            }
            else
            {
                return ForceUriToHttps(result).ToString();
            }
        }

        /// <summary>
        /// This method returns a fully qualified absolute server URL which includes
        /// the protocol, server, port in addition to the server relative URL.
        /// It work like Page.ResolveUrl, but adds these to the beginning.
        /// This method is useful for generating URLs for AJAX methods
        /// </summary>
        /// <param name="serverUrl">Any URL, either App relative or fully qualified</param>
        /// <returns>Returns a fully qualified absolute server URL</returns>
        public static string ResolveServerUrl(string serverUrl)
        {
            return ResolveServerUrl(serverUrl, false);
        }

        /// <summary>
        /// Gets query string well-formed
        /// </summary>
        /// <param name="values">List of keys and values</param>
        /// <returns>Query string</returns>
        public static string ToQueryString(NameValueCollection values)
        {
            return string.Format("?{0}", string.Join("&", Array.ConvertAll(values.AllKeys, key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(values[key])))));
        }

        /// <summary>
        /// Gets query string item by name
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string item</returns>
        public static string QueryString(string name)
        {
            string result = string.Empty;

            if (HttpContext.Current != null && HttpContext.Current.Request.QueryString[name] != null)
            {
                result = HttpContext.Current.Request.QueryString[name].ToString();
            }

            return result;
        }

        /// <summary>
        /// Gets boolean item from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string item</returns>
        public static bool QueryStringBool(string name)
        {
            string resultStr = QueryString(name).ToUpperInvariant();

            return resultStr == "YES" || resultStr == "TRUE" || resultStr == "1";
        }

        /// <summary>
        /// Gets integer item from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string item</returns>
        public static int QueryStringInt(string name)
        {
            string resultStr = QueryString(name).ToUpperInvariant();

            int result;

            int.TryParse(resultStr, out result);

            return result;
        }

        /// <summary>
        /// Gets integer item from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="defaultValue">Default item</param>
        /// <returns>Query string item</returns>
        public static int QueryStringInt(string name, int defaultValue)
        {
            string resultStr = QueryString(name).ToUpperInvariant();

            if (resultStr.Length > 0)
            {
                return int.Parse(resultStr);
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets GUID item from query string 
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string item</returns>
        public static Guid? QueryStringGuid(string name)
        {
            string resultStr = QueryString(name).ToUpperInvariant();

            Guid? result = null;

            try
            {
                result = new Guid(resultStr);
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Modifies query string
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryStringModification">Query string modification</param>
        /// <param name="targetLocationModification">Target location modification</param>
        /// <returns>New url</returns>
        public static string ModifyQueryString(string url, string queryStringModification, string targetLocationModification)
        {
            if (url == null)
            {
                url = string.Empty;
            }

            url = url.ToLowerInvariant();

            if (queryStringModification == null)
            {
                queryStringModification = string.Empty;
            }

            queryStringModification = queryStringModification.ToLowerInvariant();

            if (targetLocationModification == null)
            {
                targetLocationModification = string.Empty;
            }

            targetLocationModification = targetLocationModification.ToLowerInvariant();

            string str = string.Empty;
            string str2 = string.Empty;
            if (url.Contains("#"))
            {
                str2 = url.Substring(url.IndexOf("#") + 1);
                url = url.Substring(0, url.IndexOf("#"));
            }

            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }

            if (!string.IsNullOrEmpty(queryStringModification))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }

                    foreach (string str4 in queryStringModification.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str4))
                        {
                            string[] strArray2 = str4.Split(new char[] { '=' });
                            if (strArray2.Length == 2)
                            {
                                dictionary[strArray2[0]] = strArray2[1];
                            }
                            else
                            {
                                dictionary[str4] = null;
                            }
                        }
                    }

                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }

                        builder.Append(str5);

                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }

                    str = builder.ToString();
                }
                else
                {
                    str = queryStringModification;
                }
            }

            if (!string.IsNullOrEmpty(targetLocationModification))
            {
                str2 = targetLocationModification;
            }

            return (url + (string.IsNullOrEmpty(str) ? string.Empty : ("?" + str)) + (string.IsNullOrEmpty(str2) ? string.Empty : ("#" + str2))).ToLowerInvariant();
        }

        /// <summary>
        /// Remove query string from url
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryString">Query string to remove</param>
        /// <returns>New url</returns>
        public static string RemoveQueryString(string url, string queryString)
        {
            if (url == null)
            {
                url = string.Empty;
            }

            url = url.ToLowerInvariant();

            if (queryString == null)
            {
                queryString = string.Empty;
            }

            queryString = queryString.ToLowerInvariant();

            string str = string.Empty;
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }

                    dictionary.Remove(queryString);

                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }

                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }

                    str = builder.ToString();
                }
            }

            return url + (string.IsNullOrEmpty(str) ? string.Empty : ("?" + str));
        }

        /// <summary>
        /// Gets server variable by name
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns>Server variable</returns>
        public static string ServerVariables(string name)
        {
            string result = string.Empty;
            try
            {
                if (HttpContext.Current.Request.ServerVariables[name] != null)
                {
                    result = HttpContext.Current.Request.ServerVariables[name].ToString();
                }
            }
            catch
            {
                result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Disable browser cache
        /// </summary>
        public static void DisableBrowserCache()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                HttpContext.Current.Response.Cache.SetNoStore();
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Current.Response.Cache.AppendCacheExtension("post-check=0,pre-check=0");
            }
        }

        /// <summary>
        /// Gets the server name
        /// </summary>
        /// <returns>Return the server name</returns>
        public static string GetServerName()
        {
            string result = Environment.MachineName;

            try
            {
                if (HttpContext.Current != null &&
                    HttpContext.Current.Request != null)
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(ServerVariables("LOCAL_ADDR"));

                    // Split out the host name
                    if (hostEntry.HostName.Contains("."))
                    {
                        string[] splitHostName = hostEntry.HostName.Split('.');

                        result = splitHostName[0].ToString();
                    }
                    else
                    {
                        result = hostEntry.HostName;
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Gets user host address
        /// </summary>
        /// <returns>Returns the host address represented by IP</returns>
        public static string GetUserHostAddress()
        {
            string result = string.Empty;

            if (HttpContext.Current != null &&
                    HttpContext.Current.Request != null &&
                    HttpContext.Current.Request.UserHostAddress != null)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            if (!string.IsNullOrEmpty(ServerVariables("HTTP_X_FORWARDED_FOR")))
            {
                result = ServerVariables("HTTP_X_FORWARDED_FOR");
            }

            return result;
        }

        /// <summary>
        /// Gets a item indicating whether current connection is secured
        /// </summary>
        /// <returns>true - secured, false - not secured</returns>
        public static bool IsCurrentConnectionSecured()
        {
            bool useSSL = false;
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                useSSL = HttpContext.Current.Request.IsSecureConnection;
                // when your hosting uses a load balancer on their server then the Request.IsSecureConnection is never got set to true, use the statement below
                // just uncomment it
                // useSSL = HttpContext.Current.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] == "on" ? true : false;
            }

            return useSSL;
        }

        /// <summary>
        /// Gets store admin location
        /// </summary>
        /// <returns>Store admin location</returns>
        public static string GetAdminModuleLocation()
        {
            bool useSSL = IsCurrentConnectionSecured();
            return GetAdminModuleLocation(useSSL);
        }

        /// <summary>
        /// Gets store admin location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>Store admin location</returns>
        public static string GetAdminModuleLocation(bool useSsl)
        {
            string result = GetStoreLocation(useSsl);

            if (HostingEnvironment.IsHosted)
            {
                result = result + "admin";
            }
            else
            {
                result = GetStoreHost(useSsl) + "admin";
            }

            return result.ToLowerInvariant();
        }

        /// <summary>
        /// Gets store location
        /// </summary>
        /// <returns>Store location</returns>
        public static string GetStoreLocation()
        {
            bool useSSL = IsCurrentConnectionSecured();

            return GetStoreLocation(useSSL);
        }

        /// <summary>
        /// Gets store location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>Store location</returns>
        public static string GetStoreLocation(bool useSsl)
        {
            string applicationHost = GetStoreHost(useSsl);
            if (applicationHost.EndsWith("/"))
            {
                applicationHost = applicationHost.Substring(0, applicationHost.Length - 1);
            }

            StringBuilder result = new StringBuilder();

            result.Append(applicationHost);
            if (HostingEnvironment.IsHosted)
            {
                result.Append(HostingEnvironment.ApplicationVirtualPath);
            }

            if (!result.ToString().EndsWith("/"))
            {
                result.Append('/');
            }

            return result.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Gets store host location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>Store host location</returns>
        public static string GetStoreHost(bool useSsl)
        {
            StringBuilder result = new StringBuilder();

            result.Append("http://");
            result.Append(ServerVariables("HTTP_HOST"));

            if (result.ToString().EndsWith("/"))
            {
                result.Append('/');
            }

            if (useSsl &&
                !string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseSSL"]) &&
                SystemSettings.UseSsl)
            {
                result.Replace("http:/", "https:/");
            }

            if (!result.ToString().EndsWith("/"))
            {
                result.Append('/');
            }

            return result.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Gets the root site URL
        /// </summary>
        /// <returns>URL root site</returns>
        public static string GetSiteLocation()
        {
            string host = GetStoreHost(true);

            string result = "{0}{1}".FormatString(host, HttpContext.Current.Request.Url.Segments[1]);

            return result.ToLowerInvariant();
        }

        /// <summary>
        /// Gets this current URL page
        /// </summary>
        /// <param name="includeQueryString">A value indicating whether must include the current query string</param>
        /// <returns>Returns URL page</returns>
        public static string GetThisPageUrl(bool includeQueryString)
        {
            return GetThisPageUrl(includeQueryString, false, string.Empty);
        }

        /// <summary>
        /// Gets this current URL page
        /// </summary>
        /// <param name="includeQueryString">A value indicating whether must include the current query string</param>
        /// <param name="includeLanguage">A value indicating whether must include the language code</param>
        /// <param name="languageCode">Language code</param>
        /// <returns>Returns URL page</returns>
        public static string GetThisPageUrl(bool includeQueryString, bool includeLanguage, string languageCode)
        {
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }

            StringBuilder url = new StringBuilder();

            bool useSSL = IsCurrentConnectionSecured();
            string storeHost = GetStoreHost(useSSL);

            if (storeHost.EndsWith("/"))
            {
                storeHost = storeHost.Substring(0, storeHost.Length - 1);
            }

            url.Append(storeHost);

            if (includeQueryString)
            {
                url.Append(HttpContext.Current.Request.RawUrl);
            }
            else
            {
                url.Append(HttpContext.Current.Request.Url.AbsolutePath);
            }

            if (includeLanguage && !string.IsNullOrEmpty(languageCode))
            {
                return ModifyQueryString(url.ToString().ToLowerInvariant(), "languageId=" + languageCode, null);
            }

            return url.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Reloads current page
        /// </summary>
        public static void ReloadCurrentPage()
        {
            bool useSSL = IsCurrentConnectionSecured();

            ReloadCurrentPage(useSSL);
        }

        /// <summary>
        /// Reloads current page
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        public static void ReloadCurrentPage(bool useSsl)
        {
            string storeHost = GetStoreHost(useSsl);

            if (storeHost.EndsWith("/"))
            {
                storeHost = storeHost.Substring(0, storeHost.Length - 1);
            }

            string url = storeHost + HttpContext.Current.Request.RawUrl;

            url = url.ToLowerInvariant();

            if (useSsl)
            {
                HttpContext.Current.Response.RedirectPermanent(url);
            }
            else
            {
                HttpContext.Current.Response.Redirect(url);
            }
        }

        /// <summary>
        /// Ensures that requested page is secured (https://)
        /// </summary>
        public static void EnsureSsl()
        {
            if (!IsCurrentConnectionSecured())
            {
                bool useSSL = false;

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseSSL"]))
                {
                    useSSL = SystemSettings.UseSsl;
                }

                if (useSSL)
                {
                    ////if (!HttpContext.Current.Request.Url.IsLoopback)
                    ////{
                    ReloadCurrentPage(true);
                    ////}
                }
            }
        }

        /// <summary>
        /// Ensures that requested page is not secured (http://)
        /// </summary>
        public static void EnsureNonSsl()
        {
            if (IsCurrentConnectionSecured())
            {
                ReloadCurrentPage(false);
            }
        }

        /// <summary>
        /// Sets a session cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="cookieValue">Cookie item</param>
        [Obsolete("Use CookieHelper class instead of.")]
        public static void SetCookie(string cookieName, string cookieValue)
        {
            SetCookie(cookieName, cookieValue, TimeSpan.Zero);
        }

        /// <summary>
        /// Sets a session cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="cookieValue">Cookie item</param>
        /// <param name="httpOnly">A item indicating whether must mark like HttpOnly</param>
        [Obsolete("Use CookieHelper class instead of.")]
        public static void SetCookie(string cookieName, string cookieValue, bool httpOnly)
        {
            SetCookie(cookieName, cookieValue, TimeSpan.Zero, httpOnly);
        }

        /// <summary>
        /// Sets a cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="cookieValue">Cookie item</param>
        /// <param name="expires">Timespan expires</param>
        [Obsolete("Use CookieHelper class instead of.")]
        public static void SetCookie(string cookieName, string cookieValue, TimeSpan expires)
        {
            SetCookie(cookieName, cookieValue, expires, true);
        }

        /// <summary>
        /// Sets a cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="cookieValue">Cookie item</param>
        /// <param name="expires">Timespan expires</param>
        /// <param name="httpOnly">A item indicating whether must mark like HttpOnly</param>
        [Obsolete("Use CookieHelper class instead of.")]
        public static void SetCookie(string cookieName, string cookieValue, TimeSpan expires, bool httpOnly)
        {
            try
            {
                HttpCookie cookie = null;

                if (HttpContext.Current.Request.Cookies[cookieName] == null)
                {
                    cookie = new HttpCookie(cookieName);
                }
                else
                {
                    cookie = HttpContext.Current.Request.Cookies[cookieName];
                }

                cookie.HttpOnly = httpOnly;
                cookie.Value = HttpContext.Current.Server.UrlEncode(cookieValue);

                if (expires != null && expires != TimeSpan.Zero)
                {
                    DateTime actualDate = DateTime.UtcNow;
                    cookie.Expires = actualDate.Add(expires);
                }

                HttpContext.Current.Response.Cookies.Remove(cookieName);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Removes a cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        [Obsolete("Use CookieHelper class instead of.")]
        public static void RemoveCookie(string cookieName)
        {
            SetCookie(cookieName, string.Empty, new TimeSpan(-1, 0, 0, 0));
        }

        /// <summary>
        /// Gets cookie string
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="decode">Decode cookie item</param>
        /// <returns>Cookie string</returns>
        [Obsolete("Use CookieHelper class instead of.")]
        public static string GetCookieString(string cookieName, bool decode)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] == null)
            {
                return string.Empty;
            }

            try
            {
                string temp = HttpContext.Current.Request.Cookies[cookieName].Value.ToString();

                if (decode)
                {
                    temp = HttpContext.Current.Server.UrlDecode(temp);
                }

                return temp;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the map path from current execution directory
        /// </summary>
        /// <param name="path">Resource path</param>
        /// <returns>Return the formatted path</returns>
        public static string MapPath(string path)
        {
            if (!path.StartsWith("~"))
            {
                path = "~" + path;
            }

            return HttpContext.Current.Server.MapPath(path);
        }

        /// <summary>
        /// Restart application domain
        /// </summary>
        /// <param name="makeRedirect">A item indicating whether </param>
        /// <param name="redirectUrl">Redirect URL; empty string if you want to redirect to the current page URL</param>
        public static void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "")
        {
            if (CommonHelper.GetTrustLevel() > AspNetHostingPermissionLevel.Medium)
            {
                // full trust
                HttpRuntime.UnloadAppDomain();

                TryWriteGlobalAsax();
            }
            else
            {
                // medium trust
                bool success = TryWriteWebConfig();
                if (!success)
                {
                    throw new BusinessException("nopCommerce needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
                        "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
                        "- run the application in a full trust environment, or" + Environment.NewLine +
                        "- give the application write access to the 'web.config' file.");
                }

                success = TryWriteGlobalAsax();
                if (!success)
                {
                    throw new BusinessException("nopCommerce needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
                        "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
                        "- run the application in a full trust environment, or" + Environment.NewLine +
                        "- give the application write access to the 'Global.asax' file.");
                }
            }

            // If setting up extensions/modules requires an AppDomain restart, it's very unlikely the
            // current request can be processed correctly.  So, we redirect to the same URL, so that the
            // new request will come to the newly started AppDomain.
            if (HttpContext.Current != null && makeRedirect)
            {
                if (string.IsNullOrEmpty(redirectUrl))
                {
                    redirectUrl = GetThisPageUrl(true);
                }

                HttpContext.Current.Response.Redirect(redirectUrl, true /*endResponse*/);
            }
        }

        /// <summary>
        /// Get a item indicating whether the request is made by search engine (web crawler)
        /// </summary>
        /// <param name="request">HTTP Request</param>
        /// <returns>Returns true or false</returns>
        public static bool IsSearchEngine(HttpRequestBase request)
        {
            if (request == null)
            {
                return false;
            }

            bool result = false;
            try
            {
                result = request.Browser.Crawler;
                if (!result)
                {
                    // put any additional known crawlers in the Regex below for some custom validation
                    // var regEx = new Regex("Twiceler|twiceler|BaiDuSpider|baduspider|Slurp|slurp|ask|Ask|Teoma|teoma|Yahoo|yahoo");
                    // applicationHost = regEx.Match(request.UserAgent).Success;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return result;
        }

        /// <summary>
        /// Returns a item indicating whether the requested resource is one of
        /// the typical resources that needn't be processed by the app engine
        /// </summary>
        /// <param name="request">HTTP Request</param>
        /// <returns>True if the request targets a static resource file</returns>
        /// <remarks>
        /// These are the file extensions considered to be statis resources:
        /// .css
        /// .gif
        /// .png
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        public static bool IsStaticResource(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            string extension = request.CurrentExecutionFilePathExtension;
            if (extension == null)
            {
                return false;
            }

            switch (extension.ToLowerInvariant())
            {
                case ".axd":
                case ".ashx":
                case ".bmp":
                case ".css":
                case ".gif":
                case ".htm":
                case ".html":
                case ".ico":
                case ".jpeg":
                case ".jpg":
                case ".js":
                case ".png":
                case ".rar":
                case ".zip":
                case ".woff":
                case ".ttf":
                case ".eot":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a item indicating whether the current request is AJAX
        /// </summary>
        /// <returns>Return true or false</returns>
        public static bool IsAjaxRequest()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null)
            {
                return false;
            }

            return (HttpContext.Current.Request["X-Requested-With"] == "XMLHttpRequest") || ((HttpContext.Current.Request.Headers != null) && (HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        /// <summary>
        /// Gets a item indicating whether requested page is an admin page
        /// </summary>
        /// <returns>A item indicating whether requested page is an admin page</returns>
        public static bool IsInAdministrationModule()
        {
            string thisPageUrl = GetThisPageUrl(false);
            if (string.IsNullOrEmpty(thisPageUrl))
            {
                return false;
            }

            string appUrlNoSecured = GetStoreLocation(false);
            string appUrlSecured = GetStoreLocation(true);

            if (HostingEnvironment.IsHosted)
            {
                appUrlNoSecured = appUrlNoSecured + "admin";
                appUrlSecured = appUrlSecured + "admin";
            }
            else
            {
                appUrlNoSecured = GetStoreHost(false) + "admin";
                appUrlSecured = GetStoreHost(true) + "admin";
            }

            bool pageUrlNoSecuredIsInAdmin = thisPageUrl.ToLowerInvariant().StartsWith(appUrlNoSecured.ToLowerInvariant());
            bool pageUrlSecuredIsInAdmin = thisPageUrl.ToLowerInvariant().StartsWith(appUrlSecured.ToLowerInvariant());

            return pageUrlNoSecuredIsInAdmin || pageUrlSecuredIsInAdmin;
        }

        /// <summary>
        /// Write XLS file to response
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="targetFileName">Target file name</param>
        public static void WriteResponseXls(string filePath, string targetFileName)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/xls";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
                response.BinaryWrite(File.ReadAllBytes(filePath));
                response.End();
            }
        }

        /// <summary>
        /// Write XLS file to response
        /// </summary>
        /// <param name="content">File contents</param>
        /// <param name="targetFileName">Target file name</param>
        public static void WriteResponseXlsContent(string content, string targetFileName)
        {
            if (!string.IsNullOrEmpty(content))
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/xls";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
                response.Write(content);
                response.Flush();
                response.End();
            }
        }

        /// <summary>
        /// Write PDF file to response
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="targetFileName">Target file name</param>
        public static void WriteResponsePdf(string filePath, string targetFileName)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/pdf";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
                response.BinaryWrite(File.ReadAllBytes(filePath));
                response.End();
            }
        }

        /// <summary>
        /// Write Binary file to response
        /// </summary>
        /// <param name="content">Content file</param>
        /// <param name="contentType">Content type</param>
        /// <param name="targetFileName">Target file name</param>
        public static void WriteResponseBinaryFile(byte[] content, string contentType, string targetFileName)
        {
            if (content != null)
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = contentType;
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
                response.BinaryWrite(content);
                response.End();
            }
        }

        /// <summary>
        /// Write XML string to response
        /// </summary>
        /// <param name="content">XML content</param>
        /// <param name="targetFileName">Target file name</param>
        public static void WriteResponseXmlContent(string content, string targetFileName)
        {
            if (!string.IsNullOrEmpty(content))
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/xml";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
                response.Write(content);
                response.Flush();
                response.End();
            }
        }

        /// <summary>
        /// Cleans a HTML removing the VIEWSTATE tag
        /// </summary>
        /// <param name="htmlContents">HTML string</param>
        /// <returns>Returns a HTML string without VIEWSTATE tag</returns>
        public static string CleanHtml(string htmlContents)
        {
            return CleanHtml(htmlContents, true);
        }

        /// <summary>
        /// Cleans a HTML removing the VIEWSTATE tag
        /// </summary>
        /// <param name="htmlContents">HTML string</param>
        /// <param name="minify">Indicates whether must minify a HTML string</param>
        /// <returns>Returns a HTML string without VIEWSTATE tag</returns>
        public static string CleanHtml(string htmlContents, bool minify)
        {
            // Replace form tag
            string formTagRemoved = Regex.Replace(htmlContents, @"<[/]?(form)[^>]*?>", string.Empty, RegexOptions.IgnoreCase);

            // Replace viewstate hidden
            string viewStateRemoved = Regex.Replace(formTagRemoved, "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\".*?\" />", string.Empty, RegexOptions.IgnoreCase);

            // Replace event validation hidden
            viewStateRemoved = Regex.Replace(viewStateRemoved, "<input type=\"hidden\" name=\"__EVENTVALIDATION\" id=\"__EVENTVALIDATION\" value=\".*?\" />", string.Empty, RegexOptions.IgnoreCase);

            // Replace viewstate generator
            viewStateRemoved = Regex.Replace(viewStateRemoved, "<input type=\"hidden\" name=\"__VIEWSTATEGENERATOR\" id=\"__VIEWSTATEGENERATOR\" value=\".*?\" />", string.Empty, RegexOptions.IgnoreCase);

            return minify ? MinifyHtml(viewStateRemoved) : viewStateRemoved;
        }

        /// <summary>
        /// Minifies the given HTML string.
        /// </summary>
        /// <param name="htmlContents">The HTML to minify</param>
        /// <returns>Return a <see cref="string"/> minified</returns>
        public static string MinifyHtml(string htmlContents)
        {
            // Replace line comments
            htmlContents = Regex.Replace(htmlContents, @"// (.*?)\r?\n", "", RegexOptions.Singleline);

            // Replace spaces between quotes
            htmlContents = Regex.Replace(htmlContents, @"\s+", " ");

            // Replace line breaks
            htmlContents = Regex.Replace(htmlContents, @"\s*\n\s*", "\n");

            // Replace spaces between brackets
            htmlContents = Regex.Replace(htmlContents, @"\s*\>\s*\<\s*", "><");

            // Replace comments
            htmlContents = Regex.Replace(htmlContents, @"<!--(.*?)-->", "");

            // single-line doctype must be preserved 
            var firstEndBracketPosition = htmlContents.IndexOf(">", StringComparison.Ordinal);
            if (firstEndBracketPosition >= 0)
            {
                htmlContents = htmlContents.Remove(firstEndBracketPosition, 1);
                htmlContents = htmlContents.Insert(firstEndBracketPosition, ">");
            }

            return htmlContents;
        }

        /// <summary>
        /// Ensure whether the image exists on web site
        /// </summary>
        /// <param name="imagePath">Image Path</param>
        /// <returns>Image path validated</returns>
        public static string EnsureImagePath(string imagePath)
        {
            if (HttpContext.Current == null)
            {
                return imagePath;
            }

            if (string.IsNullOrEmpty(imagePath))
            {
                return CommonHelper.GetImageSrc("~/content/img/image_not_found.png");
            }

            string localPath = MapPath(imagePath);

            if (!File.Exists(localPath))
            {
                return CommonHelper.GetImageSrc("~/content/img/image_not_found.png");
            }

            return CommonHelper.GetImageSrc(imagePath);
        }

        /// <summary>
        /// Bind jQuery
        /// </summary>
        /// <param name="page">Page instance</param>
        public static void BindJQuery(Page page)
        {
            BindJQuery(page, false);
        }

        /// <summary>
        /// Bind jQuery
        /// </summary>
        /// <param name="page">Page instance</param>
        /// <param name="useHosted">Use hosted jQuery</param>
        public static void BindJQuery(Page page, bool useHosted)
        {
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }

            string jqueryUrl = string.Empty;

            if (useHosted)
            {
                jqueryUrl = "//ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js";
            }
            else
            {
                jqueryUrl = string.Concat(GetStoreLocation(), "Content/Scripts/jquery.js");
            }

            jqueryUrl = string.Format("<script type=\"text/javascript\" charset=\"utf-8\" src=\"{0}\" ></script>", jqueryUrl);

            if (page.Header != null)
            {
                // we have a header
                if (HttpContext.Current.Items["JQueryRegistered"] == null ||
                    !Convert.ToBoolean(HttpContext.Current.Items["JQueryRegistered"]))
                {
                    Literal script = new Literal() { Text = jqueryUrl };
                    Control control = page.Header.FindControl("SCRIPTS");
                    if (control != null)
                    {
                        control.Controls.AddAt(0, script);
                    }
                    else
                    {
                        page.Header.Controls.AddAt(0, script);
                    }
                }

                HttpContext.Current.Items["JQueryRegistered"] = true;
            }
            else
            {
                // no header found
                // page.ClientScript.RegisterClientScriptInclude(page.GetType(), "jqueryScriptKey", jQueryUrl);
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "jqueryScriptKey", jqueryUrl, false);
            }
        }

        /// <summary>
        /// Determines if GZip is supported
        /// </summary>
        /// <returns>Returns true or false</returns>
        public static bool IsGZipSupported()
        {
            string acceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(acceptEncoding) &&
                 (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate")))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds the specified encoding to the response headers.
        /// </summary>
        /// <param name="encoding">Input string</param>
        public static void SetEncoding(string encoding)
        {
            HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
        }

        /// <summary>
        /// Gets the version of Internet Explorer or -1
        /// indicating the use of another browse
        /// </summary>
        /// <returns>Returns the browser version</returns>
        public static float GetInternetExplorerVersion()
        {
            float version = -1;

            if (HttpContext.Current != null)
            {
                HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                if (browser.Browser == "IE")
                {
                    version = (float)(browser.MajorVersion + browser.MinorVersion);
                }
            }

            return version;
        }

        /// <summary>
        /// Encodes a string to be represented as a string literal. The format
        /// is essentially a JSON string that is returned in double quotes.
        /// The string returned includes outer quotes: 
        /// "Hello \"Rick\"!\r\nRock on"
        /// </summary>
        /// <param name="script">Input string</param>
        /// <returns>Returns an encoded string</returns>
        public static string EncodeJsString(string script)
        {
            if (script == null)
            {
                return "null";
            }

            StringBuilder result = new StringBuilder();
            result.Append("\"");
            foreach (char c in script)
            {
                switch (c)
                {
                    case '\"':
                        result.Append("\\\"");
                        break;
                    case '\\':
                        result.Append("\\\\");
                        break;
                    case '\b':
                        result.Append("\\b");
                        break;
                    case '\f':
                        result.Append("\\f");
                        break;
                    case '\n':
                        result.Append("\\n");
                        break;
                    case '\r':
                        result.Append("\\r");
                        break;
                    case '\t':
                        result.Append("\\t");

                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            result.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            result.Append(c);
                        }

                        break;
                }
            }

            result.Append("\"");

            return result.ToString();
        }

        /// <summary>
        /// Translates the current ASP.NET path  
        /// into an application relative path: subdir/page.aspx. The
        /// path returned is based of the application base and 
        /// starts either with a subdirectory or page name (ie. no ~)
        /// This version uses the current ASP.NET path of the request
        /// that is active and internally uses AppRelativeCurrentExecutionFilePath
        /// </summary>
        /// <returns>Returns the relative path of application</returns>
        public static string GetAppRelativePath()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Replace("~/", string.Empty);
        }

        /// <summary>
        /// Changes the application domain identifier
        /// </summary>
        public static void ChangeAppDomainAppId()
        {
            ChangeAppDomainAppId(string.Empty);
        }

        /// <summary>
        /// Changes the application domain identifier
        /// </summary>
        /// <param name="name"></param>
        public static void ChangeAppDomainAppId(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                SessionStateSection sessionSettings = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");

                name = sessionSettings.CookieName;
            }

            FieldInfo runtimeInfo = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.Static | BindingFlags.NonPublic);
            if (runtimeInfo == null)
            {
                return;
            }

            HttpRuntime runtime = (HttpRuntime)runtimeInfo.GetValue(null);
            if (runtime == null)
            {
                return;
            }

            FieldInfo appNameInfo = typeof(HttpRuntime).GetField("_appDomainAppId", BindingFlags.Instance | BindingFlags.NonPublic);
            if (appNameInfo == null)
            {
                return;
            }

            string appName = appNameInfo.GetValue(runtime).ToString();
            if (appName != "applicationName")
            {
                appNameInfo.SetValue(runtime, name);
            }
        }

        /// <summary>
        /// Downloads a image from URL
        /// </summary>
        /// <param name="downloadUrl">URL download</param>
        /// <returns>Returns the image content</returns>
        public static byte[] DownloadImageFromURL(string downloadUrl)
        {
            byte[] imageBytes;
            WebResponse response;
            WebRequest request = HttpWebRequest.Create(downloadUrl);

            response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            using (BinaryReader reader = new BinaryReader(responseStream))
            {
                imageBytes = reader.ReadBytes(500000);
                reader.Close();
            }

            responseStream.Close();
            response.Close();

            return imageBytes;
        }

        /// <summary>
        /// Gets a value indicating whether the debugging mode is enabled
        /// </summary>
        /// <returns>Returns true or false</returns>
        public static bool IsDebugging()
        {
            return HttpContext.Current != null && HttpContext.Current.IsDebuggingEnabled;
        }

        public static string GetUserName()
        {
            if (HttpContext.Current != null &&
                HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity != null)
            {
                return HttpContext.Current.User.Identity.Name;
            }
            
            return WindowsIdentity.GetCurrent().Name;
        }

        public static bool UserIsInRole(string role)
        {
            if (HttpContext.Current != null &&
                HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity != null)
            {
                return HttpContext.Current.User.IsInRole(role);
            }
            return false;
        }

        public static string[] GetRolesForUser()
        {
            if (Roles.Enabled)
            {
                return Roles.GetRolesForUser(GetUserName());
            }

            return new string[0];
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Tries to write into the web configuration
        /// </summary>
        /// <returns>Returns true or false</returns>
        private static bool TryWriteWebConfig()
        {
            try
            {
                // In medium trust, "UnloadAppDomain" is not supported. Touch web.config
                // to force an AppDomain restart.
                File.SetLastWriteTimeUtc(MapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to write into the global application
        /// </summary>
        /// <returns>Returns true or false</returns>
        private static bool TryWriteGlobalAsax()
        {
            try
            {
                // When a new plugin is dropped in the Plugins folder and is installed into nopCommerce, 
                // even if the plugin has registered routes for its controllers, 
                // these routes will not be working as the MVC framework couldn't 
                // find the new controller types and couldn't instantiate the requested controller. 
                // That's why you get these nasty errors 
                // i.e "Controller does not implement IController".
                // The issue is described here: http://www.nopcommerce.com/boards/t/10969/nop-20-plugin.aspx?p=4#51318
                // The solutino is to touch global.asax file
                File.SetLastWriteTimeUtc(MapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Forces the Uri to use https
        /// </summary>
        /// <param name="uri">URI instance</param>
        /// <returns>Returns a URI instance</returns>
        private static Uri ForceUriToHttps(Uri uri)
        {
            UriBuilder builder = new UriBuilder(uri);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = 443;

            return builder.Uri;
        }

        /// <summary>
        /// Determines whether the URL is an absolute path
        /// </summary>
        /// <param name="originalUrl">URL original</param>
        /// <returns>Returns true or false</returns>
        private static bool IsAbsolutePath(string originalUrl)
        {
            int indexOfSlashes = originalUrl.IndexOf("://");
            int indexOfQuestionMarks = originalUrl.IndexOf("?");

            if (indexOfSlashes > -1 &&
                 (indexOfQuestionMarks < 0 ||
                  (indexOfQuestionMarks > -1 && indexOfQuestionMarks > indexOfSlashes)))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}