using System;
using JetBrains.Annotations;

namespace Diploma.Framework.Messages
{
    internal sealed class ShowErrorMessage
    {
        public ShowErrorMessage([NotNull] string message)
        {
            Message = message;
        }

        public ShowErrorMessage([NotNull] Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }

        public string Message { get; }
    }
}
