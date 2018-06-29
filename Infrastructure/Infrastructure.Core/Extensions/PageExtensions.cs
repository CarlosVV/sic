namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Web;
    using System.Web.Caching;
    using System.Web.UI;

    #endregion

    /// <summary>
    /// Page extension methods.
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        /// Resolve the relative url of a static resource
        /// </summary>
        /// <param name="page">Web page</param>
        /// <param name="relativeUrl">URL relative</param>
        /// <param name="useLastModifiedDate">A item indicating whether must use the last modified date of file</param>
        /// <returns>Returns the URL aware</returns>
        public static string CacheAwareResolveUrl(this Page page, string relativeUrl, bool useLastModifiedDate = true)
        {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);

            if (context.IsDebuggingEnabled)
            {
                return WebHelper.ResolveUrl(relativeUrl);
            }

            string path = page.ResolveUrl(relativeUrl);
            string physicalPath = context.Server.MapPath(relativeUrl);

            string version;
            if (context.Cache[physicalPath] == null)
            {
                if (useLastModifiedDate)
                {
                    version = GetFileLastModifiedDate(context, physicalPath);
                }
                else
                {
                    version = GetMD5FileHash(context, physicalPath);
                }

                context.Cache.Insert(physicalPath, version, new CacheDependency(physicalPath));
            }
            else
            {
                version = context.Cache[physicalPath].ToString();
            }

            return string.Format("{0}?v={1}", path, version);
        }

        /// <summary>
        /// Get the MD5 file hash
        /// </summary>
        /// <param name="context">HttpContext instance</param>
        /// <param name="physicalPath">Physical path</param>
        /// <returns>Returns a hash string</returns>
        private static string GetMD5FileHash(HttpContextBase context, string physicalPath)
        {
            byte[] hash = MD5.Create().ComputeHash(File.ReadAllBytes(physicalPath));

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        /// <summary>
        /// Get the last modified date of any file
        /// </summary>
        /// <param name="context">HttpContext instance</param>
        /// <param name="physicalPath">Physical path</param>
        /// <returns>Returns a date with format yyyyMMddhhmmss</returns>
        private static string GetFileLastModifiedDate(HttpContextBase context, string physicalPath)
        {
            return new FileInfo(physicalPath).LastWriteTime.ToString("yyyyMMddhhmmss");
        }
    }
}