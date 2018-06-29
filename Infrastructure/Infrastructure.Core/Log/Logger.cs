namespace Nagnoi.SiC.Infrastructure.Core{
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Logger{
        private static readonly ILog Log = LogManager.GetLogger(typeof(Logger));
        private const string SourceIdIdReference = "SourceId";
        public static readonly int SourceId;

        public static ILog LoggingInstance {
            get { return Log; }
        }

        static Logger() {
            SourceId = 1;
            GlobalContext.Properties[SourceIdIdReference] = SourceId;
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
