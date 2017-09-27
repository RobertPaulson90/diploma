using System;

namespace Diploma.Framework
{
    public class OperationResult<TResult>
    {
        private OperationResult()
        {
        }

        public Exception Exception { get; private set; }

        public string NonSuccessMessage { get; private set; }

        public TResult Result { get; private set; }

        public bool Success { get; private set; }

        public static OperationResult<TResult> CreateFailure(string nonSuccessMessage)
        {
            return new OperationResult<TResult> { Success = false, NonSuccessMessage = nonSuccessMessage };
        }

        public static OperationResult<TResult> CreateFailure(Exception ex)
        {
            return new OperationResult<TResult> { Success = false, NonSuccessMessage = ex.Message, Exception = ex };
        }

        public static OperationResult<TResult> CreateFailure(Exception ex, string nonSuccessMessage)
        {
            return new OperationResult<TResult> { Success = false, NonSuccessMessage = nonSuccessMessage, Exception = ex };
        }

        public static OperationResult<TResult> CreateSuccess(TResult result)
        {
            return new OperationResult<TResult> { Success = true, Result = result };
        }
    }
}
