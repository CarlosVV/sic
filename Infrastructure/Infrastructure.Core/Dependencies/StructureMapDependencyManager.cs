namespace Nagnoi.SiC.Infrastructure.Core.Dependencies
{
    #region Referencias

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using StructureMap;
    using StructureMap.Web.Pipeline;
    #endregion

    public class StructureMapDependencyManager : IDependencyManager
    {
        #region Private Members

        /// <summary>
        /// Container of Structure Map
        /// </summary>
        private readonly IContainer container;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapGestorDependencias"/> class.
        /// </summary>
        public StructureMapDependencyManager()
            : this(new Container()) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapContainerResolver"/> class.
        /// </summary>
        /// <param name="container">Container of structure map</param>
        public StructureMapDependencyManager(IContainer container)
        {
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            this.container = container;

            this.ConfigureContainer(container);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        public void Register<T>(T instance)
        {
            this.Register(instance, false);
        }
        
        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        /// <param name="likeSingleton">indicates whether must register like singleton</param>
        public void Register<T>(T instance, bool likeSingleton)
        {
            if (Equals(instance, default(T))) {
                throw new ArgumentNullException("instance");
            }

            if (likeSingleton) {
                this.container.Configure(registry => {
                    registry
                        .For(typeof(T))
                        .Singleton()
                        .Use(instance);
                });
            }
            else {
                this.container.Configure(registry => {
                    registry
                        .For(typeof(T))
                        .Use(instance)
                        .LifecycleIs<HybridLifecycle>();
                });
            }
        }

        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation Type</typeparam>
        public void Register<TAbstraction, TImplementation>()
        {
            this.Register<TAbstraction, TImplementation>(false);
        }
        
        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        /// <param name="likeSingleton">indicates whether must register like singleton</param>
        public void Register<TAbstraction, TImplementation>(bool likeSingleton)
        {
            if (likeSingleton) {
                this.container.Configure(registry => {
                    registry
                        .For(typeof(TAbstraction))
                        .Singleton()
                        .Use(typeof(TImplementation));
                });
            }
            else {
                this.container.Configure(registry => {
                    registry
                        .For(typeof(TAbstraction))
                        .Use(typeof(TImplementation))
                        .LifecycleIs<HybridLifecycle>();
                });
            }
        }

        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        /// <param name="name">Object name</param>
        public void Register<TAbstraction, TImplementation>(string name)
        {
            this.Register<TAbstraction, TImplementation>(name, false);
        }
        
        /// <summary>
        /// Registers an implementation
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        /// <param name="name">Object name</param>
        /// <param name="likeSingleton">indicates whether must register like singleton</param>
        public void Register<TAbstraction, TImplementation>(string name, bool likeSingleton)
        {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            if (likeSingleton) {
                this.container.Configure(registry => {
                    registry
                        .For(typeof(TAbstraction))
                        .Singleton()
                        .Use(typeof(TImplementation))
                        .Named(name);
                });
            }
            else {
                this.container.Configure(registry => {
                    registry
                        .For(typeof(TAbstraction))
                        .Use(typeof(TImplementation))
                        .LifecycleIs<HybridLifecycle>()
                        .Named(name);
                });
            }
        }

        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        /// <param name="name">Object name</param>
        public void Register<T>(T instance, string name)
        {
            this.Register(instance, name, false);
        }
        
        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="instance">Object instance</param>
        /// <param name="name">Object name</param>
        /// <param name="likeSingleton">indicates whether must register like singleton</param>
        public void Register<T>(T instance, string name, bool likeSingleton)
        {
            if (Equals(instance, default(T))) {
                throw new ArgumentNullException("instance");
            }

            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            if (likeSingleton) {
                this.container.Configure(registry => {
                    registry
                        .For(typeof(T))
                        .Singleton()
                        .Use(instance)
                        .Named(name);
                });
            }
            else {
                this.container.Configure(registry => {
                    registry
                        .For(typeof(T))
                        .Use(instance)
                        .LifecycleIs<HybridLifecycle>()
                        .Named(name);
                });
            }
        }

        /// <summary>
        /// Scans an assembly for specific type
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="assemblyName">Assembly name</param>
        public void Scan<T>(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName)) {
                throw new ArgumentNullException("assemblyName");
            }

            this.container.Configure(registry => {
                registry.Scan(scan => {
                    scan.Assembly(assemblyName);
                    scan.AddAllTypesOf<T>();
                });
            });
        }

        /// <summary>
        /// Injects an existing instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="existing">Existing object</param>
        public void Inject<T>(T existing) where T : class
        {
            if (existing == null) {
                throw new ArgumentNullException("existing");
            }

            this.container.Inject(existing);
        }

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <returns>Returns the instance</returns>
        public T Resolve<T>(Type type)
        {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            return (T)this.container.GetInstance(type);
        }

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <param name="name">Object name</param>
        /// <returns>Returns the instance</returns>
        public T Resolve<T>(Type type, string name)
        {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            return (T)this.container.GetInstance(type, name);
        }

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <returns>Returns the instance</returns>
        public T Resolve<T>() {
            return this.container.GetInstance<T>();
        }

        /// <summary>
        /// Resolves an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="name">Object name</param>
        /// <returns>Returns the instance</returns>
        public T Resolve<T>(string name)
        {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            return this.container.GetInstance<T>(name);
        }

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <returns>Returns true or false</returns>
        public T TryResolve<T>()
        {
            return this.container.TryGetInstance<T>();
        }

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="name">Object key name</param>
        /// <returns>Returns true or false</returns>
        public T TryResolve<T>(string name)
        {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            return this.container.TryGetInstance<T>(name);
        }

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <returns>Returns a new instance</returns>
        public T TryResolve<T>(Type type)
        {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            return (T)this.container.TryGetInstance(type);
        }

        /// <summary>
        /// Try resolving an object instance
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="type">Object type</param>
        /// <param name="name">Object key name</param>
        /// <returns>Returns a new instance</returns>
        public T TryResolve<T>(Type type, string name)
        {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            return (T)this.container.TryGetInstance(type, name);
        }

        /// <summary>
        /// Resolves all instances
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <returns>Returns the all instances</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            IEnumerable<T> namedInstances = this.container.GetAllInstances<T>();
            T unnamedInstance = default(T);

            try {
                unnamedInstance = this.container.TryGetInstance<T>();
            }
            catch (StructureMapException) {
                throw;
            }

            if (Equals(unnamedInstance, default(T))) {
                return namedInstances;
            }

            return new ReadOnlyCollection<T>(new List<T>(namedInstances));
        }

        /// <summary>
        /// Ejects all instances
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        public void EjectAll<T>()
        {
            this.container.EjectAllInstancesOf<T>();
        }

        /// <summary>
        /// Releases and disposes all HTTP instances
        /// </summary>
        public void DisposeAndClearAll()
        {
            StructureMapObjectCacheLifecycle.DisposeAndClearAll();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Configures the container of Structure Map
        /// </summary>
        /// <param name="container">Container of structure map</param>
        protected void ConfigureContainer(IContainer container)
        {
            Debug.WriteLine(container.WhatDidIScan());
            Debug.WriteLine(container.WhatDoIHave());
        }

        #endregion
    }
}