namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    #region Imports

    using Nagnoi.SiC.Infrastructure.Core.Helpers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Hosting;

    #endregion

    /// <summary>
    /// Represents the implementation of storage provider
    /// </summary>
    public sealed class FileSystemStorageProvider : IStorageProvider
    {
        #region Private Members

        /// <summary>
        /// Storage path
        /// </summary>
        private readonly string storagePath;

        /// <summary>
        /// Public path
        /// </summary>
        private readonly string publicPath;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemStorageProvider"/> class.
        /// </summary>
        public FileSystemStorageProvider(FileSystemSettings settings) : this(settings.DirectoryName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemStorageProvider"/> class.
        /// </summary>
        /// <param name="directoryName">Directory name</param>
        public FileSystemStorageProvider(string directoryName)
        {
            string rootPath = WebHelper.MapPath("~");

            this.storagePath = Path.Combine(rootPath, directoryName).ToLowerInvariant();

            StringBuilder appPath = new StringBuilder();
            if (HostingEnvironment.IsHosted)
            {
                appPath.Append(HostingEnvironment.ApplicationVirtualPath);
            }

            if (!appPath.ToString().EndsWith("/"))
            {
                appPath.Append('/');
            }

            if (!appPath.ToString().StartsWith("/"))
            {
                appPath.Insert(0, '/');
            }

            appPath.Append(directoryName);
            appPath.Append('/');

            this.publicPath = appPath.ToString().ToLowerInvariant();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the public URL
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns the public URL</returns>
        public string GetPublicUrl(string path)
        {
            return this.publicPath + path.Replace(Path.DirectorySeparatorChar, '/');
        }

        /// <summary>
        /// Get a storage file
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns an instance of storage file</returns>
        public IStorageFile GetFile(string path)
        {
            if (!File.Exists(this.Map(path)))
            {
                throw new ArgumentException(string.Format("File {0} does not exist", path));
            }

            return new FileSystemStorageFile(Fix(path), new FileInfo(this.Map(path)));
        }

        /// <summary>
        /// Gets a list of storage files
        /// </summary>
        /// <param name="path">Target path</param>
        /// <returns>Returns the list of storage files</returns>
        public IEnumerable<IStorageFile> ListFiles(string path)
        {
            return this.ListFiles(path, string.Empty);
        }

        /// <summary>
        /// Gets a list of storage files
        /// </summary>
        /// <param name="path">Target path</param>
        /// <param name="searchPattern">Search pattern</param>
        /// <returns>Returns the list of storage files</returns>
        public IEnumerable<IStorageFile> ListFiles(string path, string searchPattern)
        {
            if (!Directory.Exists(this.Map(path)))
            {
                throw new ArgumentException(string.Format("Directory {0} does not exist", path));
            }

            if (string.IsNullOrEmpty(searchPattern))
            {
                return new DirectoryInfo(this.Map(path)).GetFiles()
                                                        .Where(fi => !IsHidden(fi))
                                                        .Select<FileInfo, IStorageFile>(fi => new FileSystemStorageFile(Path.Combine(Fix(path), fi.Name), fi));
            }
            else
            {
                return new DirectoryInfo(this.Map(path)).GetFiles(searchPattern)
                                                        .Where(fi => !IsHidden(fi))
                                                        .Select<FileInfo, IStorageFile>(fi => new FileSystemStorageFile(Path.Combine(Fix(path), fi.Name), fi));
            }
        }

        /// <summary>
        /// Gets a list of storage folders
        /// </summary>
        /// <param name="path">Target path</param>
        /// <returns>Returns the list of storage folders</returns>
        public IEnumerable<IStorageFolder> ListFolders(string path)
        {
            if (!Directory.Exists(this.Map(path)))
            {
                try
                {
                    Directory.CreateDirectory(this.Map(path));
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(string.Format("The folder could not be created at path: {0}. {1}", path, ex));
                }
            }

            return new DirectoryInfo(this.Map(path)).GetDirectories()
                                                    .Where(di => !IsHidden(di))
                                                    .Select<DirectoryInfo, IStorageFolder>(di => new FileSystemStorageFolder(Path.Combine(Fix(path), di.Name), di));
        }

        /// <summary>
        /// Creates a storage folder
        /// </summary>
        /// <param name="path">Folder path</param>
        public void CreateFolder(string path)
        {
            if (Directory.Exists(this.Map(path)))
            {
                throw new ArgumentException(string.Format("Directory {0} already exists", path));
            }

            Directory.CreateDirectory(Fix(this.Map(path)));
        }

        /// <summary>
        /// Deletes a storage folder
        /// </summary>
        /// <param name="path">Folder path</param>
        public void DeleteFolder(string path)
        {
            if (!Directory.Exists(this.Map(path)))
            {
                throw new ArgumentException(string.Format("Directory {0} does not exist", path));
            }

            Directory.Delete(this.Map(path), true);
        }

        /// <summary>
        /// Renames a storage folder
        /// </summary>
        /// <param name="path">Source path</param>
        /// <param name="newPath">New path</param>
        public void RenameFolder(string path, string newPath)
        {
            if (!Directory.Exists(this.Map(path)))
            {
                throw new ArgumentException(string.Format("Directory {0} does not exist", path));
            }

            if (Directory.Exists(this.Map(newPath)))
            {
                throw new ArgumentException(string.Format("Directory {0} already exists", newPath));
            }

            Directory.Move(this.Map(path), this.Map(newPath));
        }

        /// <summary>
        /// Creates a new storage file
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns an instance of new storage file</returns>
        public IStorageFile CreateFile(string path)
        {
            return this.CreateFile(path, new byte[0]);
        }

        /// <summary>
        /// Creates a new storage file
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="contents">File contents</param>
        /// <returns>Returns an instance of new storage file</returns>
        public IStorageFile CreateFile(string path, byte[] contents)
        {
            if (File.Exists(this.Map(path)))
            {
                throw new ArgumentException(string.Format("File {0} already exists", path));
            }

            FileInfo fileInfo = new FileInfo(this.Map(path));

            File.WriteAllBytes(this.Map(path), contents);

            return new FileSystemStorageFile(Fix(path), fileInfo);
        }

        /// <summary>
        /// Deletes a storage file
        /// </summary>
        /// <param name="path">File path</param>
        public void DeleteFile(string path)
        {
            if (!File.Exists(this.Map(path)))
            {
                throw new ArgumentException(string.Format("File {0} does not exist", path));
            }

            File.Delete(this.Map(path));
        }

        /// <summary>
        /// Renames a storage file
        /// </summary>
        /// <param name="path">Source path</param>
        /// <param name="newPath">New path</param>
        public void RenameFile(string path, string newPath)
        {
            if (!File.Exists(this.Map(path)))
            {
                throw new ArgumentException(string.Format("File {0} does not exist", path));
            }

            if (File.Exists(this.Map(newPath)))
            {
                throw new ArgumentException(string.Format("File {0} already exists", newPath));
            }

            File.Move(this.Map(path), this.Map(newPath));
        }

        /// <summary>
        /// Gets a item indicating whether the file exists or not
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns true or false</returns>
        public bool FileExists(string path)
        {
            return File.Exists(this.Map(path));
        }

        /// <summary>
        /// Gets a item indicating whether folder exists or not
        /// </summary>
        /// <param name="path">Folder path</param>
        /// <returns>Returns true or false</returns>
        public bool FolderExists(string path)
        {
            return Directory.Exists(Fix(this.Map(path)));
        }

        /// <summary>
        /// Maps a path
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns the path</returns>
        public string Map(string path)
        {
            return string.IsNullOrEmpty(path) ? this.storagePath : Path.Combine(this.storagePath, path);
        }

        /// <summary>
        /// Maps a path
        /// </summary>
        /// <param name="paths">Array paths</param>
        /// <returns>Returns the path</returns>
        public string Map(params string[] paths)
        {
            string[] temp = new string[paths.Length + 1];

            temp.SetValue(this.storagePath, 0);

            paths.CopyTo(temp, 1);

            return paths.Length == 0 ? this.storagePath : Path.Combine(temp);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Fixes a file path
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Returns the fixed file path</returns>
        private static string Fix(string path)
        {
            return string.IsNullOrEmpty(path)
                       ? string.Empty
                       : Path.DirectorySeparatorChar != '/'
                             ? path.Replace('/', Path.DirectorySeparatorChar)
                             : path;
        }

        /// <summary>
        /// Gets a item indicating whether a file is hidden
        /// </summary>
        /// <param name="di">Directory info</param>
        /// <returns>Returns true or false</returns>
        private static bool IsHidden(FileSystemInfo di)
        {
            return (di.Attributes & FileAttributes.Hidden) != 0;
        }

        #endregion
    }
}