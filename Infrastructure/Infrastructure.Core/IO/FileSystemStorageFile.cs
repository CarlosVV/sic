namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    #region Imports

    using System;
    using System.IO;

    #endregion

    /// <summary>
    /// Represents the implementation of storage file
    /// </summary>
    public sealed class FileSystemStorageFile : IStorageFile
    {
        #region Private Members

        /// <summary>
        /// File path
        /// </summary>
        private readonly string path;

        /// <summary>
        /// File info instance
        /// </summary>
        private readonly FileInfo fileInfo;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemStorageFile"/> class.
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="fileInfo">File info</param>
        public FileSystemStorageFile(string path, FileInfo fileInfo)
        {
            this.path = path;
            this.fileInfo = fileInfo;
        }

        #endregion

        #region Implementation of IStorageFile

        /// <summary>
        /// Gets the file path
        /// </summary>
        /// <returns>Returns the file path</returns>
        public string GetPath()
        {
            return this.path;
        }

        /// <summary>
        /// Gets the file name
        /// </summary>
        /// <returns>Returns the file name</returns>
        public string GetName()
        {
            return this.fileInfo.Name;
        }

        /// <summary>
        /// Gets the file size
        /// </summary>
        /// <returns>Returns the file size</returns>
        public long GetSize()
        {
            return this.fileInfo.Length;
        }

        /// <summary>
        /// Gets the last updated
        /// </summary>
        /// <returns>Returns the last updated</returns>
        public DateTime GetLastUpdated()
        {
            return this.fileInfo.LastWriteTime;
        }

        /// <summary>
        /// Gets the file type
        /// </summary>
        /// <returns>Returns the file type</returns>
        public string GetFileType()
        {
            return this.fileInfo.Extension;
        }

        /// <summary>
        /// Creates a stream for reading from the file.
        /// </summary>
        /// <returns>Returns the stream file</returns>
        public Stream OpenRead()
        {
            return new FileStream(this.fileInfo.FullName, FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// Creates a stream for writing to the file.
        /// </summary>
        /// <returns>Returns the stream file</returns>
        public Stream OpenWrite()
        {
            return new FileStream(this.fileInfo.FullName, FileMode.Open, FileAccess.ReadWrite);
        }

        /// <summary>
        /// Gets the file contents
        /// </summary>
        /// <returns>Returns a bytes array</returns>
        public byte[] GetContent()
        {
            return File.ReadAllBytes(this.fileInfo.FullName);
        }

        #endregion
    }
}