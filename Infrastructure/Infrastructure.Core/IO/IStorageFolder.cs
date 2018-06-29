namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    #region Imports

    using System;

    #endregion

    /// <summary>
    /// Represents a storage folder
    /// </summary>
    public interface IStorageFolder
    {
        /// <summary>
        /// Gets the folder path
        /// </summary>
        /// <returns>Returns the folder path</returns>
        string GetPath();

        /// <summary>
        /// Gets the folder name
        /// </summary>
        /// <returns>Returns the folder name</returns>
        string GetName();

        /// <summary>
        /// Gets the folder size
        /// </summary>
        /// <returns>Returns the folder size</returns>
        long GetSize();

        /// <summary>
        /// Gets the last updated
        /// </summary>
        /// <returns>Returns the last updated</returns>
        DateTime GetLastUpdated();

        /// <summary>
        /// Gets the parent folder
        /// </summary>
        /// <returns>Returns an instance of storage folder</returns>
        IStorageFolder GetParent();
    }
}