namespace Nagnoi.SiC.Web.Models.ThirdPartyPayment
{
    #region References

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Nagnoi.SiC.Infrastructure.Web.ViewModels;

    #endregion

    public class IndexViewModel : BaseViewModel
    {
        #region Constructor

        public IndexViewModel()
        {
            Courts = new List<SelectListItem>();
            Cities = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public CaseViewModel CaseModel = new CaseViewModel();
        public IList<SelectListItem> Courts { get; set; }

        public IList<SelectListItem> Cities { get; set; }

        #endregion
    }
}