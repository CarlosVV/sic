namespace Nagnoi.SiC.Infrastructure.Web.Utilities
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    #endregion

    public static class SelectListItemExtensions
    {
        public static IList<SelectListItem> ToSelectList<T>(this IEnumerable<T> items, Func<T, string> text, Func<T, string> value = null, Func<T, bool> selected = null, string defaultOption = null)
        {
            List<SelectListItem> selectItems = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(defaultOption))
            {
                selectItems.Add(new SelectListItem()
                {
                    Text = string.Format("-- {0} --", defaultOption),
                    Value = string.Empty
                });
            }

            foreach (var item in items)
            {
                selectItems.Add(new SelectListItem()
                {
                    Text = text.Invoke(item),
                    Value = (value == null ? text.Invoke(item) : value.Invoke(item)),
                    Selected = (selected == null ? false : selected.Invoke(item))
                });
            }

            return selectItems.OrderBy(i => i.Text).ToList();
        }
    }
}