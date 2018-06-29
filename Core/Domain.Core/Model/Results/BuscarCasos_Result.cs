namespace Nagnoi.SiC.Domain.Core.Model
{
    using System;
    
    public partial class BuscarCasos_Result
    {
        public string Nombre { get; set; }
        public string SSN { get; set; }
        public string EBT { get; set; }
        public string NumeroCaso { get; set; }
        public Nullable<int> CaseFolderId { get; set; }
    }
}
