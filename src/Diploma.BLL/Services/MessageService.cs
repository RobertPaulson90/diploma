using Caliburn.Micro;
using Diploma.BLL.Interfaces.Services;

namespace Diploma.BLL.Services
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
