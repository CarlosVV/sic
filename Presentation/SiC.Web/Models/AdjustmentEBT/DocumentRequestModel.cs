namespace Nagnoi.SiC.Web.Models.AdjustmentEBT
{
    #region References

    using Nagnoi.SiC.Infrastructure.Web.ViewModels;
    using System;

    #endregion

    public class DocumentRequestModel : BaseViewModel
    {
        #region Properties

        public int PaymentId { get; set; }

        public int AdjustmentStatusId { get; set; }

        public DateTime AdjustmentCompletedDate { get; set; }

        public decimal AdjustmentAmount { get; set; }

        public string Comments { get; set; }

        #endregion
    }
}