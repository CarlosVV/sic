namespace Nagnoi.SiC.Web.Models.AdjustmentEBT
{
    #region References

    using Nagnoi.SiC.Infrastructure.Web.ViewModels;
    using System.Collections.Generic;
    using System.Web.Mvc;

    #endregion

    public class ApproveViewModel : BaseViewModel
    {
        #region Constructor

        public ApproveViewModel()
        {
            AdjustmentStatuses = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public IList<SelectListItem> AdjustmentStatuses { get; set; }
        
        #endregion
    }
}