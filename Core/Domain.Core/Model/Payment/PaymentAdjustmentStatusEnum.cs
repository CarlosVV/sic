using System.ComponentModel;

namespace Nagnoi.SiC.Domain.Core.Model {
    public enum PaymentAdjustmentStatusEnum {
        [Description("Solicitado")]
        Solicitado = 1,
        [Description("Aprobado")]
        Aprobado = 2,
        [Description("Rechazado")]
        Rechazado = 3,
        [Description("Completado")]
        Completado = 4
    }
}
