namespace Diploma.Infrastructure.Data
{
    public class OperationResult<T>
    {
        internal OperationResult(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }

        internal OperationResult(T result)
        {
            Success = true;
            Result = result;
        }

        public T Result { get; }

        public string ErrorMessage { get; }

        public bool Success { get; }
    }
}
