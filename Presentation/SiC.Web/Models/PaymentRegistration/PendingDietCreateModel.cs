namespace Nagnoi.SiC.Web.Models.PaymentRegistration
{
    #region References

    using System;
    using System.Collections.Generic;

    #endregion

    public class PendingDietCreateModel
    {
        #region Constructor

        public PendingDietCreateModel()
        {
            Periods = new List<PeriodDietCreateModel>();
        }

        #endregion

        #region Properties

        public int CaseId { get; set; }

        public int CaseDetailId { get; set; }

        public DateTime? FechaDecision { get; set; }

        public DateTime? FechaVisita { get; set; }

        public DateTime? FechaNotificacion { get; set; }

        public string NumeroCaso { get; set; }

        public decimal? MontoTotal { get; set; }

        public string Comment { get; set; }

        public IList<PeriodDietCreateModel> Periods { get; set; }

        #endregion
    }

    public class PeriodDietCreateModel
    {
        #region Properties

        public DateTime? Desde { get; set; }

        public DateTime? Hasta { get; set; }

        public decimal? Cantidad { get; set; }

        public decimal? Descuento { get; set; }

        #endregion
    }
}