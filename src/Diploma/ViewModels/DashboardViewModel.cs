using System.Threading;
using Caliburn.Micro;
using Diploma.DAL.Entities;
using Diploma.Infrastructure;

namespace Diploma.ViewModels
{
    public sealed class DashboardViewModel : Screen
    {
        private readonly IMessageService _messageService;

        public DashboardViewModel(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public UserEntity CurrentUser { get; private set; }

        public void Init(UserEntity currentUser)
        {
            CurrentUser = currentUser;
            _messageService.Enqueue($"Hello, {CurrentUser.Username}");
            DisplayName = "Dashboard";
        }

        public void Logout()
        {
            _messageService.Enqueue($"Goodbye, {CurrentUser.Username}");
            Thread.CurrentPrincipal = null;
            CurrentUser = null;
            ((ShellViewModel)Parent).ActiveItem = IoC.Get<LoginViewModel>();
        }
    }
}
