using Caliburn.Micro;
using Diploma.Infrastructure;

namespace Diploma.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IEventAggregator _eventAggregator;

        public MessageService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Enqueue(string message)
        {
            _eventAggregator.PublishOnUIThread(message);
        }
    }
}
