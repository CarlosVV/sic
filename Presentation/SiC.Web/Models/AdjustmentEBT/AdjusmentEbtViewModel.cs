using Nagnoi.SiC.Infrastructure.Web.ViewModels;
namespace Nagnoi.SiC.Web.Models.AdjustmentEBT
{
    public class AdjusmentEbtViewModel : BaseViewModel
    {
        #region Properties

        public int PaymentId { get; set; }

        public int CaseId { get; set; }

        public int CaseDetailId { get; set; }

        public string CaseNumber { get; set; }
        public string EbtNumber { get; set; }

        public string CaseKey { get; set; }

        public int? AdjustmentStatusId { get; set; }

        public string AdjustmentStatus { get; set; }

        public int? AdjustmentTypeId { get; set; }

        public string AdjustmentType { get; set; }

        public int? ConceptId { get; set; }

        public string Concept { get; set; }

        public string TransactionNumber { get; set; }

        public string IssueDate { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public short PaymentDay { get; set; }

        public string Amount { get; set; }

        public string AdjustmentRequestedBy { get; set; }

        public string AdjustmentRequestedDate { get; set; }

        public string AdjustmentCompletedBy { get; set; }

        public string AdjustmentCompletedDate { get; set; }

        public string Comments { get; set; }

        #endregion
    }
}