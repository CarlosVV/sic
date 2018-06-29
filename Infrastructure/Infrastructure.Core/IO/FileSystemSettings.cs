namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    #region Imports

    using Nagnoi.SiC.Infrastructure.Core.Configuration;

    #endregion

    /// <summary>
    /// Represents the settings for file system
    /// </summary>
    public class FileSystemSettings : ISettings
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemSettings"/> class.
        /// </summary>
        public FileSystemSettings()
        {
            this.DirectoryName = "content";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the directory name
        /// </summary>
        public string DirectoryName { get; private set; }

        #endregion
    }
}