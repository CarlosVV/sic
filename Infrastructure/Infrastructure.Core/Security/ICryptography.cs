namespace Nagnoi.SiC.Infrastructure.Core.Security
{
    /// <summary>
    /// Represents the cryptography interface
    /// </summary>
    public interface ICryptography
    {
        /// <summary>
        /// Hashes the given clear text string
        /// </summary>
        /// <param name="clearText">String to hash</param>
        /// <returns>Hashed version of the string</returns>
        string HashString(string clearText);
    }
}
