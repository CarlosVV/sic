namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    #region Imports

    using System;
    using System.IO;

    #endregion

    /// <summary>
    /// Represents a storage file
    /// </summary>
    public interface IStorageFile
    {
        /// <summary>
        /// Gets the file path
        /// </summary>
        /// <returns>Returns the file path</returns>
        string GetPath();

        /// <summary>
        /// Gets the file name
        /// </summary>
        /// <returns>Returns the file name</returns>
        string GetName();

        /// <summary>
        /// Gets the file size
        /// </summary>
        /// <returns>Returns the file size</returns>
        long GetSize();

        /// <summary>
        /// Gets the last updated
        /// </summary>
        /// <returns>Returns the last updated</returns>
        DateTime GetLastUpdated();

        /// <summary>
        /// Gets the file type
        /// </summary>
        /// <returns>Returns the file type</returns>
        string GetFileType();

        /// <summary>
        /// Creates a stream for reading from the file.
        /// </summary>
        /// <returns>Returns the stream file</returns>
        Stream OpenRead();

        /// <summary>
        /// Creates a stream for writing to the file.
        /// </summary>
        /// <returns>Returns the stream file</returns>
        Stream OpenWrite();

        /// <summary>
        /// Gets the file contents
        /// </summary>
        /// <returns>Returns a bytes array</returns>
        byte[] GetContent();
    }
}