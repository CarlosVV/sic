namespace Nagnoi.SiC.Infrastructure.Core.Dependencies {
    /// <summary>
    /// Representa la factoria de gestor de dependencias
    /// </summary>
    public interface IFactoryDependencyManager {
        /// <summary>
        /// Create dependency resolver
        /// </summary>
        /// <returns>Dependency resolver</returns>
        IDependencyManager CreateInstance();
    }
}