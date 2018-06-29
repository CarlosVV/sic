namespace Nagnoi.SiC.Infrastructure.Core.Configuration
{
    #region Imports

    using System;
    using System.Configuration;

    #endregion

    /// <summary>
    /// Caching Section class
    /// </summary>
    public class CachingSection : ConfigurationSection
    {
        #region Properties

        /// <summary>
        /// Gets or sets the timespan of caching
        /// </summary>
        [ConfigurationProperty("CachingTimeSpan", IsRequired = true)]
        public TimeSpan CachingTimeSpan
        {
            get { return (TimeSpan)base["CachingTimeSpan"]; }
            set { base["CachingTimeSpan"] = value; }
        }

        /// <summary>
        /// Gets the file extension collection
        /// </summary>
        [ConfigurationProperty("FileExtensions", IsDefaultCollection = true, IsRequired = true)]
        public FileExtensionCollection FileExtensions
        {
            get { return (FileExtensionCollection)base["FileExtensions"]; }
        }

        #endregion
    }

    /// <summary>
    /// File Extension collection
    /// </summary>
    public class FileExtensionCollection : ConfigurationElementCollection
    {
        #region Properties

        /// <summary>
        /// Gets the collection type
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        /// <summary>
        /// Gets or sets the current scan of file extension
        /// </summary>
        /// <param name="index">Index scan</param>
        /// <returns>Returns an instance of file extension</returns>
        public FileExtension this[int index]
        {
            get 
            { 
                return (FileExtension)this.BaseGet(index); 
            }

            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }

                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Get or sets the current scan of file extension
        /// </summary>
        /// <param name="extension">Extension name</param>
        /// <returns>Returns an instance of file extension</returns>
        public new FileExtension this[string extension]
        {
            get 
            { 
                return (FileExtension)this.BaseGet(extension); 
            }

            set
            {
                if (this.BaseGet(extension) != null)
                {
                    this.BaseRemove(extension);
                }

                this.BaseAdd(value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new file extension
        /// </summary>
        /// <param name="element">A new element</param>
        public void Add(FileExtension element)
        {
            this.BaseAdd(element);
        }

        /// <summary>
        /// Clear all items
        /// </summary>
        public void Clear()
        {
            this.BaseClear();
        }

        /// <summary>
        /// Removes a file extension
        /// </summary>
        /// <param name="element">Element instance</param>
        public void Remove(FileExtension element)
        {
            this.BaseRemove(element.Extension);
        }

        /// <summary>
        /// Removes a file extension by name
        /// </summary>
        /// <param name="name">Element name</param>
        public void Remove(string name)
        {
            this.BaseRemove(name);
        }

        /// <summary>
        /// Removes a file extension by index
        /// </summary>
        /// <param name="index">Index element</param>
        public void RemoveAt(int index)
        {
            this.BaseRemoveAt(index);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a new element
        /// </summary>
        /// <returns>Return an instance of configuration element</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new FileExtension();
        }

        /// <summary>
        /// Gets an element by key
        /// </summary>
        /// <param name="element">Element instance</param>
        /// <returns>Returns an instance of file extension</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileExtension)element).Extension;
        }

        #endregion
    }

    /// <summary>
    /// File Extension class
    /// </summary>
    public class FileExtension : ConfigurationElement
    {
        #region Properties

        /// <summary>
        /// Gets or sets the file extension
        /// </summary>
        [ConfigurationProperty("Extension", IsRequired = true)]
        public string Extension
        {
            get { return (string)base["Extension"]; }
            set { base["Extension"] = value.Replace(".", string.Empty); }
        }

        /// <summary>
        /// Gets or sets the context type
        /// </summary>
        [ConfigurationProperty("ContentType", IsRequired = true)]
        public string ContentType
        {
            get { return (string)base["ContentType"]; }
            set { base["ContentType"] = value; }
        }

        #endregion
    }
}