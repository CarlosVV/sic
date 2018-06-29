namespace Nagnoi.SiC.Infrastructure.Core.Configuration
{
    /// <summary>
    /// Represents a startup task of specific event
    /// </summary>
    public interface IEventStartupTask : IStartupTask
    {
        #region Properties

        /// <summary>
        /// Gets or sets the event identifier
        /// </summary>
        int EventId { get; set; }

        #endregion
    }
}