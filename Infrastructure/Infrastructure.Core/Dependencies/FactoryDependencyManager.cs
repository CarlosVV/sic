namespace Nagnoi.SiC.Infrastructure.Core.Dependencies {
    #region Referencias

    using System;
    using Nagnoi.SiC.Infrastructure.Core.Configuration;

    #endregion

    public sealed class FactoryDependencyManager : IFactoryDependencyManager {
        #region Private Members

        /// <summary>
        /// Resolver type
        /// </summary>
        private readonly Type resolverType;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoriaGestorDependencias"/> class.
        /// </summary>
        public FactoryDependencyManager()
            : this(Settings.DependencyContainerType) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoriaGestorDependencias"/> class.
        /// </summary>
        /// <param name="resolverTypeName">Resolver type name</param>
        public FactoryDependencyManager(string resolverTypeName) {
            if (string.IsNullOrEmpty(resolverTypeName)) {
                throw new ArgumentNullException("resolverTypeName");
            }

            this.resolverType = Type.GetType(resolverTypeName, true, true);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create dependency resolver
        /// </summary>
        /// <returns>Dependency resolver</returns>
        public IDependencyManager CreateInstance() {
            return Activator.CreateInstance(this.resolverType) as IDependencyManager;
        }

        #endregion
    }
}