namespace Nagnoi.SiC.Infrastructure.Core.Configuration
{
    #region Imports

    using System.Linq;
    using Nagnoi.SiC.Infrastructure.Core.Dependencies;

    #endregion

    /// <summary>
    /// Represents the implementation of IEngine interface
    /// </summary>
    public class SystemEngine : IEngine
    {
        #region Public Methods

        /// <summary>
        /// Initializes components and plugins in the system environment
        /// </summary>
        public void Initialize()
        {
            this.RunStartupTasks<IStartupTask>();
        }

        /// <summary>
        /// Runs the startup tasks
        /// </summary>
        /// <typeparam name="T">Any interface Startup Task</typeparam>
        public void RunStartupTasks<T>() where T : IStartupTask
        {
            var startUpTasks = IoC.ResolveAll<T>();

            startUpTasks = startUpTasks.OrderBy(st => st.Order);
            foreach (var startUpTask in startUpTasks)
            {
                startUpTask.Execute();
            }
        }

        /// <summary>
        /// Runs the startup tasks  of specific event
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="eventId">Event identifier</param>
        public void RunEventStartTasks<T>(int eventId) where T : IEventStartupTask
        {
            var startUpTasks = IoC.ResolveAll<T>();

            startUpTasks = startUpTasks.OrderBy(st => st.Order);
            foreach (var startUpTask in startUpTasks)
            {
                startUpTask.EventId = eventId;
                startUpTask.Execute();
            }
        }

        #endregion
    }
}