namespace Nagnoi.SiC.Web.Models.ReferenceTables
{
    #region References

    using System.Collections.Generic;
    using System.Web.Mvc;

    #endregion

    public class IndexViewModel
    {
        #region Constructor

        public IndexViewModel()
        {
            this.ReferenceTables = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public IList<SelectListItem> ReferenceTables { get; set; }

        #endregion       
    }
}