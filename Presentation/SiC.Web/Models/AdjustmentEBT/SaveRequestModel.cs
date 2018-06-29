using Nagnoi.SiC.Infrastructure.Web.ViewModels;
namespace Nagnoi.SiC.Web.Models.AdjustmentEBT
{
    public class SaveRequestModel : BaseViewModel
    {
        #region Properties

        public int PaymentId { get; set; }

        public int AdjustmentStatusId { get; set; }

        public string AdjustmentType { get; set; }

        #endregion
    }
}