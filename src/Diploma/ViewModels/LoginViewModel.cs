using Caliburn.Micro;

namespace Diploma.ViewModels
{
    public sealed class LoginViewModel : Screen
    {
        public LoginViewModel()
        {
            DisplayName = "Authorization";
        }

        public void SignIn()
        {

        }

        public void CreateNewAccount()
        {
            ((ShellViewModel)Parent).ActiveItem = new RegisterViewModel();
        }
    }
}
