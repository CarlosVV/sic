namespace Nagnoi.SiC.Infrastructure.Core.Exceptions {
    #region Referencias

    using System;
    using System.Runtime.Serialization;

    #endregion

    [Serializable]
    public class BusinessException : Exception {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NegocioExcepcion"/> class.
        /// </summary>
        public BusinessException() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NegocioExcepcion"/> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public BusinessException(string message)
            : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NegocioExcepcion"/> class with a specified error message.
        /// </summary>
        /// <param name="messageFormat">The exception message format</param>
        /// <param name="args">The exception message arguments</param>
        public BusinessException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args)) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NegocioExcepcion"/> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="inner">Inner exception</param>
        public BusinessException(string message, Exception inner)
            : base(message, inner) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NegocioExcepcion"/> class.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected BusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }

        #endregion
    }
}
