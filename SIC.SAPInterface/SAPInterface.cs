using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Nagnoi.SiC.Infrastructure.Core.Configuration;
using Nagnoi.SiC.Infrastructure.Core.Dependencies;
using Nagnoi.SiC.Infrastructure.Web;
using Nagnoi.SiC.Domain.Core.Model;
using Nagnoi.SiC.Domain.Core.Services;

namespace SIC.SAPInterface {
    public partial class SAPInterface: ServiceBase {

        public static System.Timers.Timer SAPInterfaceTimer = new System.Timers.Timer();

        public SAPInterface() {
            InitializeComponent();
        }

        public ISettingService SettingService {
            get { return IoC.Resolve<ISettingService>(); }
        }

        protected override void OnStart(string[] args) {            

            IoC.InitializeWith(new FactoryDependencyManager());

            EngineContext.Initialize(false);          
            DependencyConfig.Register();            

            SAPInterfaceTimer.Elapsed += SapInterface_Elapsed;
            SAPInterfaceTimer.AutoReset = false;
            SAPInterfaceTimer.Interval = SettingService.GetSettingValueInteger("SAPInterface.TimerDuration");
            SAPInterfaceTimer.Enabled = true;
        }

        protected override void OnStop() {
        }

        private void SapInterface_Elapsed(System.Object sender, System.Timers.ElapsedEventArgs e) {



            //SAPInterfaceTimer.Interval = SettingService.GetSettingValueInteger("SAPInterface.TimerDuration");
        }
    }
}
