namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    #region Imports

    using System;
    using System.IO;

    #endregion

    /// <summary>
    /// Represents the implementation of storage folder
    /// </summary>
    public sealed class FileSystemStorageFolder : IStorageFolder
    {
        #region Private Members

        /// <summary>
        /// File path
        /// </summary>
        private readonly string path;

        /// <summary>
        /// Directory info instance
        /// </summary>
        private readonly DirectoryInfo directoryInfo;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemStorageFolder"/> class.
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="directoryInfo">Directory info</param>
        public FileSystemStorageFolder(string path, DirectoryInfo directoryInfo)
        {
            this.path = path;
            this.directoryInfo = directoryInfo;
        }

        #endregion

        #region Implementation of IStorageFolder

        /// <summary>
        /// Gets the folder path
        /// </summary>
        /// <returns>Returns the folder path</returns>
        public string GetPath()
        {
            return this.path;
        }

        /// <summary>
        /// Gets the folder name
        /// </summary>
        /// <returns>Returns the folder name</returns>
        public string GetName()
        {
            return this.directoryInfo.Name;
        }

        /// <summary>
        /// Gets the last updated
        /// </summary>
        /// <returns>Returns the last updated</returns>
        public DateTime GetLastUpdated()
        {
            return this.directoryInfo.LastWriteTime;
        }

        /// <summary>
        /// Gets the folder size
        /// </summary>
        /// <returns>Returns the folder size</returns>
        public long GetSize()
        {
            return GetDirectorySize(this.directoryInfo);
        }

        /// <summary>
        /// Gets the parent folder
        /// </summary>
        /// <returns>Returns an instance of storage folder</returns>
        public IStorageFolder GetParent()
        {
            if (this.directoryInfo.Parent != null)
            {
                return new FileSystemStorageFolder(Path.GetDirectoryName(this.path), this.directoryInfo.Parent);
            }

            throw new ArgumentException("Directory " + this.directoryInfo.Name + " does not have a parent directory");
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Gets the directory size
        /// </summary>
        /// <param name="directoryInfo">DirectoryInfo instance</param>
        /// <returns>Returns the directory size</returns>
        private static long GetDirectorySize(DirectoryInfo directoryInfo)
        {
            long size = 0;

            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                if (!IsHidden(fileInfo))
                {
                    size += fileInfo.Length;
                }
            }

            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            foreach (DirectoryInfo directoryInfoFound in directoryInfos)
            {
                if (!IsHidden(directoryInfoFound))
                {
                    size += GetDirectorySize(directoryInfoFound);
                }
            }

            return size;
        }

        /// <summary>
        /// Determines whether the file is hidden
        /// </summary>
        /// <param name="di">File system info</param>
        /// <returns>Returns true or false</returns>
        private static bool IsHidden(FileSystemInfo di)
        {
            return (di.Attributes & FileAttributes.Hidden) != 0;
        }

        #endregion
    }
}