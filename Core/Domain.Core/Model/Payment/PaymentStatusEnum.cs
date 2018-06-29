using System.ComponentModel;

namespace Nagnoi.SiC.Domain.Core.Model {
    public enum PaymentStatusEnum {
        [Description("C")]
        Cancelado = 1,
        [Description("E")]
        Emitido = 2,
        [Description("M")]
        Movimiento = 3,
        [Description("P")]
        Pagado = 4,
        [Description("V")]
        Vencido = 5,
        [Description("S")]
        StopPayment = 6,
        [Description("L")]
        Levantamiento = 7,
        [Description("T")]
        CanceladoNoSustituido = 8,
        [Description("X")]
        Certificado = 9,
        [Description("D")]
        Descuento = 10,
        [Description("G")]
        Generado = 11,
        [Description("R")]
        Registrado = 12,
        [Description("A")]
        Aprobado = 13,
        [Description("I")]
        SimeraSolicitado = 14,
        [Description("N")]
        SimeraProcesado = 15,
        [Description("U")]
        SapSolicitado = 16,
        [Description("Y")]
        SapProcesado = 17
    }
}
