namespace Nagnoi.SiC.Web.Models.ThirdPartyPayment
{
    #region References

    using System;

    #endregion

    public class ThirdPartyPaymentModel
    {
        #region Properties

        public int ThirdPartyScheduleId { get; set; }

        public int? CaseId { get; set; }

        public int? CaseDetailId { get; set; }

        public string CaseNumber { get; set; }

        public string CaseKey { get; set; }

        public string ClaimNumber { get; set; }

        public string OrderIdentifier { get; set; }

        public bool TerminationFlag { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string TerminationOrderNumber { get; set; }

        public decimal? SinglePaymentAmount { get; set; }

        public decimal? FirstInstallmentAmount { get; set; }

        public decimal? SecondInstallmentAmount { get; set; }

        public string Comment { get; set; }

        public int EntityTypeId { get; set; }

        public int? CourtId { get; set; }

        public string EntityName { get; set; }

        public string EntityAddressLine1 { get; set; }

        public string EntityAddressLine2 { get; set; }

        public int? EntityCityId { get; set; }

        public string EntityPostalCode { get; set; }

        public string CustodyFirstName { get; set; }

        public string CustodySecondName { get; set; }

        public string CustodyFirstLastName { get; set; }

        public string CustodySecondLastName { get; set; }

        public string CustodyAddressLine1 { get; set; }

        public string CustodyAddressLine2 { get; set; }

        public int? CustodyCityId { get; set; }

        public string CustodyPostalCode { get; set; }

        public decimal? OrderAmount { get; set; }

        public DateTime? TerminationDate { get; set; }

        public bool IsSinglePayment { get; set; }
        
        #endregion
    }
}