using System;

namespace Diploma.Framework.Interfaces
{
    public interface IMessageService
    {
        void ShowErrorMessage(string message);

        void ShowErrorMessage(Exception exception);
    }
}
