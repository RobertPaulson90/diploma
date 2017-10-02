using System;

namespace Diploma.Framework.Messages
{
    public class ShowErrorMessage
    {
        public ShowErrorMessage(string message)
        {
            Message = message;
        }

        public ShowErrorMessage(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }

        public string Message { get; }
    }
}
