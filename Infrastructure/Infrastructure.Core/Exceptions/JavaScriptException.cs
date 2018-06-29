// -----------------------------------------------------------------------
// <copyright file="JavaScriptException.cs" company="Nagnoi">
// Copyright (c) Nagnoi. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Nagnoi.SiC.Infrastructure.Core.Exceptions
{
    #region Imports

    using System;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    /// Represents errors that occur during the execution of JavaScript code from client-side
    /// </summary>
    [Serializable]
    public class JavaScriptException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptException"/> class
        /// </summary>
        public JavaScriptException() 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptException"/> class
        /// </summary>
        /// <param name="message">Exception Message</param>
        public JavaScriptException(string message) : base(message) 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptException"/> class
        /// </summary>
        /// <param name="message">Exception Message</param>
        /// <param name="inner">Inner Exception</param>
        public JavaScriptException(string message, Exception inner) : base(message, inner) 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptException"/> class
        /// </summary>
        /// <param name="info">Serialization Info</param>
        /// <param name="context">Streaming Context</param>
        protected JavaScriptException(SerializationInfo info, StreamingContext context) : base(info, context) 
        { 
        }

        #endregion
    }
}