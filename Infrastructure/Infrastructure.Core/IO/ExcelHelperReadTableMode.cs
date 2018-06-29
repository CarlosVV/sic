namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    /// <summary>
    /// Represents a read table node for ExcelHelper.ReadTable method
    /// </summary>
    public enum ExcelHelperReadTableMode
    {
        /// <summary>
        /// Read rows from all filled excel worksheet cells
        /// </summary>
        ReadFromWorkSheet,

        /// <summary>
        /// Read rows only from named range
        /// </summary>
        ReadFromNamedRange
    }
}