namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    public static class NullableExtensions
    {
        public static T DefaultValue<T>(this T? value, T defaultValue) where T : struct
        {
            if (value == null || !value.HasValue)
            {
                return defaultValue;
            }

            return value.Value;
        }

        public static string ToCurrency(this decimal? input)
        {
            if (input == null || !input.HasValue)
            {
                return string.Empty;
            }

            return input.Value.ToCurrency();
        }
    }
}