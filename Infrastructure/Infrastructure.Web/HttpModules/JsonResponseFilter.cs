// -----------------------------------------------------------------------
// <copyright file="JsonResponseFilter.cs" company="Nagnoi">
// Copyright (c) Nagnoi. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Nagnoi.SiC.Infrastructure.Web.HttpModules
{
    #region Imports

    using System;
    using System.IO;
    using System.Web;

    #endregion

    /// <summary>
    /// Represents the applied filter to current response for JSONP format
    /// </summary>
    public class JsonResponseFilter : Stream
    {
        #region Constants

        private const string GzipContentEncoding = "gzip";

        private const string DeflateContentEncoding = "deflate";

        #endregion

        #region Private Members

        private readonly Stream responseStream;

        private readonly Stream finalResponseStream;

        private readonly HttpContext context;

        private StreamReader readerStream;

        #endregion

        #region Constructor

        public JsonResponseFilter(Stream responseStream, HttpContext context)
        {
            this.finalResponseStream = responseStream;

            this.responseStream = new System.IO.MemoryStream();

            //responseStream.CopyTo(this.responseStream);

            this.context = context;
        }

        #endregion

        #region Properties

        public override bool CanRead { get { return true; } }

        public override bool CanSeek { get { return true; } }

        public override bool CanWrite { get { return true; } }

        public override long Length { get { return 0; } }

        public override long Position { get; set; }

        public string JsonCallback { get; set; }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.JsonCallback = context.Request["jsoncallback"];

            if (string.IsNullOrEmpty(this.JsonCallback))
            {
                this.JsonCallback = context.Request["callback"];
            }

            if (string.IsNullOrEmpty(this.JsonCallback))
            {
                throw new ArgumentNullException("JsonCallback required for JSONP response.");
            }

            string currentBuffer = string.Empty;

            currentBuffer = context.Response.ContentEncoding.GetString(buffer);

            byte[] data = context.Response.ContentEncoding.GetBytes(currentBuffer);

            responseStream.Write(data, 0, data.Length);
        }

        public override void Close()
        {
            responseStream.Close();
            finalResponseStream.Close();
        }

        public override void Flush()
        {
            readerStream = new StreamReader(responseStream);

            responseStream.Seek(0, SeekOrigin.Begin);

            string response = readerStream.ReadToEnd();

            response = String.Format("{0}({1})", this.JsonCallback, response);

            byte[] data = context.Response.ContentEncoding.GetBytes(response);

            finalResponseStream.Write(data, 0, data.Length);

            finalResponseStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return responseStream.Seek(offset, origin);
        }

        public override void SetLength(long length)
        {
            finalResponseStream.SetLength(length);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return finalResponseStream.Read(buffer, offset, count);
        }

        #endregion
    }
}