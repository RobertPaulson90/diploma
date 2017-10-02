namespace Diploma.BLL.Interfaces.Services
{
    public interface IMessageService
    {
        void Enqueue(string message);
    }
}
