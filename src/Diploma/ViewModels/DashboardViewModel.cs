using System;
using System.Diagnostics.CodeAnalysis;
using Caliburn.Micro;
using Diploma.BLL.Queries.Requests;
using Diploma.BLL.Queries.Responses;
using Diploma.BLL.Services.Interfaces;
using Diploma.Framework.Interfaces;
using Diploma.Framework.Messages;
using Diploma.Properties;
using Diploma.Views;
using JetBrains.Annotations;
using MaterialDesignThemes.Wpf;

namespace Diploma.ViewModels
{
    internal sealed class DashboardViewModel : Conductor<Screen>
    {
        [NotNull]
        private readonly IMessageService _messageService;

        [NotNull]
        private readonly IUserService _userService;

        [NotNull]
        private readonly IEventAggregator _eventAggregator;

        public DashboardViewModel([NotNull] IMessageService messageService, [NotNull] IUserService userService, [NotNull] IEventAggregator eventAggregator)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        }

        public UserDataResponse CurrentUser { get; private set; }

        [UsedImplicitly]
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

            var result = await DialogHost.Show(view)
                .ConfigureAwait(false);

            if (!(result is bool) || !(bool)result)
            {
                return;
            }

            if (viewModel.HasErrors)
            {
                _messageService.ShowMessage(Resources.Editing_Message_Validation_Errors);
                return;
            }

            var updateUserDataRequest = new UpdateUserDataRequest
            {
                Id = CurrentUser.Id,
                LastName = viewModel.LastName,
                FirstName = viewModel.FirstName,
                MiddleName = viewModel.MiddleName,
                Gender = viewModel.Gender,
                BirthDate = viewModel.BirthDate
            };

            var operation = await _userService.UpdateUserAsync(updateUserDataRequest)
                .ConfigureAwait(false);

            if (!operation.Success)
            {
                _messageService.ShowMessage(operation.ErrorMessage);
                return;
            }

            CurrentUser = operation.Result;
        }

        public void Init(UserDataResponse currentUser)
        {
            CurrentUser = currentUser;
            _messageService.ShowMessage(string.Format(Resources.Dashboard_Welcome_Message_Text, CurrentUser.Username));
        }

        [UsedImplicitly]
        [SuppressMessage("ReSharper", "AsyncConverter.AsyncAwaitMayBeElidedHighlighting", Justification = "This method is called on button click")]
        public async void Logout()
        {
            _messageService.ShowMessage(string.Format(Resources.Dashboard_Farewell_Message_Text, CurrentUser.Username));
            CurrentUser = null;
            var message = new LoggedOutMessage();
            await _eventAggregator.PublishOnUIThreadAsync(message)
                .ConfigureAwait(false);
        }
    }
}
