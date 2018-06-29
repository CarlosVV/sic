// -----------------------------------------------------------------------
// <copyright file="AjaxAuthorizationModule.cs" company="Nagnoi">
// Copyright (c) Nagnoi. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Nagnoi.SiC.Infrastructure.Web.HttpModules
{
    #region Imports

    using System;
    using System.Web;

    #endregion

    /// <summary>
    /// Represents the Ajax Authorization module
    /// for all responses when the authorization fails
    /// </summary>
    public class AjaxAuthorizationModule : IHttpModule
    {
        #region Public Methods

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += this.CheckForAuthorizationFailure;
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks if the authorization fails for all responses to AJAX request.
        /// Changes the status code to 401
        /// </summary>
        /// <param name="request">HttpRequest instance</param>
        /// <param name="response">HttpResponse instance</param>
        /// <param name="context">HttpContext instance</param>
        internal void CheckForAuthFailure(HttpRequestBase request, HttpResponseBase response, HttpContextBase context)
        {
            if (true.Equals(context.Items["RequestWasNotAuthorized"]) &&
                this.IsAjaxRequest(request))
            {
                response.StatusCode = 401;
                response.ClearContent();
            }
        }

        /// <summary>
        /// Indicates whether a request is AJAX
        /// </summary>
        /// <param name="request">HttpRequest instance</param>
        /// <returns>Returns true or false</returns>
        internal bool IsAjaxRequest(HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Occurs before pre-sending the request headers
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void CheckForAuthorizationFailure(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var response = new HttpResponseWrapper(app.Response);
            var request = new HttpRequestWrapper(app.Request);
            var context = new HttpContextWrapper(app.Context);

            this.CheckForAuthFailure(request, response, context);
        }

        #endregion
    }
}