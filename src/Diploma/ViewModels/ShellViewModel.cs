using Caliburn.Micro;

namespace Diploma.ViewModels
{
    public sealed class ShellViewModel : Conductor<Screen>
    {
        public ShellViewModel()
        {
            ActiveItem = new LoginViewModel();
        }
    }
}
