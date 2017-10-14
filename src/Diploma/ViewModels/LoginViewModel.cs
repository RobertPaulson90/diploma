using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Caliburn.Micro;
using Diploma.BLL.Properties;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Services.Interfaces;
using Diploma.Core.Framework;
using Diploma.Core.Framework.Validations;
using Diploma.Framework.Interfaces;
using Diploma.Framework.Messages;
using JetBrains.Annotations;

namespace Diploma.ViewModels
{
    internal sealed class LoginViewModel : ValidatableScreen
    {
        [NotNull]
        private readonly IEventAggregator _eventAggregator;

        [NotNull]
        private readonly IMessageService _messageService;

        [NotNull]
        private readonly IUserService _userService;

        private CancellationTokenSource _cancellationToken;

        private string _password;

        private string _username;

        public LoginViewModel(
            [NotNull] IMessageService messageService,
            [NotNull] IEventAggregator eventAggregator,
            [NotNull] IUserService userService,
            [NotNull] IValidationAdapter validationAdapter)
            : base(validationAdapter)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
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

        [UsedImplicitly]
        [SuppressMessage("ReSharper", "AsyncConverter.AsyncAwaitMayBeElidedHighlighting", Justification = "This method is called on button click")]
        public async void CreateNewAccount()
        {
            SignalCancellationToken();

            var message = new ChangeAuthenticationManagerStateMessage(AuthenticationManagerState.Register);

            await _eventAggregator.PublishOnUIThreadAsync(message)
                .ConfigureAwait(false);
        }

        [UsedImplicitly]
        public async void SignIn()
        {
            if (BusyWatcher.IsBusy)
            {
                return;
            }

            if (HasErrors)
            {
                _messageService.ShowMessage(Resources.Authorization_Message_Validation_Errors);
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
                    _messageService.ShowMessage(operation.ErrorMessage);
                    return;
                }

                var user = operation.Result;

                var message = new LoggedInMessage(user);

                await _eventAggregator.PublishOnUIThreadAsync(message)
                    .ConfigureAwait(false);
            }
        }

        private void SignalCancellationToken()
        {
            _cancellationToken?.Cancel();

            _cancellationToken = new CancellationTokenSource();
        }
    }
}
