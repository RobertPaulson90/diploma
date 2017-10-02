using System;
using Caliburn.Micro;
using Diploma.Framework.Messages;
using MaterialDesignThemes.Wpf;

namespace Diploma.ViewModels
{
    public sealed class ShellViewModel : Conductor<Screen>, IHandle<ShowErrorMessage>
    {
        private readonly IEventAggregator _eventAggregator;

        public ShellViewModel(IEventAggregator eventAggregator, AuthenticationManagerViewModel authenticationManagerViewModel)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            ActiveItem = authenticationManagerViewModel;
        }

        public SnackbarMessageQueue MessageQueue { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        
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
    }
}
