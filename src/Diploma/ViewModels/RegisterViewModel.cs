using System.Security.Principal;
using System.Threading;
using Caliburn.Micro;
using Diploma.Framework;
using Diploma.Infrastructure;
using Diploma.Models;
using FluentValidation;

namespace Diploma.ViewModels
{
    public sealed class RegisterViewModel : Screen
    {
        private readonly IMessageService _messageService;

        private readonly IUserService _userService;

        private bool _isRegistering;

        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public RegisterModel Model { get; }

        public RegisterViewModel(IUserService userService, IMessageService messageService, IValidator<RegisterModel> validator)
        {
            _userService = userService;
            _messageService = messageService;
            Model = new RegisterModel(validator);
            DisplayName = "Registration";
        }


        public bool IsRegistering
        {
            get
            {
                return _isRegistering;
            }

            set
            {
                Set(ref _isRegistering, value);
            }
        }

        public void Cancel()
        {
            CancelAsync();
            ((ShellViewModel)Parent).ActiveItem = IoC.Get<LoginViewModel>();
        }

        public async void Register()
        {
            if (IsRegistering)
            {
                return;
            }

            if (Model.HasErrors)
            {
                _messageService.Enqueue("There were problems creating your account.");
                return;
            }

            IsRegistering = true;
            try
            {
                var result = await _userService.CreateCustomerAsync(
                    Model.Username,
                    Model.Password,
                    Model.LastName,
                    Model.FirstName,
                    Model.MiddleName,
                    Model.BirthDate,
                    Model.Gender,
                    _cancellationToken.Token);

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
                IsRegistering = false;
            }
        }

        private void CancelAsync()
        {
            _cancellationToken?.Cancel();

            _cancellationToken = new CancellationTokenSource();
        }
    }
}
