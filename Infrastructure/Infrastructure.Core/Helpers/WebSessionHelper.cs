namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System.Web;

    #endregion

    /// <summary>
    /// Represents a set of methods to add, get or remove session variables
    /// </summary>
    public static class WebSessionHelper
    {
        /// <summary>
        /// Adds or updates a new session variable
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="key">Session key</param>
        /// <param name="item">Session item</param>
        /// <returns>Returns true if the session variable could be added</returns>
        public static bool Set<T>(string key, T value)
        {
            HttpContext current = HttpContext.Current;
            if (current.IsNull())
            {
                return false;
            }

            if (current.Session.IsNull())
            {
                return false;
            }

            current.Session[key] = value;

            return true;
        }

        /// <summary>
        /// Gets the item of session variable
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="key">Session key</param>
        /// <returns>Returns the item</returns>
        public static T Get<T>(string key)
        {
            HttpContext current = HttpContext.Current;
            if (current.IsNull())
            {
                return default(T);
            }

            if (current.Session.IsNull())
            {
                return default(T);
            }

            object value = current.Session[key];
            if (!value.IsNull())
            {
                return (T)value;
            }

            return default(T);
        }

        /// <summary>
        /// Removes a session variable
        /// </summary>
        /// <param name="key">Session key</param>
        /// <returns>Returns true if the session variable could be removed</returns>
        public static bool Remove(string key)
        {
            HttpContext current = HttpContext.Current;
            if (current.IsNull())
            {
                return false;
            }

            if (current.Session.IsNull())
            {
                return false;
            }

            current.Session.Remove(key);

            return true;
        }

        /// <summary>
        /// Gets the value of the session identifier
        /// </summary>
        /// <returns>Returns the session identifier</returns>
        public static string GetSessionId()
        {
            HttpContext current = HttpContext.Current;
            if (current.IsNull())
            {
                return null;
            }

            if (current.Session.IsNull())
            {
                return null;
            }

            return current.Session.SessionID;
        }
    }
}