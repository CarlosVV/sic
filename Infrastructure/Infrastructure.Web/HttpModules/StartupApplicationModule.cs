// -----------------------------------------------------------------------
// <copyright file="StartupApplicationModule.cs" company="Nagnoi">
// Copyright (c) Nagnoi. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Nagnoi.SiC.Infrastructure.Web.HttpModules
{
    #region Imports

    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Services;
    //using Nagnoi.SiC.Domain.Core.Model.Services.Application;
    //using Nagnoi.SiC.Infrastructure.Core.Comm;
    using Nagnoi.SiC.Infrastructure.Core.Configuration;
    using Nagnoi.SiC.Infrastructure.Core.Dependencies;
    using Nagnoi.SiC.Infrastructure.Core.Helpers;
    using Nagnoi.SiC.Infrastructure.Core.Log;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Web;

    #endregion

    /// <summary>
    /// Abstract class for Accommodation application
    /// </summary>
    public abstract class StartupApplicationModule : HttpApplication
    {
        #region Properties

        /// <summary>
        /// Gets the URL maintenance page
        /// </summary>
        protected virtual string MaintenancePageUrl
        {
            get
            {
                return "~/Maintenance.aspx";
            }
        }

        /// <summary>
        /// Gets the work context
        /// </summary>
        protected IWorkContext WorkContext
        {
            get { return IoC.Resolve<IWorkContext>(); }
        }

        /// <summary>
        /// Gets the setting service
        /// </summary>
        protected ISettingService SettingService
        {
            get { return IoC.Resolve<ISettingService>(); }
        }

        /// <summary>
        /// Gets the language service
        /// </summary>
        protected ILanguageService LanguageService
        {
            get { return IoC.Resolve<ILanguageService>(); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the session builder
        /// </summary>
        /// <returns>CurrentSession builder base</returns>
        protected abstract ByromSessionBuilderBase GetSessionBuilder();

        /// <summary>
        /// Performs the first steps to initialize application
        /// </summary>
        protected abstract void RegisterDependencyEngines();

        /// <summary>
        /// Logs an exception
        /// </summary>
        /// <param name="ex">Exception instance</param>
        protected void LogException(Exception ex)
        {
            if (ex.IsNull())
            {
                return;
            }
        }

        /// <summary>
        /// Handles the maintenance mode for all requests
        /// </summary>
        protected virtual void HandleMaintenanceMode()
        {
            if (SystemSettings.MaintenanceMode)
            {
                if (SystemSettings.ByPassUrl.Contains(WebHelper.GetUserHostAddress()) ||
                    Request.IsLocal)
                {
                    // don't nothing
                }
                else
                {
                    HttpContext.Current.RewritePath(this.MaintenancePageUrl);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether current request is the status page
        /// </summary>
        protected virtual bool IsStatusPage(HttpRequest request)
        {
            string requestUrl = request.Url.AbsolutePath.ToLowerInvariant();

            return !SystemSettings.StatusPageName.IsNullOrEmpty() && requestUrl.Contains(SystemSettings.StatusPageName.ToLowerInvariant());
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Handles the Start event of the application
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            // Set the dependency resolver
            IoC.InitializeWith(new DependencyResolverFactory());

            // Register the dependency engines
            this.RegisterDependencyEngines();

            // Initialize the engine context and run the startup tasks
            EngineContext.Initialize(false);

            LogFactory.Configure();

            // Get the current instance of CurrentSession Builder
            ByromSessionBuilderBase builder = this.GetSessionBuilder();

            // Save login framework into Application variable
            HttpContext.Current.Application[WorkContextKeys.SessionBuilderKey] = builder;

            if (SystemSettings.MaintenanceMode)
            {
                SystemApplication fakeSystemApplication = new SystemApplication()
                {
                    ApplicationId = SystemSettings.ApplicationId
                };

                HttpContext.Current.Application[WorkContextKeys.SystemApplicationKey] = fakeSystemApplication;
            }
            else
            {
                HttpContext.Current.Application[WorkContextKeys.SystemApplicationKey] = IoC.Resolve<ISettingService>().Application;
            }
        }

        /// <summary>
        /// Handles the End event of the application
        /// </summary>
        protected virtual void Application_End()
        {
            HttpRuntime runtime = (HttpRuntime)typeof(HttpRuntime).InvokeMember(
                                                                                    "_theRuntime",
                                                                                    BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField,
                                                                                    null,
                                                                                    null,
                                                                                    null);

            if (runtime.IsNull())
            {
                return;
            }

            string shutDownMessage = (string)runtime.GetType().InvokeMember(
                                                                            "_shutDownMessage",
                                                                             BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                                                                             null,
                                                                             runtime,
                                                                             null);

            string shutDownStack = (string)runtime.GetType().InvokeMember(
                                                                           "_shutDownStack",
                                                                           BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                                                                           null,
                                                                           runtime,
                                                                           null);

            if (!EventLog.SourceExists(".NET Runtime"))
            {
                EventLog.CreateEventSource(".NET Runtime", "Application");
            }

            EventLog log = new EventLog();
            log.Source = ".NET Runtime";
            log.WriteEntry(
                            string.Format(
                                            "\r\n\r\n_shutDownMessage={0}\r\n\r\n_shutDownStack={1}",
                                            shutDownMessage,
                                            shutDownStack),
                            EventLogEntryType.Error);
        }

        /// <summary>
        /// Handles the Begin Request event of the application
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            if (WebHelper.IsStaticResource(this.Request))
            {
                return;
            }

            if (this.IsStatusPage(this.Request))
            {
                return;
            }

            this.HandleMaintenanceMode();
        }

        /// <summary>
        /// Handles the Authenticate Request event of the application
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (SystemSettings.MaintenanceMode)
            {
                return;
            }
        }

        /// <summary>
        /// Handles the Error event of the Application
        /// Logs the application error to database
        /// </summary>
        /// <param name="sender">the event Sender</param>
        /// <param name="e">the event Arguments</param>
        protected virtual void Application_Error(object sender, EventArgs e)
        {
            if (SystemSettings.MaintenanceMode)
            {
                return;
            }

            LogException(Server.GetLastError());
        }

        /// <summary>
        /// Handles the End Request event of the application
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {
            var context = (HttpApplication)sender;
            var response = context.Response;

            if (response.StatusCode == 302 && WebHelper.IsAjaxRequest())
            {
                response.TrySkipIisCustomErrors = true;
                response.ClearContent();
                response.StatusCode = 401;
                response.RedirectLocation = null;
            }

            IoC.ReleaseAndDisposedAllHttpScopedObjects();
        }

        #endregion
    }
}