namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    #region Imports

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a storage provider
    /// </summary>
    public interface IStorageProvider
    {
        #region Common

        /// <summary>
        /// Gets the public URL
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns the public URL</returns>
        string GetPublicUrl(string path);

        /// <summary>
        /// Maps a path
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns the path</returns>
        string Map(string path);

        /// <summary>
        /// Maps a path
        /// </summary>
        /// <param name="paths">Array paths</param>
        /// <returns>Returns the path</returns>
        string Map(params string[] paths);

        #endregion

        #region Files

        /// <summary>
        /// Get a storage file
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns an instance of storage file</returns>
        IStorageFile GetFile(string path);

        /// <summary>
        /// Deletes a storage file
        /// </summary>
        /// <param name="path">File path</param>
        void DeleteFile(string path);

        /// <summary>
        /// Renames a storage file
        /// </summary>
        /// <param name="path">Source path</param>
        /// <param name="newPath">New path</param>
        void RenameFile(string path, string newPath);

        /// <summary>
        /// Creates a new storage file
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns an instance of new storage file</returns>
        IStorageFile CreateFile(string path);

        /// <summary>
        /// Creates a new storage file
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="contents">File contents</param>
        /// <returns>Returns an instance of new storage file</returns>
        IStorageFile CreateFile(string path, byte[] contents);

        /// <summary>
        /// Gets a item indicating whether the file exists or not
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns true or false</returns>
        bool FileExists(string path);

        /// <summary>
        /// Gets a list of storage files
        /// </summary>
        /// <param name="path">Target path</param>
        /// <returns>Returns the list of storage files</returns>
        IEnumerable<IStorageFile> ListFiles(string path);

        /// <summary>
        /// Gets a list of storage files
        /// </summary>
        /// <param name="path">Target path</param>
        /// <param name="searchPattern">Search pattern</param>
        /// <returns>Returns the list of storage files</returns>
        IEnumerable<IStorageFile> ListFiles(string path, string searchPattern);

        #endregion

        #region Directories

        /// <summary>
        /// Creates a storage folder
        /// </summary>
        /// <param name="path">Folder path</param>
        void CreateFolder(string path);

        /// <summary>
        /// Deletes a storage folder
        /// </summary>
        /// <param name="path">Folder path</param>
        void DeleteFolder(string path);

        /// <summary>
        /// Renames a storage folder
        /// </summary>
        /// <param name="path">Source path</param>
        /// <param name="newPath">New path</param>
        void RenameFolder(string path, string newPath);

        /// <summary>
        /// Gets a item indicating whether folder exists or not
        /// </summary>
        /// <param name="path">Folder path</param>
        /// <returns>Returns true or false</returns>
        bool FolderExists(string path);

        /// <summary>
        /// Gets a list of storage folders
        /// </summary>
        /// <param name="path">Target path</param>
        /// <returns>Returns the list of storage folders</returns>
        IEnumerable<IStorageFolder> ListFolders(string path);

        #endregion
    }
}