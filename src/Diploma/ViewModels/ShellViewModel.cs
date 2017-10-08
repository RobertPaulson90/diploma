using System;
using Caliburn.Micro;
using Diploma.Framework.Messages;
using JetBrains.Annotations;
using MaterialDesignThemes.Wpf;

namespace Diploma.ViewModels
{
    internal sealed class ShellViewModel : Conductor<Screen>, IHandle<ShowErrorMessage>
    {
        private readonly AuthenticationManagerViewModel _authenticationManagerViewModel;

        private readonly IEventAggregator _eventAggregator;

        public ShellViewModel([NotNull] IEventAggregator eventAggregator, [NotNull] AuthenticationManagerViewModel authenticationManagerViewModel)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _authenticationManagerViewModel =
                authenticationManagerViewModel ?? throw new ArgumentNullException(nameof(authenticationManagerViewModel));
            _eventAggregator.Subscribe(this);
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
            OpenAuthenticationManager();
        }

        public SnackbarMessageQueue MessageQueue { get; }

        public void Handle(ShowErrorMessage message)
        {
            MessageQueue.Enqueue(message.Message, true);
        }

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
        }

        private void OpenAuthenticationManager()
        {
            ActiveItem = _authenticationManagerViewModel;
        }
    }
}
