using System;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Diploma.ViewModels
{
    public sealed class AuthenticationManagerViewModel : Conductor<Screen>
    {
        [NotNull]
        private readonly LoginViewModel _loginViewModel;

        public AuthenticationManagerViewModel([NotNull] LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel ?? throw new ArgumentNullException(nameof(loginViewModel));
            OpenLogin();
        }

        private void OpenLogin()
        {
            ActiveItem = _loginViewModel;
        }
    }
}
