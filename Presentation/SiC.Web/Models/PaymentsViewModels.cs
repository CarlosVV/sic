using System;

namespace Nagnoi.SiC.Web.Models
{
    public class PaymentTransaction
    {
        public int PaymentId { get; set; }

        public string Entidad { get; set; }

        public int EntityId { get; set; }

        public decimal? Inversion { get; set; }
    }

    public class PeriodDiet
    {
        public int PaymentId { get; set; }

        public DateTime Desde { get; set; }

        public DateTime Hasta { get; set; }

        public decimal? Cantidad { get; set; }

        public decimal? Descuento { get; set; }
    }
}