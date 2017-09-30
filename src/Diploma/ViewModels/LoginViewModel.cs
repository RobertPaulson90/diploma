using System.Security.Principal;
using System.Threading;
using Caliburn.Micro;
using Diploma.Framework;
using Diploma.Framework.Validations;
using Diploma.Infrastructure;
using FluentValidation;

namespace Diploma.ViewModels
{
    public sealed class LoginViewModel : ValidatableScreen<LoginViewModel, IValidator<LoginViewModel>>
    {
        private readonly IMessageService _messageService;

        private readonly IUserService _userService;

        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private bool _isLoging;

        private string _password;

        private string _username;

        public LoginViewModel(IMessageService messageService, IUserService userService, IValidator<LoginViewModel> validator)
            : base(validator)
        {
            _messageService = messageService;
            _userService = userService;
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

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                if (Set(ref _password, value))
                {
                    Validate();
                }
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                if (Set(ref _username, value))
                {
                    Validate();
                }
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

            if (HasErrors)
            {
                _messageService.Enqueue("Incorrect username or password.");
                return;
            }

            IsLoging = true;
            try
            {
                var result = await _userService.GetUserByCredentialsAsync(Username, Password, _cancellationToken.Token);

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
