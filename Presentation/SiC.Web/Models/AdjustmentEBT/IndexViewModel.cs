namespace Nagnoi.SiC.Web.Models.AdjustmentEBT
{
    #region References

    using Nagnoi.SiC.Infrastructure.Web.ViewModels;
    using System.Collections.Generic;
    using System.Web.Mvc;

    #endregion

    public class IndexViewModel : Case.SearchViewModel
    {
        #region Constructor

        public IndexViewModel()
        {
            AdjustmentStatuses = new List<SelectListItem>();
            AdjustmentTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public IList<SelectListItem> AdjustmentStatuses { get; set; }

        public IList<SelectListItem> AdjustmentTypes { get; set; }

        public CaseDetailViewModel CaseModel { get; set; }
        #endregion
    }
}