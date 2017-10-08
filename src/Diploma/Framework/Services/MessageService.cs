using System;
using Caliburn.Micro;
using Diploma.Framework.Interfaces;
using Diploma.Framework.Messages;

namespace Diploma.Framework.Services
{
    internal sealed class MessageService : IMessageService
    {
        private readonly IEventAggregator _eventAggregator;

        public MessageService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        }

        public void ShowErrorMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            _eventAggregator.PublishOnUIThread(new ShowErrorMessage(message));
        }

        public void ShowErrorMessage(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            _eventAggregator.PublishOnUIThread(new ShowErrorMessage(exception));
        }
    }
}
