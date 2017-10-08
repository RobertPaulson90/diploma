namespace Diploma.Infrastructure.Data
{
    public static class OperationResultBuilder
    {
        public static OperationResult<T> CreateFailure<T>(string errorMessage)
        {
            return new OperationResult<T>(errorMessage);
        }

        public static OperationResult<T> CreateSuccess<T>(T result)
        {
            return new OperationResult<T>(result);
        }
    }
}
