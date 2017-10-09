using System;
using System.Threading;
using Caliburn.Micro;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Queries.Responses;
using Diploma.BLL.Services.Interfaces;
using Diploma.Core.Framework;
using Diploma.Core.Framework.Validations;
using Diploma.Framework.Interfaces;
using FluentValidation;
using JetBrains.Annotations;

namespace Diploma.ViewModels
{
    public sealed class RegisterViewModel : ValidatableScreen<RegisterViewModel, IValidator<RegisterViewModel>>
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
            [NotNull] IValidator<RegisterViewModel> validator)
            : base(validator)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _cancellationToken = new CancellationTokenSource();
            BusyScope = new BusyScope();
        }

        public DateTime? BirthDate
        {
            get => _birthDate;

            set
            {
                if (Set(ref _birthDate, value))
                {
                    Validate();
                }
            }
        }

        public BusyScope BusyScope { get; }

        public string ConfirmPassword
        {
            get => _confirmPassword;

            set
            {
                if (Set(ref _confirmPassword, value))
                {
                    Validate();
                }
            }
        }

        public string FirstName
        {
            get => _firstName;

            set
            {
                if (Set(ref _firstName, value))
                {
                    Validate();
                }
            }
        }

        public GenderType Gender
        {
            get => _gender;

            set
            {
                if (Set(ref _gender, value))
                {
                    Validate();
                }
            }
        }

        public string LastName
        {
            get => _lastName;

            set
            {
                if (Set(ref _lastName, value))
                {
                    Validate();
                }
            }
        }

        public string MiddleName
        {
            get => _middleName;

            set
            {
                if (Set(ref _middleName, value))
                {
                    Validate();
                }
            }
        }

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

        public void Cancel()
        {
            SignalCancellationToken();
            ((IConductActiveItem)Parent).ActiveItem = IoC.Get<LoginViewModel>();
        }

        public async void Register()
        {
            if (BusyScope.IsBusy)
            {
                return;
            }

            var isValid = await ValidateAsync(_cancellationToken.Token)
                .ConfigureAwait(false);

            if (!isValid)
            {
                _messageService.ShowErrorMessage("There were problems creating your account. Check input and try again.");
                return;
            }

            using (BusyScope.StartWork())
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
