namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    #region Imports

    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    #endregion

    /// <summary>
    /// Represents an Image Helper
    /// </summary>
    public static class ImageHelper
    {
        #region Private Members

        /// <summary>
        /// Default item of compression level
        /// </summary>
        public static readonly int DefaultCompressionLevel = 80;

        #endregion

        #region Public Methods

        /// <summary>
        /// Save an image from byte array
        /// </summary>
        /// <param name="imageContent">Image content</param>
        /// <param name="filePath">File path</param>
        /// <param name="targetSize">Image size</param>
        public static void SaveImageFromBytes(byte[] imageContent, string filePath, int targetSize)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (imageContent == null)
            {
                return;
            }

            Image image = ImageFromBytes(imageContent, targetSize);

            image.Save(filePath);
        }

        /// <summary>
        /// Gets an image instance from byte array
        /// </summary>
        /// <param name="imageContent">Image content</param>
        /// <returns>Returns an image instance</returns>
        public static Image ImageFromBytes(byte[] imageContent)
        {
            return ImageFromBytes(imageContent, 0, 0, false);
        }

        /// <summary>
        /// Gets an image instance from byte array
        /// </summary>
        /// <param name="imageContent">Image content</param>
        /// <param name="size">Image size</param>
        /// <returns>Returns an image instance</returns>
        public static Image ImageFromBytes(byte[] imageContent, int size)
        {
            return ImageFromBytes(imageContent, size, 0, false);
        }

        /// <summary>
        /// Gets an image instance from byte array
        /// </summary>
        /// <param name="imageContent">Image content</param>
        /// <param name="size">Image size</param>
        /// <param name="compressionLevel">Compression level</param>
        /// <returns>Returns an image instance</returns>
        public static Image ImageFromBytes(byte[] imageContent, int size, int compressionLevel)
        {
            return ImageFromBytes(imageContent, size, compressionLevel, false);
        }

        /// <summary>
        /// Gets an image instance from byte array
        /// </summary>
        /// <param name="imageContent">Image content</param>
        /// <param name="size">Image size</param>
        /// <param name="compressionLevel">Compression level</param>
        /// <param name="isThumbnail">Is Thumbnail</param>
        /// <returns>Returns an image instance</returns>
        public static Image ImageFromBytes(byte[] imageContent, int size, int compressionLevel, bool isThumbnail)
        {
            MemoryStream stream = new MemoryStream(imageContent);

            Image image = Image.FromStream(stream);

            if (size > 0)
            {
                image = ResizeImageAndCompress(image, size, compressionLevel, isThumbnail);
            }

            return image;
        }

        /// <summary>
        /// Resize and compress an image
        /// </summary>
        /// <param name="source">Image source</param>
        /// <param name="size">Image size</param>
        /// <param name="compressionLevel">Compression level</param>
        /// <param name="isThumbnail">Is Thumbnail</param>
        /// <returns>Return the image instance</returns>
        public static Image ResizeImageAndCompress(Image source, int size, int compressionLevel, bool isThumbnail)
        {
            byte[] imageArray = GetByteArrayImageResizedAndCompressed(source, size, compressionLevel, isThumbnail);

            MemoryStream stream = new MemoryStream(imageArray);

            return Image.FromStream(stream);
        }

        /// <summary>
        /// Gets the byte array resized and compressed from image object
        /// </summary>
        /// <param name="source">Image source</param>
        /// <param name="size">Image size</param>
        /// <param name="compressionLevel">Compression level</param>
        /// <param name="isThumbnail">Is Thumbnail</param>
        /// <returns>Return the byte array</returns>
        public static byte[] GetByteArrayImageResizedAndCompressed(Image source, int size, int compressionLevel, bool isThumbnail)
        {
            byte[] result = null;
            
            Image imageToWork = new Bitmap(size, size);

            Graphics graphic = Graphics.FromImage(imageToWork);

            if (source.Width == source.Height)
            {
                graphic.DrawImage(source, 0, 0, size, size);
            }
            else if ((source.Width > source.Height) && isThumbnail)
            {
                int newWidth = source.Width * size / source.Height;
                int x = (newWidth - size) / 2;
                graphic.DrawImage(source, -x, 0, newWidth, size);
            }
            else if ((source.Width > source.Height) && !isThumbnail)
            {
                graphic.FillRectangle(Brushes.White, 0, 0, size, size);
                int newHeight = source.Height * size / source.Width;
                int y = (size - newHeight) / 2;
                graphic.DrawImage(source, 0, y, size, newHeight);
            }
            else if (isThumbnail)
            {
                int newHeight = source.Height * size / source.Width;
                int y = (newHeight - size) / 2;
                graphic.DrawImage(source, 0, -y, size, newHeight);
            }
            else
            {
                graphic.FillRectangle(Brushes.White, 0, 0, size, size);
                int newWidth = source.Width * size / source.Height;
                int x = (size - newWidth) / 2;
                graphic.DrawImage(source, x, 0, newWidth, size);
            }

            // Compress image
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compressionLevel == 0 ? DefaultCompressionLevel : compressionLevel);

            ImageCodecInfo codec = GetImageCodecInfoFromMimeType("image/jpeg");

            MemoryStream stream = new MemoryStream();

            imageToWork.Save(stream, codec, parameters);

            result = stream.ToArray();

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the first ImageCodecInfo instance with the specified mime type
        /// </summary>
        /// <param name="mimeType">Mime type</param>
        /// <returns>ImageCodecInfo instance</returns>
        private static ImageCodecInfo GetImageCodecInfoFromMimeType(string mimeType)
        {
            var info = ImageCodecInfo.GetImageEncoders();

            foreach (var ici in info)
            {
                if (ici.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase))
                {
                    return ici;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first ImageCodecInfo instance with the specified extension
        /// </summary>
        /// <param name="fileExtension">File extension</param>
        /// <returns>ImageCodecInfo instance</returns>
        private static ImageCodecInfo GetImageCodecInfoFromExtension(string fileExtension)
        {
            fileExtension = fileExtension.TrimStart('.').ToLower().Trim();

            switch (fileExtension)
            {
                case "jpg":
                case "jpeg":
                    return GetImageCodecInfoFromMimeType("image/jpeg");
                case "png":
                    return GetImageCodecInfoFromMimeType("image/png");
                case "gif":
                    return GetImageCodecInfoFromMimeType("image/png");
                default:
                    return GetImageCodecInfoFromMimeType("image/jpeg");
            }
        }

        #endregion
    }
}