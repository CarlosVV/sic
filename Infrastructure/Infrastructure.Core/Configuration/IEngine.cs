namespace Nagnoi.SiC.Infrastructure.Core.Configuration
{
    /// <summary>
    /// Classes implementing this interface can serve as a portal for the 
    /// various services composing the system engine. Edit functionality, modules
    /// and implementations access most a functionality through this 
    /// interface.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Initializes components and plugins in the system environment
        /// </summary>
        void Initialize();

        /// <summary>
        /// Runs the startup tasks
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        void RunStartupTasks<T>() where T : IStartupTask;

        /// <summary>
        /// Runs the startup tasks  of specific event
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="eventId">Event identifier</param>
        void RunEventStartTasks<T>(int eventId) where T : IEventStartupTask;
    }
}