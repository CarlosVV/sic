namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Nagnoi.SiC.Infrastructure.Web.Controllers;
    using Nagnoi.SiC.Infrastructure.Web.ViewModels;

    #endregion    
    public class TransactionDetailController : BaseController
    {
        public TransactionDetailViewModel GetCompensationRegionModel()
        {
            var regions = new List<SelectListItem>();
            var codes = new List<SelectListItem>();
            
            var compensationRegions = CompensationRegionService.GetAllCompensationRegions();

            regions.Add(new SelectListItem { Value = "0", Text = "--Seleccione--" });
            codes.Add(new SelectListItem { Value = "0", Text = "--Seleccione--" });

            foreach (var compensationRegion in compensationRegions)
            {
                regions.Add(new SelectListItem
                {
                    Value = compensationRegion.CompensationRegionId.ToString(),
                    Text = compensationRegion.Region.ToString()
                });
                codes.Add(new SelectListItem
                {
                    Value = compensationRegion.CompensationRegionId.ToString(),
                    Text = compensationRegion.Code.ToString()
                });
            }

            var model = new TransactionDetailViewModel();

            model.CompensationCodeList = codes;
            model.CompensationRegionList = regions;

            return model;
        }

    }
}