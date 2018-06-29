namespace Nagnoi.SiC.Web.Models.PaymentRegistration
{
    public class PeremptoryCreateModel
    {
        #region Properties

        public int CaseId { get; set; }

        public int CaseDetailId { get; set; }

        public int EntityId { get; set; }

        public string NumeroCaso { get; set; }

        public decimal? Cantidad { get; set; }

        public int BeneficiarioId { get; set; }

        public string Comment { get; set; }

        #endregion
    }
}