using System;
using System.Security.Principal;
using System.Threading;
using Caliburn.Micro;
using Diploma.BLL.DTO.Enums;
using Diploma.BLL.Interfaces.Services;
using Diploma.Common;
using Diploma.DAL.Entities.Enums;
using Diploma.Framework;
using Diploma.Framework.Validations;
using FluentValidation;

namespace Diploma.ViewModels
{
    public sealed class RegisterViewModel : ValidatableScreen<RegisterViewModel, IValidator<RegisterViewModel>>
    {
        private readonly IMessageService _messageService;

        private readonly IUserService _userService;

        private DateTime? _birthDate;

        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private string _confirmPassword;

        private string _firstName;

        private GenderType _gender;

        private string _lastName;

        private string _middleName;

        private string _password;

        private string _username;

        public RegisterViewModel(IUserService userService, IMessageService messageService, IValidator<RegisterViewModel> validator)
            : base(validator)
        {
            _userService = userService;
            _messageService = messageService;
            BusyScope = new BusyScope();
            DisplayName = "Registration";
        }

        public DateTime? BirthDate
        {
            get
            {
                return _birthDate;
            }

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
            get
            {
                return _confirmPassword;
            }

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
            get
            {
                return _firstName;
            }

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
            get
            {
                return _gender;
            }

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
            get
            {
                return _lastName;
            }

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
            get
            {
                return _middleName;
            }

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

        public void Cancel()
        {
            CancelAsync();
            ((ShellViewModel)Parent).ActiveItem = IoC.Get<LoginViewModel>();
        }

        public async void Register()
        {
            if (BusyScope.IsBusy)
            {
                return;
            }

            if (HasErrors)
            {
                _messageService.Enqueue("There were problems creating your account.");
                return;
            }

            using (BusyScope.StartWork())
            {
                var result = await _userService.CreateUserAsync(
                    Username,
                    Password,
                    LastName,
                    FirstName,
                    MiddleName,
                    UserRoleType.Customer,
                    BirthDate,
                    Gender,
                    _cancellationToken.Token);

                if (!result.Success)
                {
                    _messageService.Enqueue(result.NonSuccessMessage);
                    return;
                }

                var user = result.Result;

                var identity = new GenericIdentity(user.Credentials.Username);
                var principal = new GenericPrincipal(identity, new[] { user.GetUserRole() });
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
