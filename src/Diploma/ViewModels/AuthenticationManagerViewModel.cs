using System;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Diploma.ViewModels
{
    public sealed class AuthenticationManagerViewModel : Conductor<Screen>
    {
        public AuthenticationManagerViewModel([NotNull] LoginViewModel loginViewModel)
        {
            ActiveItem = loginViewModel ?? throw new ArgumentNullException(nameof(loginViewModel));
        }
    }
}
