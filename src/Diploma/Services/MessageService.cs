using Caliburn.Micro;
using Diploma.Services.Interfaces;

namespace Diploma.Services
{
    public class MessageService : IMessageService
    {
        private readonly IEventAggregator _eventAggregator;

        public MessageService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void ShowMessage(string message)
        {
            _eventAggregator.PublishOnUIThread(message);
        }
    }
}
