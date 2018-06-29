namespace Nagnoi.SiC.Web.Models.PaymentRegistration
{
    #region References

    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    #endregion

    public class IppUpdateModel
    {
         #region Constructor

        public IppUpdateModel()
        {
            Desgloses = new List<IppDesgloseUpdateModel>();
        }

        #endregion

        #region Properties

        public int CaseId { get; set; }

        public int CaseDetailId { get; set; }

        public int EntityId { get; set; }

        public string CaseNumber { get; set; }

        public string CaseKey { get; set; }

        public decimal? CantidadAdjudicada { get; set; }

        public decimal? Mensualidad { get; set; }

        public decimal? Semanas { get; set; }

        public DateTime? FechaAdjudicacion { get; set; }

        public string Comments { get; set; }

        public decimal? CompSemanalInca { get; set; }

        public IList<IppDesgloseUpdateModel> Desgloses { get; set; }

        #endregion

        public int PaymentId { get; set; }

        public int TransactionId { get; set; }
    }   
}