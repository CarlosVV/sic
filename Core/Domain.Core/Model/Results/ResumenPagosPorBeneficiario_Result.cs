namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    
    public partial class ResumenPagosPorBeneficiario_Result
    {
        public string Beneficiario { get; set; }
        public decimal Pagado { get; set; }
        public decimal No_Cobrado { get; set; }
    }
}
