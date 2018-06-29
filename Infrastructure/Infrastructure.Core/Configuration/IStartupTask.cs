namespace Nagnoi.SiC.Infrastructure.Core.Configuration
{
    /// <summary>
    /// Represents a startup task interface
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        /// Gets the order task
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Executes a task
        /// </summary>
        void Execute();
    }
}