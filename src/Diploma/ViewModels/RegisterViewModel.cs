using System;
using System.Threading;
using Caliburn.Micro;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Queries.Responses;
using Diploma.BLL.Services.Interfaces;
using Diploma.Core.Framework;
using Diploma.Core.Framework.Validations;
using Diploma.Framework.Interfaces;
using JetBrains.Annotations;

namespace Diploma.ViewModels
{
    public sealed class RegisterViewModel : ValidatableScreen
    {
        private readonly IMessageService _messageService;

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
            [NotNull] IMessageService messageService,
            [NotNull] IValidationAdapter<RegisterViewModel> validationAdapter)
            : base(validationAdapter)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
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

        public void Cancel()
        {
            SignalCancellationToken();
            ((IConductActiveItem)Parent).ActiveItem = IoC.Get<LoginViewModel>();
        }

        public async void Register()
        {
            if (BusyWatcher.IsBusy)
            {
                return;
            }
            
            if (HasErrors)
            {
                _messageService.ShowErrorMessage("There were problems creating your account. Check input and try again.");
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
