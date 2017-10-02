using Caliburn.Micro;

namespace Diploma.ViewModels
{
    public class AuthenticationManagerViewModel : Conductor<Screen>
    {
        public AuthenticationManagerViewModel(LoginViewModel loginViewModel)
        {
            ActiveItem = loginViewModel;
        }
    }
}
