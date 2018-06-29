using Nagnoi.SiC.Infrastructure.Web.ViewModels;
namespace Nagnoi.SiC.Web.Models.AdjustmentEBT
{
    public class ApproveRequestModel : BaseViewModel
    {
        #region Properties

        public int PaymentId { get; set; }

        public int AdjustmentStatusId { get; set; }

        public string Comments { get; set; }

        #endregion
    }
}