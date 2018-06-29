// -----------------------------------------------------------------------
// <copyright file="YSlowHttpModule.cs" company="Nagnoi">
// Copyright (c) Nagnoi. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Nagnoi.SiC.Infrastructure.Web.HttpModules
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    #endregion

    /// <summary>
    /// Represents the HttpModule for YSlow practices
    /// </summary>
    public class YSlowHttpModule : IHttpModule
    {
        #region Private Members

        /// <summary>
        /// List of HTTP Headers ignored
        /// </summary>
        private static readonly List<string> HeadersToRemove = new List<string> { "X-AspNet-Version", "X-AspNetMvc-Version", "ETag", "Server", };

        /// <summary>
        /// List of File extensions whose will be cached
        /// </summary>
        private static readonly List<string> LongCacheExtensions = new List<string> { ".js", ".css", ".png", ".jpg", ".gif", ".ico", };

        #endregion

        #region Properties

        /// <summary>
        /// Gets the cache duration
        /// </summary>
        private TimeSpan CacheDuration
        {
            get
            {
                //int cacheForTime = IoC.Resolve<ISettingFacade>().GetSettingValueInteger("Cache.YSlowHttpModule.TimeInDays", 365);

                return TimeSpan.FromDays(365);
            }
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module 
        /// that implements <see cref="T:System.Web.IHttpModule"></see>.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose; 
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> 
        /// that provides access to the methods, properties, and events common to 
        /// all application objects within an ASP.NET application.
        /// </param>
        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(this.ContextEndRequest);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the caching headers and monitors the If-None-Match request header,
        /// to save bandwidth and CPU time.
        /// </summary>
        /// <param name="context">HttpContext instance</param>
        private void SetCachingHeaders(HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetExpires(DateTime.Now.Add(this.CacheDuration));
            context.Response.Cache.SetValidUntilExpires(true);
            context.Response.Cache.SetNoServerCaching();
            context.Response.Cache.SetMaxAge(this.CacheDuration);
            context.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Handles the EndRequest event of the current context.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ContextEndRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            if (context == null)
            {
                return;
            }

            if (context.IsDebuggingEnabled)
            {
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                context.Response.Cache.SetValidUntilExpires(false);
                context.Response.Cache.SetNoServerCaching();
                context.Response.Cache.SetNoStore();

                return;
            }

            HeadersToRemove.ForEach(header => context.Response.Headers.Remove(header));

            string extension = Path.GetExtension(context.Request.Url.AbsolutePath).ToLowerInvariant();

            if (LongCacheExtensions.Contains(extension))
            {
                this.SetCachingHeaders(context);
            }
        }

        #endregion
    }
}