namespace Nagnoi.SiC.Infrastructure.Core.Dependencies
{
    #region Imports

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Representa la factoria de control de dependencias
    /// </summary>
    public static class IoC
    {
        #region Private Members

        /// <summary>
        /// Dependency resolver
        /// </summary>
        private static IDependencyManager container;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a container of dependency resolver
        /// </summary>
        /// <param name="frameworkContainer">Framework container</param>
        public static void InitializeWith(IFactoryDependencyManager frameworkContainer)
        {
            if (frameworkContainer == null)
            {
                throw new ArgumentNullException("frameworkContainer");
            }

            container = frameworkContainer.CreateInstance();
        }

        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        public static void Register<T>(T instance)
        {
            if (Equals(instance, default(T)))
            {
                throw new ArgumentNullException("instance");
            }

            container.Register(instance, false);
        }

        /// <summary>
        /// Register an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        /// <param name="likeSingleton">A item indicating whether must be singleton</param>
        public static void Register<T>(T instance, bool likeSingleton)
        {
            if (Equals(instance, default(T)))
            {
                throw new ArgumentNullException("instance");
            }

            container.Register(instance, likeSingleton);
        }

        /// <summary>
        /// Register an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        /// <param name="name">Object name</param>
        public static void Register<T>(T instance, string name)
        {
            if (Equals(instance, default(T)))
            {
                throw new ArgumentNullException("instance");
            }

            container.Register(instance, name);
        }

        /// <summary>
        /// Register an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        /// <param name="name">Object name</param>
        /// <param name="likeSingleton">A item indicating whether must be singleton</param>
        public static void Register<T>(T instance, string name, bool likeSingleton)
        {
            if (Equals(instance, default(T)))
            {
                throw new ArgumentNullException("instance");
            }

            container.Register(instance, name, likeSingleton);
        }

        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation Type</typeparam>
        public static void Register<TAbstraction, TImplementation>()
        {
            container.Register<TAbstraction, TImplementation>(false);
        }

        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        /// <param name="likeSingleton">indicates whether must register like singleton</param>
        public static void Register<TAbstraction, TImplementation>(bool likeSingleton)
        {
            container.Register<TAbstraction, TImplementation>(likeSingleton);
        }

        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        /// <param name="name">Object name</param>
        public static void Register<TAbstraction, TImplementation>(string name)
        {
            container.Register<TAbstraction, TImplementation>(name);
        }

        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        /// <param name="name">Object name</param>
        /// <param name="likeSingleton">indicates whether must register like singleton</param>
        public static void Register<TAbstraction, TImplementation>(string name, bool likeSingleton)
        {
            container.Register<TAbstraction, TImplementation>(name, likeSingleton);
        }

        /// <summary>
        /// Scans an assembly for specific type
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="assemblyName">Assembly name</param>
        public static void Scan<T>(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }

            container.Scan<T>(assemblyName);
        }

        /// <summary>
        /// Injects an existing instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="existing">Existing object</param>
        public static void Inject<T>(T existing) where T : class
        {
            if (existing == null)
            {
                throw new ArgumentNullException("existing");
            }

            container.Inject(existing);
        }

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <returns>Returns the instance</returns>
        public static T Resolve<T>(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return container.Resolve<T>(type);
        }

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <param name="name">Object name</param>
        /// <returns>Returns the instance</returns>
        public static T Resolve<T>(Type type, string name)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return container.Resolve<T>(type, name);
        }

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <returns>Returns the instance</returns>
        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="name">Object name</param>
        /// <returns>Returns the instance</returns>
        public static T Resolve<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return container.Resolve<T>(name);
        }

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <returns>Returns true or false</returns>
        public static T TryResolve<T>()
        {
            return container.TryResolve<T>();
        }

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="name">Object key name</param>
        /// <returns>Returns true or false</returns>
        public static T TryResolve<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return container.TryResolve<T>(name);
        }

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <returns>Returns a new instance</returns>
        public static T TryResolve<T>(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return container.TryResolve<T>(type);
        }

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <param name="name">Object key name</param>
        /// <returns>Returns a new instance</returns>
        public static T TryResolve<T>(Type type, string name)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return container.TryResolve<T>(type, name);
        }

        /// <summary>
        /// Resolves all instances
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <returns>Returns the all instances</returns>
        public static IEnumerable<T> ResolveAll<T>()
        {
            return container.ResolveAll<T>();
        }

        /// <summary>
        /// Ejects all instances
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        public static void EjectAll<T>()
        {
            container.EjectAll<T>();
        }

        /// <summary>
        /// Releases and disposes all HTTP instances
        /// </summary>
        public static void DisposeAndClearAll()
        {
            container.DisposeAndClearAll();
        }

        #endregion
    }
}