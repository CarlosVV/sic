namespace Nagnoi.SiC.Web.Models.AdjustmentEBT
{
    #region References

    using Nagnoi.SiC.Infrastructure.Web.ViewModels;
    using System.Collections.Generic;
    using System.Web.Mvc;

    #endregion

    public class DocumentViewModel : BaseViewModel
    {
        #region Constructor

        public DocumentViewModel()
        {
            AdjustmentStatuses = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public IList<SelectListItem> AdjustmentStatuses { get; set; }

        #endregion
    }
}