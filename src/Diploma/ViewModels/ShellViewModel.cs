using System;
using Caliburn.Micro;
using Diploma.Core.Framework;
using Diploma.Framework.Messages;
using JetBrains.Annotations;
using MaterialDesignThemes.Wpf;

namespace Diploma.ViewModels
{
    internal sealed class ShellViewModel : Conductor<IScreen>, IHandle<ShowErrorMessage>, IHandle<LoggedInMessage>, IHandle<LoggedOutMessage>
    {
        [NotNull]
        private readonly IEventAggregator _eventAggregator;

        [NotNull]
        private readonly IScreenFactory _screenFactory;

        public ShellViewModel([NotNull] IEventAggregator eventAggregator, [NotNull] IScreenFactory screenFactory)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _screenFactory = screenFactory ?? throw new ArgumentNullException(nameof(screenFactory));
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        }

        public SnackbarMessageQueue MessageQueue { get; }

        void IHandle<ShowErrorMessage>.Handle(ShowErrorMessage message)
        {
            MessageQueue.Enqueue(message.Message, true);
        }

        void IHandle<LoggedInMessage>.Handle(LoggedInMessage message)
        {
            var dashboard = _screenFactory.CreateScreen<DashboardViewModel>();
            dashboard.Init(message.User);
            ActiveItem = dashboard;
        }

        void IHandle<LoggedOutMessage>.Handle(LoggedOutMessage message)
        {
            ActiveItem = _screenFactory.CreateScreen<AuthenticationManagerViewModel>();
        }

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);
            ActiveItem = _screenFactory.CreateScreen<AuthenticationManagerViewModel>();
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
        }
    }
}
