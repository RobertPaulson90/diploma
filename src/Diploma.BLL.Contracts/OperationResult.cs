using System;

namespace Diploma.BLL.Contracts
{
    public class OperationResult<TResult>
    {
        private OperationResult(string nonSuccessMessage)
        {
            Success = false;
            NonSuccessMessage = nonSuccessMessage;
        }

        private OperationResult(Exception exception, string nonSuccessMessage)
            : this(nonSuccessMessage)
        {
            Exception = exception;
        }

        private OperationResult(TResult result)
        {
            Success = true;
            Result = result;
        }

        public Exception Exception { get; }

        public string NonSuccessMessage { get; }

        public TResult Result { get; }

        public bool Success { get; }

        public static OperationResult<TResult> CreateFailure(string nonSuccessMessage)
        {
            return new OperationResult<TResult>(nonSuccessMessage);
        }

        public static OperationResult<TResult> CreateFailure(Exception ex)
        {
            return new OperationResult<TResult>(ex, ex.Message);
        }

        public static OperationResult<TResult> CreateFailure(Exception ex, string nonSuccessMessage)
        {
            return new OperationResult<TResult>(ex, nonSuccessMessage);
        }

        public static OperationResult<TResult> CreateSuccess(TResult result)
        {
            return new OperationResult<TResult>(result);
        }
    }
}
