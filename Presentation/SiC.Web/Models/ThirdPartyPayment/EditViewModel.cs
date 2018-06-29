namespace Nagnoi.SiC.Web.Models.ThirdPartyPayment
{
    public class EditViewModel
    {
        #region Properties

        public int ThirdPartyScheduleId { get; set; }

        public int? CaseId { get; set; }

        public int? CaseDetailId { get; set; }

        public string CaseNumber { get; set; }

        public string Lesionado { get; set; }

        public string SSN { get; set; }

        public string Custodio { get; set; }

        public string ClaimNumber { get; set; }

        public string OrderIdentifier { get; set; }

        public decimal? SinglePaymentAmount { get; set; }

        public decimal? FirstInstallAmount { get; set; }

        public decimal? SecondInstallAmount { get; set; }

        public string EffectiveDate { get; set; }

        public string Observaciones { get; set; }

        #endregion
    }
}