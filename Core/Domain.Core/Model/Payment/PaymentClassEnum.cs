using System.ComponentModel;

namespace Nagnoi.SiC.Domain.Core.Model {
    public enum PaymentClassEnum {
        PagoInicial = 1,
        Anticipo = 2,
        Inversion = 3,
        Mensualidad = 4,
        PagoInicialTotal = 5,
        PagoFinalMensualidad = 6,
        Retroactiva = 7,
        Otros = 8,
        [Description("A")]
        Ajuste = 9,
        [Description("E")]
        PagoInicialRetroactiva = 10,
        [Description("F")]
        PagoFinal = 11,
        [Description("I")]
        PagoInicialSecond = 12,
        [Description("Q")]
        Quincena = 13,
        [Description("R")]
        RetroactivaSecond = 14
    }
}
