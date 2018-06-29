// -----------------------------------------------------------------------
// <copyright file="JsonHttpModule.cs" company="Nagnoi">
// Copyright (c) Nagnoi. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Nagnoi.SiC.Infrastructure.Web.HttpModules
{
    #region Imports

    using System;
    using System.Web;
    using Nagnoi.SiC.Infrastructure.Core.Helpers;

    #endregion

    /// <summary>
    /// Provides a initialization module to handle the JSONP requests
    /// </summary>
    public class JsonHttpModule : IHttpModule
    {
        #region Constants

        private const string JsonContentType = "application/json; charset=utf-8";

        #endregion

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.BeginRequest += this.OnBeginRequest;
            app.ReleaseRequestState += this.OnReleaseRequestState;
        }

        #endregion

        #region Private Methods

        private bool ApplyCurrentRequest(HttpRequest request)
        {
            if (!request.Url.AbsolutePath.Contains(".asmx"))
            {
                return false;
            }

            string formatQueryString = WebHelper.QueryString("format");
            if (string.IsNullOrEmpty(formatQueryString) && !formatQueryString.Equals("json"))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Handlers

        private void OnBeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;

            if (!this.ApplyCurrentRequest(app.Context.Request)) return;

            if (string.IsNullOrEmpty(app.Context.Request.ContentType))
            {
                app.Context.Request.ContentType = JsonContentType;
            }
        }
        
        private void OnReleaseRequestState(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;

            if (!this.ApplyCurrentRequest(app.Context.Request)) return;

            app.Context.Response.Filter = new JsonResponseFilter(app.Context.Response.Filter, app.Context);
        }

        #endregion
    }
}