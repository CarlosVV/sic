namespace Nagnoi.SiC.Web.Models.Payment
{

    #region Reference
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Infrastructure.Web.ViewModels;
    using Domain.Core.Model;
    using System.Linq;
    using System;
    #endregion
    public class PaymentViewModel
    {
          #region Constructor
        public PaymentViewModel()
        { }
        #endregion

        #region Properties
        public Infrastructure.Web.ViewModels.CaseViewModel CaseModel = new Infrastructure.Web.ViewModels.CaseViewModel();
        public Domain.Core.Model.InformacionCaso_Result CaseInfo = new Domain.Core.Model.InformacionCaso_Result();
        #endregion
    }
}