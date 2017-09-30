using System.Security.Principal;
using System.Threading;
using Caliburn.Micro;
using Diploma.Framework;
using Diploma.Infrastructure;
using Diploma.Models;
using FluentValidation;

namespace Diploma.ViewModels
{
    public sealed class LoginViewModel : Screen
    {
        private readonly IMessageService _messageService;

        private readonly IUserService _userService;

        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private bool _isLoging;

        public LoginModel Model { get; }

        public LoginViewModel(IMessageService messageService, IUserService userService, IValidator<LoginModel> validator)
        {
            _messageService = messageService;
            _userService = userService;
            Model = new LoginModel(validator);
            DisplayName = "Authorization";
        }

        public bool IsLoging
        {
            get
            {
                return _isLoging;
            }

            set
            {
                Set(ref _isLoging, value);
            }
        }

        public void CreateNewAccount()
        {
            CancelAsync();
            ((ShellViewModel)Parent).ActiveItem = IoC.Get<RegisterViewModel>();
        }

        public async void SignIn()
        {
            if (IsLoging)
            {
                return;
            }

            if (Model.HasErrors)
            {
                _messageService.Enqueue("Incorrect username or password.");
                return;
            }

            IsLoging = true;
            try
            {
                var result = await _userService.GetUserByCredentialsAsync(Model.Username, Model.Password, _cancellationToken.Token);

                if (!result.Success)
                {
                    _messageService.Enqueue(result.NonSuccessMessage);
                    return;
                }

                var user = result.Result;

                var identity = new GenericIdentity(user.Username);
                var principal = new GenericPrincipal(identity, new[] { user.GetUserRole() });
                Thread.CurrentPrincipal = principal;

                var dashboard = IoC.Get<DashboardViewModel>();
                dashboard.Init(user);

                ((ShellViewModel)Parent).ActiveItem = dashboard;
            }
            finally
            {
                IsLoging = false;
            }
        }

        private void CancelAsync()
        {
            _cancellationToken?.Cancel();

            _cancellationToken = new CancellationTokenSource();
        }
    }
}
