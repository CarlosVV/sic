namespace Nagnoi.SiC.Web.Models.Payment
{
    public class PaymentTransactionModel
    {
        #region Properties

        public int PaymentId { get; set; }

        public int TransactionId { get; set; }

        public string TransactionDate { get; set; }

        public string DecisionDate { get; set; }

        public string NotificationDateIC { get; set; }

        public string ICCaseNumber { get; set; }

        public string TransactionAmount { get; set; }

        public string HearingDateIC { get; set; }

        public string Observaciones { get; set; }

        public string MonthlyInstallment { get; set; }

        public string NumberOfWeeks { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string Discount { get; set; }

        public string PaymentDay { get; set; }

        #endregion
    }
}