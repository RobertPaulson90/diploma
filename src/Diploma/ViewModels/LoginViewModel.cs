using System.Security.Principal;
using System.Threading;
using Caliburn.Micro;
using Diploma.BLL.DTO;
using Diploma.BLL.Interfaces.Services;
using Diploma.Common;
using Diploma.Framework.Validations;
using FluentValidation;

namespace Diploma.ViewModels
{
    public sealed class LoginViewModel : ValidatableScreen<LoginViewModel, IValidator<LoginViewModel>>
    {
        private readonly IMessageService _messageService;

        private readonly IUserService _userService;

        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private string _password;

        private string _username;

        public LoginViewModel(IMessageService messageService, IUserService userService, IValidator<LoginViewModel> validator)
            : base(validator)
        {
            _messageService = messageService;
            _userService = userService;
            BusyScope = new BusyScope();
            DisplayName = "Authorization";
        }

        public BusyScope BusyScope { get; }

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
            if (BusyScope.IsBusy)
            {
                return;
            }

            if (HasErrors)
            {
                _messageService.ShowMessage("Incorrect username or password.");
                return;
            }

            using (BusyScope.StartWork())
            {
                var userAuthorizationDataDto = new UserAuthorizationDataDto { Password = Password, Username = Username };
                var result = await _userService.GetUserByCredentialsAsync(userAuthorizationDataDto, _cancellationToken.Token);

                if (!result.Success)
                {
                    _messageService.ShowMessage(result.NonSuccessMessage);
                    return;
                }

                var user = result.Result;

                var identity = new GenericIdentity(user.Username);
                var principal = new GenericPrincipal(identity, new[] { user.Role.ToString() });
                Thread.CurrentPrincipal = principal;

                var dashboard = IoC.Get<DashboardViewModel>();
                dashboard.Init(user);

                ((ShellViewModel)Parent).ActiveItem = dashboard;
            }
        }

        private void CancelAsync()
        {
            _cancellationToken?.Cancel();

            _cancellationToken = new CancellationTokenSource();
        }
    }
}
