namespace Nagnoi.SiC.Infrastructure.Core.Log
{
    /// <summary>
    /// Represents the Log Factory for logging providers
    /// </summary>
    public static class LogFactory
    {
        #region Public Methods

        /// <summary>
        /// Configures the LogFactory
        /// </summary>
        public static void Configure()
        {
            Log4NetProvider.Configure();
        }

        #endregion
    }
}