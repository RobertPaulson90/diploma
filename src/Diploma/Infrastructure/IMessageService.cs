namespace Diploma.Infrastructure
{
    public interface IMessageService
    {
        void Enqueue(string message);
    }
}
