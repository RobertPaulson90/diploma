using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Caliburn.Micro;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Queries.Responses;
using Diploma.BLL.Services.Interfaces;
using Diploma.Core.Framework;
using Diploma.Core.Framework.Validations;
using Diploma.Framework.Interfaces;
using Diploma.Framework.Messages;
using Diploma.Properties;
using JetBrains.Annotations;

namespace Diploma.ViewModels
{
    internal sealed class RegisterViewModel : ValidatableScreen
    {
        [NotNull]
        private readonly IEventAggregator _eventAggregator;

        [NotNull]
        private readonly IMessageService _messageService;

        [NotNull]
        private readonly IUserService _userService;

        private DateTime? _birthDate;

        private CancellationTokenSource _cancellationToken;

        private string _confirmPassword;

        private string _firstName;

        private GenderType _gender;

        private string _lastName;

        private string _middleName;

        private string _password;

        private string _username;

        public RegisterViewModel(
            [NotNull] IUserService userService,
            [NotNull] IEventAggregator eventAggregator,
            [NotNull] IMessageService messageService,
            [NotNull] IValidationAdapter validationAdapter)
            : base(validationAdapter)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _cancellationToken = new CancellationTokenSource();
            BusyWatcher = new BusyWatcher();
            Validate();
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set => Set(ref _birthDate, value);
        }

        public BusyWatcher BusyWatcher { get; }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => Set(ref _confirmPassword, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => Set(ref _firstName, value);
        }

        public GenderType Gender
        {
            get => _gender;
            set => Set(ref _gender, value);
        }

        public string LastName
        {
            get => _lastName;
            set => Set(ref _lastName, value);
        }

        public string MiddleName
        {
            get => _middleName;
            set => Set(ref _middleName, value);
        }

        public string Password
        {
            get => _password;
            set
            {
                if (Set(ref _password, value))
                {
                    NotifyOfPropertyChange(nameof(ConfirmPassword));
                }
            }
        }

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        [UsedImplicitly]
        [SuppressMessage("ReSharper", "AsyncConverter.AsyncAwaitMayBeElidedHighlighting", Justification = "This method is called on button click")]
        public async void Cancel()
        {
            SignalCancellationToken();

            var message = new ChangeAuthenticationManagerStateMessage(AuthenticationManagerState.Login);

            await _eventAggregator.PublishOnUIThreadAsync(message)
                .ConfigureAwait(false);
        }

        [UsedImplicitly]
        public async void Register()
        {
            if (BusyWatcher.IsBusy)
            {
                return;
            }

            if (HasErrors)
            {
                _messageService.ShowMessage(Resources.Registration_Message_Validation_Errors);
                return;
            }

            using (BusyWatcher.GetTicket())
            {
                var request = new RegisterCustomerRequest
                {
                    BirthDate = BirthDate,
                    FirstName = FirstName,
                    Gender = Gender,
                    LastName = LastName,
                    MiddleName = MiddleName,
                    Password = Password,
                    Username = Username
                };

                var operation = await _userService.CreateCustomerAsync(request, _cancellationToken.Token)
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
