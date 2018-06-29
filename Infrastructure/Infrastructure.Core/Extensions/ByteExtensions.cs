namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using System.Web;

    #endregion

    /// <summary>
    /// Byte extension methods.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Decompress an array of bytes to a string using the specified encoding
        /// </summary>
        /// <param name="compressedString">The compressed string.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>Decompressed string</returns>
        public static string UnCompress(this byte[] compressedString, Encoding encoding)
        {
            const int BufferSize = 1024;

            MemoryStream memoryStream = null;
            MemoryStream result = null;
            GZipStream zipStream = null;

            try
            {
                memoryStream = new MemoryStream(compressedString);

                zipStream = new GZipStream(memoryStream, CompressionMode.Decompress);

                memoryStream = null;

                result = new MemoryStream();

                var buffer = new byte[BufferSize];
                var totalBytes = 0;
                int readBytes;

                while ((readBytes = zipStream.Read(buffer, 0, BufferSize)) > 0)
                {
                    result.Write(buffer, 0, readBytes);
                    totalBytes += readBytes;
                }

                return encoding.GetString(result.GetBuffer(), 0, totalBytes);
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }

                if (zipStream != null)
                {
                    zipStream.Dispose();
                }

                if (result != null)
                {
                    result.Dispose();
                }
            }
        }

        /// <summary>
        /// Decompress an array of bytes to a string using default UTF8 encoding.
        /// </summary>
        /// <param name="compressedString">The compressed string.</param>
        /// <returns>Decompressed string</returns>
        public static string UnCompress(this byte[] compressedString)
        {
            return UnCompress(compressedString, new UTF8Encoding());
        }

        /// <summary>
        /// Gets the download binary array
        /// </summary>
        /// <param name="postedFile">Posted File</param>
        /// <returns>Download binary array</returns>
        public static byte[] GetDownloadBits(this HttpPostedFile postedFile)
        {
            Stream stream = postedFile.InputStream;

            int size = postedFile.ContentLength;

            byte[] content = new byte[size];

            stream.Read(content, 0, size);

            return content;
        }

        /// <summary>
        /// Gets the download binary array
        /// </summary>
        /// <param name="stream">Stream instance</param>
        /// <returns>Download binary array</returns>
        public static byte[] GetDownloadBits(this Stream stream)
        {
            int size = Convert.ToInt32(stream.Length);

            byte[] content = new byte[size];

            stream.Read(content, 0, size);

            return content;
        }
    }
}