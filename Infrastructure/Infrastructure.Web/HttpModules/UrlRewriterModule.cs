// -----------------------------------------------------------------------
// <copyright file="UrlRewriterModule.cs" company="Nagnoi">
// Copyright (c) Nagnoi. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Nagnoi.SiC.Infrastructure.Web.HttpModules
{
    #region Imports

    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.SessionState;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Model.Services.Application;
    using Nagnoi.SiC.Infrastructure.Core.Configuration;
    using Nagnoi.SiC.Infrastructure.Core.DependencyManagement;
    using Nagnoi.SiC.Infrastructure.Core.Helpers;

    #endregion

    /// <summary>
    /// Represents a http module to rewrite or redirect
    /// any request to login page for an event
    /// </summary>
    public class UrlRewriterModule : IHttpModule, IRequiresSessionState
    {
        #region Private Members

        /// <summary>
        /// Cookie name for event code
        /// </summary>
        public readonly string CookieNameEventCode = "event";

        /// <summary>
        /// Cookie expiration time
        /// </summary>
        public readonly TimeSpan CookieExpirationTime = new TimeSpan(30, 0, 0, 0);

        /// <summary>
        /// The system event selected through the cookie
        /// </summary>
        private SystemEvent systemEventSelected = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Disposes a http module
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose
        }

        /// <summary>
        /// Initializes a http module
        /// </summary>
        /// <param name="context">HTTP context</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.ContextBeginRequest);
            context.EndRequest += new EventHandler(this.ContextEndRequest);
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Occurs when a request has began
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void ContextBeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;

            HttpContext context = application.Context;

            string path = context.Request.Path;

            string url = context.Request.Url.AbsolutePath.ToLowerInvariant();

            if (path.Length == 0)
            {
                return;
            }

            if (url.Contains(SystemSettings.StatusPageName))
            {
                return;
            }

            if (WebHelper.IsStaticResource(context.Request))
            {
                return;
            }

            ISystemEventService systemEventFacade = IoC.Resolve<ISystemEventService>();

            var systemEvents = systemEventFacade.SelectAllEvents();

            if (!systemEvents.Any())
            {
                return;
            }

            var matchingEvents = systemEvents
                                    .Where(systemEvent => url.Contains(string.Format("/{0}", systemEvent.Code.ToLowerInvariant())));

            if (matchingEvents.Any())
            {
                this.systemEventSelected = matchingEvents.First();

                CookieHelper.SetCookie(this.CookieNameEventCode, this.systemEventSelected.Code, this.CookieExpirationTime);

                context.RewritePath("~/SignIn.aspx", false);
            }
            else if (url.ToLowerInvariant().Contains("signin.aspx"))
            {
                SystemEvent systemEventFirst = systemEvents.First();

                string currentEventCode = systemEventFirst.Code;

                string cookieValue = CookieHelper.GetCookieString(this.CookieNameEventCode, true);

                if (!string.IsNullOrEmpty(cookieValue))
                {
                    currentEventCode = HttpUtility.HtmlEncode(cookieValue);
                }

                context.Response.Redirect(currentEventCode);
            }
        }

        /// <summary>
        /// Occurs when a request has ended
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void ContextEndRequest(object sender, EventArgs e)
        {
            string redirectUrl = HttpContext.Current.Response.RedirectLocation;
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                HttpContext.Current.Response.RedirectLocation = Regex.Replace(redirectUrl, "ReturnUrl=(?'url'.*)", delegate(Match m)
                {
                    string url = HttpUtility.UrlDecode(m.Groups["url"].Value);
                    Uri u = new Uri(HttpContext.Current.Request.Url, url);
                    return string.Format("ReturnUrl={0}", HttpUtility.UrlEncode(u.ToString()));
                }, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            }
        }

        #endregion
    }
}