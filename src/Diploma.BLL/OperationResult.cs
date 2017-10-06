namespace Diploma.BLL
{
    public class OperationResult<T>
    {
        private OperationResult(string errorMessage)
        {
            Succeeded = false;
            ErrorMessage = errorMessage;
        }

        private OperationResult(T data)
        {
            Succeeded = true;
            Data = data;
        }

        public T Data { get; }

        public string ErrorMessage { get; }

        public bool Succeeded { get; }

        public static OperationResult<T> CreateFailure(string errorMessage)
        {
            return new OperationResult<T>(errorMessage);
        }

        public static OperationResult<T> CreateSuccess(T result)
        {
            return new OperationResult<T>(result);
        }
    }
}
