namespace Nagnoi.SiC.Infrastructure.Core.Dependencies
{
    #region Imports

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Provee un conjunto de metodos para resolver dependencias
    /// </summary>
    public interface IDependencyManager
    {
        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        void Register<T>(T instance);
        
        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        /// <param name="likeSingleton">indicates whether must register like singleton</param>
        void Register<T>(T instance, bool likeSingleton);

        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        /// <param name="name">Object name</param>
        void Register<T>(T instance, string name);
        
        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        /// <param name="name">Object name</param>
        /// <param name="likeSingleton">indicates whether must register like singleton</param>
        void Register<T>(T instance, string name, bool likeSingleton);

        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation Type</typeparam>
        void Register<TAbstraction, TImplementation>();
        
        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        /// <param name="likeSingleton">indicates whether must register like singleton</param>
        void Register<TAbstraction, TImplementation>(bool likeSingleton);

        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        /// <param name="name">Object name</param>
        void Register<TAbstraction, TImplementation>(string name);
        
        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        /// <param name="name">Object name</param>
        /// <param name="likeSingleton">indicates whether must register like singleton</param>
        void Register<TAbstraction, TImplementation>(string name, bool likeSingleton);

        /// <summary>
        /// Scans an assembly for specific type
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="assemblyName">Assembly name</param>
        void Scan<T>(string assemblyName);

        /// <summary>
        /// Injects an existing instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="existing">Existing object</param>
        void Inject<T>(T existing) where T : class;

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <returns>Returns the instance</returns>
        T Resolve<T>(Type type);

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <param name="name">Object name</param>
        /// <returns>Returns the instance</returns>
        T Resolve<T>(Type type, string name);

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <returns>Returns the instance</returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="name">Object name</param>
        /// <returns>Returns the instance</returns>
        T Resolve<T>(string name);

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <returns>Returns true or false</returns>
        T TryResolve<T>();

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="name">Object key name</param>
        /// <returns>Returns true or false</returns>
        T TryResolve<T>(string name);

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <returns>Returns a new instance</returns>
        T TryResolve<T>(Type type);

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <param name="name">Object key name</param>
        /// <returns>Returns a new instance</returns>
        T TryResolve<T>(Type type, string name);

        /// <summary>
        /// Resolves all instances
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <returns>Returns the all instances</returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// Ejects all instances
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        void EjectAll<T>();

        /// <summary>
        /// Releases and disposes all HTTP instances
        /// </summary>
        void DisposeAndClearAll();
    }
}