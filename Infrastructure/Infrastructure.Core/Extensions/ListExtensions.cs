namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;

    #endregion

    /// <summary>
    /// List extension methods.
    /// </summary>
    public static class ListExtensions
    {
        public static IList<T> Clone<T>(this IList<T> source) where T : ICloneable
        {
            return source.Select(item => (T)item.Clone()).ToList();
        }

        public static bool IsEmpty<T>(this IList<T> source)
        {
            if (source.IsNull())
            {
                return true;
            }

            return source.Count == 0;
        }

        public static DataTable ToDataTable<T>(this IList<T> source)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in source)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}