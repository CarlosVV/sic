namespace Nagnoi.SiC.Web.Models.PaymentRegistration
{
    #region References

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Infrastructure.Web.ViewModels;

    #endregion

    public class IndexViewModel : BaseViewModel
    {
        #region Constructor

        public IndexViewModel()
        {
            CompensationRegions = new List<SelectListItem>();
            GroupedCompensationRegions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public CaseViewModel CaseModel = new CaseViewModel();

        public IEnumerable<SelectListItem> CompensationRegions { get; set; }

        public IEnumerable<SelectListItem> GroupedCompensationRegions { get; set; }

        #endregion
    }
}