namespace Nagnoi.SiC.Web
{
    #region References

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Infrastructure.Core.Configuration;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Web;
    using Infrastructure.Web.UI;

    #endregion

    public class MvcApplication : HttpApplication
    {
        #region Private Methods

        private void WriteLoggingShutdown()
        {
            HttpRuntime runtime = (HttpRuntime)typeof(HttpRuntime).InvokeMember(
                                                                                    "_theRuntime",
                                                                                    BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField,
                                                                                    null,
                                                                                    null,
                                                                                    null);

            if (runtime == null)
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

        private void ChangeCultureInfo()
        {
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            customCulture.DateTimeFormat.DateSeparator = "/";

            Thread.CurrentThread.CurrentCulture = customCulture;
        }

        #endregion

        #region Handlers

        protected void Application_Start()
        {
            IoC.InitializeWith(new FactoryDependencyManager());

            EngineContext.Initialize(false);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyConfig.Register();

            MvcHandler.DisableMvcResponseHeader = true;

            //ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            //ValueProviderFactories.Factories.Add(new JsonNetValueProviderFactory());
        }

        protected void Application_End(object sender, EventArgs e)
        {
            WriteLoggingShutdown();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            ChangeCultureInfo();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            IoC.DisposeAndClearAll();

            HttpContext.Current.Response.Headers.Remove("Server");
        }

        #endregion
    }
}