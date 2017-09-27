using System;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;

namespace Diploma.ViewModels
{
    public sealed class ShellViewModel : Conductor<Screen>, IHandle<string>
    {
        private readonly IEventAggregator _eventAggregator;

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public SnackbarMessageQueue MessageQueue { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        public void Handle(string message)
        {
            MessageQueue.Enqueue(message, true);
        }

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
        }

        protected override void OnInitialize()
        {
            ActiveItem = IoC.Get<LoginViewModel>();
        }
    }
}
