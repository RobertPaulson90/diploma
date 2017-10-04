using Caliburn.Micro;

namespace Diploma.ViewModels
{
    public sealed class AuthenticationManagerViewModel : Conductor<Screen>
    {
        public AuthenticationManagerViewModel(LoginViewModel loginViewModel)
        {
            ActiveItem = loginViewModel;
        }
    }
}
