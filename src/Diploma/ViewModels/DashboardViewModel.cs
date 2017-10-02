using System.Threading;
using Caliburn.Micro;
using Diploma.BLL.DTO;
using Diploma.BLL.Interfaces.Services;

namespace Diploma.ViewModels
{
    public sealed class DashboardViewModel : Screen
    {
        private readonly IMessageService _messageService;

        public DashboardViewModel(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public UserDto CurrentUser { get; private set; }

        public void Init(UserDto currentUser)
        {
            CurrentUser = currentUser;
            _messageService.ShowMessage($"Hello, {CurrentUser.Username}");
            DisplayName = "Dashboard";
        }

        public void Logout()
        {
            _messageService.ShowMessage($"Goodbye, {CurrentUser.Username}");
            Thread.CurrentPrincipal = null;
            CurrentUser = null;
            ((ShellViewModel)Parent).ActiveItem = IoC.Get<LoginViewModel>();
        }
    }
}
