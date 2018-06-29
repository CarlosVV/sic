namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    /// <summary>
    /// Boolean extension methods.
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        /// Gets the human translation
        /// </summary>
        /// <param name="item">Boolean item</param>
        /// <returns>Return the translation</returns>
        public static string ToYesNoString(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}