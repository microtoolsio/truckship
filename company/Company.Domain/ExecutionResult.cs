using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Domain
{
    public class ExecutionResult
    {
        public ExecutionResult()
            : this((ExecutionResult)null)
        {
        }

        public ExecutionResult(ErrorInfo error)
            : this((ExecutionResult)null)
        {
            this.Errors.Add(error);
        }

        public ExecutionResult(IEnumerable<ErrorInfo> errors)
            : this((ExecutionResult)null)
        {
            foreach (ErrorInfo errorInfo in errors)
            {
                this.Errors.Add(errorInfo);
            }
        }

        public ExecutionResult(ExecutionResult result)
        {
            this.Errors = new List<ErrorInfo>();
            if (result == null)
            {
                return;
            }
            foreach (ErrorInfo errorInfo in result.Errors)
            {
                this.Errors.Add(errorInfo);
            }
            this.Success = result.Success;
        }

        private bool? success;

        /// <summary>
        ///     Indicates if result is successful.
        /// </summary>
        public bool Success
        {
            get { return this.success == true || this.Errors.Count == 0; }
            set { this.success = value; }
        }

        /// <summary>
        /// 	Gets list of errors.
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; }
    }

    /// <summary>
    /// Represents result of an action that returns any value
    /// </summary>
    /// <typeparam name="T">Type of value to be returned with action</typeparam>
    public class ExecutionResult<T> : ExecutionResult
    {
        public ExecutionResult()
            : this((ExecutionResult)null)
        {
        }

        public ExecutionResult(T result)
            : this((ExecutionResult)null)
        {
            this.Value = result;
        }

        public ExecutionResult(ErrorInfo error)
            : this((ExecutionResult)null)
        {
            this.Errors.Add(error);
        }

        public ExecutionResult(IEnumerable<ErrorInfo> errors)
            : this((ExecutionResult)null)
        {
            foreach (ErrorInfo errorInfo in errors)
            {
                this.Errors.Add(errorInfo);
            }
        }

        public ExecutionResult(ExecutionResult result)
            : base(result)
        {
            if (result is ExecutionResult<T> r)
            {
                this.Value = r.Value;
            }
        }

        public T Value { get; set; }
    }
}
