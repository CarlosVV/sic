namespace Nagnoi.SiC.Infrastructure.Core.Security
{
    #region References

    using System.Security.Cryptography;
    using System.Text;

    #endregion

    public sealed class StringHasher : ICryptography
    {
        #region Public Methods
        
        public string HashString(string clearText)
        {
            if (string.IsNullOrEmpty(clearText))
            {
                return string.Empty;
            }

            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(clearText);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder result = new StringBuilder();
            for (int index = 0; index < hash.Length; index++)
            {
                result.Append(hash[index].ToString("X2"));
            }
            return result.ToString();
        }

        #endregion
    }
}