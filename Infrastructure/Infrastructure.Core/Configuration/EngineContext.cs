namespace Nagnoi.SiC.Infrastructure.Core.Configuration
{
    /// <summary>
    /// Provides access to the singleton instance of the system engine.
    /// </summary>
    public class EngineContext
    {
        #region Properties

        /// <summary>
        /// Gets the singleton engine used to access core services.
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }

                return Singleton<IEngine>.Instance;
            }
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Initializes a static instance of the objects factory.
        /// </summary>
        /// <param name="forceRecreate">Creates a new factory instance even though the factory has been previously initialized.</param>
        /// <returns>Returns an engine instance</returns>
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Singleton<IEngine>.Instance = new SystemEngine();

                Singleton<IEngine>.Instance.Initialize();
            }

            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.
        /// </summary>
        /// <param name="engine">The engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion
    }
}