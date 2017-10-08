using System;
using JetBrains.Annotations;

namespace Diploma.Framework.Interfaces
{
    public interface IMessageService
    {
        void ShowErrorMessage([NotNull] string message);

        void ShowErrorMessage([NotNull] Exception exception);
    }
}
