using System.Collections.Generic;

namespace Nagnoi.SiC.Domain.Core.Model
{
    public class TransaccionesAprobar
    {
        public Case Caso;

        public Transaction Transaccion { get; set; }

        public IEnumerable<Payment> PagosTransaccion { get; set; }

        public Entity Entidad { get; set; }
    }
}