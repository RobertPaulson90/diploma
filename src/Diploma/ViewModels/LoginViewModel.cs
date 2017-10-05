using System;
using System.Threading;
using Caliburn.Micro;
using Diploma.BLL.Contracts.DTO;
using Diploma.BLL.Contracts.Services;
using Diploma.Core.Framework;
using Diploma.Framework.Interfaces;
using Diploma.Framework.Validations;
using FluentValidation;

namespace Diploma.ViewModels
{
    public sealed class LoginViewModel : ValidatableScreen<LoginViewModel, IValidator<LoginViewModel>>
    {
        private readonly IMessageService _messageService;

        private readonly IUserService _userService;

        private CancellationTokenSource _cancellationToken;

        private string _password;

        private string _username;

        public LoginViewModel(IMessageService messageService, IUserService userService, IValidator<LoginViewModel> validator)
            : base(validator)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _cancellationToken = new CancellationTokenSource();
            BusyScope = new BusyScope();
        }

        public BusyScope BusyScope { get; }

        public string Password
        {
            get => _password;

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
            get => _username;

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
            SignalCancellationToken();
            ((IConductActiveItem)Parent).ActiveItem = IoC.Get<RegisterViewModel>();
        }

        public async void SignIn()
        {
            if (BusyScope.IsBusy)
            {
                return;
            }

            var isValid = Validate();
            if (!isValid)
            {
                _messageService.ShowErrorMessage("Incorrect username or password.");
                return;
            }

            using (BusyScope.StartWork())
            {
                var userAuthorizationDataDto = new UserCredentialsDto
                {
                    Password = Password,
                    Username = Username
                };

                var operation = await _userService.GetUserByCredentialsAsync(userAuthorizationDataDto, _cancellationToken.Token)
                    .ConfigureAwait(false);

                if (!operation.Success)
                {
                    _messageService.ShowErrorMessage(operation.NonSuccessMessage);
                    return;
                }

                var user = operation.Result;
                var dashboard = IoC.Get<DashboardViewModel>();
                dashboard.Init(user);
                ((IConductActiveItem)Parent).ActiveItem = dashboard;
            }
        }

        private void SignalCancellationToken()
        {
            _cancellationToken?.Cancel();

            _cancellationToken = new CancellationTokenSource();
        }
    }
}
