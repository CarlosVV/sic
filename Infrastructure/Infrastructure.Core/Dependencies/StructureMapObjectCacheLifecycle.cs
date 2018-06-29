namespace Nagnoi.SiC.Infrastructure.Core.Dependencies
{
    #region References

    using System;
    using StructureMap;
    using StructureMap.Pipeline;

    #endregion

    public class StructureMapObjectCacheLifecycle : LifecycleBase
    {
        private static readonly Lazy<IObjectCache> Cache = new Lazy<IObjectCache>(() => new LifecycleObjectCache());

        public static void DisposeAndClearAll()
        {
            Cache.Value.DisposeAndClear();
        }

        public override void EjectAll(ILifecycleContext context)
        {
            FindCache(context).DisposeAndClear();
        }

        public override IObjectCache FindCache(ILifecycleContext context)
        {
            return Cache.Value;
        }
    }
}