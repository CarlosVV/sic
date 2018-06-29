namespace Nagnoi.SiC.Domain.Core.Model
{
    public enum PaymentException : int
    {
        None = 0,
        Generic = 9001,
        DatosIncompletos = 9901,
        ErrorInsercionPagosTerceros = 55,
        ErrorInsercionPagosInversiones,
        ErrorInsercionTransacciones,
        ErrorInsercionPagosPerentorios,
        ErrorInsercionPagosIPP,
        ErrorActualizacionEstado,
        ErrorInsercionDietaPendiente,
        ErroActualizarPagoTerceros
    }
}