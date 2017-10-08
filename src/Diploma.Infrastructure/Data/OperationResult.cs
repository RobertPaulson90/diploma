namespace Diploma.Infrastructure.Data
{
    public sealed class OperationResult<T>
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

        public string ErrorMessage { get; }

        public T Result { get; }

        public bool Success { get; }
    }
}
