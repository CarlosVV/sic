namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    public static class DecimalExtensions
    {
        public static string ToCurrency(this decimal input)
        {
            return string.Format("{0:C2}", input);
        }
    }
}