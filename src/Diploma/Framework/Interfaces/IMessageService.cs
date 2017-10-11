using System;
using JetBrains.Annotations;

namespace Diploma.Framework.Interfaces
{
    public interface IMessageService
    {
        void ShowMessage([NotNull] [LocalizationRequired] string message);
        
        void ShowErrorMessage([NotNull] Exception exception);
    }
}
