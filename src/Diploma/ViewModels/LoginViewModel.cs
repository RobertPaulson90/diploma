using System;
using System.Threading;
using Caliburn.Micro;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Services.Interfaces;
using Diploma.Core.Framework;
using Diploma.Core.Framework.Validations;
using Diploma.Framework.Interfaces;
using JetBrains.Annotations;

namespace Diploma.ViewModels
{
    public sealed class LoginViewModel : ValidatableScreen
    {
        [NotNull]
        private readonly IMessageService _messageService;

        [NotNull]
        private readonly IUserService _userService;

        private CancellationTokenSource _cancellationToken;

        private string _password;

        private string _username;
        
        public LoginViewModel(
            [NotNull] IMessageService messageService,
            [NotNull] IUserService userService,
            [NotNull] IValidationAdapter<LoginViewModel> validationAdapter)
            : base(validationAdapter)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _cancellationToken = new CancellationTokenSource();
            BusyWatcher = new BusyWatcher();
            Validate();
        }

        public BusyWatcher BusyWatcher { get; }

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        public void CreateNewAccount()
        {
            SignalCancellationToken();
            ((IConductActiveItem)Parent).ActiveItem = IoC.Get<RegisterViewModel>();
        }

        public async void SignIn()
        {
            if (BusyWatcher.IsBusy)
            {
                return;
            }
            
            if (HasErrors)
            {
                _messageService.ShowErrorMessage("Incorrect username or password.");
                return;
            }

            using (BusyWatcher.GetTicket())
            {
                var request = new GetUserByCredentialsRequest
                {
                    Password = Password,
                    Username = Username
                };

                var operation = await _userService.GetUserByCredentialsAsync(request, _cancellationToken.Token)
                    .ConfigureAwait(false);

                if (!operation.Success)
                {
                    _messageService.ShowErrorMessage(operation.ErrorMessage);
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
