using System;
using System.Threading;
using Caliburn.Micro;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Queries.Responses;
using Diploma.BLL.Services.Interfaces;
using Diploma.Framework.Interfaces;
using Diploma.Views;
using JetBrains.Annotations;
using MaterialDesignThemes.Wpf;

namespace Diploma.ViewModels
{
    public sealed class DashboardViewModel : Conductor<Screen>
    {
        private readonly IMessageService _messageService;

        private readonly IUserService _userService;

        public DashboardViewModel([NotNull] IMessageService messageService, [NotNull] IUserService userService)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public UserDataResponse CurrentUser { get; private set; }

        public async void Edit()
        {
            var viewModel = IoC.Get<EditUserDataViewModel>();
            viewModel.LastName = CurrentUser.LastName;
            viewModel.FirstName = CurrentUser.FirstName;
            viewModel.MiddleName = CurrentUser.MiddleName;
            viewModel.Gender = CurrentUser.Gender;
            viewModel.BirthDate = CurrentUser.BirthDate;

            var view = IoC.Get<EditUserDataView>();
            view.DataContext = viewModel;

            var result = await DialogHost.Show(view).ConfigureAwait(false);
            if (!(result is bool) || !(bool)result)
            {
                return;
            }

            var isValid = viewModel.Validate();
            if (!isValid)
            {
                _messageService.ShowErrorMessage("There were problems saving your personal info. Check input and try again.");
                return;
            }

            var userUpdateRequestDataDto = new UpdateUserDataRequest
            {
                Id = CurrentUser.Id,
                LastName = viewModel.LastName,
                FirstName = viewModel.FirstName,
                MiddleName = viewModel.MiddleName,
                Gender = viewModel.Gender,
                BirthDate = viewModel.BirthDate
            };

            var operation = await _userService.UpdateUserAsync(userUpdateRequestDataDto).ConfigureAwait(false);

            if (!operation.Success)
            {
                _messageService.ShowErrorMessage(operation.ErrorMessage);
                return;
            }

            CurrentUser = operation.Result;
        }

        public void Init(UserDataResponse currentUser)
        {
            CurrentUser = currentUser;
            _messageService.ShowErrorMessage($"Hello, {CurrentUser.Username}");
        }

        public void Logout()
        {
            _messageService.ShowErrorMessage($"Goodbye, {CurrentUser.Username}");
            Thread.CurrentPrincipal = null;
            CurrentUser = null;
            ((IConductActiveItem)Parent).ActiveItem = IoC.Get<LoginViewModel>();
        }
    }
}
