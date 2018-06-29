namespace Nagnoi.SiC.Infrastructure.Core.Extensions
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class Extensions {

        public static IList<T> Clone<T>(this IList<T> input) where T : ICloneable
        {
            return input.Select(item => (T)item.Clone()).ToList();
        }
    }
}