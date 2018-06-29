namespace Nagnoi.SiC.Web.Models.PaymentRegistration
{
    #region References

    using System;
    using System.Collections.Generic;

    #endregion

    public class InvestmentCreateModel
    {
        #region Constructor

        public InvestmentCreateModel()
        {
            Payments = new List<InvestmentPaymentCreateModel>();
        }

        #endregion

        #region Properties

        public int CaseId { get; set; }

        public int CaseDetailId { get; set; }

        public string NumeroCaso { get; set; }

        public DateTime? FechaDecision { get; set; }

        public int BeneficiarioId { get; set; }

        public IList<InvestmentPaymentCreateModel> Payments { get; set; }

        #endregion
    }

    public class InvestmentPaymentCreateModel
    {
        #region Properties

        public string Entidad { get; set; }

        public decimal? Inversion { get; set; }

        #endregion
    }
}