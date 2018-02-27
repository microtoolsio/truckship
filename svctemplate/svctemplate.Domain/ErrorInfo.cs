using System;
using System.Collections.Generic;
using System.Text;

namespace svctemplate.Domain
{
    public class ErrorInfo
    {
        /// <summary>
        /// Code of error
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Default constructor of class
        /// </summary>
        public ErrorInfo()
            : this(String.Empty, String.Empty)
        {
        }

        public ErrorInfo(string errorMessage)
            : this(Guid.NewGuid().ToString(), errorMessage)
        {
        }

        /// <summary>
        /// Constructor of class
        /// </summary>
        /// <param name="code">Key of error (for example, property name)</param>
        /// <param name="message">Error message</param>
        public ErrorInfo(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public override string ToString()
        {
            return String.Format("{0}. Code: '{1}', Message: '{2}'", base.ToString(), this.Code, this.Message);
        }
    }
}
