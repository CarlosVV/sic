namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Web;

    #endregion

    /// <summary>
    /// Provides a set of helper methods for HTTP Cookies
    /// </summary>
    public static class CookieHelper
    {
        #region Public Methods

        /// <summary>
        /// Sets a session cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="cookieValue">Cookie item</param>
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
        public static void SetCookie(string cookieName, string cookieValue, bool httpOnly)
        {
            SetCookie(cookieName, cookieValue, TimeSpan.Zero, httpOnly);
        }

        /// <summary>
        /// Sets a cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="cookieValue">Cookie item</param>
        /// <param name="expires">Expiration time</param>
        public static void SetCookie(string cookieName, string cookieValue, TimeSpan expires)
        {
            SetCookie(cookieName, cookieValue, expires, true);
        }

        /// <summary>
        /// Sets a cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="cookieValue">Cookie item</param>
        /// <param name="expires">Expiration time</param>
        /// <param name="httpOnly">A item indicating whether must mark like HttpOnly</param>
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
        /// Sets a cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="cookieValues">Cookie values</param>
        /// <param name="expires">Expiration time</param>
        /// <param name="httpOnly">A item indicating whether must mark like HttpOnly</param>
        public static void SetCookie(string cookieName, IDictionary<string, string> cookieValues, TimeSpan expires, bool httpOnly)
        {
            try
            {
                HttpCookie cookie = null;
                NameValueCollection values = cookieValues.ToNameValueCollection();

                if (HttpContext.Current.Request.Cookies[cookieName] == null)
                {
                    cookie = new HttpCookie(cookieName);
                }
                else
                {
                    cookie = HttpContext.Current.Request.Cookies[cookieName];
                }

                cookie.HttpOnly = httpOnly;
                cookie.Values.Clear();
                cookie.Values.Add(values);

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
        /// Gets cookie values
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <returns>Returns a specialized collection</returns>
        public static NameValueCollection GetCookieValues(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] == null)
            {
                return null;
            }

            try
            {
                return HttpContext.Current.Request.Cookies[cookieName].Values;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}