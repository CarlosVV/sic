using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Nagnoi.SiC.Infrastructure.Core.Configuration;
using Nagnoi.SiC.Infrastructure.Core.Dependencies;
using Nagnoi.SiC.Infrastructure.Web;

namespace SIC.SAPInterface {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new SAPInterface() 
            };

            IoC.InitializeWith(new FactoryDependencyManager());
            EngineContext.Initialize(false);

            DependencyConfig.Register();

            ServiceBase.Run(ServicesToRun);
        }
    }
}
