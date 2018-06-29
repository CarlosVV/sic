using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIC.SapInterfaceConsole.Modules;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using Nagnoi.SiC.Infrastructure.Core.Configuration;
using Nagnoi.SiC.Infrastructure.Core.Dependencies;
using Nagnoi.SiC.Infrastructure.Web;
using Nagnoi.SiC.Infrastructure.Web.UI;

namespace SIC.SapInterfaceConsole {
    class Program {
        static void Main(string[] args) {

            IoC.InitializeWith(new FactoryDependencyManager());
            DependencyConfig.Register();

            while (true) {

                BAPI newBapi = new BAPI();

                Console.WriteLine("Interface SAP Iniciada, buscando pagos......");

                newBapi.Init();

                System.Threading.Thread.Sleep(10000);
            }
        }
    }
}
