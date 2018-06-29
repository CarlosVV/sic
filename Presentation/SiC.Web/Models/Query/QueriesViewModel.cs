namespace Nagnoi.SiC.Web.Models.Query
{
    #region Reference
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Infrastructure.Web.ViewModels;
    using Domain.Core.Model;
    using System.Linq;
    using System;
    #endregion

    public class QueriesViewModel : BaseViewModel
    {
        #region Constructor
        public QueriesViewModel()
        { }
        #endregion

        #region Properties
        public Infrastructure.Web.ViewModels.CaseViewModel CaseModel = new Infrastructure.Web.ViewModels.CaseViewModel();
        #endregion
    }
}