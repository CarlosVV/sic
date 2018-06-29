namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    
    public partial class ResumenPagosPorConcepto_Result
    {
        public string Concepto { get; set; }
        public decimal Pagado { get; set; }
        public decimal No_Cobrado { get; set; }
    }
}
