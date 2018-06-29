namespace Nagnoi.SiC.Web.Models.Approval
{
    #region References

    using System;
    using System.Collections.Generic;

    #endregion

    public class ApprovalInvestmentModel : AllowOperationsModel
    {
        #region Constructor

        public ApprovalInvestmentModel()
        {
            Pagos = new List<ApprovalInvestmentPaymentModel>();
        }

        #endregion

        #region Properties

        public int TransactionId { get; set; }

        public int CaseId { get; set; }

        public string CaseNumber { get; set; }

        public int CaseDetailId { get; set; }

        public string Lesionado { get; set; }

        public string Ssn { get; set; }

        public string FechaEmisionDecision { get; set; }

        public string TotalInversion { get; set; }

        public string RazonInversion { get; set; }

        public string Estado { get; set; }

        public int StatusId { get; set; }

        public string Comments { get; set; }

        public IList<ApprovalInvestmentPaymentModel> Pagos { get; set; }

        #endregion

        public string RazonRechazo { get; set; }
    }

    public class ApprovalInvestmentPaymentModel
    {
        #region Properties

        public int PaymentId { get; set; }

        public int EntityId { get; set; }

        public string PagoDirigidoA { get; set; }

        public string Importe { get; set; }

        #endregion
    }
}